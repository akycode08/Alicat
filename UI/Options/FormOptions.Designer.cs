using System.Drawing;
using System.Windows.Forms;

namespace Alicat
{
    partial class FormOptions
    {
        private System.ComponentModel.IContainer components = null;

        // Root
        private TableLayoutPanel root;

        // Header
        private TableLayoutPanel header;
        private Button btnUnits;
        private Button btnSave;
        private Button btnGraph;
        private Button btnFuture;

        // Content
        private Panel contentHost;
        private Panel panelUnits;

        // Units grid
        private TableLayoutPanel unitsGrid;
        private Label lblPressureUnits;
        private ComboBox cmbPressureUnits;

        private Label lblTimeUnits;
        private ComboBox cmbTimeUnits;

        private Label lblMaxPressure;
        private TextBox txtMaxPressure;

        private Label lblPressureRamp;
        private TextBox txtPressureRamp;

        private Label lblMaxIncrement;
        private TextBox txtMaxIncrement;

        // Under-grid row (restore)
        private Panel restoreRow;
        private FlowLayoutPanel restoreRight;
        private Button btnRestoreDefaults;

        // Bottom (отдельный хост + ряд кнопок)
        private Panel bottomHost;
        private FlowLayoutPanel btnRow;
        private Button btnOK;
        private Button btnCancel;
        private Button btnApply;

        private ToolTip toolTips;

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
            this.Text = "Options";
            this.ClientSize = new Size(640, 480);
            this.MinimumSize = new Size(560, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Control;

            // -------- ToolTips --------
            toolTips = new ToolTip();

            // -------- Bottom host (ВСЕГДА внизу, не в root) --------
            bottomHost = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                Padding = new Padding(12, 10, 12, 10),
                BackColor = SystemColors.Control
            };
            this.Controls.Add(bottomHost);

            btnRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            btnOK = new Button { Text = "OK", AutoSize = true, Margin = new Padding(6, 0, 6, 0) };
            btnCancel = new Button { Text = "Cancel", AutoSize = true, Margin = new Padding(6, 0, 6, 0) };
            btnApply = new Button { Text = "Apply", AutoSize = true, Margin = new Padding(6, 0, 6, 0) };

            btnRow.Controls.Add(btnOK);
            btnRow.Controls.Add(btnCancel);
            btnRow.Controls.Add(btnApply);
            bottomHost.Controls.Add(btnRow);

