using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm : Form
    {
        // --- серии графика ---
        private Series _seriesCurrent = null!;
        private Series _seriesTarget = null!;
        private Series _seriesMin = null!;
        private Series _seriesMax = null!;

        // Время в секундах (ось X)
        private double _timeSeconds = 0;
        private const double TimeStep = 0.5;   // шаг 0.5 c

        // TIME WINDOW DURATION (в секундах) - управляется ComboBox'ом
        private double _timeWindowSeconds = 60; // default: 1 min

        // Курсор
        private StripLine _cursorLineX = null!; // вертикальная пунктирная
        private StripLine _cursorLineY = null!; // горизонтальная пунктирная
        private Series _cursorMarker = null!;   // точка на линии

        // Окошко с данными под курсором
        private Panel _cursorInfoPanel = null!;
        private Label _lblInfoTime = null!;
        private Label _lblInfoPressure = null!;
        private Label _lblInfoTarget = null!;
        private Label _lblInfoRate = null!;

        public GraphForm()
        {
            InitializeComponent();
            ConfigureChart();
            ComboBoxValues();
            CreateCursorInfoPanel();

            // --- handlers ---
            cmbDuration.SelectedIndexChanged += CmbDuration_SelectedIndexChanged;
            cmbXStep.SelectedIndexChanged += (_, __) => ApplyGridSettings();
            cmbYStep.SelectedIndexChanged += (_, __) => ApplyGridSettings();
            chkShowGrid.CheckedChanged += (_, __) => ApplyGridSettings();

            nudMaximum.ValueChanged += Thresholds_ValueChanged;
            numericUpDown2.ValueChanged += Thresholds_ValueChanged;

            // применяем дефолтные значения
            _timeWindowSeconds = ParseDurationToSeconds(cmbDuration.SelectedItem?.ToString());
            ApplyGridSettings();
            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();

            chartPressure.MouseDoubleClick += ChartPressure_MouseDoubleClick;
            chartPressure.MouseMove += ChartPressure_MouseMove;
            chartPressure.MouseLeave += ChartPressure_MouseLeave;
        }

        // ====== значения для ComboBox’ов справа ======
        private void ComboBoxValues()
        {
            cmbDuration.Items.AddRange(new object[]
            {
                "1 min",
                "5 min",
                "15 min",
                "30 min",
                "1 hour",
                "2 hour",
                "4 hour",
                "7 hour"
            });
            cmbDuration.SelectedIndex = 0;

            cmbXStep.Items.AddRange(new object[] { "5", "10", "20" });
            cmbXStep.SelectedIndex = 0;

            cmbYStep.Items.AddRange(new object[] { "10", "20", "50" });
            cmbYStep.SelectedIndex = 1;

            chkShowGrid.Checked = true;
        }

        // ====== создание панели с информацией курсора ======
        private void CreateCursorInfoPanel()
        {
            _cursorInfoPanel = new Panel
            {
                Size = new Size(170, 90),
                BackColor = Color.FromArgb(25, 27, 34),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                Padding = new Padding(8)
            };

            _lblInfoTime = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(220, 224, 232),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Dock = DockStyle.Top
            };
            _lblInfoPressure = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(0, 200, 255),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                Dock = DockStyle.Top
            };
            _lblInfoTarget = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(255, 214, 69),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                Dock = DockStyle.Top
            };
            _lblInfoRate = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(180, 180, 190),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                Dock = DockStyle.Top
            };

            _cursorInfoPanel.Controls.Add(_lblInfoRate);
            _cursorInfoPanel.Controls.Add(_lblInfoTarget);
            _cursorInfoPanel.Controls.Add(_lblInfoPressure);
            _cursorInfoPanel.Controls.Add(_lblInfoTime);

            chartPressure.Controls.Add(_cursorInfoPanel);
            _cursorInfoPanel.BringToFront();
        }

        // ====== настройка графика ======
        private void ConfigureChart()
        {
            chartPressure.Series.Clear();
            chartPressure.ChartAreas.Clear();
            chartPressure.Legends.Clear();

            var area = new ChartArea("MainArea");
            area.BackColor = Color.FromArgb(24, 26, 33);
            area.BorderColor = Color.Transparent;

            // X
            area.AxisX.LineWidth = 2;
            area.AxisX.LineColor = Color.FromArgb(60, 60, 70);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(150, 150, 160);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            area.AxisX.Interval = 5;
            area.AxisX.LabelStyle.Format = "0";
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = _timeWindowSeconds;

            // Y
            area.AxisY.LineWidth = 2;
            area.AxisY.LineColor = Color.FromArgb(60, 60, 70);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(150, 150, 160);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            chartPressure.ChartAreas.Add(area);

            var legend = new Legend("MainLegend")
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Center,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(220, 224, 232),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                BorderColor = Color.Transparent
            };
            chartPressure.Legends.Add(legend);

            _seriesCurrent = new Series("Current")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.FromArgb(0, 200, 255),
                ChartArea = "MainArea",
                Legend = "MainLegend",
                IsVisibleInLegend = true
            };
            chartPressure.Series.Add(_seriesCurrent);

            _seriesTarget = new Series("Target")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 214, 69),
                ChartArea = "MainArea",
                Legend = "MainLegend",
                IsVisibleInLegend = true
            };
            chartPressure.Series.Add(_seriesTarget);

            // Threshold MIN / MAX (реальные линии)
            _seriesMin = new Series("Min")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 214, 69),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                Legend = "MainLegend",
                IsVisibleInLegend = true
            };
            chartPressure.Series.Add(_seriesMin);

            _seriesMax = new Series("Max")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 80, 80),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                Legend = "MainLegend",
                IsVisibleInLegend = true
            };
            chartPressure.Series.Add(_seriesMax);

            // Cursor lines
            _cursorLineX = new StripLine
            {
                BorderColor = Color.FromArgb(120, 120, 130),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash
            };
            area.AxisX.StripLines.Add(_cursorLineX);
            _cursorLineX.IntervalOffset = double.NaN;

            _cursorLineY = new StripLine
            {
                BorderColor = Color.FromArgb(120, 120, 130),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash
            };
            area.AxisY.StripLines.Add(_cursorLineY);
            _cursorLineY.IntervalOffset = double.NaN;

            _cursorMarker = new Series("CursorMarker")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                Color = Color.FromArgb(0, 220, 255),
                ChartArea = area.Name,
                IsVisibleInLegend = false
            };
            chartPressure.Series.Add(_cursorMarker);

            // Zoom
            area.AxisY.ScaleView.Zoomable = true;
            area.CursorY.IsUserEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;

            area.AxisX.ScaleView.Zoomable = true;
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;

            area.CursorX.LineColor = Color.Transparent;
            area.CursorY.LineColor = Color.Transparent;
            area.CursorX.LineWidth = 0;
            area.CursorY.LineWidth = 0;
        }

        // ====== добавление новой точки ======
        public void AddSample(double currentPressure, double? targetPressure)
        {
            if (chartPressure.IsDisposed || _seriesCurrent == null)
                return;

            _seriesCurrent.Points.AddXY(_timeSeconds, currentPressure);

            if (targetPressure.HasValue && _seriesTarget != null)
                _seriesTarget.Points.AddXY(_timeSeconds, targetPressure.Value);

            _timeSeconds += TimeStep;

            ApplyTimeWindow(forceTrim: true);   // включает авто-скролл (если нет зума) + trim
            ApplyThresholdLines();              // растягиваем min/max на текущий видимый X
        }

        // ===== TIME WINDOW =====
        private void CmbDuration_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _timeWindowSeconds = ParseDurationToSeconds(cmbDuration.SelectedItem?.ToString());
            ApplyTimeWindow(forceTrim: true);
            ApplyThresholdLines();
        }

        private static double ParseDurationToSeconds(string? text)
        {
            return text switch
            {
                "1 min" => 60,
                "5 min" => 5 * 60,
                "15 min" => 15 * 60,
                "30 min" => 30 * 60,
                "1 hour" => 60 * 60,
                "2 hour" => 2 * 60 * 60,
                "4 hour" => 4 * 60 * 60,
                "7 hour" => 7 * 60 * 60,
                _ => 60
            };
        }

        private void ApplyTimeWindow(bool forceTrim)
        {
            if (chartPressure.ChartAreas.Count == 0) return;

            var area = chartPressure.ChartAreas["MainArea"];

            double xMax = _timeSeconds;
            double xMin = Math.Max(0, xMax - _timeWindowSeconds);

            // Диапазон по X меняем только если нет зума
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

            if (!forceTrim) return;

            // Тримим только "живые" серии, thresholds не трогаем
            foreach (var s in chartPressure.Series)
            {
                if (s.Name is "CursorMarker" or "Min" or "Max") continue;
                TrimSeriesByX(s, xMin);
            }
        }

        private static void TrimSeriesByX(Series s, double xMin)
        {
            while (s.Points.Count > 0)
            {
                if (s.Points[0].XValue >= xMin) break;
                s.Points.RemoveAt(0);
            }
        }

        // ===== GRID SETTINGS =====
        private void ApplyGridSettings()
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var area = chartPressure.ChartAreas["MainArea"];

            if (double.TryParse(cmbXStep.SelectedItem?.ToString(), out double xStep))
                area.AxisX.Interval = xStep;

            if (double.TryParse(cmbYStep.SelectedItem?.ToString(), out double yStep))
                area.AxisY.Interval = yStep;

            bool showGrid = chkShowGrid.Checked;
            area.AxisX.MajorGrid.Enabled = showGrid;
            area.AxisY.MajorGrid.Enabled = showGrid;

            area.AxisX.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);

            area.RecalculateAxesScale();
        }

        // ===== THRESHOLDS: реальные линии Min/Max =====
        private void Thresholds_ValueChanged(object? sender, EventArgs e)
        {
            ApplyThresholdLines();
        }

        private void ApplyThresholdLines()
        {
            if (chartPressure.ChartAreas.Count == 0) return;
            var area = chartPressure.ChartAreas["MainArea"];

            // текущий видимый диапазон X (учитываем zoom)
            double x1, x2;
            if (area.AxisX.ScaleView.IsZoomed)
            {
                x1 = area.AxisX.ScaleView.ViewMinimum;
                x2 = area.AxisX.ScaleView.ViewMaximum;
            }
            else
            {
                x1 = area.AxisX.Minimum;
                x2 = area.AxisX.Maximum;
            }

            // страховка
            if (x2 <= x1)
            {
                x1 = Math.Max(0, _timeSeconds - _timeWindowSeconds);
                x2 = Math.Max(x1 + 1, _timeSeconds);
            }

            double max = (double)nudMaximum.Value;
            double min = (double)numericUpDown2.Value;

            // MAX (2 точки)
            _seriesMax.Points.Clear();
            _seriesMax.Points.AddXY(x1, max);
            _seriesMax.Points.AddXY(x2, max);

            // MIN (2 точки)
            _seriesMin.Points.Clear();
            _seriesMin.Points.AddXY(x1, min);
            _seriesMin.Points.AddXY(x2, min);
        }

        // ====== двойной клик — сброс зума ======
        private void ChartPressure_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (chartPressure.ChartAreas.Count == 0)
                return;

            var area = chartPressure.ChartAreas["MainArea"];

            area.AxisY.ScaleView.ZoomReset(0);
            area.AxisX.ScaleView.ZoomReset(0);

            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            area.RecalculateAxesScale();
        }

        // ====== мышь ушла с графика ======
        private void ChartPressure_MouseLeave(object? sender, EventArgs e)
        {
            _cursorLineX.IntervalOffset = double.NaN;
            _cursorLineY.IntervalOffset = double.NaN;

            _cursorMarker.Points.Clear();
            _cursorInfoPanel.Visible = false;
        }

        // ====== движение мыши по графику ======
        private void ChartPressure_MouseMove(object? sender, MouseEventArgs e)
        {
            var hit = chartPressure.HitTest(e.X, e.Y);
            if (hit.ChartArea == null)
            {
                ChartPressure_MouseLeave(sender, EventArgs.Empty);
                return;
            }

            var area = hit.ChartArea;

            if (_seriesCurrent.Points.Count == 0)
                return;

            double xValue = area.AxisX.PixelPositionToValue(e.X);

            int nearestIndex = -1;
            double bestDx = double.MaxValue;
            for (int i = 0; i < _seriesCurrent.Points.Count; i++)
            {
                double dx = Math.Abs(_seriesCurrent.Points[i].XValue - xValue);
                if (dx < bestDx)
                {
                    bestDx = dx;
                    nearestIndex = i;
                }
            }
            if (nearestIndex < 0) return;

            var p = _seriesCurrent.Points[nearestIndex];
            double tSec = p.XValue;
            double pressure = p.YValues[0];

            double? target = null;
            if (_seriesTarget.Points.Count > nearestIndex)
                target = _seriesTarget.Points[nearestIndex].YValues[0];

            double rate = 0;
            if (nearestIndex > 0)
            {
                double prev = _seriesCurrent.Points[nearestIndex - 1].YValues[0];
                rate = (pressure - prev) / TimeStep;
            }

            _cursorLineX.IntervalOffset = p.XValue;
            _cursorLineY.IntervalOffset = pressure;

            _cursorMarker.Points.Clear();
            _cursorMarker.Points.AddXY(p.XValue, pressure);

            _lblInfoTime.Text = $"Time:     {tSec:0.0}s";
            _lblInfoPressure.Text = $"Pressure: {pressure:0.00}";
            _lblInfoTarget.Text = $"Target:   {(target.HasValue ? target.Value.ToString("0.00") : "-")}";
            _lblInfoRate.Text = $"Δ Rate:   {rate:+0.00;-0.00;0}/s";

            int px = e.X + 15;
            int py = e.Y + 15;

            if (px + _cursorInfoPanel.Width > chartPressure.Width)
                px = chartPressure.Width - _cursorInfoPanel.Width - 5;
            if (py + _cursorInfoPanel.Height > chartPressure.Height)
                py = chartPressure.Height - _cursorInfoPanel.Height - 5;

            _cursorInfoPanel.Location = new Point(px, py);
            _cursorInfoPanel.Visible = true;
        }

        // ====== прочие обработчики из дизайнера (можно оставить пустыми) ======
        private void panelEmergencyHost_Paint(object sender, PaintEventArgs e) { }
        private void lblTimeWindowTitle_Click(object sender, EventArgs e) { }
        private void lblXStep_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label3_Click_1(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}
