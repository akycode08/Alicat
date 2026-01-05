using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Alicat.UI.Features.Graph.Views;

namespace Alicat.Services.Sequence
{
    /// <summary>
    /// Сервис для выполнения последовательности давлений
    /// Работает независимо от UI, продолжает выполнение даже когда окно закрыто
    /// </summary>
    public class SequenceService
    {
        private readonly Func<double> _getCurrentPressure;
        private readonly Action<double> _setTargetPressure;
        
        private List<TargetItem> _targets = new List<TargetItem>();
        private int _currentTargetIndex = -1;
        private SequenceState _sequenceState = SequenceState.Stopped;
        private System.Windows.Forms.Timer? _holdTimer;
        private DateTime _holdStartTime;
        private int _holdDurationSeconds = 0;
        private bool _holdTimerStarted = false;

        /// <summary>
        /// Событие: состояние последовательности изменилось
        /// </summary>
        public event Action? OnSequenceStateChanged;

        /// <summary>
        /// Событие: target изменился
        /// </summary>
        public event Action<int, TargetItem>? OnTargetChanged;

        /// <summary>
        /// Текущие targets (только чтение)
        /// </summary>
        public IReadOnlyList<TargetItem> Targets => _targets.AsReadOnly();

        /// <summary>
        /// Текущий индекс target
        /// </summary>
        public int CurrentTargetIndex => _currentTargetIndex;

        /// <summary>
        /// Состояние последовательности
        /// </summary>
        public SequenceState State => _sequenceState;

        /// <summary>
        /// Оставшееся время Hold в секундах (только если таймер активен)
        /// </summary>
        public double HoldRemainingSeconds
        {
            get
            {
                if (!_holdTimerStarted || _currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count)
                    return 0;

                var elapsed = (DateTime.Now - _holdStartTime).TotalSeconds;
                var remaining = _holdDurationSeconds - elapsed;
                return Math.Max(0, remaining);
            }
        }

        /// <summary>
        /// Достигнуто ли целевое давление (в пределах tolerance)
        /// </summary>
        public bool IsAtTarget
        {
            get
            {
                if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count)
                    return false;

                var target = _targets[_currentTargetIndex];
                double currentPressure = _getCurrentPressure();
                const double tolerance = 2.0;
                return Math.Abs(currentPressure - target.PSI) <= tolerance;
            }
        }

        /// <summary>
        /// Общая длительность Hold в секундах для текущего target
        /// </summary>
        public int HoldDurationSeconds => _holdDurationSeconds;

        public SequenceService(Func<double> getCurrentPressure, Action<double> setTargetPressure)
        {
            _getCurrentPressure = getCurrentPressure ?? throw new ArgumentNullException(nameof(getCurrentPressure));
            _setTargetPressure = setTargetPressure ?? throw new ArgumentNullException(nameof(setTargetPressure));

            // Инициализируем таймер для Hold
            _holdTimer = new System.Windows.Forms.Timer { Interval = 1000 }; // 1 секунда
            _holdTimer.Tick += HoldTimer_Tick;
        }

        /// <summary>
        /// Устанавливает targets
        /// </summary>
        public void SetTargets(List<TargetItem> targets)
        {
            _targets = new List<TargetItem>(targets.Select(t => new TargetItem
            {
                Number = t.Number,
                PSI = t.PSI,
                HoldMinutes = t.HoldMinutes,
                Status = t.Status
            }));

            SaveTargetsToFile(); // Сохраняем в файл
            OnSequenceStateChanged?.Invoke();
        }

