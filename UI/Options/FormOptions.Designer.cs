using System.Drawing;
using System.Windows.Forms;

namespace Alicat
{
    partial class FormOptions
    {
        private System.ComponentModel.IContainer components = null;

        // TabControl
        private TabControl tabControl;
        private TabPage tabUnits;
        private TabPage tabLimits;

        // Units Tab
        private TableLayoutPanel unitsGrid;
        private Label lblPressureUnits;
        private ComboBox cmbPressureUnits;
        private Label lblTimeUnits;
        private ComboBox cmbTimeUnits;
        private Label lblPressureRamp;
        private TextBox txtPressureRamp;

        // Limits Tab
        private TableLayoutPanel limitsGrid;
        private Label lblMaxPressure;
        private TextBox txtMaxPressure;
        private Label lblMinPressure;
        private TextBox txtMinPressure;
        private Label lblMaxIncrement;
        private TextBox txtMaxIncrement;
        private Label lblMinIncrement;
        private TextBox txtMinIncrement;
        private Panel safetyNoticePanel;
        private Label lblSafetyNotice;

        // Bottom buttons
        private Panel bottomHost;
        private FlowLayoutPanel btnRow;
        private Button btnOK;
        private Button btnCancel;
        private Button btnApply;
        private Button btnRestoreDefaults;

        private ToolTip toolTips;

        // Modern color palette
        private static readonly Color ModernBg = Color.FromArgb(250, 250, 250);
        private static readonly Color ModernBorder = Color.FromArgb(220, 220, 220);
        private static readonly Color ModernText = Color.FromArgb(51, 51, 51);
        private static readonly Color ModernTextMuted = Color.FromArgb(120, 120, 120);
        private static readonly Color ModernAccent = Color.FromArgb(0, 102, 170);
        private static readonly Color ModernAccentHover = Color.FromArgb(0, 85, 150);
        private static readonly Color SafetyYellow = Color.FromArgb(255, 243, 205);
        private static readonly Color SafetyYellowBorder = Color.FromArgb(255, 193, 7);
        private static readonly Color SafetyText = Color.FromArgb(133, 88, 0);

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // -------- Form --------
            this.SuspendLayout();
            this.Text = "Preferences";
            this.ClientSize = new Size(520, 380);
            this.MinimumSize = new Size(480, 340);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ModernBg;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // -------- ToolTips --------
            toolTips = new ToolTip();
            toolTips.IsBalloon = false;
            toolTips.ToolTipTitle = "";

            // -------- Bottom host (ВСЕГДА внизу) --------
            bottomHost = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(20, 12, 20, 12),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(bottomHost);

            // -------- Bottom buttons row --------
            btnRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            // Restore Defaults button (secondary style)
            btnRestoreDefaults = new Button
            {
                Text = "Restore defaults",
                Size = new Size(130, 32),
                Margin = new Padding(0, 0, 16, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnRestoreDefaults.FlatAppearance.BorderColor = ModernBorder;
            btnRestoreDefaults.FlatAppearance.BorderSize = 1;
            btnRestoreDefaults.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);
            toolTips.SetToolTip(btnRestoreDefaults, "Reset all fields to factory defaults.");

            // OK button (primary style)
            btnOK = new Button
            {
                Text = "OK",
                Size = new Size(90, 32),
                Margin = new Padding(0, 0, 8, 0),
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat,
                BackColor = ModernAccent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseOverBackColor = ModernAccentHover;

            // Cancel button (secondary style)
            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(90, 32),
                Margin = new Padding(0, 0, 8, 0),
                DialogResult = DialogResult.Cancel,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = ModernBorder;
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);

            // Apply button (secondary style)
            btnApply = new Button
            {
                Text = "Apply",
                Size = new Size(90, 32),
                Margin = new Padding(0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnApply.FlatAppearance.BorderColor = ModernBorder;
            btnApply.FlatAppearance.BorderSize = 1;
            btnApply.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);

            btnRow.Controls.Add(btnRestoreDefaults);
            btnRow.Controls.Add(btnOK);
            btnRow.Controls.Add(btnCancel);
            btnRow.Controls.Add(btnApply);
            bottomHost.Controls.Add(btnRow);

            // -------- TabControl --------
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(0, 0),
                Appearance = TabAppearance.Normal,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += (sender, e) =>
            {
                var tab = tabControl.TabPages[e.Index];
                var rect = tabControl.GetTabRect(e.Index);
                var isSelected = tabControl.SelectedIndex == e.Index;

                e.Graphics.FillRectangle(
                    new SolidBrush(isSelected ? Color.White : ModernBg),
                    rect);

                TextRenderer.DrawText(
                    e.Graphics,
                    tab.Text,
                    new Font("Segoe UI", 9F, FontStyle.Regular),
                    rect,
                    isSelected ? ModernText : ModernTextMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            // ======== Units Tab ========
            tabUnits = new TabPage
            {
                Text = "Units",
                Padding = new Padding(24, 20, 24, 20),
                BackColor = Color.White,
                UseVisualStyleBackColor = false
            };

            unitsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 3,
                AutoSize = true,
                Padding = new Padding(0),
                BackColor = Color.White
            };
            unitsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f)); // labels
            unitsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // inputs

            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));

