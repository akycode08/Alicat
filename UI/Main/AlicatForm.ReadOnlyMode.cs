using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Alicat.Domain;
using Alicat.Services.Data;

namespace Alicat
{
    /// <summary>
    /// Логика read-only режима для завершенных сессий
    /// </summary>
    public partial class AlicatForm
    {
        // ====================================================================
        // READ-ONLY MODE: Session Save/Load
        // ====================================================================

        /// <summary>
        /// Сохраняет текущую сессию в файл
        /// </summary>
        private void SaveSessionToFile(string filePath, bool markAsCompleted = false)
        {
            try
            {
                var sessionData = CreateSessionDataFromCurrent();
                
                if (markAsCompleted)
                {
                    sessionData.State = SessionState.Completed;
                    sessionData.Status = "Completed";
                }
                
                sessionData.LastModified = DateTime.Now;
                
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                
                string json = JsonSerializer.Serialize(sessionData, options);
                File.WriteAllText(filePath, json);
                
                _currentSession = sessionData;
                _currentSessionFilePath = filePath;
                
                // Обновляем состояние read-only режима
                UpdateReadOnlyMode(sessionData.State == SessionState.Completed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error saving session:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает сессию из файла (поддерживает CSV и JSON форматы)
        /// </summary>
        private bool LoadSessionFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show(this,
                        "Session file not found.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }

                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                SessionData? sessionData = null;

                // Проверяем формат файла
                if (extension == ".als")
                {
                    // JSON формат (новый)
                    sessionData = LoadSessionFromJson(filePath);
                }
                else if (extension == ".csv")
                {
                    // CSV формат (старый) - загружаем данные и создаем SessionData
                    sessionData = LoadSessionFromCsv(filePath);
                }
                else
                {
                    // Пытаемся определить формат по содержимому
                    string firstLine = File.ReadLines(filePath).FirstOrDefault() ?? "";
                    if (firstLine.StartsWith("RowNumber,Timestamp") || firstLine.StartsWith("Timestamp,Time_s"))
                    {
                        // CSV формат (новый или старый)
                        sessionData = LoadSessionFromCsv(filePath);
                    }
                    else if (firstLine.TrimStart().StartsWith("{") || firstLine.TrimStart().StartsWith("["))
                    {
                        // JSON формат
                        sessionData = LoadSessionFromJson(filePath);
                    }
                    else
                    {
                        MessageBox.Show(this,
                            "Unknown session file format. Expected .als (JSON) or .csv file.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (sessionData == null)
                {
                    MessageBox.Show(this,
                        "Failed to load session data.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }

                _currentSession = sessionData;
                _currentSessionFilePath = filePath;

                // Загружаем данные в DataStore
                LoadSessionDataIntoStore(sessionData);

                // Обновляем GraphForm, если он открыт
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    _graphForm.ReloadDataFromStore();
                }

                // Обновляем TableForm, если он открыт (он обновляется автоматически через события)
                // TableForm получает данные через события DataStore.OnNewPoint

                // Проверяем состояние и включаем read-only режим если нужно
                bool isReadOnly = sessionData.State == SessionState.Completed;
                
                // Отключаем устройство ПЕРЕД включением read-only режима
                if (isReadOnly)
                {
                    // Используем Presenter для отключения, если доступен
                    if (_presenter != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Using Presenter to disconnect device...");
                        _presenter.DisconnectDevice();
                    }
                    else
                    {
                        // Иначе отключаем напрямую
                        System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Using direct disconnect...");
                        DisconnectDeviceForReadOnly();
                    }
                }
                
                UpdateReadOnlyMode(isReadOnly);

                // Показываем предупреждение для read-only сессий
                if (isReadOnly)
                {
                    MessageBox.Show(this,
                        "⚠ READ-ONLY MODE\n\nThis is a completed session. Data cannot be modified.\nAll control buttons are disabled.",
                        "Read-Only Session",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                // Обновляем заголовок окна
                UpdateWindowTitle();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error loading session:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Загружает сессию из JSON файла (.als)
        /// </summary>
        private SessionData? LoadSessionFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var sessionData = JsonSerializer.Deserialize<SessionData>(json);
            return sessionData;
        }

        /// <summary>
        /// Загружает сессию из CSV файла (поддерживает старый и новый формат)
        /// </summary>
        private SessionData LoadSessionFromCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                throw new InvalidDataException("CSV file is empty or has no data rows.");
            }

            // Определяем формат файла по заголовку
            bool isNewFormat = lines[0].StartsWith("RowNumber,Timestamp", StringComparison.OrdinalIgnoreCase);
            
            // Индексы колонок в зависимости от формата
            int timestampIndex = isNewFormat ? 1 : 0;  // RowNumber на 0 в новом формате
            int timeIndex = isNewFormat ? 2 : 1;
            int currentIndex = isNewFormat ? 3 : 2;
            int targetIndex = isNewFormat ? 4 : 3;
            int unitIndex = isNewFormat ? 5 : 4;
            int rampSpeedIndex = isNewFormat ? 6 : 5;
            int pollingFreqIndex = isNewFormat ? 7 : 6;
            int eventIndex = isNewFormat ? 9 : 7;  // PointIndex на 8 в новом формате, Event на 9
            
            // Парсим данные
            var dataPoints = new List<DataPoint>();
            DateTime? sessionStart = null;
            DateTime? firstTimestamp = null;
            DateTime? lastTimestamp = null;

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(',');
                if (parts.Length < currentIndex + 1) continue;

                try
                {
                    // Парсим timestamp
                    string timestampStr = parts[timestampIndex];
                    if (DateTime.TryParse(timestampStr, out DateTime timestamp))
                    {
                        if (firstTimestamp == null) firstTimestamp = timestamp;
                        lastTimestamp = timestamp;
                        
                        if (sessionStart == null)
                        {
                            sessionStart = timestamp;
                        }

                        // Парсим остальные поля с учетом формата
                        double elapsed = parts.Length > timeIndex && double.TryParse(parts[timeIndex], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double e) ? e : 0;
                        double current = parts.Length > currentIndex && double.TryParse(parts[currentIndex], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double c) ? c : 0;
                        double target = parts.Length > targetIndex && double.TryParse(parts[targetIndex], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double t) ? t : 0;
                        string unit = parts.Length > unitIndex ? parts[unitIndex] : "PSIG";
                        double rampSpeed = parts.Length > rampSpeedIndex && double.TryParse(parts[rampSpeedIndex], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double rs) ? rs : 0;
                        int pollingFreq = parts.Length > pollingFreqIndex && int.TryParse(parts[pollingFreqIndex], out int pf) ? pf : 500;
                        string? eventType = parts.Length > eventIndex && !string.IsNullOrWhiteSpace(parts[eventIndex]) ? parts[eventIndex] : null;

                        var point = new DataPoint(timestamp, elapsed, current, target, unit, rampSpeed, pollingFreq, eventType);
                        dataPoints.Add(point);
                    }
                }
                catch
                {
                    // Пропускаем некорректные строки
                    continue;
                }
            }

            // Загружаем данные в DataStore
            if (dataPoints.Count > 0 && sessionStart.HasValue)
            {
                // Используем метод загрузки исторических данных
                DataStore.LoadHistoricalDataFromCsv(filePath, sessionStart.Value);
            }

            // Создаем SessionData из CSV данных
            var sessionData = new SessionData
            {
                SessionName = Path.GetFileNameWithoutExtension(filePath),
                CreatedDate = sessionStart ?? File.GetCreationTime(filePath),
                LastModified = lastTimestamp ?? File.GetLastWriteTime(filePath),
                Duration = (lastTimestamp ?? DateTime.Now) - (sessionStart ?? DateTime.Now),
                Status = "Completed",
                State = SessionState.Completed, // CSV файлы всегда завершенные сессии
                Operator = Environment.UserName,
                TotalDataPoints = dataPoints.Count
            };

            // Вычисляем статистику
            if (dataPoints.Count > 0)
            {
                sessionData.InitialPressure = (decimal)dataPoints.First().Current;
                sessionData.FinalPressure = (decimal)dataPoints.Last().Current;
                sessionData.MaxPressureReached = (decimal)dataPoints.Max(p => p.Current);
                sessionData.AverageRate = dataPoints.Count > 1
                    ? (decimal)dataPoints.Where(p => p.RampSpeed > 0).DefaultIfEmpty(dataPoints.First()).Average(p => p.RampSpeed)
                    : 0;
            }

            return sessionData;
        }

        /// <summary>
        /// Создает SessionData из текущей сессии
        /// </summary>
        private SessionData CreateSessionDataFromCurrent()
        {
            var dataStore = DataStore;
            var sessionStartTime = dataStore.IsRunning ? dataStore.SessionStart : DateTime.Now;
            
            var sessionData = new SessionData
            {
                SessionName = Path.GetFileNameWithoutExtension(_currentSessionFilePath ?? $"Session_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"),
                CreatedDate = sessionStartTime,
                LastModified = DateTime.Now,
                Duration = dataStore.IsRunning ? DateTime.Now - sessionStartTime : TimeSpan.Zero,
                Status = dataStore.IsRunning ? "Active" : "Completed",
                State = dataStore.IsRunning ? SessionState.Active : SessionState.Completed,
                Operator = Environment.UserName,

                DeviceModel = "ALICAT PC-15PSIG-D",
                SerialNumber = "12345-ABC-67890",
                FirmwareVersion = "v2.5.3",
                CalibrationDate = DateTime.Now.AddMonths(-3),

                ComPort = _serial?.PortName ?? "COM3",
                BaudRate = _serial?.BaudRate ?? 19200,
                DataBits = 8,
                Parity = "None",
                StopBits = "1",

                PressureUnit = _unit ?? "PSIG",
                TimeUnit = "s",
                RampSpeed = (decimal)_rampSpeed,
                SampleRate = 10,
                MaxPressure = (decimal)_maxPressure,
                MinPressure = (decimal)_minPressure,

                TotalDataPoints = dataStore.Points.Count
            };

            // Вычисляем статистику
            if (dataStore.Points.Count > 0)
            {
                var points = dataStore.Points;
                sessionData.InitialPressure = (decimal)points.First().Current;
                sessionData.FinalPressure = (decimal)points.Last().Current;
                sessionData.MaxPressureReached = (decimal)points.Max(p => p.Current);
                sessionData.AverageRate = points.Count > 1
                    ? (decimal)points.Where(p => p.RampSpeed > 0).DefaultIfEmpty(points.First()).Average(p => p.RampSpeed)
                    : 0;
            }

            return sessionData;
        }

        /// <summary>
        /// Загружает данные сессии в DataStore
        /// </summary>
        private void LoadSessionDataIntoStore(SessionData sessionData)
        {
            // Обновляем настройки из сессии
            _maxPressure = (double)sessionData.MaxPressure;
            _minPressure = (double)sessionData.MinPressure;
            _unit = sessionData.PressureUnit;
            
            // Данные уже загружены через LoadHistoricalDataFromCsv для CSV файлов
            // Для JSON файлов нужно будет добавить загрузку точек данных
            // Пока что данные загружаются только из CSV
        }

        /// <summary>
        /// Обновляет read-only режим и UI
        /// </summary>
        private void UpdateReadOnlyMode(bool isReadOnly)
        {
            _isReadOnlyMode = isReadOnly;
            
            // Если включается read-only режим и устройство подключено - отключаем его
            if (isReadOnly)
            {
                // Проверяем подключение - если _serial не null, значит устройство было подключено
                bool isDeviceConnected = _serial != null;
                
                // Дополнительная проверка через IsConnected, если доступно
                if (isDeviceConnected && _serial != null)
                {
                    try
                    {
                        isDeviceConnected = _serial.IsConnected;
                    }
                    catch
                    {
                        // Если проверка не удалась, считаем что подключено (если _serial не null)
                        isDeviceConnected = true;
                    }
                }
                
                if (isDeviceConnected)
                {
                    System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Device is connected, disconnecting...");
                    DisconnectDeviceForReadOnly();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Device is not connected, skipping disconnect.");
                }
            }
            
            UpdateUIForReadOnlyMode(isReadOnly);
        }

        /// <summary>
        /// Отключает устройство при включении read-only режима
        /// </summary>
        private void DisconnectDeviceForReadOnly()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Starting device disconnect...");
                
                // Останавливаем таймер опроса
                if (_pollTimer != null)
                {
                    _pollTimer.Stop();
                    _pollTimer.Dispose();
                    _pollTimer = null;
                    System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Poll timer stopped and disposed.");
                }
                
                _isWaitingForResponse = false;

                // Закрываем соединение
                if (_serial != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Disposing serial connection...");
                    _serial.Dispose();
                    _serial = null;
                    System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Serial connection disposed.");
                }
                
                _ramp = null;

                // Обновляем статус подключения
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => UI_UpdateConnectionStatus(false)));
                }
                else
                {
                    UI_UpdateConnectionStatus(false);
                }

                // Обновляем информацию о подключении в дочерних формах
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    if (_graphForm.InvokeRequired)
                    {
                        _graphForm.BeginInvoke(new Action(() => _graphForm.SetConnectionInfo(null, null)));
                    }
                    else
                    {
                        _graphForm.SetConnectionInfo(null, null);
                    }
                }

                if (_tableForm != null && !_tableForm.IsDisposed)
                {
                    if (_tableForm.InvokeRequired)
                    {
                        _tableForm.BeginInvoke(new Action(() => _tableForm.SetConnectionInfo(null, null)));
                    }
                    else
                    {
                        _tableForm.SetConnectionInfo(null, null);
                    }
                }

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => UI_AppendStatusInfo("Device disconnected - read-only mode enabled")));
                }
                else
                {
                    UI_AppendStatusInfo("Device disconnected - read-only mode enabled");
                }
                
