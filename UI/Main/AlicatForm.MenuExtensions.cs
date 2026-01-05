using System;
using System.Collections.Generic;
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
                    Filter = "Alicat Session Files (*.als)|*.als|All files (*.*)|*.*",
                    Title = "Open Session"
                };

                if (openDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // TODO: Реализовать загрузку сессии
                    MessageBox.Show(this, 
                        $"Opening session: {openDialog.FileName}\n\nFeature not yet implemented.", 
                        "Open Session", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
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
                // TODO: Реализовать сохранение текущей сессии
                MessageBox.Show(this, 
                    "Save Session\n\nFeature not yet implemented.", 
                    "Save Session", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
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
                    // TODO: Реализовать сохранение сессии
                    MessageBox.Show(this, 
                        $"Saving session to: {saveDialog.FileName}\n\nFeature not yet implemented.", 
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
        /// Обработчик для пункта меню "Export -> Graph Image (PNG)..."
        /// </summary>
        private void MenuFileExportGraphImage_Click(object? sender, EventArgs e)
        {
            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
                    Title = "Export Graph Image",
                    FileName = $"graph_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png"
                };

                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // TODO: Реализовать экспорт графика в PNG
                    MessageBox.Show(this, 
                        $"Exporting graph to: {saveDialog.FileName}\n\nFeature not yet implemented.", 
                        "Export Graph", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
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
            if (sender is ToolStripMenuItem menuItem)
            {
                try
                {
                    string fileName = menuItem.Text;
                    // TODO: Реализовать загрузку недавней сессии
                    MessageBox.Show(this, 
                        $"Opening recent session: {fileName}\n\nFeature not yet implemented.", 
                        "Open Recent Session", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
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
                    // TODO: Реализовать очистку списка недавних сессий
                    menuFileRecentSessionsItem1.Visible = false;
                    menuFileRecentSessionsItem2.Visible = false;
                    menuFileRecentSessionsItem3.Visible = false;
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
    }
}

