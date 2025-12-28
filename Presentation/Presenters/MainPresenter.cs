using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alicat.Business.Interfaces;
using Alicat.Domain;
using Alicat.Services.Controllers;
using Alicat.Services.Data;
using Alicat.Services.Protocol;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;
using Alicat.UI.Features.Terminal.Views;
using Timer = System.Windows.Forms.Timer;

namespace Alicat.Presentation.Presenters
{
    /// <summary>
    /// Presenter для MainForm - содержит всю бизнес-логику.
    /// View (AlicatForm) только отображает данные и вызывает методы Presenter.
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
        private double? _lastLoggedPressure = null;

        // Services
        private ISerialClient? _serial;
        private IRampController? _ramp;
        private readonly Timer _pollTimer = new() { Interval = 500 };

        // Settings
        private double _maxPressure = 200.0;
        private double _maxIncrementLimit = 20.0;
        private double _currentIncrement = 5.0;

        // State
        private readonly DeviceState _state = new();

        // Child forms
        private TerminalForm? _terminalForm;
        private GraphForm? _graphForm;
        private TableForm? _tableForm;

        public MainPresenter(IMainView view, IDataStore dataStore)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

            // Setup polling timer
            _pollTimer.Tick += (_, __) =>
            {
                if (_serial != null)
                {
                    _serial.Send(AlicatCommands.ReadAls);
                }
            };
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
        }

        // ====================================================================
        // CONNECTION
        // ====================================================================

        /// <summary>
        /// Подключение к устройству через диалог выбора порта.
        /// </summary>
        public void ConnectDevice(Form parentForm)
        {
            using var dlg = new FormConnect { StartPosition = FormStartPosition.CenterParent };
            dlg.ShowDialog(parentForm);

            var opened = dlg.OpenPort;
            if (opened is null) return;

            _serial?.Dispose();
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected += (_, __) => _view.BeginInvoke(new Action(() =>
            {
                _pollTimer.Start();
                _view.UI_UpdateConnectionStatus(true, opened.PortName);
            }));
            _serial.Disconnected += (_, __) => _view.BeginInvoke(new Action(() =>
            {
                _pollTimer.Stop();
                _view.UI_UpdateConnectionStatus(false);
            }));

            _serial.Attach();
            _ramp = new RampController(_serial);
            _serial.Send("ASR");

            if (!_dataStore.IsRunning)
            {
                _dataStore.StartSession();
            }

            _view.UI_UpdateConnectionStatus(true, opened.PortName);
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
            if (_terminalForm != null && !_terminalForm.IsDisposed)
            {
                _terminalForm.AppendLog("<< " + line);
            }

            // 1) Try to parse ASR (Ramp Speed)
            if (TryParseAsr(line, out var ramp, out var rampUnits))
            {
                _rampSpeed = ramp;
                _view.BeginInvoke(new Action(() =>
                {
                    _view.UI_SetRampSpeedUnits($"{TrimZeros(ramp)} {rampUnits}");
                }));
                return;
            }

            // 2) Try to parse ALS
            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;

            _current = cur;
            if (!_isExhaust) _setPoint = sp;
            if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

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

                // Record to store
                _dataStore.RecordSample(_current, _isExhaust ? 0.0 : _setPoint, _unit);

                // Update graph if open
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    double? targetForGraph = _isExhaust ? (double?)null : _setPoint;
                    _graphForm.AddSample(_current, targetForGraph);
                }

                // Update table if open
                if (_tableForm != null && !_tableForm.IsDisposed)
                {
                    if (ShouldLog(_current))
                    {
                        var spForLog = _isExhaust ? 0.0 : _setPoint;
                        _tableForm.AddRecordFromDevice(_current, spForLog, _unit);
                    }
                }
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

            if (targetValue > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(
                    $"Target value exceeds Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} {_unit}).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var ask = MessageBox.Show(
                $"Do you want to change the target value to {displayVal} {unit}?",
                "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask != DialogResult.Yes) return;

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
                _view.UI_SetSetPoint(_setPoint, _unit);
                _serial.Send(AlicatCommands.ReadAls);

                _view.TargetInputText = "";

                _view.UI_AppendStatusInfo($"Target set to {displayVal} {unit}");
            }
            catch (Exception ex)
            {
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
                _serial.Send($"AS{_current:F2}");
                _setPoint = _current;
                _view.UI_SetSetPoint(_current, _unit);

                _isPaused = true;
                _view.UI_AppendStatusInfo($"Ramp paused - setpoint set to current ({_current:F2} {_unit}). Polling continues.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to pause: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task Purge()
        {
            if (_serial is null)
            {
                MessageBox.Show("No connection to device.", "Purge",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ask = MessageBox.Show("Open exhaust and hold?",
                                      "Confirm purge",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask != DialogResult.Yes) return;

            try
            {
                _serial.Send("AE");
                _isExhaust = true;

                _view.UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true, _rampSpeed);
                _view.UI_AppendStatusInfo("Purge started");

                _setPoint = 0.0;
                _view.UI_SetSetPoint(_setPoint, _unit);

                await Task.Delay(400);
                _serial.Send("AC");
                _serial.Send(AlicatCommands.ReadAls);

                _view.UI_AppendStatusInfo("Purge complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Purge error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }

        public void Decrease()
        {
            var next = Math.Max(0, _setPoint - _currentIncrement);
            SendSetPoint(next);
        }

        private void SendSetPoint(double sp)
        {
            _setPoint = sp;
            _view.UI_SetSetPoint(_setPoint, _unit);

            if (_serial is null) return;

            if (sp > 0.05)
            {
                _serial.Send("AC");
                _isExhaust = false;
                _lastCurrent = null;
            }

            _serial.Send($"AS {sp.ToString("F2", CultureInfo.InvariantCulture)}");
            _serial.Send(AlicatCommands.ReadAls);
        }

        // ====================================================================
        // INCREMENT CONTROL
        // ====================================================================

        public void IncrementMinus()
        {
            _currentIncrement = Math.Max(0.1, _currentIncrement - 0.1);
            _view.IncrementText = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);
            _view.UpdateIncrementButtons();
        }

        public void IncrementPlus()
        {
            _currentIncrement = Math.Min(_maxIncrementLimit, _currentIncrement + 0.1);
            _view.IncrementText = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);
            _view.UpdateIncrementButtons();
        }

        public void UpdateIncrementFromText(string text)
        {
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
            {
                _currentIncrement = Math.Clamp(val, 0.1, _maxIncrementLimit);
                _view.UpdateIncrementButtons();
            }
        }

        // ====================================================================
        // OPTIONS
        // ====================================================================

        public void ApplyOptionsToUi()
        {
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _maxIncrementLimit = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;

            _view.ValidateTargetAgainstMax();
            _view.ValidateIncrementAgainstMax();
            _view.UpdateIncrementButtons();

            _view.UI_SetPressureUnits(_unit);
        }

        public void ShowOptions(Form parentForm)
        {
            using var dlg = new FormOptions();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(parentForm);

            ApplyOptionsToUi();

            var ramp = FormOptions.AppOptions.Current.PressureRamp;
            _ramp?.TryApply(ramp);

            if (_serial != null && ramp is double r)
            {
                _serial.Send($"SR {r.ToString("G", CultureInfo.InvariantCulture)}");
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

            _dataStore.StartSession(fullPath);

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
            if (_graphForm == null || _graphForm.IsDisposed)
            {
                _graphForm = new GraphForm(_dataStore);
                _graphForm.Show(parentForm);
            }
            else
            {
                if (_graphForm.WindowState == FormWindowState.Minimized)
                    _graphForm.WindowState = FormWindowState.Normal;
                _graphForm.Focus();
            }
        }

        public void ShowTable(Form parentForm)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
            {
                _tableForm = new TableForm(_dataStore);
                _tableForm.StartPosition = FormStartPosition.CenterParent;
                _tableForm.Show(parentForm);
            }
            else
            {
                if (_tableForm.WindowState == FormWindowState.Minimized)
                    _tableForm.WindowState = FormWindowState.Normal;
                _tableForm.Focus();
            }
        }

        public void ShowTerminal(Form parentForm)
        {
            if (_terminalForm == null || _terminalForm.IsDisposed)
            {
                _terminalForm = new TerminalForm();
                _terminalForm.CommandSent += TerminalForm_CommandSent;
            }

            if (!_terminalForm.Visible)
            {
                _terminalForm.Show(parentForm);
            }
            _terminalForm.Focus();
        }

        private void TerminalForm_CommandSent(string cmd)
        {
            if (_serial == null)
            {
                _terminalForm?.AppendLog("!! Serial not connected");
                return;
            }

            try
            {
                _serial.Send(cmd);
            }
            catch (Exception ex)
            {
                _terminalForm?.AppendLog("!! Error: " + ex.Message);
            }
        }

        // ====================================================================
        // HELPERS
        // ====================================================================

        private bool ShouldLog(double currentPressure)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
                return false;

            double threshold = _tableForm.Threshold;

            if (_lastLoggedPressure == null)
            {
                _lastLoggedPressure = currentPressure;
                return true;
            }

            double delta = Math.Abs(currentPressure - _lastLoggedPressure.Value);

            if (delta >= threshold)
            {
                _lastLoggedPressure = currentPressure;
                return true;
            }

            return false;
        }

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
        public double MaxIncrementLimit => _maxIncrementLimit;
    }
}