                System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Device disconnect completed successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Error disconnecting device: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[Read-Only Mode] Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Обновляет UI для read-only режима
        /// </summary>
        private void UpdateUIForReadOnlyMode(bool isReadOnly)
        {
            // Отключаем/включаем кнопки управления
            if (btnIncrease != null)
                btnIncrease.Enabled = !isReadOnly;
            
            if (btnDecrease != null)
                btnDecrease.Enabled = !isReadOnly;
            
            if (btnGoToTarget != null)
                btnGoToTarget.Enabled = !isReadOnly;
            
            if (txtTargetInput != null)
                txtTargetInput.Enabled = !isReadOnly;

            // Обновляем меню Save
            if (menuFileSaveSession != null)
                menuFileSaveSession.Enabled = !isReadOnly;

            // Обновляем заголовок окна
            UpdateWindowTitle();

            // Показываем/скрываем баннер read-only
            UpdateReadOnlyBanner(isReadOnly);
        }

        /// <summary>
        /// Обработчик события завершения сессии - автоматически сохраняет как Completed
        /// </summary>
        private void DataStore_OnSessionEnded()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(DataStore_OnSessionEnded));
                return;
            }

            // Если есть активная сессия с файлом, сохраняем её как Completed
            if (!string.IsNullOrEmpty(_currentSessionFilePath) && !_isReadOnlyMode)
            {
                try
                {
                    // Сохраняем сессию как Completed
                    SaveSessionToFile(_currentSessionFilePath, markAsCompleted: true);
                    
                    // Включаем read-only режим
                    UpdateReadOnlyMode(true);
                    
                    UI_AppendStatusInfo("Session completed and saved as read-only");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,
                        $"Error saving completed session:\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Обновляет заголовок окна
        /// </summary>
        private void UpdateWindowTitle()
        {
            string baseTitle = "Alicat Controller";
            string title = baseTitle;

            if (_currentSession != null && !string.IsNullOrEmpty(_currentSession.SessionName))
            {
                title = $"{baseTitle} - {_currentSession.SessionName}";
            }

            if (_isReadOnlyMode)
            {
                title += " [Read-Only]";
            }

            this.Text = title;
        }

        /// <summary>
        /// Обновляет баннер read-only режима
        /// </summary>
        private void UpdateReadOnlyBanner(bool show)
        {
            // Ищем существующую панель или создаем новую
            Panel? bannerPanel = null;
            foreach (Control control in this.Controls)
            {
                if (control.Name == "panelReadOnlyBanner")
                {
                    bannerPanel = control as Panel;
                    break;
                }
            }

            if (show)
            {
                if (bannerPanel == null)
                {
                    // Создаем новую панель для баннера
                    bannerPanel = new Panel
                    {
                        Name = "panelReadOnlyBanner",
                        Dock = DockStyle.Top,
                        Height = 40,
                        BackColor = System.Drawing.Color.FromArgb(255, 193, 7), // Оранжевый
                        Padding = new Padding(10, 8, 10, 8)
                    };

                    var label = new Label
                    {
                        Text = "⚠ READ-ONLY MODE - Completed session, data cannot be modified",
                        Dock = DockStyle.Fill,
                        ForeColor = System.Drawing.Color.Black,
                        Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                    };

                    bannerPanel.Controls.Add(label);
                    this.Controls.Add(bannerPanel);
                    bannerPanel.BringToFront();
                }
                else
                {
                    bannerPanel.Visible = true;
                }
            }
            else
            {
                if (bannerPanel != null)
                {
                    bannerPanel.Visible = false;
                }
            }
        }
    }
}

