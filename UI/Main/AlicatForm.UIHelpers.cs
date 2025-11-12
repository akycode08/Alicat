using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

namespace Alicat
{
    public partial class AlicatForm : Form
        {
            private void ValidateTargetAgainstMax()
                {
                    var text = txtTarget.Text?.Trim();
                    bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetVal);
                    bool over = parsed && targetVal > _maxPressure;

                    txtTarget.BackColor = over ? System.Drawing.Color.MistyRose : System.Drawing.SystemColors.Window;
                    btnGoTarget.Enabled = chkConfirmGo.Checked && parsed;
                }

            private void ValidateIncrementAgainstMax()
                {
                    double inc = (double)nudIncrement.Value;
                    bool over = inc > _maxIncrementLimit;

                    nudIncrement.BackColor = over ? System.Drawing.Color.MistyRose : System.Drawing.SystemColors.Window;
                    btnGoPlus.Enabled = !over;
                    btnGoMinus.Enabled = !over;
            }

            private static string TrimZeros(double v, int maxDecimals = 2) =>
                v.ToString("0." + new string('#', maxDecimals), CultureInfo.InvariantCulture);

            private void RefreshCurrent() => lblCurrentBig.Text = $"{_current:0.0} {_unit}";

            public void UI_SetPressureUnits(string units) =>
                boxPressureUnits.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();

            public void UI_SetRampSpeedUnits(string units) =>
                boxRampSpeedUnits.Text = string.IsNullOrWhiteSpace(units) ? "—" : units.Trim();

            public void UI_SetSetPoint(double sp, string? units = null)
            {
                var u = string.IsNullOrWhiteSpace(units) ? _unit : units!;
                boxSetPoint.Text = $"{TrimZeros(sp)} {u}";
            }

            public void UI_SetTimeToSetPoint(TimeSpan? t)
            {
                if (t == null) { boxTimeToSetPoint.Text = "—"; return; }
                var secs = Math.Max(0, t.Value.TotalSeconds);
                boxTimeToSetPoint.Text = $"{TrimZeros(secs, 1)} s";
            }

            public void UI_Status_Up(bool on) => icoUp.ForeColor = on ? System.Drawing.Color.OrangeRed : System.Drawing.Color.Gray;
            public void UI_Status_Mid(bool on) => icoMid.ForeColor = on ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Gray;
            public void UI_Status_Down(bool on) => icoDown.ForeColor = on ? System.Drawing.Color.RoyalBlue : System.Drawing.Color.Gray;

            private void UI_SetTrendStatus(double? prev, double now, bool isExhaust)
            {
                if (isExhaust)
                {
                    UI_Status_Up(false);
                    UI_Status_Mid(false);
                    UI_Status_Down(true);
                    return;
                }

                const double EPS = 0.05;

                if (prev is null)
                {
                    UI_Status_Up(false);
                    UI_Status_Mid(true);
                    UI_Status_Down(false);
                    return;
                }

                double delta = now - prev.Value;
                if (delta > EPS) { UI_Status_Up(true); UI_Status_Mid(false); UI_Status_Down(false); }
                else if (delta < -EPS) { UI_Status_Up(false); UI_Status_Mid(false); UI_Status_Down(true); }
                else { UI_Status_Up(false); UI_Status_Mid(true); UI_Status_Down(false); }
            }

        }
}