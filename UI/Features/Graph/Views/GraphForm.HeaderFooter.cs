using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO.Ports;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm
    {
        // Note: UI elements are declared in GraphForm.Designer.cs
        
        // Header timer
        private System.Windows.Forms.Timer? _headerTimer;

        // COM port and connection info
        private string? _comPortName;
        private int? _baudRate;

        /// <summary>
        /// Устанавливает информацию о COM порте для отображения в Header
        /// </summary>
        public void SetConnectionInfo(string? comPort, int? baudRate)
        {
            _comPortName = comPort;
            _baudRate = baudRate;
            UpdateHeaderConnectionInfo();
        }

        private void ConnectionStatusPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (_connectionStatusPanel == null) return;

            var panel = _connectionStatusPanel;
            
            // Draw border
            using (var pen = new Pen(Color.FromArgb(100, 200, 180), 1)) // Light teal border
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }

            // Draw green circle
            using (var brush = new SolidBrush(Color.FromArgb(76, 175, 80))) // Green
            {
                e.Graphics.FillEllipse(brush, 8, 8, 10, 10);
            }

            // Draw connection text
            string connectionText;
            if (!string.IsNullOrEmpty(_comPortName) && _baudRate.HasValue)
            {
                connectionText = $"Connected {_comPortName} @ {_baudRate.Value}";
            }
            else
            {
                connectionText = "Not Connected";
            }

            using (var brush = new SolidBrush(Color.FromArgb(100, 200, 180))) // Light teal text
            using (var font = new Font("Segoe UI", 9f))
            {
                e.Graphics.DrawString(connectionText, font, brush, 22, 6);
            }
        }

        private void UpdateHeaderConnectionInfo()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateHeaderConnectionInfo));
                return;
            }

            // Invalidate connection panel to trigger repaint
            if (_connectionStatusPanel != null)
            {
                _connectionStatusPanel.Invalidate();
            }
        }

        private void UpdateHeaderSessionTime()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateHeaderSessionTime));
                return;
            }

            if (lblSessionTime != null && _dataStore != null && _dataStore.IsRunning)
            {
                TimeSpan duration = DateTime.Now - _dataStore.SessionStart;
                int hours = (int)duration.TotalHours;
                int minutes = duration.Minutes;
                int seconds = duration.Seconds;
                
                // Format: "Session: 00:09:23"
                lblSessionTime.Text = $"Session: {hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            else if (lblSessionTime != null)
            {
                lblSessionTime.Text = "Session: 00:00:00";
            }
        }

        private void InitializeHeaderFooter()
        {
            // Initialize header timer for session time update
            _headerTimer = new System.Windows.Forms.Timer { Interval = 1000 }; // Update every second
            _headerTimer.Tick += (_, __) => UpdateHeaderSessionTime();
            _headerTimer.Start();

            // Setup header panel layout
            SetupHeaderLayout();

            // Setup chart header legend with colored squares
            SetupChartHeaderLegend();

            // Initialize footer with auto-save status
            UpdateFooterAutoSave();
            UpdateFooterStatistics();
        }

        // Checkboxes for legend visibility control
        private CheckBox? _chkLegendTarget;
        private CheckBox? _chkLegendMin;
        private CheckBox? _chkLegendMax;

        // Header connection status panel
        private Panel? _connectionStatusPanel;

        private void SetupChartHeaderLegend()
        {
            if (flowLegend == null) return;

            // Store existing labels temporarily
            var existingLabels = new List<Control>();
            if (lblLegendCurrent != null) existingLabels.Add(lblLegendCurrent);
            if (lblLegendTarget != null) existingLabels.Add(lblLegendTarget);
            if (lblLegendMin != null) existingLabels.Add(lblLegendMin);
            if (lblLegendMax != null) existingLabels.Add(lblLegendMax);

            // Clear flowLegend to rebuild in correct order
            flowLegend.Controls.Clear();

            // Update legend labels to show colored squares
            if (lblLegendCurrent != null)
            {
                lblLegendCurrent.Text = "Current";
                lblLegendCurrent.Paint += (s, e) =>
                {
                    using var brush = new SolidBrush(Color.FromArgb(0, 200, 240)); // Light blue
                    e.Graphics.FillRectangle(brush, 0, 2, 12, 12);
                };
                flowLegend.Controls.Add(lblLegendCurrent);
            }

            // Target: Add checkbox before label
            if (lblLegendTarget != null)
            {
                lblLegendTarget.Text = "Target";
                lblLegendTarget.Paint += (s, e) =>
                {
                    using var brush = new SolidBrush(Color.FromArgb(240, 200, 0)); // Yellow
                    e.Graphics.FillRectangle(brush, 0, 2, 12, 12);
                };
                
                // Create checkbox for Target
                _chkLegendTarget = new CheckBox
                {
                    AutoSize = true,
                    Checked = true, // Visible by default
                    Margin = new Padding(8, 0, 0, 0),
                    Padding = new Padding(0),
                    Size = new Size(15, 15),
                    UseVisualStyleBackColor = false,
                    BackColor = Color.FromArgb(40, 43, 52),
                    ForeColor = Color.White
                };
                _chkLegendTarget.CheckedChanged += (s, e) =>
                {
                    if (_lineSeriesTarget != null)
                    {
                        _lineSeriesTarget.IsVisible = _chkLegendTarget.Checked;
                    }
                };
                
                flowLegend.Controls.Add(_chkLegendTarget);
                flowLegend.Controls.Add(lblLegendTarget);
            }

            // Min: Add checkbox before label
            if (lblLegendMin != null)
            {
                lblLegendMin.Text = "Min";
                lblLegendMin.Paint += (s, e) =>
                {
                    using var brush = new SolidBrush(Color.FromArgb(76, 175, 80)); // Green
                    e.Graphics.FillRectangle(brush, 0, 2, 12, 12);
                };
                
                // Create checkbox for Min
                _chkLegendMin = new CheckBox
                {
                    AutoSize = true,
                    Checked = true, // Visible by default
                    Margin = new Padding(8, 0, 0, 0),
                    Padding = new Padding(0),
                    Size = new Size(15, 15),
                    UseVisualStyleBackColor = false,
                    BackColor = Color.FromArgb(40, 43, 52),
                    ForeColor = Color.White
                };
                _chkLegendMin.CheckedChanged += (s, e) =>
                {
                    if (_lineSeriesMin != null)
                    {
                        _lineSeriesMin.IsVisible = _chkLegendMin.Checked;
                    }
                };
                
                flowLegend.Controls.Add(_chkLegendMin);
                flowLegend.Controls.Add(lblLegendMin);
            }

            // Max: Add checkbox before label
            if (lblLegendMax != null)
            {
                lblLegendMax.Text = "Max";
                lblLegendMax.Paint += (s, e) =>
                {
                    using var brush = new SolidBrush(Color.FromArgb(244, 67, 54)); // Red
                    e.Graphics.FillRectangle(brush, 0, 2, 12, 12);
                };
                
                // Create checkbox for Max
                _chkLegendMax = new CheckBox
                {
                    AutoSize = true,
                    Checked = true, // Visible by default
                    Margin = new Padding(8, 0, 0, 0),
                    Padding = new Padding(0),
                    Size = new Size(15, 15),
                    UseVisualStyleBackColor = false,
                    BackColor = Color.FromArgb(40, 43, 52),
                    ForeColor = Color.White
                };
                _chkLegendMax.CheckedChanged += (s, e) =>
                {
                    if (_lineSeriesMax != null)
                    {
                        _lineSeriesMax.IsVisible = _chkLegendMax.Checked;
                    }
                };
                
                flowLegend.Controls.Add(_chkLegendMax);
                flowLegend.Controls.Add(lblLegendMax);
            }
        }

        private void SetupHeaderLayout()
        {
            if (panelHeader == null) return;

            // Setup app icon paint event
            if (appIcon != null)
            {
                appIcon.Paint += (s, e) =>
                {
                    // Draw three vertical bars
                    using (var greenBrush = new SolidBrush(Color.FromArgb(76, 175, 80))) // Green
                    using (var blueBrush = new SolidBrush(Color.FromArgb(0, 200, 240))) // Light blue
                    using (var pinkBrush = new SolidBrush(Color.FromArgb(244, 67, 54))) // Red/Pink
                    {
                        e.Graphics.FillRectangle(greenBrush, 2, 4, 4, 12);
                        e.Graphics.FillRectangle(blueBrush, 8, 2, 4, 14);
                        e.Graphics.FillRectangle(pinkBrush, 14, 6, 4, 10);
                    }
                };
            }

            // Connection status panel (created dynamically as it needs Paint event)
            if (headerLeftFlowPanel != null && _connectionStatusPanel == null)
            {
                _connectionStatusPanel = new Panel
                {
                    Size = new Size(200, 28),
                    Margin = new Padding(0, 11, 12, 0),
                    BackColor = Color.FromArgb(26, 61, 53), // #1A3D35
                    Padding = new Padding(8, 0, 8, 0)
                };
                _connectionStatusPanel.Paint += ConnectionStatusPanel_Paint;
                
                // Insert after lblAppTitle
                int insertIndex = headerLeftFlowPanel.Controls.IndexOf(lblAppTitle) + 1;
                headerLeftFlowPanel.Controls.Add(_connectionStatusPanel);
                headerLeftFlowPanel.Controls.SetChildIndex(_connectionStatusPanel, insertIndex);
            }

            // Ensure buttons are visible
            if (btnPause != null)
            {
                btnPause.Visible = true;
            }
            if (btnExport != null)
            {
                btnExport.Visible = true;
            }

            // Setup button paint events and click handlers
            if (btnPause != null)
            {
                // Remove existing handlers to avoid duplicates
                btnPause.Paint -= BtnPause_Paint;
                btnPause.Click -= btnPause_Click;
                
                // Add handlers
                btnPause.Paint += BtnPause_Paint;
                btnPause.Click += btnPause_Click;
            }

            if (btnExport != null)
            {
                // Remove existing handlers to avoid duplicates
                btnExport.Paint -= BtnExport_Paint;
                btnExport.Click -= btnExport_Click;
                
                // Add handlers
                btnExport.Paint += BtnExport_Paint;
                btnExport.Click += btnExport_Click;
            }
        }

        // Button paint event handlers
        private void BtnPause_Paint(object? sender, PaintEventArgs e)
        {
            if (btnPause == null) return;
            
            // Draw pause icon (two vertical bars)
            using (var brush = new SolidBrush(btnPause.ForeColor))
            {
                e.Graphics.FillRectangle(brush, 12, 8, 3, 12);
                e.Graphics.FillRectangle(brush, 18, 8, 3, 12);
            }

            // Draw "Pause" text
            using (var font = new Font("Segoe UI", 9f))
            using (var brush = new SolidBrush(btnPause.ForeColor))
            {
                e.Graphics.DrawString("Pause", font, brush, 28, 6);
            }
        }

        private void BtnExport_Paint(object? sender, PaintEventArgs e)
        {
            if (btnExport == null) return;
            
            // Draw document icon (blue rectangle with red arrow)
            using (var blueBrush = new SolidBrush(Color.FromArgb(66, 133, 244))) // Blue
            using (var redBrush = new SolidBrush(Color.FromArgb(244, 67, 54))) // Red
            {
                // Document rectangle
                e.Graphics.FillRectangle(blueBrush, 12, 6, 12, 16);
                
                // Red arrow pointing up-right
                var arrowPoints = new Point[]
                {
                    new Point(22, 8),
                    new Point(24, 8),
                    new Point(24, 10),
                    new Point(26, 10),
                    new Point(24, 12),
                    new Point(22, 10)
                };
                e.Graphics.FillPolygon(redBrush, arrowPoints);
            }

            // Draw "Export" text
            using (var font = new Font("Segoe UI", 9f))
            using (var brush = new SolidBrush(btnExport.ForeColor))
            {
                e.Graphics.DrawString("Export", font, brush, 28, 6);
            }
        }


        private void ResetGraph()
        {
            if (MessageBox.Show("Reset all graph data? This will clear the current session data from the graph.", 
                "Reset Graph", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _seriesCurrent.Clear();
                _timeSeconds = 0;
                _lastTargetValue = null;
                ApplyTimeWindow(forceTrim: false);
                CalculateAndUpdateStatistics();
                UpdateLiveStatus(0, null, "PSIG", false, 0);
            }
        }

        private void UpdateFooterAutoSave()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateFooterAutoSave));
                return;
            }

            // TODO: Get auto-save status from main form
            // For now, show placeholder
            if (lblAutoSaveStatus != null)
            {
                lblAutoSaveStatus.Text = "Auto-save • Enabled";
                lblAutoSaveStatus.ForeColor = Color.Green;
            }
        }

        private void UpdateFooterStatistics()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateFooterStatistics));
                return;
            }

            var points = _dataStore?.Points;
            if (points == null || points.Count == 0)
            {
                if (lblFooterMin != null) lblFooterMin.Text = "Min: 0.00";
                if (lblFooterMax != null) lblFooterMax.Text = "Max: 0.00";
                if (lblFooterAvg != null) lblFooterAvg.Text = "Avg: 0.00";
                if (lblFooterPoints != null) lblFooterPoints.Text = "Points: 0";
                return;
            }

            double min = points.Min(p => p.Current);
            double max = points.Max(p => p.Current);
            double avg = points.Average(p => p.Current);

            if (lblFooterMin != null) lblFooterMin.Text = $"Min: {min:F2}";
            if (lblFooterMax != null) lblFooterMax.Text = $"Max: {max:F2}";
            if (lblFooterAvg != null) lblFooterAvg.Text = $"Avg: {avg:F2}";
            if (lblFooterPoints != null) lblFooterPoints.Text = $"Points: {points.Count}";
        }

        // Pause handler delegate
        private Action? _pauseHandler;
        
        /// <summary>
        /// Устанавливает обработчик для кнопки Pause
        /// </summary>
        public void SetPauseHandler(Action handler)
        {
            _pauseHandler = handler;
        }
        
        /// <summary>
        /// Обновляет состояние кнопки Pause
        /// </summary>
        public void UpdatePauseState(bool isPaused)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdatePauseState(isPaused)));
                return;
            }
            
            if (btnPause != null)
            {
                btnPause.Text = isPaused ? "▶ Resume" : "II Pause";
            }
        }
        
        private void btnPause_Click(object? sender, EventArgs e)
        {
            _pauseHandler?.Invoke();
        }

        private void btnExport_Click(object? sender, EventArgs e)
        {
            // TODO: Implement export functionality
            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|PNG Files (*.png)|*.png|All Files (*.*)|*.*",
                Title = "Export Graph Data"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: Export data or graph image
                MessageBox.Show($"Export to {saveDialog.FileName} - Not yet implemented", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Target handler delegate
        private Action<double>? _targetHandler;
        private Action<double>? _targetHandlerSilent;
        
        /// <summary>
        /// Устанавливает обработчик для кнопки GO TARGET
        /// </summary>
        public void SetTargetHandler(Action<double> handler)
        {
            _targetHandler = handler;
        }

        /// <summary>
        /// Устанавливает обработчик для GO TO TARGET секции (без подтверждения)
        /// </summary>
        public void SetTargetHandlerSilent(Action<double> handler)
        {
            _targetHandlerSilent = handler;
        }
        
        private void btnGoTarget_Click(object? sender, EventArgs e)
        {
            if (txtTargetValue == null) return;

            if (double.TryParse(txtTargetValue.Text, System.Globalization.NumberStyles.Float, 
                System.Globalization.CultureInfo.InvariantCulture, out double target))
            {
                _targetHandler?.Invoke(target);
            }
            else
            {
                MessageBox.Show("Invalid target value. Please enter a number.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFullscreenHeader_Click(object? sender, EventArgs e)
        {
            // Toggle fullscreen
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
            }
        }

        /// <summary>
        /// Применяет тему (Light/Dark) к форме
        /// </summary>
        public void ApplyTheme(bool isDark)
        {
            _isDarkTheme = isDark;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ApplyTheme(isDark)));
                return;
            }

            if (isDark)
            {
                // Dark theme colors
                BackColor = Color.FromArgb(17, 19, 23);
                if (panelLeft != null) panelLeft.BackColor = Color.FromArgb(21, 23, 28);
                if (panelRight != null) panelRight.BackColor = Color.FromArgb(21, 23, 28);
                if (panelHeader != null) panelHeader.BackColor = Color.FromArgb(21, 23, 28);
                if (panelBottom != null) panelBottom.BackColor = Color.FromArgb(21, 23, 28);
                
                // LIVE STATUS panel - dark background
                if (tlpLiveStatus != null)
                {
                    tlpLiveStatus.BackColor = Color.FromArgb(22, 24, 30);
                }
                
                // SESSION STATS panel - dark background
                if (tlpSessionStats != null)
                {
                    tlpSessionStats.BackColor = Color.Transparent;
                }
                
                // Update text colors for dark theme
                if (lblCurrentPressureLarge != null)
                {
                    lblCurrentPressureLarge.ForeColor = Color.White;
                }
                if (lblCurrentUnit != null)
                {
                    lblCurrentUnit.ForeColor = Color.FromArgb(180, 185, 195);
                }
                if (lblTarget != null)
                {
                    lblTarget.ForeColor = Color.FromArgb(150, 155, 165);
                }
                if (lblDelta != null)
                {
                    lblDelta.ForeColor = Color.FromArgb(150, 155, 165);
                }
                if (lblRate != null)
                {
                    lblRate.ForeColor = Color.FromArgb(150, 155, 165);
                }
                
                // Update SESSION STATS colors for dark theme
                UpdateSessionStatsColors(isDark: true);
                
                // Update ETA and Trend colors for dark theme
                if (lblETA != null) lblETA.ForeColor = Color.FromArgb(150, 155, 165);
                if (lblTrend != null) lblTrend.ForeColor = Color.FromArgb(150, 155, 165);

                if (lblThemeIndicator != null)
                {
                    lblThemeIndicator.Text = "✓ Dark theme";
                    lblThemeIndicator.ForeColor = Color.White;
                }
            }
            else
            {
                // Light theme colors
                BackColor = Color.FromArgb(245, 245, 250);
                if (panelLeft != null) panelLeft.BackColor = Color.FromArgb(250, 250, 255);
                if (panelRight != null) panelRight.BackColor = Color.FromArgb(250, 250, 255);
                if (panelHeader != null) panelHeader.BackColor = Color.FromArgb(250, 250, 255);
                if (panelBottom != null) panelBottom.BackColor = Color.FromArgb(250, 250, 255);
                
                // LIVE STATUS panel - light background
                if (tlpLiveStatus != null)
                {
                    tlpLiveStatus.BackColor = Color.FromArgb(255, 255, 255);
                }
                
                // SESSION STATS panel - light background
                if (tlpSessionStats != null)
                {
                    tlpSessionStats.BackColor = Color.Transparent;
                }
                
                // Update text colors for light theme
                if (lblCurrentPressureLarge != null)
                {
                    lblCurrentPressureLarge.ForeColor = Color.FromArgb(30, 30, 35);
                }
                if (lblCurrentUnit != null)
                {
                    lblCurrentUnit.ForeColor = Color.FromArgb(100, 105, 115);
                }
                if (lblTarget != null)
                {
                    lblTarget.ForeColor = Color.FromArgb(80, 85, 95);
                }
                if (lblDelta != null)
                {
                    lblDelta.ForeColor = Color.FromArgb(80, 85, 95);
                }
                if (lblRate != null)
                {
                    lblRate.ForeColor = Color.FromArgb(80, 85, 95);
                }
                
                // Update SESSION STATS colors for light theme
                UpdateSessionStatsColors(isDark: false);
                
                // Update ETA and Trend colors for light theme
                if (lblETA != null) lblETA.ForeColor = Color.FromArgb(80, 85, 95);
                if (lblTrend != null) lblTrend.ForeColor = Color.FromArgb(80, 85, 95);

                if (lblThemeIndicator != null)
                {
                    lblThemeIndicator.Text = "✓ Light theme";
                    lblThemeIndicator.ForeColor = Color.Black;
                }
            }
            
            // Force repaint of LIVE STATUS panel border
            if (tlpLiveStatus != null)
            {
                tlpLiveStatus.Invalidate();
            }
        }

        /// <summary>
        /// Обновляет цвета элементов SESSION STATS в зависимости от темы
        /// </summary>
        private void UpdateSessionStatsColors(bool isDark)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateSessionStatsColors(isDark)));
                return;
            }

            Color labelColor = isDark ? Color.FromArgb(120, 125, 140) : Color.FromArgb(80, 85, 95);
            Color valueColor = isDark ? Color.White : Color.FromArgb(30, 30, 35);

            // Update labels
            if (lblSessionStatsTitle != null) lblSessionStatsTitle.ForeColor = labelColor;
            if (lblMinLabel != null) lblMinLabel.ForeColor = labelColor;
            if (lblMaxLabel != null) lblMaxLabel.ForeColor = labelColor;
            if (lblAvgLabel != null) lblAvgLabel.ForeColor = labelColor;
            if (lblStdDevLabel != null) lblStdDevLabel.ForeColor = labelColor;
            if (lblPointsLabel != null) lblPointsLabel.ForeColor = labelColor;
            if (lblDurationLabel != null) lblDurationLabel.ForeColor = labelColor;
            if (lblSampleRateLabel != null) lblSampleRateLabel.ForeColor = labelColor;

            // Update values
            if (lblMinValue != null) lblMinValue.ForeColor = valueColor;
            if (lblMaxValue != null) lblMaxValue.ForeColor = valueColor;
            if (lblAvgValue != null) lblAvgValue.ForeColor = valueColor;
            if (lblStdDevValue != null) lblStdDevValue.ForeColor = valueColor;
            if (lblPointsValue != null) lblPointsValue.ForeColor = valueColor;
            if (lblDurationValue != null) lblDurationValue.ForeColor = valueColor;
            if (lblSampleRateValue != null) lblSampleRateValue.ForeColor = valueColor;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _headerTimer?.Stop();
            _headerTimer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}

