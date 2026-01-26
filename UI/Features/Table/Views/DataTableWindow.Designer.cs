namespace PrecisionPressureController.UI.Features.Table.Views
{
    partial class DataTableWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            dgvTable = new System.Windows.Forms.DataGridView();
            colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPressure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSetpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRampSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            panelTitleBar = new System.Windows.Forms.Panel();
            panelTitleLeft = new System.Windows.Forms.FlowLayoutPanel();
            lblIcon = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            panelConnectionStatus = new System.Windows.Forms.Panel();
            lblConnectionStatus = new System.Windows.Forms.Label();
            panelTitleRight = new System.Windows.Forms.FlowLayoutPanel();
            btnPause = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();
            btnTheme = new System.Windows.Forms.Button();
            panelToolbar = new System.Windows.Forms.Panel();
            panelToolbarLeft = new System.Windows.Forms.FlowLayoutPanel();
            btnTabAll = new System.Windows.Forms.Button();
            btnTabWithComments = new System.Windows.Forms.Button();
            btnTabSetpoints = new System.Windows.Forms.Button();
            panelToolbarRight = new System.Windows.Forms.FlowLayoutPanel();
            lblThreshold = new System.Windows.Forms.Label();
            txtThreshold = new System.Windows.Forms.TextBox();
            chkAutoScroll = new System.Windows.Forms.CheckBox();
            panelFooter = new System.Windows.Forms.Panel();
            lblFooterStats = new System.Windows.Forms.Label();
            lblFooterVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvTable).BeginInit();
            panelTitleBar.SuspendLayout();
            panelTitleLeft.SuspendLayout();
            panelTitleRight.SuspendLayout();
            panelToolbar.SuspendLayout();
            panelToolbarLeft.SuspendLayout();
            panelToolbarRight.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // dgvTable
            // 
            dgvTable.AllowUserToAddRows = false;
            dgvTable.AllowUserToDeleteRows = false;
            dgvTable.BackgroundColor = System.Drawing.Color.FromArgb(10, 14, 26);
            dgvTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colIndex,
            colTime,
            colPressure,
            colSetpoint,
            colRampSpeed,
            colStatus,
            colComment});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(10, 14, 26);
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(50, 60, 80);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvTable.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvTable.EnableHeadersVisualStyles = false;
            dgvTable.GridColor = System.Drawing.Color.FromArgb(30, 35, 50);
            dgvTable.Location = new System.Drawing.Point(0, 100);
            dgvTable.MultiSelect = true;
            dgvTable.Name = "dgvTable";
            dgvTable.ReadOnly = true;
            dgvTable.RowHeadersVisible = false;
            dgvTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTable.Size = new System.Drawing.Size(1200, 500);
            dgvTable.TabIndex = 0;
            // 
            // colIndex
            // 
            colIndex.DataPropertyName = "Index";
            colIndex.HeaderText = "#";
            colIndex.Name = "colIndex";
            colIndex.ReadOnly = true;
            colIndex.Width = 60;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            colIndex.DefaultCellStyle = dataGridViewCellStyle3;
            // 
            // colTime
            // 
            colTime.DataPropertyName = "Time";
            colTime.HeaderText = "TIME";
            colTime.Name = "colTime";
            colTime.ReadOnly = true;
            colTime.Width = 120;
            // 
            // colPressure
            // 
            colPressure.DataPropertyName = "Pressure";
            colPressure.HeaderText = "PRESSURE";
            colPressure.Name = "colPressure";
            colPressure.ReadOnly = true;
            colPressure.Width = 100;
            colPressure.DefaultCellStyle.Format = "F2";
            // 
            // colSetpoint
            // 
            colSetpoint.DataPropertyName = "Setpoint";
            colSetpoint.HeaderText = "SETPOINT";
            colSetpoint.Name = "colSetpoint";
            colSetpoint.ReadOnly = true;
            colSetpoint.Width = 100;
            colSetpoint.DefaultCellStyle.Format = "F2";
            // 
            // colRampSpeed
            // 
            colRampSpeed.DataPropertyName = "Rate";
            colRampSpeed.HeaderText = "RAMP SPEED";
            colRampSpeed.Name = "colRampSpeed";
            colRampSpeed.ReadOnly = true;
            colRampSpeed.Width = 120;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "STATUS";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 100;
            // 
            // colComment
            // 
            colComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colComment.DataPropertyName = "Comment";
            colComment.HeaderText = "COMMENT";
            colComment.Name = "colComment";
            colComment.ReadOnly = true;
            colComment.MinimumWidth = 200;
            // 
            // panelTitleBar
            // 
            panelTitleBar.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            panelTitleBar.Controls.Add(panelTitleLeft);
            panelTitleBar.Controls.Add(panelTitleRight);
            panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            panelTitleBar.Location = new System.Drawing.Point(0, 0);
            panelTitleBar.Name = "panelTitleBar";
            panelTitleBar.Size = new System.Drawing.Size(1200, 50);
            panelTitleBar.TabIndex = 1;
            // 
            // panelTitleLeft
            // 
            panelTitleLeft.AutoSize = true;
            panelTitleLeft.Controls.Add(lblIcon);
            panelTitleLeft.Controls.Add(lblTitle);
            panelTitleLeft.Controls.Add(panelConnectionStatus);
            panelTitleLeft.Dock = System.Windows.Forms.DockStyle.Left;
            panelTitleLeft.Location = new System.Drawing.Point(0, 0);
            panelTitleLeft.Margin = new System.Windows.Forms.Padding(0);
            panelTitleLeft.Name = "panelTitleLeft";
            panelTitleLeft.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            panelTitleLeft.Size = new System.Drawing.Size(400, 50);
            panelTitleLeft.TabIndex = 0;
            panelTitleLeft.WrapContents = false;
            // 
            // lblIcon
            // 
            lblIcon.AutoSize = true;
            lblIcon.Font = new System.Drawing.Font("Segoe UI", 14F);
            lblIcon.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            lblIcon.Location = new System.Drawing.Point(15, 13);
            lblIcon.Margin = new System.Windows.Forms.Padding(3, 13, 8, 0);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new System.Drawing.Size(28, 25);
            lblIcon.TabIndex = 0;
            lblIcon.Text = "📋";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            lblTitle.Location = new System.Drawing.Point(51, 15);
            lblTitle.Margin = new System.Windows.Forms.Padding(0, 15, 12, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(88, 21);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Data Table";
            // 
            // panelConnectionStatus
            // 
            panelConnectionStatus.BackColor = System.Drawing.Color.FromArgb(26, 61, 53);
            panelConnectionStatus.Location = new System.Drawing.Point(151, 11);
            panelConnectionStatus.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            panelConnectionStatus.Name = "panelConnectionStatus";
            panelConnectionStatus.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            panelConnectionStatus.Size = new System.Drawing.Size(200, 28);
            panelConnectionStatus.TabIndex = 2;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblConnectionStatus.ForeColor = System.Drawing.Color.White;
            lblConnectionStatus.Location = new System.Drawing.Point(20, 6);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new System.Drawing.Size(75, 15);
            lblConnectionStatus.TabIndex = 0;
            lblConnectionStatus.Text = "Disconnected";
            lblConnectionStatus.Visible = false;
            // 
            // panelTitleRight
            // 
            panelTitleRight.AutoSize = true;
            panelTitleRight.Controls.Add(btnPause);
            panelTitleRight.Controls.Add(btnClear);
            panelTitleRight.Controls.Add(btnExport);
            panelTitleRight.Controls.Add(btnTheme);
            panelTitleRight.Dock = System.Windows.Forms.DockStyle.Right;
            panelTitleRight.Location = new System.Drawing.Point(800, 0);
            panelTitleRight.Margin = new System.Windows.Forms.Padding(0);
            panelTitleRight.Name = "panelTitleRight";
            panelTitleRight.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            panelTitleRight.Size = new System.Drawing.Size(400, 50);
            panelTitleRight.TabIndex = 1;
            panelTitleRight.WrapContents = false;
            // 
            // btnPause
            // 
            btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPause.FlatAppearance.BorderSize = 0;
            btnPause.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            btnPause.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnPause.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnPause.Location = new System.Drawing.Point(210, 11);
            btnPause.Margin = new System.Windows.Forms.Padding(0, 11, 8, 0);
            btnPause.Name = "btnPause";
            btnPause.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            btnPause.Size = new System.Drawing.Size(75, 28);
            btnPause.TabIndex = 0;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += BtnPause_Click;
            // 
            // btnClear
            // 
            btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            btnClear.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnClear.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnClear.Location = new System.Drawing.Point(293, 11);
            btnClear.Margin = new System.Windows.Forms.Padding(0, 11, 8, 0);
            btnClear.Name = "btnClear";
            btnClear.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            btnClear.Size = new System.Drawing.Size(75, 28);
            btnClear.TabIndex = 1;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += BtnClear_Click;
            // 
            // btnExport
            // 
            btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            btnExport.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnExport.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnExport.Location = new System.Drawing.Point(376, 11);
            btnExport.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            btnExport.Name = "btnExport";
            btnExport.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            btnExport.Size = new System.Drawing.Size(75, 28);
            btnExport.TabIndex = 2;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += BtnExport_Click;
            // 
            // btnTheme
            // 
            btnTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTheme.FlatAppearance.BorderSize = 0;
            btnTheme.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            btnTheme.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnTheme.Font = new System.Drawing.Font("Segoe UI", 12F);
            btnTheme.Location = new System.Drawing.Point(451, 11);
            btnTheme.Margin = new System.Windows.Forms.Padding(0, 11, 8, 0);
            btnTheme.Name = "btnTheme";
            btnTheme.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            btnTheme.Size = new System.Drawing.Size(32, 28);
            btnTheme.TabIndex = 3;
            btnTheme.Text = "🌙";
            btnTheme.UseVisualStyleBackColor = false;
            btnTheme.Click += BtnTheme_Click;
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            panelToolbar.Controls.Add(panelToolbarLeft);
            panelToolbar.Controls.Add(panelToolbarRight);
            panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            panelToolbar.Location = new System.Drawing.Point(0, 50);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new System.Drawing.Size(1200, 50);
            panelToolbar.TabIndex = 2;
            // 
            // panelToolbarLeft
            // 
            panelToolbarLeft.AutoSize = true;
            panelToolbarLeft.Controls.Add(btnTabAll);
            panelToolbarLeft.Controls.Add(btnTabWithComments);
            panelToolbarLeft.Controls.Add(btnTabSetpoints);
            panelToolbarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            panelToolbarLeft.Location = new System.Drawing.Point(0, 0);
            panelToolbarLeft.Margin = new System.Windows.Forms.Padding(0);
            panelToolbarLeft.Name = "panelToolbarLeft";
            panelToolbarLeft.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            panelToolbarLeft.Size = new System.Drawing.Size(400, 50);
            panelToolbarLeft.TabIndex = 0;
            panelToolbarLeft.WrapContents = false;
            // 
            // btnTabAll
            // 
            btnTabAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTabAll.FlatAppearance.BorderSize = 0;
            btnTabAll.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnTabAll.ForeColor = System.Drawing.Color.White;
            btnTabAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTabAll.Location = new System.Drawing.Point(15, 13);
            btnTabAll.Margin = new System.Windows.Forms.Padding(3, 13, 8, 0);
            btnTabAll.Name = "btnTabAll";
            btnTabAll.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            btnTabAll.Size = new System.Drawing.Size(75, 28);
            btnTabAll.TabIndex = 0;
            btnTabAll.Text = "All";
            btnTabAll.UseVisualStyleBackColor = false;
            btnTabAll.Click += BtnTabAll_Click;
            // 
            // btnTabWithComments
            // 
            btnTabWithComments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTabWithComments.FlatAppearance.BorderSize = 0;
            btnTabWithComments.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            btnTabWithComments.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnTabWithComments.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTabWithComments.Location = new System.Drawing.Point(98, 13);
            btnTabWithComments.Margin = new System.Windows.Forms.Padding(0, 13, 8, 0);
            btnTabWithComments.Name = "btnTabWithComments";
            btnTabWithComments.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            btnTabWithComments.Size = new System.Drawing.Size(123, 28);
            btnTabWithComments.TabIndex = 1;
            btnTabWithComments.Text = "With Comments";
            btnTabWithComments.UseVisualStyleBackColor = false;
            btnTabWithComments.Click += BtnTabWithComments_Click;
            // 
            // btnTabSetpoints
            // 
            btnTabSetpoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTabSetpoints.FlatAppearance.BorderSize = 0;
            btnTabSetpoints.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            btnTabSetpoints.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnTabSetpoints.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTabSetpoints.Location = new System.Drawing.Point(229, 13);
            btnTabSetpoints.Margin = new System.Windows.Forms.Padding(0, 13, 0, 0);
            btnTabSetpoints.Name = "btnTabSetpoints";
            btnTabSetpoints.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            btnTabSetpoints.Size = new System.Drawing.Size(75, 28);
            btnTabSetpoints.TabIndex = 2;
            btnTabSetpoints.Text = "At Target";
            btnTabSetpoints.UseVisualStyleBackColor = false;
            btnTabSetpoints.Click += BtnTabSetpoints_Click;
            // 
            // panelToolbarRight
            // 
            panelToolbarRight.AutoSize = true;
            panelToolbarRight.Controls.Add(lblThreshold);
            panelToolbarRight.Controls.Add(txtThreshold);
            panelToolbarRight.Controls.Add(chkAutoScroll);
            panelToolbarRight.Dock = System.Windows.Forms.DockStyle.Right;
            panelToolbarRight.Location = new System.Drawing.Point(800, 0);
            panelToolbarRight.Margin = new System.Windows.Forms.Padding(0);
            panelToolbarRight.Name = "panelToolbarRight";
            panelToolbarRight.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            panelToolbarRight.Size = new System.Drawing.Size(400, 50);
            panelToolbarRight.TabIndex = 1;
            panelToolbarRight.WrapContents = false;
            // 
            // lblThreshold
            // 
            lblThreshold.AutoSize = true;
            lblThreshold.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblThreshold.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            lblThreshold.Location = new System.Drawing.Point(250, 17);
            lblThreshold.Margin = new System.Windows.Forms.Padding(0, 17, 8, 0);
            lblThreshold.Name = "lblThreshold";
            lblThreshold.Size = new Size(60, 15);
            lblThreshold.TabIndex = 0;
            lblThreshold.Text = "Threshold";
            // 
            // txtThreshold
            // 
            txtThreshold.BackColor = System.Drawing.Color.FromArgb(30, 40, 55);
            txtThreshold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtThreshold.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            txtThreshold.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtThreshold.Location = new System.Drawing.Point(318, 15);
            txtThreshold.Margin = new System.Windows.Forms.Padding(0, 15, 8, 0);
            txtThreshold.Name = "txtThreshold";
            txtThreshold.Size = new System.Drawing.Size(50, 23);
            txtThreshold.TabIndex = 1;
            txtThreshold.Text = "0.10";
            txtThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.Font = new System.Drawing.Font("Segoe UI", 9F);
            chkAutoScroll.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            chkAutoScroll.Location = new System.Drawing.Point(376, 17);
            chkAutoScroll.Margin = new System.Windows.Forms.Padding(0, 17, 0, 0);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new System.Drawing.Size(85, 19);
            chkAutoScroll.TabIndex = 2;
            chkAutoScroll.Text = "Auto-scroll";
            chkAutoScroll.UseVisualStyleBackColor = true;
            chkAutoScroll.Checked = true;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            panelFooter.Controls.Add(lblFooterStats);
            panelFooter.Controls.Add(lblFooterVersion);
            panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelFooter.Location = new System.Drawing.Point(0, 600);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new System.Drawing.Size(1200, 30);
            panelFooter.TabIndex = 3;
            // 
            // lblFooterStats
            // 
            lblFooterStats.AutoSize = true;
            lblFooterStats.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblFooterStats.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            lblFooterStats.Location = new System.Drawing.Point(12, 8);
            lblFooterStats.Name = "lblFooterStats";
            lblFooterStats.Size = new System.Drawing.Size(177, 15);
            lblFooterStats.TabIndex = 0;
            lblFooterStats.Text = "Total: 0  |  Filtered: 0  |  Selected: 0";
            // 
            // lblFooterVersion
            // 
            lblFooterVersion.AutoSize = true;
            lblFooterVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblFooterVersion.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            lblFooterVersion.Location = new System.Drawing.Point(1100, 8);
            lblFooterVersion.Name = "lblFooterVersion";
            lblFooterVersion.Size = new System.Drawing.Size(140, 15);
            lblFooterVersion.TabIndex = 1;
            lblFooterVersion.Text = "Precision Pressure Controller Data Table v1.0";
            // 
            // DataTableWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(10, 14, 26);
            ClientSize = new System.Drawing.Size(1200, 630);
            Controls.Add(dgvTable);
            Controls.Add(panelToolbar);
            Controls.Add(panelTitleBar);
            Controls.Add(panelFooter);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            MinimumSize = new System.Drawing.Size(800, 500);
            Name = "DataTableWindow";
            Text = "Data Table";
            ((System.ComponentModel.ISupportInitialize)dgvTable).EndInit();
            panelTitleBar.ResumeLayout(false);
            panelTitleBar.PerformLayout();
            panelTitleLeft.ResumeLayout(false);
            panelTitleLeft.PerformLayout();
            panelConnectionStatus.ResumeLayout(false);
            panelConnectionStatus.PerformLayout();
            panelTitleRight.ResumeLayout(false);
            panelToolbar.ResumeLayout(false);
            panelToolbar.PerformLayout();
            panelToolbarLeft.ResumeLayout(false);
            panelToolbarRight.ResumeLayout(false);
            panelToolbarRight.PerformLayout();
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPressure;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetpoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRampSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.FlowLayoutPanel panelTitleLeft;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelConnectionStatus;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.FlowLayoutPanel panelTitleRight;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.FlowLayoutPanel panelToolbarLeft;
        private System.Windows.Forms.Button btnTabAll;
        private System.Windows.Forms.Button btnTabWithComments;
        private System.Windows.Forms.Button btnTabSetpoints;
        private System.Windows.Forms.FlowLayoutPanel panelToolbarRight;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblFooterStats;
        private System.Windows.Forms.Label lblFooterVersion;
    }
}
