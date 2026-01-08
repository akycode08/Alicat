using System;
using System.Diagnostics;
using System.Windows.Forms;
using Alicat.Presentation.Presenters;
using Alicat.Services.Data;
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;
using Alicat.UI.Features.Test;

namespace Alicat
{
    /// <summary>
    /// Partial class AlicatForm: интеграция с MainPresenter.
    /// Этот файл содержит связь между View (AlicatForm) и Presenter (MainPresenter).
    /// </summary>
    public partial class AlicatForm
    {
        private MainPresenter? _presenter;
        private readonly SessionDataStore _dataStore = new();

        // Property to access _dataStore from other partial class files
        internal SessionDataStore DataStore => _dataStore;

        // ====================================================================
        // IMainView Implementation - Properties
        // ====================================================================

        string IMainView.TargetInputText
        {
            get => txtTargetInput.Text;
            set => txtTargetInput.Text = value;
        }

        string IMainView.IncrementText
        {
            get => txtIncrement.Text;
            set => txtIncrement.Text = value;
        }

        double IMainView.CurrentIncrement
        {
            get => _currentIncrement;
            set => _currentIncrement = value;
        }

        double IMainView.MaxPressure
        {
            get => _maxPressure;
            set => _maxPressure = value;
        }

        double IMainView.MaxIncrementLimit
        {
            get => _maxIncrementLimit;
            set => _maxIncrementLimit = value;
        }

        double IMainView.Current
        {
            get => _current;
            set => _current = value;
        }

        double IMainView.SetPoint
        {
            get => _setPoint;
            set => _setPoint = value;
        }

        string IMainView.Unit
        {
            get => _unit;
            set => _unit = value;
        }

        double IMainView.RampSpeed
        {
            get => _rampSpeed;
            set => _rampSpeed = value;
        }

        TerminalForm? IMainView.TerminalForm
        {
            get => _terminalForm;
            set => _terminalForm = value;
        }

        GraphForm? IMainView.GraphForm
        {
            get => _graphForm;
            set => _graphForm = value;
        }

        TableForm? IMainView.TableForm
        {
            get => _tableForm;
            set => _tableForm = value;
        }

        // ====================================================================
        // IMainView Implementation - Methods
        // ====================================================================

        void IMainView.BeginInvoke(Action action)
        {
            if (InvokeRequired)
                base.BeginInvoke(action);
            else
                action();
        }

        // ====================================================================
        // Presenter Initialization
        // ====================================================================

        private void InitializePresenter()
        {
            _presenter = new MainPresenter(this, _dataStore);
            _presenter.Initialize();
        }

        // ====================================================================
        // Event Handlers - делегирование в Presenter
        // ====================================================================

        private void btnCommunication_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ConnectDevice(this);
        }

