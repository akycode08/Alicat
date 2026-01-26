using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace PrecisionPressureController.UI.Features.Graph.Views
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
        private int _savedScrollPosition = -1; // Сохраненная позиция скролла
        private bool _isUpdating = false; // Флаг обновления, чтобы не сохранять позицию во время обновления

        // Цвета из спецификации
        private static readonly Color BgPanel = Color.FromArgb(15, 23, 42);
        private static readonly Color BgTable = Color.FromArgb(17, 24, 39);
        private static readonly Color BgTableHeader = Color.FromArgb(31, 41, 55);
        private static readonly Color BgTableRowAlt = Color.FromArgb(22, 29, 45);
        private static readonly Color BgInput = Color.FromArgb(26, 31, 46);
        private static readonly Color BgButton = Color.FromArgb(31, 41, 55);
        private static readonly Color TextPrimary = Color.FromArgb(228, 231, 235);
        private static readonly Color TextMuted = Color.FromArgb(107, 114, 128);
        private static readonly Color TextHeader = Color.FromArgb(156, 163, 175);
        private static readonly Color AccentGreen = Color.FromArgb(16, 185, 129);
        private static readonly Color AccentGold = Color.FromArgb(245, 158, 11);
        private static readonly Color AccentGray = Color.FromArgb(107, 114, 128);
        private static readonly Color BorderControl = Color.FromArgb(55, 65, 81);

        public GoToTargetPanel()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupCustomProgressBars();
        }

        private void SetupCustomProgressBars()
        {
            // Цвета ProgressBar устанавливаются в Designer через ForeColor
        }

        private void SetupDataGridView()
        {
            if (dgvPoints != null)
            {
                dgvPoints.CellFormatting += DgvPoints_CellFormatting;
                dgvPoints.RowPrePaint += DgvPoints_RowPrePaint;
                
                // Отслеживаем позицию скролла в реальном времени (когда пользователь прокручивает)
                dgvPoints.Scroll += (s, e) =>
                {
                    if (!_isUpdating && dgvPoints.FirstDisplayedScrollingRowIndex >= 0)
                    {
                        _savedScrollPosition = dgvPoints.FirstDisplayedScrollingRowIndex;
                    }
                };
                
                // Отключаем автоматическую прокрутку при изменении CurrentCell
                // Используем BeginInvoke чтобы избежать реентрантных вызовов
                dgvPoints.CurrentCellChanged += (s, e) =>
                {
                    if (!_isUpdating && IsHandleCreated)
                    {
                        // Используем BeginInvoke для безопасного обновления вне контекста события
                        BeginInvoke(new Action(() =>
                        {
                            if (dgvPoints != null && !dgvPoints.IsDisposed && IsHandleCreated)
                            {
                                try
                                {
                                    dgvPoints.ClearSelection();
                                    dgvPoints.CurrentCell = null;
                                    
                                    // Восстанавливаем позицию скролла, если она была изменена автоматически
                                    if (_savedScrollPosition >= 0 && _savedScrollPosition < dgvPoints.Rows.Count)
                                    {
                                        if (dgvPoints.FirstDisplayedScrollingRowIndex != _savedScrollPosition)
                                        {
                                            dgvPoints.FirstDisplayedScrollingRowIndex = _savedScrollPosition;
                                        }
                                    }
                                }
                                catch
                                {
                                    // Игнорируем ошибки при обновлении
                                }
                            }
                        }));
                    }
                };
                
                // Предотвращаем автоматическую прокрутку при изменении данных
                dgvPoints.RowsAdded += (s, e) =>
                {
                    if (!_isUpdating)
                    {
                        dgvPoints.ClearSelection();
                        // Небольшая задержка для восстановления позиции после добавления строк
                        if (_savedScrollPosition >= 0 && _savedScrollPosition < dgvPoints.Rows.Count && IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                if (dgvPoints != null && !dgvPoints.IsDisposed && IsHandleCreated)
                                {
                                    RestoreScrollPosition();
                                }
                            }));
                        }
                    }
                };
                
                dgvPoints.RowsRemoved += (s, e) => 
                {
                    if (!_isUpdating)
                    {
                        dgvPoints.ClearSelection();
                    }
                };
                
                dgvPoints.CellValueChanged += (s, e) => 
                {
                    if (!_isUpdating)
                    {
                        RestoreScrollPosition();
                    }
                };
            }
        }
        
        private void RestoreScrollPosition()
        {
            if (dgvPoints != null && !_isUpdating && _savedScrollPosition >= 0 && _savedScrollPosition < dgvPoints.Rows.Count)
            {
                try
                {
                    dgvPoints.FirstDisplayedScrollingRowIndex = _savedScrollPosition;
                }
                catch
                {
                    // Игнорируем ошибки при восстановлении позиции
                }
            }
        }

        private void DgvPoints_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            // Подсветка активной строки (если статус ●)
            if (e.RowIndex >= 0 && e.RowIndex < dgvPoints.Rows.Count)
            {
                var row = dgvPoints.Rows[e.RowIndex];
                if (row.Cells["colStatus"]?.Value?.ToString() == "●")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 158, 11, 20); // Полупрозрачный золотой фон
                }
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
                    e.CellStyle.ForeColor = AccentGreen;
                    e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                else if (status == "●")
                {
                    e.CellStyle.ForeColor = AccentGold;
                    e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                else // ○
                {
                    e.CellStyle.ForeColor = AccentGray;
                    e.CellStyle.Font = new Font("Segoe UI", 9F);
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
            if (lblProgressValue != null)
            {
                lblProgressValue.Text = $"{current}/{total}";
            }
            if (progressBar != null)
            {
                if (total > 0)
                {
                    progressBar.Value = Math.Max(0, Math.Min(100, (int)((current / (double)total) * 100)));
                }
                else
                {
                    progressBar.Value = 0;
                }
            }
        }

        public void SetHoldTimer(string timeText, int progressPercent)
        {
            if (lblHoldValue != null)
            {
                lblHoldValue.Text = timeText;
            }
            if (holdProgressBar != null)
            {
                holdProgressBar.Value = Math.Max(0, Math.Min(100, progressPercent));
            }
        }

        public void ClearInputs()
        {
            if (nudPSI != null) nudPSI.Value = 0;
            if (nudHold != null) nudHold.Value = 0;
        }

        private void RefreshDataGridView()
        {
            if (dgvPoints == null) return;
            
            // Сохраняем текущую позицию скролла только если мы не обновляем
            if (!_isUpdating && dgvPoints.FirstDisplayedScrollingRowIndex >= 0)
            {
                _savedScrollPosition = dgvPoints.FirstDisplayedScrollingRowIndex;
            }
            
            _isUpdating = true;
            
            try
            {
                // Временно отключаем обновление и автоматическую прокрутку
                dgvPoints.SuspendLayout();
                dgvPoints.ClearSelection();
                
                // Отключаем выделение и автоматическую прокрутку
                var savedCurrentCell = dgvPoints.CurrentCell;
                dgvPoints.CurrentCell = null;
                
                // Обновляем только изменённые строки, если возможно
                bool needFullRefresh = dgvPoints.Rows.Count != _points.Count;
                
                if (needFullRefresh)
                {
                    dgvPoints.Rows.Clear();
                    foreach (var point in _points)
                    {
                        int rowIndex = dgvPoints.Rows.Add(
                            point.Number,
                            point.PSI.ToString("F1"),
                            $"{point.HoldMinutes}m",
                            point.Status
                        );
                    }
                }
                else
                {
                    // Обновляем только данные, сохраняя позицию скролла
                    for (int i = 0; i < Math.Min(_points.Count, dgvPoints.Rows.Count); i++)
                    {
                        var row = dgvPoints.Rows[i];
                        var point = _points[i];
                        row.Cells["colNum"].Value = point.Number;
                        row.Cells["colPSI"].Value = point.PSI.ToString("F1");
                        row.Cells["colHold"].Value = $"{point.HoldMinutes}m";
                        row.Cells["colStatus"].Value = point.Status;
                    }
                }
                
                // Восстанавливаем позицию скролла сразу
                dgvPoints.ResumeLayout();
                
                // Принудительно восстанавливаем позицию скролла после завершения обновления
                if (_savedScrollPosition >= 0 && _savedScrollPosition < dgvPoints.Rows.Count && IsHandleCreated)
                {
                    // Используем несколько попыток через BeginInvoke
                    BeginInvoke(new Action(() =>
                    {
                        if (dgvPoints != null && !dgvPoints.IsDisposed && _savedScrollPosition < dgvPoints.Rows.Count && IsHandleCreated)
                        {
                            try
                            {
                                // Принудительно устанавливаем позицию
                                dgvPoints.FirstDisplayedScrollingRowIndex = _savedScrollPosition;
                                dgvPoints.ClearSelection();
                                dgvPoints.CurrentCell = null;
                            }
                            catch
                            {
                                // Игнорируем ошибки
                            }
                        }
                    }));
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Подключаем обработчики событий
            if (btnAdd != null)
                btnAdd.Click += (s, ev) => AddClicked?.Invoke(this, EventArgs.Empty);
            if (btnClearAll != null)
                btnClearAll.Click += (s, ev) => ClearAllClicked?.Invoke(this, EventArgs.Empty);
            if (btnEdit != null)
                btnEdit.Click += (s, ev) => EditClicked?.Invoke(this, EventArgs.Empty);
            if (btnStart != null)
                btnStart.Click += (s, ev) => StartClicked?.Invoke(this, EventArgs.Empty);
            if (btnPause != null)
                btnPause.Click += (s, ev) => PauseClicked?.Invoke(this, EventArgs.Empty);
            if (btnStop != null)
                btnStop.Click += (s, ev) => StopClicked?.Invoke(this, EventArgs.Empty);
            if (btnSkip != null)
                btnSkip.Click += (s, ev) => SkipClicked?.Invoke(this, EventArgs.Empty);
        }

        // Удаляем OnPaint - используем стандартный ProgressBar с установкой цвета через ForeColor
    }
}
