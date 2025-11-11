using System;
using System.Globalization;
using System.Windows.Forms;

namespace Alicat
{
    public partial class FormOptions : Form
    {
        // ===== Монолитное хранилище настроек для всего приложения =====
        // Позже AlicatForm прочитает отсюда Max Pressure и др.
        internal static class AppOptions
        {
            internal class Model
            {
                public string PressureUnits = "PSI"; // "PSI" | "BAR"
                public string TimeUnits = "s";        // "ms" | "s" | "m" | "h"
                public double? MaxPressure;           // e.g. 200
                public double? PressureRamp;          // e.g. 5
                public double? MaxIncrement;          // e.g. 1

                public Model Clone() => new Model
                {
                    PressureUnits = this.PressureUnits,
                    TimeUnits = this.TimeUnits,
                    MaxPressure = this.MaxPressure,
                    PressureRamp = this.PressureRamp,
                    MaxIncrement = this.MaxIncrement
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
                MaxPressure = 200,  // разумный дефолт
                PressureRamp = 5,
                MaxIncrement = 20
            };
        }

        // Локальная копия настроек на время редактирования
        private AppOptions.Model _working;

        public FormOptions()
        {
            InitializeComponent();

            // 1) Берём текущее состояние (глобальные настройки) в локальную копию
            _working = AppOptions.Current.Clone();

            // 2) Показываем в UI
            BindModelToUi(_working);

            // 3) Подписки на кнопки
            btnApply.Click += (_, __) =>
            {
                if (TryApplyFromUi())
                    MessageBox.Show(this, "Applied.", "Options",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            btnOK.Click += (_, __) =>
            {
                if (TryApplyFromUi())
                {
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
            };
        }

        // ===================== Модель → UI =====================
        private void BindModelToUi(AppOptions.Model m)
        {
            // Выпадающие списки
            SafeSelect(cmbPressureUnits, m.PressureUnits, "PSI");
            SafeSelect(cmbTimeUnits, m.TimeUnits, "s");

            // Числовые поля (пусто, если null)
            txtMaxPressure.Text = m.MaxPressure?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtPressureRamp.Text = m.PressureRamp?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtMaxIncrement.Text = m.MaxIncrement?.ToString(CultureInfo.InvariantCulture) ?? "";
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

            var m = new AppOptions.Model
            {
                PressureUnits = cmbPressureUnits.SelectedItem?.ToString() ?? "PSI",
                TimeUnits = cmbTimeUnits.SelectedItem?.ToString() ?? "s",
                MaxPressure = ParseNullableDouble(txtMaxPressure.Text),
                PressureRamp = ParseNullableDouble(txtPressureRamp.Text),
                MaxIncrement = ParseNullableDouble(txtMaxIncrement.Text)
            };

            // Простая валидация: числа, если введены, должны быть ≥ 0
            if (!IsNullOrPositive(m.MaxPressure))
            {
                MarkError(txtMaxPressure);
                MessageBox.Show(this, "Max Pressure must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!IsNullOrPositive(m.PressureRamp))
            {
                MarkError(txtPressureRamp);
                MessageBox.Show(this, "Pressure Ramp must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!IsNullOrPositive(m.MaxIncrement))
            {
                MarkError(txtMaxIncrement);
                MessageBox.Show(this, "Max Increment must be a non-negative number (or empty).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Сохраняем: теперь AlicatForm сможет прочитать актуальные значения
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
            txtMaxPressure.BackColor = System.Drawing.SystemColors.Window;
            txtPressureRamp.BackColor = System.Drawing.SystemColors.Window;
            txtMaxIncrement.BackColor = System.Drawing.SystemColors.Window;
        }

        // (опционально) Если нужно забрать результат напрямую
        internal AppOptions.Model GetResultModel() => _working.Clone();
    }
}
