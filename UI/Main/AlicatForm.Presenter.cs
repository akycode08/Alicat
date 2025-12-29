using System;
using System.Diagnostics;
using System.Windows.Forms;
using Alicat.Presentation.Presenters;
using Alicat.Services.Data;
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;

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

        private void menuDeviceDisconnect_Click(object? sender, EventArgs e)
        {
            _presenter?.DisconnectDevice();
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
                Application.Exit();
            }
        }

        // ====================================================================
        // Settings Menu Handlers
        // ====================================================================

        private void menuSettingsAutoSave_Click(object? sender, EventArgs e)
        {
            bool isEnabled = menuSettingsAutoSave.Checked;
            
            if (isEnabled)
            {
                // Если включили Auto-save, сразу сохраняем текущие настройки
                SaveSettingsToFile();
                UI_AppendStatusInfo("Auto-save enabled - settings will be saved automatically");
            }
            else
            {
                UI_AppendStatusInfo("Auto-save disabled");
            }
        }

        // ====================================================================
        // Settings Persistence
        // ====================================================================

        private static string GetSettingsFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsDir = System.IO.Path.Combine(appDataPath, "DACTools", "AlicatController");
            return System.IO.Path.Combine(settingsDir, "settings.json");
        }

        private void SaveSettingsToFile()
        {
            if (!menuSettingsAutoSave.Checked) return;

            try
            {
                string settingsPath = GetSettingsFilePath();
                string? directory = System.IO.Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                var settingsData = new
                {
                    PressureUnits = FormOptions.AppOptions.Current.PressureUnits,
                    TimeUnits = FormOptions.AppOptions.Current.TimeUnits,
                    PressureRamp = FormOptions.AppOptions.Current.PressureRamp,
                    MaxPressure = FormOptions.AppOptions.Current.MaxPressure,
                    MinPressure = FormOptions.AppOptions.Current.MinPressure,
                    MaxIncrement = FormOptions.AppOptions.Current.MaxIncrement,
                    MinIncrement = FormOptions.AppOptions.Current.MinIncrement,
                    AutoSaveEnabled = menuSettingsAutoSave.Checked,
                    LastSaved = DateTime.Now
                };

                string json = System.Text.Json.JsonSerializer.Serialize(settingsData, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                System.IO.File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                // Не показываем ошибку пользователю при автосохранении
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        private void LoadSettingsFromFile()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!System.IO.File.Exists(settingsPath)) return;

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                // Восстанавливаем настройки
                var model = new FormOptions.AppOptions.Model
                {
                    PressureUnits = settingsData.TryGetProperty("PressureUnits", out var pu)
                        ? pu.GetString() ?? "PSI" : "PSI",
                    TimeUnits = settingsData.TryGetProperty("TimeUnits", out var tu)
                        ? tu.GetString() ?? "s" : "s",
                    PressureRamp = settingsData.TryGetProperty("PressureRamp", out var pr) && pr.ValueKind != System.Text.Json.JsonValueKind.Null
                        ? pr.GetDouble() : null,
                    MaxPressure = settingsData.TryGetProperty("MaxPressure", out var mp) && mp.ValueKind != System.Text.Json.JsonValueKind.Null
                        ? mp.GetDouble() : null,
                    MinPressure = settingsData.TryGetProperty("MinPressure", out var minp) && minp.ValueKind != System.Text.Json.JsonValueKind.Null
                        ? minp.GetDouble() : null,
                    MaxIncrement = settingsData.TryGetProperty("MaxIncrement", out var mi) && mi.ValueKind != System.Text.Json.JsonValueKind.Null
                        ? mi.GetDouble() : null,
                    MinIncrement = settingsData.TryGetProperty("MinIncrement", out var mini) && mini.ValueKind != System.Text.Json.JsonValueKind.Null
                        ? mini.GetDouble() : null
                };

                FormOptions.AppOptions.Current = model;

                // Восстанавливаем состояние Auto-save
                if (settingsData.TryGetProperty("AutoSaveEnabled", out var ase))
                {
                    menuSettingsAutoSave.Checked = ase.GetBoolean();
                }

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
            lblMinPressureValue.Text = $"{_minPressure:F0} {_unit}";
            lblMaxIncrementValue.Text = $"{_maxIncrementLimit:F1} {_unit}";
            lblMinIncrementValue.Text = $"{_minIncrementLimit:F1} {_unit}";
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

