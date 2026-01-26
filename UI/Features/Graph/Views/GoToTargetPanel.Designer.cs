namespace PrecisionPressureController.UI.Features.Graph.Views
{
    partial class GoToTargetPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.dgvPoints = new System.Windows.Forms.DataGridView();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPSI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPSI = new System.Windows.Forms.Label();
            this.nudPSI = new System.Windows.Forms.NumericUpDown();
            this.lblHold = new System.Windows.Forms.Label();
            this.nudHold = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblProgressValue = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblHoldTimer = new System.Windows.Forms.Label();
            this.lblHoldValue = new System.Windows.Forms.Label();
            this.holdProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPSI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHold)).BeginInit();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            this.grpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.grpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.grpMain.Location = new System.Drawing.Point(0, 0);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(240, 420);
            this.grpMain.TabIndex = 0;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "GO TO TARGET";
            // 
            // dgvPoints
            // 
            this.dgvPoints.AllowUserToAddRows = false;
            this.dgvPoints.AllowUserToResizeRows = false;
            this.dgvPoints.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.dgvPoints.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPoints.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPoints.ColumnHeadersHeight = 25;
            this.dgvPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colPSI,
            this.colHold,
            this.colStatus});
            this.dgvPoints.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.dgvPoints.Location = new System.Drawing.Point(8, 22);
            this.dgvPoints.MultiSelect = false;
            this.dgvPoints.Name = "dgvPoints";
            this.dgvPoints.ReadOnly = true;
            this.dgvPoints.RowHeadersVisible = false;
            this.dgvPoints.RowTemplate.Height = 24;
            this.dgvPoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPoints.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvPoints.Size = new System.Drawing.Size(224, 145);
            this.dgvPoints.TabIndex = 1;
            this.dgvPoints.EnableHeadersVisualStyles = false;
            // Header style
            this.dgvPoints.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.dgvPoints.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.dgvPoints.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPoints.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            // Cell style
            this.dgvPoints.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.dgvPoints.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dgvPoints.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.dgvPoints.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dgvPoints.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPoints.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            // Alternating rows
            this.dgvPoints.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(45)))));
            // 
            // colNum
            // 
            this.colNum.HeaderText = "#";
            this.colNum.Name = "colNum";
            this.colNum.ReadOnly = true;
            this.colNum.Width = 30;
            // 
            // colPSI
            // 
            this.colPSI.HeaderText = "PSI";
            this.colPSI.Name = "colPSI";
            this.colPSI.ReadOnly = true;
            this.colPSI.Width = 65;
            // 
            // colHold
            // 
            this.colHold.HeaderText = "Hold";
            this.colHold.Name = "colHold";
            this.colHold.ReadOnly = true;
            this.colHold.Width = 55;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 55;
            // 
            // lblPSI
            // 
            this.lblPSI.AutoSize = true;
            this.lblPSI.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPSI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblPSI.Location = new System.Drawing.Point(8, 175);
            this.lblPSI.Name = "lblPSI";
            this.lblPSI.Size = new System.Drawing.Size(26, 13);
            this.lblPSI.TabIndex = 2;
            this.lblPSI.Text = "PSI:";
            // 
            // nudPSI
            // 
            this.nudPSI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            this.nudPSI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudPSI.DecimalPlaces = 1;
            this.nudPSI.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudPSI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.nudPSI.Location = new System.Drawing.Point(8, 192);
            this.nudPSI.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudPSI.Name = "nudPSI";
            this.nudPSI.Size = new System.Drawing.Size(105, 23);
            this.nudPSI.TabIndex = 3;
            this.nudPSI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblHold
            // 
            this.lblHold.AutoSize = true;
            this.lblHold.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHold.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblHold.Location = new System.Drawing.Point(125, 175);
            this.lblHold.Name = "lblHold";
            this.lblHold.Size = new System.Drawing.Size(62, 13);
            this.lblHold.TabIndex = 4;
            this.lblHold.Text = "Hold (min):";
            // 
            // nudHold
            // 
            this.nudHold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            this.nudHold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudHold.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHold.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.nudHold.Location = new System.Drawing.Point(125, 192);
            this.nudHold.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudHold.Name = "nudHold";
            this.nudHold.Size = new System.Drawing.Size(105, 23);
            this.nudHold.TabIndex = 5;
            this.nudHold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(8, 225);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(222, 30);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "+ Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnClearAll
            // 
            this.btnClearAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnClearAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnClearAll.Location = new System.Drawing.Point(8, 262);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(107, 28);
            this.btnClearAll.TabIndex = 7;
            this.btnClearAll.Text = "🗑 Clear All";
            this.btnClearAll.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnEdit.Location = new System.Drawing.Point(123, 262);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(107, 28);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "⚙ Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblProgress.Location = new System.Drawing.Point(8, 300);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(51, 13);
            this.lblProgress.TabIndex = 9;
            this.lblProgress.Text = "Progress:";
            // 
            // lblProgressValue
            // 
            this.lblProgressValue.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.lblProgressValue.Location = new System.Drawing.Point(170, 300);
            this.lblProgressValue.Name = "lblProgressValue";
            this.lblProgressValue.Size = new System.Drawing.Size(60, 13);
            this.lblProgressValue.TabIndex = 10;
            this.lblProgressValue.Text = "0/0";
            this.lblProgressValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.progressBar.Location = new System.Drawing.Point(8, 318);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(222, 6);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            // 
            // lblHoldTimer
            // 
            this.lblHoldTimer.AutoSize = true;
            this.lblHoldTimer.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoldTimer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblHoldTimer.Location = new System.Drawing.Point(8, 335);
            this.lblHoldTimer.Name = "lblHoldTimer";
            this.lblHoldTimer.Size = new System.Drawing.Size(32, 13);
            this.lblHoldTimer.TabIndex = 12;
            this.lblHoldTimer.Text = "Hold:";
            // 
            // lblHoldValue
            // 
            this.lblHoldValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoldValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.lblHoldValue.Location = new System.Drawing.Point(160, 333);
            this.lblHoldValue.Name = "lblHoldValue";
            this.lblHoldValue.Size = new System.Drawing.Size(70, 20);
            this.lblHoldValue.TabIndex = 13;
            this.lblHoldValue.Text = "00:00";
            this.lblHoldValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // holdProgressBar
            // 
            this.holdProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.holdProgressBar.Location = new System.Drawing.Point(8, 353);
            this.holdProgressBar.Name = "holdProgressBar";
            this.holdProgressBar.Size = new System.Drawing.Size(222, 6);
            this.holdProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.holdProgressBar.TabIndex = 14;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(8, 370);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(52, 35);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "▶";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnPause.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnPause.Location = new System.Drawing.Point(64, 370);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(52, 35);
            this.btnPause.TabIndex = 16;
            this.btnPause.Text = "⏸";
            this.btnPause.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnStop.Location = new System.Drawing.Point(120, 370);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(52, 35);
            this.btnStop.TabIndex = 17;
            this.btnStop.Text = "⏹";
            this.btnStop.UseVisualStyleBackColor = false;
            // 
            // btnSkip
            // 
            this.btnSkip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnSkip.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSkip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnSkip.Location = new System.Drawing.Point(176, 370);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(52, 35);
            this.btnSkip.TabIndex = 18;
            this.btnSkip.Text = "⏭";
            this.btnSkip.UseVisualStyleBackColor = false;
            // 
            // GoToTargetPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.Controls.Add(this.grpMain);
            this.grpMain.Controls.Add(this.dgvPoints);
            this.grpMain.Controls.Add(this.lblPSI);
            this.grpMain.Controls.Add(this.nudPSI);
            this.grpMain.Controls.Add(this.lblHold);
            this.grpMain.Controls.Add(this.nudHold);
            this.grpMain.Controls.Add(this.btnAdd);
            this.grpMain.Controls.Add(this.btnClearAll);
            this.grpMain.Controls.Add(this.btnEdit);
            this.grpMain.Controls.Add(this.lblProgress);
            this.grpMain.Controls.Add(this.lblProgressValue);
            this.grpMain.Controls.Add(this.progressBar);
            this.grpMain.Controls.Add(this.lblHoldTimer);
            this.grpMain.Controls.Add(this.lblHoldValue);
            this.grpMain.Controls.Add(this.holdProgressBar);
            this.grpMain.Controls.Add(this.btnStart);
            this.grpMain.Controls.Add(this.btnPause);
            this.grpMain.Controls.Add(this.btnStop);
            this.grpMain.Controls.Add(this.btnSkip);
            this.Name = "GoToTargetPanel";
            this.Size = new System.Drawing.Size(240, 420);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPSI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHold)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpMain;
        private System.Windows.Forms.DataGridView dgvPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPSI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Label lblPSI;
        private System.Windows.Forms.NumericUpDown nudPSI;
        private System.Windows.Forms.Label lblHold;
        private System.Windows.Forms.NumericUpDown nudHold;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblProgressValue;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblHoldTimer;
        private System.Windows.Forms.Label lblHoldValue;
        private System.Windows.Forms.ProgressBar holdProgressBar;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSkip;
    }
}

