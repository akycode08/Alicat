using System.Drawing;
using System.Windows.Forms;

namespace PrecisionPressureController.UI.Features.Test
{
    partial class TestPressureWindow
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private TableLayoutPanel mainLayout;
        private GroupBox grpPressure;
        private TableLayoutPanel pressureLayout;
        private Label lblCurrentPressure;
        private Label lblCurrentPressureValue;
        private Label lblSetPressure;
        private TextBox txtPressure;
        private Button btnSetPressure;

        private GroupBox grpRampSpeed;
        private TableLayoutPanel rampLayout;
        private Label lblCurrentRampSpeed;
        private Label lblCurrentRampSpeedValue;
        private Label lblSetRampSpeed;
        private TextBox txtRampSpeed;
        private Button btnSetRampSpeed;

        private Label lblStatus;

        // Modern color palette
        private static readonly Color ModernBg = Color.FromArgb(250, 250, 250);
        private static readonly Color ModernBorder = Color.FromArgb(220, 220, 220);
        private static readonly Color ModernText = Color.FromArgb(51, 51, 51);
        private static readonly Color ModernAccent = Color.FromArgb(0, 102, 170);
        private static readonly Color ModernAccentHover = Color.FromArgb(0, 85, 150);

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
            this.Text = "Тестовое окно: Давление и Скорость";
            this.ClientSize = new Size(450, 400);
            this.MinimumSize = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ModernBg;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Padding = new Padding(20);

            // -------- Main Layout --------
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                AutoSize = true,
                Padding = new Padding(0)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // -------- Pressure Group --------
            grpPressure = new GroupBox
            {
                Text = "Давление",
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 12, 16, 16),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ModernText
            };

            pressureLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                AutoSize = true
            };
            pressureLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f));
            pressureLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pressureLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f));

            lblCurrentPressure = new Label
            {
                Text = "Текущее:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = ModernText
            };

            lblCurrentPressureValue = new Label
            {
                Text = "---",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ModernAccent
            };

            lblSetPressure = new Label
            {
                Text = "Set Target:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = ModernText
            };

            txtPressure = new TextBox
            {
                Height = 26,
                Margin = new Padding(0, 4, 8, 4),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
#if NET6_0_OR_GREATER
            txtPressure.PlaceholderText = "Enter pressure";
#endif

            btnSetPressure = new Button
            {
                Text = "🎯 Go to Target",
                Height = 32,
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 8, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSetPressure.FlatAppearance.BorderSize = 0;
            btnSetPressure.FlatAppearance.MouseOverBackColor = Color.FromArgb(69, 160, 73);

            pressureLayout.Controls.Add(lblCurrentPressure, 0, 0);
            pressureLayout.Controls.Add(lblCurrentPressureValue, 1, 0);
            pressureLayout.SetColumnSpan(lblCurrentPressureValue, 2);
            pressureLayout.Controls.Add(lblSetPressure, 0, 1);
            pressureLayout.Controls.Add(txtPressure, 1, 1);
            pressureLayout.Controls.Add(btnSetPressure, 2, 1);

            grpPressure.Controls.Add(pressureLayout);

            // -------- Ramp Speed Group --------
            grpRampSpeed = new GroupBox
            {
                Text = "Скорость давления",
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 12, 16, 16),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ModernText
            };

            rampLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                AutoSize = true
            };
            rampLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f));
            rampLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            rampLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f));

            lblCurrentRampSpeed = new Label
            {
                Text = "Текущая:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = ModernText
            };

            lblCurrentRampSpeedValue = new Label
            {
                Text = "---",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ModernAccent
            };

            lblSetRampSpeed = new Label
            {
                Text = "Set Ramp Speed:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = ModernText
            };

            txtRampSpeed = new TextBox
            {
                Height = 26,
                Margin = new Padding(0, 4, 8, 4),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
#if NET6_0_OR_GREATER
            txtRampSpeed.PlaceholderText = "Enter ramp speed";
#endif

            btnSetRampSpeed = new Button
            {
                Text = "⚡ Go to Target",
                Height = 32,
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSetRampSpeed.FlatAppearance.BorderSize = 0;
            btnSetRampSpeed.FlatAppearance.MouseOverBackColor = Color.FromArgb(69, 160, 73);

            rampLayout.Controls.Add(lblCurrentRampSpeed, 0, 0);
            rampLayout.Controls.Add(lblCurrentRampSpeedValue, 1, 0);
            rampLayout.SetColumnSpan(lblCurrentRampSpeedValue, 2);
            rampLayout.Controls.Add(lblSetRampSpeed, 0, 1);
            rampLayout.Controls.Add(txtRampSpeed, 1, 1);
            rampLayout.Controls.Add(btnSetRampSpeed, 2, 1);

            grpRampSpeed.Controls.Add(rampLayout);

            // -------- Status Label --------
            lblStatus = new Label
            {
                Text = "Готово",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Height = 24,
                Margin = new Padding(0, 12, 0, 0),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = ModernText
            };

            // Add to main layout
            mainLayout.Controls.Add(grpPressure, 0, 0);
            mainLayout.Controls.Add(grpRampSpeed, 0, 1);
            mainLayout.Controls.Add(lblStatus, 0, 2);

            this.Controls.Add(mainLayout);

            this.ResumeLayout(false);
        }
    }
}

