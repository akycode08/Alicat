using System.Drawing;
using System.Windows.Forms;

namespace Alicat
{
    partial class FormOptions
    {
        private System.ComponentModel.IContainer components = null;

        // Main content panel (all fields on one page)
        private TableLayoutPanel mainGrid;
        private Label lblPressureUnits;
        private ComboBox cmbPressureUnits;
        private Label lblTimeUnits;
        private ComboBox cmbTimeUnits;
        private Label lblPollingFrequency;
        private ComboBox cmbPollingFrequency;
        private Label lblPressureRamp;
        private TextBox txtPressureRamp;
        private Label lblMaxPressure;
        private TextBox txtMaxPressure;
        private Label lblMinPressure;
        private TextBox txtMinPressure;
        private Label lblMaxIncrement;
        private TextBox txtMaxIncrement;
        private Label lblMinIncrement;
        private TextBox txtMinIncrement;

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

            // -------- Main content panel (all fields on one page) --------
            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = Color.White,
                AutoScroll = true
            };

            mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 8,
                AutoSize = true,
                Padding = new Padding(0),
                BackColor = Color.White
            };
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f)); // labels
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // inputs

            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));

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

            // Polling Frequency
            lblPollingFrequency = new Label
            {
                Text = "Polling Frequency",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                ForeColor = ModernText,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            cmbPollingFrequency = new ComboBox
            {
                Width = 200,
                Height = 26,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            cmbPollingFrequency.Items.AddRange(new object[] { "10ms", "50ms", "100ms", "250ms", "500ms", "1000ms", "2000ms", "5000ms" });
            toolTips.SetToolTip(cmbPollingFrequency, "Frequency of device polling in milliseconds.");

            // Pressure Units
            mainGrid.Controls.Add(lblPressureUnits, 0, 0);
            mainGrid.Controls.Add(cmbPressureUnits, 1, 0);
            
            // Time / Speed Units
            mainGrid.Controls.Add(lblTimeUnits, 0, 1);
            mainGrid.Controls.Add(cmbTimeUnits, 1, 1);
            
            // Polling Frequency
            mainGrid.Controls.Add(lblPollingFrequency, 0, 2);
            mainGrid.Controls.Add(cmbPollingFrequency, 1, 2);

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
            mainGrid.Controls.Add(lblPressureRamp, 0, 3);
            mainGrid.Controls.Add(txtPressureRamp, 1, 3);
            mainGrid.Controls.Add(lblMaxPressure, 0, 4);
            mainGrid.Controls.Add(txtMaxPressure, 1, 4);
            mainGrid.Controls.Add(lblMinPressure, 0, 5);
            mainGrid.Controls.Add(txtMinPressure, 1, 5);

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
            mainGrid.Controls.Add(lblMaxIncrement, 0, 6);
            mainGrid.Controls.Add(txtMaxIncrement, 1, 6);

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
            mainGrid.Controls.Add(lblMinIncrement, 0, 7);
            mainGrid.Controls.Add(txtMinIncrement, 1, 7);

            contentPanel.Controls.Add(mainGrid);
            this.Controls.Add(contentPanel);

            this.ResumeLayout(false);
        }
    }
}
