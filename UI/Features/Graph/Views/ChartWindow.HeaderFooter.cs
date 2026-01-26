using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.IO;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using SkiaSharp;

namespace PrecisionPressureController.UI.Features.Graph.Views
{
    public partial class ChartWindow
    {
        // Note: UI elements are declared in ChartWindow.Designer.cs
        
        // Header timer
        private System.Windows.Forms.Timer? _headerTimer;

        // COM port and connection info
        private string? _comPortName;
        private int? _baudRate;

        /// <summary>
        /// Устанавливает информацию о COM порте для отображения в Header
        /// </summary>
        public void SetConnectionInfo(string? comPort, int? baudRate)
        {
            _comPortName = comPort;
            _baudRate = baudRate;
            UpdateHeaderConnectionInfo();
        }

        private void ConnectionStatusPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (_connectionStatusPanel == null) return;

            var panel = _connectionStatusPanel;
            
            // Draw border
            using (var pen = new Pen(Color.FromArgb(100, 200, 180), 1)) // Light teal border
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }

            // Draw green circle
            using (var brush = new SolidBrush(Color.FromArgb(76, 175, 80))) // Green
            {
                e.Graphics.FillEllipse(brush, 8, 8, 10, 10);
            }

            // Draw connection text
            string connectionText;
            if (!string.IsNullOrEmpty(_comPortName) && _baudRate.HasValue)
            {
                connectionText = $"Connected {_comPortName} @ {_baudRate.Value}";
            }
            else
            {
                connectionText = "Not Connected";
            }

