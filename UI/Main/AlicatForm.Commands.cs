using System;
using System.Globalization;
using System.Windows.Forms;
using Alicat.Services.Protocol;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;
using Alicat.UI.Features.Terminal.Views;
using System.Threading.Tasks;
using Alicat.Presentation.Presenters;

namespace Alicat
{
    /// <summary>
    /// Partial class AlicatForm: Command handlers.
    /// Обработчики событий кнопок и команд (БЕЗ методов из Communication.cs и Testing.cs).
    /// </summary>
    public partial class AlicatForm
    {
        // ====================================================================
        // INCREMENT CONTROL
        // ====================================================================

        private void btnIncrementMinus_Click(object? sender, EventArgs e)
        {
            _currentIncrement = Math.Max(0.1, _currentIncrement - 0.1);
            txtIncrement.Text = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);
            _lastValidIncrementText = txtIncrement.Text; // Обновляем последнее валидное значение
            UpdateIncrementButtons();
        }

        private void btnIncrementPlus_Click(object? sender, EventArgs e)
        {
            _currentIncrement = Math.Min(_maxIncrementLimit, _currentIncrement + 0.1);
            txtIncrement.Text = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);
            _lastValidIncrementText = txtIncrement.Text; // Обновляем последнее валидное значение
            UpdateIncrementButtons();
        }

        // ====================================================================
        // INCREASE / DECREASE PRESSURE
        // ====================================================================

        private void btnIncrease_Click(object? sender, EventArgs e)
        {
            var next = _setPoint + _currentIncrement;

            if (next > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(this,
                    $"Cannot exceed Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} PSI).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ValidateTargetAgainstMax();
                return;
            }

            SendSetPoint(next);
            ValidateTargetAgainstMax();
        }

        private void btnDecrease_Click(object? sender, EventArgs e)
        {
            var next = Math.Max(_minPressure, _setPoint - _currentIncrement);
            SendSetPoint(next);
            ValidateTargetAgainstMax();
        }

        // ====================================================================
        // GO TO TARGET
        // ====================================================================

        private void btnGoTarget_Click(object? sender, EventArgs e)
        {
            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = txtTargetInput.Text?.Trim() ?? string.Empty;
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

            if (targetValue < _minPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(this,
                    $"Target value must not be less than Min Pressure ({_minPressure.ToString("0.###", CultureInfo.InvariantCulture)} PSI).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (targetValue > _maxPressure)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(this,
                    $"Target value must not exceed Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} PSI).",
                    "Limit exceeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ValidateTargetAgainstMax();
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
                UI_SetSetPoint(_setPoint, _unit);
                _serial.Send(AlicatCommands.ReadAls);

                txtTargetInput.Clear();

                UI_AppendStatusInfo($"Target set to {displayVal} {unit}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send command:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ValidateTargetAgainstMax();
            }
        }

        // ====================================================================
        // PAUSE / CONTINUE
        // ====================================================================
        // Обработчик перенесен в AlicatForm.Presenter.cs (btnPause_Click_Presenter)
        // для использования Presenter паттерна

        // ====================================================================
        // PURGE
        // ====================================================================

        private async void btnPurge_Click(object? sender, EventArgs e)
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

                UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true, _rampSpeed);
                UI_AppendStatusInfo("Purge started");

                _setPoint = 0.0;
                UI_SetSetPoint(_setPoint, _unit);

                await Task.Delay(400);
                _serial.Send("AC");
                _serial.Send(AlicatCommands.ReadAls);

                UI_AppendStatusInfo("Purge complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Purge error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ValidateTargetAgainstMax();
            }
        }

        // ====================================================================
        // SEND SET POINT
        // ====================================================================

        private void SendSetPoint(double sp)
        {
            _setPoint = sp;
            UI_SetSetPoint(_setPoint, _unit);

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
        // MENU: OPTIONS
        // ====================================================================

        private void btnOptions_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormOptions();
            dlg.StartPosition = FormStartPosition.CenterParent;
            
            // Подписываемся на событие Applied для обновления при нажатии "Apply"
            dlg.Applied += (_, __) =>
            {
                // Настройки уже применены в TryApplyFromUi() перед вызовом Applied
                ApplyOptionsToUi();
                var ramp = FormOptions.AppOptions.Current.PressureRamp;
                
                // Применяем ramp speed через presenter, чтобы обновить внутреннее значение _rampSpeed
                if (ramp.HasValue && _presenter != null && _serial != null)
                {
                    try
                    {
                        _presenter.SetRampSpeed(ramp.Value);
                    }
                    catch (InvalidOperationException)
                    {
                        // Устройство не подключено, но значение уже сохранено в настройках
                    }
                }
                
                // Синхронизируем thresholds в GraphForm (если открыт)
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    _graphForm.RefreshThresholdsFromSettings();
                }
                
                // Сохраняем настройки при применении
                System.Diagnostics.Debug.WriteLine($"[btnOptions_Click] Applied event - AutoConnectOnStartup = {FormOptions.AppOptions.Current.AutoConnectOnStartup}");
                SaveSettingsToFile();
            };
            
            var result = dlg.ShowDialog(this);

            // Применяем настройки после закрытия диалога (на случай, если нажали OK)
            if (result == DialogResult.OK)
            {
                ApplyOptionsToUi();
                
                // Применяем ramp speed через presenter, чтобы обновить внутреннее значение _rampSpeed
                var ramp = FormOptions.AppOptions.Current.PressureRamp;
                if (ramp.HasValue && _presenter != null && _serial != null)
                {
                    try
                    {
                        _presenter.SetRampSpeed(ramp.Value);
                    }
                    catch (InvalidOperationException)
                    {
                        // Устройство не подключено, но значение уже сохранено в настройках
                    }
                }
                
                // Синхронизируем thresholds в GraphForm (если открыт)
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    _graphForm.RefreshThresholdsFromSettings();
                }
                
                // Сохраняем настройки при нажатии OK
                System.Diagnostics.Debug.WriteLine($"[btnOptions_Click] OK clicked - saving settings...");
                SaveSettingsToFile();
            }
        }
        
        // SaveSettingsToFile доступен напрямую из AlicatForm.Presenter.cs (partial class)
        // Метод объявлен как private в Presenter.cs, но доступен через partial class

        // ====================================================================
        // MENU: NEW SESSION
        // ====================================================================

        private void menuFileNewSession_Click(object? sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder for session data";

            if (folderDialog.ShowDialog() != DialogResult.OK)
                return;

            string fileName = $"session_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            string fullPath = System.IO.Path.Combine(folderDialog.SelectedPath, fileName);

            DataStore.StartSession(fullPath);
            
            // Сбрасываем read-only режим при создании новой сессии
            _isReadOnlyMode = false;
            _currentSessionFilePath = fullPath;
            UpdateReadOnlyMode(false);

            MessageBox.Show(
                $"Session started!\n\nSaving to:\n{fullPath}",
                "New Session",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            UI_AppendStatusInfo("New session started");
        }

        // ====================================================================
        // NAVIGATION: TABLE
        // ====================================================================

        private void btnTable_Click(object? sender, EventArgs e)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
            {
                _tableForm = new TableForm(DataStore);
                _tableForm.StartPosition = FormStartPosition.CenterParent;
                // Синхронизируем тему с главной формой
                _tableForm.ApplyTheme(isDarkTheme);
                _tableForm.Show(this);
            }
            else
            {
                if (_tableForm.WindowState == FormWindowState.Minimized)
                    _tableForm.WindowState = FormWindowState.Normal;
                _tableForm.Focus();
            }
        }

        // ====================================================================
        // NAVIGATION: TERMINAL
        // ====================================================================

        private void btnTerminal_Click(object? sender, EventArgs e)
        {
            if (_terminalForm == null || _terminalForm.IsDisposed)
            {
                _terminalForm = new TerminalForm();
                _terminalForm.CommandSent += TerminalForm_CommandSent;
            }

            if (!_terminalForm.Visible)
            {
                _terminalForm.Show(this);
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
    }
}