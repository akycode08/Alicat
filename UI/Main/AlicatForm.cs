using Alicat.Domain;
using Alicat.Services.Controllers;
using Alicat.Services.Protocol;
using Alicat.Services.Serial;
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alicat.Services.Data;
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    /// <summary>
    /// Главная форма приложения Alicat Controller.
    /// Partial class: основная логика, поля, конструктор.
    /// </summary>
    public partial class AlicatForm : Form
    {
        // ====================================================================
        // ПОЛЯ
        // ====================================================================
        private double _current = 0.0;
        private double _setPoint = 0.0;
        private string _unit = "PSIG";

        private bool _isExhaust = false;
        private double? _lastCurrent = null;
        private double? _lastLoggedPressure = null;

        private SerialClient? _serial;
        private readonly Timer _pollTimer = new() { Interval = 500 };

        private double _maxPressure = 200.0;
        private double _maxIncrementLimit = 20.0;
        private double _currentIncrement = 5.0;

        private readonly DeviceState _state = new();
        private RampController? _ramp;

        private readonly SessionDataStore _dataStore = new();

        private TerminalForm? _terminalForm;
        private GraphForm? _graphForm;
        private TableForm? _tableForm;

        // ====================================================================
        // КОНСТРУКТОР
        // ====================================================================
        public AlicatForm()
        {
            InitializeComponent();

            // ✅ Create logo AFTER InitializeComponent (not in Designer!)
            CreateLogo();

            // Меню
            menuSettingsOptions.Click += btnOptions_Click;
            menuSettingsCommunication.Click += btnCommunication_Click;
            menuFileNewSession.Click += menuFileNewSession_Click;
            menuFileTestMode.Click += menuFileTestMode_Click;

            // Навигация
            btnGraph.Click += btnGraph_Click;
            btnTable.Click += btnTable_Click;
            btnTerminal.Click += btnTerminal_Click;

            // Управление давлением
            btnGoToTarget.Click += btnGoTarget_Click;
            btnPurge.Click += btnPurge_Click;
            btnIncrease.Click += btnIncrease_Click;
            btnDecrease.Click += btnDecrease_Click;
            btnIncrementMinus.Click += btnIncrementMinus_Click;
            btnIncrementPlus.Click += btnIncrementPlus_Click;

            // Валидация
            txtTargetInput.TextChanged += (_, __) => ValidateTargetAgainstMax();
            txtIncrement.TextChanged += (_, __) => UpdateIncrementFromText();

            // Начальные значения UI
            UI_SetPressureUnits(_unit);
            UI_SetRampSpeedUnits("PSIG/s");
            UI_SetSetPoint(_setPoint, _unit);
            RefreshCurrent();
            UpdateIncrementButtons();

            // Polling timer
            _pollTimer.Tick += (_, __) => _serial?.Send(AlicatCommands.ReadAls);

            ApplyOptionsToUi();

            // Применяем тему после инициализации (цвета и стили из AlicatForm.Theme.cs)
            ApplyLightTheme();

            // Инициализируем статус подключения (по умолчанию отключен)
            UI_UpdateConnectionStatus(false);
        }

        // ====================================================================
        // OPTIONS
        // ====================================================================
        private void ApplyOptionsToUi()
        {
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _maxIncrementLimit = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;

            ValidateTargetAgainstMax();
            ValidateIncrementAgainstMax();
            UpdateIncrementButtons();

            // Update max pressure display
            lblMaxPressureValue.Text = $"{_maxPressure:F0} PSIG";
        }

        // ====================================================================
        // PARSERS (используются в Communication.cs)
        // ====================================================================
        private static bool TryParseAls(string line, out double cur, out double sp, out string? unit)
        {
            cur = 0; sp = 0; unit = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out cur)) return false;
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out sp)) return false;

            for (int i = 3; i < parts.Length; i++)
            {
                var p = parts[i].Trim().ToUpperInvariant();
                
                // Поддерживаемые единицы измерения давления из таблицы Alicat
                // 0: Unit not specified (пустая единица)
                // 1: Unknown unit ("---")
                // 2: Pa
                // 3: hPa
                // 4: kPa
                // 5: MPa
                // 6: mbar
                // 7: bar
                // 8: g/cm²
                // 9: kg/cm
                // 10: PSI
                // 11: PSF
                // 12: mTorr
                // 13: torr
                
                if (p is "PA" or "HPA" or "KPA" or "MPA" or 
                    "MBAR" or "BAR" or 
                    "G/CM²" or "G/CM2" or "GCM²" or "GCM2" or
                    "KG/CM" or "KGCM" or
                    "PSIG" or "PSI" or "PSF" or
                    "MTORR" or "TORR" or
                    "---" or "" or string.Empty)
                {
                    // Нормализуем единицы к стандартному виду
                    unit = NormalizeUnit(p);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Нормализует единицу измерения к стандартному виду для отображения.
        /// </summary>
        private static string NormalizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit == "---" || unit == "")
                return "PSIG"; // Default unit

            var upper = unit.ToUpperInvariant();
            
            return upper switch
            {
                "PA" => "Pa",
                "HPA" => "hPa",
                "KPA" => "kPa",
                "MPA" => "MPa",
                "MBAR" => "mbar",
                "BAR" => "bar",
                "G/CM²" or "G/CM2" or "GCM²" or "GCM2" => "g/cm²",
                "KG/CM" or "KGCM" => "kg/cm",
                "PSIG" => "PSIG",
                "PSI" => "PSI",
                "PSF" => "PSF",
                "MTORR" => "mTorr",
                "TORR" => "torr",
                _ => unit // Возвращаем как есть, если не распознали
            };
        }

        private static bool TryParseAsr(string line, out double ramp, out string units)
        {
            ramp = 0;
            units = "PSIG/s";

            if (string.IsNullOrWhiteSpace(line))
                return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return false;

            if (!parts[0].Equals("A", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out ramp))
                return false;

            string? foundUnits = null;
            for (int i = 1; i < parts.Length; i++)
            {
                var p = parts[i].Trim();
                if (p.EndsWith("/s", StringComparison.OrdinalIgnoreCase))
                {
                    foundUnits = p;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(foundUnits))
                return false;

            // Извлекаем единицу без "/s", нормализуем и добавляем "/s" обратно
            var unitWithoutSlash = foundUnits.Substring(0, foundUnits.Length - 2).Trim();
            var normalizedUnit = NormalizeUnit(unitWithoutSlash);
            units = $"{normalizedUnit}/s";
            
            return true;
        }


        // ====================================================================
        // VALIDATION
        // ====================================================================

        /// <summary>
        /// Валидация целевого значения давления против максимального.
        /// </summary>
        private void ValidateTargetAgainstMax()
        {
            var text = txtTargetInput.Text?.Trim();
            bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetVal);
            bool over = parsed && targetVal > _maxPressure;

            if (isDarkTheme)
            {
                txtTargetInput.BackColor = over ? System.Drawing.Color.FromArgb(60, 20, 20) : darkBgWindow;
            }
            else
            {
                txtTargetInput.BackColor = over ? System.Drawing.Color.MistyRose : lightBgWindow;
            }
        }

        /// <summary>
        /// Валидация increment против максимального лимита.
        /// Вызывается из Communication.cs при получении данных.
        /// </summary>
        private void ValidateIncrementAgainstMax()
        {
            bool overLimit = _currentIncrement > _maxIncrementLimit;

            if (isDarkTheme)
            {
                txtIncrement.BackColor = overLimit ? System.Drawing.Color.FromArgb(60, 20, 20) : darkBgWindow;
            }
            else
            {
                txtIncrement.BackColor = overLimit ? System.Drawing.Color.MistyRose : lightBgWindow;
            }

            // Опционально: можно отключать кнопки, если превышен лимит
            // btnIncrease.Enabled = !overLimit;
            // btnDecrease.Enabled = !overLimit;
        }

        private void UpdateIncrementFromText()
        {
            if (double.TryParse(txtIncrement.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
            {
                _currentIncrement = Math.Clamp(val, 0.1, _maxIncrementLimit);
                UpdateIncrementButtons();
                ValidateIncrementAgainstMax();
            }
        }

        private void UpdateIncrementButtons()
        {
            btnIncrease.Text = $"▲ Increase (+{_currentIncrement:F1} PSIG)";
            btnDecrease.Text = $"▼ Decrease (-{_currentIncrement:F1} PSIG)";
        }

        // ====================================================================
        // UTILITY
        // ====================================================================
        private static string TrimZeros(double v, int maxDecimals = 2) =>
            v.ToString("0." + new string('#', maxDecimals), CultureInfo.InvariantCulture);

        // ====================================================================
        // LOGO CREATION (called from InitializeComponent in Designer)
        // ====================================================================
        private void CreateLogo()
        {
            var logoBitmap = new System.Drawing.Bitmap(180, 45);
            using (var g = System.Drawing.Graphics.FromImage(logoBitmap))
            {
                g.Clear(System.Drawing.Color.FromArgb(0, 102, 170)); // DAC Blue

                using (var font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold))
                using (var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
                {
                    g.DrawString("DAC Tools", font, brush, new System.Drawing.PointF(50, 8));
                }

                using (var font = new System.Drawing.Font("Arial", 7))
                using (var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White))
                {
                    g.DrawString("Custom Equipment for High-Pressure Research", font, brush, new System.Drawing.PointF(50, 28));
                }

                // Simple gear icon
                g.DrawEllipse(System.Drawing.Pens.White, 15, 12, 20, 20);
                g.DrawLine(System.Drawing.Pens.White, 25, 12, 25, 32);
                g.DrawLine(System.Drawing.Pens.White, 15, 22, 35, 22);
            }

            if (picLogo != null)
            {
                picLogo.Image = logoBitmap;
            }
        }

        private void btnGoToTarget_Click(object sender, EventArgs e)
        {

        }
    }
}