            using (var brush = new SolidBrush(Color.FromArgb(100, 200, 180))) // Light teal text
            using (var font = new Font("Segoe UI", 9f))
            {
                e.Graphics.DrawString(connectionText, font, brush, 22, 6);
            }
        }

        private void UpdateHeaderConnectionInfo()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateHeaderConnectionInfo));
                return;
            }

            // Invalidate connection panel to trigger repaint
            if (_connectionStatusPanel != null)
            {
                _connectionStatusPanel.Invalidate();
            }
        }

        private void UpdateHeaderSessionTime()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateHeaderSessionTime));
                return;
            }

            // Обновляем Session time только если сессия запущена
            if (lblSessionTime != null && _dataStore != null && _dataStore.IsRunning)
            {
                TimeSpan duration = DateTime.Now - _dataStore.SessionStart;
                int hours = (int)duration.TotalHours;
                int minutes = duration.Minutes;
                int seconds = duration.Seconds;
                
                // Format: "Session: 00:09:23"
                lblSessionTime.Text = $"Session: {hours:D2}:{minutes:D2}:{seconds:D2}";
                
                // Обновляем статистику (включая Duration) при каждом обновлении времени
                CalculateAndUpdateStatistics();
            }
            else if (lblSessionTime != null)
            {
                lblSessionTime.Text = "Session: 00:00:00";
                // Если сессия не запущена, обновляем статистику чтобы показать Duration = 00:00
                CalculateAndUpdateStatistics();
            }
        }

        private void InitializeHeaderFooter()
        {
            // Initialize header timer for session time update
            _headerTimer = new System.Windows.Forms.Timer { Interval = 1000 }; // Update every second
            _headerTimer.Tick += (_, __) => UpdateHeaderSessionTime();
            // НЕ запускаем таймер по умолчанию - только когда создается новая сессия
            // _headerTimer.Start();

            // Подписываемся на события сессии
            if (_dataStore != null)
            {
                _dataStore.OnSessionStarted += () =>
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() => _headerTimer?.Start()));
                    }
                    else
                    {
                        _headerTimer?.Start();
                    }
                };

                _dataStore.OnSessionEnded += () =>
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() => _headerTimer?.Stop()));
                    }
                    else
                    {
                        _headerTimer?.Stop();
                    }
                };

                // Если сессия уже запущена, запускаем таймер
                if (_dataStore.IsRunning)
                {
                    _headerTimer.Start();
                }
            }

            // Setup header panel layout
            SetupHeaderLayout();

            // Setup chart header legend with colored squares
            SetupChartHeaderLegend();

            // Footer elements removed - no longer needed
        }

        // Checkboxes for legend visibility control - removed

        // Header connection status panel
        private Panel? _connectionStatusPanel;

        private void SetupChartHeaderLegend()
        {
            // Легенда настраивается в ConfigureChart() через chartPressure.LegendPosition
            // Старая легенда (flowLegend) была удалена
        }

        private void SetupHeaderLayout()
        {
            if (panelHeader == null) return;

            // Setup app icon paint event
            if (appIcon != null)
            {
                appIcon.Paint += (s, e) =>
                {
                    // Draw three vertical bars
                    using (var greenBrush = new SolidBrush(Color.FromArgb(76, 175, 80))) // Green
                    using (var blueBrush = new SolidBrush(Color.FromArgb(0, 200, 240))) // Light blue
                    using (var pinkBrush = new SolidBrush(Color.FromArgb(244, 67, 54))) // Red/Pink
                    {
                        e.Graphics.FillRectangle(greenBrush, 2, 4, 4, 12);
                        e.Graphics.FillRectangle(blueBrush, 8, 2, 4, 14);
                        e.Graphics.FillRectangle(pinkBrush, 14, 6, 4, 10);
                    }
                };
            }

            // Connection status panel (created dynamically as it needs Paint event)
            if (headerLeftFlowPanel != null && _connectionStatusPanel == null)
            {
                _connectionStatusPanel = new Panel
                {
                    Size = new Size(200, 28),
                    Margin = new Padding(0, 11, 12, 0),
                    BackColor = Color.FromArgb(26, 61, 53), // #1A3D35
                    Padding = new Padding(8, 0, 8, 0)
                };
                _connectionStatusPanel.Paint += ConnectionStatusPanel_Paint;
                
                // Insert after lblAppTitle
                int insertIndex = headerLeftFlowPanel.Controls.IndexOf(lblAppTitle) + 1;
                headerLeftFlowPanel.Controls.Add(_connectionStatusPanel);
                headerLeftFlowPanel.Controls.SetChildIndex(_connectionStatusPanel, insertIndex);
            }

            // Ensure buttons are visible
            if (btnPause != null)
            {
                btnPause.Visible = true;
            }
            if (btnExport != null)
            {
                btnExport.Visible = true;
            }

            // Setup button paint events and click handlers
            if (btnPause != null)
            {
                // Remove existing handlers to avoid duplicates
                btnPause.Paint -= BtnPause_Paint;
                btnPause.Click -= btnPause_Click;
                
                // Add handlers
                btnPause.Paint += BtnPause_Paint;
                btnPause.Click += btnPause_Click;
            }

            if (btnExport != null)
            {
                // Remove existing handlers to avoid duplicates
                btnExport.Paint -= BtnExport_Paint;
                btnExport.Click -= btnExport_Click;
                
                // Add handlers
                btnExport.Paint += BtnExport_Paint;
                btnExport.Click += btnExport_Click;
            }
        }

        // Button paint event handlers
        private void BtnPause_Paint(object? sender, PaintEventArgs e)
        {
            if (btnPause == null) return;
            
            // Draw pause icon (two vertical bars)
            using (var brush = new SolidBrush(btnPause.ForeColor))
            {
                e.Graphics.FillRectangle(brush, 12, 8, 3, 12);
                e.Graphics.FillRectangle(brush, 18, 8, 3, 12);
            }

            // Draw "Pause" text
            using (var font = new Font("Segoe UI", 9f))
            using (var brush = new SolidBrush(btnPause.ForeColor))
            {
                e.Graphics.DrawString("Pause", font, brush, 28, 6);
            }
        }

        private void BtnExport_Paint(object? sender, PaintEventArgs e)
        {
            if (btnExport == null) return;
            
            // Draw document icon (blue rectangle with red arrow)
            using (var blueBrush = new SolidBrush(Color.FromArgb(66, 133, 244))) // Blue
            using (var redBrush = new SolidBrush(Color.FromArgb(244, 67, 54))) // Red
            {
                // Document rectangle
                e.Graphics.FillRectangle(blueBrush, 12, 6, 12, 16);
                
                // Red arrow pointing up-right
                var arrowPoints = new Point[]
                {
                    new Point(22, 8),
                    new Point(24, 8),
                    new Point(24, 10),
                    new Point(26, 10),
                    new Point(24, 12),
                    new Point(22, 10)
                };
                e.Graphics.FillPolygon(redBrush, arrowPoints);
            }

            // Draw "Export" text
            using (var font = new Font("Segoe UI", 9f))
            using (var brush = new SolidBrush(btnExport.ForeColor))
            {
                e.Graphics.DrawString("Export", font, brush, 28, 6);
            }
        }


        private void ResetGraph()
        {
            if (MessageBox.Show("Reset all graph data? This will clear the current session data from the graph.", 
                "Reset Graph", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _seriesCurrent.Clear();
                _timeSeconds = 0;
                _lastTargetValue = null;
                ApplyTimeWindow(forceTrim: false);
                CalculateAndUpdateStatistics();
                UpdateLiveStatus(0, null, "PSIG", false, 0);
            }
        }

        // Footer elements were removed - UpdateFooterAutoSave and UpdateFooterStatistics are no longer needed

        // Pause handler delegate
        private Action? _pauseHandler;
        
        /// <summary>
        /// Устанавливает обработчик для кнопки Pause
        /// </summary>
        public void SetPauseHandler(Action handler)
        {
            _pauseHandler = handler;
        }
        
        /// <summary>
        /// Обновляет состояние кнопки Pause
        /// </summary>
        public void UpdatePauseState(bool isPaused)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdatePauseState(isPaused)));
                return;
            }
            
            if (btnPause != null)
            {
                btnPause.Text = isPaused ? "▶ Resume" : "II Pause";
            }
        }
        
        private void btnPause_Click(object? sender, EventArgs e)
        {
            _pauseHandler?.Invoke();
        }

        /// <summary>
        /// Получает размеры графика для экспорта
        /// </summary>
        public Rectangle GetChartBounds()
        {
            return chartPressure.Bounds;
        }
        
        /// <summary>
        /// Рендерит график на SKSurface используя DrawToBitmap и конвертацию в SkiaSharp
        /// </summary>
        private void RenderChartToSurface(SKSurface surface, int width, int height)
        {
            var canvas = surface.Canvas;
            
            // Очищаем canvas фоном
            canvas.Clear(_isDarkTheme ? new SKColor(22, 24, 30) : SKColors.White);
            
            // Используем DrawToBitmap для получения изображения графика
            using var bitmap = new Bitmap(chartPressure.Width, chartPressure.Height);
            chartPressure.DrawToBitmap(bitmap, new Rectangle(0, 0, chartPressure.Width, chartPressure.Height));
            
            // Конвертируем System.Drawing.Bitmap в SKBitmap через MemoryStream
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            using var skBitmap = SKBitmap.Decode(ms);
            
            // Масштабируем и рисуем на canvas с высоким качеством
            var destRect = new SKRect(0, 0, width, height);
            var srcRect = new SKRect(0, 0, skBitmap.Width, skBitmap.Height);
            
            using var paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High, // Высокое качество масштабирования
                IsAntialias = true
            };
            
            canvas.DrawBitmap(skBitmap, srcRect, destRect, paint);
        }

        /// <summary>
        /// Получает текущую тему графика
        /// </summary>
        public bool IsDarkTheme => _isDarkTheme;

        /// <summary>
        /// Показывает диалог экспорта и экспортирует график
        /// </summary>
        public void ShowExportDialog()
        {
            // Получаем размер графика для отображения в диалоге
            var chartBounds = chartPressure.Bounds;
            
            // Показываем диалог выбора формата и размера
            using var exportDialog = new ExportChartDialog(_isDarkTheme, chartBounds.Width, chartBounds.Height)
            {
                StartPosition = FormStartPosition.CenterParent
            };

            if (exportDialog.ShowDialog(this) == DialogResult.OK)
            {
                ExportChartWithSettings(exportDialog.Settings);
            }
        }

        private void btnExport_Click(object? sender, EventArgs e)
        {
            ShowExportDialog();
        }

        /// <summary>
        /// Экспортирует график с учетом настроек (формат и масштаб)
        /// Публичный метод для использования из главного меню
        /// </summary>
        public void ExportChartWithSettings(ExportSettings settings)
        {
            try
            {
                // 1. Определяем папку для сохранения
                string? targetDirectory = null;

                // Если сессия имеет CSV файл, используем его папку
                if (_dataStore?.CsvPath != null)
                {
                    targetDirectory = System.IO.Path.GetDirectoryName(_dataStore.CsvPath);
                }

                // Если папки нет, показываем диалог выбора
                if (string.IsNullOrEmpty(targetDirectory) || !System.IO.Directory.Exists(targetDirectory))
                {
                    using var folderDialog = new FolderBrowserDialog
                    {
                        Description = "Select folder for graph export",
                        ShowNewFolderButton = true
                    };

                    if (folderDialog.ShowDialog() != DialogResult.OK)
                    {
                        return; // Пользователь отменил
                    }

                    targetDirectory = folderDialog.SelectedPath;
                }

                // 2. Генерируем базовое имя файла
                string extension = settings.Format.ToLowerInvariant();
                string baseFileName = $"graph_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";
                string filePath = System.IO.Path.Combine(targetDirectory, $"{baseFileName}.{extension}");

                // 3. Проверяем существование и добавляем нумерацию если нужно
                int counter = 1;
                while (System.IO.File.Exists(filePath))
                {
                    string numberedFileName = $"{baseFileName}_{counter:D3}.{extension}";
                    filePath = System.IO.Path.Combine(targetDirectory, numberedFileName);
                    counter++;
                }

                // 4. Экспортируем график используя встроенные методы LiveCharts2 через SkiaSharp
                var chartBounds = chartPressure.Bounds;
                int width = chartBounds.Width * settings.Scale;
                int height = chartBounds.Height * settings.Scale;

                if (settings.Format == "PNG")
                {
                    // PNG экспорт через SKSurface (встроенный метод LiveCharts2 через SkiaSharp)
                    using var surface = SKSurface.Create(new SKImageInfo(width, height));
                    RenderChartToSurface(surface, width, height);
                    
                    // Сохраняем в PNG
                    using var image = surface.Snapshot();
                    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                    using var stream = File.OpenWrite(filePath);
                    data.SaveTo(stream);
                }
                else if (settings.Format == "SVG")
                {
                    // SVG экспорт: используем PNG из SkiaSharp и встраиваем в SVG
                    using var surface = SKSurface.Create(new SKImageInfo(width, height));
                    RenderChartToSurface(surface, width, height);
                    
                    // Получаем PNG изображение
                    using var image = surface.Snapshot();
                    using var pngData = image.Encode(SKEncodedImageFormat.Png, 100);
                    byte[] imageBytes = pngData.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);
                    
                    // Создаем SVG файл с встроенным PNG изображением
                    string svgContent = $@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg xmlns=""http://www.w3.org/2000/svg"" 
     xmlns:xlink=""http://www.w3.org/1999/xlink""
     width=""{width}"" 
     height=""{height}"" 
     viewBox=""0 0 {width} {height}"">
  <defs>
    <style type=""text/css"">
      <![CDATA[
        .chart-container {{
          width: 100%;
          height: 100%;
        }}
      ]]>
    </style>
  </defs>
  <image x=""0"" y=""0"" 
         width=""{width}"" 
         height=""{height}"" 
         xlink:href=""data:image/png;base64,{base64Image}""/>
</svg>";
                    
                    File.WriteAllText(filePath, svgContent, System.Text.Encoding.UTF8);
                }

                // 5. Показываем уведомление об успешном сохранении
                MessageBox.Show(
                    $"Graph exported successfully!\n\nFile: {System.IO.Path.GetFileName(filePath)}\nLocation: {targetDirectory}\nFormat: {settings.Format}\nSize: {width} × {height}",
                    "Export Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error exporting graph:\n{ex.Message}",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Target handler delegate
        private Action<double>? _targetHandler;
        private Action<double>? _targetHandlerSilent;
        
        /// <summary>
        /// Устанавливает обработчик для кнопки GO TARGET
        /// </summary>
        public void SetTargetHandler(Action<double> handler)
        {
            _targetHandler = handler;
        }

        /// <summary>
        /// Устанавливает обработчик для GO TO TARGET секции (без подтверждения)
        /// </summary>
        public void SetTargetHandlerSilent(Action<double> handler)
        {
            _targetHandlerSilent = handler;
        }
        
        private void btnGoTarget_Click(object? sender, EventArgs e)
        {
            // Удален txtTargetValue
            // if (txtTargetValue == null) return;
            // if (double.TryParse(txtTargetValue.Text, System.Globalization.NumberStyles.Float, 
            //     System.Globalization.CultureInfo.InvariantCulture, out double target))
            // {
            //     _targetHandler?.Invoke(target);
            // }
            // else
            // {
            //     MessageBox.Show("Invalid target value. Please enter a number.", "Error",
            //         MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }
            return; // Функциональность отключена
        }

        private void btnFullscreenHeader_Click(object? sender, EventArgs e)
        {
            // Toggle fullscreen
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
            }
        }

        /// <summary>
        /// Применяет тему (Light/Dark) к форме
        /// </summary>
        public void ApplyTheme(bool isDark)
        {
            _isDarkTheme = isDark;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ApplyTheme(isDark)));
                return;
            }

            if (isDark)
            {
                // Dark theme colors
                BackColor = Color.FromArgb(17, 19, 23);
                if (panelLeft != null) panelLeft.BackColor = Color.FromArgb(21, 23, 28);
                if (panelRight != null) panelRight.BackColor = Color.FromArgb(21, 23, 28);
                if (panelHeader != null) panelHeader.BackColor = Color.FromArgb(21, 23, 28);
                if (panelBottom != null) panelBottom.BackColor = Color.FromArgb(21, 23, 28);
                
                // LIVE STATUS panel - dark background (same as TIME WINDOW)
                if (pnlLiveStatus != null)
                {
                    pnlLiveStatus.BackColor = Color.FromArgb(32, 35, 44);
                }
                
                // THRESHOLDS, DISPLAY, ALERTS panels - dark background (same as LiveStatus)
                if (grpThresholds != null)
                {
                    grpThresholds.BackColor = Color.FromArgb(32, 35, 44);
                }
                if (grpDisplay != null)
                {
                    grpDisplay.BackColor = Color.FromArgb(32, 35, 44);
                }
                if (grpAlerts != null)
                {
                    grpAlerts.BackColor = Color.FromArgb(32, 35, 44);
                }
                // Left panel GroupBoxes
                if (grpLiveStatus != null)
                {
                    grpLiveStatus.BackColor = Color.FromArgb(32, 35, 44);
                }
                if (grpSessionStats != null)
                {
                    grpSessionStats.BackColor = Color.FromArgb(32, 35, 44);
                }
                if (grpGoToTarget != null)
                {
                    grpGoToTarget.BackColor = Color.FromArgb(32, 35, 44);
                }
                
                // Chart - dark background (same as LiveStatus)
                if (chartPressure != null)
                {
                    chartPressure.BackColor = Color.FromArgb(32, 35, 44);
                }
                if (panelCenter != null)
                {
                    panelCenter.BackColor = Color.FromArgb(32, 35, 44);
                }
                
                // SESSION STATS panel - dark background
                if (tlpSessionStats != null)
                {
                    tlpSessionStats.BackColor = Color.Transparent;
                }
                
                // Update text colors for dark theme
                if (lblCurrentPressureLarge != null)
                {
                    lblCurrentPressureLarge.ForeColor = Color.White;
                }
                if (lblCurrentUnit != null)
                {
                    lblCurrentUnit.ForeColor = Color.FromArgb(180, 185, 195);
                }
                if (lblTarget != null)
                {
                    lblTarget.ForeColor = Color.FromArgb(150, 155, 165);
                }
                if (lblDelta != null)
                {
                    lblDelta.ForeColor = Color.FromArgb(150, 155, 165);
                }
                if (lblRate != null)
                {
                    lblRate.ForeColor = Color.FromArgb(150, 155, 165);
                }
                
                // Update SESSION STATS colors for dark theme
                UpdateSessionStatsColors(isDark: true);
                
                // Update ETA and Trend colors for dark theme
                if (lblETA != null) lblETA.ForeColor = Color.FromArgb(150, 155, 165);
                if (lblTrend != null) lblTrend.ForeColor = Color.FromArgb(150, 155, 165);
            }
            else
            {
                // Light theme colors
                BackColor = Color.FromArgb(245, 245, 250);
                if (panelLeft != null) panelLeft.BackColor = Color.FromArgb(250, 250, 255);
                if (panelRight != null) panelRight.BackColor = Color.FromArgb(250, 250, 255);
                if (panelHeader != null) panelHeader.BackColor = Color.FromArgb(250, 250, 255);
                if (panelBottom != null) panelBottom.BackColor = Color.FromArgb(250, 250, 255);
                
                // LIVE STATUS panel - light background
                if (pnlLiveStatus != null)
                {
                    pnlLiveStatus.BackColor = Color.FromArgb(255, 255, 255);
                }
                
                // THRESHOLDS, DISPLAY, ALERTS panels - light background (same as LiveStatus)
                if (grpThresholds != null)
                {
                    grpThresholds.BackColor = Color.FromArgb(255, 255, 255);
                }
                if (grpDisplay != null)
                {
                    grpDisplay.BackColor = Color.FromArgb(255, 255, 255);
                }
                if (grpAlerts != null)
                {
                    grpAlerts.BackColor = Color.FromArgb(255, 255, 255);
                }
                // Left panel GroupBoxes
                if (grpLiveStatus != null)
                {
                    grpLiveStatus.BackColor = Color.FromArgb(255, 255, 255);
                }
                if (grpSessionStats != null)
                {
                    grpSessionStats.BackColor = Color.FromArgb(255, 255, 255);
                }
                if (grpGoToTarget != null)
                {
                    grpGoToTarget.BackColor = Color.FromArgb(255, 255, 255);
                }
                
                // Chart - light background (same as LiveStatus)
                if (chartPressure != null)
                {
                    chartPressure.BackColor = Color.FromArgb(255, 255, 255);
                }
                if (panelCenter != null)
                {
                    panelCenter.BackColor = Color.FromArgb(255, 255, 255);
                }
                
                // SESSION STATS panel - light background
                if (tlpSessionStats != null)
                {
                    tlpSessionStats.BackColor = Color.Transparent;
                }
                
                // Update text colors for light theme
                if (lblCurrentPressureLarge != null)
                {
                    lblCurrentPressureLarge.ForeColor = Color.FromArgb(30, 30, 35);
                }
                if (lblCurrentUnit != null)
                {
                    lblCurrentUnit.ForeColor = Color.FromArgb(100, 105, 115);
                }
                if (lblTarget != null)
                {
                    lblTarget.ForeColor = Color.FromArgb(80, 85, 95);
                }
                if (lblDelta != null)
                {
                    lblDelta.ForeColor = Color.FromArgb(80, 85, 95);
                }
                if (lblRate != null)
                {
                    lblRate.ForeColor = Color.FromArgb(80, 85, 95);
                }
                
                // Update SESSION STATS colors for light theme
                UpdateSessionStatsColors(isDark: false);
                
                // Update ETA and Trend colors for light theme
                if (lblETA != null) lblETA.ForeColor = Color.FromArgb(80, 85, 95);
                if (lblTrend != null) lblTrend.ForeColor = Color.FromArgb(80, 85, 95);
            }
            
            // Force repaint of LIVE STATUS panel border
            if (pnlLiveStatus != null)
            {
                pnlLiveStatus.Invalidate();
            }
        }

        /// <summary>
        /// Обновляет цвета элементов SESSION STATS в зависимости от темы
        /// </summary>
        private void UpdateSessionStatsColors(bool isDark)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateSessionStatsColors(isDark)));
                return;
            }

            Color labelColor = isDark ? Color.FromArgb(120, 125, 140) : Color.FromArgb(80, 85, 95);
            Color valueColor = isDark ? Color.White : Color.FromArgb(30, 30, 35);

            // Update labels (lblSessionStatsTitle removed - now using GroupBox Text)
            if (lblMinLabel != null) lblMinLabel.ForeColor = labelColor;
            if (lblMaxLabel != null) lblMaxLabel.ForeColor = labelColor;
            if (lblAvgLabel != null) lblAvgLabel.ForeColor = labelColor;
            if (lblStdDevLabel != null) lblStdDevLabel.ForeColor = labelColor;
            if (lblPointsLabel != null) lblPointsLabel.ForeColor = labelColor;
            if (lblDurationLabel != null) lblDurationLabel.ForeColor = labelColor;
            if (lblSampleRateLabel != null) lblSampleRateLabel.ForeColor = labelColor;

            // Update values
            if (lblMinValue != null) lblMinValue.ForeColor = valueColor;
            if (lblMaxValue != null) lblMaxValue.ForeColor = valueColor;
            if (lblAvgValue != null) lblAvgValue.ForeColor = valueColor;
            if (lblStdDevValue != null) lblStdDevValue.ForeColor = valueColor;
            if (lblPointsValue != null) lblPointsValue.ForeColor = valueColor;
            if (lblDurationValue != null) lblDurationValue.ForeColor = valueColor;
            if (lblSampleRateValue != null) lblSampleRateValue.ForeColor = valueColor;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _headerTimer?.Stop();
            _headerTimer?.Dispose();
            base.OnFormClosed(e);
        }

        private void PanelAlertsIcon_Paint(object? sender, PaintEventArgs e)
        {
            if (panelAlertsIcon == null || sender != panelAlertsIcon) return;

            var panel = panelAlertsIcon;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Золотистый цвет для колокольчика
            using (var brush = new SolidBrush(Color.FromArgb(255, 214, 69))) // Golden yellow
            {
                // Рисуем колокольчик (упрощенная форма)
                var rect = new Rectangle(2, 2, panel.Width - 4, panel.Height - 4);
                
                // Основная часть колокольчика (треугольник с закругленным низом)
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(rect.X, rect.Y + rect.Height / 3, rect.Width, rect.Height * 2 / 3);
                path.AddPolygon(new Point[] {
                    new Point(rect.X + rect.Width / 2, rect.Y),
                    new Point(rect.X + rect.Width / 4, rect.Y + rect.Height / 3),
                    new Point(rect.X + rect.Width * 3 / 4, rect.Y + rect.Height / 3)
                });
                
                g.FillPath(brush, path);
                
                // Язычок колокольчика (маленький круг внизу)
                var tongueRect = new Rectangle(rect.X + rect.Width / 2 - 2, rect.Y + rect.Height - 4, 4, 4);
                g.FillEllipse(brush, tongueRect);
            }
        }
    }
}

