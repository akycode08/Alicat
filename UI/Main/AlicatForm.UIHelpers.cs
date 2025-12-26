using System;
using System.Globalization;

namespace Alicat
{
    /// <summary>
    /// Partial class AlicatForm: UI Helper methods.
    /// Методы для обновления элементов интерфейса.
    /// </summary>
    public partial class AlicatForm
    {
        // ====================================================================
        // CURRENT PRESSURE
        // ====================================================================

        /// <summary>
        /// Обновляет отображение текущего давления в карточке Current Pressure.
        /// </summary>
        private void RefreshCurrent()
        {
            lblCurrentValue.Text = _current.ToString("F1", CultureInfo.InvariantCulture);
        }

        // ====================================================================
        // UNITS
        // ====================================================================

        /// <summary>
        /// Устанавливает единицы измерения давления во всех местах UI.
        /// </summary>
        public void UI_SetPressureUnits(string units)
        {
            string displayUnits = string.IsNullOrWhiteSpace(units) ? "PSIG" : units.Trim();

            // Обновляем внутреннюю переменную единиц
            _unit = displayUnits;

            // System Settings panel
            lblUnitsValue.Text = displayUnits;

            // Current card
            lblCurrentUnit.Text = displayUnits;

            // Target card
            lblTargetUnit.Text = displayUnits;

            // Set Target input
            lblTargetInputUnit.Text = displayUnits;

            // Increment control
            lblIncrementUnit.Text = displayUnits;

            // Update buttons with new units
            UpdateIncrementButtons();

            // Update max pressure with new units
            lblMaxPressureValue.Text = $"{_maxPressure:F0} {displayUnits}";

            // Update ramp speed with new units
            UI_SetRampSpeedUnits($"{displayUnits}/s");
        }

        /// <summary>
        /// Устанавливает единицы скорости рампы.
        /// </summary>
        public void UI_SetRampSpeedUnits(string units)
        {
            lblRampSpeedValue.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();
        }

        // ====================================================================
        // SET POINT (TARGET)
        // ====================================================================

        /// <summary>
        /// Устанавливает значение SetPoint в карточке Target Pressure.
        /// </summary>
        public void UI_SetSetPoint(double sp, string? units = null)
        {
            lblTargetValue.Text = sp.ToString("F1", CultureInfo.InvariantCulture);
        }

        // ====================================================================
        // TREND STATUS
        // ====================================================================

        /// <summary>
        /// Обновляет статус тренда (растет/падает/стабильно) и статус достижения цели.
        /// </summary>
        private void UI_SetTrendStatus(double? prev, double now, bool isExhaust)
        {
            if (isExhaust)
            {
                lblCurrentRate.Text = "↓ Exhaust";
                lblCurrentRate.ForeColor = System.Drawing.Color.Red;

                lblTargetStatus.Text = "Purging";
                lblTargetStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            const double EPS = 0.05;

            if (prev is null)
            {
                lblCurrentRate.Text = "→ 0.0 /s";
                lblCurrentRate.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
                return;
            }

            double delta = now - prev.Value;
            double rate = delta / 0.5; // Assuming 500ms poll interval

            if (delta > EPS)
            {
                lblCurrentRate.Text = $"↗ +{Math.Abs(rate):F1} /s";
                lblCurrentRate.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else if (delta < -EPS)
            {
                lblCurrentRate.Text = $"↘ -{Math.Abs(rate):F1} /s";
                lblCurrentRate.ForeColor = System.Drawing.Color.RoyalBlue;
            }
            else
            {
                lblCurrentRate.Text = "→ 0.0 /s";
                lblCurrentRate.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
            }

            // Update target status
            double diff = Math.Abs(now - _setPoint);
            if (diff < 0.5)
            {
                lblTargetStatus.Text = "At target";
                lblTargetStatus.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
            }
            else
            {
                lblTargetStatus.Text = $"→ {diff:F1} {_unit}";
                lblTargetStatus.ForeColor = isDarkTheme ? darkTextMuted : lightTextMuted;
            }
        }

        // ====================================================================
        // CONNECTION STATUS
        // ====================================================================

        /// <summary>
        /// Обновляет статус подключения в Status Bar.
        /// </summary>
        public void UI_UpdateConnectionStatus(bool connected, string? portName = null)
        {
            if (connected)
            {
                lblStatusDot.ForeColor = isDarkTheme ? darkStatusDot : lightStatusDot;
                lblConnectionStatus.Text = portName != null
                    ? $"Connected ({portName})"
                    : "Connected (COM3)";
                lblConnectionStatus.ForeColor = isDarkTheme ? darkTextSecondary : lightTextSecondary;

                lblConnectionValue.Text = portName ?? "COM3";
            }
            else
            {
                lblStatusDot.ForeColor = isDarkTheme ? darkStatusDotDisconnected : lightStatusDotDisconnected;
                lblConnectionStatus.Text = "Disconnected";
                lblConnectionStatus.ForeColor = isDarkTheme ? darkTextMuted : lightTextMuted;

                lblConnectionValue.Text = "—";
            }
        }

        /// <summary>
        /// Обновляет baud rate в Status Bar и System Settings.
        /// </summary>
        public void UI_UpdateBaudRate(int baudRate)
        {
            lblBaudRate.Text = $"Baud: {baudRate}";
            lblBaudRateValue.Text = baudRate.ToString();
        }

        /// <summary>
        /// Обновляет "Last update" в Status Bar.
        /// </summary>
        public void UI_UpdateLastUpdate(string text)
        {
            lblLastUpdate.Text = text;
        }

        // ====================================================================
        // STATUS INFORMATION (Right Panel)
        // ====================================================================

        /// <summary>
        /// Обновляет текст в панели Status Information.
        /// </summary>
        public void UI_UpdateStatusInfo(string text)
        {
            lblStatusInfoText.Text = text;
        }

        /// <summary>
        /// Добавляет строку в Status Information (сохраняя предыдущие).
        /// </summary>
        public void UI_AppendStatusInfo(string line)
        {
            var current = lblStatusInfoText.Text;
            var lines = current.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Keep last 3 lines + new one
            var keep = lines.Length > 3 ? lines[^3..] : lines;
            var newText = string.Join("\n", keep) + "\n• " + line;

            lblStatusInfoText.Text = newText.TrimStart('\n', '\r');
        }
    }
}