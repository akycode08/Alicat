using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alicat.Services.Data;
using Alicat.UI.Features.Table.Models;
using DataPointModel = Alicat.Services.Data.DataPoint;
using Alicat;

namespace Alicat.UI.Features.Table.Views
{
    /// <summary>
    /// Цветовая схема для темы
    /// </summary>
    public class ColorScheme
    {
        // Backgrounds
        public Color BgWindow { get; set; }
        public Color BgPanel { get; set; }
        public Color BgControl { get; set; }
        public Color BgButton { get; set; }
        public Color BgButtonHover { get; set; }
        public Color BgRowOdd { get; set; }
        public Color BgRowHover { get; set; }
        public Color BgRowSelected { get; set; }
        
        // Text
        public Color TextPrimary { get; set; }
        public Color TextSecondary { get; set; }
        public Color TextMuted { get; set; }
        
        // Accents
        public Color AccentGreen { get; set; }
        public Color AccentCyan { get; set; }
        public Color AccentGold { get; set; }
        public Color AccentRed { get; set; }
        public Color AccentBlue { get; set; }
        
        // Borders
        public Color BorderControl { get; set; }
        public Color BorderDark { get; set; }
    }

    public partial class TableForm : Form
    {
        private readonly SessionDataStore _dataStore;
        private BindingList<PressureDataPoint> _dataSource = new BindingList<PressureDataPoint>();
        
        private bool _isPaused = false;
        private bool _isDarkTheme = true;
        private string? _connectionStatus = null;
        private int? _connectionBaudRate = null;
        
        private double _maxPressure = 200.0;
        private double _minPressure = 0.0;
        private int _currentSortColumn = -1;
        private bool _sortAscending = true;
        
        private enum LogFilter
        {
            All,
            WithComments,
            Setpoints
        }
        
        private LogFilter _currentFilter = LogFilter.All;

        // Цветовые схемы
        private readonly ColorScheme _darkTheme = new ColorScheme
        {
            BgWindow = ColorTranslator.FromHtml("#0a0e1a"),
            BgPanel = ColorTranslator.FromHtml("#111827"),
            BgControl = ColorTranslator.FromHtml("#0f1320"),
            BgButton = ColorTranslator.FromHtml("#1f2937"),
            BgButtonHover = ColorTranslator.FromHtml("#2d3748"),
            BgRowOdd = ColorTranslator.FromHtml("#161d2d"),
            BgRowHover = ColorTranslator.FromHtml("#1f2937"),
            BgRowSelected = Color.FromArgb(38, 16, 185, 129), // rgba(16, 185, 129, 0.15) alpha = 0.15 * 255 ≈ 38
            
            TextPrimary = ColorTranslator.FromHtml("#e4e7eb"),
            TextSecondary = ColorTranslator.FromHtml("#9ca3af"),
            TextMuted = ColorTranslator.FromHtml("#6b7280"),
            
            AccentGreen = ColorTranslator.FromHtml("#10b981"),
            AccentCyan = ColorTranslator.FromHtml("#06b6d4"),
            AccentGold = ColorTranslator.FromHtml("#f59e0b"),
            AccentRed = ColorTranslator.FromHtml("#ef4444"),
            AccentBlue = ColorTranslator.FromHtml("#3b82f6"),
            
            BorderControl = ColorTranslator.FromHtml("#374151"),
            BorderDark = ColorTranslator.FromHtml("#1a1f2e")
        };

