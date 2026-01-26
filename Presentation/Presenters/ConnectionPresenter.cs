using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using PrecisionPressureController.Business.Interfaces;
using PrecisionPressureController.Services.Serial;
using PrecisionPressureController.Services.Protocol;
using PrecisionPressureController.Services.Controllers;

namespace PrecisionPressureController.Presentation.Presenters
{
    /// <summary>
    /// Presenter для управления подключением к устройству.
    /// Отвечает только за подключение/отключение устройства.
    /// </summary>
    public class ConnectionPresenter
    {
        private readonly IMainView _view;
        private readonly IDataStore _dataStore;
        private ISerialClient? _serial;
        private IRampController? _ramp;

        public ConnectionPresenter(IMainView view, IDataStore dataStore)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        /// <summary>
        /// Получить текущий SerialClient (для использования другими презентерами)
        /// </summary>
        public ISerialClient? Serial => _serial;

        /// <summary>
        /// Получить текущий RampController (для использования другими презентерами)
        /// </summary>
        public IRampController? Ramp => _ramp;

        /// <summary>
        /// Проверяет, подключено ли устройство
        /// </summary>
        public bool IsConnected => _serial?.IsConnected ?? false;

        /// <summary>
        /// Быстрое подключение используя последние сохраненные настройки порта
        /// </summary>
        public void QuickConnect(Form parentForm, Action<ISerialClient> onConnected, Action onDisconnected)
        {
            try
            {
                // Загружаем сохраненные настройки коммуникации
                var commSettings = LoadCommunicationSettings();
                
                string? portName = commSettings.PortName;
                int baudRate = commSettings.BaudRate;
                string parity = commSettings.Parity;
                string stopBits = commSettings.StopBits;
                int dataBits = commSettings.DataBits;
                int readTimeout = commSettings.ReadTimeout;
                int writeTimeout = commSettings.WriteTimeout;

                // Если порт не сохранен, показываем диалог
                if (string.IsNullOrEmpty(portName))
                {
                    MessageBox.Show(parentForm,
                        "No saved port settings found. Please use 'Connect...' to configure and save port settings first.",
                        "Quick Connect",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    // ConnectDevice должен быть вызван из MainPresenter
                    return;
                }

                // Проверяем, доступен ли порт
                if (!SerialPort.GetPortNames().Contains(portName))
                {
                    var result = MessageBox.Show(parentForm,
                        $"Port {portName} is not available.\n\nWould you like to open connection dialog?",
                        "Port Not Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        // ConnectDevice должен быть вызван из MainPresenter
                        return;
                    }
                    return;
                }

                // Создаем SerialPort из сохраненных настроек
                var port = new SerialPort(portName, baudRate)
                {
                    Parity = ParseParity(parity),
                    StopBits = ParseStopBits(stopBits),
                    DataBits = dataBits,
                    ReadTimeout = readTimeout,
                    WriteTimeout = writeTimeout,
                    Handshake = Handshake.None,
                    Encoding = System.Text.Encoding.ASCII,
                    NewLine = "\r",
                    DtrEnable = false,
                    RtsEnable = false
                };

                // Открываем порт
                port.Open();

                // Проверяем подключение (ping)
                try
                {
                    port.DiscardInBuffer();
                    port.Write("A\r");
                    string response = port.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(response) || !response.StartsWith("A"))
                    {
                        port.Close();
                        port.Dispose();
                        MessageBox.Show(parentForm,
                            "Port opened, but device response was unexpected.",
                            "Quick Connect",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch (TimeoutException)
                {
                    port.Close();
                    port.Dispose();
                    MessageBox.Show(parentForm,
                        "Device response timeout. Please check connection settings.",
                        "Quick Connect",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Подключаемся через стандартный метод
                _serial?.Dispose();
                _serial = new SerialClient(port);
                _serial.LineReceived += (_, line) => onConnected?.Invoke(_serial);
                _serial.Connected += (_, __) => _view.BeginInvoke(new Action(() =>
                {
                    _view.UI_UpdateConnectionStatus(true, port.PortName);
                    onConnected?.Invoke(_serial);
                }));
                _serial.Disconnected += (_, __) => _view.BeginInvoke(new Action(() =>
                {
                    _view.UI_UpdateConnectionStatus(false);
                    onDisconnected?.Invoke();
                }));

                _serial.Attach();
                _ramp = new RampController(_serial);
                _serial.Send(DeviceCommands.ReadRampSpeed);

                if (!_dataStore.IsRunning)
                {
                    _dataStore.StartSession();
                }

                _view.UI_UpdateConnectionStatus(true, port.PortName);
                _view.UI_AppendStatusInfo($"Quick connected to {port.PortName}");
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Quick Connect failed: {ex.Message}");
                MessageBox.Show(parentForm,
                    $"Quick Connect failed:\n{ex.Message}\n\nWould you like to open connection dialog?",
                    "Quick Connect Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Отключает устройство
        /// </summary>
        public void Disconnect()
        {
            if (_serial != null)
            {
                _serial.Dispose();
                _serial = null;
            }
            _ramp = null;
            _view.UI_UpdateConnectionStatus(false);
        }

        /// <summary>
        /// Загружает настройки коммуникации из файла
        /// </summary>
        private (string? PortName, int BaudRate, string Parity, string StopBits, int DataBits, int ReadTimeout, int WriteTimeout) LoadCommunicationSettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!System.IO.File.Exists(settingsPath))
                {
                    return (null, 19200, "None", "One", 8, 700, 700);
                }

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                if (settingsData.TryGetProperty("Communication", out var comm))
                {
                    return (
                        comm.TryGetProperty("PortName", out var pn) && pn.ValueKind != System.Text.Json.JsonValueKind.Null ? pn.GetString() : null,
                        comm.TryGetProperty("BaudRate", out var br) && br.ValueKind != System.Text.Json.JsonValueKind.Null ? br.GetInt32() : 19200,
                        comm.TryGetProperty("Parity", out var par) ? par.GetString() ?? "None" : "None",
                        comm.TryGetProperty("StopBits", out var sb) ? sb.GetString() ?? "One" : "One",
                        comm.TryGetProperty("DataBits", out var db) && db.ValueKind != System.Text.Json.JsonValueKind.Null ? db.GetInt32() : 8,
                        comm.TryGetProperty("ReadTimeout", out var rt) && rt.ValueKind != System.Text.Json.JsonValueKind.Null ? rt.GetInt32() : 700,
                        comm.TryGetProperty("WriteTimeout", out var wt) && wt.ValueKind != System.Text.Json.JsonValueKind.Null ? wt.GetInt32() : 700
                    );
                }
            }
            catch { }

            return (null, 19200, "None", "One", 8, 700, 700);
        }

        private static string GetSettingsFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsDir = System.IO.Path.Combine(appDataPath, "PrecisionPressureController");
            if (!System.IO.Directory.Exists(settingsDir))
            {
                System.IO.Directory.CreateDirectory(settingsDir);
            }
            return System.IO.Path.Combine(settingsDir, "settings.json");
        }

        private static Parity ParseParity(string parity)
        {
            return parity?.ToUpperInvariant() switch
            {
                "NONE" => Parity.None,
                "ODD" => Parity.Odd,
                "EVEN" => Parity.Even,
                "MARK" => Parity.Mark,
                "SPACE" => Parity.Space,
                _ => Parity.None
            };
        }

        private static StopBits ParseStopBits(string stopBits)
        {
            return stopBits?.ToUpperInvariant() switch
            {
                "ONE" => StopBits.One,
                "ONEPOINTFIVE" => StopBits.OnePointFive,
                "TWO" => StopBits.Two,
                _ => StopBits.One
            };
        }
    }
}
