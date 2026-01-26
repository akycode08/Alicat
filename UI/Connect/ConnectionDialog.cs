// Подключаем нужные пространства имён
using System;                     // базовые типы и исключения
using System.IO.Ports;            // работа с COM-портами
using System.Linq;                // LINQ — для сортировки и работы с массивами
using System.Text;                // кодировки (ASCII)
using System.Windows.Forms;       // элементы управления WinForms

namespace PrecisionPressureController.UI.Connect
{
    // Диалог подключения к устройству
    public partial class ConnectionDialog : Form
    {
        // Храним ссылку на объект SerialPort, если порт открыт
        private SerialPort? _port;

        // ✅ Публичный доступ к открытому порту (заменяет рефлексию в главной форме)
        public SerialPort? OpenPort => _port?.IsOpen == true ? _port : null;

        // Публичные свойства для доступа к настройкам коммуникации
        public string? PortName => cbPortName.SelectedItem as string;
        public int BaudRate
        {
            get
            {
                var baudText = cbBaudRate.SelectedItem as string ?? "19200";
                return int.TryParse(baudText, out var baud) ? baud : 19200;
            }
        }
        public string Parity => cbParity.SelectedItem as string ?? "None";
        public string StopBits => cbStopBits.SelectedItem as string ?? "One";
        public int DataBits => (int)nudDataBits.Value;
        public int ReadTimeout => (int)nudReadTimeout.Value;
        public int WriteTimeout => (int)nudWriteTimeout.Value;

        public ConnectionDialog()
        {
            InitializeComponent();
            
            // Устанавливаем DialogResult при закрытии через X
            this.FormClosing += (sender, e) =>
            {
                if (this.DialogResult == DialogResult.None)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            };
        }

        private void ConnectionDialog_Load(object? sender, EventArgs e)
        {
            cbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
            cbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            // Загружаем сохраненные настройки коммуникации
            LoadCommunicationSettings();

            RefreshPorts();

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        private void LoadCommunicationSettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!System.IO.File.Exists(settingsPath)) 
                {
                    SetDefaults();
                    return;
                }

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                if (settingsData.TryGetProperty("Communication", out var comm))
                {
                    if (comm.TryGetProperty("BaudRate", out var br) && br.ValueKind != System.Text.Json.JsonValueKind.Null)
                    {
                        var baud = br.GetInt32();
                        var baudStr = baud.ToString();
                        if (cbBaudRate.Items.Contains(baudStr))
                            cbBaudRate.SelectedItem = baudStr;
                    }

                    if (comm.TryGetProperty("PortName", out var pn) && pn.ValueKind != System.Text.Json.JsonValueKind.Null)
                    {
                        var portName = pn.GetString();
                        if (!string.IsNullOrEmpty(portName) && cbPortName.Items.Contains(portName))
                            cbPortName.SelectedItem = portName;
                    }

                    if (comm.TryGetProperty("Parity", out var par))
                    {
                        var parityStr = par.GetString();
                        if (!string.IsNullOrEmpty(parityStr) && cbParity.Items.Contains(parityStr))
                            cbParity.SelectedItem = parityStr;
                    }

                    if (comm.TryGetProperty("StopBits", out var sb))
                    {
                        var stopBitsStr = sb.GetString();
                        if (!string.IsNullOrEmpty(stopBitsStr) && cbStopBits.Items.Contains(stopBitsStr))
                            cbStopBits.SelectedItem = stopBitsStr;
                    }

                    if (comm.TryGetProperty("DataBits", out var db) && db.ValueKind != System.Text.Json.JsonValueKind.Null)
                        nudDataBits.Value = Math.Max(5, Math.Min(8, db.GetInt32()));

                    if (comm.TryGetProperty("ReadTimeout", out var rt) && rt.ValueKind != System.Text.Json.JsonValueKind.Null)
                        nudReadTimeout.Value = Math.Max(100, rt.GetInt32());

                    if (comm.TryGetProperty("WriteTimeout", out var wt) && wt.ValueKind != System.Text.Json.JsonValueKind.Null)
                        nudWriteTimeout.Value = Math.Max(100, wt.GetInt32());
                }
                else
                {
                    SetDefaults();
                }
            }
            catch
            {
                SetDefaults();
            }
        }

        private void SetDefaults()
        {
            cbBaudRate.SelectedItem = "19200";
            cbParity.SelectedItem = "None";
            cbStopBits.SelectedItem = "One";
            nudDataBits.Value = 8;
            nudReadTimeout.Value = 700;
            nudWriteTimeout.Value = 700;
        }

        private static string GetSettingsFilePath()
        {
            // Используем ту же логику, что и в MainWindow.Presenter.cs
            string? projectDir = null;
            string? currentDir = System.IO.Directory.GetCurrentDirectory();
            
            if (!string.IsNullOrEmpty(currentDir))
            {
                var dir = new System.IO.DirectoryInfo(currentDir);
                while (dir != null)
                {
                    if (dir.GetFiles("*.csproj").Length > 0)
                    {
                        projectDir = dir.FullName;
                        break;
                    }
                    dir = dir.Parent;
                }
            }
            
            if (string.IsNullOrEmpty(projectDir))
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string? exeDir = System.IO.Path.GetDirectoryName(exePath);
                
                if (exeDir != null && exeDir.Contains("bin"))
                {
                    var dir = new System.IO.DirectoryInfo(exeDir);
                    while (dir != null && dir.Name != "PrecisionPressureController" && dir.GetFiles("*.csproj").Length == 0)
                    {
                        dir = dir.Parent;
                    }
                    projectDir = dir?.FullName ?? System.IO.Directory.GetCurrentDirectory();
                }
                else
                {
                    projectDir = System.IO.Directory.GetCurrentDirectory();
                }
            }
            
            string settingsDir = System.IO.Path.Combine(projectDir, "Settings");
            return System.IO.Path.Combine(settingsDir, "settings.json");
        }

        private void btnRefreshPorts_Click(object? sender, EventArgs e) => RefreshPorts();

        private void btnDefaults_Click(object? sender, EventArgs e)
        {
            SetDefaults();
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

                string resp = PingDevice(_port);

                if (string.IsNullOrWhiteSpace(resp) || !resp.StartsWith("A"))
                    MessageBox.Show("Port opened, but device response was unexpected.", "Warning");
                else
                    MessageBox.Show($"Successfully connected.\r\nResponse: {resp}", "OK");
            }
            catch (TimeoutException)
            {
                TryReconnectWithAutoBaud();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("COM port is busy. Close terminal/driver and try again.", "Port Busy");
                TryClosePort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
            MessageBox.Show(wasOpen ? "Connection closed." : "Port was already closed.", wasOpen ? "Disconnected" : "Info");
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

            var parity = Enum.Parse<Parity>(cbParity.SelectedItem as string ?? "None");
            var stopBits = Enum.Parse<StopBits>(cbStopBits.SelectedItem as string ?? "One");

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

        private string PingDevice(SerialPort sp)
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
                    string resp = PingDevice(_port);
                    if (!string.IsNullOrWhiteSpace(resp) && resp.StartsWith("A"))
                    {
                        MessageBox.Show($"Connected at {s} baud.\r\nResponse: {resp}", "OK");
                        return;
                    }
                }
                catch
                {
                    TryClosePort();
                }
            }

            MessageBox.Show("Device response timeout.", "Error");
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
                MessageBox.Show("Error closing port: " + ex.Message, "Error");
            }
            finally
            {
                _port = null;
            }
        }
    }
}
