using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat.UI.Features.Graph.Views
{
    partial class SequenceEditorModal
    {
        private IContainer components = null;

        private TableLayoutPanel mainPanel;
        private Label lblTitle;
        private DataGridView dgvPoints;
        private Panel panelAddPoint;
        private Label lblAddPoint;
        private TableLayoutPanel tlpAddPoint;
        private Label lblAddPSI;
        private TextBox txtAddPSI;
        private Label lblAddHold;
        private TextBox txtAddHold;
        private Button btnAddPoint;
        private Panel panelQuickImport;
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
            this.ClientSize = new Size(550, 450);
            this.MinimumSize = new Size(500, 400);
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
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

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
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
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

            // Add Point section (сверху)
            panelAddPoint = new Panel
            {
                Name = "panelAddPoint",
                Dock = DockStyle.Fill
            };

            lblAddPoint = new Label
            {
                Name = "lblAddPoint",
                Text = "ADD POINT",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 20,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };
            panelAddPoint.Controls.Add(lblAddPoint);

            tlpAddPoint = new TableLayoutPanel
            {
                Name = "tlpAddPoint",
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                Padding = new Padding(0, 5, 0, 0)
            };
            tlpAddPoint.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpAddPoint.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpAddPoint.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpAddPoint.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpAddPoint.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            lblAddPSI = new Label
            {
                Name = "lblAddPSI",
                Text = "PSI:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };
            tlpAddPoint.Controls.Add(lblAddPSI, 0, 0);

            txtAddPSI = new TextBox
            {
                Name = "txtAddPSI",
                Text = "0",
                Dock = DockStyle.Fill
            };
            tlpAddPoint.Controls.Add(txtAddPSI, 1, 0);

            lblAddHold = new Label
            {
                Name = "lblAddHold",
                Text = "Hold (min):",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };
            tlpAddPoint.Controls.Add(lblAddHold, 2, 0);

            txtAddHold = new TextBox
            {
                Name = "txtAddHold",
                Text = "0",
                Dock = DockStyle.Fill
            };
            tlpAddPoint.Controls.Add(txtAddHold, 3, 0);

            btnAddPoint = new Button
            {
                Name = "btnAddPoint",
                Text = "+ Add",
                Dock = DockStyle.Fill
            };
            btnAddPoint.Click += BtnAddPoint_Click;
            tlpAddPoint.Controls.Add(btnAddPoint, 4, 0);

            panelAddPoint.Controls.Add(tlpAddPoint);
            mainPanel.Controls.Add(panelAddPoint, 0, 2);

            // Quick Import section (снизу)
            panelQuickImport = new Panel
            {
                Name = "panelQuickImport",
                Dock = DockStyle.Fill
            };

            lblQuickImport = new Label
            {
                Name = "lblQuickImport",
                Text = "QUICK IMPORT (PSI:MIN, PSI:MIN, ...)",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 20,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };
            panelQuickImport.Controls.Add(lblQuickImport);

            tlpQuickImport = new TableLayoutPanel
            {
                Name = "tlpQuickImport",
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0, 5, 0, 0)
            };
            tlpQuickImport.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tlpQuickImport.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            txtQuickImport = new TextBox
            {
                Name = "txtQuickImport",
                Text = "",
                Dock = DockStyle.Fill
            };
            tlpQuickImport.Controls.Add(txtQuickImport, 0, 0);

            btnParseAndAdd = new Button
            {
                Name = "btnParseAndAdd",
                Text = "Parse & Add",
                Dock = DockStyle.Fill
            };
            btnParseAndAdd.Click += BtnParseAndAdd_Click;
            tlpQuickImport.Controls.Add(btnParseAndAdd, 1, 0);

            panelQuickImport.Controls.Add(tlpQuickImport);
            mainPanel.Controls.Add(panelQuickImport, 0, 3);

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

