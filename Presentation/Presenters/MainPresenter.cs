using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrecisionPressureController.Business.Interfaces;
using PrecisionPressureController.Domain;
using PrecisionPressureController.Services.Controllers;
using PrecisionPressureController.Services.Data;
using PrecisionPressureController.Services.Protocol;
using PrecisionPressureController.Services.Serial;
using PrecisionPressureController.Services.Sequence;
using PrecisionPressureController.UI.Connect;
using PrecisionPressureController.UI.Options;
using PrecisionPressureController.UI.Features.Graph.Views;
using PrecisionPressureController.UI.Features.Table.Views;
using PrecisionPressureController.UI.Features.Terminal.Views;

namespace PrecisionPressureController.Presentation.Presenters
{
    /// <summary>
    /// Presenter для MainForm - содержит всю бизнес-логику.
    /// View (MainWindow) только отображает данные и вызывает методы Presenter.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IDataStore _dataStore;

        // Device state
        private double _current = 0.0;
        private double _setPoint = 0.0;
        private string _unit = "PSIG";
        private double _rampSpeed = 0.0;

        private bool _isExhaust = false;
        private bool _isPaused = false;
        private double? _lastCurrent = null;

        // Services
        private ISerialClient? _serial;
        private IRampController? _ramp;
        private readonly System.Timers.Timer _pollTimer = new(500) { AutoReset = true };
        private bool _isWaitingForResponse = false; // Защита от переполнения при малых интервалах
        
        // Презентеры для разделения ответственности
        private readonly ConnectionPresenter _connectionPresenter;
        private readonly SequencePresenter _sequencePresenter;
        private readonly DataPresenter _dataPresenter;
        
        // Connection timeout detection - сторожевой таймер
        private System.Threading.Timer? _watchdogTimer;
        private const int ConnectionTimeoutMs = 30000; // Таймаут 30 секунд без ответа (увеличено для длительных тестов)

        // Settings
        private double _maxPressure = 200.0;
        private double _minPressure = 0.0;
        private double _maxIncrementLimit = 20.0;
        private double _minIncrementLimit = 0.1;
        private double _currentIncrement = 5.0;

        // State
        private readonly DeviceState _state = new();

        // Child forms - через интерфейсы для устранения циклических зависимостей
        private ITerminalView? _terminalView;
        private IGraphView? _graphView;
        private ITableView? _tableView;
        private PrecisionPressureController.UI.Features.Test.TestPressureWindow? _testPressureForm;

        public MainPresenter(IMainView view, IDataStore dataStore)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

            // Инициализируем презентеры
            _connectionPresenter = new ConnectionPresenter(_view, _dataStore);
            _sequencePresenter = new SequencePresenter(_view, _dataStore);
            _dataPresenter = new DataPresenter(_dataStore);

            // Setup polling timer - используем System.Timers.Timer для более точного polling
            // Этот таймер работает в отдельном потоке и не зависит от UI потока
            _pollTimer.Elapsed += (_, __) =>
            {
                // Не отправляем новый запрос, пока не получен ответ на предыдущий
                // Это защищает от переполнения при малых интервалах (например, 10мс)
                var serial = _connectionPresenter.Serial ?? _serial;
                if (serial != null && !_isWaitingForResponse)
                {
                    _isWaitingForResponse = true;
                    serial.Send(DeviceCommands.ReadAls);
                }
            };

