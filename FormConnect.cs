using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alicat
{
    public partial class FormConnect : Form
    {
        private SerialPort? _port;

        public FormConnect()
        {
            InitializeComponent();
        }

        private void FormConnect_Load(object? sender, EventArgs e)
        {
            // Заполняем списки
            cbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
            cbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            // Значения по умолчанию
            cbBaudRate.SelectedItem = "19200";
            cbParity.SelectedItem = nameof(Parity.None);
            cbStopBits.SelectedItem = nameof(StopBits.One);
            nudDataBits.Value = 8;
            nudReadTimeout.Value = 700;
            nudWriteTimeout.Value = 700;

            // COM-порты
            RefreshPorts();

            // По требованию: обе кнопки всегда активны
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        private void btnRefreshPorts_Click(object? sender, EventArgs e)
        {
            RefreshPorts();
        }

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
            btnConnect.Enabled = false; // анти-даблклик на время операции

            try
            {
                // Если уже открыт — мягкий реконнект с текущими настройками UI
                if (_port?.IsOpen == true)
                {
                    TryClosePort();
                }

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
                btnConnect.Enabled = true;     // по итогу — снова доступна
                btnDisconnect.Enabled = true;  // всегда доступна
            }
        }

        private void btnDisconnect_Click(object? sender, EventArgs e)
        {
            bool wasOpen = _port?.IsOpen == true;

            TryClosePort();

            if (wasOpen)
                MessageBox.Show("Соединение разорвано.", "Disconnected");
            else
                MessageBox.Show("Порт уже был закрыт.", "Info");

            // Оставляем обе кнопки активными
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        // ---------------- Вспомогательные методы ----------------

        private void RefreshPorts()
        {
            var ports = SerialPort.GetPortNames().OrderBy(x => x).ToArray();
            cbPortName.Items.Clear();
            cbPortName.Items.AddRange(ports);
            if (cbPortName.Items.Count > 0)
                cbPortName.SelectedIndex = 0;
        }

        private SerialPort BuildPort()
        {
            var portName = cbPortName.SelectedItem as string
                ?? throw new InvalidOperationException("Select PortName.");

            var baudText = cbBaudRate.SelectedItem as string ?? "19200";
            if (!int.TryParse(baudText, out var baud)) baud = 19200;

            var parityText = cbParity.SelectedItem as string ?? nameof(Parity.None);
            var stopBitsText = cbStopBits.SelectedItem as string ?? nameof(StopBits.One);

            var parity = Enum.Parse<Parity>(parityText);
            var stopBits = Enum.Parse<StopBits>(stopBitsText);

            return new SerialPort(portName, baud, parity, (int)nudDataBits.Value, stopBits)
            {
                Handshake = Handshake.None,
                Encoding = Encoding.ASCII,
                NewLine = "\r", // Alicat завершает строки CR
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
            return sp.ReadLine(); // ждём до '\r'
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
                    try { _port.DiscardInBuffer(); } catch { /* ignore */ }

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
