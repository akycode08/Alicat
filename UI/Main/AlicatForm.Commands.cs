using System.Globalization;
using System.Threading.Tasks;
using Alicat.Services.Controllers;
using Alicat.Services.Serial;

namespace Alicat
{
    public partial class AlicatForm : Form
        {
        private void btnOptions_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormOptions();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);

            ApplyOptionsToUi();

            var ramp = FormOptions.AppOptions.Current.PressureRamp;
            _ramp?.TryApply(ramp);   // SR без конвертаций


            // Если задан Ramp — шлём SR (без конвертаций)
            // var ramp = FormOptions.AppOptions.Current.PressureRamp;
            if (_serial != null && ramp is double r)
                _serial.Send($"SR {r.ToString("G", CultureInfo.InvariantCulture)}");
        }



        // ================= GO ± =================
        private void btnGoPlus_Click(object? sender, EventArgs e)
                {
                    var inc = (double)nudIncrement.Value;
                    var next = _setPoint + inc;

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

            private void btnGoMinus_Click(object? sender, EventArgs e)
            {
                var inc = (double)nudIncrement.Value;
                SendSetPoint(_setPoint - inc);
                ValidateTargetAgainstMax();
            }

            private void btnGoTarget_Click(object? sender, EventArgs e)
            {
                // ❌ больше НЕ включаем кнопку принудительно
                if (_serial == null)
                {
                    MessageBox.Show("Device is not connected.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string raw = txtTarget.Text?.Trim() ?? string.Empty;
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
                    MessageBox.Show(this,
                        $"Target value exceeds Max Pressure ({_maxPressure.ToString("0.###", CultureInfo.InvariantCulture)} PSI).",
                        "Limit exceeded",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ValidateTargetAgainstMax();
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
                if (!chkConfirmGo.Checked)
                {
                    var ask = MessageBox.Show(
                        $"Do you want to change the target value to {displayVal} {unit}?",
                        "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ask != DialogResult.Yes) return;
                }

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

                    _serial.RequestAls();

                    chkConfirmGo.Checked = false;
                    txtTarget.Clear();
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

            // ================= PURGE =================
            private async void btnPurge_Click(object? sender, EventArgs e)
            {
                if (_serial is null)
                {
                    MessageBox.Show("Нет соединения с прибором.", "Purge",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!chkConfirmPurge.Checked)
                {
                    var ask = MessageBox.Show("Сразу открыть выхлоп и удерживать?",
                                              "Confirm purge",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ask != DialogResult.Yes) return;
                }
                chkConfirmPurge.Checked = false;

                try
                {
                    _serial.Send("AE");
                    _isExhaust = true;

                    UI_SetTrendStatus(_lastCurrent, _current, isExhaust: true);
                    UI_Status_Up(false);
                    UI_Status_Mid(false);
                    UI_Status_Down(true);

                    _setPoint = 0.0;
                    UI_SetSetPoint(_setPoint, _unit);

                    await Task.Delay(400);   // короткая пауза
                    _serial.Send("AC");

                    _serial.RequestAls();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка Purge: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ValidateTargetAgainstMax();
                    ValidateIncrementAgainstMax();
                }
            }

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
                _serial.RequestAls();
            }

    }
}