            // Pressure Units
            lblPressureUnits = new Label
            {
                Text = "Pressure Units",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            cmbPressureUnits = new ComboBox
            {
                Width = 200,
                Height = 26,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            cmbPressureUnits.Items.AddRange(new object[] 
            { 
                "PSI", "PSIG", "PSF", "PSFG",
                "Pa", "hPa", "kPa", "MPa",
                "mbar", "bar",
                "g/cm²", "kg/cm",
                "mTorr", "torr"
            });
            toolTips.SetToolTip(cmbPressureUnits, "Select pressure measurement units.");

            // Time / Speed Units
            lblTimeUnits = new Label
            {
                Text = "Time / Speed Units",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            cmbTimeUnits = new ComboBox
            {
                Width = 200,
                Height = 26,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            cmbTimeUnits.Items.AddRange(new object[] { "ms", "s", "m", "h" });
            toolTips.SetToolTip(cmbTimeUnits, "Select time or speed measurement units.");

            // Pressure Ramp
            lblPressureRamp = new Label
            {
                Text = "Ramp Speed",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            txtPressureRamp = new TextBox
            {
                Width = 200,
                Height = 26,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtPressureRamp.BorderStyle = BorderStyle.FixedSingle;
#if NET6_0_OR_GREATER
            txtPressureRamp.PlaceholderText = "e.g. 10.0";
#endif
            toolTips.SetToolTip(txtPressureRamp, "Rate of setpoint change per time unit.");

            unitsGrid.Controls.Add(lblPressureUnits, 0, 0);
            unitsGrid.Controls.Add(cmbPressureUnits, 1, 0);
            unitsGrid.Controls.Add(lblTimeUnits, 0, 1);
            unitsGrid.Controls.Add(cmbTimeUnits, 1, 1);
            unitsGrid.Controls.Add(lblPressureRamp, 0, 2);
            unitsGrid.Controls.Add(txtPressureRamp, 1, 2);

            tabUnits.Controls.Add(unitsGrid);

            // ======== Limits Tab ========
            tabLimits = new TabPage
            {
                Text = "Limits",
                Padding = new Padding(24, 20, 24, 20),
                BackColor = Color.White,
                UseVisualStyleBackColor = false
            };

            limitsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 4,
                AutoSize = true,
                Padding = new Padding(0),
                BackColor = Color.White
            };
            limitsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f)); // labels
            limitsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // inputs

            limitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            limitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            limitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            limitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));

            // Maximum Pressure
            lblMaxPressure = new Label
            {
                Text = "Maximum Pressure",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            txtMaxPressure = new TextBox
            {
                Width = 200,
                Height = 26,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
#if NET6_0_OR_GREATER
            txtMaxPressure.PlaceholderText = "e.g. 200";
#endif
            toolTips.SetToolTip(txtMaxPressure, "Maximum pressure allowed (device limit).");

            // Minimum Pressure
            lblMinPressure = new Label
            {
                Text = "Minimum Pressure",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            txtMinPressure = new TextBox
            {
                Width = 200,
                Height = 26,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
#if NET6_0_OR_GREATER
            txtMinPressure.PlaceholderText = "e.g. 0";
#endif
            toolTips.SetToolTip(txtMinPressure, "Minimum pressure allowed.");

            // Maximum Step (Increment)
            lblMaxIncrement = new Label
            {
                Text = "Maximum Step",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            txtMaxIncrement = new TextBox
            {
                Width = 200,
                Height = 26,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
#if NET6_0_OR_GREATER
            txtMaxIncrement.PlaceholderText = "e.g. 20";
#endif
            toolTips.SetToolTip(txtMaxIncrement, "Maximum increment step.");

            // Minimum Step (Increment)
            lblMinIncrement = new Label
            {
                Text = "Minimum Step",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            txtMinIncrement = new TextBox
            {
                Width = 200,
                Height = 26,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
#if NET6_0_OR_GREATER
            txtMinIncrement.PlaceholderText = "e.g. 0.1";
#endif
            toolTips.SetToolTip(txtMinIncrement, "Minimum increment step.");

            limitsGrid.Controls.Add(lblMaxPressure, 0, 0);
            limitsGrid.Controls.Add(txtMaxPressure, 1, 0);
            limitsGrid.Controls.Add(lblMinPressure, 0, 1);
            limitsGrid.Controls.Add(txtMinPressure, 1, 1);
            limitsGrid.Controls.Add(lblMaxIncrement, 0, 2);
            limitsGrid.Controls.Add(txtMaxIncrement, 1, 2);
            limitsGrid.Controls.Add(lblMinIncrement, 0, 3);
            limitsGrid.Controls.Add(txtMinIncrement, 1, 3);

            // Safety Notice Panel (modern warning banner)
            safetyNoticePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                Padding = new Padding(14, 12, 14, 12),
                BackColor = SafetyYellow,
                Margin = new Padding(0, 0, 0, 16),
                BorderStyle = BorderStyle.None
            };
            lblSafetyNotice = new Label
            {
                Text = "⚠ Safety Notice: Ensure pressure limits are set correctly to prevent equipment damage or personal injury.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = SafetyText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                AutoSize = false
            };
            safetyNoticePanel.Controls.Add(lblSafetyNotice);
            safetyNoticePanel.Paint += (sender, e) =>
            {
                var rect = safetyNoticePanel.ClientRectangle;
                using (var pen = new Pen(SafetyYellowBorder, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
                }
            };

            tabLimits.Controls.Add(safetyNoticePanel);
            tabLimits.Controls.Add(limitsGrid);

            // Add tabs to TabControl
            tabControl.TabPages.Add(tabUnits);
            tabControl.TabPages.Add(tabLimits);

            this.Controls.Add(tabControl);

            this.ResumeLayout(false);
        }
    }
}
