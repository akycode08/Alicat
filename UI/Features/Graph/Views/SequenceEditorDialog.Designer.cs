using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PrecisionPressureController.UI.Features.Graph.Views
{
    partial class SequenceEditorDialog
    {
        private IContainer components = null;

        private TableLayoutPanel mainPanel;
        private Label lblTitle;
        private DataGridView dgvPoints;
        private Panel addPointPanel;
        private Label lblPSI;
        private NumericUpDown numPSI;
        private Label lblHold;
        private NumericUpDown numHold;
        private Button btnAdd;
        private Panel quickImportPanel;
        private Label lblQuick;
        private TextBox txtQuick;
        private Button btnParse;
        private Panel bottomPanel;
        private FlowLayoutPanel btnRow;
        private Button btnClearAll;
        private Button btnDone;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Sequence Editor";
            this.ClientSize = new Size(550, 550);
            this.MinimumSize = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main panel
            mainPanel = new TableLayoutPanel
            {
                Name = "mainPanel",
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 1,
                RowCount = 4
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Add Point section
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Quick Import section

            // Title
            lblTitle = new Label
            {
                Name = "lblTitle",
                Text = "Sequence Editor",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(lblTitle, 0, 0);

            // DataGridView
            dgvPoints = new DataGridView
            {
                Name = "dgvPoints",
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.Fixed3D,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                EnableHeadersVisualStyles = true,
                GridColor = SystemColors.ControlDark,
                MultiSelect = false,
                ReadOnly = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            mainPanel.Controls.Add(dgvPoints, 0, 1);

            // === ADD POINT SECTION ===
            addPointPanel = new Panel
            {
                Name = "addPointPanel",
                Dock = DockStyle.Fill,
                Height = 50
            };
            
            lblPSI = new Label
            {
                Name = "lblPSI",
                Text = "PSI:",
                Location = new Point(20, 13),
                AutoSize = true
            };
            addPointPanel.Controls.Add(lblPSI);

            numPSI = new NumericUpDown
            {
                Name = "numPSI",
                Location = new Point(50, 11),
                Size = new Size(80, 25),
                Maximum = 1000,
                DecimalPlaces = 1
            };
            addPointPanel.Controls.Add(numPSI);

            lblHold = new Label
            {
                Name = "lblHold",
                Text = "Hold (min):",
                Location = new Point(140, 13),
                AutoSize = true
            };
            addPointPanel.Controls.Add(lblHold);

            numHold = new NumericUpDown
            {
                Name = "numHold",
                Location = new Point(220, 11),
                Size = new Size(60, 25),
                Maximum = 999,
                DecimalPlaces = 0
            };
            addPointPanel.Controls.Add(numHold);

            btnAdd = new Button
            {
                Name = "btnAdd",
                Text = "+ Add",
                Location = new Point(300, 9),
                Size = new Size(80, 28)
            };
            btnAdd.Click += BtnAdd_Click;
            addPointPanel.Controls.Add(btnAdd);

            mainPanel.Controls.Add(addPointPanel, 0, 2);

            // === QUICK IMPORT SECTION ===
            quickImportPanel = new Panel
            {
                Name = "quickImportPanel",
                Dock = DockStyle.Fill,
                Height = 50
            };

            lblQuick = new Label
            {
                Name = "lblQuick",
                Text = "Quick (PSI:MIN, ...):",
                Location = new Point(20, 13),
                AutoSize = true
            };
            quickImportPanel.Controls.Add(lblQuick);

            txtQuick = new TextBox
            {
                Name = "txtQuick",
                Location = new Point(140, 11),
                Size = new Size(200, 25)
            };
            quickImportPanel.Controls.Add(txtQuick);

            btnParse = new Button
            {
                Name = "btnParse",
                Text = "Parse",
                Location = new Point(350, 9),
                Size = new Size(70, 28)
            };
            btnParse.Click += BtnParse_Click;
            quickImportPanel.Controls.Add(btnParse);

            mainPanel.Controls.Add(quickImportPanel, 0, 3);

            this.Controls.Add(mainPanel);

            // Bottom panel
            bottomPanel = new Panel
            {
                Name = "bottomPanel",
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(20, 12, 20, 12)
            };
            this.Controls.Add(bottomPanel);

            btnRow = new FlowLayoutPanel
            {
                Name = "btnRow",
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            btnClearAll = new Button
            {
                Name = "btnClearAll",
                Text = "Clear All",
                Size = new Size(110, 32),
                Margin = new Padding(0, 0, 12, 0)
            };
            btnClearAll.Click += BtnClearAll_Click;

            btnDone = new Button
            {
                Name = "btnDone",
                Text = "Done",
                Size = new Size(90, 32),
                Margin = new Padding(0, 0, 8, 0),
                DialogResult = DialogResult.OK
            };
            btnDone.Click += BtnDone_Click;

            btnCancel = new Button
            {
                Name = "btnCancel",
                Text = "Cancel",
                Size = new Size(90, 32),
                Margin = new Padding(0),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += BtnCancel_Click;

            btnRow.Controls.Add(btnClearAll);
            btnRow.Controls.Add(btnDone);
            btnRow.Controls.Add(btnCancel);
            bottomPanel.Controls.Add(btnRow);

            this.ResumeLayout(false);
        }
    }
}

