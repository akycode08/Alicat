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
        private double _rampSpeed = 0.0; // Current ramp speed value

        private bool _isExhaust = false;
        private bool _isPaused = false;
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

            // Инициализируем Presenter
            InitializePresenter();

            // Меню
            menuSettingsOptions.Click += btnOptions_Click_Presenter;
            menuSettingsCommunication.Click += btnCommunication_Click_Presenter;
            menuFileNewSession.Click += menuFileNewSession_Click_Presenter;
            menuFileTestMode.Click += menuFileTestMode_Click;

            // Навигация
            btnGraph.Click += btnGraph_Click_Presenter;
            btnTable.Click += btnTable_Click_Presenter;
            btnTerminal.Click += btnTerminal_Click_Presenter;

            // Управление давлением
            btnGoToTarget.Click += btnGoTarget_Click_Presenter;
            btnPause.Click += btnPause_Click_Presenter;
            btnPurge.Click += btnPurge_Click_Presenter;
            btnIncrease.Click += btnIncrease_Click_Presenter;
            btnDecrease.Click += btnDecrease_Click_Presenter;
            btnIncrementMinus.Click += btnIncrementMinus_Click_Presenter;
            btnIncrementPlus.Click += btnIncrementPlus_Click_Presenter;

            // Валидация
            txtTargetInput.TextChanged += (_, __) => ValidateTargetAgainstMax();
            txtIncrement.TextChanged += txtIncrement_TextChanged_Presenter;

            // Применяем тему после инициализации (цвета и стили из AlicatForm.Theme.cs)
            ApplyLightTheme();
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
            lblMaxPressureValue.Text = $"{_maxPressure:F0} {_unit}";
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
                // Устройство возвращает единицы с "G" в конце (barG, kPaG, PSIG и т.д.)
                // 0: Unit not specified (пустая единица)
                // 1: Unknown unit ("---")
                // 2: Pa / PaG
                // 3: hPa / hPaG
                // 4: kPa / kPaG
                // 5: MPa / MPaG
                // 6: mbar / mbarG
                // 7: bar / barG
                // 8: g/cm² / g/cm²G
                // 9: kg/cm / kg/cmG
                // 10: PSI / PSIG
                // 11: PSF / PSFG
                // 12: mTorr / mTorrG
                // 13: torr / torrG


                // Проверяем известные единицы (избегаем символа ² в pattern matching)
                bool isKnownUnit = p == "PA" || p == "PAG" || p == "HPA" || p == "HPAG" || 
                                   p == "KPA" || p == "KPAG" || p == "MPA" || p == "MPAG" || 
                                   p == "MBAR" || p == "MBARG" || p == "BAR" || p == "BARG" || 
                                   p == "KG/CM" || p == "KGCM" || p == "KG/CMG" || p == "KGCMG" ||
                                   p == "PSIG" || p == "PSI" || p == "PSFG" || p == "PSF" ||
                                   p == "MTORR" || p == "MTORRG" || p == "TORR" || p == "TORRG" ||
                                   p == "---" || p == "" ||
                                   p.StartsWith("G/CM") || p.StartsWith("GCM");
                
                if (isKnownUnit)
                {
                    // Нормализуем единицы к стандартному виду (убираем "G" в конце)
                    unit = NormalizeUnit(p);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Нормализует единицу измерения к стандартному виду для отображения.
        /// Убирает "G" в конце для единиц, которые не должны его иметь (barG → bar, kPaG → kPa).
        /// </summary>
        private static string NormalizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit == "---" || unit == "")
                return "PSIG"; // Default unit

            var upper = unit.ToUpperInvariant();

            // Убираем "G" в конце, если есть (кроме PSIG, который уже содержит G)
            if (upper.EndsWith("G") && upper != "PSIG" && upper != "PSFG")
            {
                upper = upper.Substring(0, upper.Length - 1);
            }


            // Обработка вариантов g/cm²
            if (upper == "G/CM²" || upper == "G/CM2" || upper == "GCM²" || upper == "GCM2")
                return "g/cm²";

            // Обработка вариантов kg/cm
            if (upper == "KG/CM" || upper == "KGCM")
                return "kg/cm";

            
            // Обработка вариантов g/cm²
            if (upper == "G/CM²" || upper == "G/CM2" || upper == "GCM²" || upper == "GCM2")
                return "g/cm²";
            
            // Обработка вариантов kg/cm
            if (upper == "KG/CM" || upper == "KGCM")
                return "kg/cm";
            

            return upper switch
            {
                "PA" => "Pa",
                "HPA" => "hPa",
                "KPA" => "kPa",
                "MPA" => "MPa",
                "MBAR" => "mbar",
                "BAR" => "bar",
                "PSIG" => "PSIG",
                "PSI" => "PSI",
                "PSFG" => "PSF",
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
            btnIncrease.Text = $"▲ Increase (+{_currentIncrement:F1} {_unit})";
            btnDecrease.Text = $"▼ Decrease (-{_currentIncrement:F1} {_unit})";
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