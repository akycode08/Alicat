using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.Easing;
using SkiaSharp;
using PrecisionPressureController.Business.Interfaces;
using PrecisionPressureController.Services.Data;
using DataPointModel = PrecisionPressureController.Services.Data.DataPoint;
using PrecisionPressureController.UI.Main;
using PrecisionPressureController.UI.Options;

namespace PrecisionPressureController.UI.Features.Graph.Views
{
    public partial class ChartWindow : Form, IGraphView
    {
        private readonly SessionDataStore _dataStore;
        
        // Pagination buttons
        private Button? btnPage1;
        private Button? btnPage2;
        private Button? btnPage3;
        private Button? btnPage4;
        private Button? btnPage5;
        private Button? btnPage6;
        // --- series (LiveCharts2) ---
        private ObservableCollection<ObservablePoint> _seriesCurrent = null!;
        private ObservableCollection<ObservablePoint> _seriesTarget = null!;
        private ObservableCollection<ObservablePoint> _seriesMin = null!;
        private ObservableCollection<ObservablePoint> _seriesMax = null!;
        
        // Series references for visibility control (accessible from partial classes)
        internal LineSeries<ObservablePoint>? _lineSeriesTarget;
        internal LineSeries<ObservablePoint>? _lineSeriesMin;
        internal LineSeries<ObservablePoint>? _lineSeriesMax;

        // time in seconds (X)
        private double _timeSeconds = 0;
        private const double TimeStep = 0.5;

        // Time window (seconds)
        private double _timeWindowSeconds = 60;

        // Grid step X (auto)
        private double _gridStepXSeconds = 10;

        // Cursor (LiveCharts2 uses different approach)
        private ObservableCollection<ObservablePoint> _cursorMarker = null!;

        // Cursor info panel
        private Panel _cursorInfoPanel = null!;
        private Label _lblInfoTime = null!;
        private Label _lblInfoPressure = null!;
        private Label _lblInfoTarget = null!;
        private Label _lblInfoRate = null!;

        // Cursor performance
        private int _lastNearestIndex = -1;
        private long _lastCursorTick = 0;
        private const int CursorThrottleMs = 16; // ~60fps

        // Target value cache (for flat line)
        private double? _lastTargetValue = null;

        // Update batching for performance
        private readonly List<(double time, double pressure, double? target)> _updateBuffer = new();
        private System.Windows.Forms.Timer? _updateTimer;
        private const int UpdateIntervalMs = 100; // 10 FPS (100ms = 10 updates per second)

        // Theme support
        protected bool _isDarkTheme = true;

        // Optional delegate for saving settings when thresholds change
        private Action? _onThresholdsChanged;

        // Pagination data: (Page Name, Seconds, GridStepXSeconds)
        private static readonly (string Name, int Seconds, int GridStep)[] PaginationData = new[]
        {
            ("Page 1",   300,    30),   // 5 mins
            ("Page 2",   900,    60),   // 15 mins
            ("Page 3",   3600,   600),  // 1 hour
            ("Page 4",   14400,  1800), // 4 hours
            ("Page 5",   36000,  3600), // 10 hours
            ("Page 6",   -1,     1800)  // All data (special case: -1 = no limit)
        };
        
        // Legacy Duration data (kept for compatibility)
        private static readonly (string Name, int Seconds, int GridStep)[] DurationData = PaginationData;
        
        // Current selected page index
        private int _currentPageIndex = 0;

