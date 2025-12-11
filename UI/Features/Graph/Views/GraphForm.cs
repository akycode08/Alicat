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

        // Время в секундах (ось X)
        private double _timeSeconds = 0;
        private const double TimeStep = 0.5;   // шаг 0.5 c
        private const double TimeWindow = 30;    // показываем последние 30 секунд

        // Курсор
        private StripLine _cursorLineX = null!; // вертикальная пунктирная
        private StripLine _cursorLineY = null!; // горизонтальная пунктирная
        private Series _cursorMarker = null!; // точка на линии

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

            cmbXStep.Items.AddRange(new object[]
            {
                "5",
                "10",
                "20"
            });
            cmbXStep.SelectedIndex = 0;

            cmbYStep.Items.AddRange(new object[]
            {
                "10",
                "20",
                "50"
            });
            cmbYStep.SelectedIndex = 1;
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

            // добавляем в обратном порядке (Dock = Top стекается снизу вверх)
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

            // === ChartArea ===
            var area = new ChartArea("MainArea");
            area.BackColor = Color.FromArgb(24, 26, 33);
            area.BorderColor = Color.Transparent;

            // Ось X
            area.AxisX.LineWidth = 2;
            area.AxisX.LineColor = Color.FromArgb(60, 60, 70);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(150, 150, 160);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            area.AxisX.Interval = 5;   // шаг подписей 5 сек
            area.AxisX.LabelStyle.Format = "0"; // 0, 5, 10...
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = TimeWindow; // 30 секунд

            // Ось Y
            area.AxisY.LineWidth = 2;
            area.AxisY.LineColor = Color.FromArgb(60, 60, 70);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(150, 150, 160);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            chartPressure.ChartAreas.Add(area);

            // === CURRENT LINE ===
            _seriesCurrent = new Series("Current")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.FromArgb(0, 200, 255),
                ChartArea = "MainArea",
                IsVisibleInLegend = false
            };
            chartPressure.Series.Add(_seriesCurrent);

            // === LEGEND справа ===
            var legend = new Legend("MainLegend")
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Center,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(200, 200, 210),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular)
            };
            chartPressure.Legends.Add(legend);


            // === TARGET LINE ===
            _seriesTarget = new Series("Target")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 214, 69),
                ChartArea = "MainArea",
                IsVisibleInLegend = false
            };
            chartPressure.Series.Add(_seriesTarget);

            // === VERTICAL CURSOR LINE (X) ===
            _cursorLineX = new StripLine
            {
                BorderColor = Color.FromArgb(120, 120, 130),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash
            };
            area.AxisX.StripLines.Add(_cursorLineX);
            _cursorLineX.IntervalOffset = double.NaN;   // по умолчанию скрыта

            // === HORIZONTAL CURSOR LINE (Y) ===
            _cursorLineY = new StripLine
            {
                BorderColor = Color.FromArgb(120, 120, 130),
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash
            };
            area.AxisY.StripLines.Add(_cursorLineY);
            _cursorLineY.IntervalOffset = double.NaN;

            // === POINT UNDER CURSOR ===
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

            // === MIN LIMIT (yellow dashed) ===
            var sMin = new Series("Min")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 214, 69),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                IsVisibleInLegend = false
            };
            chartPressure.Series.Add(sMin);

            // === MAX LIMIT (red dashed) ===
            var sMax = new Series("Max")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 80, 80),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                IsVisibleInLegend = false
            };
            chartPressure.Series.Add(sMax);

            // === Zoom ===
            area.AxisY.ScaleView.Zoomable = true;
            area.CursorY.IsUserEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;

            area.AxisX.ScaleView.Zoomable = true;
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;

            // на всякий случай удаляем Series1 из дизайнера
            if (chartPressure.Series.Count > 6)
                chartPressure.Series.RemoveAt(6);

            // отключаем отображение системного курсора Chart (оставляем только наш)
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

            if (chartPressure.ChartAreas.Count == 0)
                return;

            var area = chartPressure.ChartAreas["MainArea"];

            // Автопрокрутка только если по X нет зума
            if (!area.AxisX.ScaleView.IsZoomed)
            {
                if (_timeSeconds > TimeWindow)
                {
                    area.AxisX.Minimum = _timeSeconds - TimeWindow;
                    area.AxisX.Maximum = _timeSeconds;
                }
            }
        }

        // ====== двойной клик — сброс зума ======
        private void ChartPressure_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (chartPressure.ChartAreas.Count == 0)
                return;

            var area = chartPressure.ChartAreas["MainArea"];

            area.AxisY.ScaleView.ZoomReset(0);
            area.AxisX.ScaleView.ZoomReset(0);

            if (_timeSeconds <= TimeWindow)
            {
                area.AxisX.Minimum = 0;
                area.AxisX.Maximum = TimeWindow;
            }
            else
            {
                area.AxisX.Minimum = _timeSeconds - TimeWindow;
                area.AxisX.Maximum = _timeSeconds;
            }

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
            // hit-test: проверяем, что мышь над ChartArea
            var hit = chartPressure.HitTest(e.X, e.Y);
            if (hit.ChartArea == null)
            {
                ChartPressure_MouseLeave(sender, EventArgs.Empty);
                return;
            }

            var area = hit.ChartArea;

            if (_seriesCurrent.Points.Count == 0)
                return;

            // конвертируем пиксели в координаты графика
            double xValue = area.AxisX.PixelPositionToValue(e.X);

            // ищем ближайшую точку по X
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
                rate = (pressure - prev) / TimeStep;   // ΔP / Δt
            }

            // обновляем вертикальную и горизонтальную линии
            _cursorLineX.IntervalOffset = p.XValue;   // по X
            _cursorLineY.IntervalOffset = pressure;   // по Y

            // точка на линии
            _cursorMarker.Points.Clear();
            _cursorMarker.Points.AddXY(p.XValue, pressure);

            // обновляем текст в окошке
            _lblInfoTime.Text = $"Time:     {tSec:0.0}s";
            _lblInfoPressure.Text = $"Pressure: {pressure:0.00}";
            _lblInfoTarget.Text = $"Target:   {(target.HasValue ? target.Value.ToString("0.00") : "-")}";
            _lblInfoRate.Text = $"Δ Rate:   {rate:+0.00;-0.00;0}/s";

            // позиционируем панель рядом с курсором
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

        // на случай, если дизайнер ссылается на label1_Click
        private void label1_Click(object sender, EventArgs e) { }
    }
}
