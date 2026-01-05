using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Alicat.UI.Features.Graph.Views
{
    /// <summary>
    /// UserControl для панели GO TO TARGET
    /// Точный дизайн согласно спецификации
    /// </summary>
    public partial class GoToTargetPanel : UserControl
    {
        // Модель данных для точки
        public class TargetPoint
        {
            public int Number { get; set; }
            public double PSI { get; set; }
            public int HoldMinutes { get; set; }
            public string Status { get; set; } = "○"; // ○ wait, ● active, ✓ done
        }

        private List<TargetPoint> _points = new List<TargetPoint>();

        public GoToTargetPanel()
        {
            InitializeComponent();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            // Уже настроено в Designer, но можем добавить обработчики событий
            if (dgvPoints != null)
            {
                dgvPoints.CellFormatting += DgvPoints_CellFormatting;
            }
        }

        private void DgvPoints_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvPoints.Columns["colStatus"].Index && e.RowIndex >= 0 && e.CellStyle != null)
            {
                string status = e.Value?.ToString() ?? "";
                // Устанавливаем цвет текста в зависимости от статуса
                if (status == "✓")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);  // Зелёный
                }
                else if (status == "●")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(245, 158, 11);  // Золотой
                }
                else // ○
                {
                    e.CellStyle.ForeColor = Color.FromArgb(107, 114, 128);  // Серый
                }
            }
        }

        // События для внешнего использования
        public event EventHandler? AddClicked;
        public event EventHandler? ClearAllClicked;
        public event EventHandler? EditClicked;
        public event EventHandler? StartClicked;
        public event EventHandler? PauseClicked;
        public event EventHandler? StopClicked;
        public event EventHandler? SkipClicked;

        // Свойства для доступа к значениям
        public double PSIValue => nudPSI != null ? (double)nudPSI.Value : 0;
        public int HoldValue => nudHold != null ? (int)nudHold.Value : 0;

        // Методы для обновления данных
        public void SetPoints(List<TargetPoint> points)
        {
            _points = points;
            RefreshDataGridView();
        }

        public void SetProgress(int current, int total)
        {
            lblProgressValue.Text = $"{current}/{total}";
            if (total > 0)
            {
                progressBar.Value = (int)((current / (double)total) * 100);
            }
            else
            {
                progressBar.Value = 0;
            }
        }

        public void SetHoldTimer(string timeText, int progressPercent)
        {
            lblHoldValue.Text = timeText;
            holdProgressBar.Value = Math.Max(0, Math.Min(100, progressPercent));
        }

        private void RefreshDataGridView()
        {
            if (dgvPoints == null) return;
            
            dgvPoints.Rows.Clear();
            foreach (var point in _points)
            {
                dgvPoints.Rows.Add(
                    point.Number,
                    point.PSI.ToString("F1"),
                    $"{point.HoldMinutes}m",
                    point.Status
                );
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Подключаем обработчики событий
            btnAdd.Click += (s, ev) => AddClicked?.Invoke(this, EventArgs.Empty);
            btnClearAll.Click += (s, ev) => ClearAllClicked?.Invoke(this, EventArgs.Empty);
            btnEdit.Click += (s, ev) => EditClicked?.Invoke(this, EventArgs.Empty);
            btnStart.Click += (s, ev) => StartClicked?.Invoke(this, EventArgs.Empty);
            btnPause.Click += (s, ev) => PauseClicked?.Invoke(this, EventArgs.Empty);
            btnStop.Click += (s, ev) => StopClicked?.Invoke(this, EventArgs.Empty);
            btnSkip.Click += (s, ev) => SkipClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