        public ChartWindow(SessionDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeComponent();
            
            ConfigureChart();
            ComboBoxValues();
            CreateCursorInfoPanel();

            // handlers (pagination / grid / thresholds)
            // Duration ComboBox removed - now using pagination buttons
            InitializePaginationButtonHandlers();
            
            // Right panel handlers
            // Y Step removed - LiveCharts2 automatically manages Y axis grid step
            
            if (chkShowGrid != null)
                chkShowGrid.CheckedChanged += ChkShowGrid_CheckedChanged;
            
            // Use NumericUpDown for thresholds (new design)
            if (numMaxThreshold != null)
            {
                numMaxThreshold.ValueChanged += (s, e) => 
                {
                    if (txtMaxThreshold != null)
                        txtMaxThreshold.Text = numMaxThreshold.Value.ToString("F0");
                };
            }
            
            if (numMinThreshold != null)
            {
                numMinThreshold.ValueChanged += (s, e) => 
                {
                    if (txtMinThreshold != null)
                        txtMinThreshold.Text = numMinThreshold.Value.ToString("F0");
                };
            }
            
            // Apply button handler
            if (btnApplyThresholds != null)
            {
                btnApplyThresholds.Click += (s, e) =>
                {
                    if (numMaxThreshold != null && txtMaxThreshold != null)
                        txtMaxThreshold.Text = numMaxThreshold.Value.ToString("F0");
                    if (numMinThreshold != null && txtMinThreshold != null)
                        txtMinThreshold.Text = numMinThreshold.Value.ToString("F0");
                    Thresholds_TextChanged(null, EventArgs.Empty);
                };
            }
            
            // Keep old TextBox handlers for backward compatibility
            if (txtMaxThreshold != null)
            {
                txtMaxThreshold.TextChanged += Thresholds_TextChanged;
                txtMaxThreshold.Leave += Thresholds_TextChanged;
            }
            
            if (txtMinThreshold != null)
            {
                txtMinThreshold.TextChanged += Thresholds_TextChanged;
                txtMinThreshold.Leave += Thresholds_TextChanged;
            }
            
            
            if (chkShowTarget != null)
                chkShowTarget.CheckedChanged += ChkShowTarget_CheckedChanged;
            
            // Emergency Vent button (right panel)
            if (btnEmergencyRight != null)
            {
                btnEmergencyRight.Click += BtnEmergency_Click;
                // Hover effects for Emergency button
                btnEmergencyRight.MouseEnter += (s, e) => btnEmergencyRight.BackColor = Color.FromArgb(220, 38, 38);
                btnEmergencyRight.MouseLeave += (s, e) => btnEmergencyRight.BackColor = Color.FromArgb(239, 68, 68);
            }
            
            // Hover effects for Apply button
            if (btnApplyThresholds != null)
            {
                btnApplyThresholds.MouseEnter += (s, e) => btnApplyThresholds.BackColor = Color.FromArgb(5, 150, 105);
                btnApplyThresholds.MouseLeave += (s, e) => btnApplyThresholds.BackColor = Color.FromArgb(16, 185, 129);
            }
            
            // Move existing CheckBoxes from tlpDisplay to grpDisplay
            // This must be done in constructor, not in InitializeComponent (Designer cannot process dynamic control movement)
            if (chkShowGrid != null && tlpDisplay != null && grpDisplay != null && tlpDisplay.Controls.Contains(chkShowGrid))
            {
                tlpDisplay.Controls.Remove(chkShowGrid);
                chkShowGrid.Location = new Point(12, 22);
                chkShowGrid.Size = new Size(200, 20);
                chkShowGrid.Dock = DockStyle.None;
                chkShowGrid.ForeColor = Color.FromArgb(156, 163, 175);
                chkShowGrid.Font = new Font("Segoe UI", 9F);
                chkShowGrid.FlatStyle = FlatStyle.Flat;
                grpDisplay.Controls.Add(chkShowGrid);
            }
            if (chkShowTarget != null && tlpDisplay != null && grpDisplay != null && tlpDisplay.Controls.Contains(chkShowTarget))
            {
                tlpDisplay.Controls.Remove(chkShowTarget);
                chkShowTarget.Location = new Point(12, 44);
                chkShowTarget.Size = new Size(200, 20);
                chkShowTarget.Dock = DockStyle.None;
                chkShowTarget.ForeColor = Color.FromArgb(156, 163, 175);
                chkShowTarget.Font = new Font("Segoe UI", 9F);
                chkShowTarget.FlatStyle = FlatStyle.Flat;
                grpDisplay.Controls.Add(chkShowTarget);
            }
            if (chkShowMax != null && tlpDisplay != null && grpDisplay != null && tlpDisplay.Controls.Contains(chkShowMax))
            {
                tlpDisplay.Controls.Remove(chkShowMax);
                chkShowMax.Location = new Point(12, 66);
                chkShowMax.Size = new Size(200, 20);
                chkShowMax.Dock = DockStyle.None;
                chkShowMax.ForeColor = Color.FromArgb(156, 163, 175);
                chkShowMax.Font = new Font("Segoe UI", 9F);
                chkShowMax.FlatStyle = FlatStyle.Flat;
                grpDisplay.Controls.Add(chkShowMax);
            }
            if (chkShowMin != null && tlpDisplay != null && grpDisplay != null && tlpDisplay.Controls.Contains(chkShowMin))
            {
                tlpDisplay.Controls.Remove(chkShowMin);
                chkShowMin.Location = new Point(12, 88);
                chkShowMin.Size = new Size(200, 20);
                chkShowMin.Dock = DockStyle.None;
                chkShowMin.ForeColor = Color.FromArgb(156, 163, 175);
                chkShowMin.Font = new Font("Segoe UI", 9F);
                chkShowMin.FlatStyle = FlatStyle.Flat;
                grpDisplay.Controls.Add(chkShowMin);
            }
            
            // Move existing CheckBoxes from tlpAlerts to grpAlerts
            if (chkSound != null && tlpAlerts != null && grpAlerts != null && tlpAlerts.Controls.Contains(chkSound))
            {
                tlpAlerts.Controls.Remove(chkSound);
                chkSound.Location = new Point(12, 22);
                chkSound.Size = new Size(200, 20);
                chkSound.Dock = DockStyle.None;
                chkSound.ForeColor = Color.FromArgb(156, 163, 175);
                chkSound.Font = new Font("Segoe UI", 9F);
                chkSound.FlatStyle = FlatStyle.Flat;
                grpAlerts.Controls.Add(chkSound);
            }
            if (chkAtTarget != null && tlpAlerts != null && grpAlerts != null && tlpAlerts.Controls.Contains(chkAtTarget))
            {
                tlpAlerts.Controls.Remove(chkAtTarget);
                chkAtTarget.Location = new Point(12, 44);
                chkAtTarget.Size = new Size(200, 20);
                chkAtTarget.Dock = DockStyle.None;
                chkAtTarget.ForeColor = Color.FromArgb(156, 163, 175);
                chkAtTarget.Font = new Font("Segoe UI", 9F);
                chkAtTarget.FlatStyle = FlatStyle.Flat;
                grpAlerts.Controls.Add(chkAtTarget);
            }
            if (chkAtMax != null && tlpAlerts != null && grpAlerts != null && tlpAlerts.Controls.Contains(chkAtMax))
            {
                tlpAlerts.Controls.Remove(chkAtMax);
                chkAtMax.Location = new Point(12, 66);
                chkAtMax.Size = new Size(200, 20);
                chkAtMax.Dock = DockStyle.None;
                chkAtMax.ForeColor = Color.FromArgb(156, 163, 175);
                chkAtMax.Font = new Font("Segoe UI", 9F);
                chkAtMax.FlatStyle = FlatStyle.Flat;
                grpAlerts.Controls.Add(chkAtMax);
            }
            
            // Hide old TableLayoutPanels (they are no longer used)
            if (tlpThresholds != null) tlpThresholds.Visible = false;
            if (tlpDisplay != null) tlpDisplay.Visible = false;
            if (tlpAlerts != null) tlpAlerts.Visible = false;
            
            if (chkShowMax != null)
                chkShowMax.CheckedChanged += ChkShowMax_CheckedChanged;
            
            if (chkShowMin != null)
                chkShowMin.CheckedChanged += ChkShowMin_CheckedChanged;
            
            if (chkSound != null)
                chkSound.CheckedChanged += ChkSound_CheckedChanged;
            
            if (chkAtTarget != null)
                chkAtTarget.CheckedChanged += ChkAtTarget_CheckedChanged;
            
            if (chkAtMax != null)
                chkAtMax.CheckedChanged += ChkAtMax_CheckedChanged;

            // Load thresholds from settings
            LoadThresholdsFromSettings();

            // default page (Page 1 = 5 mins)
            _currentPageIndex = 0;
            _timeWindowSeconds = GetDurationSeconds(0);
            _gridStepXSeconds = GetAutoGridStepX(_timeWindowSeconds);
            UpdateGridStepXDisplay();

            ApplyGridSettings();
            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            UpdateTargetLine();        // <- target flat line
            UpdateCustomLabelsX();     // <- X labels
            UpdateSeriesVisibility();  // <- update series visibility based on checkboxes

            // Zoom and pan modes will be controlled by toolbar buttons
            // Default: no zoom/pan (None)
            chartPressure.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.None;
            
            // cursor events
            chartPressure.MouseDoubleClick += ChartPressure_MouseDoubleClick;
            chartPressure.MouseMove += ChartPressure_MouseMove;
            chartPressure.MouseLeave += ChartPressure_MouseLeave;
            chartPressure.MouseDown += ChartPressure_MouseDown;
            chartPressure.MouseUp += ChartPressure_MouseUp;

            // ApplySmoothing(); // Удален chkSmoothing

            // Загрузить историю из Store
            LoadHistoryFromStore();

            // Подписаться на новые точки
            _dataStore.OnNewPoint += OnNewPointReceived;

            // Initialize statistics
            CalculateAndUpdateStatistics();

            // Initialize with zero values
            UpdateLiveStatus(0, null, "PSIG", false, 0);

            // Initialize header and footer
            InitializeHeaderFooter();

            // Initialize toolbar (after header is ready)
            InitializeToolbar();

            // Initialize GO TO TARGET section
            InitializeGoToTarget();
            
            // Load Display settings from JSON (after all controls are initialized)
            LoadDisplaySettings();
            
            // Load Duration settings from JSON (after pagination buttons are initialized)
            LoadDurationSettings();
            
            // Remove custom zoom/pan setup - using LiveCharts2 built-in functionality
            
            // Initialize Hold timer to 00:00
            if (lblHoldTimer != null)
            {
                lblHoldTimer.Text = "Hold:           00:00";
            }
            if (progressBarHold != null)
            {
                progressBarHold.Value = 0;
            }
            
            // Устанавливаем обработчик для установки целевого давления
            // (будет установлен извне через SetTargetHandler)

            // Setup mouse wheel zoom and pan
            // LiveCharts2 built-in zoom/pan is enabled via ZoomMode property
            
            // Setup keyboard shortcuts (Escape to cancel zoom/pan mode)
            KeyDown += ChartWindow_KeyDown;
            KeyPreview = true; // Enable key preview for form-level key handling


            // Setup chart header buttons (visual only, no logic)
            // Reset and Fullscreen buttons remain for design, but handlers are removed

            // Setup LIVE STATUS panel border
            if (pnlLiveStatus != null)
            {
                pnlLiveStatus.Paint += PnlLiveStatus_Paint;
            }

            // Initialize update timer for batching
            _updateTimer = new System.Windows.Forms.Timer { Interval = UpdateIntervalMs };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        // =========================
        // UI / ComboBox
        // =========================
        private void ComboBoxValues()
        {
            // Duration ComboBox removed - now using pagination buttons
            // Initialize pagination buttons will be done in InitializePaginationButtons()

            // Y Step dropdown removed - LiveCharts2 automatically manages Y axis grid step
        }
        
        private void InitializePaginationButtons()
        {
            // This will be called after controls are created
            // Buttons are created in Designer, we just need to wire up handlers
        }

        private double GetDurationSeconds(int index)
        {
            if (index < 0 || index >= PaginationData.Length) return 300; // Default: 5 mins
            var data = PaginationData[index];
            // -1 means "all data" - return max time from store
            if (data.Seconds == -1)
            {
                if (_dataStore.Points.Count > 0)
                {
                    return _dataStore.Points.Max(p => p.ElapsedSeconds);
                }
                return 3600; // Default 1 hour if no data
            }
            return data.Seconds;
        }
        
        private bool IsAllDataPage(int index)
        {
            if (index < 0 || index >= PaginationData.Length) return false;
            return PaginationData[index].Seconds == -1;
        }

        private double GetAutoGridStepX(double durationSeconds)
        {
            // For "all data" page, use default grid step
            if (durationSeconds < 0) return 1800;
            
            foreach (var d in PaginationData)
            {
                if (d.Seconds == -1) continue; // Skip "all data" page
                if (d.Seconds == (int)durationSeconds)
                    return d.GridStep;
            }
            return 60;
        }

        private string FormatGridStep(double seconds)
        {
            if (seconds < 60) return $"{(int)seconds}s";
            if (seconds < 3600) return $"{(int)(seconds / 60)}m";
            return $"{(int)(seconds / 3600)}h";
        }

        private void UpdateGridStepXDisplay()
        {
            // Удален cmbXStep
            // cmbXStep.Items.Clear();
            // cmbXStep.Items.Add($"AUTO ({FormatGridStep(_gridStepXSeconds)})");
            // cmbXStep.SelectedIndex = 0;
        }

        // =========================
        // X labels formatting (CustomLabels)
        // =========================
        private string FormatTimeLabel(double totalSeconds)
        {
            if (totalSeconds <= 0) return "0";

            int secs = (int)Math.Round(totalSeconds);
            int hours = secs / 3600;
            int mins = (secs % 3600) / 60;
            int seconds = secs % 60;

            // 1..5 min -> show seconds
            if (_timeWindowSeconds <= 300)
            {
                if (mins > 0 && seconds > 0) return $"{mins}m{seconds}s";
                if (mins > 0) return $"{mins}m";
                return $"{secs}s";
            }

            // 15 min .. 2h -> show minutes/hours
            if (_timeWindowSeconds <= 7200)
            {
                if (hours > 0 && mins > 0) return $"{hours}h{mins}m";
                if (hours > 0) return $"{hours}h";
                return $"{mins}m";
            }

            // 4h+ -> show hours/minutes
            if (hours > 0 && mins > 0) return $"{hours}h{mins}m";
            if (hours > 0) return $"{hours}h";
            return $"{mins}m";
        }

        private void UpdateCustomLabelsX()
        {
            // LiveCharts2 uses Labeler function for custom labels
            // This is already set in ConfigureChart(), so we don't need to do anything here
            // The FormatTimeLabel function is used automatically by the Labeler
        }

        // =========================
        // Cursor panel
        // =========================
        private void CreateCursorInfoPanel()
        {
            _cursorInfoPanel = new Panel
            {
                Size = new Size(160, 95),
                BackColor = Color.FromArgb(245, 18, 20, 26),
                Visible = false
            };

            _cursorInfoPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(60, 65, 75), 1);
                e.Graphics.DrawRectangle(pen, 0, 0, _cursorInfoPanel.Width - 1, _cursorInfoPanel.Height - 1);
            };

