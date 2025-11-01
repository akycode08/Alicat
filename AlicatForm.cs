using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

// Разруливаем конфликт двух Timer
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        // ---- Состояние ----
        private double _current = 0.0;     // текущее давление
        private double _setPoint = 0.0;    // уставка
        private string _unit = "PSIG";     // единицы из прибора

        private SerialClient? _serial;
        private readonly Timer _pollTimer = new() { Interval = 500 };

        public AlicatForm()
        {
            InitializeComponent();

            RefreshCurrent();

            // Привязка событий
            btnGoPlus.Click += btnGoPlus_Click;
            btnGoMinus.Click += btnGoMinus_Click;
            btnCommunication.Click += btnCommunication_Click;

            _pollTimer.Tick += (_, __) => _serial?.RequestAls();
        }

        // ================= Коммуникация через твою FormConnect =================
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormConnect
            {
                StartPosition = FormStartPosition.CenterParent
            };

            // показываем диалог настройки/подключения
            dlg.ShowDialog(this);

            // Если в форме порт открыт — берём его и начинаем работу
            var opened = GetOpenPortFrom(dlg);
            if (opened == null) return;

            // Закрываем предыдущее соединение, если было
            _serial?.Dispose();

            // Оборачиваем уже открытый SerialPort
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected    += (_, __) => BeginInvoke(new Action(() => _pollTimer.Start()));
            _serial.Disconnected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Stop()));

            // Считаем, что уже «подключены»
            _serial.Attach();      // повесить обработчики и вызвать Connected
            _serial.RequestAls();  // сразу спросим состояние
        }

        // Аккуратно пытаемся достать открытый порт у FormConnect (не ломая инкапсуляцию)
        private static SerialPort? GetOpenPortFrom(FormConnect fc)
        {
            // Попробуем через рефлексию получить поле _port (оно private).
            // Это безопасно и одноразово, чтобы не менять твою FormConnect.
            try
            {
                var f = typeof(FormConnect).GetField("_port",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var sp = f?.GetValue(fc) as SerialPort;
                if (sp != null && sp.IsOpen) return sp;
            }
            catch { /* игнор */ }
            return null;
        }

        // ================= Управление (GO ±) =================
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
            var cmd = $"AS {sp.ToString("0.###", CultureInfo.InvariantCulture)}";
            _serial?.Send(cmd);
            _serial?.RequestAls(); // сразу обновим состояние
        }

        // ================= Обновление данных =================
        private void Serial_LineReceived(object? sender, string line)
        {
            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;

            _current = cur;
            _setPoint = sp;
            _unit = unit ?? _unit;

            BeginInvoke(new Action(RefreshCurrent));
        }

        private void RefreshCurrent()
        {
            lblCurrentBig.Text = $"{_current:0.000} {_unit}";
        }

        // ================= Парсер ответа ALS =================
        private static bool TryParseAls(string line, out double cur, out double sp, out string? unit)
        {
            cur = 0;
            sp = 0;
            unit = null;

            if (string.IsNullOrWhiteSpace(line)) return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out cur)) return false;
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out sp)) return false;

            // Ищем единицы (PSIG/PSI/KPA/BAR)
            for (int i = 3; i < parts.Length; i++)
            {
                var p = parts[i].Trim().ToUpperInvariant();
                if (p is "PSIG" or "PSI" or "KPA" or "BAR")
                {
                    unit = p;
                    break;
                }
            }
            return true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _pollTimer.Stop();
            _serial?.Dispose();
        }

        // ================= Встроенный клиент SerialPort =================
        private sealed class SerialClient : IDisposable
        {
            private readonly SerialPort _port;
            private bool _attached;

            public event EventHandler? Connected;
            public event EventHandler? Disconnected;
            public event EventHandler<string>? LineReceived;

            // Конструктор №1: берём СУЩЕСТВУЮЩИЙ открытый порт от FormConnect
            public SerialClient(SerialPort existingOpenPort)
            {
                _port = existingOpenPort;
                // Alicat: CR без LF
                _port.NewLine = "\r";
                _port.Encoding = Encoding.ASCII;
            }

            // Конструктор №2 (на всякий случай): имя порта + скорость
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

            // Вешаем обработчики и сигналим «подключено»
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
                    // читаем по строке (до CR)
                    var line = _port.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        LineReceived?.Invoke(this, line);
                }
                catch { /* игнор/лог */ }
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
