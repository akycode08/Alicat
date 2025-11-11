using System;                           // базовые типы
using System.Diagnostics;
using System.Globalization;             // парс чисел Invariant
using System.IO.Ports;                  // SerialPort
using System.Text;                      // Encoding.ASCII
using System.Threading.Tasks;           // Task.Delay (на будущее)
using System.Windows.Forms;             // WinForms
using Alicat.Services.Serial;           // ✅ внешний SerialClient
using Alicat.Domain;
using Alicat.Services.Controllers;
using Alicat.Services.Protocol;
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        private double _current = 0.0;
        private double _setPoint = 0.0;
        private string _unit = "PSIG";

        private bool _isExhaust = false;
        private double? _lastCurrent = null;

        private SerialClient? _serial;
        private readonly Timer _pollTimer = new() { Interval = 500 };

        private double _maxPressure = 200.0;
        private double _maxIncrementLimit = 20.0;

        private readonly DeviceState _state = new();
        private RampController? _ramp;


        public AlicatForm()
        {
            InitializeComponent();

            btnGoTarget.Click += btnGoTarget_Click;
            btnPurge.Click += btnPurge_Click;
            btnGoPlus.Click += btnGoPlus_Click;
            btnGoMinus.Click += btnGoMinus_Click;
            btnCommunication.Click += btnCommunication_Click;
            btnOptions.Click += btnOptions_Click;

            txtTarget.TextChanged += (_, __) => ValidateTargetAgainstMax();
            chkConfirmGo.CheckedChanged += (_, __) => ValidateTargetAgainstMax();
            nudIncrement.ValueChanged += (_, __) => ValidateIncrementAgainstMax();

            UI_SetPressureUnits(_unit);
            UI_SetRampSpeedUnits("PSIG/s");
            UI_SetSetPoint(_setPoint, _unit);
            UI_SetTimeToSetPoint(null);
            UI_Status_Up(false);
            UI_Status_Mid(false);
            UI_Status_Down(false);

            RefreshCurrent();

            _pollTimer.Tick += (_, __) => _serial?.RequestAls();
            ApplyOptionsToUi();
        }

        private void ApplyOptionsToUi()
        {
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _maxIncrementLimit = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;

            nudIncrement.DecimalPlaces = 1;
            nudIncrement.Increment = 0.1M;

            if (nudIncrement.Minimum <= 0) nudIncrement.Minimum = 0.1M;
            if (nudIncrement.Maximum < 100000M) nudIncrement.Maximum = 100000M;

            ValidateTargetAgainstMax();
            ValidateIncrementAgainstMax();
        }

        private void ValidateTargetAgainstMax()
        {
            var text = txtTarget.Text?.Trim();
            bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetVal);
            bool over = parsed && targetVal > _maxPressure;

            txtTarget.BackColor = over ? System.Drawing.Color.MistyRose : System.Drawing.SystemColors.Window;
            btnGoTarget.Enabled = chkConfirmGo.Checked && parsed;
        }

        private void ValidateIncrementAgainstMax()
        {
            double inc = (double)nudIncrement.Value;
            bool over = inc > _maxIncrementLimit;

            nudIncrement.BackColor = over ? System.Drawing.Color.MistyRose : System.Drawing.SystemColors.Window;
            btnGoPlus.Enabled = !over;
            btnGoMinus.Enabled = !over;
        }

        // ================= CONNECT =================
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormConnect { StartPosition = FormStartPosition.CenterParent };
            dlg.ShowDialog(this);

            // ✅ БЕЗ рефлексии — берём напрямую
            var opened = dlg.OpenPort;
            if (opened is null) return;

            _serial?.Dispose();
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Start()));
            _serial.Disconnected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Stop()));

            _serial.Attach();
            _ramp = new RampController(_serial);
            _serial.RequestAls();

        }

        private void btnOptions_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormOptions();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);

            ApplyOptionsToUi();

            var ramp = FormOptions.AppOptions.Current.PressureRamp;
            _ramp?.TryApply(ramp);   // SR без конвертаций


            // Если задан Ramp — шлём SR (без конвертаций)
           // var ramp = FormOptions.AppOptions.Current.PressureRamp;
            if (_serial != null && ramp is double r)
                _serial.Send($"SR {r.ToString("G", CultureInfo.InvariantCulture)}");
        }

        // ================= GO ± =================
        private void btnGoPlus_Click(object? sender, EventArgs e)
        {
            var inc = (double)nudIncrement.Value;
            var next = _setPoint + inc;

            if (next > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(this,
                    $"Cannot exceed Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} PSI).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ValidateTargetAgainstMax();
                return;
            }

            SendSetPoint(next);
            ValidateTargetAgainstMax();
        }

        private void btnGoMinus_Click(object? sender, EventArgs e)
        {
            var inc = (double)nudIncrement.Value;
            SendSetPoint(_setPoint - inc);
            ValidateTargetAgainstMax();
        }

        private void SendSetPoint(double sp)
        {
            _setPoint = sp;
            UI_SetSetPoint(_setPoint, _unit);

            if (_serial is null) return;

            if (sp > 0.05)
            {
                _serial.Send("AC");
                _isExhaust = false;
                _lastCurrent = null;
            }

            _serial.Send($"AS {sp.ToString("F2", CultureInfo.InvariantCulture)}");
            _serial.RequestAls();
        }

        // ================= GO TO TARGET =================
        private void btnGoTarget_Click(object? sender, EventArgs e)
        {
            // ❌ больше НЕ включаем кнопку принудительно
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = txtTarget.Text?.Trim() ?? string.Empty;
            if (raw.Length == 0)
            {
                MessageBox.Show("Enter target value.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetValue))
            {
                MessageBox.Show("Invalid target value format.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (targetValue > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(this,
                    $"Target value exceeds Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} PSI).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ValidateTargetAgainstMax();
                return;
            }

            const double MIN = 0.0, MAX_SOFT = 1000.0;
            if (targetValue < MIN || targetValue > MAX_SOFT)
            {
                MessageBox.Show($"Target value must be between {MIN} and {MAX_SOFT}.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string unit = string.IsNullOrWhiteSpace(_unit) ? "PSIG" : _unit;
            string displayVal = targetValue.ToString("F1", CultureInfo.InvariantCulture);
            if (!chkConfirmGo.Checked)
            {
                var ask = MessageBox.Show(
                    $"Do you want to change the target value to {displayVal} {unit}?",
                    "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask != DialogResult.Yes) return;
            }

            try
            {
                if (targetValue > 0.05)
                {
                    _serial.Send("AC");
                    _isExhaust = false;
                    _lastCurrent = null;
                }

                _serial.Send($"AS {targetValue:F1}");

                _setPoint = targetValue;
                UI_SetSetPoint(_setPoint, _unit);

                _serial.RequestAls();

                chkConfirmGo.Checked = false;
                txtTarget.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send command:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ValidateTargetAgainstMax();
            }
        }

        // ================= PURGE =================
        private async void btnPurge_Click(object? sender, EventArgs e)
        {
            if (_serial is null)
            {
                MessageBox.Show("Нет соединения с прибором.", "Purge",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!chkConfirmPurge.Checked)
            {
                var ask = MessageBox.Show("Сразу открыть выхлоп и удерживать?",
                                          "Confirm purge",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask != DialogResult.Yes) return;
            }
            chkConfirmPurge.Checked = false;

            try
            {
                _serial.Send("AE");
                _isExhaust = true;

                UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true);
                UI_Status_Up(false);
                UI_Status_Mid(false);
                UI_Status_Down(true);

                _setPoint = 0.0;
                UI_SetSetPoint(_setPoint, _unit);

                await Task.Delay(400);   // короткая пауза
                _serial.Send("AC");

                _serial.RequestAls();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка Purge: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ValidateTargetAgainstMax();
                ValidateIncrementAgainstMax();
            }
        }

        // ================= RX =================
        private void Serial_LineReceived(object? sender, string line)
        {
            Debug.WriteLine("RX: " + line);

            bool exh = line.IndexOf("EXH", StringComparison.OrdinalIgnoreCase) >= 0;
            if (exh) _isExhaust = true;

            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;

            _current = cur;
            if (!_isExhaust) _setPoint = sp;
            if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

            BeginInvoke(new Action(() =>
            {
                UI_SetTrendStatus(_lastCurrent, _current, _isExhaust);
                RefreshCurrent();
                UI_SetPressureUnits(_unit);
                UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                ValidateTargetAgainstMax();
                ValidateIncrementAgainstMax();

                _state.Update(_current, _setPoint, _unit, _isExhaust);
                _lastCurrent = _current;
            }));
        }

        private void RefreshCurrent() => lblCurrentBig.Text = $"{_current:0.0} {_unit}";

        private static bool TryParseAls(string line, out double cur, out double sp, out string? unit)
        {
            cur = 0; sp = 0; unit = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out cur)) return false;
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out sp)) return false;

            for (int i = 3; i < parts.Length; i++)
            {
                var p = parts[i].Trim().ToUpperInvariant();
                if (p is "PSIG" or "PSI" or "KPA" or "BAR") { unit = p; break; }
            }
            return true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _pollTimer.Stop();
            _serial?.Dispose();
        }

        // ===== SHOW VALUE helpers =====
        private static string TrimZeros(double v, int maxDecimals = 2) =>
            v.ToString("0." + new string('#', maxDecimals), CultureInfo.InvariantCulture);

        public void UI_SetPressureUnits(string units) =>
            boxPressureUnits.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();

        public void UI_SetRampSpeedUnits(string units) =>
            boxRampSpeedUnits.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();

        public void UI_SetSetPoint(double sp, string? units = null)
        {
            var u = string.IsNullOrWhiteSpace(units) ? _unit : units!;
            boxSetPoint.Text = $"{TrimZeros(sp)} {u}";
        }

        public void UI_SetTimeToSetPoint(TimeSpan? t)
        {
            if (t == null) { boxTimeToSetPoint.Text = "—"; return; }
            var secs = Math.Max(0, t.Value.TotalSeconds);
            boxTimeToSetPoint.Text = $"{TrimZeros(secs, 1)} s";
        }

        public void UI_Status_Up(bool on) => icoUp.ForeColor = on ? System.Drawing.Color.OrangeRed : System.Drawing.Color.Gray;
        public void UI_Status_Mid(bool on) => icoMid.ForeColor = on ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Gray;
        public void UI_Status_Down(bool on) => icoDown.ForeColor = on ? System.Drawing.Color.RoyalBlue : System.Drawing.Color.Gray;

        private void UI_SetTrendStatus(double? prev, double now, bool isExhaust)
        {
            if (isExhaust)
            {
                UI_Status_Up(false);
                UI_Status_Mid(false);
                UI_Status_Down(true);
                return;
            }

            const double EPS = 0.05;

            if (prev is null)
            {
                UI_Status_Up(false);
                UI_Status_Mid(true);
                UI_Status_Down(false);
                return;
            }

            double delta = now - prev.Value;
            if (delta > EPS) { UI_Status_Up(true); UI_Status_Mid(false); UI_Status_Down(false); }
            else if (delta < -EPS) { UI_Status_Up(false); UI_Status_Mid(false); UI_Status_Down(true); }
            else { UI_Status_Up(false); UI_Status_Mid(true); UI_Status_Down(false); }
        }
    }
}
