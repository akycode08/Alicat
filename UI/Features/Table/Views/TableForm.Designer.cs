namespace Alicat.UI.Features.Table.Views
{
    partial class TableForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvLog = new DataGridView();
            colTime = new DataGridViewTextBoxColumn();
            colPressure = new DataGridViewTextBoxColumn();
            colSetpoint = new DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();
            panelTop = new Panel();
            panelRight = new Panel();
            btnExport = new Button();
            btnClear = new Button();
            btnPause = new Button();
            panelLeft = new Panel();
            chkAutoScroll = new CheckBox();
            numThreshold = new NumericUpDown();
            lblThreshold = new Label();
            btnTabSetpoints = new Button();
            btnTabWithComments = new Button();
            btnTabAll = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLog).BeginInit();
            panelTop.SuspendLayout();
            panelRight.SuspendLayout();
            panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
            SuspendLayout();
            // 
            // dgvLog
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLog.Columns.AddRange(new DataGridViewColumn[] { colTime, colPressure, colSetpoint, colComment });
            dgvLog.Dock = DockStyle.Fill;
            dgvLog.EnableHeadersVisualStyles = false;
            dgvLog.Location = new Point(0, 50);
            dgvLog.MultiSelect = false;
            dgvLog.Name = "dgvLog";
            dgvLog.RowHeadersVisible = false;
            dgvLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLog.Size = new Size(796, 301);
            dgvLog.TabIndex = 0;
            dgvLog.CellContentClick += dgvLog_CellContentClick;
            // 
            // colTime
            // 
            colTime.HeaderText = "Time";
            colTime.Name = "colTime";
            colTime.Width = 120;
            colTime.ReadOnly = true;
            // 
            // colPressure
            // 
            colPressure.HeaderText = "Pressure";
            colPressure.Name = "colPressure";
            colPressure.Width = 120;
            colTime.ReadOnly = true;
            // 
            // colSetpoint
            // 
            colSetpoint.HeaderText = "Setpoint";
            colSetpoint.Name = "colSetpoint";
            colSetpoint.Width = 120;
            colTime.ReadOnly = true;
            // 
            // colComment
            // 
            colComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            colComment.DefaultCellStyle = dataGridViewCellStyle2;
            colComment.HeaderText = "Comment";
            colComment.Name = "colComment";
            // 
            // panelTop
            // 
            panelTop.BackColor = SystemColors.ControlDarkDark;
            panelTop.Controls.Add(panelRight);
            panelTop.Controls.Add(panelLeft);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(796, 50);
            panelTop.TabIndex = 1;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(btnExport);
            panelRight.Controls.Add(btnClear);
            panelRight.Controls.Add(btnPause);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(496, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(300, 50);
            panelRight.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Location = new Point(204, 12);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(75, 23);
            btnExport.TabIndex = 2;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Location = new Point(112, 12);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 1;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            btnPause.FlatStyle = FlatStyle.Flat;
            btnPause.Location = new Point(21, 12);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(75, 23);
            btnPause.TabIndex = 0;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(chkAutoScroll);
            panelLeft.Controls.Add(numThreshold);
            panelLeft.Controls.Add(lblThreshold);
            panelLeft.Controls.Add(btnTabSetpoints);
            panelLeft.Controls.Add(btnTabWithComments);
            panelLeft.Controls.Add(btnTabAll);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(500, 50);
            panelLeft.TabIndex = 0;
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.ForeColor = Color.WhiteSmoke;
            chkAutoScroll.Location = new Point(408, 16);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new Size(85, 19);
            chkAutoScroll.TabIndex = 5;
            chkAutoScroll.Text = "Auto-scroll";
            chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // numThreshold
            // 
            numThreshold.DecimalPlaces = 2;
            numThreshold.Increment = new decimal(new int[] { 10, 0, 0, 131072 });
            numThreshold.Location = new Point(347, 12);
            numThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numThreshold.Name = "numThreshold";
            numThreshold.Size = new Size(50, 23);
            numThreshold.TabIndex = 4;
            numThreshold.Value = new decimal(new int[] { 10, 0, 0, 131072 });
            // 
            // lblThreshold
            // 
            lblThreshold.AutoSize = true;
            lblThreshold.ForeColor = Color.WhiteSmoke;
            lblThreshold.Location = new Point(294, 16);
            lblThreshold.Name = "lblThreshold";
            lblThreshold.Size = new Size(60, 15);
            lblThreshold.TabIndex = 3;
            lblThreshold.Text = "Threshold";
            // 
            // btnTabSetpoints
            // 
            btnTabSetpoints.FlatStyle = FlatStyle.Flat;
            btnTabSetpoints.ForeColor = Color.WhiteSmoke;
            btnTabSetpoints.Location = new Point(213, 12);
            btnTabSetpoints.Name = "btnTabSetpoints";
            btnTabSetpoints.Size = new Size(75, 23);
            btnTabSetpoints.TabIndex = 2;
            btnTabSetpoints.Text = "Setpoints";
            btnTabSetpoints.UseVisualStyleBackColor = true;
            btnTabSetpoints.Click += btnTabSetpoints_Click;
            // 
            // btnTabWithComments
            // 
            btnTabWithComments.FlatStyle = FlatStyle.Flat;
            btnTabWithComments.ForeColor = Color.WhiteSmoke;
            btnTabWithComments.Location = new Point(84, 12);
            btnTabWithComments.Name = "btnTabWithComments";
            btnTabWithComments.Size = new Size(123, 23);
            btnTabWithComments.TabIndex = 1;
            btnTabWithComments.Text = "With Comments";
            btnTabWithComments.UseVisualStyleBackColor = true;
            btnTabWithComments.Click += btnTabWidthComments_Click;
            // 
            // btnTabAll
            // 
            btnTabAll.FlatStyle = FlatStyle.Flat;
            btnTabAll.ForeColor = Color.WhiteSmoke;
            btnTabAll.Location = new Point(3, 12);
            btnTabAll.Name = "btnTabAll";
            btnTabAll.Size = new Size(75, 23);
            btnTabAll.TabIndex = 0;
            btnTabAll.Text = "All";
            btnTabAll.UseVisualStyleBackColor = true;
            btnTabAll.Click += btnTabAll_Click;
            // 
            // TableForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 351);
            Controls.Add(dgvLog);
            Controls.Add(panelTop);
            Name = "TableForm";
            Text = "м";
            ((System.ComponentModel.ISupportInitialize)dgvLog).EndInit();
            panelTop.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numThreshold).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvLog;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewTextBoxColumn colPressure;
        private DataGridViewTextBoxColumn colSetpoint;
        private DataGridViewTextBoxColumn colComment;
        private Panel panelTop;
        private Panel panelRight;
        private Panel panelLeft;
        private Button btnTabSetpoints;
        private Button btnTabWithComments;
        private Button btnTabAll;
        private NumericUpDown numThreshold;
        private Label lblThreshold;
        private Button btnClear;
        private Button btnPause;
        private CheckBox chkAutoScroll;
        private Button btnExport;
    }
}