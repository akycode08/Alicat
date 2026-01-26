using System;
using System.Globalization;
using System.Windows.Forms;

namespace PrecisionPressureController.UI.Options
{
    public partial class OptionsWindow : Form
    {
        // ===== Монолитное хранилище настроек для всего приложения =====
        // Позже MainWindow прочитает отсюда Max Pressure и др.
        internal static class AppOptions
        {
            internal class Model
            {
                public string PressureUnits = "PSI"; // PSI, PSIG, PSF, PSFG, Pa, hPa, kPa, MPa, mbar, bar, g/cm², kg/cm, mTorr, torr
                public string TimeUnits = "s";        // "ms" | "s" | "m" | "h"
                public double? PressureRamp;          // e.g. 5
                public double? MaxPressure;           // e.g. 200
                public double? MinPressure;           // e.g. 0
                public double? MaxIncrement;          // e.g. 20 (Maximum Step)
                public double? MinIncrement;          // e.g. 0.1 (Minimum Step)
                public int? PollingFrequency;         // Polling frequency in milliseconds (10, 50, 100, 250, 500, 1000, 2000, 5000)
                public bool AutoConnectOnStartup;     // Auto-connect to device on program startup

                public Model Clone() => new Model
                {
                    PressureUnits = this.PressureUnits,
                    TimeUnits = this.TimeUnits,
                    PressureRamp = this.PressureRamp,
                    MaxPressure = this.MaxPressure,
                    MinPressure = this.MinPressure,
                    MaxIncrement = this.MaxIncrement,
                    MinIncrement = this.MinIncrement,
                    PollingFrequency = this.PollingFrequency,
                    AutoConnectOnStartup = this.AutoConnectOnStartup
                };
            }

            private static Model _current = Defaults();

            public static Model Current
            {
                get => _current;
                set => _current = (value ?? Defaults()).Clone();
            }

            public static Model Defaults() => new Model
            {
                PressureUnits = "PSI",
                TimeUnits = "s",
                PressureRamp = 5,
                MaxPressure = 200,
                MinPressure = 0,
                MaxIncrement = 20,
                MinIncrement = 0.1,
                PollingFrequency = 500, // Default 500ms
                AutoConnectOnStartup = false // Default: no auto-connect
            };
        }

        // Локальная копия настроек на время редактирования
        private AppOptions.Model _working;

        // Событие для уведомления о применении настроек (без закрытия диалога)
        public event EventHandler? Applied;
        
        // Событие для уведомления о восстановлении настроек по умолчанию
        public event EventHandler? RestoredDefaults;

        public OptionsWindow()
        {
            InitializeComponent();

            // 1) Берём текущее состояние (глобальные настройки) в локальную копию
            // Всегда загружаем актуальные значения из AppOptions.Current
            RefreshFromCurrentSettings();

            // 3) Подписки на кнопки
            btnApply.Click += (_, __) =>
            {
                if (TryApplyFromUi())
                {
                    // Вызываем событие для обновления UI в главной форме
                    Applied?.Invoke(this, EventArgs.Empty);
                }
            };
            btnOK.Click += (_, __) =>
            {
                if (TryApplyFromUi())
                {
                    // Вызываем событие для обновления UI в главной форме перед закрытием
                    Applied?.Invoke(this, EventArgs.Empty);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };
            btnCancel.Click += (_, __) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            btnRestoreDefaults.Click += (_, __) =>
            {
                var d = AppOptions.Defaults();
                BindModelToUi(d);
                ClearErrorStyle();
                RestoredDefaults?.Invoke(this, EventArgs.Empty);
            };
        }

        // ===================== Модель → UI =====================
        private void BindModelToUi(AppOptions.Model m)
        {
            // Выпадающие списки
            SafeSelect(cmbPressureUnits, m.PressureUnits, "PSI");
            SafeSelect(cmbTimeUnits, m.TimeUnits, "s");
            
            // Polling Frequency
            int pollingFreq = m.PollingFrequency ?? 500;
            int[] intervals = { 10, 50, 100, 250, 500, 1000, 2000, 5000 };
            int index = Array.IndexOf(intervals, pollingFreq);
            if (index >= 0)
            {
                cmbPollingFrequency.SelectedIndex = index;
            }
            else
            {
                // Выбираем ближайшее значение
                int closestIndex = 0;
                int minDiff = Math.Abs(intervals[0] - pollingFreq);
                for (int i = 1; i < intervals.Length; i++)
                {
                    int diff = Math.Abs(intervals[i] - pollingFreq);
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        closestIndex = i;
                    }
                }
                cmbPollingFrequency.SelectedIndex = closestIndex;
            }

            // Числовые поля (пусто, если null)
            txtPressureRamp.Text = m.PressureRamp?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtMaxPressure.Text = m.MaxPressure?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtMinPressure.Text = m.MinPressure?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtMaxIncrement.Text = m.MaxIncrement?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtMinIncrement.Text = m.MinIncrement?.ToString(CultureInfo.InvariantCulture) ?? "";
            
            // Checkbox
            if (chkAutoConnectOnStartup != null)
                chkAutoConnectOnStartup.Checked = m.AutoConnectOnStartup;
        }

