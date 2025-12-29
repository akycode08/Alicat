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
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            chartPressure = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            panelLeft = new Panel();
            tlpLeft = new TableLayoutPanel();
            tlpLiveStatus = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblTarget = new Label();
            lblDelta = new Label();
            lblRate = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblETA = new Label();
            lblTrend = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblCurrentPressureLarge = new Label();
            lblCurrentUnit = new Label();
            pnlWarnIndicator = new Panel();
            tlpSessionStats = new TableLayoutPanel();
            lblSessionStatsTitle = new Label();
            lblMinLabel = new Label();
            lblMinValue = new Label();
            lblMaxLabel = new Label();
            lblMaxValue = new Label();
            lblAvgLabel = new Label();
            lblAvgValue = new Label();
            lblStdDevLabel = new Label();
            lblStdDevValue = new Label();
            lblPointsLabel = new Label();
            lblPointsValue = new Label();
            lblDurationLabel = new Label();
            lblDurationValue = new Label();
            lblSampleRateLabel = new Label();
            lblSampleRateValue = new Label();
            btnEmergency = new Button();
            panelRight = new Panel();
            tableSettings = new TableLayoutPanel();
            tlpAlerts = new TableLayoutPanel();
            lblFlash = new Label();
            lblSound = new Label();
            lblAlertsTitle = new Label();
            chkSound = new CheckBox();
            chkFlash = new CheckBox();
            tlpDisplay = new TableLayoutPanel();
            lblDisplayTitle = new Label();
            chkShowGrid = new CheckBox();
            chkSmoothing = new CheckBox();
            tlpThresholds = new TableLayoutPanel();
            lblMinimum = new Label();
            lblMaximum = new Label();
            lblThresholdsTitle = new Label();
            nudMaximum = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            tlpGrid = new TableLayoutPanel();
            cmbXStep = new ComboBox();
            lblGridTitle = new Label();
            cmbYStep = new ComboBox();
            lblXStep = new Label();
            lblYStep = new Label();
            tlpTimeWindow = new TableLayoutPanel();
            lblTimeWindowTitle = new Label();
            cmbDuration = new ComboBox();
            lblDuration = new Label();
            tlpTargetControl = new TableLayoutPanel();
            lblTargetControlTitle = new Label();
            txtTargetValue = new TextBox();
            btnGoTarget = new Button();
            panelBottom = new Panel();
            footerLayout = new TableLayoutPanel();
            lblAutoSaveStatus = new Label();
            lblFooterMin = new Label();
            lblFooterAvg = new Label();
            lblFooterPoints = new Label();
            lblThemeIndicator = new Label();
            panelHeader = new Panel();
            panelChartHeader = new Panel();
            panelChartButtons = new FlowLayoutPanel();
            btnChartReset = new Button();
            btnFullscreen = new Button();
            flowLegend = new FlowLayoutPanel();
            lblLegendCurrent = new Label();
            lblLegendTarget = new Label();
            panelCenter = new Panel();
            lblComPort = new Label();
            lblSessionTime = new Label();
            lblHotkeys = new Label();
            btnPause = new Button();
            btnExport = new Button();
            btnReset = new Button();
            btnFullscreenHeader = new Button();
            lblFooterMax = new Label();
            panelLeft.SuspendLayout();
            tlpLeft.SuspendLayout();
            tlpLiveStatus.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tlpSessionStats.SuspendLayout();
            panelRight.SuspendLayout();
            tableSettings.SuspendLayout();
            tlpAlerts.SuspendLayout();
            tlpDisplay.SuspendLayout();
            tlpThresholds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaximum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            tlpGrid.SuspendLayout();
            tlpTimeWindow.SuspendLayout();
            tlpTargetControl.SuspendLayout();
            panelBottom.SuspendLayout();
            footerLayout.SuspendLayout();
            panelChartHeader.SuspendLayout();
            panelChartButtons.SuspendLayout();
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
            chartPressure.Size = new Size(554, 373);
            chartPressure.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(21, 23, 28);
            panelLeft.Controls.Add(tlpLeft);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 50);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(200, 483);
            panelLeft.TabIndex = 1;
            // 
            // tlpLeft
            // 
            tlpLeft.ColumnCount = 1;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.Controls.Add(tlpLiveStatus, 0, 1);
            tlpLeft.Controls.Add(tlpSessionStats, 0, 2);
            tlpLeft.Controls.Add(btnEmergency, 0, 3);
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.Location = new Point(0, 0);
            tlpLeft.Name = "tlpLeft";
            tlpLeft.RowCount = 4;
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 4F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 46F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpLeft.Size = new Size(200, 483);
            tlpLeft.TabIndex = 0;
            // 
            // tlpLiveStatus
            // 
            tlpLiveStatus.ColumnCount = 1;
            tlpLiveStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLiveStatus.Controls.Add(tableLayoutPanel1, 0, 1);
            tlpLiveStatus.Controls.Add(tableLayoutPanel2, 0, 2);
            tlpLiveStatus.Controls.Add(tableLayoutPanel3, 0, 0);
            tlpLiveStatus.Dock = DockStyle.Fill;
            tlpLiveStatus.Location = new Point(3, 22);
            tlpLiveStatus.Name = "tlpLiveStatus";
            tlpLiveStatus.RowCount = 3;
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tlpLiveStatus.Size = new Size(194, 216);
            tlpLiveStatus.TabIndex = 0;
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
            // lblTarget
            // 
            lblTarget.AutoSize = true;
            lblTarget.Dock = DockStyle.Fill;
            lblTarget.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTarget.ForeColor = Color.FromArgb(120, 125, 140);
            lblTarget.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblTarget.Location = new Point(5, 5);
            lblTarget.Margin = new Padding(5, 5, 0, 0);
            lblTarget.Name = "lblTarget";
            lblTarget.Size = new Size(89, 21);
            lblTarget.TabIndex = 13;
            lblTarget.Text = "Target";
            lblTarget.Click += label1_Click;
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
            // lblETA
            // 
            lblETA.AutoSize = true;
            lblETA.BackColor = Color.FromArgb(21, 23, 28);
            lblETA.Dock = DockStyle.Fill;
            lblETA.ForeColor = Color.FromArgb(120, 125, 140);
            lblETA.Location = new Point(0, 0);
            lblETA.Margin = new Padding(0);
            lblETA.Name = "lblETA";
            lblETA.Size = new Size(94, 16);
            lblETA.TabIndex = 0;
            lblETA.Text = "ETA";
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
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(lblCurrentPressureLarge, 0, 0);
            tableLayoutPanel3.Controls.Add(lblCurrentUnit, 0, 1);
            tableLayoutPanel3.Controls.Add(pnlWarnIndicator, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Size = new Size(188, 102);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // lblCurrentPressureLarge
            // 
            lblCurrentPressureLarge.Dock = DockStyle.Fill;
            lblCurrentPressureLarge.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentPressureLarge.ForeColor = Color.White;
            lblCurrentPressureLarge.Location = new Point(3, 0);
            lblCurrentPressureLarge.Name = "lblCurrentPressureLarge";
            lblCurrentPressureLarge.Size = new Size(182, 51);
            lblCurrentPressureLarge.TabIndex = 0;
            lblCurrentPressureLarge.Text = "0.00";
            lblCurrentPressureLarge.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentPressureLarge.Click += lblCurrentPressureLarge_Click;
            // 
            // lblCurrentUnit
            // 
            lblCurrentUnit.AutoSize = true;
            lblCurrentUnit.Dock = DockStyle.Fill;
            lblCurrentUnit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCurrentUnit.ForeColor = Color.FromArgb(120, 125, 140);
            lblCurrentUnit.Location = new Point(3, 51);
            lblCurrentUnit.Name = "lblCurrentUnit";
            lblCurrentUnit.Size = new Size(182, 25);
            lblCurrentUnit.TabIndex = 1;
            lblCurrentUnit.Text = "PSIG";
            lblCurrentUnit.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentUnit.Click += lblCurrentUnit_Click;
            // 
            // pnlWarnIndicator
            // 
            pnlWarnIndicator.BackColor = Color.FromArgb(191, 0, 0);
            pnlWarnIndicator.Dock = DockStyle.Fill;
            pnlWarnIndicator.Location = new Point(3, 79);
            pnlWarnIndicator.Name = "pnlWarnIndicator";
            pnlWarnIndicator.Size = new Size(182, 25);
            pnlWarnIndicator.TabIndex = 2;
            pnlWarnIndicator.Visible = false;
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
            tlpSessionStats.Controls.Add(lblStdDevLabel, 0, 4);
            tlpSessionStats.Controls.Add(lblStdDevValue, 1, 4);
            tlpSessionStats.Controls.Add(lblPointsLabel, 0, 5);
            tlpSessionStats.Controls.Add(lblPointsValue, 1, 5);
            tlpSessionStats.Controls.Add(lblDurationLabel, 0, 6);
            tlpSessionStats.Controls.Add(lblDurationValue, 1, 6);
            tlpSessionStats.Controls.Add(lblSampleRateLabel, 0, 7);
            tlpSessionStats.Controls.Add(lblSampleRateValue, 1, 7);
            tlpSessionStats.Dock = DockStyle.Fill;
            tlpSessionStats.Location = new Point(3, 244);
            tlpSessionStats.Name = "tlpSessionStats";
            tlpSessionStats.RowCount = 8;
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tlpSessionStats.Size = new Size(194, 187);
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
            lblSessionStatsTitle.Size = new Size(188, 25);
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
            lblMinLabel.Size = new Size(91, 23);
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
            lblMinValue.Location = new Point(100, 25);
            lblMinValue.Name = "lblMinValue";
            lblMinValue.Size = new Size(91, 23);
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
            lblMaxLabel.Location = new Point(3, 48);
            lblMaxLabel.Name = "lblMaxLabel";
            lblMaxLabel.Size = new Size(91, 23);
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
            lblMaxValue.Location = new Point(100, 48);
            lblMaxValue.Name = "lblMaxValue";
            lblMaxValue.Size = new Size(91, 23);
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
            lblAvgLabel.Location = new Point(3, 71);
            lblAvgLabel.Name = "lblAvgLabel";
            lblAvgLabel.Size = new Size(91, 23);
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
            lblAvgValue.Location = new Point(100, 71);
            lblAvgValue.Name = "lblAvgValue";
            lblAvgValue.Size = new Size(91, 23);
            lblAvgValue.TabIndex = 6;
            lblAvgValue.Text = "0.00";
            lblAvgValue.TextAlign = ContentAlignment.MiddleRight;
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
            // lblPointsLabel
            // 
            lblPointsLabel.AutoSize = true;
            lblPointsLabel.Dock = DockStyle.Fill;
            lblPointsLabel.Font = new Font("Segoe UI", 8F);
            lblPointsLabel.ForeColor = Color.FromArgb(120, 125, 140);
            lblPointsLabel.Location = new Point(3, 117);
            lblPointsLabel.Name = "lblPointsLabel";
            lblPointsLabel.Size = new Size(91, 23);
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
            lblPointsValue.Location = new Point(100, 117);
            lblPointsValue.Name = "lblPointsValue";
            lblPointsValue.Size = new Size(91, 23);
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
            lblDurationLabel.Location = new Point(3, 140);
            lblDurationLabel.Name = "lblDurationLabel";
            lblDurationLabel.Size = new Size(91, 23);
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
            lblDurationValue.Location = new Point(100, 140);
            lblDurationValue.Name = "lblDurationValue";
            lblDurationValue.Size = new Size(91, 23);
            lblDurationValue.TabIndex = 12;
            lblDurationValue.Text = "00:00";
            lblDurationValue.TextAlign = ContentAlignment.MiddleRight;
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
            // btnEmergency
            // 
            btnEmergency.Anchor = AnchorStyles.None;
            btnEmergency.AutoSize = true;
            btnEmergency.BackColor = Color.FromArgb(191, 0, 0);
            btnEmergency.ForeColor = Color.FromArgb(210, 215, 225);
            btnEmergency.Location = new Point(25, 437);
            btnEmergency.Name = "btnEmergency";
            btnEmergency.Size = new Size(150, 43);
            btnEmergency.TabIndex = 1;
            btnEmergency.Text = "Emergency Vent";
            btnEmergency.UseVisualStyleBackColor = false;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(21, 23, 28);
            panelRight.Controls.Add(tableSettings);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(754, 50);
            panelRight.Margin = new Padding(4);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(10, 4, 10, 4);
            panelRight.Size = new Size(200, 483);
            panelRight.TabIndex = 2;
            // 
            // tableSettings
            // 
            tableSettings.ColumnCount = 1;
            tableSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableSettings.Controls.Add(tlpAlerts, 0, 4);
            tableSettings.Controls.Add(tlpDisplay, 0, 3);
            tableSettings.Controls.Add(tlpThresholds, 0, 2);
            tableSettings.Controls.Add(tlpGrid, 0, 1);
            tableSettings.Controls.Add(tlpTimeWindow, 0, 0);
            tableSettings.Controls.Add(tlpTargetControl, 0, 5);
            tableSettings.Dock = DockStyle.Fill;
            tableSettings.Location = new Point(10, 4);
            tableSettings.Name = "tableSettings";
            tableSettings.RowCount = 6;
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableSettings.Size = new Size(180, 475);
            tableSettings.TabIndex = 0;
            // 
            // tlpAlerts
            // 
            tlpAlerts.BackColor = Color.FromArgb(32, 35, 44);
            tlpAlerts.ColumnCount = 2;
            tlpAlerts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpAlerts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpAlerts.Controls.Add(lblFlash, 0, 2);
            tlpAlerts.Controls.Add(lblSound, 0, 1);
            tlpAlerts.Controls.Add(lblAlertsTitle, 0, 0);
            tlpAlerts.Controls.Add(chkSound, 1, 1);
            tlpAlerts.Controls.Add(chkFlash, 1, 2);
            tlpAlerts.Dock = DockStyle.Fill;
            tlpAlerts.Location = new Point(3, 319);
            tlpAlerts.Name = "tlpAlerts";
            tlpAlerts.RowCount = 3;
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpAlerts.Size = new Size(174, 73);
            tlpAlerts.TabIndex = 4;
            // 
            // lblFlash
            // 
            lblFlash.AutoSize = true;
            lblFlash.Dock = DockStyle.Top;
            lblFlash.ForeColor = Color.FromArgb(210, 215, 225);
            lblFlash.Location = new Point(4, 50);
            lblFlash.Margin = new Padding(4, 4, 0, 0);
            lblFlash.Name = "lblFlash";
            lblFlash.Padding = new Padding(5, 5, 0, 0);
            lblFlash.Size = new Size(100, 20);
            lblFlash.TabIndex = 8;
            lblFlash.Text = "⚡Flash";
            // 
            // lblSound
            // 
            lblSound.AutoSize = true;
            lblSound.Dock = DockStyle.Top;
            lblSound.ForeColor = Color.FromArgb(210, 215, 225);
            lblSound.Location = new Point(4, 25);
            lblSound.Margin = new Padding(4, 4, 0, 0);
            lblSound.Name = "lblSound";
            lblSound.Padding = new Padding(5, 5, 0, 0);
            lblSound.Size = new Size(100, 20);
            lblSound.TabIndex = 8;
            lblSound.Text = "🔔 Sound";
            // 
            // lblAlertsTitle
            // 
            lblAlertsTitle.AutoSize = true;
            lblAlertsTitle.Dock = DockStyle.Top;
            lblAlertsTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAlertsTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblAlertsTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblAlertsTitle.Location = new Point(5, 5);
            lblAlertsTitle.Margin = new Padding(5, 5, 0, 0);
            lblAlertsTitle.Name = "lblAlertsTitle";
            lblAlertsTitle.Size = new Size(99, 13);
            lblAlertsTitle.TabIndex = 9;
            lblAlertsTitle.Text = "ALERTS";
            // 
            // chkSound
            // 
            chkSound.AutoSize = true;
            chkSound.Dock = DockStyle.Fill;
            chkSound.FlatAppearance.BorderSize = 0;
            chkSound.FlatStyle = FlatStyle.Flat;
            chkSound.Location = new Point(107, 24);
            chkSound.Name = "chkSound";
            chkSound.Size = new Size(64, 19);
            chkSound.TabIndex = 10;
            chkSound.UseVisualStyleBackColor = true;
            // 
            // chkFlash
            // 
            chkFlash.AutoSize = true;
            chkFlash.Dock = DockStyle.Fill;
            chkFlash.FlatAppearance.BorderSize = 0;
            chkFlash.FlatStyle = FlatStyle.Flat;
            chkFlash.Location = new Point(107, 49);
            chkFlash.Name = "chkFlash";
            chkFlash.Size = new Size(64, 21);
            chkFlash.TabIndex = 11;
            chkFlash.UseVisualStyleBackColor = true;
            // 
            // tlpDisplay
            // 
            tlpDisplay.BackColor = Color.FromArgb(32, 35, 44);
            tlpDisplay.ColumnCount = 2;
            tlpDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpDisplay.Controls.Add(lblDisplayTitle, 0, 0);
            tlpDisplay.Controls.Add(chkShowGrid, 0, 1);
            tlpDisplay.Controls.Add(chkSmoothing, 0, 2);
            tlpDisplay.Dock = DockStyle.Fill;
            tlpDisplay.Location = new Point(3, 240);
            tlpDisplay.Name = "tlpDisplay";
            tlpDisplay.RowCount = 3;
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpDisplay.Size = new Size(174, 73);
            tlpDisplay.TabIndex = 3;
            // 
            // lblDisplayTitle
            // 
            lblDisplayTitle.AutoSize = true;
            lblDisplayTitle.Dock = DockStyle.Top;
            lblDisplayTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDisplayTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblDisplayTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblDisplayTitle.Location = new Point(5, 5);
            lblDisplayTitle.Margin = new Padding(5, 5, 0, 0);
            lblDisplayTitle.Name = "lblDisplayTitle";
            lblDisplayTitle.Size = new Size(99, 13);
            lblDisplayTitle.TabIndex = 8;
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
            chkShowGrid.Location = new Point(4, 25);
            chkShowGrid.Margin = new Padding(4, 4, 0, 0);
            chkShowGrid.Name = "chkShowGrid";
            chkShowGrid.Padding = new Padding(5, 1, 0, 0);
            chkShowGrid.Size = new Size(100, 20);
            chkShowGrid.TabIndex = 9;
            chkShowGrid.Text = "Show Grid";
            chkShowGrid.UseVisualStyleBackColor = true;
            // 
            // chkSmoothing
            // 
            chkSmoothing.AutoSize = true;
            chkSmoothing.Checked = true;
            chkSmoothing.CheckState = CheckState.Checked;
            chkSmoothing.Dock = DockStyle.Top;
            chkSmoothing.FlatStyle = FlatStyle.Flat;
            chkSmoothing.ForeColor = Color.FromArgb(210, 215, 225);
            chkSmoothing.Location = new Point(4, 50);
            chkSmoothing.Margin = new Padding(4, 4, 0, 0);
            chkSmoothing.Name = "chkSmoothing";
            chkSmoothing.Padding = new Padding(5, 1, 0, 0);
            chkSmoothing.Size = new Size(100, 20);
            chkSmoothing.TabIndex = 10;
            chkSmoothing.Text = "Smoothing";
            chkSmoothing.UseVisualStyleBackColor = true;
            // 
            // tlpThresholds
            // 
            tlpThresholds.BackColor = Color.FromArgb(32, 35, 44);
            tlpThresholds.ColumnCount = 2;
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpThresholds.Controls.Add(lblMinimum, 0, 2);
            tlpThresholds.Controls.Add(lblMaximum, 0, 1);
            tlpThresholds.Controls.Add(lblThresholdsTitle, 0, 0);
            tlpThresholds.Controls.Add(nudMaximum, 1, 1);
            tlpThresholds.Controls.Add(numericUpDown2, 1, 2);
            tlpThresholds.Dock = DockStyle.Fill;
            tlpThresholds.Location = new Point(3, 161);
            tlpThresholds.Name = "tlpThresholds";
            tlpThresholds.RowCount = 3;
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpThresholds.Size = new Size(174, 73);
            tlpThresholds.TabIndex = 2;
            // 
            // lblMinimum
            // 
            lblMinimum.AutoSize = true;
            lblMinimum.Dock = DockStyle.Top;
            lblMinimum.ForeColor = Color.FromArgb(210, 215, 225);
            lblMinimum.Location = new Point(4, 50);
            lblMinimum.Margin = new Padding(4, 4, 0, 0);
            lblMinimum.Name = "lblMinimum";
            lblMinimum.Padding = new Padding(5, 5, 0, 0);
            lblMinimum.Size = new Size(100, 20);
            lblMinimum.TabIndex = 7;
            lblMinimum.Text = "Minimum";
            // 
            // lblMaximum
            // 
            lblMaximum.AutoSize = true;
            lblMaximum.Dock = DockStyle.Top;
            lblMaximum.ForeColor = Color.FromArgb(210, 215, 225);
            lblMaximum.Location = new Point(4, 25);
            lblMaximum.Margin = new Padding(4, 4, 0, 0);
            lblMaximum.Name = "lblMaximum";
            lblMaximum.Padding = new Padding(5, 5, 0, 0);
            lblMaximum.Size = new Size(100, 20);
            lblMaximum.TabIndex = 7;
            lblMaximum.Text = "Maximum";
            lblMaximum.Click += label3_Click_1;
            // 
            // lblThresholdsTitle
            // 
            lblThresholdsTitle.AutoSize = true;
            lblThresholdsTitle.Dock = DockStyle.Top;
            lblThresholdsTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThresholdsTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblThresholdsTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblThresholdsTitle.Location = new Point(5, 5);
            lblThresholdsTitle.Margin = new Padding(5, 5, 0, 0);
            lblThresholdsTitle.Name = "lblThresholdsTitle";
            lblThresholdsTitle.Size = new Size(99, 13);
            lblThresholdsTitle.TabIndex = 7;
            lblThresholdsTitle.Text = "THRESHOLDS";
            lblThresholdsTitle.Click += label3_Click;
            // 
            // nudMaximum
            // 
            nudMaximum.BackColor = Color.FromArgb(21, 23, 28);
            nudMaximum.BorderStyle = BorderStyle.None;
            nudMaximum.DecimalPlaces = 1;
            nudMaximum.Dock = DockStyle.Top;
            nudMaximum.ForeColor = Color.FromArgb(230, 80, 80);
            nudMaximum.Location = new Point(107, 24);
            nudMaximum.Maximum = new decimal(new int[] { 150, 0, 0, 0 });
            nudMaximum.Name = "nudMaximum";
            nudMaximum.Size = new Size(64, 19);
            nudMaximum.TabIndex = 8;
            nudMaximum.TextAlign = HorizontalAlignment.Center;
            nudMaximum.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // numericUpDown2
            // 
            numericUpDown2.BackColor = Color.FromArgb(21, 23, 28);
            numericUpDown2.BorderStyle = BorderStyle.None;
            numericUpDown2.DecimalPlaces = 1;
            numericUpDown2.Dock = DockStyle.Top;
            numericUpDown2.ForeColor = Color.FromArgb(255, 214, 69);
            numericUpDown2.Location = new Point(107, 49);
            numericUpDown2.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(64, 19);
            numericUpDown2.TabIndex = 9;
            numericUpDown2.TextAlign = HorizontalAlignment.Center;
            numericUpDown2.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // tlpGrid
            // 
            tlpGrid.BackColor = Color.FromArgb(32, 35, 44);
            tlpGrid.ColumnCount = 2;
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpGrid.Controls.Add(cmbXStep, 1, 1);
            tlpGrid.Controls.Add(lblGridTitle, 0, 0);
            tlpGrid.Controls.Add(cmbYStep, 1, 2);
            tlpGrid.Controls.Add(lblXStep, 0, 2);
            tlpGrid.Controls.Add(lblYStep, 0, 1);
            tlpGrid.Dock = DockStyle.Fill;
            tlpGrid.ForeColor = Color.FromArgb(120, 125, 140);
            tlpGrid.Location = new Point(3, 82);
            tlpGrid.Name = "tlpGrid";
            tlpGrid.RowCount = 3;
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpGrid.Size = new Size(174, 73);
            tlpGrid.TabIndex = 1;
            // 
            // cmbXStep
            // 
            cmbXStep.BackColor = Color.FromArgb(40, 43, 52);
            cmbXStep.Dock = DockStyle.Top;
            cmbXStep.ForeColor = Color.White;
            cmbXStep.FormattingEnabled = true;
            cmbXStep.Location = new Point(107, 24);
            cmbXStep.Name = "cmbXStep";
            cmbXStep.Size = new Size(64, 23);
            cmbXStep.TabIndex = 6;
            // 
            // lblGridTitle
            // 
            lblGridTitle.AutoSize = true;
            lblGridTitle.Dock = DockStyle.Top;
            lblGridTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGridTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblGridTitle.Location = new Point(5, 5);
            lblGridTitle.Margin = new Padding(5, 5, 0, 0);
            lblGridTitle.Name = "lblGridTitle";
            lblGridTitle.Size = new Size(99, 13);
            lblGridTitle.TabIndex = 0;
            lblGridTitle.Text = "GRID";
            // 
            // cmbYStep
            // 
            cmbYStep.BackColor = Color.FromArgb(40, 43, 52);
            cmbYStep.Dock = DockStyle.Top;
            cmbYStep.ForeColor = Color.White;
            cmbYStep.FormattingEnabled = true;
            cmbYStep.Location = new Point(107, 49);
            cmbYStep.Name = "cmbYStep";
            cmbYStep.Size = new Size(64, 23);
            cmbYStep.TabIndex = 4;
            // 
            // lblXStep
            // 
            lblXStep.AutoSize = true;
            lblXStep.Dock = DockStyle.Top;
            lblXStep.ForeColor = Color.FromArgb(210, 215, 225);
            lblXStep.Location = new Point(4, 50);
            lblXStep.Margin = new Padding(4, 4, 0, 0);
            lblXStep.Name = "lblXStep";
            lblXStep.Padding = new Padding(5, 5, 0, 0);
            lblXStep.Size = new Size(100, 20);
            lblXStep.TabIndex = 1;
            lblXStep.Text = "Y Step";
            lblXStep.Click += lblXStep_Click;
            // 
            // lblYStep
            // 
            lblYStep.AutoSize = true;
            lblYStep.Dock = DockStyle.Top;
            lblYStep.ForeColor = Color.FromArgb(210, 215, 225);
            lblYStep.Location = new Point(4, 25);
            lblYStep.Margin = new Padding(4, 4, 0, 0);
            lblYStep.Name = "lblYStep";
            lblYStep.Padding = new Padding(5, 5, 0, 0);
            lblYStep.Size = new Size(100, 20);
            lblYStep.TabIndex = 2;
            lblYStep.Text = "X Step";
            // 
            // tlpTimeWindow
            // 
            tlpTimeWindow.BackColor = Color.FromArgb(32, 35, 44);
            tlpTimeWindow.ColumnCount = 2;
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
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpTimeWindow.Size = new Size(174, 73);
            tlpTimeWindow.TabIndex = 0;
            // 
            // lblTimeWindowTitle
            // 
            lblTimeWindowTitle.AutoSize = true;
            lblTimeWindowTitle.Dock = DockStyle.Top;
            lblTimeWindowTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTimeWindowTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblTimeWindowTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblTimeWindowTitle.Location = new Point(5, 5);
            lblTimeWindowTitle.Margin = new Padding(5, 5, 0, 0);
            lblTimeWindowTitle.Name = "lblTimeWindowTitle";
            lblTimeWindowTitle.Size = new Size(99, 13);
            lblTimeWindowTitle.TabIndex = 12;
            lblTimeWindowTitle.Text = "TIME WINDOW";
            // 
            // cmbDuration
            // 
            cmbDuration.BackColor = Color.FromArgb(40, 43, 52);
            cmbDuration.Dock = DockStyle.Top;
            cmbDuration.ForeColor = Color.White;
            cmbDuration.FormattingEnabled = true;
            cmbDuration.Location = new Point(107, 24);
            cmbDuration.Name = "cmbDuration";
            cmbDuration.Size = new Size(64, 23);
            cmbDuration.TabIndex = 10;
            // 
            // lblDuration
            // 
            lblDuration.AutoSize = true;
            lblDuration.Dock = DockStyle.Top;
            lblDuration.ForeColor = Color.FromArgb(210, 215, 225);
            lblDuration.Location = new Point(4, 25);
            lblDuration.Margin = new Padding(4, 4, 0, 0);
            lblDuration.Name = "lblDuration";
            lblDuration.Padding = new Padding(5, 5, 0, 0);
            lblDuration.Size = new Size(100, 20);
            lblDuration.TabIndex = 11;
            lblDuration.Text = "Duration";
            // 
            // tlpTargetControl
            // 
            tlpTargetControl.BackColor = Color.FromArgb(32, 35, 44);
            tlpTargetControl.ColumnCount = 2;
            tlpTargetControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpTargetControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpTargetControl.Controls.Add(lblTargetControlTitle, 0, 0);
            tlpTargetControl.Controls.Add(txtTargetValue, 0, 1);
            tlpTargetControl.Controls.Add(btnGoTarget, 1, 1);
            tlpTargetControl.Dock = DockStyle.Fill;
            tlpTargetControl.Location = new Point(3, 398);
            tlpTargetControl.Name = "tlpTargetControl";
            tlpTargetControl.RowCount = 2;
            tlpTargetControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlpTargetControl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTargetControl.Size = new Size(174, 74);
            tlpTargetControl.TabIndex = 6;
            // 
            // lblTargetControlTitle
            // 
            lblTargetControlTitle.AutoSize = true;
            tlpTargetControl.SetColumnSpan(lblTargetControlTitle, 2);
            lblTargetControlTitle.Dock = DockStyle.Fill;
            lblTargetControlTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lblTargetControlTitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblTargetControlTitle.Location = new Point(3, 0);
            lblTargetControlTitle.Name = "lblTargetControlTitle";
            lblTargetControlTitle.Size = new Size(168, 25);
            lblTargetControlTitle.TabIndex = 0;
            lblTargetControlTitle.Text = "TARGET CONTROL";
            lblTargetControlTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTargetValue
            // 
            txtTargetValue.BackColor = Color.FromArgb(45, 48, 58);
            txtTargetValue.BorderStyle = BorderStyle.FixedSingle;
            txtTargetValue.Dock = DockStyle.Fill;
            txtTargetValue.Font = new Font("Segoe UI", 9F);
            txtTargetValue.ForeColor = Color.White;
            txtTargetValue.Location = new Point(3, 28);
            txtTargetValue.Name = "txtTargetValue";
            txtTargetValue.Size = new Size(98, 23);
            txtTargetValue.TabIndex = 1;
            txtTargetValue.Text = "120";
            // 
            // btnGoTarget
            // 
            btnGoTarget.BackColor = Color.FromArgb(240, 200, 0);
            btnGoTarget.Dock = DockStyle.Fill;
            btnGoTarget.FlatAppearance.BorderSize = 0;
            btnGoTarget.FlatStyle = FlatStyle.Flat;
            btnGoTarget.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnGoTarget.ForeColor = Color.Black;
            btnGoTarget.Location = new Point(107, 28);
            btnGoTarget.Name = "btnGoTarget";
            btnGoTarget.Size = new Size(64, 43);
            btnGoTarget.TabIndex = 2;
            btnGoTarget.Text = "GO TARGET";
            btnGoTarget.UseVisualStyleBackColor = false;
            btnGoTarget.Click += btnGoTarget_Click;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(21, 23, 28);
            panelBottom.Controls.Add(footerLayout);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(200, 463);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(554, 70);
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
            footerLayout.Size = new Size(554, 70);
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
            lblAutoSaveStatus.Size = new Size(132, 70);
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
            lblFooterMin.Location = new Point(141, 0);
            lblFooterMin.Name = "lblFooterMin";
            lblFooterMin.Size = new Size(104, 70);
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
            lblFooterAvg.Location = new Point(251, 0);
            lblFooterAvg.Name = "lblFooterAvg";
            lblFooterAvg.Size = new Size(104, 70);
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
            lblFooterPoints.Location = new Point(361, 0);
            lblFooterPoints.Name = "lblFooterPoints";
            lblFooterPoints.Size = new Size(104, 70);
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
            lblThemeIndicator.Location = new Point(471, 0);
            lblThemeIndicator.Name = "lblThemeIndicator";
            lblThemeIndicator.Size = new Size(80, 70);
            lblThemeIndicator.TabIndex = 4;
            lblThemeIndicator.Text = "✓ Light theme";
            lblThemeIndicator.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(21, 23, 28);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(954, 50);
            panelHeader.TabIndex = 4;
            // 
            // panelChartHeader
            // 
            panelChartHeader.BackColor = Color.FromArgb(21, 23, 28);
            panelChartHeader.Controls.Add(panelChartButtons);
            panelChartHeader.Controls.Add(flowLegend);
            panelChartHeader.Dock = DockStyle.Top;
            panelChartHeader.Location = new Point(200, 50);
            panelChartHeader.Name = "panelChartHeader";
            panelChartHeader.Size = new Size(554, 40);
            panelChartHeader.TabIndex = 5;
            // 
            // panelChartButtons
            // 
            panelChartButtons.Controls.Add(btnChartReset);
            panelChartButtons.Controls.Add(btnFullscreen);
            panelChartButtons.Dock = DockStyle.Right;
            panelChartButtons.Location = new Point(354, 0);
            panelChartButtons.Name = "panelChartButtons";
            panelChartButtons.Size = new Size(200, 40);
            panelChartButtons.TabIndex = 1;
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
            // flowLegend
            // 
            flowLegend.AutoSize = true;
            flowLegend.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLegend.Controls.Add(lblLegendCurrent);
            flowLegend.Controls.Add(lblLegendTarget);
            flowLegend.Dock = DockStyle.Left;
            flowLegend.Location = new Point(0, 0);
            flowLegend.Margin = new Padding(0);
            flowLegend.Name = "flowLegend";
            flowLegend.Padding = new Padding(12, 10, 0, 0);
            flowLegend.Size = new Size(157, 40);
            flowLegend.TabIndex = 0;
            flowLegend.WrapContents = false;
            // 
            // lblLegendCurrent
            // 
            lblLegendCurrent.AutoSize = true;
            lblLegendCurrent.ForeColor = Color.FromArgb(46, 197, 255);
            lblLegendCurrent.Location = new Point(15, 10);
            lblLegendCurrent.Name = "lblLegendCurrent";
            lblLegendCurrent.Padding = new Padding(0, 0, 16, 0);
            lblLegendCurrent.Size = new Size(78, 15);
            lblLegendCurrent.TabIndex = 0;
            lblLegendCurrent.Text = "▬ Current";
            // 
            // lblLegendTarget
            // 
            lblLegendTarget.AutoSize = true;
            lblLegendTarget.ForeColor = Color.FromArgb(255, 214, 69);
            lblLegendTarget.Location = new Point(99, 10);
            lblLegendTarget.Name = "lblLegendTarget";
            lblLegendTarget.Size = new Size(55, 15);
            lblLegendTarget.TabIndex = 1;
            lblLegendTarget.Text = "▬ Target";
            // 
            // panelCenter
            // 
            panelCenter.BackColor = Color.FromArgb(21, 23, 28);
            panelCenter.Controls.Add(chartPressure);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(200, 90);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(554, 373);
            panelCenter.TabIndex = 6;
            // 
            // lblComPort
            // 
            lblComPort.Location = new Point(0, 0);
            lblComPort.Name = "lblComPort";
            lblComPort.Size = new Size(100, 23);
            lblComPort.TabIndex = 0;
            // 
            // lblSessionTime
            // 
            lblSessionTime.Location = new Point(0, 0);
            lblSessionTime.Name = "lblSessionTime";
            lblSessionTime.Size = new Size(100, 23);
            lblSessionTime.TabIndex = 0;
            // 
            // lblHotkeys
            // 
            lblHotkeys.Location = new Point(0, 0);
            lblHotkeys.Name = "lblHotkeys";
            lblHotkeys.Size = new Size(100, 23);
            lblHotkeys.TabIndex = 0;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(0, 0);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(75, 23);
            btnPause.TabIndex = 0;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(0, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(75, 23);
            btnExport.TabIndex = 0;
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
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 19, 23);
            ClientSize = new Size(954, 533);
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
            tlpLiveStatus.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tlpSessionStats.ResumeLayout(false);
            tlpSessionStats.PerformLayout();
            panelRight.ResumeLayout(false);
            tableSettings.ResumeLayout(false);
            tlpAlerts.ResumeLayout(false);
            tlpAlerts.PerformLayout();
            tlpDisplay.ResumeLayout(false);
            tlpDisplay.PerformLayout();
            tlpThresholds.ResumeLayout(false);
            tlpThresholds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaximum).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            tlpGrid.ResumeLayout(false);
            tlpGrid.PerformLayout();
            tlpTimeWindow.ResumeLayout(false);
            tlpTimeWindow.PerformLayout();
            tlpTargetControl.ResumeLayout(false);
            tlpTargetControl.PerformLayout();
            panelBottom.ResumeLayout(false);
            footerLayout.ResumeLayout(false);
            footerLayout.PerformLayout();
            panelChartHeader.ResumeLayout(false);
            panelChartHeader.PerformLayout();
            panelChartButtons.ResumeLayout(false);
            panelChartButtons.PerformLayout();
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
        private Label lblXStep;
        private Label lblYStep;
        private ComboBox cmbXStep;
        private ComboBox cmbYStep;
        private Label lblThresholdsTitle;
        private Label lblAlertsTitle;
        private Label lblDisplayTitle;
        private Label lblMinimum;
        private Label lblMaximum;
        private Label lblFlash;
        private Label lblSound;
        private Label lblTimeWindowTitle;
        private ComboBox cmbDuration;
        private Label lblDuration;
        private NumericUpDown nudMaximum;
        private NumericUpDown numericUpDown2;
        private CheckBox chkShowGrid;
        private CheckBox chkSmoothing;
        private CheckBox chkSound;
        private CheckBox chkFlash;
        private TableLayoutPanel tlpLeft;
        private TableLayoutPanel tlpLiveStatus;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Button btnEmergency;
        private Label lblTarget;
        private Label lblDelta;
        private Label lblRate;
        private Label lblETA;
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
        // Footer elements
        private Label lblAutoSaveStatus;
        private Label lblFooterMin;
        private Label lblFooterMax;
        private Label lblFooterAvg;
        private Label lblFooterPoints;
        private Label lblThemeIndicator;
        // Target Control elements
        private TableLayoutPanel tlpTargetControl;
        private Label lblTargetControlTitle;
        private TextBox txtTargetValue;
        private Button btnGoTarget;
        private TableLayoutPanel footerLayout;
    }
}