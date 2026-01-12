using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Text.Json.Nodes;

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
        // Используем SequenceService для выполнения последовательности в фоне
        private Alicat.Services.Sequence.SequenceService? _sequenceService;
        
        private List<TargetItem> _targets = new List<TargetItem>();
        private int _currentTargetIndex = -1;
        private SequenceState _sequenceState = SequenceState.Stopped;
        private int _savedScrollPosition = -1; // Сохраненная позиция скролла
        private bool _isUpdatingTable = false; // Флаг обновления таблицы
        private System.Windows.Forms.Timer? _holdTimer;
        private DateTime _holdStartTime;
        private int _holdDurationSeconds = 0;
        private bool _holdTimerStarted = false; // Флаг: начал ли Hold timer отсчет
        // Используем _targetHandler из GraphForm.HeaderFooter.cs

        /// <summary>
        /// Устанавливает SequenceService для синхронизации с фоновым выполнением
        /// </summary>
        public void SetSequenceService(Alicat.Services.Sequence.SequenceService service)
        {
            _sequenceService = service;
            
            // Подписываемся на события сервиса
            if (_sequenceService != null)
            {
                _sequenceService.OnSequenceStateChanged += OnSequenceServiceStateChanged;
                _sequenceService.OnTargetChanged += OnSequenceServiceTargetChanged;
                
                // Синхронизируем targets из сервиса только если DataGridView уже инициализирован
                if (dgvTargets != null && dgvTargets.Columns.Count > 0)
                {
                    SyncTargetsFromService();
                    
                    // Запускаем таймер для обновления UI, если последовательность активна
                    if (_sequenceService.State == SequenceState.Playing && _holdTimer != null && !_holdTimer.Enabled)
                    {
                        _holdTimer.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Синхронизирует targets из SequenceService
        /// Обновляет таблицу только при реальных изменениях (статус, индекс, состояние)
        /// </summary>
        private void SyncTargetsFromService()
        {
            if (_sequenceService == null) return;
            
            // Проверяем, что DataGridView инициализирован
            if (dgvTargets == null || dgvTargets.Columns.Count == 0) return;

            // Сохраняем предыдущие значения для сравнения
            int previousTargetIndex = _currentTargetIndex;
            SequenceState previousState = _sequenceState;
            var previousTargets = new List<(int Number, TargetStatus Status)>();
            foreach (var t in _targets)
            {
                previousTargets.Add((t.Number, t.Status));
            }

            // Обновляем данные
            _targets.Clear();
            foreach (var target in _sequenceService.Targets)
            {
                _targets.Add(new TargetItem
                {
                    Number = target.Number,
                    PSI = target.PSI,
                    HoldMinutes = target.HoldMinutes,
                    Status = target.Status
                });
            }

            _currentTargetIndex = _sequenceService.CurrentTargetIndex;
            _sequenceState = _sequenceService.State;

            // Проверяем, были ли реальные изменения
            bool targetsChanged = _targets.Count != previousTargets.Count;
            if (!targetsChanged)
            {
                for (int i = 0; i < _targets.Count; i++)
                {
                    if (i < previousTargets.Count)
                    {
                        if (_targets[i].Number != previousTargets[i].Number || 
                            _targets[i].Status != previousTargets[i].Status)
                        {
                            targetsChanged = true;
                            break;
                        }
                    }
                    else
                    {
                        targetsChanged = true;
                        break;
                    }
                }
            }

            bool stateChanged = previousTargetIndex != _currentTargetIndex || previousState != _sequenceState;

            // Обновляем таблицу ТОЛЬКО при реальных изменениях
            if (targetsChanged || stateChanged)
            {
                UpdateTargetsTable();
            }

            // Управляем таймером UI в зависимости от состояния последовательности
            if (_holdTimer != null)
            {
                if (_sequenceState == SequenceState.Playing && !_holdTimer.Enabled)
                {
                    _holdTimer.Start();
                }
                else if (_sequenceState == SequenceState.Stopped && _holdTimer.Enabled)
                {
                    _holdTimer.Stop();
                }
            }

            // Прогресс и кнопки обновляем всегда (но это не затрагивает таблицу)
            UpdateProgress();
            UpdateControlButtons();
        }

        /// <summary>
        /// Обработчик изменения состояния последовательности в сервисе
        /// </summary>
        private void OnSequenceServiceStateChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnSequenceServiceStateChanged));
                return;
            }

            SyncTargetsFromService();
        }

        /// <summary>
        /// Обработчик изменения target в сервисе
        /// </summary>
        private void OnSequenceServiceTargetChanged(int index, TargetItem target)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnSequenceServiceTargetChanged(index, target)));
                return;
            }

            SyncTargetsFromService();
        }

        /// <summary>
        /// Обновляет состояние последовательности в UI (вызывается извне)
        /// </summary>
        public void RefreshSequenceState()
        {
            SyncTargetsFromService();
        }

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

            // Колонка DELETE удалена - доступна только в модальном окне Sequence Editor
            dgvTargets.Columns.AddRange(new DataGridViewColumn[] 
            { 
                colNumber, 
                colPSI, 
                colHold, 
                colStatus
            });

            // Настройка стилей для строк
            dgvTargets.RowsDefaultCellStyle.BackColor = Color.FromArgb(21, 23, 28);
            dgvTargets.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(25, 27, 32);

            // Обработчик клика по кнопке DELETE удален - удаление доступно только в модальном окне

            // Обработчик отрисовки ячеек для Status (круг)
            dgvTargets.CellFormatting += DgvTargets_CellFormatting;
            dgvTargets.CellPainting += DgvTargets_CellPainting;

            // Подключаем обработчики кнопок
            if (btnAddTarget != null)
                btnAddTarget.Click += BtnAddTarget_Click;
            
            
            if (btnClearAll != null)
                btnClearAll.Click += BtnClearAll_Click;
            
            if (btnEdit != null)
                btnEdit.Click += BtnEdit_Click;
            
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

            // Если SequenceService установлен, синхронизируем с ним (не загружаем из файла)
            if (_sequenceService != null)
            {
                SyncTargetsFromService();
                
                // Запускаем таймер для обновления UI, если последовательность активна
                if (_sequenceService.State == SequenceState.Playing && !_holdTimer.Enabled)
                {
                    _holdTimer.Start();
                }
            }
            else
            {
                // Загружаем сохраненные targets из файла настроек (старый способ)
                LoadTargetsFromFile();
                RestoreSequenceState();
            }

            // Настраиваем отслеживание скролла
            if (dgvTargets != null)
            {
                // Отслеживаем позицию скролла в реальном времени (когда пользователь прокручивает)
                dgvTargets.Scroll += (s, e) =>
                {
                    if (!_isUpdatingTable && dgvTargets.FirstDisplayedScrollingRowIndex >= 0)
                    {
                        _savedScrollPosition = dgvTargets.FirstDisplayedScrollingRowIndex;
                    }
                };
                
                // Отключаем автоматическую прокрутку при изменении CurrentCell
                // Используем BeginInvoke чтобы избежать реентрантных вызовов
                dgvTargets.CurrentCellChanged += (s, e) =>
                {
                    if (!_isUpdatingTable && IsHandleCreated)
                    {
                        // Используем BeginInvoke для безопасного обновления вне контекста события
                        BeginInvoke(new Action(() =>
                        {
                            if (dgvTargets != null && !dgvTargets.IsDisposed && IsHandleCreated)
                            {
                                try
                                {
                                    dgvTargets.ClearSelection();
                                    dgvTargets.CurrentCell = null;
                                    
                                    // Восстанавливаем позицию скролла, если она была изменена автоматически
                                    if (_savedScrollPosition >= 0 && _savedScrollPosition < dgvTargets.Rows.Count)
                                    {
                                        if (dgvTargets.FirstDisplayedScrollingRowIndex != _savedScrollPosition)
                                        {
                                            dgvTargets.FirstDisplayedScrollingRowIndex = _savedScrollPosition;
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
            }
            
            // Обновляем UI
            UpdateTargetsTable();
            UpdateProgress();
            
            // Отключаем автоматическое выделение строк (чтобы не было золотой подсветки)
            if (dgvTargets != null)
            {
                dgvTargets.ClearSelection();
                // Используем BeginInvoke только если handle создан
                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (dgvTargets != null && !dgvTargets.IsDisposed && IsHandleCreated)
                        {
                            try
                            {
                                dgvTargets.CurrentCell = null;
                            }
                            catch
                            {
                                // Игнорируем ошибки
                            }
                        }
                    }));
                }
            }
            
            // Initialize Hold timer to 00:00
            if (lblHoldTimer != null)
            {
                lblHoldTimer.Text = "Hold:           00:00";
            }
            if (progressBarHold != null)
            {
                progressBarHold.Value = 0;
            }
        }

        // SetTargetHandler уже определен в GraphForm.HeaderFooter.cs
        // _targetHandler также определен там, используем его напрямую

        private void DgvTargets_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _targets.Count) return;

            var column = dgvTargets.Columns[e.ColumnIndex];
            if (column.Name == "colStatus")
            {
                // Используем реальный статус из _targets
                var target = _targets[e.RowIndex];
                
                // Устанавливаем символ и цвет статуса согласно CSS схеме
                switch (target.Status)
                {
                    case TargetStatus.Completed:
                        e.Value = "✓";
                        // --accent-green: #10b981
                        e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                        break;
                    case TargetStatus.Active:
                        e.Value = "●";
                        // --accent-gold: #f59e0b
                        e.CellStyle.ForeColor = Color.FromArgb(245, 158, 11);
                        break;
                    default: // Waiting
                        e.Value = "○";
                        // --text-muted: #6b7280
                        e.CellStyle.ForeColor = Color.FromArgb(107, 114, 128);
                        break;
                }
                e.FormattingApplied = true;
            }
        }

        private void DgvTargets_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _targets.Count) return;

            // Все строки имеют одинаковый темный фон - подсветка фона удалена
            // НЕ перезаписываем ForeColor здесь, чтобы не перезаписать цвета статусов из CellFormatting
            e.CellStyle.BackColor = Color.FromArgb(21, 23, 28);
            
            // Устанавливаем ForeColor только для ячеек, которые НЕ являются колонкой Status
            // (цвета Status устанавливаются в CellFormatting)
            var column = dgvTargets.Columns[e.ColumnIndex];
            if (column.Name != "colStatus")
            {
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
            
            // Обновляем SequenceService, если он установлен
            if (_sequenceService != null)
            {
                _sequenceService.SetTargets(_targets);
            }
            else
            {
                SaveTargetsToFile(); // Сохраняем в файл настроек (старый способ)
            }
            
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
                
                // Обновляем SequenceService, если он установлен
                if (_sequenceService != null)
                {
                    _sequenceService.SetTargets(_targets);
                    _sequenceService.Stop();
                }
                else
                {
                    SaveTargetsToFile(); // Сохраняем в файл настроек (старый способ)
                }
                
                UpdateTargetsTable();
                UpdateProgress();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            // Останавливаем последовательность перед редактированием (безопасность)
            if (_sequenceState != SequenceState.Stopped)
            {
                var result = MessageBox.Show(
                    "Sequence is active. Stop the sequence before editing?",
                    "Stop Sequence",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    StopSequence();
                }
                else
                {
                    return; // Пользователь отменил
                }
            }

            // Получаем тему из GraphForm (используем internal поле)
            bool isDarkTheme = _isDarkTheme;

            // Открываем модальное окно
            using var modal = new SequenceEditorModal(_targets, isDarkTheme);
            if (modal.ShowDialog(this) == DialogResult.OK && modal.DialogResultApplied)
            {
                // Применяем изменения
                _targets = modal.Points;
                
                // Обновляем номера
                for (int i = 0; i < _targets.Count; i++)
                {
                    _targets[i].Number = i + 1;
                    _targets[i].Status = TargetStatus.Waiting; // Все точки сбрасываются в Waiting
                }

                // Обновляем SequenceService, если он установлен
                if (_sequenceService != null)
                {
                    _sequenceService.SetTargets(_targets);
                }
                else
                {
                    SaveTargetsToFile(); // Сохраняем в файл настроек (старый способ)
                }
                
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

            // Используем SequenceService, если он установлен
            if (_sequenceService != null)
            {
                _sequenceService.SetTargets(_targets);
                _sequenceService.Start();
                SyncTargetsFromService();
            }
            else
            {
                // Старый способ (если сервис не установлен)
                if (_sequenceState == SequenceState.Stopped)
                {
                    _currentTargetIndex = 0;
                    StartCurrentTarget();
                }
                else if (_sequenceState == SequenceState.Paused)
                {
                    ResumeCurrentTarget();
                }

                _sequenceState = SequenceState.Playing;
                UpdateControlButtons();
            }
        }

        private void BtnPauseTarget_Click(object? sender, EventArgs e)
        {
            if (_sequenceService != null)
            {
                _sequenceService.Pause();
                SyncTargetsFromService();
            }
            else
            {
                // Старый способ
                if (_sequenceState == SequenceState.Playing)
                {
                    _sequenceState = SequenceState.Paused;
                    if (_holdTimer != null)
                        _holdTimer.Stop();
                    UpdateControlButtons();
                }
            }
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            if (_sequenceService != null)
            {
                _sequenceService.Stop();
                SyncTargetsFromService();
            }
            else
            {
                StopSequence();
            }
        }

        private void BtnSkip_Click(object? sender, EventArgs e)
        {
            if (_sequenceService != null)
            {
                _sequenceService.Skip();
                SyncTargetsFromService();
            }
            else
            {
                // Старый способ
                if (_sequenceState == SequenceState.Playing || _sequenceState == SequenceState.Paused)
                {
                    MoveToNextTarget();
                }
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
            SaveTargetsToFile(); // Сохраняем статус

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

            // Запускаем таймер Hold (но он начнет отсчет только после достижения цели)
            _holdDurationSeconds = target.HoldMinutes * 60;
            _holdStartTime = DateTime.Now; // Будет сброшен при достижении цели
            _holdTimerStarted = false; // Таймер начнет отсчет только после достижения цели
            
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
            _holdTimerStarted = false;
            SaveTargetsToFile(); // Сохраняем изменения
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
            SaveTargetsToFile(); // Сохраняем статус

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
            // Если используется SequenceService, обновляем только Hold Timer, НЕ всю таблицу
            if (_sequenceService != null)
            {
                // Обновляем только Hold Timer UI, таблицу обновляем только при реальных изменениях
                UpdateHoldTimerFromService();
                return;
            }

            // Старый способ (если сервис не установлен)
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count)
            {
                if (_holdTimer != null)
                    _holdTimer.Stop();
                return;
            }

            var target = _targets[_currentTargetIndex];
            
            // Получаем текущее давление из DataStore
            double currentPressure = GetCurrentPressure();
            
            // Проверяем, достигли ли мы целевого давления (tolerance ±2 PSI)
            const double tolerance = 2.0;
            bool atTarget = Math.Abs(currentPressure - target.PSI) <= tolerance;

            // Если ещё не достигли цели, не запускаем Hold timer
            if (!atTarget)
            {
                _holdTimerStarted = false;
                // Обновляем таймер с текстом "Approaching..."
                if (lblHoldTimer != null)
                {
                    lblHoldTimer.Text = "Hold:      Approaching...";
                }
                if (progressBarHold != null)
                {
                    progressBarHold.Value = 0;
                }
                return;
            }

            // Достигли цели - запускаем Hold timer (если ещё не запущен)
            if (!_holdTimerStarted)
            {
                _holdTimerStarted = true;
                _holdStartTime = DateTime.Now; // Начинаем отсчет с момента достижения цели
            }

            // Продолжаем Hold timer
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

        /// <summary>
        /// Обновляет Hold Timer UI из SequenceService
        /// </summary>
        private void UpdateHoldTimerFromService()
        {
            if (_sequenceService == null || _sequenceService.CurrentTargetIndex < 0)
            {
                if (lblHoldTimer != null)
                    lblHoldTimer.Text = "Hold:           00:00";
                if (progressBarHold != null)
                    progressBarHold.Value = 0;
                return;
            }

            // Проверяем, достигли ли цели
            bool isAtTarget = _sequenceService.IsAtTarget;

            if (!isAtTarget)
            {
                // Ещё не достигли цели
                if (lblHoldTimer != null)
                    lblHoldTimer.Text = "Hold:      Approaching...";
                if (progressBarHold != null)
                    progressBarHold.Value = 0;
            }
            else
            {
                // Достигли цели - показываем реальное время
                double remaining = _sequenceService.HoldRemainingSeconds;
                int totalSeconds = _sequenceService.HoldDurationSeconds;

                if (totalSeconds > 0)
                {
                    int minutes = (int)(remaining / 60);
                    int seconds = (int)(remaining % 60);
                    
                    if (lblHoldTimer != null)
                        lblHoldTimer.Text = $"Hold:         {minutes:D2}:{seconds:D2}";

                    // Обновляем progress bar
                    if (progressBarHold != null)
                    {
                        double progress = (totalSeconds - remaining) / totalSeconds;
                        int progressValue = (int)(progress * 100);
                        progressBarHold.Value = Math.Max(0, Math.Min(100, progressValue));
                    }
                }
                else
                {
                    if (lblHoldTimer != null)
                        lblHoldTimer.Text = "Hold:      Holding...";
                    if (progressBarHold != null)
                        progressBarHold.Value = 0;
                }
            }
        }

        /// <summary>
        /// Получает текущее давление из DataStore
        /// </summary>
        private double GetCurrentPressure()
        {
            // Используем _dataStore из GraphForm (partial class)
            if (_dataStore?.Points != null && _dataStore.Points.Count > 0)
            {
                return _dataStore.Points.Last().Current;
            }
            return 0.0;
        }

        // ====================================================================
        // СОХРАНЕНИЕ И ЗАГРУЗКА TARGETS
        // ====================================================================

        /// <summary>
        /// Получает путь к файлу настроек (используем тот же, что и для других настроек)
        /// </summary>
        private static string GetSettingsFilePath()
        {
            // ВСЕГДА используем папку проекта для хранения настроек
            // Находим папку проекта через путь к exe файлу (надежнее, чем текущая директория)
            string? projectDir = null;
            
            // Получаем путь к exe файлу
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string? exeDir = System.IO.Path.GetDirectoryName(exePath);
            
            if (!string.IsNullOrEmpty(exeDir))
            {
                // Поднимаемся вверх от exe до папки проекта (где есть .csproj файл)
                var dir = new System.IO.DirectoryInfo(exeDir);
                while (dir != null)
                {
                    // Проверяем, есть ли .csproj файл в этой директории
                    if (dir.GetFiles("*.csproj").Length > 0)
                    {
                        projectDir = dir.FullName;
                        break;
                    }
                    dir = dir.Parent;
                }
            }
            
            // Если не нашли через exe путь, пробуем текущую директорию
            if (string.IsNullOrEmpty(projectDir))
            {
                string? currentDir = System.IO.Directory.GetCurrentDirectory();
                if (!string.IsNullOrEmpty(currentDir))
                {
                    var dir = new System.IO.DirectoryInfo(currentDir);
                    while (dir != null)
                    {
                        if (dir.GetFiles("*.csproj").Length > 0)
                        {
                            projectDir = dir.FullName;
                            break;
                        }
                        dir = dir.Parent;
                    }
                }
            }
            
            // Если все еще не нашли, используем папку exe (fallback)
            if (string.IsNullOrEmpty(projectDir))
            {
                projectDir = exeDir ?? System.IO.Directory.GetCurrentDirectory();
            }
            
            string settingsDir = System.IO.Path.Combine(projectDir, "Settings");
            return System.IO.Path.Combine(settingsDir, "settings.json");
        }

        /// <summary>
        /// Загружает сохраненные targets из файла настроек
        /// Восстанавливает статусы, чтобы продолжить выполнение последовательности
        /// </summary>
        private void LoadTargetsFromFile()
        {
            _targets.Clear();
            
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!System.IO.File.Exists(settingsPath)) return;

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                if (settingsData.TryGetProperty("SequenceTargets", out var targetsElement))
                {
                    if (targetsElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var targetElement in targetsElement.EnumerateArray())
                        {
                            double psi = 0;
                            int holdMinutes = 0;
                            TargetStatus status = TargetStatus.Waiting;

                            if (targetElement.TryGetProperty("PSI", out var psiElement))
                                psi = psiElement.GetDouble();
                            
                            if (targetElement.TryGetProperty("HoldMinutes", out var holdElement))
                                holdMinutes = holdElement.GetInt32();

                            // Восстанавливаем статус (если есть)
                            if (targetElement.TryGetProperty("Status", out var statusElement))
                            {
                                string statusStr = statusElement.GetString() ?? "Waiting";
                                status = statusStr switch
                                {
                                    "Active" => TargetStatus.Active,
                                    "Completed" => TargetStatus.Completed,
                                    _ => TargetStatus.Waiting
                                };
                            }

                            if (psi > 0 && holdMinutes > 0)
                            {
                                _targets.Add(new TargetItem
                                {
                                    Number = _targets.Count + 1,
                                    PSI = psi,
                                    HoldMinutes = holdMinutes,
                                    Status = status // Восстанавливаем статус
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Не критично, просто не загружаем targets
                System.Diagnostics.Debug.WriteLine($"Failed to load targets from file: {ex.Message}");
            }
        }

        /// <summary>
        /// Сохраняет текущие targets в файл настроек
        /// Сохраняем PSI, HoldMinutes и Status, чтобы продолжить выполнение после перезапуска
        /// </summary>
        private void SaveTargetsToFile()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                string? directory = System.IO.Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                // Читаем существующий файл настроек (если есть) или создаем новый
                JsonObject? rootObject = null;
                if (System.IO.File.Exists(settingsPath))
                {
                    string existingJson = System.IO.File.ReadAllText(settingsPath);
                    var parsed = JsonNode.Parse(existingJson);
                    rootObject = parsed?.AsObject();
                }
                
                if (rootObject == null)
                {
                    rootObject = new JsonObject();
                }

                // Обновляем или добавляем SequenceTargets с сохранением статусов
                var targetsArray = new JsonArray();
                foreach (var target in _targets)
                {
                    string statusStr = target.Status switch
                    {
                        TargetStatus.Active => "Active",
                        TargetStatus.Completed => "Completed",
                        _ => "Waiting"
                    };
                    
                    targetsArray.Add(new JsonObject
                    {
                        ["PSI"] = target.PSI,
                        ["HoldMinutes"] = target.HoldMinutes,
                        ["Status"] = statusStr
                    });
                }
                
                rootObject["SequenceTargets"] = targetsArray;

                // Сериализуем и сохраняем
                string json = rootObject.ToJsonString(new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                System.IO.File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                // Не критично, просто не сохраняем
                System.Diagnostics.Debug.WriteLine($"Failed to save targets to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Восстанавливает состояние последовательности после загрузки targets
        /// Если есть Active target, продолжает выполнение
        /// </summary>
        private void RestoreSequenceState()
        {
            // Ищем активный target
            int activeIndex = -1;
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].Status == TargetStatus.Active)
                {
                    activeIndex = i;
                    break;
                }
            }

            // Если нашли активный target, продолжаем выполнение
            if (activeIndex >= 0)
            {
                _currentTargetIndex = activeIndex;
                _sequenceState = SequenceState.Playing;
                
                // Запускаем таймер, если он еще не запущен
                if (_holdTimer != null && !_holdTimer.Enabled)
                {
                    _holdTimer.Start();
                    _holdTimerStarted = false; // Таймер еще не начал отсчет (ждем достижения цели)
                }
            }
        }

        // ====================================================================
        // ОБНОВЛЕНИЕ UI
        // ====================================================================

        private void UpdateTargetsTable()
        {
            if (dgvTargets == null) return;

            // Сохраняем текущую позицию скролла только если мы не обновляем
            if (!_isUpdatingTable && dgvTargets.FirstDisplayedScrollingRowIndex >= 0)
            {
                _savedScrollPosition = dgvTargets.FirstDisplayedScrollingRowIndex;
            }

            _isUpdatingTable = true;

            try
            {
                // Временно отключаем обновление
                dgvTargets.SuspendLayout();
                dgvTargets.ClearSelection();
                // Не изменяем CurrentCell здесь, чтобы избежать реентрантных вызовов

                // Проверяем, нужно ли полное обновление
                bool needFullRefresh = dgvTargets.Rows.Count != _targets.Count;

                if (needFullRefresh)
                {
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
                            statusSymbol
                        );

                        // Все строки имеют одинаковый темный фон - подсветка фона удалена
                        // Цвет символов статуса устанавливается в DgvTargets_CellFormatting
                        foreach (DataGridViewCell cell in dgvTargets.Rows[i].Cells)
                        {
                            cell.Style.BackColor = Color.FromArgb(21, 23, 28);
                            
                            // Устанавливаем ForeColor только для ячеек, которые НЕ являются колонкой Status
                            // (цвета Status устанавливаются в CellFormatting)
                            if (dgvTargets.Columns[cell.ColumnIndex].Name != "colStatus")
                            {
                                cell.Style.ForeColor = Color.White;
                            }
                        }
                    }
                }
                else
                {
                    // Обновляем только измененные данные, сохраняя позицию скролла
                    for (int i = 0; i < Math.Min(_targets.Count, dgvTargets.Rows.Count); i++)
                    {
                        var target = _targets[i];
                        var statusSymbol = target.Status switch
                        {
                            TargetStatus.Active => "●",
                            TargetStatus.Completed => "✓",
                            _ => "○"
                        };

                        var holdText = target.HoldMinutes > 0 ? $"{target.HoldMinutes}m" : "0m";
                        
                        var row = dgvTargets.Rows[i];
                        row.Cells["colNumber"].Value = target.Number.ToString();
                        row.Cells["colPSI"].Value = target.PSI.ToString("F1");
                        row.Cells["colHold"].Value = holdText;
                        row.Cells["colStatus"].Value = statusSymbol;

                        // Обновляем стили ячеек
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.FromArgb(21, 23, 28);
                            if (dgvTargets.Columns[cell.ColumnIndex].Name != "colStatus")
                            {
                                cell.Style.ForeColor = Color.White;
                            }
                        }
                    }
                }

                dgvTargets.ResumeLayout();

                // Восстанавливаем позицию скролла после обновления
                if (_savedScrollPosition >= 0 && _savedScrollPosition < dgvTargets.Rows.Count && IsHandleCreated)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (dgvTargets != null && !dgvTargets.IsDisposed && _savedScrollPosition < dgvTargets.Rows.Count && IsHandleCreated)
                        {
                            try
                            {
                                dgvTargets.FirstDisplayedScrollingRowIndex = _savedScrollPosition;
                                dgvTargets.ClearSelection();
                                dgvTargets.CurrentCell = null;
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
                _isUpdatingTable = false;
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

        // Обработчик удаления удален - удаление доступно только в модальном окне Sequence Editor
    }
}

