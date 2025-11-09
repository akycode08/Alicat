using System;                           // базовые типы
using System.Diagnostics;
using System.Globalization;             // парс чисел Invariant
using System.IO.Ports;                  // SerialPort
using System.Text;                      // Encoding.ASCII
using System.Threading.Tasks;           // Task.Delay (на будущее)
using System.Windows.Forms;             // WinForms

// Явно используем таймер WinForms
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        // ---- Состояние измерений/уставки ----
        private double _current = 0.0;        // текущее давление
        private double _setPoint = 0.0;       // уставка
        private string _unit = "PSIG";        // единицы измерения

        private bool _isExhaust = false;      // сейчас в режиме выхлопа (после AE)
        private double? _lastCurrent = null;  // предыдущее текущее давление для тренда

        private SerialClient? _serial;        // клиент поверх SerialPort
        private readonly Timer _pollTimer = new() { Interval = 500 }; // опрос каждые 500мс

        // ---- Лимиты/настройки (из Options) ----
        private double _maxPressure = 200.0;        // PSI: лимит по умолчанию (Target)
        private double _maxIncrementLimit = 20.0;   // потолок для шага (из Options)

        public AlicatForm()
        {
            InitializeComponent();

            // Привязка UI-событий
            btnGoTarget.Click += btnGoTarget_Click;
            btnPurge.Click += btnPurge_Click;
            btnGoPlus.Click += btnGoPlus_Click;
            btnGoMinus.Click += btnGoMinus_Click;
            btnCommunication.Click += btnCommunication_Click;
            btnOptions.Click += btnOptions_Click;

            // Онлайн-валидация
            txtTarget.TextChanged += (_, __) => ValidateTargetAgainstMax();
            chkConfirmGo.CheckedChanged += (_, __) => ValidateTargetAgainstMax();

            // Для инкремента — постоянная подсветка при нарушении лимита
            nudIncrement.ValueChanged += (_, __) => ValidateIncrementAgainstMax();

            // Начальные значения в окнах «SHOW VALUE»
            UI_SetPressureUnits(_unit);         // PSIG
            UI_SetRampSpeedUnits("PSIG/s");     // при необходимости поменяешь
            UI_SetSetPoint(_setPoint, _unit);   // 0.0 PSIG
            UI_SetTimeToSetPoint(null);         // "—"
            UI_Status_Up(false);
            UI_Status_Mid(false);
            UI_Status_Down(false);

            RefreshCurrent(); // большой дисплей

            // Периодический опрос
            _pollTimer.Tick += (_, __) => _serial?.RequestAls();

            // Применить настройки из Options
            ApplyOptionsToUi();
        }

        // ========================== Options → Применение в главном окне ==========================
        private void ApplyOptionsToUi()
        {
            // Подтянуть лимиты из Options (дефолты на случай null)
            _maxPressure        = FormOptions.AppOptions.Current.MaxPressure  ?? 200.0;
            _maxIncrementLimit  = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;

            // Оформление nudIncrement (формат)
            nudIncrement.DecimalPlaces = 1;
            nudIncrement.Increment     = 0.1M;

            // ВАЖНО: не зажимать Maximum до лимита — иначе авто-кламп сломает UX.
            // Держим широкий диапазон, чтобы можно было увидеть красную подсветку при нарушении.
            if (nudIncrement.Minimum <= 0) nudIncrement.Minimum = 0.1M;
            if (nudIncrement.Maximum < 100000M) nudIncrement.Maximum = 100000M;

            // Пересчитать состояния
            ValidateTargetAgainstMax();
            ValidateIncrementAgainstMax();
        }

        // ===== Проверка Target против Max Pressure (как было)
        private void ValidateTargetAgainstMax()
        {
            var text = txtTarget.Text?.Trim();
            bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetVal);
            bool over = parsed && targetVal > _maxPressure;

            // Подсветка поля Target — горит, пока не исправим
            txtTarget.BackColor = over ? System.Drawing.Color.MistyRose : System.Drawing.SystemColors.Window;

            // Кнопка "Go to target": активна, если формат корректен и чекбокс отмечен
            btnGoTarget.Enabled = chkConfirmGo.Checked && parsed;
        }

        // ===== Проверка Increment против Max Increment (НОВАЯ логика)
        private void ValidateIncrementAgainstMax()
        {
            double inc = (double)nudIncrement.Value;
            bool over = inc > _maxIncrementLimit;

            // Подсветка поля Increment — горит, пока не вернут в предел
            nudIncrement.BackColor = over ? System.Drawing.Color.MistyRose : System.Drawing.SystemColors.Window;

            // При недопустимом инкременте отключаем +/− (без сообщений)
            btnGoPlus.Enabled  = !over;
            btnGoMinus.Enabled = !over;
        }

        // ================= Окно коммуникаций (FormConnect) =================
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormConnect { StartPosition = FormStartPosition.CenterParent };
            dlg.ShowDialog(this);

            // Забираем открытый SerialPort из FormConnect
            var opened = GetOpenPortFrom(dlg);
            if (opened == null) return;

            _serial?.Dispose();
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Start()));
            _serial.Disconnected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Stop()));

            _serial.Attach();
            _serial.RequestAls();
        }

        private void btnOptions_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormOptions();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);

            // Пользователь мог нажать Apply или OK — подтянем актуальные настройки
            ApplyOptionsToUi();
        }

        // Аккуратно вытащить private поле _port у FormConnect
        private static SerialPort? GetOpenPortFrom(FormConnect fc)
        {
            try
            {
                var f = typeof(FormConnect).GetField("_port",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var sp = f?.GetValue(fc) as SerialPort;
                if (sp != null && sp.IsOpen) return sp;
            }
            catch { }
            return null;
        }

        // ================= Управление уставкой (GO ±) =================
        private void btnGoPlus_Click(object? sender, EventArgs e)
        {
            // Если инкремент недопустим — кнопка уже отключена ValidateIncrementAgainstMax()
            var inc = (double)nudIncrement.Value;
            var next = _setPoint + inc;

            // (Поведение Max Pressure оставляем как было — с предупреждением)
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
            // Если инкремент недопустим — кнопка уже отключена ValidateIncrementAgainstMax()
            var inc = (double)nudIncrement.Value;
            SendSetPoint(_setPoint - inc);
            ValidateTargetAgainstMax();
        }

        private void SendSetPoint(double sp)
        {
            _setPoint = sp;
            UI_SetSetPoint(_setPoint, _unit);

            if (_serial is null) return;

            // Если были в EXH и хотим поднять давление — выйти из EXH
            if (sp > 0.05)
            {
                _serial.Send("AC");
                _isExhaust = false;
                _lastCurrent = null; // начнём тренд заново после выхода из EXH
            }

            _serial.Send($"AS {sp.ToString("F2", CultureInfo.InvariantCulture)}");
            _serial.RequestAls();
        }

        // ================= Кнопка Go to target =================
        private void btnGoTarget_Click(object? sender, EventArgs e)
        {
            btnGoTarget.Enabled = true;

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

            // ---- Проверка Max Pressure (как было) ----
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

            // Доп. базовый диапазон, если нужен
            const double MIN = 0.0, MAX_SOFT = 1000.0;
            if (targetValue < MIN || targetValue > MAX_SOFT)
            {
                MessageBox.Show($"Target value must be between {MIN} and {MAX_SOFT}.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Подтверждение только если чекбокс не отмечен
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
                btnGoTarget.Enabled = true;
            }
        }

        // ================= Purge: МГНОВЕННЫЙ ВЫХЛОП и УДЕРЖАНИЕ EXH =================
        private void btnPurge_Click(object? sender, EventArgs e)
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

        // ================= Приём/отрисовка данных =================
        private void Serial_LineReceived(object? sender, string line)
        {
            System.Diagnostics.Debug.WriteLine("RX: " + line); //==========================OUTPUT==========11/7/2025 JUST FOR OUTPUT

            bool exh = line.IndexOf("EXH", StringComparison.OrdinalIgnoreCase) >= 0;
            if (exh) _isExhaust = true;

            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;

            _current = cur;

            if (!_isExhaust)
                _setPoint = sp;

            if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

            BeginInvoke(new Action(() =>
            {
                UI_SetTrendStatus(_lastCurrent, _current, _isExhaust);

                RefreshCurrent();
                UI_SetPressureUnits(_unit);
                UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                ValidateTargetAgainstMax();
                ValidateIncrementAgainstMax();

                _lastCurrent = _current;
            }));
        }

        private void RefreshCurrent()
        {
            lblCurrentBig.Text = $"{_current:0.0} {_unit}";
        }

        // ================= Парсер ответа ALS =================
        // Примеры строк: "A +0030.0 +0030.0 10 PSIG", "A +0030.0 +0030.0"
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

        // ========= SHOW VALUE: helpers (показ данных) =========
        private static string TrimZeros(double v, int maxDecimals = 2)
        {
            return v.ToString("0." + new string('#', maxDecimals), CultureInfo.InvariantCulture);
        }

        // Units
        public void UI_SetPressureUnits(string units)
        {
            boxPressureUnits.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();
        }

        public void UI_SetRampSpeedUnits(string units)
        {
            boxRampSpeedUnits.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();
        }

        // Set point
        public void UI_SetSetPoint(double sp, string? units = null)
        {
            var u = string.IsNullOrWhiteSpace(units) ? _unit : units!;
            boxSetPoint.Text = $"{TrimZeros(sp)} {u}";
        }

        // Time to set point
        public void UI_SetTimeToSetPoint(TimeSpan? t)
        {
            if (t == null) { boxTimeToSetPoint.Text = "—"; return; }
            var secs = Math.Max(0, t.Value.TotalSeconds);
            boxTimeToSetPoint.Text = $"{TrimZeros(secs, 1)} s";
        }

        // Статус-индикаторы (только визуал)
        public void UI_Status_Up(bool on) => icoUp.ForeColor = on ? System.Drawing.Color.OrangeRed : System.Drawing.Color.Gray;
        public void UI_Status_Mid(bool on) => icoMid.ForeColor = on ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Gray;
        public void UI_Status_Down(bool on) => icoDown.ForeColor = on ? System.Drawing.Color.RoyalBlue : System.Drawing.Color.Gray;

        // Тренд: выбор индикатора ▲ ● ▼
        private void UI_SetTrendStatus(double? prev, double now, bool isExhaust)
        {
            if (isExhaust)
            {
                UI_Status_Up(false);
                UI_Status_Mid(false);
                UI_Status_Down(true);
                return;
            }

            const double EPS = 0.05; // чувствительность (единицы давления)

            if (prev is null)
            {
                UI_Status_Up(false);
                UI_Status_Mid(true);
                UI_Status_Down(false);
                return;
            }

            double delta = now - prev.Value;
            if (delta > EPS)
            {
                UI_Status_Up(true);
                UI_Status_Mid(false);
                UI_Status_Down(false);
            }
            else if (delta < -EPS)
            {
                UI_Status_Up(false);
                UI_Status_Mid(false);
                UI_Status_Down(true);
            }
            else
            {
                UI_Status_Up(false);
                UI_Status_Mid(true);
                UI_Status_Down(false);
            }
        }

        // ================= Мини-клиент поверх SerialPort =================
        private sealed class SerialClient : IDisposable
        {
            private readonly SerialPort _port;
            private bool _attached;

            public event EventHandler? Connected;
            public event EventHandler? Disconnected;
            public event EventHandler<string>? LineReceived;

            public SerialClient(SerialPort existingOpenPort)
            {
                _port = existingOpenPort;
                _port.NewLine = "\r";
                _port.Encoding = Encoding.ASCII;
            }

            public SerialClient(string portName, int baud)
            {
                _port = new SerialPort(portName, baud)
                {
                    NewLine = "\r",
                    Encoding = Encoding.ASCII,
                    DtrEnable = false,
                    RtsEnable = false,
                    ReadTimeout = 1500,
                    WriteTimeout = 1500
                };
            }

            public void Attach()
            {
                if (_attached) return;
                _port.DataReceived += Port_DataReceived;
                _attached = true;
                Connected?.Invoke(this, EventArgs.Empty);
            }

            public void Open()
            {
                if (_port.IsOpen) { Attach(); return; }
                _port.Open();
                Attach();
            }

            public void Close()
            {
                if (!_port.IsOpen) return;
                _port.Close();
                Disconnected?.Invoke(this, EventArgs.Empty);
            }

            public void Send(string cmd)
            {
                if (!_port.IsOpen) return;
                try { _port.Write(cmd + "\r"); } catch { }
            }

            public void RequestAls() => Send("ALS");

            private void Port_DataReceived(object? sender, SerialDataReceivedEventArgs e)
            {
                try
                {
                    var line = _port.ReadLine(); // до CR
                    if (!string.IsNullOrWhiteSpace(line))
                        LineReceived?.Invoke(this, line);
                }
                catch { }
            }

            public void Dispose()
            {
                try
                {
                    _port.DataReceived -= Port_DataReceived;
                    if (_port.IsOpen) _port.Close();
                    _port.Dispose();
                }
                catch { }
            }
        }
    }
}
