using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            chartPressure.MouseDoubleClick += ChartPressure_MouseDoubleClick;

        }

        private void ConfigureChart()
        {
            // чистим всё
            chartPressure.Series.Clear();
            chartPressure.ChartAreas.Clear();
            chartPressure.Legends.Clear();

            // ==== DARK THEME ====
            chartPressure.BackColor = Color.FromArgb(20, 20, 20);

            var area = new ChartArea("MainArea");
            area.BackColor = Color.FromArgb(20, 20, 20);

            area.AxisX.Title = "Time, s";
            area.AxisY.Title = "Pressure, PSI";

            var gridColor = Color.FromArgb(60, 60, 60);
            area.AxisX.MajorGrid.LineColor = gridColor;
            area.AxisY.MajorGrid.LineColor = gridColor;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisX.LineColor = Color.LightGray;
            area.AxisY.LineColor = Color.LightGray;

            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisX.TitleForeColor = Color.White;
            area.AxisY.TitleForeColor = Color.White;

            // окно по оси X
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = TimeWindow;

            chartPressure.ChartAreas.Add(area);

            // Легенда
            var legend = new Legend("MainLegend");
            legend.BackColor = Color.FromArgb(20, 20, 20);
            legend.ForeColor = Color.White;
            chartPressure.Legends.Add(legend);

            // Текущее давление
            _seriesCurrent = new Series("Current pressure")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.DeepSkyBlue,
                ChartArea = "MainArea",
                Legend = "MainLegend",
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double
            };

            // Уставка
            _seriesTarget = new Series("Target pressure")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.Gold,
                ChartArea = "MainArea",
                Legend = "MainLegend",
                XValueType = ChartValueType.Double,
                YValueType = ChartValueType.Double
            };

            chartPressure.Series.Add(_seriesCurrent);
            chartPressure.Series.Add(_seriesTarget);

            // === Zoom по оси Y ===
            area.AxisY.ScaleView.Zoomable = true;          // можно масштабировать
            area.CursorY.IsUserEnabled = true;             // включаем курсор
            area.CursorY.IsUserSelectionEnabled = true;    // разрешаем выделение мышкой

            // === Zoom по оси X ===
            area.AxisX.ScaleView.Zoomable = true;
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;

        }

        // === Публичный метод: потом сюда подадим реальные данные ===
        public void AddSample(double currentPressure, double? targetPressure)
        {
            // если форму уже закрыли/разобрали — ничего не делаем
            if (chartPressure.IsDisposed || _seriesCurrent == null)
                return;

            _seriesCurrent.Points.AddXY(_timeSeconds, currentPressure);

            if (targetPressure.HasValue && _seriesTarget != null)
                _seriesTarget.Points.AddXY(_timeSeconds, targetPressure.Value);

            _timeSeconds += TimeStep;

            // Автопрокрутка окна по X
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

        private void ChartPressure_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            var area = chartPressure.ChartAreas["MainArea"];

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


    }
}