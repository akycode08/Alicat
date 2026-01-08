using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Alicat.Services.Data;
using Alicat.Domain;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm
    {
        // Status levels for LIVE STATUS indicator
        private enum StatusLevel { OK, WARN, ALERT }

        // Statistics calculation
        // Note: UI elements are declared in GraphForm.Designer.cs
        private void CalculateAndUpdateStatistics()
        {
            if (tlpSessionStats == null || _dataStore == null) return;

            var points = _dataStore.Points;
            if (points.Count == 0)
            {
                UpdateStatisticsUI(0, 0, 0, 0, 0, TimeSpan.Zero, 0);
                return;
            }

            // Calculate statistics
            double min = points.Min(p => p.Current);
            double max = points.Max(p => p.Current);
            double avg = points.Average(p => p.Current);
            
            // Standard deviation
            double variance = points.Average(p => Math.Pow(p.Current - avg, 2));
            double stdDev = Math.Sqrt(variance);

            // Duration - считаем только если сессия запущена
            TimeSpan duration = _dataStore.IsRunning 
                ? DateTime.Now - _dataStore.SessionStart 
                : TimeSpan.Zero; // Если сессия не запущена, Duration = 0

            // Sample rate (points per second)
            double sampleRate = duration.TotalSeconds > 0 
                ? points.Count / duration.TotalSeconds 
                : 0;

            UpdateStatisticsUI(min, max, avg, stdDev, points.Count, duration, sampleRate);
        }

        private void UpdateStatisticsUI(double min, double max, double avg, double stdDev, int points, TimeSpan duration, double sampleRate)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateStatisticsUI(min, max, avg, stdDev, points, duration, sampleRate)));
                return;
            }

            if (lblMinValue != null) lblMinValue.Text = min.ToString("F2");
            if (lblMaxValue != null) lblMaxValue.Text = max.ToString("F2");
            if (lblAvgValue != null) lblAvgValue.Text = avg.ToString("F2");
            // Std Dev removed from UI
            // if (lblStdDevValue != null) lblStdDevValue.Text = stdDev.ToString("F2");
            if (lblPointsValue != null) lblPointsValue.Text = points.ToString();

            // Format duration
            string durationStr = duration.TotalHours >= 1
                ? $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}"
                : $"{duration.Minutes:D2}:{duration.Seconds:D2}";
            if (lblDurationValue != null) lblDurationValue.Text = durationStr;

            // Format sample rate (removed - no longer displayed)
            // string sampleRateStr = sampleRate > 0 
            //     ? $"~{sampleRate:F1} Hz" 
            //     : "0 Hz";
            // if (lblSampleRateValue != null) lblSampleRateValue.Text = sampleRateStr;

            // Update footer statistics
            UpdateFooterStatistics();
        }

        // Update Live Status with large pressure display
        private void UpdateLiveStatus(double currentPressure, double? targetPressure, string unit, bool isExhaust, double rate)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateLiveStatus(currentPressure, targetPressure, unit, isExhaust, rate)));
                return;
            }

            // Update large pressure display
            if (lblCurrentPressureLarge != null)
            {
                lblCurrentPressureLarge.Text = currentPressure.ToString("F2");
            }

            if (lblCurrentUnit != null)
            {
                lblCurrentUnit.Text = unit;
            }

            // Update Target value
            if (lblTargetValue != null)
            {
                if (targetPressure.HasValue)
                {
                    lblTargetValue.Text = targetPressure.Value.ToString("F2");
                    lblTargetValue.ForeColor = Color.FromArgb(245, 158, 11);  // Золотой цвет
                }
                else
                {
                    lblTargetValue.Text = "--";
                    lblTargetValue.ForeColor = Color.FromArgb(107, 114, 128);  // Серый когда нет значения
                }
            }

            // Determine and update status indicator
            StatusLevel status = DetermineStatusLevel(currentPressure, targetPressure);
            UpdateStatusIndicator(status);

            // Update ETA value using unified function
            if (lblETAValue != null)
            {
                var etaResult = ETACalculator.CalculateETA(currentPressure, targetPressure, rate, isExhaust);
                lblETAValue.Text = etaResult.DisplayText;
                
                // Set color based on result
                if (etaResult.IsAtTarget || etaResult.EtaSeconds.HasValue)
                {
                    lblETAValue.ForeColor = Color.FromArgb(16, 185, 129);  // Зелёный цвет
                }
                else
                {
                    lblETAValue.ForeColor = Color.FromArgb(107, 114, 128);  // Серый цвет
                }
            }
        }

        /// <summary>
        /// Определяет уровень статуса на основе текущего давления и порогов
        /// </summary>
        private StatusLevel DetermineStatusLevel(double currentPressure, double? targetPressure)
        {
            // Удалены nudMaximum и numericUpDown2 - используем значения по умолчанию
            double? maxPressure = 130; // Значение по умолчанию
            double? minPressure = 10; // Значение по умолчанию

            // ALERT: превышение 95% от максимума или ниже минимума
            if (maxPressure.HasValue && currentPressure >= maxPressure.Value * 0.95)
            {
                return StatusLevel.ALERT;
            }
            if (minPressure.HasValue && currentPressure <= minPressure.Value * 1.05)
            {
                return StatusLevel.ALERT;
            }

            // WARN: 85-95% от максимума или близко к минимуму
            if (maxPressure.HasValue && currentPressure >= maxPressure.Value * 0.85)
            {
                return StatusLevel.WARN;
            }
            if (minPressure.HasValue && currentPressure <= minPressure.Value * 1.15)
            {
                return StatusLevel.WARN;
            }

            // OK: в безопасных пределах
            return StatusLevel.OK;
        }

        /// <summary>
        /// Обновляет визуальный индикатор статуса (OK/WARN/ALERT)
        /// </summary>
        private void UpdateStatusIndicator(StatusLevel status)
        {
            if (pnlWarnIndicator == null) return;

            // Clear existing controls
            pnlWarnIndicator.Controls.Clear();

            Label? statusLabel = null;

            switch (status)
            {
                case StatusLevel.OK:
                    pnlWarnIndicator.BackColor = Color.FromArgb(0, 200, 0); // Green
                    pnlWarnIndicator.Visible = true;
                    statusLabel = new Label
                    {
                        Text = "• OK",
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false
                    };
                    break;
                case StatusLevel.WARN:
                    pnlWarnIndicator.BackColor = Color.FromArgb(255, 165, 0); // Orange
                    pnlWarnIndicator.Visible = true;
                    statusLabel = new Label
                    {
                        Text = "• WARN",
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false
                    };
                    break;
                case StatusLevel.ALERT:
                    pnlWarnIndicator.BackColor = Color.FromArgb(220, 20, 60); // Red
                    pnlWarnIndicator.Visible = true;
                    statusLabel = new Label
                    {
                        Text = "• ALERT",
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false
                    };
                    break;
            }

            if (statusLabel != null)
            {
                pnlWarnIndicator.Controls.Add(statusLabel);
            }
        }
    }
}

