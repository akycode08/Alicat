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

        private void UpdateHeaderConnectionInfo()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateHeaderConnectionInfo));
                return;
            }

            if (lblComPort != null)
            {
                if (!string.IsNullOrEmpty(_comPortName) && _baudRate.HasValue)
                {
                    lblComPort.Text = $"{_comPortName} @ {_baudRate.Value}";
                    lblComPort.ForeColor = Color.Green;
                }
                else
                {
                    lblComPort.Text = "Not Connected";
                    lblComPort.ForeColor = Color.Gray;
                }
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
                
                if (hours > 0)
                {
                    lblSessionTime.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
                }
                else
                {
                    lblSessionTime.Text = $"{minutes:D2}:{seconds:D2}";
                }
            }
            else if (lblSessionTime != null)
            {
                lblSessionTime.Text = "00:00";
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

            // Initialize footer with auto-save status
            UpdateFooterAutoSave();
            UpdateFooterStatistics();
        }

        private void SetupHeaderLayout()
        {
            if (panelHeader == null) return;

            // Clear existing controls
            panelHeader.Controls.Clear();

            // Left: COM port and session time
            var leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                Padding = new Padding(10, 0, 0, 0),
                FlowDirection = FlowDirection.LeftToRight
            };

            if (lblComPort != null)
            {
                lblComPort.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                lblComPort.AutoSize = true;
                lblComPort.Padding = new Padding(0, 12, 10, 0);
                leftPanel.Controls.Add(lblComPort);
            }

            if (lblSessionTime != null)
            {
                lblSessionTime.Font = new Font("Segoe UI", 9f);
                lblSessionTime.AutoSize = true;
                lblSessionTime.Padding = new Padding(0, 12, 10, 0);
                leftPanel.Controls.Add(lblSessionTime);
            }

            // Middle: Hotkeys
            if (lblHotkeys != null)
            {
                lblHotkeys.Text = "ESC Vent | Space Pause | S Save | F Fullscreen";
                lblHotkeys.Font = new Font("Segoe UI", 8f);
                lblHotkeys.ForeColor = Color.FromArgb(120, 125, 140);
                lblHotkeys.Dock = DockStyle.Fill;
                lblHotkeys.TextAlign = ContentAlignment.MiddleCenter;
                lblHotkeys.Padding = new Padding(0, 12, 0, 0);
            }

            // Right: Control buttons
            var rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 10, 0)
            };

            if (btnPause != null)
            {
                btnPause.Text = "II Pause";
                btnPause.Size = new Size(80, 30);
                btnPause.BackColor = Color.FromArgb(30, 33, 40);
                btnPause.ForeColor = Color.FromArgb(220, 224, 232);
                btnPause.FlatStyle = FlatStyle.Flat;
                btnPause.FlatAppearance.BorderSize = 0;
                btnPause.Click += btnPause_Click;
                rightPanel.Controls.Add(btnPause);
            }

            if (btnExport != null)
            {
                btnExport.Text = "Export";
                btnExport.Size = new Size(80, 30);
                btnExport.BackColor = Color.FromArgb(0, 150, 0);
                btnExport.ForeColor = Color.White;
                btnExport.FlatStyle = FlatStyle.Flat;
                btnExport.FlatAppearance.BorderSize = 0;
                btnExport.Click += btnExport_Click;
                rightPanel.Controls.Add(btnExport);
            }

            if (btnReset != null)
            {
                btnReset.Text = "Reset";
                btnReset.Size = new Size(80, 30);
                btnReset.BackColor = Color.FromArgb(30, 33, 40);
                btnReset.ForeColor = Color.FromArgb(220, 224, 232);
                btnReset.FlatStyle = FlatStyle.Flat;
                btnReset.FlatAppearance.BorderSize = 0;
                btnReset.Click += (_, __) => ResetGraph();
                rightPanel.Controls.Add(btnReset);
            }

            if (btnFullscreenHeader != null)
            {
                btnFullscreenHeader.Text = "Fullscreen";
                btnFullscreenHeader.Size = new Size(100, 30);
                btnFullscreenHeader.BackColor = Color.FromArgb(30, 33, 40);
                btnFullscreenHeader.ForeColor = Color.FromArgb(220, 224, 232);
                btnFullscreenHeader.FlatStyle = FlatStyle.Flat;
                btnFullscreenHeader.FlatAppearance.BorderSize = 0;
                btnFullscreenHeader.Click += btnFullscreenHeader_Click;
                rightPanel.Controls.Add(btnFullscreenHeader);
            }

            panelHeader.Controls.Add(leftPanel);
            if (lblHotkeys != null) panelHeader.Controls.Add(lblHotkeys);
            panelHeader.Controls.Add(rightPanel);
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
        
        /// <summary>
        /// Устанавливает обработчик для кнопки GO TARGET
        /// </summary>
        public void SetTargetHandler(Action<double> handler)
        {
            _targetHandler = handler;
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