        private readonly ColorScheme _lightTheme = new ColorScheme
        {
            BgWindow = ColorTranslator.FromHtml("#f3f4f6"),
            BgPanel = ColorTranslator.FromHtml("#ffffff"),
            BgControl = ColorTranslator.FromHtml("#e5e7eb"),
            BgButton = ColorTranslator.FromHtml("#e5e7eb"),
            BgButtonHover = ColorTranslator.FromHtml("#d1d5db"),
            BgRowOdd = ColorTranslator.FromHtml("#f9fafb"),
            BgRowHover = ColorTranslator.FromHtml("#e5e7eb"),
            BgRowSelected = Color.FromArgb(26, 5, 150, 105), // rgba(5, 150, 105, 0.1) alpha = 0.1 * 255 ≈ 26
            
            TextPrimary = ColorTranslator.FromHtml("#111827"),
            TextSecondary = ColorTranslator.FromHtml("#4b5563"),
            TextMuted = ColorTranslator.FromHtml("#6b7280"),
            
            AccentGreen = ColorTranslator.FromHtml("#059669"),
            AccentCyan = ColorTranslator.FromHtml("#0891b2"),
            AccentGold = ColorTranslator.FromHtml("#d97706"),
            AccentRed = ColorTranslator.FromHtml("#dc2626"),
            AccentBlue = ColorTranslator.FromHtml("#2563eb"),
            
            BorderControl = ColorTranslator.FromHtml("#9ca3af"),
            BorderDark = ColorTranslator.FromHtml("#e5e7eb")
        };

        private ColorScheme _currentTheme;

        public TableForm(SessionDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeComponent();
            SetupDataGridView();
            SetupConnectionStatusPanel();
            LoadSettings();
            
            // Установить темную тему по умолчанию
            _currentTheme = _darkTheme;
            ApplyTheme();
            
            // Загрузить историю из Store
            LoadHistoryFromStore();
            
            // Подписаться на новые точки
            _dataStore.OnNewPoint += OnNewPointReceived;
        }

        private void SetupConnectionStatusPanel()
        {
            if (panelConnectionStatus != null)
            {
                panelConnectionStatus.Paint += ConnectionStatusPanel_Paint;
            }
        }

        private void ConnectionStatusPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (panelConnectionStatus == null || sender != panelConnectionStatus) return;

            var panel = panelConnectionStatus;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw border (light teal for both themes)
            Color borderColor = Color.FromArgb(100, 200, 180);
            using (var pen = new Pen(borderColor, 1))
            {
                g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }

            // Draw circle (green if connected, gray if disconnected)
            Color circleColor;
            if (!string.IsNullOrEmpty(_connectionStatus) && _connectionBaudRate.HasValue)
            {
                circleColor = Color.FromArgb(76, 175, 80); // Green
            }
            else
            {
                circleColor = _isDarkTheme 
                    ? Color.FromArgb(107, 114, 128)  // Gray for dark theme
                    : Color.FromArgb(156, 163, 175); // Gray for light theme
            }

            using (var brush = new SolidBrush(circleColor))
            {
                g.FillEllipse(brush, 8, 8, 10, 10);
            }

            // Draw connection text (light teal for both themes)
            string connectionText;
            if (!string.IsNullOrEmpty(_connectionStatus) && _connectionBaudRate.HasValue)
            {
                connectionText = $"Connected {_connectionStatus} @ {_connectionBaudRate.Value}";
            }
            else
            {
                connectionText = "Not Connected";
            }

            using (var brush = new SolidBrush(Color.FromArgb(100, 200, 180)))
            using (var font = new Font("Segoe UI", 9f))
            {
                g.DrawString(connectionText, font, brush, 22, 6);
            }
        }

        private void LoadSettings()
        {
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _minPressure = FormOptions.AppOptions.Current.MinPressure ?? 0.0;
        }

        private void SetupDataGridView()
        {
            dgvTable.AutoGenerateColumns = false;
            dgvTable.DataSource = _dataSource;
            dgvTable.AllowUserToAddRows = false;
            dgvTable.AllowUserToDeleteRows = false;
            dgvTable.ReadOnly = true;
            dgvTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTable.MultiSelect = true;
            dgvTable.EnableHeadersVisualStyles = false;
            dgvTable.RowHeadersVisible = false;
            
            // Cell double click
            dgvTable.CellDoubleClick += DgvTable_CellDoubleClick;
            dgvTable.ColumnHeaderMouseClick += DgvTable_ColumnHeaderMouseClick;
            dgvTable.CellPainting += DgvTable_CellPainting;
            dgvTable.RowPostPaint += DgvTable_RowPostPaint;
            dgvTable.CellFormatting += DgvTable_CellFormatting;
        }

