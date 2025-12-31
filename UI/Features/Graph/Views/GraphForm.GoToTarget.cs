using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Globalization;

namespace Alicat.UI.Features.Graph.Views
{
    // Модель данных для цели
    public class TargetItem
    {
        public int Number { get; set; }
        public double PSI { get; set; }
        public int HoldMinutes { get; set; }
        public TargetStatus Status { get; set; }
    }

    public enum TargetStatus
    {
        Waiting,    // Ожидает
        Active,     // Активна
        Completed   // Завершена
    }

    public enum SequenceState
    {
        Stopped,
        Playing,
        Paused
    }

    public partial class GraphForm
    {
        // Данные для GO TO TARGET
        private List<TargetItem> _targets = new List<TargetItem>();
        private int _currentTargetIndex = -1;
        private SequenceState _sequenceState = SequenceState.Stopped;
        private System.Windows.Forms.Timer? _holdTimer;
        private DateTime _holdStartTime;
        private int _holdDurationSeconds = 0;
        // Используем _targetHandler из GraphForm.HeaderFooter.cs

        private void InitializeGoToTarget()
        {
            if (dgvTargets == null) return;

            // Настраиваем колонки таблицы
            dgvTargets.Columns.Clear();

            // Колонка #
            var colNumber = new DataGridViewTextBoxColumn
            {
                Name = "colNumber",
                HeaderText = "#",
                Width = 30,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False
            };

            // Колонка PSI
            var colPSI = new DataGridViewTextBoxColumn
            {
                Name = "colPSI",
                HeaderText = "PSI",
                Width = 40,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False
            };

            // Колонка Hold
            var colHold = new DataGridViewTextBoxColumn
            {
                Name = "colHold",
                HeaderText = "Hold",
                Width = 40,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False
            };

            // Колонка Status
            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                Width = 40,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False
            };

            // Колонка DELETE (с иконкой корзины)
            var colDelete = new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "DELETE",
                Width = 50,
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                Resizable = DataGridViewTriState.False
            };

            dgvTargets.Columns.AddRange(new DataGridViewColumn[] 
            { 
                colNumber, 
                colPSI, 
                colHold, 
                colStatus, 
                colDelete 
            });

            // Настройка стилей для строк
            dgvTargets.RowsDefaultCellStyle.BackColor = Color.FromArgb(21, 23, 28);
            dgvTargets.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(25, 27, 32);

            // Обработчик клика по кнопке DELETE (определен ниже)
            dgvTargets.CellContentClick += DgvTargets_CellContentClick;

            // Обработчик отрисовки ячеек для Status (круг)
            dgvTargets.CellFormatting += DgvTargets_CellFormatting;
            dgvTargets.CellPainting += DgvTargets_CellPainting;

            // Подключаем обработчики кнопок
            if (btnAddTarget != null)
                btnAddTarget.Click += BtnAddTarget_Click;
            
            if (btnClearAll != null)
                btnClearAll.Click += BtnClearAll_Click;
            
            if (btnPlay != null)
                btnPlay.Click += BtnPlay_Click;
            
            if (btnPauseTarget != null)
                btnPauseTarget.Click += BtnPauseTarget_Click;
            
            if (btnStop != null)
                btnStop.Click += BtnStop_Click;
            
            if (btnSkip != null)
                btnSkip.Click += BtnSkip_Click;

            // Инициализируем таймер для Hold
            _holdTimer = new System.Windows.Forms.Timer { Interval = 1000 }; // 1 секунда
            _holdTimer.Tick += HoldTimer_Tick;

