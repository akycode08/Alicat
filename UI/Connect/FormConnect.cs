// Подключаем нужные пространства имён
using System;                     // базовые типы и исключения
using System.IO.Ports;            // работа с COM-портами
using System.Linq;                // LINQ — для сортировки и работы с массивами
using System.Text;                // кодировки (ASCII)
using System.Windows.Forms;       // элементы управления WinForms

namespace Alicat
{
    // Определяем частичный класс формы (часть кода — здесь, часть в Designer)
    public partial class FormConnect : Form
    {
        // Храним ссылку на объект SerialPort, если порт открыт
        private SerialPort? _port;

        // ✅ Публичный доступ к открытому порту (заменяет рефлексию в главной форме)
        public SerialPort? OpenPort => _port?.IsOpen == true ? _port : null;

        public FormConnect()
        {
            InitializeComponent();
        }

        private void FormConnect_Load(object? sender, EventArgs e)
        {
            cbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
            cbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            cbBaudRate.SelectedItem = "19200";
            cbParity.SelectedItem = nameof(Parity.None);
            cbStopBits.SelectedItem = nameof(StopBits.One);
            nudDataBits.Value = 8;
            nudReadTimeout.Value = 700;
            nudWriteTimeout.Value = 700;

            RefreshPorts();

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        private void btnRefreshPorts_Click(object? sender, EventArgs e) => RefreshPorts();

        private void btnDefaults_Click(object? sender, EventArgs e)
        {
            cbBaudRate.SelectedItem = "19200";
            cbParity.SelectedItem = nameof(Parity.None);
            cbStopBits.SelectedItem = nameof(StopBits.One);
            nudDataBits.Value = 8;
            nudReadTimeout.Value = 700;
            nudWriteTimeout.Value = 700;
        }

        private void btnConnect_Click(object? sender, EventArgs e)
        {
            var prev = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            btnConnect.Enabled = false;

            try
            {
                if (_port?.IsOpen == true) TryClosePort();

                _port = BuildPort();
                _port.Open();

                string resp = PingAlicat(_port);

                if (string.IsNullOrWhiteSpace(resp) || !resp.StartsWith("A"))
                    MessageBox.Show("Порт открыт, но прибор ответил не так, как ожидалось.", "Внимание");
                else
                    MessageBox.Show($"Успешно подключено.\r\nОтвет: {resp}", "OK");
            }
            catch (TimeoutException)
            {
                TryReconnectWithAutoBaud();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("COM-порт занят другой программой. Закройте терминал/драйвер и попробуйте снова.", "Порт занят");
                TryClosePort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                TryClosePort();
            }
            finally
            {
                Cursor.Current = prev;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = true;
            }
        }

        private void btnDisconnect_Click(object? sender, EventArgs e)
        {
            bool wasOpen = _port?.IsOpen == true;
            TryClosePort();
            MessageBox.Show(wasOpen ? "Соединение разорвано." : "Порт уже был закрыт.", wasOpen ? "Disconnected" : "Info");
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        // ----- Вспомогательные -----

        private void RefreshPorts()
        {
            var ports = SerialPort.GetPortNames().OrderBy(x => x).ToArray();
            cbPortName.Items.Clear();
            cbPortName.Items.AddRange(ports);
            if (cbPortName.Items.Count > 0) cbPortName.SelectedIndex = 0;
        }

        private SerialPort BuildPort()
        {
            var portName = cbPortName.SelectedItem as string ?? throw new InvalidOperationException("Select PortName.");
            var baudText = cbBaudRate.SelectedItem as string ?? "19200";
            if (!int.TryParse(baudText, out var baud)) baud = 19200;

            var parity = Enum.Parse<Parity>(cbParity.SelectedItem as string ?? nameof(Parity.None));
            var stopBits = Enum.Parse<StopBits>(cbStopBits.SelectedItem as string ?? nameof(StopBits.One));

            return new SerialPort(portName, baud, parity, (int)nudDataBits.Value, stopBits)
            {
                Handshake = Handshake.None,
                Encoding = Encoding.ASCII,
                NewLine = "\r",
                ReadTimeout = Math.Max(1500, (int)nudReadTimeout.Value),
                WriteTimeout = Math.Max(1500, (int)nudWriteTimeout.Value),
                DtrEnable = false,
                RtsEnable = false
            };
        }

        private string PingAlicat(SerialPort sp)
        {
            sp.DiscardInBuffer();
            sp.Write("A\r");
            return sp.ReadLine();
        }

        private void TryReconnectWithAutoBaud()
        {
            TryClosePort();
            var speeds = new[] { "19200", "9600" };

            foreach (var s in speeds)
            {
                try
                {
                    cbBaudRate.SelectedItem = s;
                    _port = BuildPort();
                    _port.Open();
                    string resp = PingAlicat(_port);
                    if (!string.IsNullOrWhiteSpace(resp) && resp.StartsWith("A"))
                    {
                        MessageBox.Show($"Подключено на {s} бод.\r\nОтвет: {resp}", "OK");
                        return;
                    }
                }
                catch
                {
                    TryClosePort();
                }
            }

            MessageBox.Show("Таймаут ожидания ответа прибора.", "Ошибка");
            TryClosePort();
        }

        private void TryClosePort()
        {
            try
            {
                if (_port != null)
                {
                    try { _port.DiscardInBuffer(); } catch { }
                    if (_port.IsOpen)
                    {
                        _port.DtrEnable = false;
                        _port.RtsEnable = false;
                        _port.Close();
                    }
                    _port.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при закрытии порта: " + ex.Message, "Ошибка");
            }
            finally
            {
                _port = null;
            }
        }
    }
}
