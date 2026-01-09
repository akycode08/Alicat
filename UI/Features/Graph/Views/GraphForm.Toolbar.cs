using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm
    {
        // Zoom and Pan state
        private bool _isPanning = false;
        private bool _isZooming = false;
        private Point _lastMousePosition;
        private Point _zoomStartPoint;
        private Rectangle? _zoomSelectionRect;
        private double _savedXMin, _savedXMax, _savedYMin, _savedYMax;
        private bool _hasSavedZoom = false;
        
        // Zoom selection overlay panel (for visual feedback)
        private Panel? _zoomOverlayPanel;

        // Toolbar buttons are now created in Designer (btnZoom, btnPan, btnPlus, btnMinus, btnFullscreenChart, btnHome)

        /// <summary>
        /// Инициализирует панель инструментов графика
        /// Кнопки созданы в Designer - связываем обработчики событий
        /// </summary>
        private void InitializeToolbar()
        {
            // Connect event handlers for toolbar buttons (created in Designer)
            if (btnZoom != null) btnZoom.Click += BtnZoom_Click;
            if (btnPan != null) btnPan.Click += BtnPan_Click;
            if (btnPlus != null) btnPlus.Click += BtnZoomIn_Click;
            if (btnMinus != null) btnMinus.Click += BtnZoomOut_Click;
            if (btnFullscreenChart != null) btnFullscreenChart.Click += BtnFitToScreen_Click;
            if (btnHome != null) btnHome.Click += BtnHome_Click;
        }

        // Event handler for btnZoom (redirects to BtnZoomToSelection_Click)
        private void BtnZoom_Click(object? sender, EventArgs e)
        {
            BtnZoomToSelection_Click(sender, e);
        }

        // Buttons are now created in Designer - no need for CreateToolbarButtons() or CreateToolbarButton()

        // =========================
        // Toolbar button handlers
        // =========================

        private void BtnZoomToSelection_Click(object? sender, EventArgs e)
        {
            // Toggle zoom to selection mode
            _isZooming = !_isZooming;
            if (_isZooming)
            {
                _isPanning = false;
                UpdateButtonStates();
                chartPressure.Cursor = Cursors.Cross;
                if (_zoomOverlayPanel != null)
                {
                    _zoomOverlayPanel.Visible = true;
                    _zoomOverlayPanel.BringToFront();
                }
            }
            else
            {
                chartPressure.Cursor = Cursors.Default;
                if (_zoomOverlayPanel != null)
                {
                    _zoomOverlayPanel.Visible = false;
                }
                _zoomSelectionRect = null;
            }
        }

        private void BtnPan_Click(object? sender, EventArgs e)
        {
            // Toggle pan mode
            _isPanning = !_isPanning;
            if (_isPanning)
            {
                _isZooming = false;
                UpdateButtonStates();
                chartPressure.Cursor = Cursors.Hand;
                
                // Visual feedback: show status (optional - can add status bar message later)
                // For now, button highlight is enough
            }
            else
            {
                chartPressure.Cursor = Cursors.Default;
                UpdateButtonStates();
            }
        }

        private void BtnZoomIn_Click(object? sender, EventArgs e)
        {
            ZoomChart(1.2); // Zoom in by 20%
        }

        private void BtnZoomOut_Click(object? sender, EventArgs e)
        {
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

        private void UpdateButtonStates()
        {
            // Update button states using buttons from Designer
            if (btnPan != null)
            {
                btnPan.BackColor = _isPanning 
                    ? Color.FromArgb(60, 100, 150) 
                    : Color.FromArgb(30, 33, 40);
            }

            if (btnZoom != null)
            {
                btnZoom.BackColor = _isZooming 
                    ? Color.FromArgb(60, 100, 150) 
                    : Color.FromArgb(30, 33, 40);
            }
        }

        // =========================
        // Zoom and Pan functionality
        // =========================

        private void ZoomChart(double factor)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            double xMin = xAxis.MinLimit ?? 0;
            double xMax = xAxis.MaxLimit ?? _timeWindowSeconds;
            double xCenter = (xMin + xMax) / 2;
            double xRange = xMax - xMin;
            double newXRange = xRange / factor;

            // Apply zoom limits (expert recommendation: min 1s, max full data range)
            double minXRange = 1.0; // Minimum 1 second
            double maxXRange = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds; // Maximum: all available data

            if (newXRange < minXRange)
            {
                newXRange = minXRange;
            }
            else if (newXRange > maxXRange)
            {
                newXRange = maxXRange;
            }

            // Ensure we don't go beyond data boundaries
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

            double yMin = yAxis.MinLimit ?? 0;
            double yMax = yAxis.MaxLimit ?? double.MaxValue;
            if (yMax == double.MaxValue)
            {
                // Auto-calculate Y range from data
                if (_seriesCurrent.Count > 0)
                {
                    var values = _seriesCurrent.Select(p => p.Y.Value).ToList();
                    yMin = values.Min();
                    yMax = values.Max();
                    double yPadding = (yMax - yMin) * 0.1;
                    yMin -= yPadding;
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

            // Ensure Y doesn't go below 0
            double newYMin = yCenter - newYRange / 2;
            double newYMax = yCenter + newYRange / 2;
            if (newYMin < 0)
            {
                newYMin = 0;
                newYMax = newYRange;
            }

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

            // Fit X axis to time window (show all data in current time window)
            double xMax = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds;
            double xMin = Math.Max(0, xMax - _timeWindowSeconds);
            
            // If we have less data than time window, start from 0
            if (_timeSeconds < _timeWindowSeconds)
            {
                xMin = 0;
                xMax = _timeWindowSeconds;
            }
            
            xAxis.MinLimit = xMin;
            xAxis.MaxLimit = xMax;

            // Fit Y axis to data range in visible X range
            if (_seriesCurrent.Count > 0)
            {
                // Get values in the visible X range
                var valuesInRange = _seriesCurrent
                    .Where(p => p.X.Value >= xMin && p.X.Value <= xMax)
                    .Select(p => p.Y.Value)
                    .ToList();
                
                if (valuesInRange.Count > 0)
                {
                    double yMin = valuesInRange.Min();
                    double yMax = valuesInRange.Max();
                    double yPadding = (yMax - yMin) * 0.1; // 10% padding
                    yAxis.MinLimit = Math.Max(0, yMin - yPadding);
                    yAxis.MaxLimit = yMax + yPadding;
                }
                else
                {
                    // If no points in range, use all data
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
                yAxis.MaxLimit = null; // Auto mode
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

            _isPanning = false;
            _isZooming = false;
            UpdateButtonStates();
            chartPressure.Cursor = Cursors.Default;

            ApplyThresholdLines();
            UpdateTargetLine();
        }

        // =========================
        // Mouse wheel zoom
        // =========================

        private void SetupMouseWheelZoom()
        {
            chartPressure.MouseWheel += ChartPressure_MouseWheel;
            // Note: MouseDown, MouseMove, MouseUp are handled separately to avoid conflicts
            // with existing cursor tracking in GraphForm.cs
            
            // Create overlay panel for zoom selection rectangle
            CreateZoomOverlayPanel();
        }
        
        /// <summary>
        /// Creates an overlay panel for drawing zoom selection rectangle
        /// </summary>
        private void CreateZoomOverlayPanel()
        {
            _zoomOverlayPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = false,
                Cursor = Cursors.Cross
            };
            
            // Make overlay panel not intercept mouse events when not in zoom mode
            // But allow events to pass through to chart when needed
            _zoomOverlayPanel.MouseDown += (s, e) =>
            {
                // Forward mouse events to chart when in zoom mode
                if (_isZooming)
                {
                    HandleMouseDownForZoomPan(e);
                }
            };
            
            _zoomOverlayPanel.MouseMove += (s, e) =>
            {
                if (_isZooming)
                {
                    HandleMouseMoveForZoomPan(e);
                }
            };
            
            _zoomOverlayPanel.MouseUp += (s, e) =>
            {
                if (_isZooming)
                {
                    HandleMouseUpForZoomPan(e);
                }
            };
            
            // Draw zoom selection rectangle on Paint
            // Investing.com/TradingView style: bright, clear selection rectangle
            _zoomOverlayPanel.Paint += (s, e) =>
            {
                if (_zoomSelectionRect.HasValue && _isZooming)
                {
                    var rect = _zoomSelectionRect.Value;
                    
                    // Investing.com style: Bright blue border (more visible)
                    // 2px solid line (not dashed for better visibility)
                    using (var pen = new Pen(Color.FromArgb(255, 0, 150, 255), 2))
                    {
                        // Solid line for better visibility (like Investing.com)
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                    
                    // Semi-transparent fill (30% alpha for better visibility)
                    using (var brush = new SolidBrush(Color.FromArgb(77, 0, 150, 255))) // 30% alpha
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }
                    
                    // Optional: Draw corner handles (like TradingView)
                    int handleSize = 6;
                    using (var handleBrush = new SolidBrush(Color.FromArgb(255, 0, 150, 255)))
                    {
                        // Top-left corner
                        e.Graphics.FillRectangle(handleBrush, rect.Left - handleSize/2, rect.Top - handleSize/2, handleSize, handleSize);
                        // Top-right corner
                        e.Graphics.FillRectangle(handleBrush, rect.Right - handleSize/2, rect.Top - handleSize/2, handleSize, handleSize);
                        // Bottom-left corner
                        e.Graphics.FillRectangle(handleBrush, rect.Left - handleSize/2, rect.Bottom - handleSize/2, handleSize, handleSize);
                        // Bottom-right corner
                        e.Graphics.FillRectangle(handleBrush, rect.Right - handleSize/2, rect.Bottom - handleSize/2, handleSize, handleSize);
                    }
                }
            };
            
            // Add overlay to chart (behind cursor panel but above chart)
            chartPressure.Controls.Add(_zoomOverlayPanel);
            // Position: after chart but before cursor panel
            // SendToBack would put it behind chart, so we need to position it correctly
            _zoomOverlayPanel.BringToFront();
            // Then send cursor panel to front
            if (_cursorInfoPanel != null)
            {
                _cursorInfoPanel.BringToFront();
            }
        }

        private void ChartPressure_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Convert mouse position to chart coordinates (zoom center)
            double xMin = xAxis.MinLimit ?? 0;
            double xMax = xAxis.MaxLimit ?? _timeWindowSeconds;
            double xRange = xMax - xMin;
            double xValue = xMin + (e.X / (double)chartPressure.Width) * xRange;

            // Get current Y axis range
            // If Y axis is in auto mode (MaxLimit == null), calculate from visible data
            double yMin = yAxis.MinLimit ?? 0;
            double yMax = yAxis.MaxLimit ?? double.MaxValue;
            
            if (yMax == double.MaxValue)
            {
                // Y axis is in auto mode - calculate from visible data points
                if (_seriesCurrent.Count > 0)
                {
                    // Get points in visible X range
                    var visiblePoints = _seriesCurrent
                        .Where(p => p.X.Value >= xMin && p.X.Value <= xMax)
                        .Select(p => p.Y.Value)
                        .ToList();
                    
                    if (visiblePoints.Count > 0)
                    {
                        yMin = visiblePoints.Min();
                        yMax = visiblePoints.Max();
                        double yPadding = (yMax - yMin) * 0.1;
                        yMin = Math.Max(0, yMin - yPadding);
                        yMax = yMax + yPadding;
                    }
                    else
                    {
                        // No points in visible range, use all data
                        var allValues = _seriesCurrent.Select(p => p.Y.Value).ToList();
                        yMin = allValues.Min();
                        yMax = allValues.Max();
                        double yPadding = (yMax - yMin) * 0.1;
                        yMin = Math.Max(0, yMin - yPadding);
                        yMax = yMax + yPadding;
                    }
                }
                else
                {
                    yMin = 0;
                    yMax = 100;
                }
            }

            double yRange = yMax - yMin;
            // Convert mouse Y position to chart Y coordinate
            // Y axis is inverted: top of chart (e.Y = 0) = yMax, bottom (e.Y = Height) = yMin
            double yValue = yMax - (e.Y / (double)chartPressure.Height) * yRange;

            // Zoom factor: 1.1x per step (smooth zoom as per expert recommendation)
            double zoomFactor = e.Delta > 0 ? 1.1 : 1.0 / 1.1;

            // Calculate new ranges centered on cursor position
            double newXRange = xRange / zoomFactor;
            double newYRange = yRange / zoomFactor;

            // Calculate new limits centered on cursor
            double newXMin = xValue - newXRange / 2;
            double newXMax = xValue + newXRange / 2;
            double newYMin = yValue - newYRange / 2;
            double newYMax = yValue + newYRange / 2;

            // Apply zoom limits (expert recommendation: min 1s, max full data range)
            double minXRange = 1.0; // Minimum 1 second
            double maxXRange = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds; // Maximum: all available data

            if (newXRange < minXRange)
            {
                newXRange = minXRange;
                newXMin = xValue - newXRange / 2;
                newXMax = xValue + newXRange / 2;
            }
            else if (newXRange > maxXRange)
            {
                newXRange = maxXRange;
                newXMin = Math.Max(0, _timeSeconds - newXRange);
                newXMax = _timeSeconds;
            }

            // Ensure we don't go beyond data boundaries
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

            // Apply Y limits
            // Ensure we don't go below 0
            if (newYMin < 0)
            {
                newYMin = 0;
                newYMax = newYRange;
            }
            
            // Calculate min/max Y from all data for range limits
            double minYValue = 0;
            double maxYValue = 100;
            if (_seriesCurrent.Count > 0)
            {
                var allYValues = _seriesCurrent.Select(p => p.Y.Value).ToList();
                minYValue = Math.Min(0, allYValues.Min() - Math.Abs(allYValues.Min()) * 0.1);
                maxYValue = allYValues.Max() + Math.Abs(allYValues.Max()) * 0.1;
            }
            
            // Ensure Y range doesn't exceed data bounds (with padding)
            if (newYMin < minYValue)
            {
                double diff = minYValue - newYMin;
                newYMin = minYValue;
                newYMax += diff;
            }
            if (newYMax > maxYValue)
            {
                double diff = newYMax - maxYValue;
                newYMax = maxYValue;
                newYMin = Math.Max(0, newYMin - diff);
            }
            
            // Ensure minimum Y range (prevent zooming too much)
            double minYRange = (maxYValue - minYValue) * 0.01; // At least 1% of full range
            if (newYRange < minYRange)
            {
                newYRange = minYRange;
                newYMin = yValue - newYRange / 2;
                newYMax = yValue + newYRange / 2;
                // Re-apply bounds
                if (newYMin < minYValue)
                {
                    newYMin = minYValue;
                    newYMax = minYValue + newYRange;
                }
                if (newYMax > maxYValue)
                {
                    newYMax = maxYValue;
                    newYMin = maxYValue - newYRange;
                }
            }

            // Apply the zoom
            xAxis.MinLimit = newXMin;
            xAxis.MaxLimit = newXMax;
            yAxis.MinLimit = newYMin;
            yAxis.MaxLimit = newYMax;

            ApplyThresholdLines();
            UpdateTargetLine();
        }

        // These handlers are called from GraphForm.cs MouseDown/MouseMove/MouseUp
        // to integrate with existing cursor tracking
        internal void HandleMouseDownForZoomPan(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isZooming)
                {
                    // Start new selection (Investing.com pattern: click to start, drag to select)
                    _zoomStartPoint = e.Location;
                    _zoomSelectionRect = null;
                    
                    // Clear previous selection visually
                    if (_zoomOverlayPanel != null)
                    {
                        _zoomOverlayPanel.Invalidate();
                    }
                }
                else if (_isPanning)
                {
                    // Start panning (Investing.com pattern: click and drag to pan)
                    _lastMousePosition = e.Location;
                }
            }
            else if (e.Button == MouseButtons.Right && _isZooming)
            {
                // Right-click cancels selection (Investing.com pattern)
                _zoomSelectionRect = null;
                _zoomStartPoint = Point.Empty;
                if (_zoomOverlayPanel != null)
                {
                    _zoomOverlayPanel.Invalidate();
                }
            }
        }

        internal void HandleMouseMoveForZoomPan(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isZooming && _zoomStartPoint != Point.Empty)
                {
                    // Draw zoom selection rectangle (Investing.com/TradingView style)
                    // Expert: Zoom only on X axis, but show full rectangle for visual feedback
                    int x = Math.Min(_zoomStartPoint.X, e.X);
                    int y = Math.Min(_zoomStartPoint.Y, e.Y);
                    int width = Math.Abs(e.X - _zoomStartPoint.X);
                    int height = Math.Abs(e.Y - _zoomStartPoint.Y);
                    _zoomSelectionRect = new Rectangle(x, y, width, height);
                    
                    // Invalidate overlay panel to redraw selection rectangle in real-time
                    if (_zoomOverlayPanel != null)
                    {
                        _zoomOverlayPanel.Invalidate();
                    }
                }
                else if (_isPanning && _lastMousePosition != Point.Empty)
                {
                    // Smooth panning (Investing.com style: 1:1 pixel movement)
                    int deltaX = e.X - _lastMousePosition.X;
                    int deltaY = e.Y - _lastMousePosition.Y;
                    
                    // Only pan if movement is significant (avoid jitter)
                    if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
                    {
                        PanChart(deltaX, deltaY);
                        _lastMousePosition = e.Location;
                    }
                }
            }
            else if (_isZooming && _zoomStartPoint != Point.Empty)
            {
                // Mouse move without button pressed - update selection rectangle
                // (for visual feedback even when not dragging)
                int x = Math.Min(_zoomStartPoint.X, e.X);
                int y = Math.Min(_zoomStartPoint.Y, e.Y);
                int width = Math.Abs(e.X - _zoomStartPoint.X);
                int height = Math.Abs(e.Y - _zoomStartPoint.Y);
                _zoomSelectionRect = new Rectangle(x, y, width, height);
                
                if (_zoomOverlayPanel != null)
                {
                    _zoomOverlayPanel.Invalidate();
                }
            }
        }

        internal void HandleMouseUpForZoomPan(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isZooming && _zoomSelectionRect.HasValue)
                {
                    // Check if selection is valid (minimum size)
                    var rect = _zoomSelectionRect.Value;
                    if (rect.Width > 5 && rect.Height > 5) // Minimum selection size
                    {
                        ZoomToSelection(rect);
                    }
                    
                    // Investing.com/TradingView pattern: Auto-disable zoom mode after selection
                    _isZooming = false;
                    UpdateButtonStates();
                    chartPressure.Cursor = Cursors.Default;
                    if (_zoomOverlayPanel != null)
                    {
                        _zoomOverlayPanel.Visible = false;
                    }
                    _zoomSelectionRect = null;
                }
                else if (_isZooming && _zoomStartPoint != Point.Empty)
                {
                    // Click without drag - cancel selection (Investing.com pattern)
                    _zoomSelectionRect = null;
                    if (_zoomOverlayPanel != null)
                    {
                        _zoomOverlayPanel.Invalidate();
                    }
                }
                
                _zoomStartPoint = Point.Empty;
                _lastMousePosition = Point.Empty;
            }
        }

        private void PanChart(int deltaX, int deltaY)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Get current visible range
            double xMin = xAxis.MinLimit ?? 0;
            double xMax = xAxis.MaxLimit ?? _timeWindowSeconds;
            double xRange = xMax - xMin;
            
            double yMin = yAxis.MinLimit ?? 0;
            double yMax = yAxis.MaxLimit ?? double.MaxValue;
            if (yMax == double.MaxValue && _seriesCurrent.Count > 0)
            {
                // If Y is in auto mode, calculate from visible data
                var visiblePoints = _seriesCurrent
                    .Where(p => p.X.Value >= xMin && p.X.Value <= xMax)
                    .Select(p => p.Y.Value)
                    .ToList();
                if (visiblePoints.Count > 0)
                {
                    yMin = visiblePoints.Min();
                    yMax = visiblePoints.Max();
                    double yPadding = (yMax - yMin) * 0.1;
                    yMin = Math.Max(0, yMin - yPadding);
                    yMax = yMax + yPadding;
                }
                else
                {
                    var allValues = _seriesCurrent.Select(p => p.Y.Value).ToList();
                    yMin = allValues.Min();
                    yMax = allValues.Max();
                    double yPadding = (yMax - yMin) * 0.1;
                    yMin = Math.Max(0, yMin - yPadding);
                    yMax = yMax + yPadding;
                }
            }
            if (yMax == double.MaxValue) yMax = 100;

            double yRange = yMax - yMin;

            // Calculate pan delta: 1:1 ratio (pixel = pixel)
            // Convert pixel movement to chart coordinate movement
            double xDelta = -(deltaX / (double)chartPressure.Width) * xRange;
            double yDelta = (deltaY / (double)chartPressure.Height) * yRange; // Y is inverted

            // Calculate new limits
            double newXMin = xMin + xDelta;
            double newXMax = xMax + xDelta;
            double newYMin = yMin + yDelta;
            double newYMax = yMax + yDelta;

            // Define data boundaries (all available data)
            // X boundaries: from 0 to _timeSeconds (all available time data)
            double dataXMin = 0;
            double dataXMax = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds;
            
            // Y boundaries: from min to max of all data (with padding)
            double dataYMin = 0;
            double dataYMax = 100;
            if (_seriesCurrent.Count > 0)
            {
                var allYValues = _seriesCurrent.Select(p => p.Y.Value).ToList();
                dataYMin = Math.Min(0, allYValues.Min() - Math.Abs(allYValues.Min()) * 0.1);
                dataYMax = allYValues.Max() + Math.Abs(allYValues.Max()) * 0.1;
            }

            // Apply boundaries: don't allow panning beyond data limits
            // X axis boundaries
            if (newXMin < dataXMin)
            {
                double diff = dataXMin - newXMin;
                newXMin = dataXMin;
                newXMax -= diff;
            }
            if (newXMax > dataXMax)
            {
                double diff = newXMax - dataXMax;
                newXMax = dataXMax;
                newXMin += diff;
            }

            // Y axis boundaries
            if (newYMin < dataYMin)
            {
                double diff = dataYMin - newYMin;
                newYMin = dataYMin;
                newYMax -= diff;
            }
            if (newYMax > dataYMax)
            {
                double diff = newYMax - dataYMax;
                newYMax = dataYMax;
                newYMin += diff;
            }

            // Ensure we don't pan beyond the visible range (if range is larger than data)
            if (xRange > (dataXMax - dataXMin))
            {
                // If visible range is larger than data, center it
                if (newXMin < dataXMin) newXMin = dataXMin;
                if (newXMax > dataXMax) newXMax = dataXMax;
            }

            if (yRange > (dataYMax - dataYMin))
            {
                // If visible range is larger than data, center it
                if (newYMin < dataYMin) newYMin = dataYMin;
                if (newYMax > dataYMax) newYMax = dataYMax;
            }

            // Apply the pan
            xAxis.MinLimit = newXMin;
            xAxis.MaxLimit = newXMax;
            yAxis.MinLimit = newYMin;
            yAxis.MaxLimit = newYMax;

            ApplyThresholdLines();
            UpdateTargetLine();
        }

        private void ZoomToSelection(Rectangle selection)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Expert decision: Zoom only on X axis (time), Y automatically adjusts
            // Convert screen coordinates to chart X coordinates
            double xMin = xAxis.MinLimit ?? 0;
            double xMax = xAxis.MaxLimit ?? _timeWindowSeconds;
            double xRange = xMax - xMin;

            // Calculate selected X range (only horizontal selection matters)
            double selXMin = xMin + (selection.Left / (double)chartPressure.Width) * xRange;
            double selXMax = xMin + (selection.Right / (double)chartPressure.Width) * xRange;

            // Ensure minimum 1 second (expert recommendation)
            if (selXMax - selXMin < 1.0)
            {
                double center = (selXMin + selXMax) / 2;
                selXMin = center - 0.5;
                selXMax = center + 0.5;
            }

            // Ensure we don't go beyond data boundaries
            if (selXMin < 0) selXMin = 0;
            if (selXMax > _timeSeconds && _timeSeconds > 0) selXMax = _timeSeconds;

            // Y axis: automatically adjust to min/max of data in selected X range
            double selYMin = 0;
            double selYMax = 100;

            if (_seriesCurrent.Count > 0)
            {
                // Find all points in the selected X range
                var pointsInRange = _seriesCurrent
                    .Where(p => p.X.Value >= selXMin && p.X.Value <= selXMax)
                    .Select(p => p.Y.Value)
                    .ToList();

                if (pointsInRange.Count > 0)
                {
                    selYMin = pointsInRange.Min();
                    selYMax = pointsInRange.Max();
                    double yPadding = (selYMax - selYMin) * 0.1; // 10% padding
                    selYMin = Math.Max(0, selYMin - yPadding);
                    selYMax = selYMax + yPadding;
                }
                else
                {
                    // If no points in range, use all data
                    var allValues = _seriesCurrent.Select(p => p.Y.Value).ToList();
                    selYMin = allValues.Min();
                    selYMax = allValues.Max();
                    double yPadding = (selYMax - selYMin) * 0.1;
                    selYMin = Math.Max(0, selYMin - yPadding);
                    selYMax = selYMax + yPadding;
                }
            }

            // Apply the zoom
            xAxis.MinLimit = selXMin;
            xAxis.MaxLimit = selXMax;
            yAxis.MinLimit = selYMin;
            yAxis.MaxLimit = selYMax;

            // Exit zoom mode after selection (already handled in HandleMouseUpForZoomPan)
            // But ensure state is correct
            if (_isZooming)
            {
                _isZooming = false;
                UpdateButtonStates();
                chartPressure.Cursor = Cursors.Default;
                if (_zoomOverlayPanel != null)
                {
                    _zoomOverlayPanel.Visible = false;
                }
            }

            ApplyThresholdLines();
            UpdateTargetLine();
        }

        // =========================
        // Export chart as image
        // =========================

        private void ExportChartAsImage()
        {
            try
            {
                // 1. Определяем папку для сохранения
                string? targetDirectory = null;

                // Если сессия имеет CSV файл, используем его папку
                if (_dataStore?.CsvPath != null)
                {
                    targetDirectory = Path.GetDirectoryName(_dataStore.CsvPath);
                }

                // Если папки нет, показываем диалог выбора
                if (string.IsNullOrEmpty(targetDirectory) || !Directory.Exists(targetDirectory))
                {
                    using var folderDialog = new FolderBrowserDialog
                    {
                        Description = "Select folder for graph export",
                        ShowNewFolderButton = true
                    };

                    if (folderDialog.ShowDialog() != DialogResult.OK)
                    {
                        return; // Пользователь отменил
                    }

                    targetDirectory = folderDialog.SelectedPath;
                }

                // 2. Генерируем базовое имя файла (как у сессий)
                string baseFileName = $"graph_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";
                string filePath = Path.Combine(targetDirectory, $"{baseFileName}.png");

                // 3. Проверяем существование и добавляем нумерацию если нужно
                int counter = 1;
                while (File.Exists(filePath))
                {
                    string numberedFileName = $"{baseFileName}_{counter:D3}.png";
                    filePath = Path.Combine(targetDirectory, numberedFileName);
                    counter++;
                }

                // 4. Экспортируем график в текущем разрешении
                var chartBounds = chartPressure.Bounds;
                using var bitmap = new Bitmap(chartBounds.Width, chartBounds.Height);
                chartPressure.DrawToBitmap(bitmap, chartBounds);

                // 5. Сохраняем как PNG
                bitmap.Save(filePath, ImageFormat.Png);

                // 6. Показываем уведомление об успешном сохранении
                MessageBox.Show(
                    $"Graph exported successfully!\n\nFile: {Path.GetFileName(filePath)}\nLocation: {targetDirectory}",
                    "Export Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error exporting graph:\n{ex.Message}",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Cancels zoom/pan mode (called from GraphForm on Escape key)
        /// Expert recommendation: Allow canceling current mode with Escape
        /// </summary>
        private void CancelZoomPanMode()
        {
            if (_isZooming)
            {
                _isZooming = false;
                UpdateButtonStates();
                chartPressure.Cursor = Cursors.Default;
                if (_zoomOverlayPanel != null)
                {
                    _zoomOverlayPanel.Visible = false;
                }
                _zoomSelectionRect = null;
            }
            
            if (_isPanning)
            {
                _isPanning = false;
                UpdateButtonStates();
                chartPressure.Cursor = Cursors.Default;
            }
        }
    }
}