        private void ApplyTheme()
        {
            _currentTheme = _isDarkTheme ? _darkTheme : _lightTheme;
            
            // Form
            BackColor = _currentTheme.BgWindow;
            
            // Title Bar
            panelTitleBar.BackColor = _currentTheme.BgControl;
            lblIcon.ForeColor = _currentTheme.TextPrimary;
            lblTitle.ForeColor = _currentTheme.TextPrimary;
            
            // Connection Status Panel - update background color based on connection state and theme
            if (panelConnectionStatus != null)
            {
                if (!string.IsNullOrEmpty(_connectionStatus) && _connectionBaudRate.HasValue)
                {
                    // Connected - dark green background for dark theme, light green for light theme
                    panelConnectionStatus.BackColor = _isDarkTheme 
                        ? Color.FromArgb(26, 61, 53)  // #1A3D35 - dark theme
                        : Color.FromArgb(220, 252, 231); // Light green for light theme
                }
                else
                {
                    // Disconnected - muted background
                    panelConnectionStatus.BackColor = _isDarkTheme
                        ? Color.FromArgb(55, 65, 81)  // #374151 - dark theme muted
                        : Color.FromArgb(229, 231, 235); // Light muted for light theme
                }
                panelConnectionStatus.Invalidate(); // Trigger repaint
            }
            
            // Toolbar
            panelToolbar.BackColor = _currentTheme.BgControl;
            if (lblThreshold != null)
                lblThreshold.ForeColor = _currentTheme.TextPrimary;
            
            // Buttons in title bar
            SetupButtonTheme(btnPause);
            SetupButtonTheme(btnClear);
            SetupButtonTheme(btnExport);
            if (btnTheme != null)
            {
                SetupButtonTheme(btnTheme);
                btnTheme.Text = _isDarkTheme ? "🌙" : "☀️";
            }
            
            // Filter buttons
            SetupFilterButtonTheme(btnTabAll, _currentFilter == LogFilter.All);
            SetupFilterButtonTheme(btnTabWithComments, _currentFilter == LogFilter.WithComments);
            SetupFilterButtonTheme(btnTabSetpoints, _currentFilter == LogFilter.Setpoints);
            
            // TextBox and CheckBox
            txtThreshold.BackColor = _currentTheme.BgPanel;
            txtThreshold.ForeColor = _currentTheme.TextPrimary;
            txtThreshold.BorderStyle = BorderStyle.FixedSingle;
            chkAutoScroll.ForeColor = _currentTheme.TextPrimary;
            chkAutoScroll.BackColor = _currentTheme.BgControl;
            
            // DataGridView
            dgvTable.BackgroundColor = _currentTheme.BgPanel;
            dgvTable.GridColor = _currentTheme.BorderDark;
            dgvTable.DefaultCellStyle.BackColor = _currentTheme.BgPanel;
            dgvTable.DefaultCellStyle.ForeColor = _currentTheme.TextPrimary;
            dgvTable.DefaultCellStyle.SelectionBackColor = _currentTheme.BgRowSelected;
            dgvTable.DefaultCellStyle.SelectionForeColor = _currentTheme.TextPrimary;
            dgvTable.AlternatingRowsDefaultCellStyle.BackColor = _currentTheme.BgRowOdd;
            
            dgvTable.ColumnHeadersDefaultCellStyle.BackColor = _currentTheme.BgControl;
            dgvTable.ColumnHeadersDefaultCellStyle.ForeColor = _currentTheme.TextMuted;
            dgvTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = _currentTheme.BgControl;
            dgvTable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            // Column colors
            if (colIndex != null)
                colIndex.DefaultCellStyle.ForeColor = _currentTheme.TextMuted;
            if (colTime != null)
                colTime.DefaultCellStyle.ForeColor = _currentTheme.TextSecondary;
            if (colPressure != null)
                colPressure.DefaultCellStyle.ForeColor = _currentTheme.AccentCyan;
            if (colSetpoint != null)
                colSetpoint.DefaultCellStyle.ForeColor = _currentTheme.AccentGold;
            if (colComment != null)
                colComment.DefaultCellStyle.ForeColor = _currentTheme.TextSecondary;
            
            // Footer
            panelFooter.BackColor = _currentTheme.BgControl;
            lblFooterStats.ForeColor = _currentTheme.TextMuted;
            lblFooterVersion.ForeColor = _currentTheme.TextMuted;
            
            // Refresh display
            dgvTable.Refresh();
            Refresh();
        }

