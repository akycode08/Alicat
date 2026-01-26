using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;

namespace PrecisionPressureController.UI.Features.Graph.Views
{
    public partial class ChartWindow
    {
        // Current zoom/pan mode state (track separately for UI)
        private bool _isZoomModeActive = false;
        private bool _isPanModeActive = false;

        /// <summary>
        /// Инициализирует панель инструментов графика
        /// Использует встроенные возможности LiveCharts2 для zoom/pan
        /// </summary>
        private void InitializeToolbar()
        {
            // Connect event handlers for toolbar buttons (created in Designer)
            if (btnZoom != null)
            {
                btnZoom.Visible = true;
                btnZoom.Click += BtnZoom_Click;
            }
            if (btnPan != null)
            {
                btnPan.Visible = true;
                btnPan.Click += BtnPan_Click;
            }
            if (btnPlus != null) btnPlus.Click += BtnZoomIn_Click;
            if (btnMinus != null) btnMinus.Click += BtnZoomOut_Click;
            if (btnFullscreenChart != null) btnFullscreenChart.Click += BtnFitToScreen_Click;
            if (btnHome != null) btnHome.Click += BtnHome_Click;
            
            // Initialize button states
            UpdateButtonStates();
        }

        // =========================
        // Zoom and Pan mode handlers (using LiveCharts2 built-in functionality)
        // =========================

        private void BtnZoom_Click(object? sender, EventArgs e)
        {
            // Toggle zoom mode only (zoom with mouse wheel, no pan)
            if (_isZoomModeActive)
            {
                // Zoom is active - turn it off
                _isZoomModeActive = false;
                _isPanModeActive = false;
                chartPressure.ZoomMode = ZoomAndPanMode.None;
                
                // Restore normal MinZoomDelta
                RestoreAxisZoomSettings();
            }
            else
            {
                // Activate zoom mode only (X and Y axes for zoom)
                // This enables zoom with mouse wheel, but pan (drag) is disabled
                _isZoomModeActive = true;
                _isPanModeActive = false; // Disable pan when zoom is activated
                chartPressure.ZoomMode = ZoomAndPanMode.X | ZoomAndPanMode.Y;
                
                // Restore normal MinZoomDelta for zoom
                RestoreAxisZoomSettings();
            }
            
            UpdateButtonStates();
        }

        private void BtnPan_Click(object? sender, EventArgs e)
        {
            // Toggle pan mode only (drag to pan, no zoom)
            if (_isPanModeActive)
            {
                // Pan is active - turn it off
                _isPanModeActive = false;
                _isZoomModeActive = false;
                chartPressure.ZoomMode = ZoomAndPanMode.None;
                
                // Restore normal MinZoomDelta
                RestoreAxisZoomSettings();
            }
            else
            {
                // Activate pan mode only
                // In LiveCharts2, we use Both for pan, but disable zoom by setting very large MinZoomDelta
                _isPanModeActive = true;
                _isZoomModeActive = false; // Disable zoom when pan is activated
                
                // Use Both mode - this enables pan (drag), but we'll disable zoom via MinZoomDelta
                chartPressure.ZoomMode = ZoomAndPanMode.Both;
                
                // Disable zoom by mouse wheel by setting MinZoomDelta to very large value
                // This effectively disables zoom while keeping pan enabled
                if (chartPressure.XAxes != null && chartPressure.XAxes.Any())
                {
                    var xAxis = chartPressure.XAxes.FirstOrDefault();
                    if (xAxis != null)
                    {
                        xAxis.MinZoomDelta = double.MaxValue; // Disable zoom
                    }
                }
                if (chartPressure.YAxes != null && chartPressure.YAxes.Any())
                {
                    var yAxis = chartPressure.YAxes.FirstOrDefault();
                    if (yAxis != null)
                    {
                        yAxis.MinZoomDelta = double.MaxValue; // Disable zoom
                    }
                }
            }
            
            UpdateButtonStates();
        }

        private void RestoreAxisZoomSettings()
        {
            // Restore normal MinZoomDelta (allow zoom)
            if (chartPressure.XAxes != null && chartPressure.XAxes.Any())
            {
                var xAxis = chartPressure.XAxes.FirstOrDefault();
                if (xAxis != null)
                {
                    xAxis.MinZoomDelta = null; // Restore default (allow zoom)
                }
            }
            if (chartPressure.YAxes != null && chartPressure.YAxes.Any())
            {
                var yAxis = chartPressure.YAxes.FirstOrDefault();
                if (yAxis != null)
                {
                    yAxis.MinZoomDelta = null; // Restore default (allow zoom)
                }
            }
        }

        private void UpdateButtonStates()
        {
            // Update visual state of zoom and pan buttons
            bool isZoomActive = _isZoomModeActive;
            bool isPanActive = _isPanModeActive;

            if (btnZoom != null)
            {
                btnZoom.BackColor = isZoomActive 
                    ? Color.FromArgb(60, 100, 150) // Active - highlighted
                    : Color.FromArgb(30, 33, 40); // Inactive - default
            }

            if (btnPan != null)
            {
                btnPan.BackColor = isPanActive 
                    ? Color.FromArgb(60, 100, 150) // Active - highlighted
                    : Color.FromArgb(30, 33, 40); // Inactive - default
            }
        }

