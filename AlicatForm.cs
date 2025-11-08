using System;                           // базовые типы
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

        public AlicatForm()
        {
            InitializeComponent();

            // Привязка UI-событий
            btnGoTarget.Click += btnGoTarget_Click;
            btnPurge.Click += btnPurge_Click;
            btnGoPlus.Click += btnGoPlus_Click;
            btnGoMinus.Click += btnGoMinus_Click;
            btnCommunication.Click += btnCommunication_Click;

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


            // TEST

            TestSetRampSR_Number(1, 4);



            // TEST
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
            var inc = (double)nudIncrement.Value;
            SendSetPoint(_setPoint + inc);
        }

        private void btnGoMinus_Click(object? sender, EventArgs e)
        {
            var inc = (double)nudIncrement.Value;
            SendSetPoint(_setPoint - inc);
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
            // Кнопка остаётся активной по ТЗ
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

            const double MIN = 0.0, MAX = 100.0; // при необходимости подправь
            if (targetValue < MIN || targetValue > MAX)
            {
                MessageBox.Show($"Target value must be between {MIN} and {MAX}.", "Error",
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
                // Если выходим из EXH для ненулевой уставки
                if (targetValue > 0.05)
                {
                    _serial.Send("AC");
                    _isExhaust = false;
                    _lastCurrent = null; // сброс базы тренда
                }

                // Установить новую уставку
                _serial.Send($"AS {targetValue:F1}");

                // Моментально обновить UI
                _setPoint = targetValue;
                UI_SetSetPoint(_setPoint, _unit);

                // Подтянуть фактические данные с прибора
                _serial.RequestAls();

                // Сброс UI
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
                // 1) МГНОВЕННЫЙ ВЫХЛОП — только AE
                _serial.Send("AE");
                _isExhaust = true;

                // тренд сразу ▼ относительно последнего значения
                UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true);

                // 2) Визуал: стрелка вниз
                UI_Status_Up(false);
                UI_Status_Mid(false);
                UI_Status_Down(true);

                // 3) В UI фиксируем SP=0 (не посылая AS/AC — удерживаем EXH!)
                _setPoint = 0.0;
                UI_SetSetPoint(_setPoint, _unit);

                // 4) Опрос для обновления текущего
                _serial.RequestAls();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка Purge: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================= Приём/отрисовка данных =================
        private void Serial_LineReceived(object? sender, string line)
        {
            System.Diagnostics.Debug.WriteLine("RX: " + line); //==========================OUTPUT==========11/7/2025 JUST FOR OUTPUT

            // Флаг EXH по тексту ответа
            bool exh = line.IndexOf("EXH", StringComparison.OrdinalIgnoreCase) >= 0;
            if (exh) _isExhaust = true;

            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;

            _current = cur;

            // Если НЕ в EXH — обновляем уставку из прибора; если в EXH — держим локально (0)
            if (!_isExhaust)
                _setPoint = sp;

            if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

            BeginInvoke(new Action(() =>
            {
                // 1) статус по тренду (до перерисовки)
                UI_SetTrendStatus(_lastCurrent, _current, _isExhaust);

                // 2) перерисовка
                RefreshCurrent();            // большой дисплей
                UI_SetPressureUnits(_unit);  // единицы
                UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                // 3) сохранить текущее как предыдущее для следующего тика
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
            // В режиме выхлопа всегда горит ▼
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





        // TEST
        // ===== SR (numeric time code) =====

        // Прочитать текущий Ramp (пытаемся двумя способами)
        private async void TestReadRampSR_Number()
        {
            if (_serial == null) { MessageBox.Show("Прибор не подключён."); return; }

            System.Diagnostics.Debug.WriteLine("TX: SR");
            _serial.Send("SR");        // без UnitID
            await Task.Delay(120);

            System.Diagnostics.Debug.WriteLine("TX: ASR");
            _serial.Send("ASR");       // с UnitID=A
            await Task.Delay(120);
        }

        // Установить Ramp со временем по коду (напр. timeCode=4 => секунды)
        private async void TestSetRampSR_Number(double rampValue, int timeCode = 4)
        {
            if (_serial == null) { MessageBox.Show("Прибор не подключён."); return; }

            // на всякий включим контур
            _serial.Send("AC");
            await Task.Delay(80);

            // запросим текущее
            System.Diagnostics.Debug.WriteLine("TX: SR");
            _serial.Send("SR");
            await Task.Delay(120);

            // установка с UnitID=A (как в твоём примере "ASR 7 4")
            string cmdA = $"ASR {rampValue.ToString("0.###", CultureInfo.InvariantCulture)} {timeCode}";
            System.Diagnostics.Debug.WriteLine("TX: " + cmdA);
            _serial.Send(cmdA);
            await Task.Delay(150);

            // дубль без UnitID (на случай другой прошивки)
            string cmd = $"SR {rampValue.ToString("0.###", CultureInfo.InvariantCulture)} {timeCode}";
            System.Diagnostics.Debug.WriteLine("TX: " + cmd);
            _serial.Send(cmd);
            await Task.Delay(150);

            // проверим и подтянем статус
            _serial.Send("SR");
            await Task.Delay(120);
            _serial.RequestAls();
        }


    }
}