        private void SetupButtonTheme(Button btn)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = _currentTheme.BgButton;
            btn.ForeColor = _currentTheme.TextSecondary;
            btn.Font = new Font("Segoe UI", 9F);
            btn.Cursor = Cursors.Hand;
            
            // Remove old handlers and add new ones
            btn.MouseEnter -= Button_MouseEnter;
            btn.MouseLeave -= Button_MouseLeave;
            btn.MouseEnter += Button_MouseEnter;
            btn.MouseLeave += Button_MouseLeave;
        }

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button btn)
                btn.BackColor = _currentTheme.BgButtonHover;
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn != btnTabAll && btn != btnTabWithComments && btn != btnTabSetpoints)
                btn.BackColor = _currentTheme.BgButton;
        }

        private void SetupFilterButtonTheme(Button btn, bool active)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9F);
            btn.Cursor = Cursors.Hand;
            
            if (active)
            {
                btn.BackColor = _currentTheme.AccentGreen;
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = _currentTheme.BgButton;
                btn.ForeColor = _currentTheme.TextSecondary;
            }
        }

        private string CalculateStatus(double pressure, double setpoint)
        {
            const double tolerance = 0.5;
            
            if (pressure > _maxPressure)
                return "Above Max";
            if (pressure < _minPressure)
                return "Below Min";
            if (Math.Abs(pressure - setpoint) <= tolerance)
                return "At Target";
            return "Normal";
        }

        private void LoadHistoryFromStore()
        {
            _dataSource.Clear();
            int index = 1;
            foreach (var point in _dataStore.Points)
            {
                var dataPoint = new PressureDataPoint
                {
                    Index = index++,
                    Time = point.Timestamp,
                    Pressure = point.Current,
                    Setpoint = point.Target,
                    Rate = point.RampSpeed,
                    Status = CalculateStatus(point.Current, point.Target),
                    Comment = point.Event ?? ""
                };
                _dataSource.Add(dataPoint);
            }
            UpdateStatistics();
        }

        private void OnNewPointReceived(DataPointModel point)
        {
            if (IsDisposed || _isPaused) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnNewPointReceived(point)));
                return;
            }

            // Проверяем threshold
            if (ShouldLogPoint(point))
            {
                var dataPoint = new PressureDataPoint
                {
                    Index = _dataSource.Count + 1,
                    Time = point.Timestamp,
                    Pressure = point.Current,
                    Setpoint = point.Target,
                    Rate = point.RampSpeed,
                    Status = CalculateStatus(point.Current, point.Target),
                    Comment = point.Event ?? ""
                };
                
                _dataSource.Add(dataPoint);
                
                // Auto-scroll
                if (chkAutoScroll.Checked && dgvTable.Rows.Count > 0)
                {
                    dgvTable.FirstDisplayedScrollingRowIndex = dgvTable.Rows.Count - 1;
                }
                
                ApplyCurrentFilter();
                UpdateStatistics();
            }
        }

        private double? _lastLoggedPressure = null;
        private double? _lastLoggedSetpoint = null;

        private bool ShouldLogPoint(DataPointModel point)
        {
            double threshold = double.TryParse(txtThreshold.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double t) ? t : 0.10;

            if (_lastLoggedPressure == null)
            {
                _lastLoggedPressure = point.Current;
                _lastLoggedSetpoint = point.Target;
                return true;
            }

            if (_lastLoggedSetpoint.HasValue && Math.Abs(point.Target - _lastLoggedSetpoint.Value) > 0.01)
            {
                _lastLoggedPressure = point.Current;
                _lastLoggedSetpoint = point.Target;
                return true;
            }

            double delta = Math.Abs(point.Current - _lastLoggedPressure.Value);
            if (delta >= threshold)
            {
                _lastLoggedPressure = point.Current;
                _lastLoggedSetpoint = point.Target;
                return true;
            }

            return false;
        }

        private void DgvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvTable.Rows[e.RowIndex];
            var dataPoint = row.DataBoundItem as PressureDataPoint;
            if (dataPoint == null) return;

            string info = $"{dataPoint.Time:HH:mm:ss.ff} — {dataPoint.Pressure:F2} → {dataPoint.Setpoint:F2}";
            using var dlg = new AddCommentForm(info, dataPoint.Comment);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dataPoint.Comment = dlg.CommentText;
                _dataSource.ResetItem(e.RowIndex);
                UpdateStatistics();
            }
        }

        private void DgvTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            if (_currentSortColumn == e.ColumnIndex)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _currentSortColumn = e.ColumnIndex;
                _sortAscending = true;
            }

            SortData();
        }

        private void SortData()
        {
            if (_currentSortColumn < 0) return;

            var columnName = dgvTable.Columns[_currentSortColumn].DataPropertyName;
            List<PressureDataPoint> sorted;

            switch (columnName)
            {
                case "Index":
                    sorted = _sortAscending 
                        ? _dataSource.OrderBy(x => x.Index).ToList()
                        : _dataSource.OrderByDescending(x => x.Index).ToList();
                    break;
                case "Time":
                    sorted = _sortAscending
                        ? _dataSource.OrderBy(x => x.Time).ToList()
                        : _dataSource.OrderByDescending(x => x.Time).ToList();
                    break;
                case "Pressure":
                    sorted = _sortAscending
                        ? _dataSource.OrderBy(x => x.Pressure).ToList()
                        : _dataSource.OrderByDescending(x => x.Pressure).ToList();
                    break;
                case "Setpoint":
                    sorted = _sortAscending
                        ? _dataSource.OrderBy(x => x.Setpoint).ToList()
                        : _dataSource.OrderByDescending(x => x.Setpoint).ToList();
                    break;
                case "Rate":
                    sorted = _sortAscending
                        ? _dataSource.OrderBy(x => x.Rate).ToList()
                        : _dataSource.OrderByDescending(x => x.Rate).ToList();
                    break;
                case "Status":
                    sorted = _sortAscending
                        ? _dataSource.OrderBy(x => x.Status).ToList()
                        : _dataSource.OrderByDescending(x => x.Status).ToList();
                    break;
                case "Comment":
                    sorted = _sortAscending
                        ? _dataSource.OrderBy(x => x.Comment).ToList()
                        : _dataSource.OrderByDescending(x => x.Comment).ToList();
                    break;
                default:
                    return;
            }

            _dataSource.Clear();
            foreach (var item in sorted)
            {
                _dataSource.Add(item);
            }

            UpdateColumnHeaderSortIndicator();
        }

        private void UpdateColumnHeaderSortIndicator()
        {
            foreach (DataGridViewColumn col in dgvTable.Columns)
            {
                if (col.Index == _currentSortColumn)
                {
                    col.HeaderText = col.HeaderText.TrimEnd('▲', '▼') + (_sortAscending ? " ▲" : " ▼");
                }
                else
                {
                    col.HeaderText = col.HeaderText.TrimEnd('▲', '▼');
                }
            }
        }

        private void DgvTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Ramp Speed column - color coding
            if (e.ColumnIndex == colRampSpeed.Index && e.RowIndex >= 0)
            {
                var dataPoint = dgvTable.Rows[e.RowIndex].DataBoundItem as PressureDataPoint;
                if (dataPoint != null)
                {
                    Color textColor = _currentTheme.TextMuted;
                    if (dataPoint.Rate > 0.1)
                        textColor = _currentTheme.AccentRed;
                    else if (dataPoint.Rate < -0.1)
                        textColor = _currentTheme.AccentBlue;
                    
                    e.CellStyle.ForeColor = textColor;
                }
            }
            
            // Pressure column - cyan
            if (e.ColumnIndex == colPressure.Index && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = _currentTheme.AccentCyan;
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
            
            // Setpoint column - gold
            if (e.ColumnIndex == colSetpoint.Index && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = _currentTheme.AccentGold;
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
            
            // Status column - badge background
            if (e.ColumnIndex == colStatus.Index && e.RowIndex >= 0)
            {
                var dataPoint = dgvTable.Rows[e.RowIndex].DataBoundItem as PressureDataPoint;
                if (dataPoint != null)
                {
                    Color bgColor = Color.Transparent;
                    switch (dataPoint.Status)
                    {
                        case "Normal":
                            bgColor = Color.FromArgb(30, _currentTheme.AccentGreen.R, _currentTheme.AccentGreen.G, _currentTheme.AccentGreen.B);
                            break;
                        case "At Target":
                            bgColor = Color.FromArgb(30, _currentTheme.AccentGold.R, _currentTheme.AccentGold.G, _currentTheme.AccentGold.B);
                            break;
                        case "Above Max":
                            bgColor = Color.FromArgb(30, _currentTheme.AccentRed.R, _currentTheme.AccentRed.G, _currentTheme.AccentRed.B);
                            break;
                        case "Below Min":
                            bgColor = Color.FromArgb(30, _currentTheme.AccentBlue.R, _currentTheme.AccentBlue.G, _currentTheme.AccentBlue.B);
                            break;
                    }
                    e.CellStyle.BackColor = bgColor;
                }
            }
            
            // Comment column - italic, gray
            if (e.ColumnIndex == colComment.Index && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = _currentTheme.TextSecondary;
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            }
        }

        private void DgvTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvTable.Rows[e.RowIndex];
            var dataPoint = row.DataBoundItem as PressureDataPoint;
            if (dataPoint == null) return;

            // Format Time column
            if (e.ColumnIndex == colTime.Index)
            {
                if (dataPoint.Time != null)
                {
                    e.Value = dataPoint.Time.ToString("HH:mm:ss.ff");
                    e.FormattingApplied = true;
                }
            }

            // Format Ramp Speed column with arrow
            if (e.ColumnIndex == colRampSpeed.Index)
            {
                string arrow = "→";
                if (dataPoint.Rate > 0.1)
                    arrow = "↗";
                else if (dataPoint.Rate < -0.1)
                    arrow = "↘";
                
                e.Value = $"{arrow} {Math.Abs(dataPoint.Rate):F2} PSI/s";
                e.FormattingApplied = true;
            }
        }

        private void DgvTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Draw left border based on status
            var row = dgvTable.Rows[e.RowIndex];
            var dataPoint = row.DataBoundItem as PressureDataPoint;
            if (dataPoint != null)
            {
                Color borderColor = Color.Empty;
                int borderWidth = 0;
                
                switch (dataPoint.Status)
                {
                    case "At Target":
                        borderColor = _currentTheme.AccentGreen;
                        borderWidth = 3;
                        break;
                    case "Above Max":
                        borderColor = _currentTheme.AccentRed;
                        borderWidth = 3;
                        break;
                    case "Below Min":
                        borderColor = _currentTheme.AccentBlue;
                        borderWidth = 3;
                        break;
                }
                
                if (borderWidth > 0 && borderColor != Color.Empty)
                {
                    using (var pen = new Pen(borderColor, borderWidth))
                    {
                        e.Graphics.DrawLine(pen, 
                            e.RowBounds.Left, 
                            e.RowBounds.Top,
                            e.RowBounds.Left, 
                            e.RowBounds.Bottom);
                    }
                }
            }
        }

        private void BtnTabAll_Click(object sender, EventArgs e)
        {
            _currentFilter = LogFilter.All;
            ApplyTheme(); // Переприменяем тему для обновления кнопок фильтров
            ApplyCurrentFilter();
        }

        private void BtnTabWithComments_Click(object sender, EventArgs e)
        {
            _currentFilter = LogFilter.WithComments;
            ApplyTheme(); // Переприменяем тему для обновления кнопок фильтров
            ApplyCurrentFilter();
        }

        private void BtnTabSetpoints_Click(object sender, EventArgs e)
        {
            _currentFilter = LogFilter.Setpoints;
            ApplyTheme(); // Переприменяем тему для обновления кнопок фильтров
            ApplyCurrentFilter();
        }
        
        private void BtnTheme_Click(object sender, EventArgs e)
        {
            _isDarkTheme = !_isDarkTheme;
            ApplyTheme();
        }
        
        /// <summary>
        /// Публичный метод для применения темы из главной формы
        /// </summary>
        /// <summary>
        /// Публичный метод для применения темы из главного окна
        /// </summary>
        /// <param name="isDark">true для темной темы, false для светлой</param>
        public void ApplyTheme(bool isDark)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ApplyTheme(isDark)));
                return;
            }
            
            _isDarkTheme = isDark;
            ApplyTheme();
        }

        private void ApplyCurrentFilter()
        {
            foreach (DataGridViewRow row in dgvTable.Rows)
            {
                if (row.DataBoundItem is PressureDataPoint dataPoint)
                {
                    bool visible = false;
                    
                    switch (_currentFilter)
                    {
                        case LogFilter.All:
                            visible = true;
                            break;
                        case LogFilter.WithComments:
                            visible = !string.IsNullOrWhiteSpace(dataPoint.Comment);
                            break;
                        case LogFilter.Setpoints:
                            visible = !string.IsNullOrWhiteSpace(dataPoint.Comment) && 
                                     (dataPoint.Comment.Contains("Setpoint", StringComparison.OrdinalIgnoreCase) ||
                                      dataPoint.Status == "At Target");
                            break;
                    }
                    
                    row.Visible = visible;
                }
            }
            UpdateStatistics();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            _isPaused = !_isPaused;
            btnPause.Text = _isPaused ? "Resume" : "Pause";
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all data? This action cannot be undone.", "Clear Data",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _dataSource.Clear();
                _lastLoggedPressure = null;
                _lastLoggedSetpoint = null;
                UpdateStatistics();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"Alicat_Data_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToCsv(saveDialog.FileName);
            }
        }

        private void ExportToCsv(string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                
                // Header
                writer.WriteLine("Index,Time,Pressure,Setpoint,RampSpeed,Status,Comment");
                
                // Data
                foreach (var point in _dataSource)
                {
                    writer.WriteLine($"{point.Index}," +
                                   $"{point.Time:HH:mm:ss.fff}," +
                                   $"{point.Pressure.ToString("F2", CultureInfo.InvariantCulture)}," +
                                   $"{point.Setpoint.ToString("F2", CultureInfo.InvariantCulture)}," +
                                   $"{point.Rate.ToString("F2", CultureInfo.InvariantCulture)}," +
                                   $"{point.Status}," +
                                   $"\"{point.Comment.Replace("\"", "\"\"")}\"");
                }
                
                MessageBox.Show($"Data exported successfully to {filePath}", "Export Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            int total = _dataSource.Count;
            int filtered = dgvTable.Rows.Cast<DataGridViewRow>().Count(r => r.Visible);
            int selected = dgvTable.SelectedRows.Count;
            
            lblFooterStats.Text = $"Total: {total}  |  Filtered: {filtered}  |  Selected: {selected}";
        }

        public void SetConnectionInfo(string? portName, int? baudRate)
        {
            _connectionStatus = portName;
            _connectionBaudRate = baudRate;
            
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetConnectionInfo(portName, baudRate)));
                return;
            }
            
            // Update connection status panel background and trigger repaint
            if (panelConnectionStatus != null)
            {
                if (portName != null && baudRate.HasValue)
                {
                    // Connected
                    panelConnectionStatus.BackColor = _isDarkTheme 
                        ? Color.FromArgb(26, 61, 53)  // #1A3D35 - dark theme
                        : Color.FromArgb(220, 252, 231); // Light green for light theme
                }
                else
                {
                    // Disconnected
                    panelConnectionStatus.BackColor = _isDarkTheme
                        ? Color.FromArgb(55, 65, 81)  // #374151 - dark theme muted
                        : Color.FromArgb(229, 231, 235); // Light muted for light theme
                }
                panelConnectionStatus.Invalidate(); // Trigger repaint to draw circle and text
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _dataStore.OnNewPoint -= OnNewPointReceived;
            base.OnFormClosing(e);
        }
    }
}