            // Обновляем UI
            UpdateTargetsTable();
            UpdateProgress();
        }

        // SetTargetHandler уже определен в GraphForm.HeaderFooter.cs
        // _targetHandler также определен там, используем его напрямую

        private void DgvTargets_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var column = dgvTargets.Columns[e.ColumnIndex];
            if (column.Name == "colStatus")
            {
                // Устанавливаем текст для Status (● или ○)
                var row = dgvTargets.Rows[e.RowIndex];
                if (row.Index == 0) // Первая строка - активная
                {
                    e.Value = "●";
                    e.CellStyle.ForeColor = Color.FromArgb(255, 152, 0); // Оранжевый
                }
                else
                {
                    e.Value = "○";
                    e.CellStyle.ForeColor = Color.White;
                }
                e.FormattingApplied = true;
            }
        }

        private void DgvTargets_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Подсветка активной строки (первая строка - оранжевый фон)
            if (e.RowIndex == 0)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 152, 0);
                e.CellStyle.ForeColor = Color.White;
            }
            else
            {
                e.CellStyle.BackColor = Color.FromArgb(21, 23, 28);
                e.CellStyle.ForeColor = Color.White;
            }
        }

        // ====================================================================
        // ОБРАБОТЧИКИ КНОПОК
        // ====================================================================

        private void BtnAddTarget_Click(object? sender, EventArgs e)
        {
            if (txtPSI == null || txtHold == null) return;

            // Парсим PSI
            if (!double.TryParse(txtPSI.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double psi))
            {
                MessageBox.Show("Invalid PSI value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Парсим Hold (минуты)
            if (!int.TryParse(txtHold.Text, out int holdMinutes) || holdMinutes < 0)
            {
                MessageBox.Show("Invalid Hold value. Must be a positive integer (minutes).", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Добавляем новую цель
            var newTarget = new TargetItem
            {
                Number = _targets.Count + 1,
                PSI = psi,
                HoldMinutes = holdMinutes,
                Status = TargetStatus.Waiting
            };

            _targets.Add(newTarget);
            UpdateTargetsTable();
            UpdateProgress();

            // Очищаем поля ввода
            txtPSI.Text = "0";
            txtHold.Text = "0";
        }

        private void BtnClearAll_Click(object? sender, EventArgs e)
        {
            if (_targets.Count == 0) return;

            var result = MessageBox.Show(
                "Are you sure you want to clear all targets?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopSequence();
                _targets.Clear();
                _currentTargetIndex = -1;
                UpdateTargetsTable();
                UpdateProgress();
            }
        }

        private void BtnPlay_Click(object? sender, EventArgs e)
        {
            if (_targets.Count == 0)
            {
                MessageBox.Show("No targets to execute. Add targets first.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_sequenceState == SequenceState.Stopped)
            {
                // Начинаем с первой цели
                _currentTargetIndex = 0;
                StartCurrentTarget();
            }
            else if (_sequenceState == SequenceState.Paused)
            {
                // Продолжаем с текущей цели
                ResumeCurrentTarget();
            }

            _sequenceState = SequenceState.Playing;
            UpdateControlButtons();
        }

        private void BtnPauseTarget_Click(object? sender, EventArgs e)
        {
            if (_sequenceState == SequenceState.Playing)
            {
                _sequenceState = SequenceState.Paused;
                if (_holdTimer != null)
                    _holdTimer.Stop();
                UpdateControlButtons();
            }
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            StopSequence();
        }

        private void BtnSkip_Click(object? sender, EventArgs e)
        {
            if (_sequenceState == SequenceState.Playing || _sequenceState == SequenceState.Paused)
            {
                // Пропускаем текущую цель и переходим к следующей
                MoveToNextTarget();
            }
        }

        // ====================================================================
        // УПРАВЛЕНИЕ ПОСЛЕДОВАТЕЛЬНОСТЬЮ
        // ====================================================================

        private void StartCurrentTarget()
        {
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count) return;

            var target = _targets[_currentTargetIndex];
            target.Status = TargetStatus.Active;

            // Устанавливаем целевое давление через обработчик без подтверждения
            if (_targetHandlerSilent != null)
            {
                _targetHandlerSilent(target.PSI);
            }
            else if (_targetHandler != null)
            {
                // Fallback на обычный обработчик, если silent не установлен
                _targetHandler(target.PSI);
            }

            // Запускаем таймер Hold
            _holdDurationSeconds = target.HoldMinutes * 60;
            _holdStartTime = DateTime.Now;
            
            if (_holdTimer != null)
            {
                _holdTimer.Start();
            }

            UpdateTargetsTable();
            UpdateProgress();
            UpdateHoldTimer();
        }

        private void ResumeCurrentTarget()
        {
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count) return;

            // Возобновляем таймер с учетом уже прошедшего времени
            var elapsed = (DateTime.Now - _holdStartTime).TotalSeconds;
            _holdStartTime = DateTime.Now.AddSeconds(-elapsed);
            
            if (_holdTimer != null)
            {
                _holdTimer.Start();
            }
        }

        private void StopSequence()
        {
            _sequenceState = SequenceState.Stopped;
            
            if (_holdTimer != null)
            {
                _holdTimer.Stop();
            }

            // Сбрасываем статусы всех целей
            foreach (var target in _targets)
            {
                if (target.Status == TargetStatus.Active)
                    target.Status = TargetStatus.Waiting;
            }

            _currentTargetIndex = -1;
            UpdateTargetsTable();
            UpdateProgress();
            UpdateControlButtons();
            UpdateHoldTimer();
        }

        private void MoveToNextTarget()
        {
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count) return;

            // Помечаем текущую цель как завершенную
            _targets[_currentTargetIndex].Status = TargetStatus.Completed;

            // Переходим к следующей цели
            _currentTargetIndex++;

            if (_currentTargetIndex >= _targets.Count)
            {
                // Все цели завершены
                StopSequence();
                MessageBox.Show("All targets completed!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Запускаем следующую цель
                StartCurrentTarget();
            }
        }

        // ====================================================================
        // ТАЙМЕР HOLD
        // ====================================================================

        private void HoldTimer_Tick(object? sender, EventArgs e)
        {
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count)
            {
                if (_holdTimer != null)
                    _holdTimer.Stop();
                return;
            }

            var elapsed = (DateTime.Now - _holdStartTime).TotalSeconds;
            var remaining = _holdDurationSeconds - elapsed;

            if (remaining <= 0)
            {
                // Hold время истекло, переходим к следующей цели
                if (_holdTimer != null)
                    _holdTimer.Stop();
                
                MoveToNextTarget();
            }
            else
            {
                UpdateHoldTimer();
            }
        }

        // ====================================================================
        // ОБНОВЛЕНИЕ UI
        // ====================================================================

        private void UpdateTargetsTable()
        {
            if (dgvTargets == null) return;

            dgvTargets.Rows.Clear();

            for (int i = 0; i < _targets.Count; i++)
            {
                var target = _targets[i];
                var statusSymbol = target.Status switch
                {
                    TargetStatus.Active => "●",
                    TargetStatus.Completed => "✓",
                    _ => "○"
                };

                var holdText = target.HoldMinutes > 0 ? $"{target.HoldMinutes}m" : "0m";
                dgvTargets.Rows.Add(
                    target.Number.ToString(),
                    target.PSI.ToString("F1"),
                    holdText,
                    statusSymbol,
                    "" // DELETE button
                );

                // Устанавливаем оранжевый фон для активной строки
                if (target.Status == TargetStatus.Active)
                {
                    foreach (DataGridViewCell cell in dgvTargets.Rows[i].Cells)
                    {
                        cell.Style.BackColor = Color.FromArgb(255, 152, 0);
                        cell.Style.ForeColor = Color.White;
                    }
                }
                else
                {
                    foreach (DataGridViewCell cell in dgvTargets.Rows[i].Cells)
                    {
                        cell.Style.BackColor = Color.FromArgb(21, 23, 28);
                        cell.Style.ForeColor = Color.White;
                    }
                }
            }
        }

        private void UpdateProgress()
        {
            if (lblProgress == null || progressBarProgress == null) return;

            int completed = _targets.Count(t => t.Status == TargetStatus.Completed);
            int total = _targets.Count;

            lblProgress.Text = $"Progress:        {completed}/{total}";
            
            if (total > 0)
            {
                progressBarProgress.Maximum = total;
                progressBarProgress.Value = completed;
            }
            else
            {
                progressBarProgress.Value = 0;
            }
        }

        private void UpdateHoldTimer()
        {
            if (lblHoldTimer == null || progressBarHold == null) return;

            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count || 
                _sequenceState == SequenceState.Stopped)
            {
                lblHoldTimer.Text = "Hold:           00:00";
                progressBarHold.Value = 0;
                return;
            }

            var elapsed = (DateTime.Now - _holdStartTime).TotalSeconds;
            var remaining = Math.Max(0, _holdDurationSeconds - elapsed);

            int minutes = (int)(remaining / 60);
            int seconds = (int)(remaining % 60);

            lblHoldTimer.Text = $"Hold:           {minutes:D2}:{seconds:D2}";

            // Обновляем прогресс-бар Hold
            if (_holdDurationSeconds > 0)
            {
                progressBarHold.Maximum = _holdDurationSeconds;
                progressBarHold.Value = (int)elapsed;
            }
        }

        private void UpdateControlButtons()
        {
            if (btnPlay == null || btnPauseTarget == null || btnStop == null || btnSkip == null) return;

            bool isPlaying = _sequenceState == SequenceState.Playing;
            bool isPaused = _sequenceState == SequenceState.Paused;
            bool isStopped = _sequenceState == SequenceState.Stopped;

            btnPlay.Enabled = isStopped || isPaused;
            btnPauseTarget.Enabled = isPlaying;
            btnStop.Enabled = isPlaying || isPaused;
            btnSkip.Enabled = isPlaying || isPaused;
        }

        // ====================================================================
        // ОБРАБОТЧИКИ ТАБЛИЦЫ
        // ====================================================================

        private void DgvTargets_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dgvTargets.Columns[e.ColumnIndex];
            if (column.Name == "colDelete")
            {
                // Нельзя удалять активную цель
                if (e.RowIndex == _currentTargetIndex && _sequenceState == SequenceState.Playing)
                {
                    MessageBox.Show("Cannot delete active target. Stop the sequence first.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Удаляем цель
                _targets.RemoveAt(e.RowIndex);
                
                // Обновляем номера
                for (int i = 0; i < _targets.Count; i++)
                {
                    _targets[i].Number = i + 1;
                }

                // Если удалили текущую цель, останавливаем последовательность
                if (e.RowIndex == _currentTargetIndex)
                {
                    StopSequence();
                }
                else if (e.RowIndex < _currentTargetIndex)
                {
                    // Если удалили цель до текущей, уменьшаем индекс
                    _currentTargetIndex--;
                }

                UpdateTargetsTable();
                UpdateProgress();
            }
        }
    }
}

