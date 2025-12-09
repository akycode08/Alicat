using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm : Form
    {
        // --- поля для графика ---
        private Series _seriesCurrent;
        private Series _seriesTarget;

        // Время в секундах (ось X)
        private double _timeSeconds = 0;
        private const double TimeStep = 0.5;   // шаг 0.5 c
        private const double TimeWindow = 30;  // показываем последние 30 секунд

        public GraphForm()
        {
            InitializeComponent();
            ConfigureChart();
            SetupGridControls();

            chartPressure.MouseDoubleClick += ChartPressure_MouseDoubleClick;
        }

        private void SetupGridControls()
        {
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


        private void ConfigureChart()
        {
            chartPressure.Series.Clear();
            chartPressure.ChartAreas.Clear();
            chartPressure.Legends.Clear();

            // === ChartArea ===
            var area = new ChartArea("MainArea");
            area.BackColor = Color.FromArgb(24, 26, 33);
            area.BorderColor = Color.Transparent;

            if (!area.AxisX.ScaleView.IsZoomed)
            {
                if (_timeSeconds > TimeWindow)
                {
                    const double majorStep = 10.0; // тот же шаг, что и Interval

                    // максимальное значение по X округляем вверх до ближайших 10 сек
                    var max = Math.Ceiling(_timeSeconds / majorStep) * majorStep;
                    var min = max - TimeWindow;

                    area.AxisX.Minimum = min;
                    area.AxisX.Maximum = max;
                }
            }

            // X axis
            area.AxisX.LineWidth = 2;
            area.AxisX.LineColor = Color.FromArgb(60, 60, 70);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(150, 150, 160);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            area.AxisX.Interval = 10;          // шаг сетки и подписей 10 секунд
            area.AxisX.LabelStyle.Format = "0"; // без десятых: 0, 10, 20...
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = TimeWindow;    // 30, как у тебя

            // Y axis
            area.AxisY.LineWidth = 2;
            area.AxisY.LineColor = Color.FromArgb(60, 60, 70);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(150, 150, 160);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(40, 40, 50);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            chartPressure.ChartAreas.Add(area);

            // === Legend ===
            /*var legend = new Legend("Legend1");
            legend.BackColor = Color.Transparent;
            legend.ForeColor = Color.FromArgb(200, 200, 210);
            chartPressure.Legends.Add(legend);
            */

            // === CURRENT LINE ===
            _seriesCurrent = new Series("Current")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.FromArgb(0, 200, 255),
                ChartArea = "MainArea",
                Legend = "Legend1"
            };
            chartPressure.Series.Add(_seriesCurrent);

            // === TARGET LINE ===
            _seriesTarget = new Series("Target")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 214, 69),
                ChartArea = "MainArea",
                Legend = "Legend1"
            };
            chartPressure.Series.Add(_seriesTarget);

            // === MIN LIMIT (yellow dashed) ===
            var sMin = new Series("Min")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.FromArgb(255, 214, 69),
                BorderDashStyle = ChartDashStyle.Dash,
                ChartArea = "MainArea",
                Legend = "Legend1"
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
                Legend = "Legend1"
            };
            chartPressure.Series.Add(sMax);

            // === Zoom ===
            area.AxisY.ScaleView.Zoomable = true;
            area.CursorY.IsUserEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;

            area.AxisX.ScaleView.Zoomable = true;
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;

            // отключаем Series1 из дизайнера
            if (chartPressure.Series.Count > 4)
                chartPressure.Series.RemoveAt(4);
        }


        // === Публичный метод: сюда подаём реальные данные ===
        public void AddSample(double currentPressure, double? targetPressure)
        {
            // если форму уже закрыли/разобрали — ничего не делаем
            if (chartPressure.IsDisposed || _seriesCurrent == null)
                return;

            _seriesCurrent.Points.AddXY(_timeSeconds, currentPressure);

            if (targetPressure.HasValue && _seriesTarget != null)
                _seriesTarget.Points.AddXY(_timeSeconds, targetPressure.Value);

            _timeSeconds += TimeStep;

            // Автопрокрутка окна по X (работаем с ChartArea из дизайнера)
            if (chartPressure.ChartAreas.Count == 0)
                return;

            var area = chartPressure.ChartAreas[0];

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

        private void ChartPressure_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (chartPressure.ChartAreas.Count == 0)
                return;

            var area = chartPressure.ChartAreas[0];

            // Сброс zoom по обеим осям
            area.AxisY.ScaleView.ZoomReset(0);
            area.AxisX.ScaleView.ZoomReset(0);

            // Восстанавливаем наше "окно" по X
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

            // По Y пусть авто-подбор сделает красивый диапазон
            area.RecalculateAxesScale();
        }

        private void panelEmergencyHost_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblTimeWindowTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblXStep_Click(object sender, EventArgs e)
        {

        }
    }
}
