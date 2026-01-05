namespace Alicat.UI.Features.Graph.Views
{
    partial class SequenceEditorModal
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel mainPanel;
        private Label lblTitle;
        private DataGridView dgvPoints;
        private Label lblAddPoint;
        private TableLayoutPanel tlpAddPoint;
        private Label lblAddPSI;
        private TextBox txtAddPSI;
        private Label lblAddHold;
        private TextBox txtAddHold;
        private Button btnAddPoint;
        private Label lblQuickImport;
        private TableLayoutPanel tlpQuickImport;
        private TextBox txtQuickImport;
        private Button btnParseAndAdd;
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
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main panel
            mainPanel = new TableLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(20),
                ColumnCount = 1,
                RowCount = 5
            };
            mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));

            // Title
            lblTitle = new Label
            {
                Text = "Sequence Editor",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                Dock = System.Windows.Forms.DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                // --text-primary: #e4e7eb
                ForeColor = System.Drawing.Color.FromArgb(228, 231, 235)
            };
            mainPanel.Controls.Add(lblTitle, 0, 0);

            // DataGridView
            dgvPoints = new DataGridView
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None,
                // --bg-panel: #111827
                BackgroundColor = System.Drawing.Color.FromArgb(17, 24, 39),
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                EnableHeadersVisualStyles = false,
                // --border-control: #374151
                GridColor = System.Drawing.Color.FromArgb(55, 65, 81),
                MultiSelect = false,
                ReadOnly = false,
                RowHeadersVisible = false,
                SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            };
            mainPanel.Controls.Add(dgvPoints, 0, 1);

            // Add Point section
            lblAddPoint = new Label
            {
                Text = "ADD POINT",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Dock = System.Windows.Forms.DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175)
            };
            mainPanel.Controls.Add(lblAddPoint, 0, 2);

            tlpAddPoint = new TableLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1
            };
            tlpAddPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tlpAddPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tlpAddPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            tlpAddPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tlpAddPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));

            lblAddPSI = new Label
            {
                Text = "PSI:",
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = false,
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175)
            };
            tlpAddPoint.Controls.Add(lblAddPSI, 0, 0);

            txtAddPSI = new TextBox
            {
                Text = "0",
                Dock = System.Windows.Forms.DockStyle.Fill,
                // --bg-input: #1a1f2e
                BackColor = System.Drawing.Color.FromArgb(26, 31, 46),
                // --text-primary: #e4e7eb
                ForeColor = System.Drawing.Color.FromArgb(228, 231, 235),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            tlpAddPoint.Controls.Add(txtAddPSI, 1, 0);

            lblAddHold = new Label
            {
                Text = "Hold (min):",
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = false,
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175)
            };
            tlpAddPoint.Controls.Add(lblAddHold, 2, 0);

            txtAddHold = new TextBox
            {
                Text = "0",
                Dock = System.Windows.Forms.DockStyle.Fill,
                // --bg-input: #1a1f2e
                BackColor = System.Drawing.Color.FromArgb(26, 31, 46),
                // --text-primary: #e4e7eb
                ForeColor = System.Drawing.Color.FromArgb(228, 231, 235),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            tlpAddPoint.Controls.Add(txtAddHold, 3, 0);

            btnAddPoint = new Button
            {
                Text = "+ Add",
                Dock = System.Windows.Forms.DockStyle.Fill,
                // --accent-green: #10b981 (btn-primary)
                BackColor = System.Drawing.Color.FromArgb(16, 185, 129),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnAddPoint.Click += BtnAddPoint_Click;
            tlpAddPoint.Controls.Add(btnAddPoint, 4, 0);

            mainPanel.Controls.Add(tlpAddPoint, 0, 2);

            // Quick Import section
            lblQuickImport = new Label
            {
                Text = "QUICK IMPORT (PSI:MIN, PSI:MIN, ...)",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Dock = System.Windows.Forms.DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175)
            };
            mainPanel.Controls.Add(lblQuickImport, 0, 3);

            tlpQuickImport = new TableLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            tlpQuickImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            tlpQuickImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));

            txtQuickImport = new TextBox
            {
                Text = "",
                Dock = System.Windows.Forms.DockStyle.Fill,
                // --bg-input: #1a1f2e
                BackColor = System.Drawing.Color.FromArgb(26, 31, 46),
                // --text-primary: #e4e7eb
                ForeColor = System.Drawing.Color.FromArgb(228, 231, 235),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            tlpQuickImport.Controls.Add(txtQuickImport, 0, 0);

            btnParseAndAdd = new Button
            {
                Text = "Parse & Add",
                Dock = System.Windows.Forms.DockStyle.Fill,
                // --bg-button: #1f2937
                BackColor = System.Drawing.Color.FromArgb(31, 41, 55),
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                // --border-control: #374151
                FlatAppearance = { BorderColor = System.Drawing.Color.FromArgb(55, 65, 81), BorderSize = 1 },
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnParseAndAdd.Click += BtnParseAndAdd_Click;
            tlpQuickImport.Controls.Add(btnParseAndAdd, 1, 0);

            mainPanel.Controls.Add(tlpQuickImport, 0, 3);

            this.Controls.Add(mainPanel);

            // Bottom panel
            bottomPanel = new Panel
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 60,
                Padding = new System.Windows.Forms.Padding(20, 12, 20, 12),
                // --bg-panel: #111827
                BackColor = System.Drawing.Color.FromArgb(17, 24, 39)
            };
            this.Controls.Add(bottomPanel);

            btnRow = new FlowLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Right,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new System.Windows.Forms.Padding(0),
                Margin = new System.Windows.Forms.Padding(0)
            };

            btnClearAll = new Button
            {
                Text = "🗑 Clear All",
                Size = new System.Drawing.Size(110, 32),
                Margin = new System.Windows.Forms.Padding(0, 0, 12, 0),
                // --bg-button: #1f2937
                BackColor = System.Drawing.Color.FromArgb(31, 41, 55),
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                // --border-control: #374151
                FlatAppearance = { BorderColor = System.Drawing.Color.FromArgb(55, 65, 81), BorderSize = 1 },
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnClearAll.Click += BtnClearAll_Click;

            btnDone = new Button
            {
                Text = "✓ Done",
                Size = new System.Drawing.Size(90, 32),
                Margin = new System.Windows.Forms.Padding(0, 0, 8, 0),
                // --accent-green: #10b981 (btn-primary)
                BackColor = System.Drawing.Color.FromArgb(16, 185, 129),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnDone.Click += BtnDone_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new System.Drawing.Size(90, 32),
                Margin = new System.Windows.Forms.Padding(0),
                // --bg-button: #1f2937
                BackColor = System.Drawing.Color.FromArgb(31, 41, 55),
                // --text-secondary: #9ca3af
                ForeColor = System.Drawing.Color.FromArgb(156, 163, 175),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                // --border-control: #374151
                FlatAppearance = { BorderColor = System.Drawing.Color.FromArgb(55, 65, 81), BorderSize = 1 },
                Cursor = System.Windows.Forms.Cursors.Hand
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