            var font = new Font("Consolas", 9f);

            _lblInfoTime = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(160, 165, 175),
                Font = font,
                Location = new Point(8, 8)
            };
            _lblInfoPressure = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(0, 200, 240),
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                Location = new Point(8, 28)
            };
            _lblInfoTarget = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(240, 200, 0),
                Font = font,
                Location = new Point(8, 48)
            };
            _lblInfoRate = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(160, 100, 240),
                Font = font,
                Location = new Point(8, 68)
            };

            _cursorInfoPanel.Controls.Add(_lblInfoTime);
            _cursorInfoPanel.Controls.Add(_lblInfoPressure);
            _cursorInfoPanel.Controls.Add(_lblInfoTarget);
            _cursorInfoPanel.Controls.Add(_lblInfoRate);

            chartPressure.Controls.Add(_cursorInfoPanel);
            _cursorInfoPanel.BringToFront();
        }

        // =========================
        // Chart setup (LiveCharts2)
        // =========================
        private void ConfigureChart()
        {
            // Initialize data collections
            _seriesCurrent = new ObservableCollection<ObservablePoint>();
            _seriesTarget = new ObservableCollection<ObservablePoint>();
            _seriesMin = new ObservableCollection<ObservablePoint>();
            _seriesMax = new ObservableCollection<ObservablePoint>();
            _cursorMarker = new ObservableCollection<ObservablePoint>();

            // Configure chart
            var darkBackground = new SKColor(22, 24, 30);
            var gridColor = new SKColor(100, 105, 115); // Сделал более светлым для лучшей видимости
            var axisColor = new SKColor(50, 55, 65);
            var labelColor = new SKColor(120, 125, 135);

            // Target line
            _lineSeriesTarget = new LineSeries<ObservablePoint>
            {
                Name = "Target",
                Values = _seriesTarget,
                Stroke = new SolidColorPaint(new SKColor(240, 200, 0), 2),
                Fill = null,
                GeometryStroke = null,
                GeometrySize = 0
            };
            
            // Min threshold
            _lineSeriesMin = new LineSeries<ObservablePoint>
            {
                Name = "Min",
                Values = _seriesMin,
                Stroke = new SolidColorPaint(new SKColor(76, 175, 80), 2), // Green
                Fill = null,
                GeometryStroke = null,
                GeometrySize = 0
            };
            
            // Max threshold
            _lineSeriesMax = new LineSeries<ObservablePoint>
            {
                Name = "Max",
                Values = _seriesMax,
                Stroke = new SolidColorPaint(new SKColor(244, 67, 54), 2), // Red
                Fill = null,
                GeometryStroke = null,
                GeometrySize = 0
            };

            chartPressure.Series = new ISeries[]
            {
                // Current pressure line
                new LineSeries<ObservablePoint>
                {
                    Name = "Current",
                    Values = _seriesCurrent,
                    Stroke = new SolidColorPaint(new SKColor(0, 200, 240), 2),
                    Fill = null,
                    GeometryStroke = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.6 // Плавные переходы вместо "лестниц" (0.0 = прямые линии, 1.0 = максимальное сглаживание)
                },
                // Target line
                _lineSeriesTarget,
                // Min threshold
                _lineSeriesMin,
                // Max threshold
                _lineSeriesMax,
                // Cursor marker
                new ScatterSeries<ObservablePoint>
                {
                    Name = "CursorMarker",
                    Values = _cursorMarker,
                    Stroke = new SolidColorPaint(SKColors.White, 2),
                    Fill = new SolidColorPaint(new SKColor(0, 220, 255)),
                    GeometrySize = 10,
                    IsVisible = false
                }
            };

            // Configure X axis
            chartPressure.XAxes = new[]
            {
                new Axis
                {
                    Name = "Time",
                    NamePaint = new SolidColorPaint(labelColor, 11), // Подпись оси "Time" снизу
                    MinLimit = 0,
                    MaxLimit = _timeWindowSeconds,
                    Labeler = value => FormatTimeLabel(value),
                    LabelsPaint = new SolidColorPaint(labelColor),
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(gridColor),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(axisColor),
                    TicksAtCenter = false
                }
            };

            // Configure Y axis
            chartPressure.YAxes = new[]
            {
                new Axis
                {
                    Name = "Pressure",
                    NamePaint = new SolidColorPaint(labelColor, 11), // Подпись оси "Pressure" слева
                    MinLimit = 0,
                    Labeler = value => value.ToString("F1"),
                    LabelsPaint = new SolidColorPaint(labelColor),
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(gridColor),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(axisColor),
                    TicksAtCenter = false
                }
            };

            // Chart background
            chartPressure.BackColor = Color.FromArgb(32, 35, 44); // Same as LiveStatus

            // ===== ВКЛЮЧАЕМ ВСТРОЕННЫЕ ФИЧИ LIVECHARTS2 =====
            
            // 1. Анимации: плавные переходы при изменении данных
            chartPressure.AnimationsSpeed = TimeSpan.FromMilliseconds(300); // 300ms для плавных переходов
            chartPressure.EasingFunction = EasingFunctions.CubicOut; // Плавная функция сглаживания

            // 2. Zoom и Pan: управляются через кнопки toolbar (не здесь)
            // ZoomMode устанавливается в InitializeToolbar() и через кнопки
            chartPressure.ZoomingSpeed = 0.1; // Скорость масштабирования (0.1 = медленнее, 1.0 = быстрее)

            // 3. Tooltips: кастомные всплывающие подсказки при наведении
            chartPressure.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Top;
            
            // Настройка стиля tooltip (темная тема)
            chartPressure.TooltipBackgroundPaint = new SolidColorPaint(new SKColor(30, 32, 38), 255);
            chartPressure.TooltipTextPaint = new SolidColorPaint(new SKColor(220, 220, 220), 255);
            chartPressure.TooltipTextSize = 12;
            
            // 4. Легенда: встроенная легенда LiveCharts2 (заменяет старую flowLegend)
            chartPressure.LegendPosition = LiveChartsCore.Measure.LegendPosition.Top; // Легенда сверху
            chartPressure.LegendBackgroundPaint = new SolidColorPaint(new SKColor(30, 32, 38), 255);
            chartPressure.LegendTextPaint = new SolidColorPaint(new SKColor(220, 220, 220), 255);
            chartPressure.LegendTextSize = 11;
            
            // Применяем кастомные tooltips к сериям с форматированием
            foreach (var series in chartPressure.Series.OfType<LineSeries<ObservablePoint>>())
            {
                // Tooltip будет показывать название серии и значение
                // Форматирование происходит автоматически через Name и Values
                // LiveCharts2 автоматически показывает название серии и значение при наведении
            }
        }

        private void ApplySmoothing()
        {
            // Удален chkSmoothing
            return;
            // if (Controls.Find("chkSmoothing", true).Length == 0 || chkSmoothing == null) return;
            // bool smooth = chkSmoothing.Checked;

            // Update LineSmoothness for current series
            // Smoothing functionality removed - chkSmoothing was deleted
            // if (chartPressure.Series != null && chartPressure.Series.Any())
            // {
            //     var currentSeries = chartPressure.Series.FirstOrDefault() as LineSeries<ObservablePoint>;
            //     if (currentSeries != null)
            //     {
            //         currentSeries.LineSmoothness = smooth ? 0.2 : 0; // 0..1 (меньше = спокойнее)
            //     }
            // }
        }

        /// <summary>
        /// Применяет сглаживание линии к существующей серии (для плавных переходов вместо "лестниц")
        /// </summary>
        private void ApplyLineSmoothing()
        {
            if (chartPressure.Series == null || !chartPressure.Series.Any()) return;

            // Находим серию "Current" (первая серия)
            var currentSeries = chartPressure.Series.FirstOrDefault() as LineSeries<ObservablePoint>;
            if (currentSeries != null)
            {
                currentSeries.LineSmoothness = 0.6; // Плавные переходы (0.0 = прямые линии, 1.0 = максимальное сглаживание)
            }
        }

        // =========================
        // Add sample (LiveCharts2) - with batching
        // =========================
        public void AddSample(double currentPressure, double? targetPressure)
        {
            if (chartPressure.IsDisposed) return;

            // Добавляем в буфер вместо прямого обновления
            lock (_updateBuffer)
            {
                _updateBuffer.Add((_timeSeconds, currentPressure, targetPressure));
            }

            // Обновляем время сразу (для следующего вызова)
            _timeSeconds += TimeStep;

            // Обновляем target value cache сразу (для быстрого доступа)
            if (targetPressure.HasValue)
            {
                if (_lastTargetValue == null || Math.Abs(_lastTargetValue.Value - targetPressure.Value) > 1e-9)
                {
                    _lastTargetValue = targetPressure.Value;
                }
            }

            // Live status обновляем сразу (не критично для производительности)
            double rate = 0;
            if (_seriesCurrent.Count > 0)
            {
                var lastPoint = _seriesCurrent[_seriesCurrent.Count - 1];
                rate = (currentPressure - lastPoint.Y.Value) / TimeStep;
            }

            string unit = _dataStore.Points.Count > 0
                ? _dataStore.Points.Last().Unit
                : "PSIG";

            UpdateLiveStatus(currentPressure, targetPressure, unit, false, rate);
        }

        // =========================
        // Reset all values to zero (called on disconnect)
        // =========================
        public void ResetToZero()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ResetToZero));
                return;
            }

            if (chartPressure.IsDisposed) return;

            // Clear all series
            _seriesCurrent.Clear();
            _seriesTarget.Clear();
            _seriesMin.Clear();
            _seriesMax.Clear();
            _cursorMarker.Clear();

            // Reset time
            _timeSeconds = 0;

            // Clear update buffer
            lock (_updateBuffer)
            {
                _updateBuffer.Clear();
            }

            // Reset target value cache
            _lastTargetValue = null;

            // Update UI with zero values
            UpdateLiveStatus(0, null, "PSIG", false, 0);
            
            // Update statistics with zero values
            CalculateAndUpdateStatistics();

            // Update target line (will be empty now)
            UpdateTargetLine();

            // Update threshold lines
            ApplyThresholdLines();
            
            // Reset Hold timer
            if (lblHoldTimer != null)
            {
                lblHoldTimer.Text = "Hold:           00:00";
            }
            if (progressBarHold != null)
            {
                progressBarHold.Value = 0;
            }
            
            // Reset threshold text boxes to 0
            if (txtMaxThreshold != null)
            {
                txtMaxThreshold.Text = "0";
            }
            if (txtMinThreshold != null)
            {
                txtMinThreshold.Text = "0";
            }
        }

        // =========================
        // Timer tick - обрабатывает накопленные точки
        // =========================
        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (chartPressure.IsDisposed) return;

            List<(double time, double pressure, double? target)> batch;
            lock (_updateBuffer)
            {
                if (_updateBuffer.Count == 0) return;
                
                batch = new List<(double time, double pressure, double? target)>(_updateBuffer);
                _updateBuffer.Clear();
            }

            // Применяем все изменения разом
            foreach (var (time, pressure, target) in batch)
            {
                _seriesCurrent.Add(new ObservablePoint(time, pressure));
            }

            // Обновляем target line если изменилось значение
            bool targetChanged = batch.Any(x => x.target.HasValue && 
                (_lastTargetValue == null || Math.Abs(_lastTargetValue.Value - x.target.Value) > 1e-9));
            
            if (targetChanged && batch.Any(x => x.target.HasValue))
            {
                var newTarget = batch.Last(x => x.target.HasValue).target;
                if (newTarget.HasValue)
                {
                    _lastTargetValue = newTarget.Value;
                }
            }

            // Применяем обновления графика один раз для всего батча
            ApplyTimeWindow(forceTrim: true);
            
            // Автоматическая очистка старых точек из графика для производительности
            // Оставляем только видимое окно + 20% буфер (~1.2 * _timeWindowSeconds)
            AutoTrimGraphSeries();
            ApplyThresholdLines();
            UpdateTargetLine();

            // Update statistics
            CalculateAndUpdateStatistics();
        }

        // =========================
        // Pagination button handlers
        // =========================
        private void InitializePaginationButtonHandlers()
        {
            // Wire up new duration button handlers (replacing old btnPage1-6)
            // btn5M -> index 0 (5 mins)
            if (btn5M != null) btn5M.Click += (s, e) => OnPaginationPageClicked(0);
            // btn15M -> index 1 (15 mins)
            if (btn15M != null) btn15M.Click += (s, e) => OnPaginationPageClicked(1);
            // btn1H -> index 2 (1 hour)
            if (btn1H != null) btn1H.Click += (s, e) => OnPaginationPageClicked(2);
            // btn4H -> index 3 (4 hours)
            if (btn4H != null) btn4H.Click += (s, e) => OnPaginationPageClicked(3);
            // btn10H -> index 4 (10 hours)
            if (btn10H != null) btn10H.Click += (s, e) => OnPaginationPageClicked(4);
            // btnALL -> index 5 (All data)
            if (btnALL != null) btnALL.Click += (s, e) => OnPaginationPageClicked(5);
            
            // Set default page (Page 1 = 5M)
            UpdatePaginationButtonStates(0);
        }
        
        private void OnPaginationPageClicked(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= PaginationData.Length) return;
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] OnPaginationPageClicked: pageIndex={pageIndex}, currentPageIndex={_currentPageIndex}, _seriesCurrent.Count={_seriesCurrent.Count}, _timeWindowSeconds={_timeWindowSeconds}");
            
            _currentPageIndex = pageIndex;
            
            // Get duration for this page
            _timeWindowSeconds = GetDurationSeconds(pageIndex);
            _gridStepXSeconds = GetAutoGridStepX(_timeWindowSeconds);
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] After change: _timeWindowSeconds={_timeWindowSeconds}, _seriesCurrent.Count={_seriesCurrent.Count}");
            
            UpdateGridStepXDisplay();
            UpdatePaginationButtonStates(pageIndex);
            
            RedrawFromStore();
            
            ApplyGridSettings();
            UpdateCustomLabelsX();
            
            // Сохраняем выбранный индекс в JSON
            SaveDurationSettings();
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] After RedrawFromStore: _seriesCurrent.Count={_seriesCurrent.Count}");
        }
        
        private void UpdatePaginationButtonStates(int selectedIndex)
        {
            // Update new duration button visual states (highlight selected)
            var buttons = new[] { btn5M, btn15M, btn1H, btn4H, btn10H, btnALL };
            for (int i = 0; i < buttons.Length && i < PaginationData.Length; i++)
            {
                if (buttons[i] != null)
                {
                    if (i == selectedIndex)
                    {
                        // Selected button - highlight (green)
                        buttons[i].BackColor = Color.FromArgb(76, 175, 80); // Green
                        buttons[i].ForeColor = Color.White;
                    }
                    else
                    {
                        // Unselected button - default (dark theme colors)
                        buttons[i].BackColor = Color.FromArgb(30, 33, 40);
                        buttons[i].ForeColor = Color.FromArgb(220, 224, 232);
                    }
                }
            }
        }

        private void RedrawFromStore()
        {
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] RedrawFromStore START: _currentPageIndex={_currentPageIndex}, _timeWindowSeconds={_timeWindowSeconds}, _seriesCurrent.Count={_seriesCurrent.Count}, RAM points={_dataStore.Points.Count}");
            
            // Очистить текущие точки
            _seriesCurrent.Clear();
            _lastTargetValue = null;
            _timeSeconds = 0;

            // Определяем, нужно ли читать из CSV (для длительностей > 1 часа или если в RAM недостаточно данных)
            bool needsCsvData = _currentPageIndex >= 3; // Индексы 3, 4, 5 = 4h, 10h, all
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] CSV check: needsCsvData={needsCsvData}, CsvPath={_dataStore.CsvPath ?? "null"}, FileExists={(_dataStore.CsvPath != null ? File.Exists(_dataStore.CsvPath) : false)}");
            
            // Для теста: если в RAM недостаточно точек для выбранного окна, загружаем из CSV
            if (!needsCsvData && _dataStore.CsvPath != null && File.Exists(_dataStore.CsvPath))
            {
                // Проверяем, хватает ли данных в RAM для выбранного окна времени
                if (_dataStore.Points.Count > 0)
                {
                    double maxTimeInRam = _dataStore.Points.Max(p => p.ElapsedSeconds);
                    System.Diagnostics.Debug.WriteLine($"[ChartWindow] RAM data check: maxTimeInRam={maxTimeInRam}, _timeWindowSeconds={_timeWindowSeconds}, Points.Count={_dataStore.Points.Count}");
                    // Если максимальное время в RAM меньше выбранного окна, загружаем из CSV
                    if (maxTimeInRam < _timeWindowSeconds)
                    {
                        needsCsvData = true;
                        System.Diagnostics.Debug.WriteLine($"[ChartWindow] RAM insufficient: maxTimeInRam={maxTimeInRam}, _timeWindowSeconds={_timeWindowSeconds}, switching to CSV");
                    }
                }
            }
            
            IEnumerable<DataPointModel> pointsToUse;
            
            if (needsCsvData && _dataStore.CsvPath != null && File.Exists(_dataStore.CsvPath))
            {
                // Читаем данные из CSV (для длительностей > 1 часа или если в RAM недостаточно данных)
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] Loading from CSV: {_dataStore.CsvPath}");
                pointsToUse = LoadDataFromCsv(_dataStore.CsvPath, _dataStore.SessionStart);
            }
            else
            {
                // Используем данные из RAM (для длительностей <= 1 часа и если данных достаточно)
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] Using RAM data: {_dataStore.Points.Count} points");
                pointsToUse = _dataStore.Points;
            }

            int pointsAdded = 0;
            // Перерисовать из выбранного источника
            foreach (var point in pointsToUse)
            {
                _seriesCurrent.Add(new ObservablePoint(point.ElapsedSeconds, point.Current));
                pointsAdded++;

                if (point.Target > 0)
                {
                    _lastTargetValue = point.Target;
                }

                _timeSeconds = point.ElapsedSeconds + TimeStep;
            }
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] RedrawFromStore END: pointsAdded={pointsAdded}, _seriesCurrent.Count={_seriesCurrent.Count}, _timeSeconds={_timeSeconds}");

            // Применить окно времени БЕЗ трима
            ApplyTimeWindow(forceTrim: false);
            
            // Применяем сглаживание линии
            ApplyLineSmoothing();
            
            ApplyThresholdLines();
            UpdateTargetLine();
        }
        
        /// <summary>
        /// Загружает данные из CSV файла с фильтрацией по времени для выбранного окна.
        /// Для окон "5 мин", "15 мин", "1 час", "4 часа", "10 часов" загружает только последние N секунд от конца сессии.
        /// Для "all" загружает все данные из CSV.
        /// </summary>
        private List<DataPointModel> LoadDataFromCsv(string csvFilePath, DateTime sessionStart)
        {
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv START: csvFilePath={csvFilePath}, _currentPageIndex={_currentPageIndex}, _timeWindowSeconds={_timeWindowSeconds}");
            
            var points = new List<DataPointModel>();
            
            if (!File.Exists(csvFilePath))
            {
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv: File not found");
                return points;
            }

            try
            {
                // Используем FileShare.ReadWrite для чтения файла, который может быть открыт для записи
                var lines = new List<string>();
                using (var fileStream = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine() ?? string.Empty);
                    }
                }
                
                if (lines.Count < 2)
                    return points;

                // Определяем формат файла по заголовку
                bool isNewFormat = lines[0].StartsWith("RowNumber,Timestamp", StringComparison.OrdinalIgnoreCase);
                int timestampIndex = isNewFormat ? 1 : 0;
                int timeIndex = isNewFormat ? 2 : 1;
                int currentIndex = isNewFormat ? 3 : 2;
                int targetIndex = isNewFormat ? 4 : 3;
                int unitIndex = isNewFormat ? 5 : 4;
                int rampSpeedIndex = isNewFormat ? 6 : 5;
                int pollingFreqIndex = isNewFormat ? 7 : 6;
                int eventIndex = isNewFormat ? 9 : 7;

                // Определяем диапазон времени для загрузки на основе выбранного окна
                double minTimeToLoad = 0;
                double maxTimeToLoad = double.MaxValue;
                
                // Сначала проходим по файлу один раз, чтобы найти maxTimeInFile и определить диапазон
                double maxTimeInFile = 0;
                for (int i = 1; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    var parts = lines[i].Split(',');
                    if (parts.Length > timeIndex && 
                        double.TryParse(parts[timeIndex], System.Globalization.NumberStyles.Float, 
                            System.Globalization.CultureInfo.InvariantCulture, out double elapsed))
                    {
                        if (elapsed > maxTimeInFile) maxTimeInFile = elapsed;
                    }
                }

                // Используем PaginationData для определения длительности окна (избегаем дублирования констант)
                if (_currentPageIndex >= 0 && _currentPageIndex < PaginationData.Length)
                {
                    var paginationData = PaginationData[_currentPageIndex];
                    int windowSeconds = paginationData.Seconds;
                    
                    // Для "all" (windowSeconds == -1) загружаем все данные
                    if (windowSeconds > 0)
                    {
                        // Загружаем последние N секунд от конца сессии
                        minTimeToLoad = Math.Max(0, maxTimeInFile - windowSeconds);
                        maxTimeToLoad = maxTimeInFile;
                    }
                    // Для "all" (windowSeconds == -1) minTimeToLoad = 0, maxTimeToLoad = MaxValue (уже установлено выше)
                }
                
                for (int i = 1; i < lines.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;

                    var parts = lines[i].Split(',');
                    if (parts.Length < currentIndex + 1) continue;

                    try
                    {
                        double elapsed = parts.Length > timeIndex && 
                            double.TryParse(parts[timeIndex], System.Globalization.NumberStyles.Float, 
                                System.Globalization.CultureInfo.InvariantCulture, out double e) ? e : 0;
                        
                        // Фильтруем по времени (для 4h и 10h загружаем только последние N часов)
                        if (elapsed < minTimeToLoad || elapsed > maxTimeToLoad) continue;

                        string timestampStr = parts[timestampIndex];
                        if (DateTime.TryParse(timestampStr, out DateTime timestamp))
                        {
                            double current = parts.Length > currentIndex && 
                                double.TryParse(parts[currentIndex], System.Globalization.NumberStyles.Float, 
                                    System.Globalization.CultureInfo.InvariantCulture, out double c) ? c : 0;
                            double target = parts.Length > targetIndex && 
                                double.TryParse(parts[targetIndex], System.Globalization.NumberStyles.Float, 
                                    System.Globalization.CultureInfo.InvariantCulture, out double t) ? t : 0;
                            string unit = parts.Length > unitIndex ? parts[unitIndex] : "PSIG";
                            double rampSpeed = parts.Length > rampSpeedIndex && 
                                double.TryParse(parts[rampSpeedIndex], System.Globalization.NumberStyles.Float, 
                                    System.Globalization.CultureInfo.InvariantCulture, out double rs) ? rs : 0;
                            int pollingFreq = parts.Length > pollingFreqIndex && 
                                int.TryParse(parts[pollingFreqIndex], out int pf) ? pf : 500;
                            string? eventType = parts.Length > eventIndex && 
                                !string.IsNullOrWhiteSpace(parts[eventIndex]) ? parts[eventIndex] : null;

                            var point = new DataPointModel(timestamp, elapsed, current, target, unit, rampSpeed, pollingFreq, eventType);
                            points.Add(point);
                        }
                    }
                    catch (Exception parseEx)
                    {
                        // Логируем ошибки парсинга для отладки (только некритичные - пропускаем строку)
                        System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv: Error parsing line {i}: {parseEx.Message}");
                        continue;
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv: Loaded {points.Count} points (minTimeToLoad={minTimeToLoad}, maxTimeToLoad={maxTimeToLoad}, maxTimeInFile={maxTimeInFile})");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv ERROR StackTrace: {ex.StackTrace}");
            }

            System.Diagnostics.Debug.WriteLine($"[ChartWindow] LoadDataFromCsv END: returning {points.Count} points");
            return points;
        }

        // =========================
        // Time window + trim (LiveCharts2)
        // =========================
        private void ApplyTimeWindow(bool forceTrim)
        {
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] ApplyTimeWindow START: forceTrim={forceTrim}, _currentPageIndex={_currentPageIndex}, _timeWindowSeconds={_timeWindowSeconds}, _timeSeconds={_timeSeconds}, _seriesCurrent.Count={_seriesCurrent.Count}");
            
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            var xAxis = chartPressure.XAxes.FirstOrDefault();
            if (xAxis == null) return;

            // Check if "all data" page is selected
            bool isAllDataPage = IsAllDataPage(_currentPageIndex);
            
            double xMax = _timeSeconds;
            double xMin = 0;
            
            if (isAllDataPage)
            {
                // "All data" page - show all data from store or CSV
                // Определяем источник данных
                IEnumerable<DataPointModel> dataSource;
                if (_dataStore.CsvPath != null && File.Exists(_dataStore.CsvPath))
                {
                    dataSource = LoadDataFromCsv(_dataStore.CsvPath, _dataStore.SessionStart);
                }
                else
                {
                    dataSource = _dataStore.Points;
                }
                
                var dataList = dataSource.ToList();
                if (dataList.Count > 0)
                {
                    xMax = dataList.Max(p => p.ElapsedSeconds);
                    xMin = 0;
                }
                else
                {
                    xMax = _timeSeconds;
                    xMin = 0;
                }
                
                // No limits on axis for "all data"
                xAxis.MinLimit = null;
                xAxis.MaxLimit = null;
            }
            else
            {
                // Regular page - apply time window
                xMin = Math.Max(0, xMax - _timeWindowSeconds);

                // Update axis limits
                if (_timeSeconds <= _timeWindowSeconds)
                {
                    xAxis.MinLimit = 0;
                    xAxis.MaxLimit = _timeWindowSeconds;
                }
                else
                {
                    xAxis.MinLimit = xMin;
                    xAxis.MaxLimit = xMax;
                }
            }

            UpdateCustomLabelsX();

            // Для исторических данных (read-only режим) не обрезаем точки
            // Только изменяем видимое окно через MinLimit/MaxLimit
            if (!_dataStore.IsRunning)
            {
                // Исторические данные - не обрезаем, только меняем видимое окно
                return;
            }

            if (!forceTrim)
            {
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] ApplyTimeWindow END: forceTrim=false, no trim");
                return;
            }
            
            // For "all data" page, don't trim
            if (isAllDataPage)
            {
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] ApplyTimeWindow END: isAllDataPage=true, no trim");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[ChartWindow] ApplyTimeWindow: Calling TrimSeriesByX with xMin={xMin}, _seriesCurrent.Count={_seriesCurrent.Count}");
            // Trim old points from current series (только для активных сессий)
            TrimSeriesByX(_seriesCurrent, xMin);

            // после трима индекс курсора может стать неверным
            _lastNearestIndex = -1;
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] ApplyTimeWindow END: after trim _seriesCurrent.Count={_seriesCurrent.Count}");
        }

        private static void TrimSeriesByX(ObservableCollection<ObservablePoint> series, double xMin)
        {
            if (series.Count == 0) return;
            
            int countBefore = series.Count;
            
            // Найти индекс первой точки >= xMin
            int firstKeepIndex = -1;
            for (int i = 0; i < series.Count; i++)
            {
                if (series[i].X.Value >= xMin)
                {
                    firstKeepIndex = i;
                    break;
                }
            }
            
            // Если все точки нужно удалить
            if (firstKeepIndex < 0)
            {
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] TrimSeriesByX: Removing ALL points (xMin={xMin}, count={series.Count})");
                series.Clear();
                return;
            }
            
            // Если первая точка уже >= xMin, ничего не делаем
            if (firstKeepIndex == 0)
            {
                System.Diagnostics.Debug.WriteLine($"[ChartWindow] TrimSeriesByX: No trim needed (xMin={xMin}, first point X={series[0].X.Value})");
                return;
            }
            
            // Более эффективный способ: создаем список элементов для сохранения
            // и пересоздаем коллекцию (O(n) вместо O(k*n) для k удалений)
            var itemsToKeep = new List<ObservablePoint>();
            for (int i = firstKeepIndex; i < series.Count; i++)
            {
                itemsToKeep.Add(series[i]);
            }
            
            int removed = series.Count - itemsToKeep.Count;
            series.Clear();
            foreach (var item in itemsToKeep)
            {
                series.Add(item);
            }
            
            System.Diagnostics.Debug.WriteLine($"[ChartWindow] TrimSeriesByX: Removed {removed} points (xMin={xMin}, before={countBefore}, after={series.Count}, firstKeepIndex={firstKeepIndex})");
        }

        // =========================
        // Auto-trim graph series (Performance optimization)
        // =========================
        /// <summary>
        /// Автоматически удаляет старые точки из графика для производительности.
        /// Оставляет только видимое окно + 20% буфер (~1.2 * _timeWindowSeconds).
        /// Это предотвращает накопление памяти при длительных тестах.
        /// </summary>
        private void AutoTrimGraphSeries()
        {
            if (_seriesCurrent.Count == 0) return;
            
            // Для исторических данных (read-only режим) не обрезаем точки
            // Все данные должны оставаться доступными для просмотра
            if (!_dataStore.IsRunning)
            {
                return;
            }
            
            // Определяем максимальный X (время) для удержания
            // Видимое окно + 20% буфер (~1.2 * _timeWindowSeconds)
            double maxTimeToKeep = _timeSeconds;
            double minTimeToKeep = Math.Max(0, maxTimeToKeep - (_timeWindowSeconds * 1.2));
            
            // Если графика еще мало, не трогаем
            if (_seriesCurrent.Count < 100) return;
            
            // Находим первую точку, которую нужно сохранить
            int firstKeepIndex = -1;
            for (int i = 0; i < _seriesCurrent.Count; i++)
            {
                if (_seriesCurrent[i].X.Value >= minTimeToKeep)
                {
                    firstKeepIndex = i;
                    break;
                }
            }
            
            // Если все точки в диапазоне, ничего не делаем
            if (firstKeepIndex <= 0) return;
            
            // Эффективное удаление всех старых точек сразу (O(n) вместо O(k*n) для k удалений)
            // Создаем новый список только с нужными точками
            var itemsToKeep = new List<ObservablePoint>(_seriesCurrent.Count - firstKeepIndex);
            for (int i = firstKeepIndex; i < _seriesCurrent.Count; i++)
            {
                itemsToKeep.Add(_seriesCurrent[i]);
            }
            
            // Очищаем и перезаполняем коллекцию
            _seriesCurrent.Clear();
            foreach (var item in itemsToKeep)
            {
                _seriesCurrent.Add(item);
            }
        }

        // =========================
        // Grid settings (LiveCharts2)
        // =========================
        private void ApplyGridSettings()
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Get grid visibility from checkbox
            bool showGrid = chkShowGrid?.Checked ?? true;

            // Update grid visibility
            if (showGrid)
            {
                var gridColor = new SKColor(100, 105, 115); // Более светлый цвет для лучшей видимости
                if (xAxis is Axis xAxisConcrete)
                    xAxisConcrete.SeparatorsPaint = new SolidColorPaint(gridColor);
                if (yAxis is Axis yAxisConcrete)
                    yAxisConcrete.SeparatorsPaint = new SolidColorPaint(gridColor);
            }
            else
            {
                if (xAxis is Axis xAxisConcrete)
                    xAxisConcrete.SeparatorsPaint = null;
                if (yAxis is Axis yAxisConcrete)
                    yAxisConcrete.SeparatorsPaint = null;
            }

            UpdateCustomLabelsX();
        }

        // =========================
        // Thresholds (LiveCharts2)
        // =========================
        private void Thresholds_TextChanged(object? sender, EventArgs e)
        {
            // Save thresholds to settings
            SaveThresholdsToSettings();
            // Apply threshold lines to chart
            ApplyThresholdLines();
        }
        
        // Y Step dropdown handler removed - LiveCharts2 automatically manages Y axis grid step
        
        // =========================
        // Show Grid checkbox handler
        // =========================
        private void ChkShowGrid_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkShowGrid?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkShowGrid_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            ApplyGridSettings();
            SaveDisplaySettings(); // Save Display section to JSON
        }
        
        // =========================
        // Show Target checkbox handler
        // =========================
        private void ChkShowTarget_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkShowTarget?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkShowTarget_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            UpdateSeriesVisibility();
            SaveDisplaySettings(); // Save Display section to JSON
        }
        
        // =========================
        // Show Max checkbox handler
        // =========================
        private void ChkShowMax_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkShowMax?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkShowMax_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            UpdateSeriesVisibility();
            SaveDisplaySettings(); // Save Display section to JSON
        }
        
        // =========================
        // Show Min checkbox handler
        // =========================
        private void ChkShowMin_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkShowMin?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkShowMin_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            UpdateSeriesVisibility();
            SaveDisplaySettings(); // Save Display section to JSON
        }
        
        // =========================
        // Update series visibility based on checkboxes
        // =========================
        private void UpdateSeriesVisibility()
        {
            if (_lineSeriesTarget != null)
            {
                _lineSeriesTarget.IsVisible = chkShowTarget?.Checked ?? true;
            }
            
            if (_lineSeriesMax != null)
            {
                _lineSeriesMax.IsVisible = chkShowMax?.Checked ?? true;
            }
            
            if (_lineSeriesMin != null)
            {
                _lineSeriesMin.IsVisible = chkShowMin?.Checked ?? true;
            }
        }
        
        // =========================
        // Sound alerts handlers
        // =========================
        private void ChkSound_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkSound?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkSound_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            // Sound alerts enabled/disabled
            // This will be used by alert logic
        }
        
        private void ChkAtTarget_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkAtTarget?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkAtTarget_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            // Alert when at target enabled/disabled
        }
        
        private void ChkAtMax_CheckedChanged(object? sender, EventArgs e)
        {
            bool newValue = chkAtMax?.Checked ?? false;
            System.Diagnostics.Debug.WriteLine($"[ChkAtMax_CheckedChanged] Checked changed to: {newValue}, sender: {sender?.GetType().Name ?? "null"}");
            // Alert when at max enabled/disabled
        }

        /// <summary>
        /// Загружает значения thresholds из настроек приложения
        /// </summary>
        private void LoadThresholdsFromSettings()
        {
            // Load from AppOptions.Current (single source of truth)
            var maxPressure = OptionsWindow.AppOptions.Current.MaxPressure ?? 0;
            var minPressure = OptionsWindow.AppOptions.Current.MinPressure ?? 0;
            
            // Update NumericUpDown controls (primary controls)
            if (numMaxThreshold != null)
            {
                numMaxThreshold.Value = (decimal)maxPressure;
            }
            
            if (numMinThreshold != null)
            {
                numMinThreshold.Value = (decimal)minPressure;
            }
            
            // Update TextBox controls (for backward compatibility)
            if (txtMaxThreshold != null)
            {
                txtMaxThreshold.Text = maxPressure.ToString("F0");
            }
            
            if (txtMinThreshold != null)
            {
                txtMinThreshold.Text = minPressure.ToString("F0");
            }
        }

        /// <summary>
        /// Сохраняет значения thresholds в настройки приложения
        /// </summary>
        private void SaveThresholdsToSettings()
        {
            var settings = OptionsWindow.AppOptions.Current.Clone();
            
            // Use NumericUpDown values (primary controls)
            if (numMaxThreshold != null)
            {
                settings.MaxPressure = (double)numMaxThreshold.Value;
            }
            else if (txtMaxThreshold != null && double.TryParse(txtMaxThreshold.Text, out double maxVal))
            {
                settings.MaxPressure = maxVal;
            }
            
            if (numMinThreshold != null)
            {
                settings.MinPressure = (double)numMinThreshold.Value;
            }
            else if (txtMinThreshold != null && double.TryParse(txtMinThreshold.Text, out double minVal))
            {
                settings.MinPressure = minVal;
            }
            
            OptionsWindow.AppOptions.Current = settings;
            _onThresholdsChanged?.Invoke();
        }

        /// <summary>
        /// Устанавливает делегат, который вызывается при изменении thresholds
        /// (для автоматического сохранения настроек, если auto-save включен)
        /// </summary>
        public void SetThresholdsChangedHandler(Action handler)
        {
            _onThresholdsChanged = handler;
        }

        /// <summary>
        /// Обновляет значения thresholds из настроек приложения
        /// (вызывается при изменении настроек в Preferences)
        /// </summary>
        public void RefreshThresholdsFromSettings()
        {
            LoadThresholdsFromSettings();
            ApplyThresholdLines();
        }

        /// <summary>
        /// Обновляет thresholds из настроек (вызывается извне при изменении настроек)
        /// </summary>
        public void UpdateThresholdsFromSettings()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateThresholdsFromSettings));
                return;
            }

            LoadThresholdsFromSettings();
            ApplyThresholdLines();
        }

        private void ApplyThresholdLines()
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            var xAxis = chartPressure.XAxes.FirstOrDefault();
            if (xAxis == null) return;

            double x1 = 0; // Always start from 0
            double x2;
            
            // Check if "ALL" is selected (both limits are null)
            if (xAxis.MinLimit == null && xAxis.MaxLimit == null)
            {
                // "ALL" mode - use maximum time from all data in store
                // This ensures lines extend to the full right edge of the graph
                if (_dataStore.Points.Count > 0)
                {
                    x2 = _dataStore.Points.Max(p => p.ElapsedSeconds);
                }
                else if (_timeSeconds > 0)
                {
                    x2 = _timeSeconds;
                }
                else
                {
                    x2 = _timeWindowSeconds;
                }
            }
            else
            {
                // Regular mode - use axis MaxLimit (right edge of visible graph)
                // If MaxLimit is null, use time window or current time
                x2 = xAxis.MaxLimit ?? _timeWindowSeconds;
                
                // Ensure x2 is valid
                if (x2 <= 0)
                {
                    x2 = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds;
                }
            }

            // Get values from TextBox controls
            double maxVal = 0; // Default value
            double minVal = 0; // Default value
            
            if (txtMaxThreshold != null && double.TryParse(txtMaxThreshold.Text, out double parsedMax))
            {
                maxVal = parsedMax;
            }
            
            if (txtMinThreshold != null && double.TryParse(txtMinThreshold.Text, out double parsedMin))
            {
                minVal = parsedMin;
            }

            _seriesMax.Clear();
            _seriesMax.Add(new ObservablePoint(x1, maxVal));
            _seriesMax.Add(new ObservablePoint(x2, maxVal));

            _seriesMin.Clear();
            _seriesMin.Add(new ObservablePoint(x1, minVal));
            _seriesMin.Add(new ObservablePoint(x2, minVal));
        }

        // =========================
        // Target flat line (2 points) (LiveCharts2)
        // =========================
        private void UpdateTargetLine()
        {
            if (_lastTargetValue == null) return;
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            if (xAxis == null) return;

            double x1 = 0; // Always start from 0
            double x2;
            
            // Check if "ALL" is selected (both limits are null)
            if (xAxis.MinLimit == null && xAxis.MaxLimit == null)
            {
                // "ALL" mode - use maximum time from all data in store
                // This ensures lines extend to the full right edge of the graph
                if (_dataStore.Points.Count > 0)
                {
                    x2 = _dataStore.Points.Max(p => p.ElapsedSeconds);
                }
                else if (_timeSeconds > 0)
                {
                    x2 = _timeSeconds;
                }
                else
                {
                    x2 = _timeWindowSeconds;
                }
            }
            else
            {
                // Regular mode - use axis MaxLimit (right edge of visible graph)
                // If MaxLimit is null, use time window or current time
                x2 = xAxis.MaxLimit ?? _timeWindowSeconds;
                
                // Ensure x2 is valid
                if (x2 <= 0)
                {
                    x2 = _timeSeconds > 0 ? _timeSeconds : _timeWindowSeconds;
                }
            }

            _seriesTarget.Clear();
            _seriesTarget.Add(new ObservablePoint(x1, _lastTargetValue.Value));
            _seriesTarget.Add(new ObservablePoint(x2, _lastTargetValue.Value));
        }

        // =========================
        // Reset zoom (LiveCharts2)
        // =========================
        private void ChartPressure_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            if (chartPressure.YAxes == null || !chartPressure.YAxes.Any()) return;

            var xAxis = chartPressure.XAxes.FirstOrDefault();
            var yAxis = chartPressure.YAxes.FirstOrDefault();
            if (xAxis == null || yAxis == null) return;

            // Reset zoom by resetting limits
            ApplyTimeWindow(forceTrim: false);
            yAxis.MinLimit = null;
            yAxis.MaxLimit = null;

            ApplyThresholdLines();
            UpdateTargetLine();
            UpdateCustomLabelsX();
        }

        // =========================
        // Cursor fast search (binary) (LiveCharts2)
        // =========================
        private int FindNearestIndexByX(ObservableCollection<ObservablePoint> series, double x)
        {
            int n = series.Count;
            if (n <= 0) return -1;

            // quick edges
            double x0 = series[0].X.Value;
            double xN = series[n - 1].X.Value;
            if (x <= x0) return 0;
            if (x >= xN) return n - 1;

            int lo = 0;
            int hi = n - 1;

            while (hi - lo > 1)
            {
                int mid = (lo + hi) >> 1;
                double xm = series[mid].X.Value;
                if (xm < x) lo = mid;
                else hi = mid;
            }

            // now lo and hi are neighbors around x
            double dlo = Math.Abs(series[lo].X.Value - x);
            double dhi = Math.Abs(series[hi].X.Value - x);
            return (dhi < dlo) ? hi : lo;
        }

        // =========================
        // Mouse leave (LiveCharts2)
        // =========================
        private void ChartPressure_MouseLeave(object? sender, EventArgs e)
        {
            _cursorMarker.Clear();
            if (chartPressure.Series != null && chartPressure.Series.Count() > 4)
            {
                var cursorSeries = chartPressure.Series.Skip(4).FirstOrDefault() as ScatterSeries<ObservablePoint>;
                if (cursorSeries != null)
                    cursorSeries.IsVisible = false;
            }
            _cursorInfoPanel.Visible = false;
            _lastNearestIndex = -1;
        }

        // =========================
        // Mouse down
        // =========================
        private void ChartPressure_MouseDown(object? sender, MouseEventArgs e)
        {
            // LiveCharts2 handles zoom/pan automatically
        }

        // =========================
        // Mouse up
        // =========================
        private void ChartPressure_MouseUp(object? sender, MouseEventArgs e)
        {
            // LiveCharts2 handles zoom/pan automatically
        }

        // =========================
        // Mouse move (fast + throttled) (LiveCharts2)
        // =========================
        private void ChartPressure_MouseMove(object? sender, MouseEventArgs e)
        {

            // throttle for cursor tracking
            long now = Environment.TickCount64;
            if (now - _lastCursorTick < CursorThrottleMs) return;
            _lastCursorTick = now;

            if (_seriesCurrent.Count == 0) return;

            // Convert mouse position to chart coordinates
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            var xAxis = chartPressure.XAxes.FirstOrDefault();
            if (xAxis == null) return;

            // Simple conversion (may need adjustment based on chart size)
            double xValue = (e.X / (double)chartPressure.Width) * (_timeWindowSeconds) + (xAxis.MinLimit ?? 0);

            int idx = FindNearestIndexByX(_seriesCurrent, xValue);
            if (idx < 0) return;

            // panel always follows mouse fast
            int px = e.X + 15;
            int py = e.Y + 15;

            if (px + _cursorInfoPanel.Width > chartPressure.Width - 10)
                px = e.X - _cursorInfoPanel.Width - 15;
            if (py + _cursorInfoPanel.Height > chartPressure.Height - 10)
                py = e.Y - _cursorInfoPanel.Height - 15;

            if (px < 5) px = 5;
            if (py < 5) py = 5;

            _cursorInfoPanel.Location = new Point(px, py);

            // update data only if index changed (less redraw)
            if (idx != _lastNearestIndex)
            {
                _lastNearestIndex = idx;

                var p = _seriesCurrent[idx];
                double tSec = p.X.Value;
                double pressure = p.Y.Value;

                double? target = _lastTargetValue;

                double rate = 0;
                if (idx > 0)
                {
                    double prev = _seriesCurrent[idx - 1].Y.Value;
                    rate = (pressure - prev) / TimeStep;
                }

                _cursorMarker.Clear();
                _cursorMarker.Add(new ObservablePoint(p.X.Value, pressure));

                if (chartPressure.Series != null && chartPressure.Series.Count() > 4)
                {
                    var cursorSeries = chartPressure.Series.Skip(4).FirstOrDefault() as ScatterSeries<ObservablePoint>;
                    if (cursorSeries != null)
                    {
                        cursorSeries.IsVisible = true;
                    }
                }

                int totalSec = (int)Math.Round(tSec);
                int mins = totalSec / 60;
                int secs = totalSec % 60;

                _lblInfoTime.Text = $"Time:     {mins}:{secs:D2}";
                _lblInfoPressure.Text = $"Pressure: {pressure:F2}";
                _lblInfoTarget.Text = $"Target:   {(target.HasValue ? target.Value.ToString("F2") : "-")}";
                _lblInfoRate.Text = $"Δ Rate:   {rate:+0.00;-0.00;0}/s";
            }

            _cursorInfoPanel.Visible = true;
        }

        // =========================
        // SessionDataStore integration (LiveCharts2)
        // =========================
        private void LoadHistoryFromStore()
        {
            ReloadDataFromStore();
        }

        /// <summary>
        /// Публичный метод для перезагрузки данных из DataStore (используется при загрузке сессии)
        /// </summary>
        public void ReloadDataFromStore()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ReloadDataFromStore));
                return;
            }

            // Очищаем существующие данные
            _seriesCurrent.Clear();
            _seriesTarget.Clear();
            _timeSeconds = 0;
            _lastTargetValue = null;

            // Определяем, нужно ли читать из CSV (для длительностей > 1 часа или если в RAM недостаточно данных)
            bool needsCsvData = _currentPageIndex >= 3; // Индексы 3, 4, 5 = 4h, 10h, all
            
            // Для теста: если в RAM недостаточно точек для выбранного окна, загружаем из CSV
            if (!needsCsvData && _dataStore.CsvPath != null && File.Exists(_dataStore.CsvPath))
            {
                // Проверяем, хватает ли данных в RAM для выбранного окна времени
                if (_dataStore.Points.Count > 0)
                {
                    double maxTimeInRam = _dataStore.Points.Max(p => p.ElapsedSeconds);
                    // Если максимальное время в RAM меньше выбранного окна, загружаем из CSV
                    if (maxTimeInRam < _timeWindowSeconds)
                    {
                        needsCsvData = true;
                    }
                }
            }
            
            IEnumerable<DataPointModel> pointsToUse;
            
            if (needsCsvData && _dataStore.CsvPath != null && File.Exists(_dataStore.CsvPath))
            {
                // Читаем данные из CSV (для длительностей > 1 часа или если в RAM недостаточно данных)
                pointsToUse = LoadDataFromCsv(_dataStore.CsvPath, _dataStore.SessionStart);
            }
            else
            {
                // Используем данные из RAM (для длительностей <= 1 часа и если данных достаточно)
                pointsToUse = _dataStore.Points;
            }

            // Загружаем все точки из выбранного источника
            foreach (var point in pointsToUse)
            {
                _seriesCurrent.Add(new ObservablePoint(point.ElapsedSeconds, point.Current));

                // Сохраняем последнее значение target для отображения линии
                if (point.Target > 0)
                {
                    _lastTargetValue = point.Target;
                }

                // Обновляем время до последней точки
                _timeSeconds = point.ElapsedSeconds + TimeStep;
            }

            // Обновляем target линию (использует _lastTargetValue)
            UpdateTargetLine();

            // Применяем настройки окна времени
            ApplyTimeWindow(forceTrim: true);
            
            // Обновляем сглаживание линии (применяем к существующей серии)
            ApplyLineSmoothing();
            
            // Обновляем линии порогов
            ApplyThresholdLines();
            
            // Обновляем live status панель
            var pointsList = pointsToUse.ToList();
            if (pointsList.Count > 0)
            {
                var lastPoint = pointsList.Last();
                double rate = 0;
                if (pointsList.Count > 1)
                {
                    var prevPoint = pointsList[pointsList.Count - 2];
                    var timeDiff = lastPoint.ElapsedSeconds - prevPoint.ElapsedSeconds;
                    if (timeDiff > 0)
                    {
                        rate = (lastPoint.Current - prevPoint.Current) / timeDiff;
                    }
                }
                UpdateLiveStatus(lastPoint.Current, lastPoint.Target > 0 ? lastPoint.Target : null, lastPoint.Unit, false, rate);
            }

            // Принудительно обновляем график
            if (chartPressure != null && !chartPressure.IsDisposed)
            {
                chartPressure.Invalidate();
            }
        }

        private void OnNewPointReceived(DataPointModel point)
        {
            if (IsDisposed) return;

            // Вызываем в UI потоке
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnNewPointReceived(point)));
                return;
            }

            AddSample(point.Current, point.Target > 0 ? point.Target : null);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Остановить и освободить таймер
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
                _updateTimer.Dispose();
                _updateTimer = null;
            }

            // Обработать оставшиеся точки в буфере перед закрытием
            if (_updateBuffer.Count > 0)
            {
                UpdateTimer_Tick(null, EventArgs.Empty);
            }

            // Отписаться от событий
            _dataStore.OnNewPoint -= OnNewPointReceived;
            base.OnFormClosing(e);
        }

        // ===== designer stubs =====
        private void panelEmergencyHost_Paint(object sender, PaintEventArgs e) { }
        private void lblTimeWindowTitle_Click(object sender, EventArgs e) { }
        private void lblXStep_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label3_Click_1(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        private void lblCurrentPressureLarge_Click(object sender, EventArgs e)
        {

        }

        private void lblCurrentUnit_Click(object sender, EventArgs e)
        {

        }

        // Emergency Vent handler delegate

        /// <summary>
        /// Устанавливает сервис последовательностей (реализация IGraphView)
        /// </summary>
        void IGraphView.SetSequenceService(object? sequenceService)
        {
            // Метод SetSequenceServiceInternal определен в ChartWindow.GoToTarget.cs
            // Он принимает SequenceService и устанавливает его в _sequenceService
            if (sequenceService is PrecisionPressureController.Services.Sequence.SequenceService seqService)
            {
                // Вызываем метод из ChartWindow.GoToTarget.cs (partial class)
                SetSequenceServiceInternal(seqService);
            }
        }

        /// <summary>
        /// Явная реализация IGraphView.Focus() - интерфейс требует void, а Control.Focus() возвращает bool
        /// </summary>
        void IGraphView.Focus()
        {
            base.Focus();
        }

        // BtnFullscreen_Click removed - replaced with toolbar icon

        private void PnlLiveStatus_Paint(object? sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // Draw border around LIVE STATUS panel (опционально, если нужна рамка)
            // using var pen = new Pen(_isDarkTheme ? Color.FromArgb(60, 65, 75) : Color.FromArgb(200, 200, 210), 1);
            // var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            // e.Graphics.DrawRectangle(pen, rect);
        }

        // btnGoTarget_Click is implemented in ChartWindow.HeaderFooter.cs
        
        private Action? _emergencyVentHandler;

        /// <summary>
        /// Устанавливает обработчик для кнопки Emergency Vent
        /// </summary>
        public void SetEmergencyVentHandler(Action handler)
        {
            _emergencyVentHandler = handler;
        }

        private void BtnEmergency_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Emergency Vent will release all pressure immediately.\n\nThis action cannot be undone. Continue?",
                "Emergency Vent",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _emergencyVentHandler?.Invoke();
            }
        }
        
        private void ChartWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            // LiveCharts2 handles zoom/pan automatically - no custom handling needed
        }
    }
}
