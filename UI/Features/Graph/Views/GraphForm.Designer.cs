namespace Alicat.UI.Features.Graph.Views
{
    partial class GraphForm
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
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            chartPressure = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            panelLeft = new Panel();
            tlpLeft = new TableLayoutPanel();
            pnlLiveStatus = new Panel();
            lblCurrentPressureLarge = new Label();
            lblCurrentUnit = new Label();
            lblTargetLabel = new Label();
            lblTargetValue = new Label();
            pnlLiveStatusDivider = new Panel();
            lblETALabel = new Label();
            lblETAValue = new Label();
            lblTarget = new Label();
            lblETA = new Label();
            tlpSessionStats = new TableLayoutPanel();
            lblSessionStatsTitle = new Label();
            lblMinLabel = new Label();
            lblMinValue = new Label();
            lblMaxLabel = new Label();
            lblMaxValue = new Label();
            lblAvgLabel = new Label();
            lblAvgValue = new Label();
            lblPointsLabel = new Label();
            lblPointsValue = new Label();
            lblDurationLabel = new Label();
            lblDurationValue = new Label();
            tlpGoToTarget = new TableLayoutPanel();
            lblGoToTargetTitle = new Label();
            dgvTargets = new DataGridView();
            tlpTargetInputs = new TableLayoutPanel();
            lblPSILabel = new Label();
            txtPSI = new TextBox();
            lblHoldLabel = new Label();
            txtHold = new TextBox();
            btnAddTarget = new Button();
            tlpButtonsRow = new FlowLayoutPanel();
            btnClearAll = new Button();
            btnEdit = new Button();
            lblProgress = new Label();
            progressBarProgress = new ProgressBar();
            lblHoldTimer = new Label();
            progressBarHold = new ProgressBar();
            tlpControlButtons = new FlowLayoutPanel();
            btnPlay = new Button();
            btnPauseTarget = new Button();
            btnStop = new Button();
            btnSkip = new Button();
            btnEmergency = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblDelta = new Label();
            lblRate = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblTrend = new Label();
            pnlWarnIndicator = new Panel();
            lblStdDevLabel = new Label();
            lblStdDevValue = new Label();
            lblSampleRateLabel = new Label();
            lblSampleRateValue = new Label();
            panelRight = new Panel();
            tableSettings = new TableLayoutPanel();
            tlpTimeWindow = new TableLayoutPanel();
            lblTimeWindowTitle = new Label();
            lblDuration = new Label();
            cmbDuration = new ComboBox();
            tlpGrid = new TableLayoutPanel();
            lblGridTitle = new Label();
            lblYStep = new Label();
            cmbYStep = new ComboBox();
            tlpThresholds = new TableLayoutPanel();
            lblThresholdsTitle = new Label();
            lblMaximum = new Label();
            txtMaxThreshold = new TextBox();
            panelMaxIndicator = new Panel();
            lblMinimum = new Label();
            txtMinThreshold = new TextBox();
            panelMinIndicator = new Panel();
            tlpDisplay = new TableLayoutPanel();
            lblDisplayTitle = new Label();
            chkShowGrid = new CheckBox();
            chkShowTarget = new CheckBox();
            chkShowMax = new CheckBox();
            chkShowMin = new CheckBox();
            tlpAlerts = new TableLayoutPanel();
            lblAlertsTitle = new Label();
            panelAlertsIcon = new Panel();
            chkSound = new CheckBox();
            chkAtTarget = new CheckBox();
            chkAtMax = new CheckBox();
            panelBottom = new Panel();
            footerLayout = new TableLayoutPanel();
            lblAutoSaveStatus = new Label();
            lblFooterMin = new Label();
            lblFooterAvg = new Label();
            lblFooterPoints = new Label();
            lblThemeIndicator = new Label();
            panelHeader = new Panel();
            headerLayoutPanel = new TableLayoutPanel();
            headerLeftFlowPanel = new FlowLayoutPanel();
            appIcon = new Panel();
            lblAppTitle = new Label();
            lblSessionTime = new Label();
            headerRightFlowPanel = new FlowLayoutPanel();
            btnPause = new Button();
            btnExport = new Button();
            panelChartHeader = new Panel();
            panelChartButtons = new FlowLayoutPanel();
            flowLegend = new FlowLayoutPanel();
            lblLegendCurrent = new Label();
            lblLegendTarget = new Label();
            lblLegendMin = new Label();
            lblLegendMax = new Label();
            btnChartReset = new Button();
            btnFullscreen = new Button();
            panelCenter = new Panel();
            lblComPort = new Label();
            lblHotkeys = new Label();
            btnReset = new Button();
            btnFullscreenHeader = new Button();
            lblFooterMax = new Label();
            panelLeft.SuspendLayout();
            tlpLeft.SuspendLayout();
            pnlLiveStatus.SuspendLayout();
            tlpSessionStats.SuspendLayout();
            tlpGoToTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTargets).BeginInit();
            tlpTargetInputs.SuspendLayout();
            tlpControlButtons.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panelRight.SuspendLayout();
            tableSettings.SuspendLayout();
            tlpTimeWindow.SuspendLayout();
            tlpGrid.SuspendLayout();
            tlpThresholds.SuspendLayout();
            tlpDisplay.SuspendLayout();
            tlpAlerts.SuspendLayout();
            panelBottom.SuspendLayout();
            footerLayout.SuspendLayout();
            panelHeader.SuspendLayout();
            headerLayoutPanel.SuspendLayout();
            headerLeftFlowPanel.SuspendLayout();
            headerRightFlowPanel.SuspendLayout();
            panelChartHeader.SuspendLayout();
            flowLegend.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // chartPressure
            // 
            chartPressure.BackColor = Color.FromArgb(21, 23, 28);
            chartPressure.Dock = DockStyle.Fill;
            chartPressure.Location = new Point(0, 0);
            chartPressure.Margin = new Padding(0);
            chartPressure.Name = "chartPressure";
            chartPressure.Size = new Size(910, 710);
            chartPressure.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(21, 23, 28);
            panelLeft.Controls.Add(tlpLeft);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 50);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(260, 770);
            panelLeft.TabIndex = 1;
            // 
            // tlpLeft
            // 
            tlpLeft.ColumnCount = 1;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.Controls.Add(pnlLiveStatus, 0, 1);
            tlpLeft.Controls.Add(tlpSessionStats, 0, 2);
            tlpLeft.Controls.Add(tlpGoToTarget, 0, 3);
            tlpLeft.Controls.Add(btnEmergency, 0, 4);
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.Location = new Point(0, 0);
            tlpLeft.Name = "tlpLeft";
            tlpLeft.RowCount = 5;
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 2F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpLeft.Size = new Size(189, 704);
            tlpLeft.TabIndex = 0;
            // 
            // pnlLiveStatus
            // 
            pnlLiveStatus.BackColor = Color.FromArgb(32, 35, 44);
            pnlLiveStatus.Controls.Add(lblCurrentPressureLarge);
            pnlLiveStatus.Controls.Add(lblCurrentUnit);
            pnlLiveStatus.Controls.Add(lblTargetLabel);
            pnlLiveStatus.Controls.Add(lblTargetValue);
            pnlLiveStatus.Controls.Add(pnlLiveStatusDivider);
            pnlLiveStatus.Controls.Add(lblETALabel);
            pnlLiveStatus.Controls.Add(lblETAValue);
            pnlLiveStatus.Dock = DockStyle.Fill;
            pnlLiveStatus.Location = new Point(3, 17);
            pnlLiveStatus.Name = "pnlLiveStatus";
            pnlLiveStatus.Size = new Size(183, 140);
            pnlLiveStatus.TabIndex = 0;
            pnlLiveStatus.Paint += PnlLiveStatus_Paint;
            // 
            // lblCurrentPressureLarge
            // 
            lblCurrentPressureLarge.BackColor = Color.Transparent;
            lblCurrentPressureLarge.Font = new Font("Consolas", 32F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentPressureLarge.ForeColor = Color.FromArgb(228, 231, 235);
            lblCurrentPressureLarge.Location = new Point(0, 25);
            lblCurrentPressureLarge.Name = "lblCurrentPressureLarge";
            lblCurrentPressureLarge.Size = new Size(183, 45);
            lblCurrentPressureLarge.TabIndex = 0;
            lblCurrentPressureLarge.Text = "0.00";
            lblCurrentPressureLarge.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentPressureLarge.Click += lblCurrentPressureLarge_Click;
            // 
            // lblCurrentUnit
            // 
            lblCurrentUnit.BackColor = Color.Transparent;
            lblCurrentUnit.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCurrentUnit.ForeColor = Color.FromArgb(107, 114, 128);
            lblCurrentUnit.Location = new Point(0, 70);
            lblCurrentUnit.Name = "lblCurrentUnit";
            lblCurrentUnit.Size = new Size(183, 20);
            lblCurrentUnit.TabIndex = 1;
            lblCurrentUnit.Text = "PSIG";
            lblCurrentUnit.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentUnit.Click += lblCurrentUnit_Click;
            // 
            // lblTargetLabel
            // 
            lblTargetLabel.BackColor = Color.Transparent;
            lblTargetLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTargetLabel.ForeColor = Color.FromArgb(107, 114, 128);
            lblTargetLabel.Location = new Point(10, 98);
            lblTargetLabel.Name = "lblTargetLabel";
            lblTargetLabel.Size = new Size(50, 18);
            lblTargetLabel.TabIndex = 2;
            lblTargetLabel.Text = "Target:";
            lblTargetLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTargetValue
            // 
            lblTargetValue.BackColor = Color.Transparent;
            lblTargetValue.Font = new Font("Consolas", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTargetValue.ForeColor = Color.FromArgb(245, 158, 11);
            lblTargetValue.Location = new Point(60, 98);
            lblTargetValue.Name = "lblTargetValue";
            lblTargetValue.Size = new Size(113, 18);
            lblTargetValue.TabIndex = 3;
            lblTargetValue.Text = "--";
            lblTargetValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlLiveStatusDivider
            // 
            pnlLiveStatusDivider.BackColor = Color.FromArgb(38, 45, 62);
            pnlLiveStatusDivider.Location = new Point(10, 118);
            pnlLiveStatusDivider.Name = "pnlLiveStatusDivider";
            pnlLiveStatusDivider.Size = new Size(163, 1);
            pnlLiveStatusDivider.TabIndex = 4;
            // 
            // lblETALabel
            // 
            lblETALabel.BackColor = Color.Transparent;
            lblETALabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblETALabel.ForeColor = Color.FromArgb(107, 114, 128);
            lblETALabel.Location = new Point(10, 122);
            lblETALabel.Name = "lblETALabel";
            lblETALabel.Size = new Size(35, 18);
            lblETALabel.TabIndex = 5;
            lblETALabel.Text = "ETA:";
            lblETALabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblETAValue
            // 
            lblETAValue.BackColor = Color.Transparent;
            lblETAValue.Font = new Font("Consolas", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblETAValue.ForeColor = Color.FromArgb(16, 185, 129);
            lblETAValue.Location = new Point(60, 122);
            lblETAValue.Name = "lblETAValue";
            lblETAValue.Size = new Size(113, 18);
            lblETAValue.TabIndex = 6;
            lblETAValue.Text = "--";
            lblETAValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tlpSessionStats
            // 
            tlpSessionStats.ColumnCount = 2;
            tlpSessionStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpSessionStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpSessionStats.Controls.Add(lblSessionStatsTitle, 0, 0);
            tlpSessionStats.Controls.Add(lblMinLabel, 0, 1);
            tlpSessionStats.Controls.Add(lblMinValue, 1, 1);
            tlpSessionStats.Controls.Add(lblMaxLabel, 0, 2);
            tlpSessionStats.Controls.Add(lblMaxValue, 1, 2);
            tlpSessionStats.Controls.Add(lblAvgLabel, 0, 3);
            tlpSessionStats.Controls.Add(lblAvgValue, 1, 3);
            tlpSessionStats.Controls.Add(lblPointsLabel, 0, 4);
            tlpSessionStats.Controls.Add(lblPointsValue, 1, 4);
            tlpSessionStats.Controls.Add(lblDurationLabel, 0, 5);
            tlpSessionStats.Controls.Add(lblDurationValue, 1, 5);
            tlpSessionStats.Dock = DockStyle.Fill;
            tlpSessionStats.Location = new Point(3, 157);
            tlpSessionStats.Name = "tlpSessionStats";
            tlpSessionStats.RowCount = 6;
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpSessionStats.Size = new Size(183, 120);
            tlpSessionStats.TabIndex = 3;
            // 
            // lblSessionStatsTitle
            // 
            lblSessionStatsTitle.AutoSize = true;
            tlpSessionStats.SetColumnSpan(lblSessionStatsTitle, 2);
            lblSessionStatsTitle.Dock = DockStyle.Fill;
            lblSessionStatsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSessionStatsTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSessionStatsTitle.Location = new Point(3, 0);
            lblSessionStatsTitle.Name = "lblSessionStatsTitle";
            lblSessionStatsTitle.Size = new Size(177, 25);
            lblSessionStatsTitle.TabIndex = 0;
            lblSessionStatsTitle.Text = "SESSION STATS";
            lblSessionStatsTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMinLabel
            // 
            lblMinLabel.AutoSize = true;
            lblMinLabel.Dock = DockStyle.Fill;
            lblMinLabel.Font = new Font("Segoe UI", 8F);
            lblMinLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblMinLabel.Location = new Point(3, 25);
            lblMinLabel.Name = "lblMinLabel";
            lblMinLabel.Size = new Size(85, 19);
            lblMinLabel.TabIndex = 1;
            lblMinLabel.Text = "Min:";
            lblMinLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMinValue
            // 
            lblMinValue.AutoSize = true;
            lblMinValue.Dock = DockStyle.Fill;
            lblMinValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblMinValue.ForeColor = Color.White;
            lblMinValue.Location = new Point(94, 25);
            lblMinValue.Name = "lblMinValue";
            lblMinValue.Size = new Size(86, 19);
            lblMinValue.TabIndex = 2;
            lblMinValue.Text = "0.00";
            lblMinValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblMaxLabel
            // 
            lblMaxLabel.AutoSize = true;
            lblMaxLabel.Dock = DockStyle.Fill;
            lblMaxLabel.Font = new Font("Segoe UI", 8F);
            lblMaxLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblMaxLabel.Location = new Point(3, 44);
            lblMaxLabel.Name = "lblMaxLabel";
            lblMaxLabel.Size = new Size(85, 19);
            lblMaxLabel.TabIndex = 3;
            lblMaxLabel.Text = "Max:";
            lblMaxLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMaxValue
            // 
            lblMaxValue.AutoSize = true;
            lblMaxValue.Dock = DockStyle.Fill;
            lblMaxValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblMaxValue.ForeColor = Color.White;
            lblMaxValue.Location = new Point(94, 44);
            lblMaxValue.Name = "lblMaxValue";
            lblMaxValue.Size = new Size(86, 19);
            lblMaxValue.TabIndex = 4;
            lblMaxValue.Text = "0.00";
            lblMaxValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblAvgLabel
            // 
            lblAvgLabel.AutoSize = true;
            lblAvgLabel.Dock = DockStyle.Fill;
            lblAvgLabel.Font = new Font("Segoe UI", 8F);
            lblAvgLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblAvgLabel.Location = new Point(3, 63);
            lblAvgLabel.Name = "lblAvgLabel";
            lblAvgLabel.Size = new Size(85, 19);
            lblAvgLabel.TabIndex = 5;
            lblAvgLabel.Text = "Average:";
            lblAvgLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAvgValue
            // 
            lblAvgValue.AutoSize = true;
            lblAvgValue.Dock = DockStyle.Fill;
            lblAvgValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblAvgValue.ForeColor = Color.White;
            lblAvgValue.Location = new Point(94, 63);
            lblAvgValue.Name = "lblAvgValue";
            lblAvgValue.Size = new Size(86, 19);
            lblAvgValue.TabIndex = 6;
            lblAvgValue.Text = "0.00";
            lblAvgValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPointsLabel
            // 
            lblPointsLabel.AutoSize = true;
            lblPointsLabel.Dock = DockStyle.Fill;
            lblPointsLabel.Font = new Font("Segoe UI", 8F);
            lblPointsLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblPointsLabel.Location = new Point(3, 82);
            lblPointsLabel.Name = "lblPointsLabel";
            lblPointsLabel.Size = new Size(85, 19);
            lblPointsLabel.TabIndex = 9;
            lblPointsLabel.Text = "Points:";
            lblPointsLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPointsValue
            // 
            lblPointsValue.AutoSize = true;
            lblPointsValue.Dock = DockStyle.Fill;
            lblPointsValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblPointsValue.ForeColor = Color.White;
            lblPointsValue.Location = new Point(94, 82);
            lblPointsValue.Name = "lblPointsValue";
            lblPointsValue.Size = new Size(86, 19);
            lblPointsValue.TabIndex = 10;
            lblPointsValue.Text = "0";
            lblPointsValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDurationLabel
            // 
            lblDurationLabel.AutoSize = true;
            lblDurationLabel.Dock = DockStyle.Fill;
            lblDurationLabel.Font = new Font("Segoe UI", 8F);
            lblDurationLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblDurationLabel.Location = new Point(3, 101);
            lblDurationLabel.Name = "lblDurationLabel";
            lblDurationLabel.Size = new Size(85, 19);
            lblDurationLabel.TabIndex = 11;
            lblDurationLabel.Text = "Duration:";
            lblDurationLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDurationValue
            // 
            lblDurationValue.AutoSize = true;
            lblDurationValue.Dock = DockStyle.Fill;
            lblDurationValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblDurationValue.ForeColor = Color.White;
            lblDurationValue.Location = new Point(94, 101);
            lblDurationValue.Name = "lblDurationValue";
            lblDurationValue.Size = new Size(86, 19);
            lblDurationValue.TabIndex = 12;
            lblDurationValue.Text = "00:00";
            lblDurationValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tlpGoToTarget
            // 
            tlpGoToTarget.ColumnCount = 1;
            tlpGoToTarget.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpGoToTarget.Controls.Add(lblGoToTargetTitle, 0, 0);
            tlpGoToTarget.Controls.Add(dgvTargets, 0, 1);
            tlpGoToTarget.Controls.Add(tlpTargetInputs, 0, 2);
            tlpGoToTarget.Controls.Add(btnAddTarget, 0, 3);
            tlpGoToTarget.Controls.Add(tlpButtonsRow, 0, 4);
            tlpGoToTarget.Controls.Add(lblProgress, 0, 5);
            tlpGoToTarget.Controls.Add(progressBarProgress, 0, 6);
            tlpGoToTarget.Controls.Add(lblHoldTimer, 0, 7);
            tlpGoToTarget.Controls.Add(progressBarHold, 0, 8);
            tlpGoToTarget.Controls.Add(tlpControlButtons, 0, 9);
            tlpGoToTarget.Dock = DockStyle.Fill;
            tlpGoToTarget.Location = new Point(3, 283);
            tlpGoToTarget.Name = "tlpGoToTarget";
            tlpGoToTarget.Padding = new Padding(5);
            tlpGoToTarget.RowCount = 10;
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tlpGoToTarget.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpGoToTarget.Size = new Size(183, 346);
            tlpGoToTarget.TabIndex = 4;
            // 
            // lblGoToTargetTitle
            // 
            lblGoToTargetTitle.AutoSize = true;
            lblGoToTargetTitle.Dock = DockStyle.Fill;
            lblGoToTargetTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGoToTargetTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblGoToTargetTitle.Location = new Point(8, 5);
            lblGoToTargetTitle.Name = "lblGoToTargetTitle";
            lblGoToTargetTitle.Size = new Size(178, 25);
            lblGoToTargetTitle.TabIndex = 0;
            lblGoToTargetTitle.Text = "GO TO TARGET";
            lblGoToTargetTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvTargets
            // 
            dgvTargets.AllowUserToAddRows = false;
            dgvTargets.AllowUserToDeleteRows = false;
            dgvTargets.AllowUserToResizeRows = false;
            dgvTargets.BackgroundColor = Color.FromArgb(21, 23, 28);
            dgvTargets.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(26, 31, 46);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(156, 163, 175);
            // SelectionBackColor установлен в тот же цвет, что и BackColor, чтобы не было синей подсветки при выделении заголовка
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(26, 31, 46);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(156, 163, 175);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTargets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTargets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(21, 23, 28);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            // SelectionBackColor установлен в тот же цвет, что и BackColor, чтобы не было подсветки выбранной строки
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(21, 23, 28);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTargets.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTargets.Dock = DockStyle.Fill;
            dgvTargets.EnableHeadersVisualStyles = false;
            dgvTargets.GridColor = Color.FromArgb(42, 45, 53);
            dgvTargets.Location = new Point(8, 33);
            dgvTargets.MultiSelect = false;
            dgvTargets.Name = "dgvTargets";
            dgvTargets.ReadOnly = true;
            dgvTargets.RowHeadersVisible = false;
            dgvTargets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTargets.Size = new Size(178, 90);
            dgvTargets.TabIndex = 1;
            // 
            // tlpTargetInputs
            // 
            tlpTargetInputs.ColumnCount = 4;
            tlpTargetInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            tlpTargetInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTargetInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tlpTargetInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTargetInputs.Controls.Add(lblPSILabel, 0, 0);
            tlpTargetInputs.Controls.Add(txtPSI, 1, 1);
            tlpTargetInputs.Controls.Add(lblHoldLabel, 2, 0);
            tlpTargetInputs.Controls.Add(txtHold, 3, 1);
            tlpTargetInputs.Dock = DockStyle.Fill;
            tlpTargetInputs.Location = new Point(8, 126);
            tlpTargetInputs.Name = "tlpTargetInputs";
            tlpTargetInputs.RowCount = 2;
            tlpTargetInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpTargetInputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpTargetInputs.Size = new Size(178, 44);
            tlpTargetInputs.TabIndex = 2;
            // 
            // lblPSILabel
            // 
            lblPSILabel.AutoSize = true;
            lblPSILabel.Dock = DockStyle.Fill;
            lblPSILabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblPSILabel.Location = new Point(0, 0);
            lblPSILabel.Margin = new Padding(0);
            lblPSILabel.Name = "lblPSILabel";
            lblPSILabel.Size = new Size(45, 20);
            lblPSILabel.TabIndex = 0;
            lblPSILabel.Text = "PSI:";
            lblPSILabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPSI
            // 
            txtPSI.BackColor = Color.FromArgb(40, 43, 52);
            txtPSI.BorderStyle = BorderStyle.FixedSingle;
            txtPSI.Dock = DockStyle.Fill;
            txtPSI.ForeColor = Color.White;
            txtPSI.Location = new Point(45, 20);
            txtPSI.Margin = new Padding(0);
            txtPSI.Name = "txtPSI";
            txtPSI.Size = new Size(29, 23);
            txtPSI.TabIndex = 1;
            txtPSI.Text = "0";
            // 
            // lblHoldLabel
            // 
            lblHoldLabel.AutoSize = true;
            lblHoldLabel.Dock = DockStyle.Fill;
            lblHoldLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblHoldLabel.Location = new Point(74, 0);
            lblHoldLabel.Margin = new Padding(0);
            lblHoldLabel.Name = "lblHoldLabel";
            lblHoldLabel.Size = new Size(75, 20);
            lblHoldLabel.TabIndex = 2;
            lblHoldLabel.Text = "Hold (min):";
            lblHoldLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtHold
            // 
            txtHold.BackColor = Color.FromArgb(40, 43, 52);
            txtHold.BorderStyle = BorderStyle.FixedSingle;
            txtHold.Dock = DockStyle.Fill;
            txtHold.ForeColor = Color.White;
            txtHold.Location = new Point(149, 20);
            txtHold.Margin = new Padding(0);
            txtHold.Name = "txtHold";
            txtHold.Size = new Size(29, 23);
            txtHold.TabIndex = 3;
            txtHold.Text = "0";
            // 
            // btnAddTarget
            // 
            btnAddTarget.BackColor = Color.FromArgb(76, 175, 80);
            btnAddTarget.Dock = DockStyle.Fill;
            btnAddTarget.FlatAppearance.BorderSize = 0;
            btnAddTarget.FlatStyle = FlatStyle.Flat;
            btnAddTarget.ForeColor = Color.White;
            btnAddTarget.Location = new Point(8, 176);
            btnAddTarget.Name = "btnAddTarget";
            btnAddTarget.Size = new Size(178, 24);
            btnAddTarget.TabIndex = 3;
            btnAddTarget.Text = "+ Add";
            btnAddTarget.UseVisualStyleBackColor = false;
            // 
            // tlpButtonsRow
            // 
            tlpButtonsRow.Dock = DockStyle.Fill;
            tlpButtonsRow.FlowDirection = FlowDirection.LeftToRight;
            tlpButtonsRow.WrapContents = false;
            tlpButtonsRow.AutoSize = false;
            tlpButtonsRow.Location = new Point(8, 206);
            tlpButtonsRow.Name = "tlpButtonsRow";
            tlpButtonsRow.Size = new Size(178, 24);
            tlpButtonsRow.TabIndex = 4;
            // 
            // btnClearAll
            // 
            btnClearAll.BackColor = Color.FromArgb(42, 45, 53);
            btnClearAll.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnClearAll.FlatStyle = FlatStyle.Flat;
            btnClearAll.ForeColor = Color.FromArgb(200, 205, 215);
            btnClearAll.Size = new Size(86, 24);
            btnClearAll.Margin = new Padding(0, 0, 6, 0);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.TabIndex = 0;
            btnClearAll.Text = "🗑️ Clear";
            btnClearAll.UseVisualStyleBackColor = false;
            tlpButtonsRow.Controls.Add(btnClearAll);
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(42, 45, 53);
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.ForeColor = Color.FromArgb(200, 205, 215);
            btnEdit.Size = new Size(86, 24);
            btnEdit.Margin = new Padding(0);
            btnEdit.Name = "btnEdit";
            btnEdit.TabIndex = 1;
            btnEdit.Text = "⚙ Edit";
            btnEdit.UseVisualStyleBackColor = false;
            tlpButtonsRow.Controls.Add(btnEdit);
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Dock = DockStyle.Fill;
            lblProgress.ForeColor = Color.FromArgb(120, 125, 140);
            lblProgress.Location = new Point(8, 236);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(178, 20);
            lblProgress.TabIndex = 5;
            lblProgress.Text = "Progress:        0/2";
            lblProgress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBarProgress
            // 
            progressBarProgress.Dock = DockStyle.Fill;
            // --accent-green: #10b981 (progress bar)
            progressBarProgress.ForeColor = Color.FromArgb(16, 185, 129);
            progressBarProgress.Location = new Point(8, 256);
            progressBarProgress.Name = "progressBarProgress";
            progressBarProgress.Size = new Size(178, 9);
            progressBarProgress.Style = ProgressBarStyle.Continuous;
            progressBarProgress.TabIndex = 6;
            // 
            // lblHoldTimer
            // 
            lblHoldTimer.AutoSize = true;
            lblHoldTimer.Dock = DockStyle.Fill;
            lblHoldTimer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            // --accent-gold: #f59e0b
            lblHoldTimer.ForeColor = Color.FromArgb(245, 158, 11);
            lblHoldTimer.Location = new Point(8, 271);
            lblHoldTimer.Name = "lblHoldTimer";
            lblHoldTimer.Size = new Size(178, 25);
            lblHoldTimer.TabIndex = 7;
            lblHoldTimer.Text = "Hold:           00:00";
            lblHoldTimer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBarHold
            // 
            progressBarHold.Dock = DockStyle.Fill;
            // --accent-gold: #f59e0b
            progressBarHold.ForeColor = Color.FromArgb(245, 158, 11);
            progressBarHold.Location = new Point(8, 296);
            progressBarHold.Name = "progressBarHold";
            progressBarHold.Size = new Size(178, 9);
            progressBarHold.Style = ProgressBarStyle.Continuous;
            progressBarHold.TabIndex = 8;
            // 
            // tlpControlButtons
            // 
            tlpControlButtons.AutoSize = true;
            tlpControlButtons.Controls.Add(btnPlay);
            tlpControlButtons.Controls.Add(btnPauseTarget);
            tlpControlButtons.Controls.Add(btnStop);
            tlpControlButtons.Controls.Add(btnSkip);
            tlpControlButtons.Dock = DockStyle.Fill;
            tlpControlButtons.Location = new Point(8, 311);
            tlpControlButtons.Name = "tlpControlButtons";
            tlpControlButtons.Size = new Size(178, 34);
            tlpControlButtons.TabIndex = 9;
            tlpControlButtons.WrapContents = false;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.FromArgb(76, 175, 80);
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(3, 3);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(40, 30);
            btnPlay.TabIndex = 0;
            btnPlay.Text = "▶";
            btnPlay.UseVisualStyleBackColor = false;
            // 
            // btnPauseTarget
            // 
            btnPauseTarget.BackColor = Color.FromArgb(42, 45, 53);
            btnPauseTarget.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnPauseTarget.FlatStyle = FlatStyle.Flat;
            btnPauseTarget.ForeColor = Color.White;
            btnPauseTarget.Location = new Point(49, 3);
            btnPauseTarget.Name = "btnPauseTarget";
            btnPauseTarget.Size = new Size(40, 30);
            btnPauseTarget.TabIndex = 1;
            btnPauseTarget.Text = "⏸";
            btnPauseTarget.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(42, 45, 53);
            btnStop.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(95, 3);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(40, 30);
            btnStop.TabIndex = 2;
            btnStop.Text = "⏹";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // btnSkip
            // 
            btnSkip.BackColor = Color.FromArgb(42, 45, 53);
            btnSkip.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnSkip.FlatStyle = FlatStyle.Flat;
            btnSkip.ForeColor = Color.White;
            btnSkip.Location = new Point(141, 3);
            btnSkip.Name = "btnSkip";
            btnSkip.Size = new Size(40, 30);
            btnSkip.TabIndex = 3;
            btnSkip.Text = "⏭";
            btnSkip.UseVisualStyleBackColor = false;
            // 
            // btnEmergency
            // 
            btnEmergency.Anchor = AnchorStyles.None;
            btnEmergency.AutoSize = true;
            btnEmergency.BackColor = Color.FromArgb(191, 0, 0);
            btnEmergency.ForeColor = Color.FromArgb(210, 215, 225);
            btnEmergency.Location = new Point(19, 646);
            btnEmergency.Name = "btnEmergency";
            btnEmergency.Size = new Size(150, 43);
            btnEmergency.TabIndex = 1;
            btnEmergency.Text = "Emergency Vent";
            btnEmergency.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblTarget, 0, 0);
            tableLayoutPanel1.Controls.Add(lblDelta, 0, 1);
            tableLayoutPanel1.Controls.Add(lblRate, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 111);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.343338F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.343338F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.313324F));
            tableLayoutPanel1.Size = new Size(188, 80);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblDelta
            // 
            lblDelta.AutoSize = true;
            lblDelta.Dock = DockStyle.Fill;
            lblDelta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDelta.ForeColor = Color.FromArgb(120, 125, 140);
            lblDelta.Location = new Point(5, 31);
            lblDelta.Margin = new Padding(5, 5, 0, 0);
            lblDelta.Name = "lblDelta";
            lblDelta.Size = new Size(89, 21);
            lblDelta.TabIndex = 14;
            lblDelta.Text = "Delta";
            // 
            // lblRate
            // 
            lblRate.AutoSize = true;
            lblRate.Dock = DockStyle.Fill;
            lblRate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRate.ForeColor = Color.FromArgb(120, 125, 140);
            lblRate.Location = new Point(5, 57);
            lblRate.Margin = new Padding(5, 5, 0, 0);
            lblRate.Name = "lblRate";
            lblRate.Size = new Size(89, 23);
            lblRate.TabIndex = 15;
            lblRate.Text = "Rate";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblETA, 0, 0);
            tableLayoutPanel2.Controls.Add(lblTrend, 1, 0);
            tableLayoutPanel2.Location = new Point(3, 197);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(188, 16);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lblTrend
            // 
            lblTrend.AutoSize = true;
            lblTrend.Dock = DockStyle.Fill;
            lblTrend.ForeColor = Color.FromArgb(120, 125, 140);
            lblTrend.Location = new Point(97, 0);
            lblTrend.Name = "lblTrend";
            lblTrend.Size = new Size(88, 16);
            lblTrend.TabIndex = 1;
            lblTrend.Text = "Trend";
            // 
            // pnlWarnIndicator
            // 
            pnlWarnIndicator.BackColor = Color.FromArgb(191, 0, 0);
            pnlWarnIndicator.Dock = DockStyle.Fill;
            pnlWarnIndicator.Location = new Point(3, 79);
            pnlWarnIndicator.Name = "pnlWarnIndicator";
            pnlWarnIndicator.Size = new Size(182, 20);
            pnlWarnIndicator.TabIndex = 2;
            pnlWarnIndicator.Visible = false;
            // 
            // lblStdDevLabel
            // 
            lblStdDevLabel.AutoSize = true;
            lblStdDevLabel.Dock = DockStyle.Fill;
            lblStdDevLabel.Font = new Font("Segoe UI", 8F);
            lblStdDevLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblStdDevLabel.Location = new Point(3, 94);
            lblStdDevLabel.Name = "lblStdDevLabel";
            lblStdDevLabel.Size = new Size(91, 23);
            lblStdDevLabel.TabIndex = 7;
            lblStdDevLabel.Text = "Std Dev (σ):";
            lblStdDevLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStdDevValue
            // 
            lblStdDevValue.AutoSize = true;
            lblStdDevValue.Dock = DockStyle.Fill;
            lblStdDevValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblStdDevValue.ForeColor = Color.White;
            lblStdDevValue.Location = new Point(100, 94);
            lblStdDevValue.Name = "lblStdDevValue";
            lblStdDevValue.Size = new Size(91, 23);
            lblStdDevValue.TabIndex = 8;
            lblStdDevValue.Text = "0.00";
            lblStdDevValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSampleRateLabel
            // 
            lblSampleRateLabel.AutoSize = true;
            lblSampleRateLabel.Dock = DockStyle.Fill;
            lblSampleRateLabel.Font = new Font("Segoe UI", 8F);
            lblSampleRateLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblSampleRateLabel.Location = new Point(3, 163);
            lblSampleRateLabel.Name = "lblSampleRateLabel";
            lblSampleRateLabel.Size = new Size(91, 24);
            lblSampleRateLabel.TabIndex = 13;
            lblSampleRateLabel.Text = "Sample Rate:";
            lblSampleRateLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSampleRateValue
            // 
            lblSampleRateValue.AutoSize = true;
            lblSampleRateValue.Dock = DockStyle.Fill;
            lblSampleRateValue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblSampleRateValue.ForeColor = Color.White;
            lblSampleRateValue.Location = new Point(100, 163);
            lblSampleRateValue.Name = "lblSampleRateValue";
            lblSampleRateValue.Size = new Size(91, 24);
            lblSampleRateValue.TabIndex = 14;
            lblSampleRateValue.Text = "0 Hz";
            lblSampleRateValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(21, 23, 28);
            panelRight.Controls.Add(tableSettings);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(1040, 50);
            panelRight.Margin = new Padding(4);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(10, 4, 10, 4);
            panelRight.Size = new Size(260, 770);
            panelRight.TabIndex = 2;
            // 
            // tableSettings
            // 
            tableSettings.ColumnCount = 1;
            tableSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableSettings.Controls.Add(tlpTimeWindow, 0, 0);
            tableSettings.Controls.Add(tlpGrid, 0, 1);
            tableSettings.Controls.Add(tlpThresholds, 0, 2);
            tableSettings.Controls.Add(tlpDisplay, 0, 3);
            tableSettings.Controls.Add(tlpAlerts, 0, 4);
            tableSettings.Dock = DockStyle.Fill;
            tableSettings.Location = new Point(10, 4);
            tableSettings.Name = "tableSettings";
            tableSettings.RowCount = 5;
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableSettings.Size = new Size(167, 694);
            tableSettings.TabIndex = 0;
            // 
            // tlpTimeWindow
            // 
            tlpTimeWindow.BackColor = Color.FromArgb(32, 35, 44);
            tlpTimeWindow.ColumnCount = 2;
            tlpTimeWindow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpTimeWindow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpTimeWindow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpTimeWindow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpTimeWindow.Controls.Add(lblTimeWindowTitle, 0, 0);
            tlpTimeWindow.Controls.Add(cmbDuration, 1, 1);
            tlpTimeWindow.Controls.Add(lblDuration, 0, 1);
            tlpTimeWindow.Dock = DockStyle.Fill;
            tlpTimeWindow.Location = new Point(3, 3);
            tlpTimeWindow.Name = "tlpTimeWindow";
            tlpTimeWindow.RowCount = 3;
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpTimeWindow.Size = new Size(161, 132);
            tlpTimeWindow.TabIndex = 0;
            // 
            // lblTimeWindowTitle
            // 
            lblTimeWindowTitle.AutoSize = true;
            tlpTimeWindow.SetColumnSpan(lblTimeWindowTitle, 2);
            lblTimeWindowTitle.Dock = DockStyle.Top;
            lblTimeWindowTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTimeWindowTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblTimeWindowTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblTimeWindowTitle.Location = new Point(5, 5);
            lblTimeWindowTitle.Margin = new Padding(5, 5, 0, 0);
            lblTimeWindowTitle.Name = "lblTimeWindowTitle";
            lblTimeWindowTitle.Size = new Size(156, 13);
            lblTimeWindowTitle.TabIndex = 12;
            lblTimeWindowTitle.Text = "TIME WINDOW";
            // 
            // lblDuration
            // 
            lblDuration.AutoSize = true;
            lblDuration.Dock = DockStyle.Top;
            lblDuration.ForeColor = Color.FromArgb(210, 215, 225);
            lblDuration.Location = new Point(4, 34);
            lblDuration.Margin = new Padding(4, 4, 0, 0);
            lblDuration.Name = "lblDuration";
            lblDuration.Padding = new Padding(5, 5, 0, 0);
            lblDuration.Size = new Size(92, 20);
            lblDuration.TabIndex = 11;
            lblDuration.Text = "Duration";
            // 
            // cmbDuration
            // 
            cmbDuration.BackColor = Color.FromArgb(40, 43, 52);
            cmbDuration.Dock = DockStyle.Top;
            cmbDuration.ForeColor = Color.White;
            cmbDuration.FormattingEnabled = true;
            cmbDuration.Location = new Point(99, 33);
            cmbDuration.Name = "cmbDuration";
            cmbDuration.Size = new Size(59, 23);
            cmbDuration.TabIndex = 10;
            // 
            // tlpGrid
            // 
            tlpGrid.BackColor = Color.FromArgb(32, 35, 44);
            tlpGrid.ColumnCount = 2;
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpGrid.Controls.Add(lblGridTitle, 0, 0);
            tlpGrid.Controls.Add(lblYStep, 0, 1);
            tlpGrid.Controls.Add(cmbYStep, 1, 1);
            tlpGrid.Dock = DockStyle.Fill;
            tlpGrid.Location = new Point(3, 141);
            tlpGrid.Name = "tlpGrid";
            tlpGrid.RowCount = 2;
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tlpGrid.Size = new Size(161, 132);
            tlpGrid.TabIndex = 1;
            // 
            // lblGridTitle
            // 
            lblGridTitle.AutoSize = true;
            tlpGrid.SetColumnSpan(lblGridTitle, 2);
            lblGridTitle.Dock = DockStyle.Top;
            lblGridTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGridTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblGridTitle.Location = new Point(5, 5);
            lblGridTitle.Margin = new Padding(5, 5, 0, 0);
            lblGridTitle.Name = "lblGridTitle";
            lblGridTitle.Size = new Size(156, 13);
            lblGridTitle.TabIndex = 0;
            lblGridTitle.Text = "GRID";
            // 
            // lblYStep
            // 
            lblYStep.AutoSize = true;
            lblYStep.Dock = DockStyle.Top;
            lblYStep.ForeColor = Color.FromArgb(210, 215, 225);
            lblYStep.Location = new Point(4, 43);
            lblYStep.Margin = new Padding(4, 4, 0, 0);
            lblYStep.Name = "lblYStep";
            lblYStep.Padding = new Padding(5, 5, 0, 0);
            lblYStep.Size = new Size(92, 20);
            lblYStep.TabIndex = 1;
            lblYStep.Text = "Y Step:";
            // 
            // cmbYStep
            // 
            cmbYStep.BackColor = Color.FromArgb(40, 43, 52);
            cmbYStep.Dock = DockStyle.Top;
            cmbYStep.ForeColor = Color.White;
            cmbYStep.FormattingEnabled = true;
            cmbYStep.Location = new Point(99, 42);
            cmbYStep.Name = "cmbYStep";
            cmbYStep.Size = new Size(59, 23);
            cmbYStep.TabIndex = 2;
            // 
            // tlpThresholds
            // 
            tlpThresholds.BackColor = Color.FromArgb(32, 35, 44);
            tlpThresholds.ColumnCount = 3;
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpThresholds.Controls.Add(lblThresholdsTitle, 1, 0);
            tlpThresholds.Controls.Add(lblMaximum, 1, 1);
            tlpThresholds.Controls.Add(txtMaxThreshold, 2, 1);
            tlpThresholds.Controls.Add(panelMaxIndicator, 0, 1);
            tlpThresholds.Controls.Add(lblMinimum, 1, 2);
            tlpThresholds.Controls.Add(txtMinThreshold, 2, 2);
            tlpThresholds.Controls.Add(panelMinIndicator, 0, 2);
            tlpThresholds.Dock = DockStyle.Fill;
            tlpThresholds.Location = new Point(3, 279);
            tlpThresholds.Name = "tlpThresholds";
            tlpThresholds.RowCount = 3;
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpThresholds.Size = new Size(161, 132);
            tlpThresholds.TabIndex = 2;
            // 
            // lblThresholdsTitle
            // 
            lblThresholdsTitle.AutoSize = true;
            tlpThresholds.SetColumnSpan(lblThresholdsTitle, 3);
            lblThresholdsTitle.Dock = DockStyle.Top;
            lblThresholdsTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThresholdsTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblThresholdsTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblThresholdsTitle.Location = new Point(5, 38);
            lblThresholdsTitle.Margin = new Padding(5, 5, 0, 0);
            lblThresholdsTitle.Name = "lblThresholdsTitle";
            lblThresholdsTitle.Size = new Size(156, 13);
            lblThresholdsTitle.TabIndex = 7;
            lblThresholdsTitle.Text = "THRESHOLDS";
            lblThresholdsTitle.Click += label3_Click;
            // 
            // lblMaximum
            // 
            lblMaximum.AutoSize = true;
            lblMaximum.Dock = DockStyle.Top;
            lblMaximum.ForeColor = Color.FromArgb(210, 215, 225);
            lblMaximum.Location = new Point(8, 76);
            lblMaximum.Margin = new Padding(4, 4, 0, 0);
            lblMaximum.Name = "lblMaximum";
            lblMaximum.Padding = new Padding(5, 5, 0, 0);
            lblMaximum.Size = new Size(74, 20);
            lblMaximum.TabIndex = 7;
            lblMaximum.Text = "Maximum";
            lblMaximum.Click += label3_Click_1;
            // 
            // txtMaxThreshold
            // 
            txtMaxThreshold.BackColor = Color.FromArgb(40, 43, 52);
            txtMaxThreshold.BorderStyle = BorderStyle.FixedSingle;
            txtMaxThreshold.Dock = DockStyle.Top;
            txtMaxThreshold.ForeColor = Color.FromArgb(230, 80, 80);
            txtMaxThreshold.Location = new Point(85, 76);
            txtMaxThreshold.Margin = new Padding(3, 4, 0, 0);
            txtMaxThreshold.Name = "txtMaxThreshold";
            txtMaxThreshold.Size = new Size(76, 23);
            txtMaxThreshold.TabIndex = 3;
            txtMaxThreshold.Text = "0";
            txtMaxThreshold.TextAlign = HorizontalAlignment.Center;
            // 
            // panelMaxIndicator
            // 
            panelMaxIndicator.BackColor = Color.FromArgb(230, 80, 80);
            panelMaxIndicator.Dock = DockStyle.Fill;
            panelMaxIndicator.Location = new Point(0, 72);
            panelMaxIndicator.Margin = new Padding(0);
            panelMaxIndicator.Name = "panelMaxIndicator";
            panelMaxIndicator.Size = new Size(4, 39);
            panelMaxIndicator.TabIndex = 1;
            // 
            // lblMinimum
            // 
            lblMinimum.AutoSize = true;
            lblMinimum.Dock = DockStyle.Top;
            lblMinimum.ForeColor = Color.FromArgb(210, 215, 225);
            lblMinimum.Location = new Point(8, 115);
            lblMinimum.Margin = new Padding(4, 4, 0, 0);
            lblMinimum.Name = "lblMinimum";
            lblMinimum.Padding = new Padding(5, 5, 0, 0);
            lblMinimum.Size = new Size(74, 17);
            lblMinimum.TabIndex = 7;
            lblMinimum.Text = "Minimum";
            // 
            // txtMinThreshold
            // 
            txtMinThreshold.BackColor = Color.FromArgb(40, 43, 52);
            txtMinThreshold.BorderStyle = BorderStyle.FixedSingle;
            txtMinThreshold.Dock = DockStyle.Top;
            txtMinThreshold.ForeColor = Color.FromArgb(100, 150, 255);
            txtMinThreshold.Location = new Point(85, 115);
            txtMinThreshold.Margin = new Padding(3, 4, 0, 0);
            txtMinThreshold.Name = "txtMinThreshold";
            txtMinThreshold.Size = new Size(76, 23);
            txtMinThreshold.TabIndex = 6;
            txtMinThreshold.Text = "0";
            txtMinThreshold.TextAlign = HorizontalAlignment.Center;
            // 
            // panelMinIndicator
            // 
            panelMinIndicator.BackColor = Color.FromArgb(100, 150, 255);
            panelMinIndicator.Dock = DockStyle.Fill;
            panelMinIndicator.Location = new Point(0, 111);
            panelMinIndicator.Margin = new Padding(0);
            panelMinIndicator.Name = "panelMinIndicator";
            panelMinIndicator.Size = new Size(4, 21);
            panelMinIndicator.TabIndex = 4;
            // 
            // tlpDisplay
            // 
            tlpDisplay.BackColor = Color.FromArgb(32, 35, 44);
            tlpDisplay.ColumnCount = 1;
            tlpDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDisplay.Controls.Add(lblDisplayTitle, 0, 0);
            tlpDisplay.Controls.Add(chkShowGrid, 0, 1);
            tlpDisplay.Controls.Add(chkShowTarget, 0, 2);
            tlpDisplay.Controls.Add(chkShowMax, 0, 3);
            tlpDisplay.Controls.Add(chkShowMin, 0, 4);
            tlpDisplay.Dock = DockStyle.Fill;
            tlpDisplay.Location = new Point(3, 417);
            tlpDisplay.Name = "tlpDisplay";
            tlpDisplay.RowCount = 5;
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpDisplay.Size = new Size(161, 132);
            tlpDisplay.TabIndex = 3;
            // 
            // lblDisplayTitle
            // 
            lblDisplayTitle.AutoSize = true;
            lblDisplayTitle.Dock = DockStyle.Top;
            lblDisplayTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDisplayTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblDisplayTitle.Location = new Point(5, 5);
            lblDisplayTitle.Margin = new Padding(5, 5, 0, 0);
            lblDisplayTitle.Name = "lblDisplayTitle";
            lblDisplayTitle.Size = new Size(156, 13);
            lblDisplayTitle.TabIndex = 0;
            lblDisplayTitle.Text = "DISPLAY";
            // 
            // chkShowGrid
            // 
            chkShowGrid.AutoSize = true;
            chkShowGrid.Checked = true;
            chkShowGrid.CheckState = CheckState.Checked;
            chkShowGrid.Dock = DockStyle.Top;
            chkShowGrid.FlatStyle = FlatStyle.Flat;
            chkShowGrid.ForeColor = Color.FromArgb(210, 215, 225);
            chkShowGrid.Location = new Point(4, 30);
            chkShowGrid.Margin = new Padding(4, 4, 0, 0);
            chkShowGrid.Name = "chkShowGrid";
            chkShowGrid.Padding = new Padding(5, 1, 0, 0);
            chkShowGrid.Size = new Size(157, 20);
            chkShowGrid.TabIndex = 1;
            chkShowGrid.Text = "Show Grid";
            chkShowGrid.UseVisualStyleBackColor = true;
            // 
            // chkShowTarget
            // 
            chkShowTarget.AutoSize = true;
            chkShowTarget.Checked = true;
            chkShowTarget.CheckState = CheckState.Checked;
            chkShowTarget.Dock = DockStyle.Top;
            chkShowTarget.FlatStyle = FlatStyle.Flat;
            chkShowTarget.ForeColor = Color.FromArgb(210, 215, 225);
            chkShowTarget.Location = new Point(4, 56);
            chkShowTarget.Margin = new Padding(4, 4, 0, 0);
            chkShowTarget.Name = "chkShowTarget";
            chkShowTarget.Padding = new Padding(5, 1, 0, 0);
            chkShowTarget.Size = new Size(157, 20);
            chkShowTarget.TabIndex = 2;
            chkShowTarget.Text = "Show Target";
            chkShowTarget.UseVisualStyleBackColor = true;
            // 
            // chkShowMax
            // 
            chkShowMax.AutoSize = true;
            chkShowMax.Checked = true;
            chkShowMax.CheckState = CheckState.Checked;
            chkShowMax.Dock = DockStyle.Top;
            chkShowMax.FlatStyle = FlatStyle.Flat;
            chkShowMax.ForeColor = Color.FromArgb(210, 215, 225);
            chkShowMax.Location = new Point(4, 82);
            chkShowMax.Margin = new Padding(4, 4, 0, 0);
            chkShowMax.Name = "chkShowMax";
            chkShowMax.Padding = new Padding(5, 1, 0, 0);
            chkShowMax.Size = new Size(157, 20);
            chkShowMax.TabIndex = 3;
            chkShowMax.Text = "Show Max";
            chkShowMax.UseVisualStyleBackColor = true;
            // 
            // chkShowMin
            // 
            chkShowMin.AutoSize = true;
            chkShowMin.Checked = true;
            chkShowMin.CheckState = CheckState.Checked;
            chkShowMin.Dock = DockStyle.Top;
            chkShowMin.FlatStyle = FlatStyle.Flat;
            chkShowMin.ForeColor = Color.FromArgb(210, 215, 225);
            chkShowMin.Location = new Point(4, 108);
            chkShowMin.Margin = new Padding(4, 4, 0, 0);
            chkShowMin.Name = "chkShowMin";
            chkShowMin.Padding = new Padding(5, 1, 0, 0);
            chkShowMin.Size = new Size(157, 20);
            chkShowMin.TabIndex = 4;
            chkShowMin.Text = "Show Min";
            chkShowMin.UseVisualStyleBackColor = true;
            // 
            // tlpAlerts
            // 
            tlpAlerts.BackColor = Color.FromArgb(32, 35, 44);
            tlpAlerts.ColumnCount = 2;
            tlpAlerts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tlpAlerts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpAlerts.Controls.Add(lblAlertsTitle, 0, 0);
            tlpAlerts.Controls.Add(panelAlertsIcon, 1, 0);
            tlpAlerts.Controls.Add(chkSound, 0, 1);
            tlpAlerts.Controls.Add(chkAtTarget, 0, 2);
            tlpAlerts.Controls.Add(chkAtMax, 0, 3);
            tlpAlerts.Dock = DockStyle.Fill;
            tlpAlerts.Location = new Point(3, 555);
            tlpAlerts.Name = "tlpAlerts";
            tlpAlerts.RowCount = 4;
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpAlerts.Size = new Size(161, 136);
            tlpAlerts.TabIndex = 4;
            // 
            // lblAlertsTitle
            // 
            lblAlertsTitle.AutoSize = true;
            lblAlertsTitle.Dock = DockStyle.Top;
            lblAlertsTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAlertsTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblAlertsTitle.Location = new Point(5, 5);
            lblAlertsTitle.Margin = new Padding(5, 5, 0, 0);
            lblAlertsTitle.Name = "lblAlertsTitle";
            lblAlertsTitle.Size = new Size(123, 13);
            lblAlertsTitle.TabIndex = 0;
            lblAlertsTitle.Text = "ALERTS";
            // 
            // panelAlertsIcon
            // 
            panelAlertsIcon.BackColor = Color.Transparent;
            panelAlertsIcon.Dock = DockStyle.Fill;
            panelAlertsIcon.Location = new Point(131, 3);
            panelAlertsIcon.Name = "panelAlertsIcon";
            panelAlertsIcon.Size = new Size(27, 28);
            panelAlertsIcon.TabIndex = 1;
            panelAlertsIcon.Paint += PanelAlertsIcon_Paint;
            // 
            // chkSound
            // 
            chkSound.AutoSize = true;
            chkSound.Checked = true;
            chkSound.CheckState = CheckState.Checked;
            tlpAlerts.SetColumnSpan(chkSound, 2);
            chkSound.Dock = DockStyle.Top;
            chkSound.FlatStyle = FlatStyle.Flat;
            chkSound.ForeColor = Color.FromArgb(210, 215, 225);
            chkSound.Location = new Point(4, 38);
            chkSound.Margin = new Padding(4, 4, 0, 0);
            chkSound.Name = "chkSound";
            chkSound.Padding = new Padding(5, 1, 0, 0);
            chkSound.Size = new Size(157, 20);
            chkSound.TabIndex = 2;
            chkSound.Text = "Sound";
            chkSound.UseVisualStyleBackColor = true;
            // 
            // chkAtTarget
            // 
            chkAtTarget.AutoSize = true;
            chkAtTarget.Checked = true;
            chkAtTarget.CheckState = CheckState.Checked;
            tlpAlerts.SetColumnSpan(chkAtTarget, 2);
            chkAtTarget.Dock = DockStyle.Top;
            chkAtTarget.FlatStyle = FlatStyle.Flat;
            chkAtTarget.ForeColor = Color.FromArgb(210, 215, 225);
            chkAtTarget.Location = new Point(4, 72);
            chkAtTarget.Margin = new Padding(4, 4, 0, 0);
            chkAtTarget.Name = "chkAtTarget";
            chkAtTarget.Padding = new Padding(5, 1, 0, 0);
            chkAtTarget.Size = new Size(157, 20);
            chkAtTarget.TabIndex = 3;
            chkAtTarget.Text = "At Target";
            chkAtTarget.UseVisualStyleBackColor = true;
            // 
            // chkAtMax
            // 
            chkAtMax.AutoSize = true;
            chkAtMax.Checked = true;
            chkAtMax.CheckState = CheckState.Checked;
            tlpAlerts.SetColumnSpan(chkAtMax, 2);
            chkAtMax.Dock = DockStyle.Top;
            chkAtMax.FlatStyle = FlatStyle.Flat;
            chkAtMax.ForeColor = Color.FromArgb(210, 215, 225);
            chkAtMax.Location = new Point(4, 106);
            chkAtMax.Margin = new Padding(4, 4, 0, 0);
            chkAtMax.Name = "chkAtMax";
            chkAtMax.Padding = new Padding(5, 1, 0, 0);
            chkAtMax.Size = new Size(157, 20);
            chkAtMax.TabIndex = 4;
            chkAtMax.Text = "At Max";
            chkAtMax.UseVisualStyleBackColor = true;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(21, 23, 28);
            panelBottom.Controls.Add(footerLayout);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 750);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1300, 70);
            panelBottom.TabIndex = 3;
            // 
            // footerLayout
            // 
            footerLayout.ColumnCount = 5;
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            footerLayout.Controls.Add(lblAutoSaveStatus, 0, 0);
            footerLayout.Controls.Add(lblFooterMin, 1, 0);
            footerLayout.Controls.Add(lblFooterAvg, 2, 0);
            footerLayout.Controls.Add(lblFooterPoints, 3, 0);
            footerLayout.Controls.Add(lblThemeIndicator, 4, 0);
            footerLayout.Dock = DockStyle.Fill;
            footerLayout.Location = new Point(0, 0);
            footerLayout.Name = "footerLayout";
            footerLayout.RowCount = 1;
            footerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            footerLayout.Size = new Size(1300, 70);
            footerLayout.TabIndex = 0;
            // 
            // lblAutoSaveStatus
            // 
            lblAutoSaveStatus.AutoSize = true;
            lblAutoSaveStatus.Dock = DockStyle.Fill;
            lblAutoSaveStatus.Font = new Font("Segoe UI", 8F);
            lblAutoSaveStatus.ForeColor = Color.Green;
            lblAutoSaveStatus.Location = new Point(3, 0);
            lblAutoSaveStatus.Name = "lblAutoSaveStatus";
            lblAutoSaveStatus.Size = new Size(319, 70);
            lblAutoSaveStatus.TabIndex = 0;
            lblAutoSaveStatus.Text = "Auto-save • Enabled";
            lblAutoSaveStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFooterMin
            // 
            lblFooterMin.AutoSize = true;
            lblFooterMin.Dock = DockStyle.Fill;
            lblFooterMin.Font = new Font("Segoe UI", 8F);
            lblFooterMin.ForeColor = Color.White;
            lblFooterMin.Location = new Point(328, 0);
            lblFooterMin.Name = "lblFooterMin";
            lblFooterMin.Size = new Size(254, 70);
            lblFooterMin.TabIndex = 1;
            lblFooterMin.Text = "Min: 0.00";
            lblFooterMin.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFooterAvg
            // 
            lblFooterAvg.AutoSize = true;
            lblFooterAvg.Dock = DockStyle.Fill;
            lblFooterAvg.Font = new Font("Segoe UI", 8F);
            lblFooterAvg.ForeColor = Color.White;
            lblFooterAvg.Location = new Point(588, 0);
            lblFooterAvg.Name = "lblFooterAvg";
            lblFooterAvg.Size = new Size(254, 70);
            lblFooterAvg.TabIndex = 2;
            lblFooterAvg.Text = "Avg: 0.00";
            lblFooterAvg.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFooterPoints
            // 
            lblFooterPoints.AutoSize = true;
            lblFooterPoints.Dock = DockStyle.Fill;
            lblFooterPoints.Font = new Font("Segoe UI", 8F);
            lblFooterPoints.ForeColor = Color.White;
            lblFooterPoints.Location = new Point(848, 0);
            lblFooterPoints.Name = "lblFooterPoints";
            lblFooterPoints.Size = new Size(254, 70);
            lblFooterPoints.TabIndex = 3;
            lblFooterPoints.Text = "Points: 0";
            lblFooterPoints.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblThemeIndicator
            // 
            lblThemeIndicator.AutoSize = true;
            lblThemeIndicator.Dock = DockStyle.Fill;
            lblThemeIndicator.Font = new Font("Segoe UI", 8F);
            lblThemeIndicator.ForeColor = Color.Black;
            lblThemeIndicator.Location = new Point(1108, 0);
            lblThemeIndicator.Name = "lblThemeIndicator";
            lblThemeIndicator.Size = new Size(189, 70);
            lblThemeIndicator.TabIndex = 4;
            lblThemeIndicator.Text = "✓ Light theme";
            lblThemeIndicator.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(26, 29, 36);
            panelHeader.Controls.Add(headerLayoutPanel);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 40);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1300, 50);
            panelHeader.TabIndex = 4;
            // 
            // headerLayoutPanel
            // 
            headerLayoutPanel.ColumnCount = 3;
            headerLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            headerLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            headerLayoutPanel.Controls.Add(headerLeftFlowPanel, 0, 0);
            headerLayoutPanel.Controls.Add(headerRightFlowPanel, 2, 0);
            headerLayoutPanel.Dock = DockStyle.Fill;
            headerLayoutPanel.Location = new Point(0, 0);
            headerLayoutPanel.Margin = new Padding(0);
            headerLayoutPanel.Name = "headerLayoutPanel";
            headerLayoutPanel.RowCount = 1;
            headerLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            headerLayoutPanel.Size = new Size(1300, 50);
            headerLayoutPanel.TabIndex = 0;
            // 
            // headerLeftFlowPanel
            // 
            headerLeftFlowPanel.AutoSize = true;
            headerLeftFlowPanel.Controls.Add(appIcon);
            headerLeftFlowPanel.Controls.Add(lblAppTitle);
            headerLeftFlowPanel.Controls.Add(lblSessionTime);
            headerLeftFlowPanel.Dock = DockStyle.Fill;
            headerLeftFlowPanel.Location = new Point(0, 0);
            headerLeftFlowPanel.Margin = new Padding(0);
            headerLeftFlowPanel.Name = "headerLeftFlowPanel";
            headerLeftFlowPanel.Padding = new Padding(12, 0, 0, 0);
            headerLeftFlowPanel.Size = new Size(333, 50);
            headerLeftFlowPanel.TabIndex = 0;
            headerLeftFlowPanel.WrapContents = false;
            // 
            // appIcon
            // 
            appIcon.BackColor = Color.Transparent;
            appIcon.Location = new Point(12, 15);
            appIcon.Margin = new Padding(0, 15, 8, 0);
            appIcon.Name = "appIcon";
            appIcon.Size = new Size(20, 20);
            appIcon.TabIndex = 0;
            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 10F);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(40, 15);
            lblAppTitle.Margin = new Padding(0, 15, 12, 0);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(166, 19);
            lblAppTitle.TabIndex = 1;
            lblAppTitle.Text = "Alicat Pressure Controller:";
            // 
            // lblSessionTime
            // 
            lblSessionTime.AutoSize = true;
            lblSessionTime.Font = new Font("Segoe UI", 10F);
            lblSessionTime.ForeColor = Color.White;
            lblSessionTime.Location = new Point(218, 15);
            lblSessionTime.Margin = new Padding(0, 15, 0, 0);
            lblSessionTime.Name = "lblSessionTime";
            lblSessionTime.Size = new Size(115, 19);
            lblSessionTime.TabIndex = 2;
            lblSessionTime.Text = "Session: 00:00:00";
            // 
            // headerRightFlowPanel
            // 
            headerRightFlowPanel.AutoSize = true;
            headerRightFlowPanel.Controls.Add(btnPause);
            headerRightFlowPanel.Controls.Add(btnExport);
            headerRightFlowPanel.Dock = DockStyle.Fill;
            headerRightFlowPanel.Location = new Point(1130, 0);
            headerRightFlowPanel.Margin = new Padding(0);
            headerRightFlowPanel.Name = "headerRightFlowPanel";
            headerRightFlowPanel.Padding = new Padding(0, 0, 12, 0);
            headerRightFlowPanel.Size = new Size(170, 50);
            headerRightFlowPanel.TabIndex = 1;
            headerRightFlowPanel.WrapContents = false;
            // 
            // btnPause
            // 
            btnPause.BackColor = Color.FromArgb(42, 45, 53);
            btnPause.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnPause.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 38, 47);
            btnPause.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 53, 61);
            btnPause.FlatStyle = FlatStyle.Flat;
            btnPause.ForeColor = Color.FromArgb(200, 205, 215);
            btnPause.Location = new Point(0, 11);
            btnPause.Margin = new Padding(0, 11, 8, 0);
            btnPause.Name = "btnPause";
            btnPause.Padding = new Padding(8, 0, 8, 0);
            btnPause.Size = new Size(75, 28);
            btnPause.TabIndex = 0;
            btnPause.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(42, 45, 53);
            btnExport.FlatAppearance.BorderColor = Color.FromArgb(80, 85, 95);
            btnExport.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 38, 47);
            btnExport.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 53, 61);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.ForeColor = Color.FromArgb(200, 205, 215);
            btnExport.Location = new Point(83, 11);
            btnExport.Margin = new Padding(0, 11, 0, 0);
            btnExport.Name = "btnExport";
            btnExport.Padding = new Padding(8, 0, 8, 0);
            btnExport.Size = new Size(75, 28);
            btnExport.TabIndex = 1;
            btnExport.UseVisualStyleBackColor = false;
            // 
            // panelChartHeader
            // 
            panelChartHeader.BackColor = Color.FromArgb(21, 23, 28);
            panelChartHeader.Controls.Add(panelChartButtons);
            panelChartHeader.Controls.Add(flowLegend);
            panelChartHeader.Dock = DockStyle.Top;
            panelChartHeader.Location = new Point(0, 0);
            panelChartHeader.Name = "panelChartHeader";
            panelChartHeader.Size = new Size(1300, 40);
            panelChartHeader.TabIndex = 5;
            // 
            // panelChartButtons
            // 
            panelChartButtons.Dock = DockStyle.Right;
            panelChartButtons.Location = new Point(1020, 0);
            panelChartButtons.Name = "panelChartButtons";
            panelChartButtons.Size = new Size(280, 40);
            panelChartButtons.TabIndex = 1;
            // 
            // flowLegend
            // 
            flowLegend.AutoSize = true;
            flowLegend.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLegend.Controls.Add(lblLegendCurrent);
            flowLegend.Controls.Add(lblLegendTarget);
            flowLegend.Controls.Add(lblLegendMin);
            flowLegend.Controls.Add(lblLegendMax);
            flowLegend.Dock = DockStyle.Left;
            flowLegend.Location = new Point(0, 0);
            flowLegend.Margin = new Padding(0);
            flowLegend.Name = "flowLegend";
            flowLegend.Padding = new Padding(12, 10, 0, 0);
            flowLegend.Size = new Size(244, 40);
            flowLegend.TabIndex = 0;
            flowLegend.WrapContents = false;
            // 
            // lblLegendCurrent
            // 
            lblLegendCurrent.AutoSize = true;
            lblLegendCurrent.ForeColor = Color.White;
            lblLegendCurrent.Location = new Point(15, 10);
            lblLegendCurrent.Name = "lblLegendCurrent";
            lblLegendCurrent.Padding = new Padding(16, 0, 0, 0);
            lblLegendCurrent.Size = new Size(63, 15);
            lblLegendCurrent.TabIndex = 0;
            lblLegendCurrent.Text = "Current";
            // 
            // lblLegendTarget
            // 
            lblLegendTarget.AutoSize = true;
            lblLegendTarget.ForeColor = Color.White;
            lblLegendTarget.Location = new Point(84, 10);
            lblLegendTarget.Name = "lblLegendTarget";
            lblLegendTarget.Padding = new Padding(16, 0, 0, 0);
            lblLegendTarget.Size = new Size(56, 15);
            lblLegendTarget.TabIndex = 1;
            lblLegendTarget.Text = "Target";
            // 
            // lblLegendMin
            // 
            lblLegendMin.AutoSize = true;
            lblLegendMin.ForeColor = Color.White;
            lblLegendMin.Location = new Point(146, 10);
            lblLegendMin.Name = "lblLegendMin";
            lblLegendMin.Padding = new Padding(16, 0, 0, 0);
            lblLegendMin.Size = new Size(44, 15);
            lblLegendMin.TabIndex = 2;
            lblLegendMin.Text = "Min";
            // 
            // lblLegendMax
            // 
            lblLegendMax.AutoSize = true;
            lblLegendMax.ForeColor = Color.White;
            lblLegendMax.Location = new Point(196, 10);
            lblLegendMax.Name = "lblLegendMax";
            lblLegendMax.Padding = new Padding(16, 0, 0, 0);
            lblLegendMax.Size = new Size(45, 15);
            lblLegendMax.TabIndex = 3;
            lblLegendMax.Text = "Max";
            // 
            // btnChartReset
            // 
            btnChartReset.AutoSize = true;
            btnChartReset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnChartReset.BackColor = Color.FromArgb(30, 33, 40);
            btnChartReset.FlatAppearance.BorderColor = Color.FromArgb(70, 75, 85);
            btnChartReset.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 49, 58);
            btnChartReset.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 39, 48);
            btnChartReset.FlatStyle = FlatStyle.Flat;
            btnChartReset.ForeColor = Color.FromArgb(220, 224, 232);
            btnChartReset.Location = new Point(4, 4);
            btnChartReset.Margin = new Padding(4);
            btnChartReset.Name = "btnChartReset";
            btnChartReset.Padding = new Padding(6, 4, 6, 4);
            btnChartReset.Size = new Size(59, 35);
            btnChartReset.TabIndex = 0;
            btnChartReset.Text = "Reset";
            btnChartReset.UseVisualStyleBackColor = false;
            // 
            // btnFullscreen
            // 
            btnFullscreen.AutoSize = true;
            btnFullscreen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFullscreen.BackColor = Color.FromArgb(30, 33, 40);
            btnFullscreen.FlatAppearance.BorderColor = Color.FromArgb(70, 75, 85);
            btnFullscreen.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 49, 59);
            btnFullscreen.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 39, 48);
            btnFullscreen.FlatStyle = FlatStyle.Flat;
            btnFullscreen.ForeColor = Color.FromArgb(220, 224, 232);
            btnFullscreen.Location = new Point(71, 4);
            btnFullscreen.Margin = new Padding(4);
            btnFullscreen.Name = "btnFullscreen";
            btnFullscreen.Padding = new Padding(6, 4, 6, 4);
            btnFullscreen.Size = new Size(84, 35);
            btnFullscreen.TabIndex = 1;
            btnFullscreen.Text = "Fullscreen";
            btnFullscreen.UseVisualStyleBackColor = false;
            // 
            // panelCenter
            // 
            panelCenter.BackColor = Color.FromArgb(21, 23, 28);
            panelCenter.Controls.Add(chartPressure);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(260, 90);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(780, 700);
            panelCenter.TabIndex = 6;
            // 
            // lblComPort
            // 
            lblComPort.Location = new Point(0, 0);
            lblComPort.Name = "lblComPort";
            lblComPort.Size = new Size(100, 23);
            lblComPort.TabIndex = 0;
            // 
            // lblHotkeys
            // 
            lblHotkeys.Location = new Point(0, 0);
            lblHotkeys.Name = "lblHotkeys";
            lblHotkeys.Size = new Size(100, 23);
            lblHotkeys.TabIndex = 0;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(0, 0);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 0;
            // 
            // btnFullscreenHeader
            // 
            btnFullscreenHeader.Location = new Point(0, 0);
            btnFullscreenHeader.Name = "btnFullscreenHeader";
            btnFullscreenHeader.Size = new Size(75, 23);
            btnFullscreenHeader.TabIndex = 0;
            // 
            // lblFooterMax
            // 
            lblFooterMax.Location = new Point(0, 0);
            lblFooterMax.Name = "lblFooterMax";
            lblFooterMax.Size = new Size(100, 23);
            lblFooterMax.TabIndex = 0;
            // 
            // 
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 19, 23);
            ClientSize = new Size(1300, 820);
            Controls.Add(panelCenter);
            Controls.Add(panelChartHeader);
            Controls.Add(panelBottom);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Controls.Add(panelHeader);
            Name = "GraphForm";
            Text = "Pressure Graph";
            panelLeft.ResumeLayout(false);
            tlpLeft.ResumeLayout(false);
            tlpLeft.PerformLayout();
            pnlLiveStatus.ResumeLayout(false);
            tlpSessionStats.ResumeLayout(false);
            tlpSessionStats.PerformLayout();
            tlpGoToTarget.ResumeLayout(false);
            tlpGoToTarget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTargets).EndInit();
            tlpTargetInputs.ResumeLayout(false);
            tlpTargetInputs.PerformLayout();
            tlpControlButtons.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panelRight.ResumeLayout(false);
            tableSettings.ResumeLayout(false);
            tlpTimeWindow.ResumeLayout(false);
            tlpTimeWindow.PerformLayout();
            tlpGrid.ResumeLayout(false);
            tlpGrid.PerformLayout();
            tlpThresholds.ResumeLayout(false);
            tlpThresholds.PerformLayout();
            tlpDisplay.ResumeLayout(false);
            tlpDisplay.PerformLayout();
            tlpAlerts.ResumeLayout(false);
            tlpAlerts.PerformLayout();
            panelBottom.ResumeLayout(false);
            footerLayout.ResumeLayout(false);
            footerLayout.PerformLayout();
            panelHeader.ResumeLayout(false);
            headerLayoutPanel.ResumeLayout(false);
            headerLayoutPanel.PerformLayout();
            headerLeftFlowPanel.ResumeLayout(false);
            headerLeftFlowPanel.PerformLayout();
            headerRightFlowPanel.ResumeLayout(false);
            panelChartHeader.ResumeLayout(false);
            panelChartHeader.PerformLayout();
            flowLegend.ResumeLayout(false);
            flowLegend.PerformLayout();
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart chartPressure;
        private Panel panelLeft;
        private Panel panelRight;
        private Panel panelBottom;
        private Panel panelHeader;
        private Panel panelChartHeader;
        private Panel panelCenter;
        private FlowLayoutPanel flowLegend;
        private Label lblLegendCurrent;
        private Label lblLegendTarget;
        private Label lblLegendMin;
        private Label lblLegendMax;
        private FlowLayoutPanel panelChartButtons;
        private Button btnChartReset;
        private Button btnFullscreen;
        private TableLayoutPanel tableSettings;
        private TableLayoutPanel tlpTimeWindow;
        private TableLayoutPanel tlpAlerts;
        private TableLayoutPanel tlpDisplay;
        private TableLayoutPanel tlpThresholds;
        private TableLayoutPanel tlpGrid;
        private Label lblGridTitle;
        private Label lblYStep;
        private ComboBox cmbYStep;
        private Label lblThresholdsTitle;
        private Label lblAlertsTitle;
        private Label lblDisplayTitle;
        private Label lblMinimum;
        private Label lblMaximum;
        private Label lblTimeWindowTitle;
        private ComboBox cmbDuration;
        private Label lblDuration;
        private TextBox txtMaxThreshold;
        private Panel panelMaxIndicator;
        private TextBox txtMinThreshold;
        private Panel panelMinIndicator;
        private CheckBox chkShowGrid;
        private CheckBox chkShowTarget;
        private CheckBox chkShowMax;
        private CheckBox chkShowMin;
        private CheckBox chkSound;
        private CheckBox chkAtTarget;
        private CheckBox chkAtMax;
        private Panel panelAlertsIcon;
        private TableLayoutPanel tlpLeft;
        private Panel pnlLiveStatus;
        private Panel pnlLiveStatusDivider;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnEmergency;
        private Label lblTarget;
        private Label lblTargetLabel;
        private Label lblTargetValue;
        private Label lblDelta;
        private Label lblRate;
        private Label lblETA;
        private Label lblETALabel;
        private Label lblETAValue;
        private Label lblTrend;
        // Session Stats
        private TableLayoutPanel tlpSessionStats;
        private Label lblSessionStatsTitle;
        private Label lblMinLabel;
        private Label lblMinValue;
        private Label lblMaxLabel;
        private Label lblMaxValue;
        private Label lblAvgLabel;
        private Label lblAvgValue;
        private Label lblStdDevLabel;
        private Label lblStdDevValue;
        private Label lblPointsLabel;
        private Label lblPointsValue;
        private Label lblDurationLabel;
        private Label lblDurationValue;
        private Label lblSampleRateLabel;
        private Label lblSampleRateValue;
        // GO TO TARGET section
        private TableLayoutPanel tlpGoToTarget;
        private Label lblGoToTargetTitle;
        private DataGridView dgvTargets;
        private TableLayoutPanel tlpTargetInputs;
        private Label lblPSILabel;
        private TextBox txtPSI;
        private Label lblHoldLabel;
        private TextBox txtHold;
        private Button btnAddTarget;
        private FlowLayoutPanel tlpButtonsRow;
        private Button btnClearAll;
        private Button btnEdit;
        private Label lblProgress;
        private ProgressBar progressBarProgress;
        private Label lblHoldTimer;
        private ProgressBar progressBarHold;
        private FlowLayoutPanel tlpControlButtons;
        private Button btnPlay;
        private Button btnPauseTarget;
        private Button btnStop;
        private Button btnSkip;
        // Live Status improvements
        private Label lblCurrentPressureLarge;
        private Label lblCurrentUnit;
        private Panel pnlWarnIndicator;
        // Header elements
        private Label lblComPort;
        private Label lblSessionTime;
        private Label lblHotkeys;
        private Button btnPause;
        private Button btnExport;
        private Button btnReset;
        private Button btnFullscreenHeader;
        // Header layout panels
        private TableLayoutPanel headerLayoutPanel;
        private FlowLayoutPanel headerLeftFlowPanel;
        private FlowLayoutPanel headerRightFlowPanel;
        private Panel appIcon;
        private Label lblAppTitle;
        // Connection status panel (will be added to headerLeftFlowPanel in SetupHeaderLayout)
        // Footer elements
        private Label lblAutoSaveStatus;
        private Label lblFooterMin;
        private Label lblFooterMax;
        private Label lblFooterAvg;
        private Label lblFooterPoints;
        private Label lblThemeIndicator;
        // Target Control elements
        private TableLayoutPanel footerLayout;
    }
}