            // -------- Root layout (заполняет остальное) --------
            root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,              // header + content (без bottom строки!)
                Padding = new Padding(0),
                BackColor = SystemColors.Control
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56f));   // Header
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));   // Content
            this.Controls.Add(root);

            // -------- Header (4 равные кнопки) --------
            header = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(12, 8, 12, 8),
                BackColor = SystemColors.Control
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            btnUnits = new Button
            {
                Text = "Units",
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 4, 6, 4),
                AutoSize = false
            };
            btnSave = new Button
            {
                Text = "Save",
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 4, 6, 4),
                AutoSize = false
            };
            btnGraph = new Button
            {
                Text = "Graph",
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 4, 6, 4),
                AutoSize = false
            };
            btnFuture = new Button
            {
                Text = "—",
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 4, 6, 4),
                AutoSize = false
            };

            // Активная вкладка Units — визуально выделена
            btnUnits.Font = new Font(btnUnits.Font, FontStyle.Bold);
            btnUnits.UseVisualStyleBackColor = false;
            btnUnits.BackColor = SystemColors.ControlLight;

            header.Controls.Add(btnUnits, 0, 0);
            header.Controls.Add(btnSave, 1, 0);
            header.Controls.Add(btnGraph, 2, 0);
            header.Controls.Add(btnFuture, 3, 0);
            root.Controls.Add(header, 0, 0);

            // -------- Content host --------
            contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = SystemColors.Control
            };
            root.Controls.Add(contentHost, 0, 1);

            // ======== Units panel ========
            panelUnits = new Panel { Dock = DockStyle.Fill, BackColor = SystemColors.Control };

            // Grid: labels left, inputs right
            unitsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 5,
                AutoSize = true,
                Padding = new Padding(4),
                BackColor = SystemColors.Control
            };
            unitsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180f)); // labels
            unitsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // inputs

            // 5 строк по 36 px (без циклов)
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            unitsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));

            // ---- Pressure Units
            lblPressureUnits = new Label
            {
                Text = "Pressure Units",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 8, 0)
            };
            cmbPressureUnits = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 6, 0, 6)
            };
            cmbPressureUnits.Items.AddRange(new object[] { "PSI", "BAR" });
            toolTips.SetToolTip(cmbPressureUnits, "Select pressure measurement units.");

            // ---- Time / Speed Units
            lblTimeUnits = new Label
            {
                Text = "Time / Speed Units",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 8, 0)
            };
            cmbTimeUnits = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 6, 0, 6)
            };
            cmbTimeUnits.Items.AddRange(new object[] { "ms", "s", "m", "h" });
            toolTips.SetToolTip(cmbTimeUnits, "Select time or speed measurement units.");

            // ---- Max Pressure
            lblMaxPressure = new Label
            {
                Text = "Max Pressure",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 8, 0)
            };
            txtMaxPressure = new TextBox
            {
                Dock = DockStyle.Left,
                Width = 160,
                Margin = new Padding(0, 6, 0, 6)
#if NET6_0_OR_GREATER
                ,
                PlaceholderText = "e.g. 100"
#endif
            };
            toolTips.SetToolTip(txtMaxPressure, "Maximum pressure allowed (device limit).");

            // ---- Pressure Ramp
            lblPressureRamp = new Label
            {
                Text = "Pressure Ramp",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 8, 0)
            };
            txtPressureRamp = new TextBox
            {
                Dock = DockStyle.Left,
                Width = 160,
                Margin = new Padding(0, 6, 0, 6)
#if NET6_0_OR_GREATER
                ,
                PlaceholderText = "e.g. 5"
#endif
            };
            toolTips.SetToolTip(txtPressureRamp, "Rate of setpoint change per time unit.");

            // ---- Max Increment
            lblMaxIncrement = new Label
            {
                Text = "Max Increment",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 8, 0)
            };
            txtMaxIncrement = new TextBox
            {
                Dock = DockStyle.Left,
                Width = 160,
                Margin = new Padding(0, 6, 0, 6)
#if NET6_0_OR_GREATER
                ,
                PlaceholderText = "e.g. 1"
#endif
            };
            toolTips.SetToolTip(txtMaxIncrement, "Maximum manual increment step.");

            // Размещение в сетке
            unitsGrid.Controls.Add(lblPressureUnits, 0, 0);
            unitsGrid.Controls.Add(cmbPressureUnits, 1, 0);

            unitsGrid.Controls.Add(lblTimeUnits, 0, 1);
            unitsGrid.Controls.Add(cmbTimeUnits, 1, 1);

            unitsGrid.Controls.Add(lblMaxPressure, 0, 2);
            unitsGrid.Controls.Add(txtMaxPressure, 1, 2);

            unitsGrid.Controls.Add(lblPressureRamp, 0, 3);
            unitsGrid.Controls.Add(txtPressureRamp, 1, 3);

            unitsGrid.Controls.Add(lblMaxIncrement, 0, 4);
            unitsGrid.Controls.Add(txtMaxIncrement, 1, 4);

            // -------- Restore defaults row (компактно справа) --------
            restoreRow = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = SystemColors.Control
            };
            btnRestoreDefaults = new Button
            {
                Text = "Restore defaults",
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 0)
            };
            restoreRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            restoreRight.Controls.Add(btnRestoreDefaults);
            restoreRow.Controls.Add(restoreRight);
            toolTips.SetToolTip(btnRestoreDefaults, "Reset all fields to factory defaults.");

            // Добавляем в Units панель
            panelUnits.Controls.Add(restoreRow);
            panelUnits.Controls.Add(unitsGrid);

            contentHost.Controls.Add(panelUnits);

            this.ResumeLayout(false);
        }
    }
}