        private void btnGoTarget_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.GoToTarget(txtTargetInput.Text);
        }

        private void btnPause_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.Pause();
        }

        private async void btnPurge_Click_Presenter(object? sender, EventArgs e)
        {
            if (_presenter != null)
                await _presenter.Purge();
        }

        private void btnIncrease_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.Increase();
        }

        private void btnDecrease_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.Decrease();
        }

        private void btnIncrementMinus_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.IncrementMinus();
        }

        private void btnIncrementPlus_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.IncrementPlus();
        }

        private void txtIncrement_TextChanged_Presenter(object? sender, EventArgs e)
        {
            _presenter?.UpdateIncrementFromText(txtIncrement.Text);
        }

        private void btnOptions_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowOptions(this);
        }

        private void menuFileNewSession_Click_Presenter(object? sender, EventArgs e)
        {
            // Отключаем read-only режим перед началом новой сессии
            if (_isReadOnlyMode)
            {
                // Очищаем текущую сессию
                _currentSession = null;
                _currentSessionFilePath = null;
                
                // Отключаем read-only режим
                UpdateReadOnlyMode(false);
            }
            
            _presenter?.StartNewSession();
        }

        private void btnGraph_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowGraph(this);
        }

        private void btnTable_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowTable(this);
        }

        private void btnTerminal_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowTerminal(this);
        }

        // ====================================================================
        // Device Menu Handlers
        // ====================================================================

        private void menuDeviceQuickConnect_Click(object? sender, EventArgs e)
        {
            QuickConnect();
        }

        private void menuDeviceDisconnect_Click(object? sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[menuDeviceDisconnect_Click] Called. _presenter is null: {_presenter == null}");
            if (_presenter != null)
            {
                _presenter.DisconnectDevice();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[menuDeviceDisconnect_Click] _presenter is null, cannot disconnect.");
                MessageBox.Show("Presenter is not initialized.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void menuDeviceEmergencyStop_Click(object? sender, EventArgs e)
        {
            if (_presenter != null)
                await _presenter.EmergencyStop();
        }

        // ====================================================================
        // File Menu Handlers
        // ====================================================================

        private void menuFileExit_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to exit?\n\nAny unsaved data will be lost.",
                "Exit Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Очищаем данные последовательности при закрытии программы
                _presenter?.ClearSequenceOnExit();
                
                Application.Exit();
            }
        }

        // NOTE: menuFileTestPressure removed from File menu
        // Test Pressure functionality can be accessed via other means if needed

        // ====================================================================
        // Settings Menu Handlers
        // ====================================================================

        // ====================================================================
        // Settings Persistence
        // ====================================================================

        private static string GetSettingsFilePath()
        {
            // Используем папку проекта для хранения настроек
            // Находим папку проекта через поиск .csproj файла
            string? projectDir = null;
            
            // Пробуем найти папку проекта через поиск .csproj файла
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
            
            // Если не нашли через поиск, используем папку где находится exe и поднимаемся до проекта
            if (string.IsNullOrEmpty(projectDir))
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string? exeDir = System.IO.Path.GetDirectoryName(exePath);
                
                if (exeDir != null && exeDir.Contains("bin"))
                {
                    var dir = new System.IO.DirectoryInfo(exeDir);
                    while (dir != null && dir.Name != "Alicat" && dir.GetFiles("*.csproj").Length == 0)
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

        // Метод доступен из других partial class файлов (AlicatForm.Commands.cs)
        private void SaveSettingsToFile()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] Starting save...");
                string settingsPath = GetSettingsFilePath();
                System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] Settings path: {settingsPath}");
                
                string? directory = System.IO.Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                    System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] Created directory: {directory}");
                }

                // Читаем RecentSessions из существующего файла, если он есть
                string[]? recentSessions = null;
                if (System.IO.File.Exists(settingsPath))
                {
                    try
                    {
                        string existingJson = System.IO.File.ReadAllText(settingsPath);
                        var existingData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(existingJson);
                        if (existingData.TryGetProperty("RecentSessions", out var recentSessionsProp))
                        {
                            recentSessions = System.Text.Json.JsonSerializer.Deserialize<string[]>(recentSessionsProp.GetRawText());
                        }
                    }
                    catch
                    {
                        // Если не удалось прочитать RecentSessions, продолжаем без них
                    }
                }

                var settingsData = new
                {
                    General = new
                    {
                        PressureUnits = FormOptions.AppOptions.Current.PressureUnits,
                        TimeUnits = FormOptions.AppOptions.Current.TimeUnits,
                        PollingFrequency = FormOptions.AppOptions.Current.PollingFrequency
                    },
                    Device = new
                    {
                        PressureRamp = FormOptions.AppOptions.Current.PressureRamp,
                        MaxPressure = FormOptions.AppOptions.Current.MaxPressure,
                        MinPressure = FormOptions.AppOptions.Current.MinPressure
                    },
                    Control = new
                    {
                        MaxIncrement = FormOptions.AppOptions.Current.MaxIncrement,
                        MinIncrement = FormOptions.AppOptions.Current.MinIncrement
                    },
                    Communication = LoadCommunicationSettingsFromFile(),
                    AutoConnect = new
                    {
                        AutoConnectOnStartup = FormOptions.AppOptions.Current.AutoConnectOnStartup
                    },
                    RecentSessions = recentSessions ?? Array.Empty<string>(),
                    LastSaved = DateTime.Now
                };

                System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] AutoConnectOnStartup = {FormOptions.AppOptions.Current.AutoConnectOnStartup}");

                string json = System.Text.Json.JsonSerializer.Serialize(settingsData, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                System.IO.File.WriteAllText(settingsPath, json);
                System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] Settings saved successfully to {settingsPath}");
            }
            catch (Exception ex)
            {
                // Не показываем ошибку пользователю при автосохранении
                System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] Failed to save settings: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[SaveSettingsToFile] Stack trace: {ex.StackTrace}");
            }
        }

        private static object LoadCommunicationSettingsFromFile()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!System.IO.File.Exists(settingsPath))
                    return new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 };

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                if (settingsData.TryGetProperty("Communication", out var comm))
                {
                    return new
                    {
                        PortName = comm.TryGetProperty("PortName", out var pn) && pn.ValueKind != System.Text.Json.JsonValueKind.Null ? pn.GetString() : (string?)null,
                        BaudRate = comm.TryGetProperty("BaudRate", out var br) && br.ValueKind != System.Text.Json.JsonValueKind.Null ? br.GetInt32() : 19200,
                        Parity = comm.TryGetProperty("Parity", out var par) ? par.GetString() ?? "None" : "None",
                        StopBits = comm.TryGetProperty("StopBits", out var sb) ? sb.GetString() ?? "One" : "One",
                        DataBits = comm.TryGetProperty("DataBits", out var db) && db.ValueKind != System.Text.Json.JsonValueKind.Null ? db.GetInt32() : 8,
                        ReadTimeout = comm.TryGetProperty("ReadTimeout", out var rt) && rt.ValueKind != System.Text.Json.JsonValueKind.Null ? rt.GetInt32() : 700,
                        WriteTimeout = comm.TryGetProperty("WriteTimeout", out var wt) && wt.ValueKind != System.Text.Json.JsonValueKind.Null ? wt.GetInt32() : 700
                    };
                }
            }
            catch { }

            return new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 };
        }

        private void LoadSettingsFromFile()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!System.IO.File.Exists(settingsPath)) return;

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                // Восстанавливаем настройки из категоризированной структуры
                var model = new FormOptions.AppOptions.Model();

                // General category
                if (settingsData.TryGetProperty("General", out var general))
                {
                    if (general.TryGetProperty("PressureUnits", out var pu))
                        model.PressureUnits = pu.GetString() ?? "PSI";
                    if (general.TryGetProperty("TimeUnits", out var tu))
                        model.TimeUnits = tu.GetString() ?? "s";
                    if (general.TryGetProperty("PollingFrequency", out var pf) && pf.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.PollingFrequency = pf.GetInt32();
                }
                else
                {
                    // Обратная совместимость: если структура старая (плоская)
                    if (settingsData.TryGetProperty("PressureUnits", out var pu))
                        model.PressureUnits = pu.GetString() ?? "PSI";
                    if (settingsData.TryGetProperty("TimeUnits", out var tu))
                        model.TimeUnits = tu.GetString() ?? "s";
                    if (settingsData.TryGetProperty("PollingFrequency", out var pf) && pf.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.PollingFrequency = pf.GetInt32();
                }

                // Device category
                if (settingsData.TryGetProperty("Device", out var device))
                {
                    if (device.TryGetProperty("PressureRamp", out var pr) && pr.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.PressureRamp = pr.GetDouble();
                    if (device.TryGetProperty("MaxPressure", out var mp) && mp.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MaxPressure = mp.GetDouble();
                    if (device.TryGetProperty("MinPressure", out var minp) && minp.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MinPressure = minp.GetDouble();
                }
                else
                {
                    // Обратная совместимость
                    if (settingsData.TryGetProperty("PressureRamp", out var pr) && pr.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.PressureRamp = pr.GetDouble();
                    if (settingsData.TryGetProperty("MaxPressure", out var mp) && mp.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MaxPressure = mp.GetDouble();
                    if (settingsData.TryGetProperty("MinPressure", out var minp) && minp.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MinPressure = minp.GetDouble();
                }

                // Control category
                if (settingsData.TryGetProperty("Control", out var control))
                {
                    if (control.TryGetProperty("MaxIncrement", out var mi) && mi.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MaxIncrement = mi.GetDouble();
                    if (control.TryGetProperty("MinIncrement", out var mini) && mini.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MinIncrement = mini.GetDouble();
                }
                else
                {
                    // Обратная совместимость
                    if (settingsData.TryGetProperty("MaxIncrement", out var mi) && mi.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MaxIncrement = mi.GetDouble();
                    if (settingsData.TryGetProperty("MinIncrement", out var mini) && mini.ValueKind != System.Text.Json.JsonValueKind.Null)
                        model.MinIncrement = mini.GetDouble();
                }

                // AutoConnect category
                if (settingsData.TryGetProperty("AutoConnect", out var autoConnect))
                {
                    if (autoConnect.TryGetProperty("AutoConnectOnStartup", out var acos) && acos.ValueKind == System.Text.Json.JsonValueKind.True)
                        model.AutoConnectOnStartup = true;
                }

                FormOptions.AppOptions.Current = model;

                // Применяем загруженные настройки
                _presenter?.ApplyOptionsToUi();
            }
            catch (Exception ex)
            {
                // При ошибке загрузки используем настройки по умолчанию
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
            }
        }

        // ====================================================================
        // IMainView Implementation - Settings Persistence
        // ====================================================================

        void IMainView.SaveSettingsIfAutoSaveEnabled()
        {
            SaveSettingsToFile();
        }

        void IMainView.ApplyOptionsToUi()
        {
            // Update internal fields from FormOptions
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _minPressure = FormOptions.AppOptions.Current.MinPressure ?? 0.0;
            _maxIncrementLimit = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;
            _minIncrementLimit = FormOptions.AppOptions.Current.MinIncrement ?? 0.1;

            // Update Ramp Speed from Preferences
            // НЕ обновляем значение, если оно уже получено от устройства
            // Значение должно обновляться только от устройства через ASR
            // Если значение еще не получено от устройства, используем значение из Preferences
            if (_rampSpeed < 0.001)
            {
                var rampSpeed = FormOptions.AppOptions.Current.PressureRamp;
                if (rampSpeed.HasValue && rampSpeed.Value > 0.001)
                {
                    // _rampSpeed is in AlicatForm.cs, we need to update it through a method or property
                    // For now, just update the display
                    var rampValue = rampSpeed.Value;
                    var rampText = rampValue.ToString("F1", System.Globalization.CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                    UI_SetRampSpeedUnits($"{rampText} {_unit}/s");
                }
            }
            else
            {
                // Если значение уже есть, просто обновляем отображение с текущими единицами
                var rampText = _rampSpeed.ToString("F1", System.Globalization.CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                UI_SetRampSpeedUnits($"{rampText} {_unit}/s");
            }

            ValidateTargetAgainstMax();
            ValidateIncrementAgainstMax();
            UpdateIncrementButtons();

            // Update System Settings display
            lblMaxPressureValue.Text = $"{_maxPressure:F0} {_unit}";
            lblMaxIncrementValue.Text = $"{_maxIncrementLimit:F1} {_unit}";
        }

        bool IMainView.IsDarkTheme => isDarkTheme;

        // ====================================================================
        // Help Menu Handlers
        // ====================================================================

        private void menuHelpAboutDACTools_Click(object? sender, EventArgs e)
        {
            // Создаем кастомный диалог с кликабельной ссылкой
            using var aboutDialog = new Form
            {
                Text = "About DACTools",
                Size = new System.Drawing.Size(500, 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var titleLabel = new Label
            {
                Text = "DACTools",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20)
            };

            var descriptionLabel = new Label
            {
                Text = "Custom Equipment for High-Pressure Research\n\n" +
                       "DACTools specializes in developing professional software and\n" +
                       "equipment solutions for Diamond Anvil Cell (DAC) experiments\n" +
                       "and high-pressure research applications.\n\n" +
                       "Our tools are designed for leading research laboratories\n" +
                       "worldwide, providing precise control and monitoring capabilities\n" +
                       "for critical scientific experiments.",
                AutoSize = false,
                Size = new System.Drawing.Size(440, 180),
                Location = new System.Drawing.Point(20, 55)
            };

            var websiteLabel = new Label
            {
                Text = "Website: https://dactools.com/",
                AutoSize = true,
                Location = new System.Drawing.Point(20, 245),
                ForeColor = System.Drawing.Color.Blue,
                Cursor = Cursors.Hand,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Underline)
            };

            websiteLabel.Click += (s, args) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://dactools.com/",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open website:\n{ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var copyrightLabel = new Label
            {
                Text = "© 2025 DACTools\nAll rights reserved",
                AutoSize = true,
                Location = new System.Drawing.Point(20, 275),
                ForeColor = System.Drawing.Color.Gray
            };

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new System.Drawing.Size(75, 25),
                Location = new System.Drawing.Point(385, 280)
            };

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(descriptionLabel);
            panel.Controls.Add(websiteLabel);
            panel.Controls.Add(copyrightLabel);
            panel.Controls.Add(okButton);

            aboutDialog.Controls.Add(panel);
            aboutDialog.AcceptButton = okButton;

            aboutDialog.ShowDialog(this);
        }

        private void menuHelpAboutAlicat_Click(object? sender, EventArgs e)
        {
            string version = "1.2.0";
            string message = "Alicat Controller\n\n" +
                           $"Version {version}\n\n" +
                           "Professional Windows application for controlling\n" +
                           "Alicat Pressure Controllers via RS-232.\n\n" +
                           "Features:\n" +
                           "• Real-time pressure monitoring\n" +
                           "• Precise digital pressure control\n" +
                           "• Live graphing and data logging\n" +
                           "• Terminal for RS-232 debugging\n\n" +
                           "© 2025 DAC Tools\n" +
                           "Built with .NET 8.0";

            MessageBox.Show(message, "About Alicat Controller",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ====================================================================
        // Device Menu Handlers
        // ====================================================================

        private void menuDeviceInfo_Click(object? sender, EventArgs e)
        {
            _presenter?.ShowDeviceInfo(this);
        }

        // ====================================================================
        // Cleanup
        // ====================================================================

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Сохраняем настройки перед закрытием программы
            try
            {
                SaveSettingsToFile();
                System.Diagnostics.Debug.WriteLine("[OnFormClosing] Settings saved successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OnFormClosing] Failed to save settings: {ex.Message}");
            }
            
            // Stop polling timer
            _pollTimer?.Stop();
            
            // End data store session
            DataStore.EndSession();
            
            // Dispose serial connection
            _serial?.Dispose();
            
            // Dispose presenter
            _presenter?.Dispose();
            
            base.OnFormClosing(e);
        }
    }
}

