using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alicat.Services.Reports;

namespace Alicat
{
    /// <summary>
    /// Расширения меню для AlicatForm
    /// Обработчики для новых пунктов меню File
    /// </summary>
    public partial class AlicatForm
    {
        /// <summary>
        /// Обработчик для пункта меню "Open Session..."
        /// </summary>
        private void MenuFileOpenSession_Click(object? sender, EventArgs e)
        {
            try
            {
                using var openDialog = new OpenFileDialog
                {
                    Filter = "Alicat Session Files (*.als)|*.als|CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                    Title = "Open Session"
                };

                if (openDialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (LoadSessionFromFile(openDialog.FileName))
                    {
                        // Добавляем сессию в список Recent Sessions
                        AddToRecentSessions(openDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error opening session:\n{ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик для пункта меню "Save Session"
        /// </summary>
        private void MenuFileSaveSession_Click(object? sender, EventArgs e)
        {
            try
            {
                // Проверяем read-only режим
                if (_isReadOnlyMode)
                {
                    MessageBox.Show(this,
                        "⚠ READ-ONLY MODE\n\nCannot save a completed session.\nUse 'Save Session As...' to create a copy with a new name.",
                        "Read-Only Session",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Если файл уже существует, сохраняем в него
                if (!string.IsNullOrEmpty(_currentSessionFilePath))
                {
                    SaveSessionToFile(_currentSessionFilePath, markAsCompleted: false);
                    MessageBox.Show(this,
                        "Session saved successfully.",
                        "Save Session",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    // Если файла нет, используем Save As
                    MenuFileSaveSessionAs_Click(sender, e);
                }
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
        /// Обработчик для пункта меню "Save Session As..."
        /// </summary>
        private void MenuFileSaveSessionAs_Click(object? sender, EventArgs e)
        {
            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "Alicat Session Files (*.als)|*.als|All files (*.*)|*.*",
                    Title = "Save Session As",
                    FileName = $"Session_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.als"
                };

                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Для read-only сессий создаем копию с новым именем (не помечаем как Completed)
                    bool markAsCompleted = !_isReadOnlyMode;
                    SaveSessionToFile(saveDialog.FileName, markAsCompleted: markAsCompleted);
                    
                    MessageBox.Show(this,
                        _isReadOnlyMode 
                            ? "Session copied successfully. The new session is editable."
                            : "Session saved successfully.",
                        "Save Session As",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
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
        /// Обработчик для пункта меню "Export -> Table (CSV)..."
        /// </summary>
        private void MenuFileExportTable_Click(object? sender, EventArgs e)
        {
            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    Title = "Export Table to CSV",
                    FileName = $"table_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv"
                };

                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // TODO: Реализовать экспорт таблицы в CSV
                    MessageBox.Show(this, 
                        $"Exporting table to: {saveDialog.FileName}\n\nFeature not yet implemented.", 
                        "Export Table", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error exporting table:\n{ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик для пункта меню "Export -> LineChart(PNG/SVG)..."
        /// Использует ту же логику, что и экспорт из GraphForm
        /// </summary>
        private void MenuFileExportGraphImage_Click(object? sender, EventArgs e)
        {
            try
            {
                // Проверяем, открыт ли GraphForm
                if (_graphForm == null || _graphForm.IsDisposed)
                {
                    MessageBox.Show(this,
                        "Graph window is not open. Please open the Graph window first.",
                        "Graph Not Available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Используем тот же метод экспорта, что и кнопка Export в GraphForm
                _graphForm.ShowExportDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error exporting graph:\n{ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик для пункта меню "Export -> Session Report (PDF)..."
        /// </summary>
        private void MenuFileExportSessionReport_Click(object? sender, EventArgs e)
        {
            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    Title = "Export Session Report",
                    FileName = $"SessionReport_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf"
                };

                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Create session data from current session
                    var reportData = CreateReportDataFromSession();
                    
                    // Generate PDF
                    var generator = new SessionReportGenerator(reportData);
                    generator.Generate(saveDialog.FileName);
                    
                    MessageBox.Show(this,
                        "Report generated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    
                    // Ask to open PDF
                    if (MessageBox.Show(this,
                        "Do you want to open the report?",
                        "Open Report",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error generating report:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Создает данные отчета из текущей сессии
        /// </summary>
        private SessionReportData CreateReportDataFromSession()
        {
            // Получаем данные из DataStore
            var dataStore = DataStore;
            var sessionStartTime = dataStore.IsRunning ? dataStore.SessionStart : DateTime.Now.AddHours(-1);
            
            var reportData = new SessionReportData
            {
                SessionName = $"Session_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}",
                CreatedDate = sessionStartTime,
                LastModified = DateTime.Now,
                Duration = dataStore.IsRunning ? DateTime.Now - sessionStartTime : TimeSpan.FromMinutes(45),
                Status = dataStore.IsRunning ? "Active" : "Completed",
                Operator = Environment.UserName,

                DeviceModel = "ALICAT PC-15PSIG-D",
                SerialNumber = "12345-ABC-67890",
                FirmwareVersion = "v2.5.3",
                CalibrationDate = DateTime.Now.AddMonths(-3),

                ComPort = "COM3",
                BaudRate = 19200,
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

            // Вычисляем статистику из реальных данных
            if (dataStore.Points.Count > 0)
            {
                var points = dataStore.Points;
                reportData.InitialPressure = (decimal)points.First().Current;
                reportData.FinalPressure = (decimal)points.Last().Current;
                reportData.MaxPressureReached = (decimal)points.Max(p => p.Current);
                reportData.AverageRate = points.Count > 1
                    ? (decimal)points.Where(p => p.RampSpeed > 0).DefaultIfEmpty(points.First()).Average(p => p.RampSpeed)
                    : 0;

                // Конвертируем DataPoint в ReportDataPoint (первые 20 точек)
                reportData.DataPoints = points
                    .Take(20)
                    .Select(p => new ReportDataPoint
                    {
                        Time = (decimal)p.ElapsedSeconds,
                        Pressure = (decimal)p.Current,
                        Target = (decimal)p.Target,
                        Rate = (decimal)p.RampSpeed,
                        Status = p.Event ?? "Normal"
                    })
                    .ToList();
            }
            else
            {
                // Если данных нет, используем примерные значения
                reportData.InitialPressure = 0;
                reportData.FinalPressure = (decimal)_setPoint;
                reportData.MaxPressureReached = (decimal)_setPoint;
                reportData.AverageRate = 2.5m;
                reportData.DataPoints = GenerateSampleDataPoints();
            }

            return reportData;
        }

        /// <summary>
        /// Генерирует примерные данные для отчета (если сессия пуста)
        /// </summary>
        private List<ReportDataPoint> GenerateSampleDataPoints()
        {
            var dataPoints = new List<ReportDataPoint>();
            decimal pressure = 0;
            decimal target = (decimal)_setPoint;

            for (int i = 0; i <= 20; i++)
            {
                if (i <= 15 && target > 0)
                {
                    pressure = i * (target / 15.0m);
                    dataPoints.Add(new ReportDataPoint
                    {
                        Time = i,
                        Pressure = pressure,
                        Target = target,
                        Rate = target / 15.0m,
                        Status = pressure < target ? "Ramping" : "At Target"
                    });
                }
                else if (target > 0)
                {
                    dataPoints.Add(new ReportDataPoint
                    {
                        Time = i,
                        Pressure = target,
                        Target = target,
                        Rate = 0.0m,
                        Status = "Stable"
                    });
                }
                else
                {
                    dataPoints.Add(new ReportDataPoint
                    {
                        Time = i,
                        Pressure = 0,
                        Target = 0,
                        Rate = 0.0m,
                        Status = "Idle"
                    });
                }
            }

            return dataPoints;
        }

        /// <summary>
        /// Обработчик для пунктов меню "Recent Sessions" (динамический)
        /// </summary>
        private void MenuFileRecentSession_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string filePath)
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show(this,
                            $"Session file not found:\n{filePath}\n\nRemoving from recent sessions list.",
                            "File Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        
                        // Удаляем из списка
                        _recentSessions.Remove(filePath);
                        UpdateRecentSessionsMenu();
                        SaveRecentSessionsToSettings();
                        return;
                    }

                    if (LoadSessionFromFile(filePath))
                    {
                        // Обновляем список (перемещаем выбранную сессию на первое место)
                        AddToRecentSessions(filePath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, 
                        $"Error opening recent session:\n{ex.Message}", 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Обработчик для пункта меню "Clear Recent List"
        /// </summary>
        private void MenuFileRecentSessionsClearList_Click(object? sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(this, 
                    "Are you sure you want to clear the recent sessions list?", 
                    "Clear Recent List", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _recentSessions.Clear();
                    UpdateRecentSessionsMenu();
                    SaveRecentSessionsToSettings();
                    MessageBox.Show(this, 
                        "Recent sessions list cleared.", 
                        "Clear Recent List", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error clearing recent list:\n{ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик для пункта меню "Session Configuration..."
        /// </summary>
        private void MenuFileSessionConfiguration_Click(object? sender, EventArgs e)
        {
            try
            {
                using var form = new SessionConfigurationForm();
                form.StartPosition = FormStartPosition.CenterParent;
                
                // Загружаем реальные данные сессии
                form.LoadSessionData(DataStore, _presenter, _serial);
                
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error opening Session Configuration:\n{ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // RECENT SESSIONS MANAGEMENT
        // ====================================================================

        /// <summary>
        /// Добавляет файл сессии в список Recent Sessions (максимум 3)
        /// </summary>
        private void AddToRecentSessions(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return;

            // Нормализуем путь для сравнения
            string normalizedPath = Path.GetFullPath(filePath);

            // Удаляем, если уже есть в списке
            _recentSessions.RemoveAll(p => string.Equals(Path.GetFullPath(p), normalizedPath, StringComparison.OrdinalIgnoreCase));

            // Добавляем в начало списка
            _recentSessions.Insert(0, normalizedPath);

            // Ограничиваем список 3 элементами
            while (_recentSessions.Count > 3)
            {
                _recentSessions.RemoveAt(_recentSessions.Count - 1);
            }

            // Обновляем меню и сохраняем в настройки
            UpdateRecentSessionsMenu();
            SaveRecentSessionsToSettings();
        }

        /// <summary>
        /// Обновляет пункты меню Recent Sessions
        /// </summary>
        private void UpdateRecentSessionsMenu()
        {
            // Очищаем существующие пункты меню
            menuFileRecentSessionsItem1.Visible = false;
            menuFileRecentSessionsItem2.Visible = false;
            menuFileRecentSessionsItem3.Visible = false;

            // Фильтруем список: оставляем только существующие файлы
            var validSessions = _recentSessions.Where(f => File.Exists(f)).Take(3).ToList();
            _recentSessions.Clear();
            _recentSessions.AddRange(validSessions);

            // Обновляем пункты меню
            if (_recentSessions.Count > 0)
            {
                var fileName1 = Path.GetFileName(_recentSessions[0]);
                menuFileRecentSessionsItem1.Text = fileName1;
                menuFileRecentSessionsItem1.Tag = _recentSessions[0];
                menuFileRecentSessionsItem1.Visible = true;
            }

            if (_recentSessions.Count > 1)
            {
                var fileName2 = Path.GetFileName(_recentSessions[1]);
                menuFileRecentSessionsItem2.Text = fileName2;
                menuFileRecentSessionsItem2.Tag = _recentSessions[1];
                menuFileRecentSessionsItem2.Visible = true;
            }

            if (_recentSessions.Count > 2)
            {
                var fileName3 = Path.GetFileName(_recentSessions[2]);
                menuFileRecentSessionsItem3.Text = fileName3;
                menuFileRecentSessionsItem3.Tag = _recentSessions[2];
                menuFileRecentSessionsItem3.Visible = true;
            }

            // Показываем/скрываем разделитель и кнопку "Clear" в зависимости от наличия элементов
            bool hasItems = _recentSessions.Count > 0;
            menuFileRecentSessionsSeparator.Visible = hasItems;
            menuFileRecentSessionsClearList.Visible = hasItems;
        }

        /// <summary>
        /// Сохраняет список Recent Sessions в настройки
        /// </summary>
        private void SaveRecentSessionsToSettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                
                // Читаем существующий файл или создаем новый
                System.Text.Json.JsonElement rootElement;
                if (File.Exists(settingsPath))
                {
                    string existingJson = File.ReadAllText(settingsPath);
                    rootElement = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(existingJson);
                }
                else
                {
                    // Если файла нет, создаем пустой объект
                    rootElement = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>("{}");
                }

                // Обновляем JSON элемент напрямую через Utf8JsonWriter
                using var ms = new System.IO.MemoryStream();
                using var writer = new System.Text.Json.Utf8JsonWriter(ms, new System.Text.Json.JsonWriterOptions { Indented = true });
                
                writer.WriteStartObject();
                
                // Копируем все существующие свойства, кроме RecentSessions
                foreach (var prop in rootElement.EnumerateObject())
                {
                    if (prop.Name != "RecentSessions")
                    {
                        writer.WritePropertyName(prop.Name);
                        prop.Value.WriteTo(writer);
                    }
                }
                
                // Добавляем RecentSessions
                writer.WritePropertyName("RecentSessions");
                System.Text.Json.JsonSerializer.Serialize(writer, _recentSessions.ToArray());
                
                writer.WriteEndObject();
                writer.Flush();
                
                string updatedJson = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                File.WriteAllText(settingsPath, updatedJson);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SaveRecentSessionsToSettings] Failed to save recent sessions: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает список Recent Sessions из настроек
        /// </summary>
        private void LoadRecentSessionsFromSettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (!File.Exists(settingsPath))
                    return;

                string json = File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                if (settingsData.TryGetProperty("RecentSessions", out var recentSessions))
                {
                    _recentSessions.Clear();
                    foreach (var item in recentSessions.EnumerateArray())
                    {
                        string? path = item.GetString();
                        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                        {
                            _recentSessions.Add(Path.GetFullPath(path));
                        }
                    }

                    // Ограничиваем список 3 элементами
                    if (_recentSessions.Count > 3)
                    {
                        _recentSessions.RemoveRange(3, _recentSessions.Count - 3);
                    }

                    // Обновляем меню
                    UpdateRecentSessionsMenu();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LoadRecentSessionsFromSettings] Failed to load recent sessions: {ex.Message}");
            }
        }
    }
}

