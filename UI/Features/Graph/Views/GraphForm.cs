using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Alicat.Services.Data;
using DataPointModel = Alicat.Services.Data.DataPoint;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm : Form
    {
        private readonly SessionDataStore _dataStore;
        // --- series ---
        private Series _seriesCurrent = null!;
        private Series _seriesTarget = null!;
        private Series _seriesMin = null!;
        private Series _seriesMax = null!;

        // time in seconds (X)
        private double _timeSeconds = 0;
        private const double TimeStep = 0.5;

        // Time window (seconds)
        private double _timeWindowSeconds = 60;

        // Grid step X (auto)
        private double _gridStepXSeconds = 10;

        // Cursor
        private StripLine _cursorLineX = null!;
        private StripLine _cursorLineY = null!;
        private Series _cursorMarker = null!;

        // Cursor info panel
        private Panel _cursorInfoPanel = null!;
        private Label _lblInfoTime = null!;
        private Label _lblInfoPressure = null!;
        private Label _lblInfoTarget = null!;
        private Label _lblInfoRate = null!;

        // Cursor performance
        private int _lastNearestIndex = -1;
        private long _lastCursorTick = 0;
        private const int CursorThrottleMs = 16; // ~60fps

        // Target value cache (for flat line)
        private double? _lastTargetValue = null;

        // Duration data: (Name, Seconds, GridStepXSeconds)
        private static readonly (string Name, int Seconds, int GridStep)[] DurationData = new[]
        {
            ("1 min",    60,     10),
            ("5 min",    300,    30),
            ("15 min",   900,    60),
            ("30 min",   1800,   120),
            ("1 hour",   3600,   600),
            ("2 hours",  7200,   900),
            ("4 hours",  14400,  1800),
            ("6 hours",  21600,  1800),
            ("8 hours",  28800,  1800),
            ("10 hours", 36000,  3600)
        };

        public GraphForm(SessionDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeComponent();
            ConfigureChart();
            ComboBoxValues();
            CreateCursorInfoPanel();

            // handlers (duration / grid / thresholds)
            cmbDuration.SelectedIndexChanged += CmbDuration_SelectedIndexChanged;
            cmbYStep.SelectedIndexChanged += (_, __) => ApplyGridSettings();
            chkShowGrid.CheckedChanged += (_, __) => ApplyGridSettings();

            // smoothing (если чекбокс есть на форме)
            // если у тебя chkSmoothing называется иначе — поменяй имя здесь
            if (Controls.Find("chkSmoothing", true).Length > 0 && chkSmoothing != null)
            {
                chkSmoothing.CheckedChanged += (_, __) => ApplySmoothing();
            }

            nudMaximum.ValueChanged += Thresholds_ValueChanged;
            numericUpDown2.ValueChanged += Thresholds_ValueChanged;

            // default duration
            _timeWindowSeconds = GetDurationSeconds(0);
            _gridStepXSeconds = GetAutoGridStepX(_timeWindowSeconds);
            UpdateGridStepXDisplay();

            ApplyGridSettings();
            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            UpdateTargetLine();        // <- target flat line
            UpdateCustomLabelsX();     // <- X labels

            // cursor events
            chartPressure.MouseDoubleClick += ChartPressure_MouseDoubleClick;
            chartPressure.MouseMove += ChartPressure_MouseMove;
            chartPressure.MouseLeave += ChartPressure_MouseLeave;

            ApplySmoothing();

            // Загрузить историю из Store
            LoadHistoryFromStore();

            // Подписаться на новые точки
            _dataStore.OnNewPoint += OnNewPointReceived;
        }

        // =========================
        // UI / ComboBox
        // =========================
        private void ComboBoxValues()
        {
            cmbDuration.Items.Clear();
            foreach (var d in DurationData)
                cmbDuration.Items.Add(d.Name);
            cmbDuration.SelectedIndex = 0;

            // X step AUTO (disabled)
            cmbXStep.Items.Clear();
            cmbXStep.Items.Add("AUTO (10s)");
            cmbXStep.SelectedIndex = 0;
            cmbXStep.Enabled = false;

            // Y step
            cmbYStep.Items.Clear();
            cmbYStep.Items.AddRange(new object[] { "10", "20", "50", "100" });
            cmbYStep.SelectedIndex = 1;

            chkShowGrid.Checked = true;
        }

        private double GetDurationSeconds(int index)
        {
            if (index < 0 || index >= DurationData.Length) return 60;
            return DurationData[index].Seconds;
        }

        private double GetAutoGridStepX(double durationSeconds)
        {
            foreach (var d in DurationData)
                if (d.Seconds == (int)durationSeconds)
                    return d.GridStep;
            return 60;
        }

        private string FormatGridStep(double seconds)
        {
            if (seconds < 60) return $"{(int)seconds}s";
            if (seconds < 3600) return $"{(int)(seconds / 60)}m";
            return $"{(int)(seconds / 3600)}h";
        }

        private void UpdateGridStepXDisplay()
        {
            cmbXStep.Items.Clear();
            cmbXStep.Items.Add($"AUTO ({FormatGridStep(_gridStepXSeconds)})");
            cmbXStep.SelectedIndex = 0;
        }

        // =========================
        // X labels formatting (CustomLabels)
        // =========================
        private string FormatTimeLabel(double totalSeconds)
        {
            if (totalSeconds <= 0) return "0";

            int secs = (int)Math.Round(totalSeconds);
            int hours = secs / 3600;
            int mins = (secs % 3600) / 60;
            int seconds = secs % 60;

            // 1..5 min -> show seconds
            if (_timeWindowSeconds <= 300)
            {
                if (mins > 0 && seconds > 0) return $"{mins}m{seconds}s";
                if (mins > 0) return $"{mins}m";
                return $"{secs}s";
            }

            // 15 min .. 2h -> show minutes/hours
            if (_timeWindowSeconds <= 7200)
            {
                if (hours > 0 && mins > 0) return $"{hours}h{mins}m";
                if (hours > 0) return $"{hours}h";
                return $"{mins}m";
            }

            // 4h+ -> show hours/minutes
            if (hours > 0 && mins > 0) return $"{hours}h{mins}m";
            if (hours > 0) return $"{hours}h";
            return $"{mins}m";
        }

        private void UpdateCustomLabelsX()
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var axis = chartPressure.ChartAreas["MainArea"].AxisX;

            axis.CustomLabels.Clear();

            double xMin = axis.ScaleView.IsZoomed ? axis.ScaleView.ViewMinimum : axis.Minimum;
            double xMax = axis.ScaleView.IsZoomed ? axis.ScaleView.ViewMaximum : axis.Maximum;

            if (xMax <= xMin) return;

            double step = _gridStepXSeconds > 0 ? _gridStepXSeconds : 10;
            double half = step / 2.0;

            // align to step
            double start = Math.Ceiling(xMin / step) * step;
            for (double x = start; x <= xMax + 0.0001; x += step)
            {
                axis.CustomLabels.Add(x - half, x + half, FormatTimeLabel(x));
            }
        }

        // =========================
        // Cursor panel
        // =========================
        private void CreateCursorInfoPanel()
        {
            _cursorInfoPanel = new Panel
            {
                Size = new Size(160, 95),
                BackColor = Color.FromArgb(245, 18, 20, 26),
                Visible = false
            };

            _cursorInfoPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(60, 65, 75), 1);
                e.Graphics.DrawRectangle(pen, 0, 0, _cursorInfoPanel.Width - 1, _cursorInfoPanel.Height - 1);
            };

            var font = new Font("Consolas", 9f);

            _lblInfoTime = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(160, 165, 175),
                Font = font,
                Location = new Point(8, 8)
            };
            _lblInfoPressure = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(0, 200, 240),
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                Location = new Point(8, 28)
            };
            _lblInfoTarget = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(240, 200, 0),
                Font = font,
                Location = new Point(8, 48)
            };
            _lblInfoRate = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(160, 100, 240),
                Font = font,
                Location = new Point(8, 68)
            };

            _cursorInfoPanel.Controls.Add(_lblInfoTime);
            _cursorInfoPanel.Controls.Add(_lblInfoPressure);
            _cursorInfoPanel.Controls.Add(_lblInfoTarget);
            _cursorInfoPanel.Controls.Add(_lblInfoRate);

            chartPressure.Controls.Add(_cursorInfoPanel);
            _cursorInfoPanel.BringToFront();
        }

        // =========================
        // Chart setup
        // =========================
        private void ConfigureChart()
        {
            chartPressure.Series.Clear();
            chartPressure.ChartAreas.Clear();
            chartPressure.Legends.Clear();

            // render quality
            chartPressure.AntiAliasing = AntiAliasingStyles.All;
            chartPressure.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

            var area = new ChartArea("MainArea");
            area.BackColor = Color.FromArgb(22, 24, 30);
            area.BorderColor = Color.Transparent;

            // X axis
            area.AxisX.LineWidth = 1;
            area.AxisX.LineColor = Color.FromArgb(50, 55, 65);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(120, 125, 135);
            area.AxisX.LabelStyle.Font = new Font("Consolas", 8f);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(35, 40, 50);
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            area.AxisX.Interval = _gridStepXSeconds;
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = _timeWindowSeconds;

            // Y axis
            area.AxisY.LineWidth = 1;
            area.AxisY.LineColor = Color.FromArgb(50, 55, 65);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(120, 125, 135);
            area.AxisY.LabelStyle.Font = new Font("Consolas", 8f);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(35, 40, 50);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            area.AxisY.Interval = 20;

            // Zoom
            area.AxisY.ScaleView.Zoomable = true;
            area.CursorY.IsUserEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;

            area.AxisX.ScaleView.Zoomable = true;
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;

            area.CursorX.LineColor = Color.Transparent;
            area.CursorY.LineColor = Color.Transparent;

            chartPressure.ChartAreas.Add(area);

            // Legend
            var legend = new Legend("MainLegend")
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Center,
                BackColor = Color.FromArgb(180, 22, 24, 30),
                ForeColor = Color.FromArgb(200, 205, 215),
                Font = new Font("Segoe UI", 8.5f),
                BorderColor = Color.Transparent,
                IsDockedInsideChartArea = true,
                DockedToChartArea = "MainArea"
            };
            chartPressure.Legends.Add(legend);

            // Current
            _seriesCurrent = new Series("Current")
            {
                ChartType = SeriesChartType.Line, // smoothing changes this
                BorderWidth = 2,
                Color = Color.FromArgb(0, 200, 240),
                ChartArea = "MainArea",
                Legend = "MainLegend"
            };
            chartPressure.Series.Add(_seriesCurrent);

            // Target (flat line будет рисоваться 2 точками)
            _seriesTarget = new Series("Target")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(240, 200, 0),
                ChartArea = "MainArea",
                Legend = "MainLegend"
            };
            chartPressure.Series.Add(_seriesTarget);

            // Min
            _seriesMin = new Series("Min")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(240, 180, 0),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                Legend = "MainLegend"
            };
            chartPressure.Series.Add(_seriesMin);

            // Max
            _seriesMax = new Series("Max")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(240, 70, 70),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                Legend = "MainLegend"
            };
            chartPressure.Series.Add(_seriesMax);

            // Cursor strip lines
            _cursorLineX = new StripLine
            {
                BorderColor = Color.FromArgb(100, 255, 255, 255),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash,
                IntervalOffset = double.NaN
            };
            area.AxisX.StripLines.Add(_cursorLineX);

            _cursorLineY = new StripLine
            {
                BorderColor = Color.FromArgb(100, 255, 255, 255),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash,
                IntervalOffset = double.NaN
            };
            area.AxisY.StripLines.Add(_cursorLineY);

            // Cursor marker
            _cursorMarker = new Series("CursorMarker")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 10,
                MarkerColor = Color.FromArgb(0, 220, 255),
                MarkerBorderColor = Color.White,
                MarkerBorderWidth = 2,
                ChartArea = "MainArea",
                IsVisibleInLegend = false
            };
            chartPressure.Series.Add(_cursorMarker);
        }

        private void ApplySmoothing()
        {
            // если чекбокса нет — просто ничего
            if (Controls.Find("chkSmoothing", true).Length == 0 || chkSmoothing == null) return;

            bool smooth = chkSmoothing.Checked;

            _seriesCurrent.ChartType = smooth ? SeriesChartType.Spline : SeriesChartType.Line;

            // чтобы spline не был “резиновый”
            if (smooth)
                _seriesCurrent["LineTension"] = "0.2"; // 0..1 (меньше = спокойнее)
        }

        // =========================
        // Add sample
        // =========================
        public void AddSample(double currentPressure, double? targetPressure)
        {
            if (chartPressure.IsDisposed) return;

            _seriesCurrent.Points.AddXY(_timeSeconds, currentPressure);

            // TARGET: не добавляем точки каждый раз -> делаем ровную линию
            if (targetPressure.HasValue)
            {
                if (_lastTargetValue == null || Math.Abs(_lastTargetValue.Value - targetPressure.Value) > 1e-9)
                {
                    _lastTargetValue = targetPressure.Value;
                    UpdateTargetLine();
                }
            }

            _timeSeconds += TimeStep;

            ApplyTimeWindow(forceTrim: true);
            ApplyThresholdLines();
            UpdateTargetLine();
        }

        // =========================
        // Duration change
        // =========================
        private void CmbDuration_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = cmbDuration.SelectedIndex;
            if (idx < 0 || idx >= DurationData.Length) return;

            _timeWindowSeconds = DurationData[idx].Seconds;
            _gridStepXSeconds = DurationData[idx].GridStep;

            UpdateGridStepXDisplay();

            RedrawFromStore();

            ApplyGridSettings();
            UpdateCustomLabelsX();
        }

        private void RedrawFromStore()
        {
            // Очистить текущие точки
            _seriesCurrent.Points.Clear();
            _lastTargetValue = null;
            _timeSeconds = 0;

            // Перерисовать из Store
            foreach (var point in _dataStore.Points)
            {
                _seriesCurrent.Points.AddXY(point.ElapsedSeconds, point.Current);

                if (point.Target > 0)
                {
                    _lastTargetValue = point.Target;
                }

                _timeSeconds = point.ElapsedSeconds + TimeStep;
            }

            // Применить окно времени БЕЗ трима
            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            UpdateTargetLine();
        }

        // =========================
        // Time window + trim
        // =========================
        private void ApplyTimeWindow(bool forceTrim)
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var area = chartPressure.ChartAreas["MainArea"];

            double xMax = _timeSeconds;
            double xMin = Math.Max(0, xMax - _timeWindowSeconds);

            if (!area.AxisX.ScaleView.IsZoomed)
            {
                if (_timeSeconds <= _timeWindowSeconds)
                {
                    area.AxisX.Minimum = 0;
                    area.AxisX.Maximum = _timeWindowSeconds;
                }
                else
                {
                    area.AxisX.Minimum = xMin;
                    area.AxisX.Maximum = xMax;
                }
            }

            area.AxisX.Interval = _gridStepXSeconds;

            UpdateCustomLabelsX();

            if (!forceTrim) return;

            foreach (var s in chartPressure.Series)
            {
                if (s.Name is "CursorMarker" or "Min" or "Max" or "Target") continue;
                TrimSeriesByX(s, xMin);
            }

            // после трима индекс курсора может стать неверным
            _lastNearestIndex = -1;
        }

        private static void TrimSeriesByX(Series s, double xMin)
        {
            while (s.Points.Count > 0 && s.Points[0].XValue < xMin)
                s.Points.RemoveAt(0);
        }

        // =========================
        // Grid settings
        // =========================
        private void ApplyGridSettings()
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var area = chartPressure.ChartAreas["MainArea"];

            area.AxisX.Interval = _gridStepXSeconds;

            if (double.TryParse(cmbYStep.SelectedItem?.ToString(), out double yStep))
                area.AxisY.Interval = yStep;

            bool showGrid = chkShowGrid.Checked;
            area.AxisX.MajorGrid.Enabled = showGrid;
            area.AxisY.MajorGrid.Enabled = showGrid;

            UpdateCustomLabelsX();
            area.RecalculateAxesScale();
        }

        // =========================
        // Thresholds
        // =========================
        private void Thresholds_ValueChanged(object? sender, EventArgs e) => ApplyThresholdLines();

        private void ApplyThresholdLines()
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var area = chartPressure.ChartAreas["MainArea"];

            double x1 = area.AxisX.ScaleView.IsZoomed ? area.AxisX.ScaleView.ViewMinimum : area.AxisX.Minimum;
            double x2 = area.AxisX.ScaleView.IsZoomed ? area.AxisX.ScaleView.ViewMaximum : area.AxisX.Maximum;

            if (x2 <= x1)
            {
                x1 = Math.Max(0, _timeSeconds - _timeWindowSeconds);
                x2 = Math.Max(x1 + 1, _timeSeconds);
            }

            double maxVal = (double)nudMaximum.Value;
            double minVal = (double)numericUpDown2.Value;

            _seriesMax.Points.Clear();
            _seriesMax.Points.AddXY(x1, maxVal);
            _seriesMax.Points.AddXY(x2, maxVal);

            _seriesMin.Points.Clear();
            _seriesMin.Points.AddXY(x1, minVal);
            _seriesMin.Points.AddXY(x2, minVal);
        }

        // =========================
        // Target flat line (2 points)
        // =========================
        private void UpdateTargetLine()
        {
            if (_lastTargetValue == null) return;
            if (chartPressure.ChartAreas.Count == 0) return;

            var area = chartPressure.ChartAreas["MainArea"];

            double x1 = area.AxisX.ScaleView.IsZoomed ? area.AxisX.ScaleView.ViewMinimum : area.AxisX.Minimum;
            double x2 = area.AxisX.ScaleView.IsZoomed ? area.AxisX.ScaleView.ViewMaximum : area.AxisX.Maximum;

            if (x2 <= x1)
            {
                x1 = Math.Max(0, _timeSeconds - _timeWindowSeconds);
                x2 = Math.Max(x1 + 1, _timeSeconds);
            }

            _seriesTarget.Points.Clear();
            _seriesTarget.Points.AddXY(x1, _lastTargetValue.Value);
            _seriesTarget.Points.AddXY(x2, _lastTargetValue.Value);
        }

        // =========================
        // Reset zoom
        // =========================
        private void ChartPressure_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var area = chartPressure.ChartAreas["MainArea"];

            area.AxisY.ScaleView.ZoomReset(0);
            area.AxisX.ScaleView.ZoomReset(0);

            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            UpdateTargetLine();
            UpdateCustomLabelsX();

            area.RecalculateAxesScale();
        }

        // =========================
        // Cursor fast search (binary)
        // =========================
        private int FindNearestIndexByX(Series s, double x)
        {
            int n = s.Points.Count;
            if (n <= 0) return -1;

            // quick edges
            double x0 = s.Points[0].XValue;
            double xN = s.Points[n - 1].XValue;
            if (x <= x0) return 0;
            if (x >= xN) return n - 1;

            int lo = 0;
            int hi = n - 1;

            while (hi - lo > 1)
            {
                int mid = (lo + hi) >> 1;
                double xm = s.Points[mid].XValue;
                if (xm < x) lo = mid;
                else hi = mid;
            }

            // now lo and hi are neighbors around x
            double dlo = Math.Abs(s.Points[lo].XValue - x);
            double dhi = Math.Abs(s.Points[hi].XValue - x);
            return (dhi < dlo) ? hi : lo;
        }

        // =========================
        // Mouse leave
        // =========================
        private void ChartPressure_MouseLeave(object? sender, EventArgs e)
        {
            _cursorLineX.IntervalOffset = double.NaN;
            _cursorLineY.IntervalOffset = double.NaN;
            _cursorMarker.Points.Clear();
            _cursorInfoPanel.Visible = false;
            _lastNearestIndex = -1;
        }

        // =========================
        // Mouse move (fast + throttled)
        // =========================
        private void ChartPressure_MouseMove(object? sender, MouseEventArgs e)
        {
            // throttle
            long now = Environment.TickCount64;
            if (now - _lastCursorTick < CursorThrottleMs) return;
            _lastCursorTick = now;

            var hit = chartPressure.HitTest(e.X, e.Y);
            if (hit.ChartArea == null)
            {
                ChartPressure_MouseLeave(sender, EventArgs.Empty);
                return;
            }

            if (_seriesCurrent.Points.Count == 0) return;

            var area = hit.ChartArea;
            double xValue = area.AxisX.PixelPositionToValue(e.X);

            int idx = FindNearestIndexByX(_seriesCurrent, xValue);
            if (idx < 0) return;

            // panel always follows mouse fast
            int px = e.X + 15;
            int py = e.Y + 15;

            if (px + _cursorInfoPanel.Width > chartPressure.Width - 10)
                px = e.X - _cursorInfoPanel.Width - 15;
            if (py + _cursorInfoPanel.Height > chartPressure.Height - 10)
                py = e.Y - _cursorInfoPanel.Height - 15;

            if (px < 5) px = 5;
            if (py < 5) py = 5;

            _cursorInfoPanel.Location = new Point(px, py);

            // update data only if index changed (less redraw)
            if (idx != _lastNearestIndex)
            {
                _lastNearestIndex = idx;

                var p = _seriesCurrent.Points[idx];
                double tSec = p.XValue;
                double pressure = p.YValues[0];

                double? target = _lastTargetValue;

                double rate = 0;
                if (idx > 0)
                {
                    double prev = _seriesCurrent.Points[idx - 1].YValues[0];
                    rate = (pressure - prev) / TimeStep;
                }

                _cursorLineX.IntervalOffset = p.XValue;
                _cursorLineY.IntervalOffset = pressure;

                _cursorMarker.Points.Clear();
                _cursorMarker.Points.AddXY(p.XValue, pressure);

                int totalSec = (int)Math.Round(tSec);
                int mins = totalSec / 60;
                int secs = totalSec % 60;

                _lblInfoTime.Text = $"Time:     {mins}:{secs:D2}";
                _lblInfoPressure.Text = $"Pressure: {pressure:F2}";
                _lblInfoTarget.Text = $"Target:   {(target.HasValue ? target.Value.ToString("F2") : "-")}";
                _lblInfoRate.Text = $"Δ Rate:   {rate:+0.00;-0.00;0}/s";
            }

            _cursorInfoPanel.Visible = true;
        }

        // =========================
        // SessionDataStore integration
        // =========================
        private void LoadHistoryFromStore()
        {
            foreach (var point in _dataStore.Points)
            {
                _seriesCurrent.Points.AddXY(point.ElapsedSeconds, point.Current);

                if (point.Target > 0)
                {
                    _lastTargetValue = point.Target;
                }

                _timeSeconds = point.ElapsedSeconds + TimeStep;
            }

            ApplyTimeWindow(forceTrim: true);
            ApplyThresholdLines();
            UpdateTargetLine();
        }

        private void OnNewPointReceived(DataPointModel point)
        {
            if (IsDisposed) return;

            // Вызываем в UI потоке
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnNewPointReceived(point)));
                return;
            }

            AddSample(point.Current, point.Target > 0 ? point.Target : null);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Отписаться от событий
            _dataStore.OnNewPoint -= OnNewPointReceived;
            base.OnFormClosing(e);
        }

        // ===== designer stubs =====
        private void panelEmergencyHost_Paint(object sender, PaintEventArgs e) { }
        private void lblTimeWindowTitle_Click(object sender, EventArgs e) { }
        private void lblXStep_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label3_Click_1(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}
