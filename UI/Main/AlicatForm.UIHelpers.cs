using System;
using System.Globalization;
using Alicat.Presentation.Presenters;
using Alicat.Domain;

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
        public void RefreshCurrent()
        {
            // Используем свойство из IMainView, если доступно, иначе используем локальное поле
            double currentValue = ((IMainView)this).Current;
            lblCurrentValue.Text = currentValue.ToString("F1", CultureInfo.InvariantCulture);
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

            // Update System Settings with new units
            lblMaxPressureValue.Text = $"{_maxPressure:F0} {displayUnits}";
            lblMaxIncrementValue.Text = $"{_maxIncrementLimit:F1} {displayUnits}";

            // Update ramp speed with new units (preserve value if available)
            if (_rampSpeed > 0.001)
            {
                UI_SetRampSpeedUnits($"{TrimZeros(_rampSpeed)} {displayUnits}/s");
            }
            else
            {
                UI_SetRampSpeedUnits($"{displayUnits}/s");
            }
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
        public void UI_SetTrendStatus(double? prev, double now, bool isExhaust, double rampSpeed)
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
                lblCurrentRate.Text = $"→ 0.0 {_unit}/s";
                lblCurrentRate.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
                return;
            }

            double delta = now - prev.Value;
            double rate = delta / 0.5; // Assuming 500ms poll interval

            if (delta > EPS)
            {
                lblCurrentRate.Text = $"↗ +{Math.Abs(rate):F1} {_unit}/s";
                lblCurrentRate.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else if (delta < -EPS)
            {
                lblCurrentRate.Text = $"↘ -{Math.Abs(rate):F1} {_unit}/s";
                lblCurrentRate.ForeColor = System.Drawing.Color.RoyalBlue;
            }
            else
            {
                lblCurrentRate.Text = $"→ 0.0 {_unit}/s";
                lblCurrentRate.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
            }

            // Update target status with ETA using unified function
            var etaResult = ETACalculator.CalculateETA(now, _setPoint, rate, isExhaust);
            
            if (isExhaust)
            {
                lblTargetStatus.Text = etaResult.DisplayText;
                lblTargetStatus.ForeColor = System.Drawing.Color.Red;
            }
            else if (etaResult.IsAtTarget)
            {
                lblTargetStatus.Text = etaResult.DisplayText;
                lblTargetStatus.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
            }
            else if (etaResult.IsStable)
            {
                // Если стабильное состояние - показываем разницу
                double diff = Math.Abs(now - _setPoint);
                lblTargetStatus.Text = $"→ {diff:F1} {_unit}";
                lblTargetStatus.ForeColor = isDarkTheme ? darkTextMuted : lightTextMuted;
            }
            else if (etaResult.EtaSeconds.HasValue)
            {
                // Показываем ETA в формате MM:SS (как в GraphForm)
                lblTargetStatus.Text = etaResult.DisplayText;
                lblTargetStatus.ForeColor = isDarkTheme ? darkAccentGreen : lightAccentGreen;
            }
            else
            {
                // Нет данных для расчета ETA
                lblTargetStatus.Text = etaResult.DisplayText;
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
                
                // Возвращаем нормальные цвета для значений
                lblCurrentValue.ForeColor = isDarkTheme ? darkAccentBlue : lightAccentBlue;
                lblTargetValue.ForeColor = isDarkTheme ? darkAccentGold : lightAccentGold;
            }
            else
            {
                lblStatusDot.ForeColor = isDarkTheme ? darkStatusDotDisconnected : lightStatusDotDisconnected;
                lblConnectionStatus.Text = "Disconnected";
                lblConnectionStatus.ForeColor = isDarkTheme ? darkTextMuted : lightTextMuted;
                
                // Устанавливаем красный цвет для значений при отключении
                lblCurrentValue.ForeColor = isDarkTheme ? darkValueDisconnected : lightValueDisconnected;
                lblTargetValue.ForeColor = isDarkTheme ? darkValueDisconnected : lightValueDisconnected;
                
                // Обновляем rate с правильными единицами при отключении
                UI_SetTrendStatus(null, 0.0, false, 0.0);
            }
        }

        /// <summary>
        /// Обновляет baud rate в Status Bar.
        /// </summary>
        public void UI_UpdateBaudRate(int baudRate)
        {
            lblBaudRate.Text = $"Baud: {baudRate}";
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
        /// Последняя строка выделяется жирным шрифтом.
        /// </summary>
        public void UI_AppendStatusInfo(string line)
        {
            var current = lblStatusInfoText.Text;
            var lines = current.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Keep last 3 lines + new one
            var keep = lines.Length > 3 ? lines[^3..] : lines;
            
            // Очищаем и добавляем текст с форматированием
            lblStatusInfoText.Clear();
            
            // Добавляем предыдущие строки обычным шрифтом
            foreach (var oldLine in keep)
            {
                if (!string.IsNullOrWhiteSpace(oldLine))
                {
                    lblStatusInfoText.SelectionFont = new Font("Segoe UI", 9F, FontStyle.Regular);
                    lblStatusInfoText.SelectionColor = isDarkTheme ? darkTextSecondary : lightTextSecondary;
                    lblStatusInfoText.AppendText(oldLine + "\n");
                }
            }
            
            // Добавляем новую строку жирным шрифтом
            lblStatusInfoText.SelectionFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusInfoText.SelectionColor = isDarkTheme ? darkTextPrimary : lightTextPrimary;
            lblStatusInfoText.AppendText("• " + line);
            
            // Прокручиваем вниз
            lblStatusInfoText.SelectionStart = lblStatusInfoText.Text.Length;
            lblStatusInfoText.ScrollToCaret();
        }
    }
}