            // Инициализируем SequenceService - работает независимо от UI
            InitializeSequenceService();
        }

        /// <summary>
        /// Очищает данные последовательности при закрытии программы
        /// </summary>
        public void ClearSequenceOnExit()
        {
            _sequencePresenter.ClearSequenceOnExit();
        }

        /// <summary>
        /// Получает текущий PointIndex из SequenceService
        /// PointIndex = 0 когда последовательность не запущена
        /// PointIndex = 1 когда активна первая точка
        /// PointIndex = 2 когда активна вторая точка и т.д.
        /// </summary>
        private int GetCurrentPointIndex()
        {
            return _sequencePresenter.GetCurrentPointIndex();
        }

        /// <summary>
        /// Инициализирует SequenceService для работы последовательности в фоне
        /// </summary>
        private void InitializeSequenceService()
        {
            // Функция для получения текущего давления
            double GetCurrentPressure()
            {
                if (_dataStore is SessionDataStore sessionStore && sessionStore.Points.Count > 0)
                {
                    return sessionStore.Points.Last().Current;
                }
                return _current;
            }

            // Функция для установки целевого давления
            void SetTargetPressure(double target)
            {
                SetTargetSilent(target);
            }

            // Инициализируем SequencePresenter
            _sequencePresenter.Initialize(
                GetCurrentPressure,
                SetTargetPressure,
                () =>
                {
                    // Обновляем ChartWindow, если он открыт
                    if (_graphView != null && !_graphView.IsDisposed)
                    {
                        _view.BeginInvoke(() =>
                        {
                            _graphView.RefreshSequenceState();
                        });
                    }
                }
            );
        }

        // ====================================================================
        // INITIALIZATION
        // ====================================================================

        /// <summary>
        /// Инициализация Presenter после создания View.
        /// </summary>
        public void Initialize()
        {
            // Initial UI state
            _view.UI_SetPressureUnits(_unit);
            _view.UI_SetSetPoint(_setPoint, _unit);
            _view.RefreshCurrent();
            _view.UI_UpdateConnectionStatus(false);

            // Load options
            ApplyOptionsToUi();

            // Обновляем текст "Last update" при запуске программы
            UpdateLastUpdateText();
        }

        // ====================================================================
        // CONNECTION
        // ====================================================================

        /// <summary>
        /// Быстрое подключение используя последние сохраненные настройки порта
        /// </summary>
        public void QuickConnect(Form parentForm)
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
                    ConnectDevice(parentForm);
                    return;
                }

                // Проверяем, доступен ли порт
                if (!System.IO.Ports.SerialPort.GetPortNames().Contains(portName))
                {
                    var result = MessageBox.Show(parentForm,
                        $"Port {portName} is not available.\n\nWould you like to open connection dialog?",
                        "Port Not Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        ConnectDevice(parentForm);
                    }
                    return;
                }

                // Создаем SerialPort из сохраненных настроек
                var port = new System.IO.Ports.SerialPort(portName, baudRate)
                {
                    Parity = ParseParity(parity),
                    StopBits = ParseStopBits(stopBits),
                    DataBits = dataBits,
                    ReadTimeout = readTimeout,
                    WriteTimeout = writeTimeout,
                    Handshake = System.IO.Ports.Handshake.None,
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
                _serial.LineReceived += Serial_LineReceived;
                _serial.Connected += (_, __) => _view.BeginInvoke(new Action(() =>
                {
                    _pollTimer.Start();
                    StartWatchdogTimer();
                    _view.UI_UpdateConnectionStatus(true, port.PortName);
                    
                    if (_graphView != null && !_graphView.IsDisposed)
                    {
                        _graphView.SetConnectionInfo(port.PortName, port.BaudRate);
                    }
                    
                    if (_tableView != null && !_tableView.IsDisposed)
                    {
                        _tableView.SetConnectionInfo(port.PortName, port.BaudRate);
                    }
                }));
                _serial.Disconnected += (_, __) => _view.BeginInvoke(new Action(() =>
                {
                    _pollTimer.Stop();
                    StopWatchdogTimer();
                    _view.UI_UpdateConnectionStatus(false);
                    
                    if (_graphView != null && !_graphView.IsDisposed)
                    {
                        _graphView.SetConnectionInfo(null, null);
                    }
                    
                    if (_tableView != null && !_tableView.IsDisposed)
                    {
                        _tableView.SetConnectionInfo(null, null);
                    }
                }));

                _serial.Attach();
                _ramp = new RampController(_serial);
                _serial.Send(DeviceCommands.ReadRampSpeed);

                if (!_dataStore.IsRunning)
                {
                    _dataPresenter.StartSession();
                }

                StartWatchdogTimer();

                _view.UI_UpdateConnectionStatus(true, port.PortName);
                _view.UI_AppendStatusInfo($"Quick connected to {port.PortName}");
                
                if (_graphView != null && !_graphView.IsDisposed)
                {
                    _graphView.SetConnectionInfo(port.PortName, port.BaudRate);
                }
                
                if (_tableView != null && !_tableView.IsDisposed)
                {
                    _tableView.SetConnectionInfo(port.PortName, port.BaudRate);
                }
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

        private static System.IO.Ports.Parity ParseParity(string parity)
        {
            return parity?.ToUpperInvariant() switch
            {
                "NONE" => System.IO.Ports.Parity.None,
                "ODD" => System.IO.Ports.Parity.Odd,
                "EVEN" => System.IO.Ports.Parity.Even,
                "MARK" => System.IO.Ports.Parity.Mark,
                "SPACE" => System.IO.Ports.Parity.Space,
                _ => System.IO.Ports.Parity.None
            };
        }

        private static System.IO.Ports.StopBits ParseStopBits(string stopBits)
        {
            return stopBits?.ToUpperInvariant() switch
            {
                "ONE" => System.IO.Ports.StopBits.One,
                "TWO" => System.IO.Ports.StopBits.Two,
                "ONEPOINTFIVE" => System.IO.Ports.StopBits.OnePointFive,
                _ => System.IO.Ports.StopBits.One
            };
        }

        /// <summary>
        /// Подключение к устройству через диалог выбора порта.
        /// </summary>
        public void ConnectDevice(Form parentForm)
        {
            // Сохраняем состояние подключения до открытия диалога
            bool wasConnected = _serial != null;
            
            using var dlg = new ConnectionDialog { StartPosition = FormStartPosition.CenterParent };
            dlg.ShowDialog(parentForm);

            var opened = dlg.OpenPort;
            
            // Если порт не открыт, но до этого было подключение - отключаем
            if (opened is null)
            {
                if (wasConnected)
                {
                    // Пользователь отключил устройство через диалог
                    _serial?.Dispose();
                    _serial = null;
                    _ramp = null;
                    _pollTimer.Stop();
                    _view.UI_UpdateConnectionStatus(false);
                }
                return;
            }

            _serial?.Dispose();
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected += (_, __) => _view.BeginInvoke(new Action(() =>
            {
                _pollTimer.Start();
                StartWatchdogTimer();
                _view.UI_UpdateConnectionStatus(true, opened.PortName);
                
                // Update ChartWindow connection info if open
                if (_graphView != null && !_graphView.IsDisposed && _serial is SerialClient serialClient)
                {
                    _graphView.SetConnectionInfo(serialClient.PortName, serialClient.BaudRate);
                }
                
                // Update DataTableWindow connection info if open
                if (_tableView != null && !_tableView.IsDisposed && _serial is SerialClient serialClient2)
                {
                    _tableView.SetConnectionInfo(serialClient2.PortName, serialClient2.BaudRate);
                }
            }));
            _serial.Disconnected += (_, __) => _view.BeginInvoke(new Action(() =>
            {
                _pollTimer.Stop();
                StopWatchdogTimer();
                _view.UI_UpdateConnectionStatus(false);
                
                // Update ChartWindow connection info if open
                if (_graphView != null && !_graphView.IsDisposed)
                {
                    _graphView.SetConnectionInfo(null, null);
                }
                
                // Update DataTableWindow connection info if open
                if (_tableView != null && !_tableView.IsDisposed)
                {
                    _tableView.SetConnectionInfo(null, null);
                }
            }));

            try
            {
                _serial.Attach();
                _ramp = new RampController(_serial);
                _serial.Send(DeviceCommands.ReadRampSpeed);

                if (!_dataStore.IsRunning)
                {
                    _dataPresenter.StartSession();
                }

                // Запускаем сторожевой таймер
                StartWatchdogTimer();

                // Сохраняем настройки коммуникации после успешного подключения
                SaveCommunicationSettings(dlg, opened);

                _view.UI_UpdateConnectionStatus(true, opened.PortName);
                _view.UI_AppendStatusInfo($"Device connected to {opened.PortName}");
                
                // Update ChartWindow connection info if open
                if (_graphView != null && !_graphView.IsDisposed)
                {
                    _graphView.SetConnectionInfo(opened.PortName, opened.BaudRate);
                }
                
                // Update DataTableWindow connection info if open
                if (_tableView != null && !_tableView.IsDisposed)
                {
                    _tableView.SetConnectionInfo(opened.PortName, opened.BaudRate);
                }
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Connection failed: {ex.Message}");
                _view.UI_UpdateConnectionStatus(false);
                MessageBox.Show($"Failed to connect: {ex.Message}", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // SERIAL COMMUNICATION
        // ====================================================================

        private void Serial_LineReceived(object? sender, string line)
        {
            Debug.WriteLine("RX: " + line);

            bool exh = line.IndexOf("EXH", StringComparison.OrdinalIgnoreCase) >= 0;
            if (exh) _isExhaust = true;

            // Log to terminal if open
            if (_terminalView != null && !_terminalView.IsDisposed)
            {
                _terminalView.AppendLog("<< " + line);
            }

            // Сбрасываем сторожевой таймер при получении любого ответа
            ResetWatchdogTimer();

            // 1) Try to parse ASR (Ramp Speed)
            if (TryParseAsr(line, out var ramp, out var rampUnits))
            {
                _rampSpeed = ramp;
                // Сбрасываем флаг ожидания ответа
                _isWaitingForResponse = false;
                _view.BeginInvoke(new Action(() =>
                {
                    _view.UI_SetRampSpeedUnits($"{TrimZeros(ramp)} {rampUnits}");
                    
                    // Update test pressure form if open
                    if (_testPressureForm != null && !_testPressureForm.IsDisposed)
                    {
                        _testPressureForm.UpdateCurrentRampSpeed(ramp, rampUnits);
                    }
                }));
                return;
            }

            // 2) Try to parse ALS
            if (!TryParseAls(line, out var cur, out var sp, out var unit))
            {
                // Если это не ASR и не ALS, сбрасываем флаг ожидания
                // (на случай, если получен другой ответ)
                _isWaitingForResponse = false;
                return;
            }

            _current = cur;
            if (!_isExhaust) _setPoint = sp;
            // Единицы всегда берутся из ответов устройства
            if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

            // Сбрасываем флаг ожидания ответа
            _isWaitingForResponse = false;

            _view.BeginInvoke(new Action(() =>
            {
                // Обновляем свойства в View
                ((IMainView)_view).Current = _current;
                ((IMainView)_view).SetPoint = _setPoint;
                ((IMainView)_view).Unit = _unit;
                ((IMainView)_view).RampSpeed = _rampSpeed;

                _view.UI_SetTrendStatus(_lastCurrent, _current, _isExhaust, _rampSpeed);
                _view.RefreshCurrent();
                _view.UI_SetPressureUnits(_unit);
                _view.UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                _view.ValidateTargetAgainstMax();
                _view.ValidateIncrementAgainstMax();

                _state.Update(_current, _setPoint, _unit, _isExhaust);
                _lastCurrent = _current;

                // Обновляем "Last update" на основе интервала таймера
                UpdateLastUpdateText();

                // Record to store
                // Получаем PointIndex из SequenceService (0 = до старта, 1+ = номер активной точки)
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordSample(_current, _isExhaust ? 0.0 : _setPoint, _unit, _rampSpeed, (int)_pollTimer.Interval, pointIndex);

                // Update graph if open
                if (_graphView != null && !_graphView.IsDisposed)
                {
                    double? targetForGraph = _isExhaust ? (double?)null : _setPoint;
                    _graphView.AddSample(_current, targetForGraph);
                }

                // Update test pressure form if open
                if (_testPressureForm != null && !_testPressureForm.IsDisposed)
                {
                    _testPressureForm.UpdateCurrentPressure(_isExhaust ? 0.0 : _setPoint, _unit);
                    _testPressureForm.UpdateCurrentRampSpeed(_rampSpeed, _unit);
                }

                // DataTableWindow получает данные через события DataStore.OnNewPoint
                // Не нужно вызывать AddRecordFromDevice напрямую
            }));
        }

        // ====================================================================
        // PARSERS
        // ====================================================================

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

                bool isKnownUnit = p == "PA" || p == "PAG" || p == "HPA" || p == "HPAG" ||
                                   p == "KPA" || p == "KPAG" || p == "MPA" || p == "MPAG" ||
                                   p == "MBAR" || p == "MBARG" || p == "BAR" || p == "BARG" ||
                                   p == "KG/CM" || p == "KGCM" || p == "KG/CMG" || p == "KGCMG" ||
                                   p == "PSIG" || p == "PSI" || p == "PSFG" || p == "PSF" ||
                                   p == "MTORR" || p == "MTORRG" || p == "TORR" || p == "TORRG" ||
                                   p == "---" || p == "" ||
                                   p.StartsWith("G/CM") || p.StartsWith("GCM");

                if (isKnownUnit)
                {
                    unit = NormalizeUnit(p);
                    break;
                }
            }
            return true;
        }

        private static string NormalizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit == "---" || unit == "")
                return "PSIG";

            var upper = unit.ToUpperInvariant();

            if (upper.EndsWith("G") && upper != "PSIG" && upper != "PSFG")
            {
                upper = upper.Substring(0, upper.Length - 1);
            }

            if (upper == "G/CM²" || upper == "G/CM2" || upper == "GCM²" || upper == "GCM2")
                return "g/cm²";

            if (upper == "KG/CM" || upper == "KGCM")
                return "kg/cm";

            return upper switch
            {
                "PA" => "Pa",
                "HPA" => "hPa",
                "KPA" => "kPa",
                "MPA" => "MPa",
                "MBAR" => "mbar",
                "BAR" => "bar",
                "PSIG" => "PSIG",
                "PSI" => "PSI",
                "PSFG" => "PSF",
                "PSF" => "PSF",
                "MTORR" => "mTorr",
                "TORR" => "torr",
                _ => unit
            };
        }

        private static bool TryParseAsr(string line, out double ramp, out string units)
        {
            ramp = 0;
            units = "PSIG/s";

            if (string.IsNullOrWhiteSpace(line))
                return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return false;

            if (!parts[0].Equals("A", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out ramp))
                return false;

            string? foundUnits = null;
            for (int i = 1; i < parts.Length; i++)
            {
                var p = parts[i].Trim();
                if (p.EndsWith("/s", StringComparison.OrdinalIgnoreCase))
                {
                    foundUnits = p;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(foundUnits))
                return false;

            var unitWithoutSlash = foundUnits.Substring(0, foundUnits.Length - 2).Trim();
            var normalizedUnit = NormalizeUnit(unitWithoutSlash);
            units = $"{normalizedUnit}/s";

            return true;
        }

        private static string TrimZeros(double v, int maxDecimals = 2) =>
            v.ToString("0." + new string('#', maxDecimals), CultureInfo.InvariantCulture);

        // ====================================================================
        // COMMANDS
        // ====================================================================

        public void GoToTarget(string targetText)
        {
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = targetText?.Trim() ?? string.Empty;
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

            // Проверка на превышение Max Pressure
            if (targetValue > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Target value exceeds Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _view.ValidateTargetAgainstMax();
                return;
            }

            // Проверка на значение ниже Min Pressure
            if (targetValue < _minPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Target value is below Min Pressure ({_minPressure.ToString("0.###", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _view.ValidateTargetAgainstMax();
                return;
            }

            string unit = string.IsNullOrWhiteSpace(_unit) ? "PSIG" : _unit;
            string displayVal = targetValue.ToString("F1", CultureInfo.InvariantCulture);

            var ask = MessageBox.Show(
                $"Do you want to change the target value to {displayVal} {unit}?",
                "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask != DialogResult.Yes) return;

            try
            {
                // Если выхлоп открыт (purge режим), закрываем его перед установкой давления
                if (_isExhaust)
                {
                    _serial.Send(DeviceCommands.ControlOn);
                    _isExhaust = false;
                    _lastCurrent = null;
                    _view.UI_SetTrendStatus(_lastCurrent, _current, isExhaust: false, _rampSpeed);
                    _view.UI_AppendStatusInfo("Exhaust closed - returning to control mode");
                }

                _serial.Send(DeviceCommands.SetSetPoint(targetValue));

                _setPoint = targetValue;
                _view.UI_SetSetPoint(_setPoint, _unit);
                _serial.Send(DeviceCommands.ReadAls);

                _view.TargetInputText = "";

                _view.UI_AppendStatusInfo($"Target set to {displayVal} {unit}");
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Command failed: GoToTarget - {ex.Message}");
                MessageBox.Show("Failed to send command:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Pause()
        {
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Pause",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Toggle pause state
                if (_isPaused)
                {
                    // Resume - restore previous setpoint or use current target
                    _isPaused = false;
                    _view.UI_AppendStatusInfo("Ramp resumed");
                    int pointIndex = GetCurrentPointIndex();
                    _dataPresenter.RecordEvent(_current, _setPoint, _unit, "RESUMED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
                }
                else
                {
                    // Pause - set setpoint to current
                    _serial.Send($"AS{_current:F2}");
                    _setPoint = _current;
                    _view.UI_SetSetPoint(_current, _unit);
                    _isPaused = true;
                    _view.UI_AppendStatusInfo($"Ramp paused - setpoint set to current ({_current:F2} {_unit}). Polling continues.");
                    int pointIndex = GetCurrentPointIndex();
                    _dataPresenter.RecordEvent(_current, _setPoint, _unit, "PAUSED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
                }
                
                // Update ChartWindow pause state
                if (_graphView != null && !_graphView.IsDisposed)
                {
                    _graphView.UpdatePauseState(_isPaused);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to pause: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Task Purge()
        {
            if (_serial is null)
            {
                MessageBox.Show("No connection to device.", "Purge",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Task.CompletedTask;
            }

            var ask = MessageBox.Show("Open exhaust and hold?",
                                      "Confirm purge",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask != DialogResult.Yes) return Task.CompletedTask;

            try
            {
                _serial.Send(DeviceCommands.ExhaustHold);
                _isExhaust = true;

                _view.UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true, _rampSpeed);
                _view.UI_AppendStatusInfo("Purge started - exhaust open");

                // Устанавливаем setpoint на устройстве в 0.0
                _serial.Send(DeviceCommands.SetSetPoint(0.0));
                _setPoint = 0.0;
                _view.UI_SetSetPoint(_setPoint, _unit);

                _serial.Send(DeviceCommands.ReadAls);

                // Записываем событие Purge
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordEvent(_current, _setPoint, _unit, "PURGE_STARTED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Purge error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Task.CompletedTask;
        }

        public void Increase()
        {
            var next = _setPoint + _currentIncrement;

            if (next > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Cannot exceed Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SendSetPoint(next);
            _view.UI_AppendStatusInfo($"Pressure increased by {_currentIncrement:F1} {_unit}");
        }

        public void Decrease()
        {
            var next = _setPoint - _currentIncrement;
            
            if (next < _minPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Cannot go below Min Pressure ({_minPressure.ToString("0.###", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            SendSetPoint(next);
            _view.UI_AppendStatusInfo($"Pressure decreased by {_currentIncrement:F1} {_unit}");
        }

        private void SendSetPoint(double sp)
        {
            _setPoint = sp;
            _view.UI_SetSetPoint(_setPoint, _unit);

            if (_serial is null) return;

            // Если выхлоп открыт (purge режим), закрываем его перед установкой давления
            if (_isExhaust)
            {
                _serial.Send(DeviceCommands.ControlOn);
                _isExhaust = false;
                _lastCurrent = null;
                _view.UI_SetTrendStatus(_lastCurrent, _current, isExhaust: false, _rampSpeed);
                _view.UI_AppendStatusInfo("Exhaust closed - returning to control mode");
            }

            try
            {
                _serial.Send(DeviceCommands.SetSetPoint(sp));
                _serial.Send(DeviceCommands.ReadAls);

                // Записываем событие изменения давления
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordEvent(_current, _setPoint, _unit, "TARGET_CHANGED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Command failed: AS {sp:F2} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Устанавливает целевое давление без подтверждения (для тестового режима)
        /// </summary>
        public void SetTargetSilent(double targetValue)
        {
            if (_serial == null)
            {
                throw new InvalidOperationException("Device is not connected.");
            }

            // Проверка на превышение Max Pressure (только предупреждение, не блокируем)
            if (targetValue > _maxPressure)
            {
                Debug.WriteLine($"TEST: Target value {targetValue} exceeds Max Pressure {_maxPressure}");
            }

            // Проверка на значение ниже Min Pressure (только предупреждение, не блокируем)
            if (targetValue < _minPressure)
            {
                Debug.WriteLine($"TEST: Target value {targetValue} is below Min Pressure {_minPressure}");
            }

            try
            {
                // Если выхлоп открыт (purge режим), закрываем его перед установкой давления
                if (_isExhaust)
                {
                    _serial.Send(DeviceCommands.ControlOn);
                    _isExhaust = false;
                    _lastCurrent = null;
                    _view.UI_SetTrendStatus(_lastCurrent, _current, isExhaust: false, _rampSpeed);
                    _view.UI_AppendStatusInfo("Exhaust closed - returning to control mode");
                }

                _serial.Send(DeviceCommands.SetSetPoint(targetValue));
                _setPoint = targetValue;
                _view.UI_SetSetPoint(_setPoint, _unit);
                _serial.Send(DeviceCommands.ReadAls);

                // Записываем событие изменения давления
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordEvent(_current, _setPoint, _unit, "TARGET_CHANGED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Command failed: AS {targetValue:F1} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Устанавливает скорость рампа (скорость изменения давления).
        /// </summary>
        public void SetRampSpeed(double rampSpeed)
        {
            if (_serial == null)
            {
                throw new InvalidOperationException("Device is not connected.");
            }

            if (rampSpeed < 0)
            {
                throw new ArgumentException("Ramp speed cannot be negative.", nameof(rampSpeed));
            }

            try
            {
                // Команда: ASR value 4 (где 4 = секунды)
                _serial.Send($"ASR {rampSpeed.ToString("G", CultureInfo.InvariantCulture)} 4");
                _rampSpeed = rampSpeed;
                
                // Обновляем отображение
                _view.UI_SetRampSpeedUnits($"{TrimZeros(rampSpeed)} {_unit}/s");
                
                // Отправляем команду для получения подтверждения от устройства
                _serial.Send(DeviceCommands.ReadRampSpeed);

                // Записываем событие изменения скорости рампа
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordEvent(_current, _setPoint, _unit, "RAMP_SPEED_CHANGED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Command failed: SetRampSpeed {rampSpeed:F2} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Устанавливает скорость рампа с подтверждением (аналогично GoToTarget).
        /// </summary>
        public void GoToRampSpeed(string rampSpeedText)
        {
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = rampSpeedText?.Trim() ?? string.Empty;
            if (raw.Length == 0)
            {
                MessageBox.Show("Enter ramp speed value.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double rampSpeed))
            {
                MessageBox.Show("Invalid ramp speed value format.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (rampSpeed < 0)
            {
                MessageBox.Show("Ramp speed cannot be negative.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string displayVal = rampSpeed.ToString("F2", CultureInfo.InvariantCulture);

            var ask = MessageBox.Show(
                $"Do you want to change the ramp speed to {displayVal} {_unit}/s?",
                "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask != DialogResult.Yes) return;

            try
            {
                // Команда: ASR value 4 (где 4 = секунды)
                _serial.Send($"ASR {rampSpeed.ToString("G", CultureInfo.InvariantCulture)} 4");
                _rampSpeed = rampSpeed;
                
                // Обновляем отображение
                _view.UI_SetRampSpeedUnits($"{TrimZeros(rampSpeed)} {_unit}/s");
                
                // Отправляем команду для получения подтверждения от устройства
                _serial.Send(DeviceCommands.ReadRampSpeed);
                
                _view.UI_AppendStatusInfo($"Ramp speed set to {displayVal} {_unit}/s");

                // Записываем событие изменения скорости рампа
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordEvent(_current, _setPoint, _unit, "RAMP_SPEED_CHANGED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
            }
            catch (Exception ex)
            {
                _view.UI_AppendStatusInfo($"Command failed: GoToRampSpeed {rampSpeed:F2} - {ex.Message}");
                MessageBox.Show("Failed to send command:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // INCREMENT CONTROL
        // ====================================================================

        public void IncrementMinus()
        {
            var newValue = _currentIncrement - 0.1;
            
            if (newValue < _minIncrementLimit)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Cannot go below Minimum Step ({_minIncrementLimit.ToString("F1", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentIncrement = newValue;
            _view.IncrementText = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);
            _view.UpdateIncrementButtons();
            _view.UI_AppendStatusInfo($"Increment changed to {_currentIncrement:F1} {_unit}");
        }

        public void IncrementPlus()
        {
            var newValue = _currentIncrement + 0.1;
            
            if (newValue > _maxIncrementLimit)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Cannot exceed Maximum Step ({_maxIncrementLimit.ToString("F1", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentIncrement = newValue;
            _view.IncrementText = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);
            _view.UpdateIncrementButtons();
            _view.UI_AppendStatusInfo($"Increment changed to {_currentIncrement:F1} {_unit}");
        }

        public void UpdateIncrementFromText(string text)
        {
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
            {
                var oldIncrement = _currentIncrement;
                _currentIncrement = Math.Clamp(val, _minIncrementLimit, _maxIncrementLimit);
                _view.UpdateIncrementButtons();
                
                // Логируем только если значение действительно изменилось
                if (Math.Abs(oldIncrement - _currentIncrement) > 0.01)
                {
                    _view.UI_AppendStatusInfo($"Increment changed to {_currentIncrement:F1} {_unit}");
                }
            }
        }

        // ====================================================================
        // OPTIONS
        // ====================================================================

        public void ApplyOptionsToUi()
        {
            // Сохраняем старые значения для сравнения
            var oldUnit = _unit;
            var oldMaxPressure = _maxPressure;
            var oldMinPressure = _minPressure;
            var oldMaxIncrement = _maxIncrementLimit;
            var oldMinIncrement = _minIncrementLimit;

            // Обновляем настройки в Presenter
            _maxPressure = OptionsWindow.AppOptions.Current.MaxPressure ?? 200.0;
            _minPressure = OptionsWindow.AppOptions.Current.MinPressure ?? 0.0;
            _maxIncrementLimit = OptionsWindow.AppOptions.Current.MaxIncrement ?? 20.0;
            _minIncrementLimit = OptionsWindow.AppOptions.Current.MinIncrement ?? 0.1;

            // Update Polling Frequency from Preferences
            var pollingFreq = OptionsWindow.AppOptions.Current.PollingFrequency ?? 500;
            bool wasRunning = _pollTimer.Enabled;
            _pollTimer.Stop();
            _pollTimer.Interval = pollingFreq;
            if (wasRunning)
            {
                _pollTimer.Start();
            }

            // Обновляем значения в View для валидации
            _view.MaxPressure = _maxPressure;
            _view.MaxIncrementLimit = _maxIncrementLimit;

            // Вызываем валидацию и обновление UI
            _view.ValidateTargetAgainstMax();
            _view.ValidateIncrementAgainstMax();
            _view.UpdateIncrementButtons();

            // Проверяем, изменились ли единицы измерения
            var newUnit = OptionsWindow.AppOptions.Current.PressureUnits ?? "PSIG";
            if (newUnit != oldUnit)
            {
                // Отправляем команду ADCU на устройство для изменения единиц
                // НЕ устанавливаем _unit вручную - единицы придут из ответов устройства
                if (_serial != null && _serial.IsConnected)
                {
                    try
                    {
                        var unitCode = GetUnitCodeForADCU(newUnit);
                        Debug.WriteLine($"GetUnitCodeForADCU('{newUnit}') = {unitCode}");
                        if (unitCode.HasValue)
                        {
                            string command = DeviceCommands.SetPressureUnits(unitCode.Value);
                            Debug.WriteLine($"Sending command: {command}");
                            _serial.Send(command);
                            _view.UI_AppendStatusInfo($"Unit changed from {oldUnit} -> {newUnit}");
                        }
                        else
                        {
                            _view.UI_AppendStatusInfo($"Unit change command not sent - unknown unit: '{newUnit}'");
                            Debug.WriteLine($"Unknown unit: '{newUnit}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        _view.UI_AppendStatusInfo($"Failed to send unit change command: {ex.Message}");
                        Debug.WriteLine($"Exception in ADCU: {ex}");
                    }
                }
                else
                {
                    _view.UI_AppendStatusInfo($"Unit change command not sent - device not connected");
                    Debug.WriteLine($"Device not connected: _serial={_serial != null}, IsConnected={_serial?.IsConnected}");
                }
            }
            
            // Единицы всегда обновляются из текущего значения _unit (которое берется из ответов устройства)
            _view.UI_SetPressureUnits(_unit);

            // Обновляем thresholds в ChartWindow, если он открыт
            if (_graphView != null && !_graphView.IsDisposed)
            {
                _graphView.UpdateThresholdsFromSettings();
            }
        }

        public void ShowOptions(Form parentForm)
        {
            using var dlg = new OptionsWindow();
            dlg.StartPosition = FormStartPosition.CenterParent;
            
            // Подписываемся на событие Applied для обновления UI при нажатии Apply
            dlg.Applied += (_, __) =>
            {
                ApplyOptionsToUi();
                _view.ApplyOptionsToUi();

                // Применяем Ramp Speed если задано
                var ramp = OptionsWindow.AppOptions.Current.PressureRamp;
                if (_serial != null && ramp.HasValue && ramp.Value > 0.001)
                {
                    try
                    {
                        SetRampSpeed(ramp.Value);
                        _view.UI_AppendStatusInfo($"Ramp speed set to {ramp.Value:F2} {_unit}/s");
                    }
                    catch (Exception ex)
                    {
                        _view.UI_AppendStatusInfo($"Failed to set ramp speed: {ex.Message}");
                    }
                }
                else if (ramp.HasValue && ramp.Value <= 0.001)
                {
                    // Если ramp speed = 0 или очень маленький, сбрасываем
                    _rampSpeed = 0.0;
                    _view.UI_SetRampSpeedUnits("0 " + _unit + "/s");
                }

                // Сохраняем настройки, если Auto-save включен
                _view.SaveSettingsIfAutoSaveEnabled();
                _view.UI_AppendStatusInfo("Settings applied");
            };

            var result = dlg.ShowDialog(parentForm);

            if (result == DialogResult.OK || result == DialogResult.None)
            {
                ApplyOptionsToUi();
                
                // Также обновляем внутренние поля View (MinPressure, MinIncrementLimit)
                _view.ApplyOptionsToUi();

                // Применяем Ramp Speed если задано
                var ramp = OptionsWindow.AppOptions.Current.PressureRamp;
                if (_serial != null && ramp.HasValue && ramp.Value > 0.001)
                {
                    try
                    {
                        SetRampSpeed(ramp.Value);
                        _view.UI_AppendStatusInfo($"Ramp speed set to {ramp.Value:F2} {_unit}/s");
                    }
                    catch (Exception ex)
                    {
                        _view.UI_AppendStatusInfo($"Failed to set ramp speed: {ex.Message}");
                    }
                }
                else if (ramp.HasValue && ramp.Value <= 0.001)
                {
                    // Если ramp speed = 0 или очень маленький, сбрасываем
                    _rampSpeed = 0.0;
                    _view.UI_SetRampSpeedUnits("0 " + _unit + "/s");
                }

                // Сохраняем настройки, если Auto-save включен
                _view.SaveSettingsIfAutoSaveEnabled();
                _view.UI_AppendStatusInfo("Settings applied");
            }
        }

        // ====================================================================
        // SESSION MANAGEMENT
        // ====================================================================

        public void StartNewSession()
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder for session data";

            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;

            string fileName = $"session_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            string fullPath = System.IO.Path.Combine(folderDialog.SelectedPath, fileName);

            _dataPresenter.StartSession(fullPath);

            MessageBox.Show(
                $"Session started!\n\nSaving to:\n{fullPath}",
                "New Session",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            _view.UI_AppendStatusInfo("New session started");
        }

        // ====================================================================
        // NAVIGATION
        // ====================================================================

        public void ShowGraph(Form parentForm)
        {
            if (_graphView == null || _graphView.IsDisposed)
            {
                // Convert IDataStore to SessionDataStore (it's actually SessionDataStore instance)
                var sessionDataStore = _dataStore as SessionDataStore 
                    ?? throw new InvalidOperationException("DataStore must be SessionDataStore instance");
                var graphForm = new ChartWindow(sessionDataStore);
                _graphView = graphForm;
                _view.GraphView = graphForm;
                
                // Set connection info if device is connected
                if (_serial != null && _serial is SerialClient serialClient)
                {
                    _graphView.SetConnectionInfo(serialClient.PortName, serialClient.BaudRate);
                }
                
                // Set pause handler
                _graphView.SetPauseHandler(() => Pause());
                
                // Set target handler
                _graphView.SetTargetHandler((target) => GoToTarget(target.ToString("F1", CultureInfo.InvariantCulture)));
                // Устанавливаем обработчик для GO TO TARGET секции (без подтверждения)
                _graphView.SetTargetHandlerSilent((target) => SetTargetSilent(target));
                
                // Set emergency vent handler
                _graphView.SetEmergencyVentHandler(async () => await EmergencyStop());
                
                // Передаем SequenceService в ChartWindow для синхронизации
                if (_sequencePresenter.SequenceService != null)
                {
                    _graphView.SetSequenceService(_sequencePresenter.SequenceService);
                }
                
                // Apply theme from main form
                bool isDark = _view.IsDarkTheme;
                _graphView.ApplyTheme(isDark);
                
                // Update pause state
                _graphView.UpdatePauseState(_isPaused);
                
                _graphView.Show(parentForm);
            }
            else
            {
                // Update theme if form is already open
                bool isDark = _view.IsDarkTheme;
                _graphView.ApplyTheme(isDark);
                
                // НЕ загружаем состояние при показе существующей формы
                // Загрузка происходит только один раз в конструкторе
                // Это предотвращает перезапись текущих значений пользователя
                
                if (_graphView.WindowState == FormWindowState.Minimized)
                    _graphView.WindowState = FormWindowState.Normal;
                _graphView.Focus();
            }
            
            // Устанавливаем обработчик для синхронизации thresholds с главной формой
            _graphView.SetThresholdsChangedHandler(() =>
            {
                // Обновляем внутренние значения в Presenter
                _maxPressure = OptionsWindow.AppOptions.Current.MaxPressure ?? 200.0;
                _minPressure = OptionsWindow.AppOptions.Current.MinPressure ?? 0.0;
                
                // Обновляем значения в View
                _view.MaxPressure = _maxPressure;
                
                // Сохраняем настройки в файл
                _view.SaveSettingsIfAutoSaveEnabled();
                
                // Обновляем UI через View (thread-safe)
                _view.BeginInvoke(new Action(() =>
                {
                    _view.ApplyOptionsToUi();
                    _view.ValidateTargetAgainstMax();
                    _view.ValidateIncrementAgainstMax();
                }));
            });
            
            // Синхронизируем thresholds с текущими настройками при открытии
            _graphView.RefreshThresholdsFromSettings();
        }

        public void ShowTable(Form parentForm)
        {
            if (_tableView == null || _tableView.IsDisposed)
            {
                // Convert IDataStore to SessionDataStore (it's actually SessionDataStore instance)
                var sessionDataStore = _dataStore as SessionDataStore 
                    ?? throw new InvalidOperationException("DataStore must be SessionDataStore instance");
                var tableForm = new DataTableWindow(sessionDataStore);
                _tableView = tableForm;
                _view.TableView = tableForm;
                _tableView.StartPosition = FormStartPosition.CenterParent;
                
                // Set connection info if device is connected
                if (_serial != null && _serial is SerialClient serialClient)
                {
                    _tableView.SetConnectionInfo(serialClient.PortName, serialClient.BaudRate);
                }
                
                // Синхронизируем тему с главной формой
                _tableView.ApplyTheme(_view.IsDarkTheme);
                _tableView.Show(parentForm);
            }
            else
            {
                if (_tableView.WindowState == FormWindowState.Minimized)
                    _tableView.WindowState = FormWindowState.Normal;
                _tableView.Focus();
            }
        }

        public void ShowTestPressure(Form parentForm)
        {
            if (_testPressureForm == null || _testPressureForm.IsDisposed)
            {
                _testPressureForm = new PrecisionPressureController.UI.Features.Test.TestPressureWindow(this);
                _testPressureForm.FormClosed += (s, e) => _testPressureForm = null;
            }

            _testPressureForm.Show();
            _testPressureForm.BringToFront();
        }

        public void ShowTerminal(Form parentForm)
        {
            if (_terminalView == null || _terminalView.IsDisposed)
            {
                var terminalForm = new TerminalWindow();
                terminalForm.CommandSent += TerminalWindow_CommandSent;
                _terminalView = terminalForm;
                _view.TerminalView = terminalForm;
            }

            if (!_terminalView.Visible)
            {
                _terminalView.Show(parentForm);
            }
            _terminalView.Focus();
        }

        private void TerminalWindow_CommandSent(string cmd)
        {
            if (_serial == null)
            {
                _terminalView?.AppendLog("!! Serial not connected");
                return;
            }

            try
            {
                _serial.Send(cmd);
            }
            catch (Exception ex)
            {
                _terminalView?.AppendLog("!! Error: " + ex.Message);
            }
        }

        // ====================================================================
        // DEVICE MANAGEMENT
        // ====================================================================

        /// <summary>
        /// Отключение устройства напрямую (без диалога).
        /// </summary>
        public void DisconnectDevice()
        {
            System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Called. _serial is null: {_serial == null}");
            
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Disconnect",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Starting disconnect...");
                
                // Останавливаем таймер опроса
                if (_pollTimer != null)
                {
                    _pollTimer.Stop();
                    System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Poll timer stopped.");
                }
                
                // Останавливаем watchdog таймер
                StopWatchdogTimer();
                
                // Сбрасываем флаг ожидания ответа
                _isWaitingForResponse = false;
                
                // Закрываем соединение
                if (_serial != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Disposing serial connection...");
                    _serial.Dispose();
                    _serial = null;
                    System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Serial connection disposed.");
                }
                
                _ramp = null;

                // Обновляем UI
                _view.UI_UpdateConnectionStatus(false);
                _view.UI_AppendStatusInfo("Device disconnected");
                
                // Update ChartWindow connection info if open
                if (_graphView != null && !_graphView.IsDisposed)
                {
                    _graphView.SetConnectionInfo(null, null);
                }
                
                // Update DataTableWindow connection info if open
                if (_tableView != null && !_tableView.IsDisposed)
                {
                    _tableView.SetConnectionInfo(null, null);
                }
                
                System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Disconnect completed successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[DisconnectDevice] Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error disconnecting device:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Emergency Stop - немедленная остановка и сброс давления до нуля.
        /// </summary>
        public Task EmergencyStop()
        {
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Emergency Stop",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Task.CompletedTask;
            }

            try
            {
                // Открываем выхлоп и оставляем открытым (как в Purge)
                _serial.Send(DeviceCommands.ExhaustHold);
                _isExhaust = true;

                _view.UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true, _rampSpeed);
                _view.UI_AppendStatusInfo("Emergency vent started - exhaust open");

                // Устанавливаем setpoint на устройстве в 0.0
                _serial.Send(DeviceCommands.SetSetPoint(0.0));
                _setPoint = 0.0;
                _view.UI_SetSetPoint(_setPoint, _unit);

                _serial.Send(DeviceCommands.ReadAls);

                // Записываем событие Emergency Vent
                int pointIndex = GetCurrentPointIndex();
                _dataPresenter.RecordEvent(_current, _setPoint, _unit, "EMERGENCY_VENT_STARTED", _rampSpeed, (int)_pollTimer.Interval, pointIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Emergency Stop error:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Task.CompletedTask;
        }

        // ====================================================================
        // DEVICE INFORMATION
        // ====================================================================

        /// <summary>
        /// Получение и отображение информации об устройстве.
        /// </summary>
        public async void ShowDeviceInfo(Form parentForm)
        {
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.\n\nPlease connect to a device first.", 
                    "Device Info", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Собираем информацию об устройстве асинхронно
                string deviceInfo = await GetDeviceInfoAsync();

                // Показываем информацию в MessageBox
                MessageBox.Show(deviceInfo, "Device Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to retrieve device information:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Асинхронное получение информации об устройстве через RS-232.
        /// </summary>
        private async Task<string> GetDeviceInfoAsync()
        {
            if (_serial == null)
                return "Device not connected";

            var info = new System.Text.StringBuilder();
            info.AppendLine("Precision Pressure Controller");
            info.AppendLine("═══════════════════════════════════");
            info.AppendLine();

            // Получаем информацию из текущего состояния
            info.AppendLine("Current Status:");
            info.AppendLine("─────────────────────────");
            info.AppendLine($"Connection Status: Connected");
            info.AppendLine($"Current Pressure: {_current:F2} {_unit}");
            info.AppendLine($"Set Point: {_setPoint:F2} {_unit}");
            info.AppendLine($"Ramp Speed: {_rampSpeed:F2} {_unit}/s");
            info.AppendLine($"Units: {_unit}");
            info.AppendLine($"Exhaust: {(_isExhaust ? "Open" : "Closed")}");
            info.AppendLine($"Paused: {(_isPaused ? "Yes" : "No")}");
            info.AppendLine();

            // Получаем детальную информацию об устройстве
            info.AppendLine("Device Information:");
            info.AppendLine("─────────────────────────");

            try
            {
                // Запрашиваем информацию об устройстве через команду AVE
                // Формат ответа: "A 10v22.0-R24 Apr 29 2025,10:58:06"
                string? deviceInfoResponse = await RequestDeviceResponseAsync(DeviceCommands.GetDeviceInfo, 1500);
                if (!string.IsNullOrWhiteSpace(deviceInfoResponse))
                {
                    // Парсим ответ AVE
                    // Формат: "A <model/version> <date> <time>"
                    string response = deviceInfoResponse.Trim();
                    
                    // Убираем префикс "A " если есть
                    if (response.StartsWith("A "))
                    {
                        response = response.Substring(2).Trim();
                    }
                    else if (response.StartsWith("A"))
                    {
                        response = response.Substring(1).Trim();
                    }
                    
                    // Разбиваем на части
                    var parts = response.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length >= 1)
                    {
                        // Первая часть - модель/версия (например, "10v22.0-R24")
                        string modelVersion = parts[0];
                        info.AppendLine($"Model / Version: {modelVersion}");
                    }
                    
                    if (parts.Length >= 2)
                    {
                        // Остальные части - дата и время
                        string dateTime = string.Join(" ", parts, 1, parts.Length - 1);
                        info.AppendLine($"Firmware Date: {dateTime}");
                    }
                }
                else
                {
                    info.AppendLine("Model / Version: Not available");
                    info.AppendLine("Firmware Date: Not available");
                }
            }
            catch (Exception ex)
            {
                info.AppendLine($"Error retrieving device info: {ex.Message}");
            }

            info.AppendLine();
            info.AppendLine($"Last Update: {_state.UpdatedAtUtc:yyyy-MM-dd HH:mm:ss} UTC");

            return info.ToString();
        }

        /// <summary>
        /// Отправляет команду устройству и ждет ответа с таймаутом.
        /// ВАЖНО: Временно останавливает polling, чтобы избежать конфликтов.
        /// </summary>
        private async Task<string?> RequestDeviceResponseAsync(string command, int timeoutMs)
        {
            if (_serial == null)
                return null;

            // Временно останавливаем polling
            bool wasPolling = _pollTimer.Enabled;
            _pollTimer.Stop();

            try
            {
                string? response = null;
                var responseReceived = new System.Threading.Tasks.TaskCompletionSource<string?>();
                var timeout = false;

                // Подписываемся на получение ответа
                EventHandler<string>? handler = null;
                handler = (sender, line) =>
                {
                    if (!timeout && !string.IsNullOrWhiteSpace(line))
                    {
                        string trimmedLine = line.Trim();
                        
                        // Для команды AVE принимаем ответ, который содержит версию/дату
                        // Формат AVE ответа: "A 10v22.0-R24 Apr 29 2025,10:58:06"
                        // ALS ответы: "A +0053.9 +0053.9 10 PSIG" (содержат два числа давления подряд)
                        
                        // Проверяем, является ли это ALS ответом (содержит два числа давления)
                        // Паттерн: "A" + пробел + число + пробел + число
                        bool isAlsResponse = trimmedLine.StartsWith("A") && 
                                            System.Text.RegularExpressions.Regex.IsMatch(trimmedLine, @"^A\s+[+-]?\d+\.\d+\s+[+-]?\d+\.\d+");
                        
                        // Если это не ALS ответ (не содержит два числа давления подряд), принимаем его
                        // Это может быть ответ AVE или другой информационный ответ
                        if (!isAlsResponse)
                        {
                            response = line;
                            responseReceived.TrySetResult(line);
                        }
                    }
                };

                _serial.LineReceived += handler;

                try
                {
                    // Очищаем буфер перед отправкой команды
                    await Task.Delay(50); // Небольшая задержка для очистки буфера

                    // Отправляем команду
                    _serial.Send(command);

                    // Ждем ответа с таймаутом
                    var timeoutTask = Task.Delay(timeoutMs);
                    var completedTask = await Task.WhenAny(responseReceived.Task, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        timeout = true;
                        responseReceived.TrySetResult(null);
                        return null;
                    }

                    return await responseReceived.Task;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    // Отписываемся от события
                    if (handler != null)
                    {
                        _serial.LineReceived -= handler;
                    }
                }
            }
            finally
            {
                // Восстанавливаем polling, если он был активен
                if (wasPolling)
                {
                    _pollTimer.Start();
                }
            }
        }

        // ====================================================================
        // HELPERS
        // ====================================================================

        // ====================================================================
        // CLEANUP
        // ====================================================================

        public void Dispose()
        {
            _pollTimer.Stop();
            _dataStore.EndSession();
            _serial?.Dispose();
        }

        // ====================================================================
        // PROPERTIES FOR VIEW
        // ====================================================================

        public double Current => _current;
        public double SetPoint => _setPoint;
        public string Unit => _unit;
        public double RampSpeed => _rampSpeed;
        public double MaxPressure => _maxPressure;
        public double CurrentIncrement => _currentIncrement;

        /// <summary>
        /// Запрашивает актуальные значения у устройства (давление и скорость рампа).
        /// </summary>
        public void RequestCurrentValues()
        {
            if (_serial == null || !_serial.IsConnected)
            {
                return;
            }

            try
            {
                // Запрашиваем текущее давление (ALS)
                _serial.Send(DeviceCommands.ReadAls);
                // Запрашиваем текущую скорость рампа (ASR)
                _serial.Send(DeviceCommands.ReadRampSpeed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RequestCurrentValues error: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает код единицы измерения для команды ADCU.
        /// Коды единиц согласно документации устройства (Appendix B-6):
        /// 2: Pa
        /// 3: hPa
        /// 4: kPa
        /// 5: MPa
        /// 6: mbar
        /// 7: bar
        /// 8: g/cm²
        /// 9: kg/cm
        /// 10: PSI
        /// 11: PSF
        /// 12: mTorr
        /// 13: torr
        /// </summary>
        private int? GetUnitCodeForADCU(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return 10; // Default: PSI

            var upper = unit.ToUpperInvariant().Trim();

            // Убираем "G" в конце для сопоставления
            if (upper.EndsWith("G") && upper != "PSIG" && upper != "PSFG")
            {
                upper = upper.Substring(0, upper.Length - 1);
            }

            var code = upper switch
            {
                "PA" => 2,
                "HPA" => 3,
                "KPA" => 4,
                "MPA" => 5,
                "MBAR" => 6,
                "BAR" => 7,
                "G/CM²" or "G/CM2" or "GCM²" or "GCM2" => 8,
                "KG/CM" or "KGCM" => 9,
                "PSI" or "PSIG" => 10,
                "PSF" or "PSFG" => 11,
                "MTORR" => 12,
                "TORR" => 13,
                _ => (int?)null // Unknown unit
            };
            
            Debug.WriteLine($"GetUnitCodeForADCU: unit='{unit}' -> upper='{upper}' -> code={code}");
            return code;
        }
        public double MaxIncrementLimit => _maxIncrementLimit;

        /// <summary>
        /// Обновляет текст "Last update" на основе интервала таймера опроса.
        /// </summary>
        private void UpdateLastUpdateText()
        {
            int intervalMs = (int)_pollTimer.Interval;
            string text;
            
            if (intervalMs < 1000)
            {
                // Меньше секунды - показываем в миллисекундах с 2 знаками после запятой
                double seconds = intervalMs / 1000.0;
                text = $"Last update: {seconds.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}s ago";
            }
            else if (intervalMs < 60000)
            {
                // Меньше минуты - показываем в секундах
                double seconds = intervalMs / 1000.0;
                text = $"Last update: {TrimZeros(seconds, 0)}s ago";
            }
            else
            {
                // Больше минуты - показываем в минутах
                double minutes = intervalMs / 60000.0;
                text = $"Last update: {TrimZeros(minutes, 1)}m ago";
            }
            
            _view.UI_UpdateLastUpdate(text);
        }

        /// <summary>
        /// Запускает сторожевой таймер для контроля связи с устройством.
        /// </summary>
        private void StartWatchdogTimer()
        {
            StopWatchdogTimer(); // Останавливаем предыдущий, если был
            
            _watchdogTimer = new System.Threading.Timer(
                WatchdogTimerCallback,
                null,
                ConnectionTimeoutMs,
                System.Threading.Timeout.Infinite // Одноразовый таймер
            );
        }

        /// <summary>
        /// Сбрасывает сторожевой таймер (перезапускает отсчет).
        /// Вызывается при получении данных от устройства.
        /// </summary>
        private void ResetWatchdogTimer()
        {
            if (_watchdogTimer != null && _serial != null && _serial.IsConnected)
            {
                // Перезапускаем таймер на новый период
                _watchdogTimer.Change(ConnectionTimeoutMs, System.Threading.Timeout.Infinite);
            }
        }

        /// <summary>
        /// Останавливает сторожевой таймер.
        /// </summary>
        private void StopWatchdogTimer()
        {
            if (_watchdogTimer != null)
            {
                _watchdogTimer.Dispose();
                _watchdogTimer = null;
            }
        }

        /// <summary>
        /// Обработчик срабатывания сторожевого таймера.
        /// Вызывается, если устройство не отвечает в течение таймаута.
        /// </summary>
        private void WatchdogTimerCallback(object? state)
        {
            if (_serial == null || !_serial.IsConnected)
            {
                StopWatchdogTimer();
                return;
            }

            // Логируем срабатывание watchdog для диагностики
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Watchdog timeout - device not responding for {ConnectionTimeoutMs}ms");
            
            // Таймер сработал - значит нет данных больше таймаута
            _view.BeginInvoke(new Action(() =>
            {
                string timeoutMsg = $"Connection timeout ({ConnectionTimeoutMs / 1000}s) - device not responding";
                _view.UI_AppendStatusInfo(timeoutMsg);
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {timeoutMsg}");
                _view.UI_UpdateConnectionStatus(false);
                
                // Останавливаем таймеры
                _pollTimer.Stop();
                StopWatchdogTimer();
                
                // Закрываем соединение
                try
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Disposing serial connection due to timeout");
                    _serial?.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Error disposing serial connection: {ex.Message}");
                }
                
                _serial = null;
                _ramp = null;
                _isWaitingForResponse = false;
            }));
        }

        private static string GetSettingsFilePath()
        {
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

        private void SaveCommunicationSettings(ConnectionDialog dlg, SerialPort opened)
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                string? directory = System.IO.Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                var existingSettings = new
                {
                    General = new { PressureUnits = "PSI", TimeUnits = "s", PollingFrequency = 500 },
                    Device = new { PressureRamp = (double?)null, MaxPressure = (double?)null, MinPressure = (double?)null },
                    Control = new { MaxIncrement = (double?)null, MinIncrement = (double?)null },
                    Communication = new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 },
                    LastSaved = DateTime.Now
                };

                if (System.IO.File.Exists(settingsPath))
                {
                    try
                    {
                        string jsonContent = System.IO.File.ReadAllText(settingsPath);
                        var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(jsonContent);
                        
                        if (settingsData.TryGetProperty("General", out var gen))
                        {
                            existingSettings = new
                            {
                                General = new
                                {
                                    PressureUnits = gen.TryGetProperty("PressureUnits", out var pu) ? pu.GetString() ?? "PSI" : "PSI",
                                    TimeUnits = gen.TryGetProperty("TimeUnits", out var tu) ? tu.GetString() ?? "s" : "s",
                                    PollingFrequency = gen.TryGetProperty("PollingFrequency", out var pf) && pf.ValueKind != System.Text.Json.JsonValueKind.Null ? pf.GetInt32() : 500
                                },
                                Device = settingsData.TryGetProperty("Device", out var dev) ? new
                                {
                                    PressureRamp = dev.TryGetProperty("PressureRamp", out var pr) && pr.ValueKind != System.Text.Json.JsonValueKind.Null ? pr.GetDouble() : (double?)null,
                                    MaxPressure = dev.TryGetProperty("MaxPressure", out var mp) && mp.ValueKind != System.Text.Json.JsonValueKind.Null ? mp.GetDouble() : (double?)null,
                                    MinPressure = dev.TryGetProperty("MinPressure", out var minp) && minp.ValueKind != System.Text.Json.JsonValueKind.Null ? minp.GetDouble() : (double?)null
                                } : new { PressureRamp = (double?)null, MaxPressure = (double?)null, MinPressure = (double?)null },
                                Control = settingsData.TryGetProperty("Control", out var ctrl) ? new
                                {
                                    MaxIncrement = ctrl.TryGetProperty("MaxIncrement", out var mi) && mi.ValueKind != System.Text.Json.JsonValueKind.Null ? mi.GetDouble() : (double?)null,
                                    MinIncrement = ctrl.TryGetProperty("MinIncrement", out var mini) && mini.ValueKind != System.Text.Json.JsonValueKind.Null ? mini.GetDouble() : (double?)null
                                } : new { MaxIncrement = (double?)null, MinIncrement = (double?)null },
                                Communication = new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 },
                                LastSaved = DateTime.Now
                            };
                        }
                    }
                    catch { }
                }

                var updatedSettings = new
                {
                    existingSettings.General,
                    existingSettings.Device,
                    existingSettings.Control,
                    Communication = new
                    {
                        PortName = dlg.PortName,
                        BaudRate = dlg.BaudRate,
                        Parity = dlg.Parity,
                        StopBits = dlg.StopBits,
                        DataBits = dlg.DataBits,
                        ReadTimeout = dlg.ReadTimeout,
                        WriteTimeout = dlg.WriteTimeout
                    },
                    LastSaved = DateTime.Now
                };

                string json = System.Text.Json.JsonSerializer.Serialize(updatedSettings, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                System.IO.File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save communication settings: {ex.Message}");
            }
        }
    }
}