        // =========================
        // Toolbar button handlers (using LiveCharts2 built-in zoom/pan)
        // =========================

        private void BtnZoomIn_Click(object? sender, EventArgs e)
        {
            // Use LiveCharts2 built-in zoom
            // Zoom in by adjusting axis limits
            ZoomChart(1.2); // Zoom in by 20%
        }

        private void BtnZoomOut_Click(object? sender, EventArgs e)
        {
            // Use LiveCharts2 built-in zoom
            // Zoom out by adjusting axis limits
            ZoomChart(1.0 / 1.2); // Zoom out by 20%
        }

        private void BtnFitToScreen_Click(object? sender, EventArgs e)
        {
            FitChartToScreen();
        }

        private void BtnHome_Click(object? sender, EventArgs e)
        {
            ResetChartView();
        }

        // =========================
        // Zoom functionality (simplified - using LiveCharts2 axis limits)
        // =========================

        private void ZoomChart(double factor)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Get current limits
            double xMin = xAxis.MinLimit ?? 0;
            double xMax = xAxis.MaxLimit ?? _timeWindowSeconds;
            double xCenter = (xMin + xMax) / 2;
            double xRange = xMax - xMin;
            double newXRange = xRange / factor;

            // Apply zoom limits
            double minXRange = 1.0; // Minimum 1 second
            double maxXRange = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds;

            if (newXRange < minXRange) newXRange = minXRange;
            else if (newXRange > maxXRange) newXRange = maxXRange;

            // Calculate new limits
            double newXMin = xCenter - newXRange / 2;
            double newXMax = xCenter + newXRange / 2;

            if (newXMin < 0)
            {
                newXMin = 0;
                newXMax = newXRange;
            }
            if (newXMax > _timeSeconds && _timeSeconds > 0)
            {
                newXMax = _timeSeconds;
                newXMin = Math.Max(0, newXMax - newXRange);
            }

            // Y axis: auto-adjust or use current limits
            double yMin = yAxis.MinLimit ?? 0;
            double yMax = yAxis.MaxLimit ?? double.MaxValue;
            if (yMax == double.MaxValue)
            {
                if (_seriesCurrent.Count > 0)
                {
                    var values = _seriesCurrent.Select(p => p.Y.Value).ToList();
                    yMin = values.Min();
                    yMax = values.Max();
                    double yPadding = (yMax - yMin) * 0.1;
                    yMin = Math.Max(0, yMin - yPadding);
                    yMax += yPadding;
                }
                else
                {
                    yMin = 0;
                    yMax = 100;
                }
            }

            double yCenter = (yMin + yMax) / 2;
            double yRange = yMax - yMin;
            double newYRange = yRange / factor;

            double newYMin = yCenter - newYRange / 2;
            double newYMax = yCenter + newYRange / 2;
            if (newYMin < 0)
            {
                newYMin = 0;
                newYMax = newYRange;
            }

            // Apply new limits
            xAxis.MinLimit = newXMin;
            xAxis.MaxLimit = newXMax;
            yAxis.MinLimit = newYMin;
            yAxis.MaxLimit = newYMax;

            ApplyThresholdLines();
            UpdateTargetLine();
        }

        private void FitChartToScreen()
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Fit X axis to time window
            double xMax = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds;
            double xMin = Math.Max(0, xMax - _timeWindowSeconds);
            
            if (_timeSeconds < _timeWindowSeconds)
            {
                xMin = 0;
                xMax = _timeWindowSeconds;
            }
            
            xAxis.MinLimit = xMin;
            xAxis.MaxLimit = xMax;

            // Fit Y axis to data range
            if (_seriesCurrent.Count > 0)
            {
                var valuesInRange = _seriesCurrent
                    .Where(p => p.X.Value >= xMin && p.X.Value <= xMax)
                    .Select(p => p.Y.Value)
                    .ToList();
                
                if (valuesInRange.Count > 0)
                {
                    double yMin = valuesInRange.Min();
                    double yMax = valuesInRange.Max();
                    double yPadding = (yMax - yMin) * 0.1;
                    yAxis.MinLimit = Math.Max(0, yMin - yPadding);
                    yAxis.MaxLimit = yMax + yPadding;
                }
                else
                {
                    var allValues = _seriesCurrent.Select(p => p.Y.Value).ToList();
                    double yMin = allValues.Min();
                    double yMax = allValues.Max();
                    double yPadding = (yMax - yMin) * 0.1;
                    yAxis.MinLimit = Math.Max(0, yMin - yPadding);
                    yAxis.MaxLimit = yMax + yPadding;
                }
            }
            else
            {
                yAxis.MinLimit = 0;
                yAxis.MaxLimit = null;
            }

            ApplyThresholdLines();
            UpdateTargetLine();
        }

        private void ResetChartView()
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Reset to default view
            ApplyTimeWindow(forceTrim: false);
            yAxis.MinLimit = null;
            yAxis.MaxLimit = null;

            ApplyThresholdLines();
            UpdateTargetLine();
        }
    }
}