        /// <summary>
        /// Очищает targets и удаляет их из файла настроек (при закрытии программы)
        /// </summary>
        public void ClearTargetsOnExit()
        {
            Stop(); // Останавливаем последовательность
            
            _targets.Clear();
            
            // Удаляем SequenceTargets из файла настроек
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (System.IO.File.Exists(settingsPath))
                {
                    string existingJson = System.IO.File.ReadAllText(settingsPath);
                    var parsed = JsonNode.Parse(existingJson);
                    if (parsed is JsonObject rootObject)
                    {
                        rootObject.Remove("SequenceTargets");
                        
                        string json = rootObject.ToJsonString(new System.Text.Json.JsonSerializerOptions
                        {
                            WriteIndented = true
                        });
                        
                        System.IO.File.WriteAllText(settingsPath, json);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to clear targets from file on exit: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает targets из файла настроек
        /// </summary>
        public void LoadTargetsFromFile()
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
                                    Status = status
                                });
                            }
                        }
                    }
                }

                // Восстанавливаем состояние последовательности
                RestoreSequenceState();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load targets from file: {ex.Message}");
            }
        }

        /// <summary>
        /// Сохраняет targets в файл настроек
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
                System.Diagnostics.Debug.WriteLine($"Failed to save targets to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает путь к файлу настроек
        /// </summary>
        private static string GetSettingsFilePath()
        {
            string? projectDir = null;
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
            
            if (string.IsNullOrEmpty(projectDir))
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string? exeDir = System.IO.Path.GetDirectoryName(exePath);
                
                if (exeDir != null && exeDir.Contains("bin"))
                {
                    var dir = new System.IO.DirectoryInfo(exeDir);
                    while (dir != null && dir.Name != "Alicat" && dir.GetFiles("*.csproj").Length == 0)
                    {
                        dir = dir.Parent;
                    }
                    projectDir = dir?.FullName ?? System.IO.Directory.GetCurrentDirectory();
                }
                else
                {
                    projectDir = System.IO.Directory.GetCurrentDirectory();
                }
            }
            
            string settingsDir = System.IO.Path.Combine(projectDir, "Settings");
            return System.IO.Path.Combine(settingsDir, "settings.json");
        }

        /// <summary>
        /// Восстанавливает состояние последовательности после загрузки targets
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
                    _holdTimerStarted = false;
                }
            }
        }

        /// <summary>
        /// Запускает последовательность
        /// </summary>
        public void Start()
        {
            if (_targets.Count == 0) return;

            if (_sequenceState == SequenceState.Stopped)
            {
                // Начинаем с первой цели или с первой невыполненной
                _currentTargetIndex = FindFirstIncompleteTarget();
                if (_currentTargetIndex < 0) return;
                
                StartCurrentTarget();
            }
            else if (_sequenceState == SequenceState.Paused)
            {
                ResumeCurrentTarget();
            }

            _sequenceState = SequenceState.Playing;
            OnSequenceStateChanged?.Invoke();
        }

        /// <summary>
        /// Пауза последовательности
        /// </summary>
        public void Pause()
        {
            if (_sequenceState == SequenceState.Playing)
            {
                _sequenceState = SequenceState.Paused;
                if (_holdTimer != null)
                    _holdTimer.Stop();
                OnSequenceStateChanged?.Invoke();
            }
        }

        /// <summary>
        /// Останавливает последовательность
        /// </summary>
        public void Stop()
        {
            _sequenceState = SequenceState.Stopped;
            
            if (_holdTimer != null)
            {
                _holdTimer.Stop();
            }

            // Сбрасываем статусы всех активных целей
            foreach (var target in _targets)
            {
                if (target.Status == TargetStatus.Active)
                    target.Status = TargetStatus.Waiting;
            }

            _currentTargetIndex = -1;
            _holdTimerStarted = false;
            SaveTargetsToFile(); // Сохраняем изменения
            OnSequenceStateChanged?.Invoke();
        }

        /// <summary>
        /// Пропускает текущий target
        /// </summary>
        public void Skip()
        {
            if (_sequenceState == SequenceState.Playing || _sequenceState == SequenceState.Paused)
            {
                MoveToNextTarget();
            }
        }

        /// <summary>
        /// Находит первый невыполненный target
        /// </summary>
        private int FindFirstIncompleteTarget()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].Status != TargetStatus.Completed)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Запускает текущий target
        /// </summary>
        private void StartCurrentTarget()
        {
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count) return;

            var target = _targets[_currentTargetIndex];
            target.Status = TargetStatus.Active;
            SaveTargetsToFile(); // Сохраняем статус

            // Устанавливаем целевое давление
            _setTargetPressure(target.PSI);

            _holdDurationSeconds = target.HoldMinutes * 60;
            _holdTimerStarted = false;

            // Запускаем таймер для проверки давления и отсчета Hold
            if (_holdTimer != null && !_holdTimer.Enabled)
            {
                _holdTimer.Start();
            }

            OnTargetChanged?.Invoke(_currentTargetIndex, target);
            OnSequenceStateChanged?.Invoke();
        }

        /// <summary>
        /// Возобновляет текущий target
        /// </summary>
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

        /// <summary>
        /// Переходит к следующему target
        /// </summary>
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
                Stop();
                OnSequenceStateChanged?.Invoke();
            }
            else
            {
                // Запускаем следующую цель
                StartCurrentTarget();
            }
        }

        /// <summary>
        /// Обработчик таймера Hold
        /// </summary>
        private void HoldTimer_Tick(object? sender, EventArgs e)
        {
            if (_currentTargetIndex < 0 || _currentTargetIndex >= _targets.Count)
            {
                if (_holdTimer != null)
                    _holdTimer.Stop();
                return;
            }

            var target = _targets[_currentTargetIndex];
            
            // Получаем текущее давление
            double currentPressure = _getCurrentPressure();
            
            // Проверяем, достигли ли мы целевого давления (tolerance ±2 PSI)
            const double tolerance = 2.0;
            bool atTarget = Math.Abs(currentPressure - target.PSI) <= tolerance;

            // Если ещё не достигли цели, не запускаем Hold timer
            if (!atTarget)
            {
                _holdTimerStarted = false;
                return;
            }

            // Достигли цели - запускаем Hold timer (если ещё не запущен)
            if (!_holdTimerStarted)
            {
                _holdTimerStarted = true;
                _holdStartTime = DateTime.Now;
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
        }

        /// <summary>
        /// Освобождает ресурсы
        /// </summary>
        public void Dispose()
        {
            if (_holdTimer != null)
            {
                _holdTimer.Stop();
                _holdTimer.Dispose();
                _holdTimer = null;
            }
        }
    }
}

