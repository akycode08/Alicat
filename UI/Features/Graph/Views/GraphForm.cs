using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using Alicat.Services.Data;
using DataPointModel = Alicat.Services.Data.DataPoint;
using Alicat;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm : Form
    {
        private readonly SessionDataStore _dataStore;
        // --- series (LiveCharts2) ---
        private ObservableCollection<ObservablePoint> _seriesCurrent = null!;
        private ObservableCollection<ObservablePoint> _seriesTarget = null!;
        private ObservableCollection<ObservablePoint> _seriesMin = null!;
        private ObservableCollection<ObservablePoint> _seriesMax = null!;

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

        // Duration data: (Name, Seconds, GridStepXSeconds)
        private static readonly (string Name, int Seconds, int GridStep)[] DurationData = new[]
        {
            ("1 min",    60,     10),
            ("5 min",    300,    30),
            ("15 min",   900,    60),
            ("30 min",   1800,   120),
            ("1 hour",   3600,   600),
            ("2 hours",  7200,   900),
            ("4 hours",  14400,  1800),
            ("6 hours",  21600,  1800),
            ("8 hours",  28800,  1800),
            ("10 hours", 36000,  3600)
        };

        public GraphForm(SessionDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeComponent();
            ConfigureChart();
            ComboBoxValues();
            CreateCursorInfoPanel();

            // handlers (duration / grid / thresholds)
            cmbDuration.SelectedIndexChanged += CmbDuration_SelectedIndexChanged;
            cmbYStep.SelectedIndexChanged += (_, __) => ApplyGridSettings();
            chkShowGrid.CheckedChanged += (_, __) => ApplyGridSettings();

            // smoothing (если чекбокс есть на форме)
            // если у тебя chkSmoothing называется иначе — поменяй имя здесь
            if (Controls.Find("chkSmoothing", true).Length > 0 && chkSmoothing != null)
            {
                chkSmoothing.CheckedChanged += (_, __) => ApplySmoothing();
            }

            nudMaximum.ValueChanged += Thresholds_ValueChanged;
            numericUpDown2.ValueChanged += Thresholds_ValueChanged;

            // Load thresholds from settings
            LoadThresholdsFromSettings();

            // default duration
            _timeWindowSeconds = GetDurationSeconds(0);
            _gridStepXSeconds = GetAutoGridStepX(_timeWindowSeconds);
            UpdateGridStepXDisplay();

            ApplyGridSettings();
            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            UpdateTargetLine();        // <- target flat line
            UpdateCustomLabelsX();     // <- X labels

            // cursor events
            chartPressure.MouseDoubleClick += ChartPressure_MouseDoubleClick;
            chartPressure.MouseMove += ChartPressure_MouseMove;
            chartPressure.MouseLeave += ChartPressure_MouseLeave;

            ApplySmoothing();

            // Загрузить историю из Store
            LoadHistoryFromStore();

            // Подписаться на новые точки
            _dataStore.OnNewPoint += OnNewPointReceived;

            // Initialize statistics
            CalculateAndUpdateStatistics();

            // Initialize header and footer
            InitializeHeaderFooter();

            // Setup Emergency Vent button
            if (btnEmergency != null)
            {
                btnEmergency.Click += BtnEmergency_Click;
            }

            // Setup chart header buttons
            if (btnChartReset != null)
            {
                btnChartReset.Click += (_, __) => ResetGraph();
            }

            if (btnFullscreen != null)
            {
                btnFullscreen.Click += BtnFullscreen_Click;
            }

            // Setup LIVE STATUS panel border
            if (tlpLiveStatus != null)
            {
                tlpLiveStatus.Paint += TlpLiveStatus_Paint;
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
            cmbDuration.Items.Clear();
            foreach (var d in DurationData)
                cmbDuration.Items.Add(d.Name);
            cmbDuration.SelectedIndex = 0;

            // X step AUTO (disabled)
            cmbXStep.Items.Clear();
            cmbXStep.Items.Add("AUTO (10s)");
            cmbXStep.SelectedIndex = 0;
            cmbXStep.Enabled = false;

            // Y step
            cmbYStep.Items.Clear();
            cmbYStep.Items.AddRange(new object[] { "10", "20", "50", "100" });
            cmbYStep.SelectedIndex = 1;

            chkShowGrid.Checked = true;
        }

        private double GetDurationSeconds(int index)
        {
            if (index < 0 || index >= DurationData.Length) return 60;
            return DurationData[index].Seconds;
        }

        private double GetAutoGridStepX(double durationSeconds)
        {
            foreach (var d in DurationData)
                if (d.Seconds == (int)durationSeconds)
                    return d.GridStep;
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
            cmbXStep.Items.Clear();
            cmbXStep.Items.Add($"AUTO ({FormatGridStep(_gridStepXSeconds)})");
            cmbXStep.SelectedIndex = 0;
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
            var gridColor = new SKColor(35, 40, 50);
            var axisColor = new SKColor(50, 55, 65);
            var labelColor = new SKColor(120, 125, 135);

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
                    LineSmoothness = 0 // Will be changed by ApplySmoothing()
                },
                // Target line
                new LineSeries<ObservablePoint>
                {
                    Name = "Target",
                    Values = _seriesTarget,
                    Stroke = new SolidColorPaint(new SKColor(240, 200, 0), 2),
                    Fill = null,
                    GeometryStroke = null,
                    GeometrySize = 0
                },
                // Min threshold
                new LineSeries<ObservablePoint>
                {
                    Name = "Min",
                    Values = _seriesMin,
                    Stroke = new SolidColorPaint(new SKColor(240, 180, 0), 2),
                    Fill = null,
                    GeometryStroke = null,
                    GeometrySize = 0
                },
                // Max threshold
                new LineSeries<ObservablePoint>
                {
                    Name = "Max",
                    Values = _seriesMax,
                    Stroke = new SolidColorPaint(new SKColor(240, 70, 70), 2),
                    Fill = null,
                    GeometryStroke = null,
                    GeometrySize = 0
                },
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
            chartPressure.BackColor = Color.FromArgb(22, 24, 30);
        }

        private void ApplySmoothing()
        {
            // если чекбокса нет — просто ничего
            if (Controls.Find("chkSmoothing", true).Length == 0 || chkSmoothing == null) return;

            bool smooth = chkSmoothing.Checked;

            // Update LineSmoothness for current series
            if (chartPressure.Series != null && chartPressure.Series.Any())
            {
                var currentSeries = chartPressure.Series.FirstOrDefault() as LineSeries<ObservablePoint>;
                if (currentSeries != null)
                {
                    currentSeries.LineSmoothness = smooth ? 0.2 : 0; // 0..1 (меньше = спокойнее)
                }
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
            ApplyThresholdLines();
            UpdateTargetLine();

            // Update statistics
            CalculateAndUpdateStatistics();
        }

        // =========================
        // Duration change
        // =========================
        private void CmbDuration_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = cmbDuration.SelectedIndex;
            if (idx < 0 || idx >= DurationData.Length) return;

            _timeWindowSeconds = DurationData[idx].Seconds;
            _gridStepXSeconds = DurationData[idx].GridStep;

            UpdateGridStepXDisplay();

            RedrawFromStore();

            ApplyGridSettings();
            UpdateCustomLabelsX();
        }

        private void RedrawFromStore()
        {
            // Очистить текущие точки
            _seriesCurrent.Clear();
            _lastTargetValue = null;
            _timeSeconds = 0;

            // Перерисовать из Store
            foreach (var point in _dataStore.Points)
            {
                _seriesCurrent.Add(new ObservablePoint(point.ElapsedSeconds, point.Current));

                if (point.Target > 0)
                {
                    _lastTargetValue = point.Target;
                }

                _timeSeconds = point.ElapsedSeconds + TimeStep;
            }

            // Применить окно времени БЕЗ трима
            ApplyTimeWindow(forceTrim: false);
            ApplyThresholdLines();
            UpdateTargetLine();
        }

        // =========================
        // Time window + trim (LiveCharts2)
        // =========================
        private void ApplyTimeWindow(bool forceTrim)
        {
            if (chartPressure.XAxes == null || !chartPressure.XAxes.Any()) return;
            var xAxis = chartPressure.XAxes.FirstOrDefault();
            if (xAxis == null) return;

            double xMax = _timeSeconds;
            double xMin = Math.Max(0, xMax - _timeWindowSeconds);

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

            UpdateCustomLabelsX();

            if (!forceTrim) return;

            // Trim old points from current series
            TrimSeriesByX(_seriesCurrent, xMin);

            // после трима индекс курсора может стать неверным
            _lastNearestIndex = -1;
        }

        private static void TrimSeriesByX(ObservableCollection<ObservablePoint> series, double xMin)
        {
            if (series.Count == 0) return;
            
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
                series.Clear();
                return;
            }
            
            // Если первая точка уже >= xMin, ничего не делаем
            if (firstKeepIndex == 0) return;
            
            // Более эффективный способ: создаем список элементов для сохранения
            // и пересоздаем коллекцию (O(n) вместо O(k*n) для k удалений)
            var itemsToKeep = new List<ObservablePoint>();
            for (int i = firstKeepIndex; i < series.Count; i++)
            {
                itemsToKeep.Add(series[i]);
            }
            
            series.Clear();
            foreach (var item in itemsToKeep)
            {
                series.Add(item);
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

            bool showGrid = chkShowGrid.Checked;

            // Update grid visibility
            if (showGrid)
            {
                if (xAxis is Axis xAxisConcrete)
                    xAxisConcrete.SeparatorsPaint = new SolidColorPaint(new SKColor(35, 40, 50));
                if (yAxis is Axis yAxisConcrete)
                    yAxisConcrete.SeparatorsPaint = new SolidColorPaint(new SKColor(35, 40, 50));
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
        private void Thresholds_ValueChanged(object? sender, EventArgs e)
        {
            // Save thresholds to settings
            SaveThresholdsToSettings();
            // Apply threshold lines to chart
            ApplyThresholdLines();
        }

        /// <summary>
        /// Загружает значения thresholds из настроек приложения
        /// </summary>
        private void LoadThresholdsFromSettings()
        {
            var settings = FormOptions.AppOptions.Current;
            
            // Временно отключаем обработчики событий, чтобы не сохранять при загрузке
            nudMaximum.ValueChanged -= Thresholds_ValueChanged;
            numericUpDown2.ValueChanged -= Thresholds_ValueChanged;

            // Обновляем максимальные значения на основе настроек
            if (settings.MaxPressure.HasValue)
            {
                // Увеличиваем Maximum, если значение из настроек больше текущего Maximum
                decimal maxValue = (decimal)settings.MaxPressure.Value;
                if (maxValue > nudMaximum.Maximum)
                {
                    nudMaximum.Maximum = maxValue + 100; // Добавляем запас
                }
                if (maxValue >= nudMaximum.Minimum)
                {
                    nudMaximum.Value = maxValue;
                }
            }

            if (settings.MinPressure.HasValue)
            {
                decimal minValue = (decimal)settings.MinPressure.Value;
                // Увеличиваем Maximum для Minimum, если нужно
                if (minValue > numericUpDown2.Maximum)
                {
                    numericUpDown2.Maximum = minValue + 100; // Добавляем запас
                }
                if (minValue >= numericUpDown2.Minimum)
                {
                    numericUpDown2.Value = minValue;
                }
            }

            // Включаем обработчики обратно
            nudMaximum.ValueChanged += Thresholds_ValueChanged;
            numericUpDown2.ValueChanged += Thresholds_ValueChanged;
        }

        /// <summary>
        /// Сохраняет значения thresholds в настройки приложения
        /// </summary>
        private void SaveThresholdsToSettings()
        {
            var settings = FormOptions.AppOptions.Current.Clone();
            settings.MaxPressure = (double)nudMaximum.Value;
            settings.MinPressure = (double)numericUpDown2.Value;
            FormOptions.AppOptions.Current = settings;

            // Вызываем делегат для сохранения настроек (если auto-save включен)
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

            double x1 = xAxis.MinLimit ?? 0;
            double x2 = xAxis.MaxLimit ?? _timeWindowSeconds;

            if (x2 <= x1)
            {
                x1 = Math.Max(0, _timeSeconds - _timeWindowSeconds);
                x2 = Math.Max(x1 + 1, _timeSeconds);
            }

            double maxVal = (double)nudMaximum.Value;
            double minVal = (double)numericUpDown2.Value;

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

            double x1 = xAxis.MinLimit ?? 0;
            double x2 = xAxis.MaxLimit ?? _timeWindowSeconds;

            if (x2 <= x1)
            {
                x1 = Math.Max(0, _timeSeconds - _timeWindowSeconds);
                x2 = Math.Max(x1 + 1, _timeSeconds);
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
        // Mouse move (fast + throttled) (LiveCharts2)
        // =========================
        private void ChartPressure_MouseMove(object? sender, MouseEventArgs e)
        {
            // throttle
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
                        cursorSeries.IsVisible = true;
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
            foreach (var point in _dataStore.Points)
            {
                _seriesCurrent.Add(new ObservablePoint(point.ElapsedSeconds, point.Current));

                if (point.Target > 0)
                {
                    _lastTargetValue = point.Target;
                }

                _timeSeconds = point.ElapsedSeconds + TimeStep;
            }

            ApplyTimeWindow(forceTrim: true);
            ApplyThresholdLines();
            UpdateTargetLine();
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

            // Update footer statistics
            UpdateFooterStatistics();
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

        private void BtnFullscreen_Click(object? sender, EventArgs e)
        {
            // Toggle fullscreen
            if (WindowState == FormWindowState.Maximized && FormBorderStyle == FormBorderStyle.None)
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

        private void TlpLiveStatus_Paint(object? sender, PaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            if (panel == null) return;

            // Draw border around LIVE STATUS panel
            using var pen = new Pen(_isDarkTheme ? Color.FromArgb(60, 65, 75) : Color.FromArgb(200, 200, 210), 2);
            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            e.Graphics.DrawRectangle(pen, rect);
        }

        // btnGoTarget_Click is implemented in GraphForm.HeaderFooter.cs
    }
}
