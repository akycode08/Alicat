using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;
using Alicat.UI.Features.Statistics.Views;
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Terminal.Views;


namespace Alicat
{
    public partial class AlicatForm : Form
        {
        private void btnGraph_Click(object? sender, EventArgs e)
        {
            // если окна ещё нет или оно было закрыто — создаём новое
            if (_graphForm == null || _graphForm.IsDisposed)
            {
                _graphForm = new GraphForm(_dataStore);
                _graphForm.Show(this); // делаем AlicatForm владельцем
            }
            else
            {
                // если уже открыто — просто выводим на передний план
                if (_graphForm.WindowState == FormWindowState.Minimized)
                    _graphForm.WindowState = FormWindowState.Normal;

                _graphForm.Focus();
            }
        }

        private void btnTable_Click(object? sender, EventArgs e)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
            {
                _tableForm = new TableForm(_dataStore);
                _tableForm.StartPosition = FormStartPosition.CenterParent;
                _tableForm.Show(this);   // не ShowDialog, окно живёт, пока не закроешь
            }
            else
            {
                if (_tableForm.WindowState == FormWindowState.Minimized)
                    _tableForm.WindowState = FormWindowState.Normal;

                _tableForm.Focus();
            }
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            var form = new StatisticsForm();
            form.Show(this);
        }

        private void btnTerminal_Click(object? sender, EventArgs e)
        {
            if (_terminalForm == null || _terminalForm.IsDisposed)
            {
                _terminalForm = new TerminalForm();
                _terminalForm.CommandSent += TerminalForm_CommandSent;
            }

            _terminalForm.Show(this);
            _terminalForm.Focus();
        }

        private void TerminalForm_CommandSent(string cmd)
        {
            if (_serial == null)
            {
                _terminalForm?.AppendLog("!! Serial not connected");
                return;
            }

            try
            {
                _serial.Send(cmd);   // если метод называется иначе – подправь
            }
            catch (Exception ex)
            {
                _terminalForm?.AppendLog("!! Error: " + ex.Message);
            }
        }






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