        private static void SafeSelect(ComboBox cmb, string value, string fallback)
        {
            if (cmb.Items.Count == 0) return;
            int idx = cmb.Items.IndexOf(value);
            if (idx < 0) idx = cmb.Items.IndexOf(fallback);
            if (idx < 0) idx = 0;
            cmb.SelectedIndex = idx;
        }

        // ===================== UI → Модель (+валидация) =====================
        private bool TryApplyFromUi()
        {
            ClearErrorStyle();

            // Получаем Polling Frequency из ComboBox
            int? pollingFreq = null;
            if (cmbPollingFrequency.SelectedIndex >= 0)
            {
                int[] intervals = { 10, 50, 100, 250, 500, 1000, 2000, 5000 };
                if (cmbPollingFrequency.SelectedIndex < intervals.Length)
                {
                    pollingFreq = intervals[cmbPollingFrequency.SelectedIndex];
                }
            }

            var m = new AppOptions.Model
            {
                PressureUnits = cmbPressureUnits.SelectedItem?.ToString() ?? "PSI",
                TimeUnits = cmbTimeUnits.SelectedItem?.ToString() ?? "s",
                PressureRamp = ParseNullableDouble(txtPressureRamp.Text),
                MaxPressure = ParseNullableDouble(txtMaxPressure.Text),
                MinPressure = ParseNullableDouble(txtMinPressure.Text),
                MaxIncrement = ParseNullableDouble(txtMaxIncrement.Text),
                MinIncrement = ParseNullableDouble(txtMinIncrement.Text),
                PollingFrequency = pollingFreq,
                AutoConnectOnStartup = chkAutoConnectOnStartup?.Checked ?? false
            };

            // Валидация: числа, если введены, должны быть ≥ 0
            if (!IsNullOrPositive(m.PressureRamp))
            {
                MarkError(txtPressureRamp);
                MessageBox.Show(this, "Pressure Ramp must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!IsNullOrPositive(m.MaxPressure))
            {
                MarkError(txtMaxPressure);
                MessageBox.Show(this, "Maximum Pressure must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!IsNullOrPositive(m.MinPressure))
            {
                MarkError(txtMinPressure);
                MessageBox.Show(this, "Minimum Pressure must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!IsNullOrPositive(m.MaxIncrement))
            {
                MarkError(txtMaxIncrement);
                MessageBox.Show(this, "Maximum Step must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!IsNullOrPositive(m.MinIncrement))
            {
                MarkError(txtMinIncrement);
                MessageBox.Show(this, "Minimum Step must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Валидация: Min ≤ Max
            if (m.MinPressure.HasValue && m.MaxPressure.HasValue && m.MinPressure > m.MaxPressure)
            {
                MarkError(txtMinPressure);
                MessageBox.Show(this, "Minimum Pressure cannot be greater than Maximum Pressure.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (m.MinIncrement.HasValue && m.MaxIncrement.HasValue && m.MinIncrement > m.MaxIncrement)
            {
                MarkError(txtMinIncrement);
                MessageBox.Show(this, "Minimum Step cannot be greater than Maximum Step.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Сохраняем: теперь MainWindow сможет прочитать актуальные значения
            AppOptions.Current = m;
            _working = m.Clone();
            return true;
        }

        // ===================== Утилиты =====================
        private static double? ParseNullableDouble(string text)
        {
            if (text == null) return null;
            var t = text.Trim();
            if (t.Length == 0) return null;
            return double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out var v)
                ? v : (double?)null;
        }

        private static bool IsNullOrPositive(double? v) => v == null || v >= 0;

        private void MarkError(TextBox tb)
        {
            tb.BackColor = System.Drawing.Color.MistyRose;
            tb.Focus();
            tb.SelectAll();
        }

        private void ClearErrorStyle()
        {
            txtPressureRamp.BackColor = System.Drawing.SystemColors.Window;
            txtMaxPressure.BackColor = System.Drawing.SystemColors.Window;
            txtMinPressure.BackColor = System.Drawing.SystemColors.Window;
            txtMaxIncrement.BackColor = System.Drawing.SystemColors.Window;
            txtMinIncrement.BackColor = System.Drawing.SystemColors.Window;
        }

        // (опционально) Если нужно забрать результат напрямую
        internal AppOptions.Model GetResultModel() => _working.Clone();

        /// <summary>
        /// Обновляет UI из текущих настроек AppOptions.Current
        /// (вызывается при изменении настроек извне, например, из ChartWindow)
        /// </summary>
        public void RefreshFromCurrentSettings()
        {
            // Обновляем локальную копию из текущих настроек
            _working = AppOptions.Current.Clone();
            
            // Обновляем UI
            BindModelToUi(_working);
        }
    }
}
