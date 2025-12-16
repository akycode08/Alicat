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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            chartPressure = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            panelBottom = new Panel();
            panelHeader = new Panel();
            panelChartHeader = new Panel();
            panelChartButtons = new FlowLayoutPanel();
            btnChartReset = new Button();
            btnFullscreen = new Button();
            flowLegend = new FlowLayoutPanel();
            lblLegendCurrent = new Label();
            lblLegendTarget = new Label();
            panelCenter = new Panel();
            ((System.ComponentModel.ISupportInitialize)chartPressure).BeginInit();
            panelLeft.SuspendLayout();
            tlpLeft.SuspendLayout();
            tlpLiveStatus.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panelRight.SuspendLayout();
            tableSettings.SuspendLayout();
            tlpAlerts.SuspendLayout();
            tlpDisplay.SuspendLayout();
            tlpThresholds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaximum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            tlpGrid.SuspendLayout();
            tlpTimeWindow.SuspendLayout();
            panelChartHeader.SuspendLayout();
            panelChartButtons.SuspendLayout();
            flowLegend.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // chartPressure
            // 
            chartPressure.BackColor = Color.FromArgb(21, 23, 28);
            chartPressure.BorderlineColor = Color.FromArgb(220, 224, 232);
            chartPressure.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.AxisX.LineColor = Color.FromArgb(60, 60, 70);
            chartArea1.AxisX.LineWidth = 2;
            chartArea1.AxisX.MajorGrid.LineColor = Color.FromArgb(35, 38, 45);
            chartArea1.AxisX.MajorTickMark.LineColor = Color.FromArgb(80, 80, 90);
            chartArea1.BackColor = Color.FromArgb(17, 19, 23);
            chartArea1.BorderColor = Color.Transparent;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            chartPressure.ChartAreas.Add(chartArea1);
            chartPressure.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chartPressure.Legends.Add(legend1);
            chartPressure.Location = new Point(0, 0);
            chartPressure.Margin = new Padding(0);
            chartPressure.Name = "chartPressure";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartPressure.Series.Add(series1);
            chartPressure.Size = new Size(554, 373);
            chartPressure.TabIndex = 0;
            chartPressure.Text = "chart1";
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
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLeft.Controls.Add(tlpLiveStatus, 0, 1);
            tlpLeft.Controls.Add(btnEmergency, 0, 3);
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.Location = new Point(0, 0);
            tlpLeft.Name = "tlpLeft";
            tlpLeft.RowCount = 4;
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tlpLeft.Size = new Size(200, 483);
            tlpLeft.TabIndex = 0;
            // 
            // tlpLiveStatus
            // 
            tlpLiveStatus.ColumnCount = 1;
            tlpLiveStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLiveStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLiveStatus.Controls.Add(tableLayoutPanel1, 0, 1);
            tlpLiveStatus.Controls.Add(tableLayoutPanel2, 0, 2);
            tlpLiveStatus.Controls.Add(tableLayoutPanel3, 0, 0);
            tlpLiveStatus.Dock = DockStyle.Fill;
            tlpLiveStatus.Location = new Point(3, 27);
            tlpLiveStatus.Name = "tlpLiveStatus";
            tlpLiveStatus.RowCount = 3;
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpLiveStatus.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpLiveStatus.Size = new Size(194, 187);
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
            tableLayoutPanel1.Location = new Point(3, 77);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.343338F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.343338F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.313324F));
            tableLayoutPanel1.Size = new Size(188, 68);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTarget
            // 
            lblTarget.AutoSize = true;
            lblTarget.Dock = DockStyle.Top;
            lblTarget.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTarget.ForeColor = Color.FromArgb(120, 125, 140);
            lblTarget.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblTarget.Location = new Point(5, 5);
            lblTarget.Margin = new Padding(5, 5, 0, 0);
            lblTarget.Name = "lblTarget";
            lblTarget.Size = new Size(89, 13);
            lblTarget.TabIndex = 13;
            lblTarget.Text = "Target";
            lblTarget.Click += label1_Click;
            // 
            // lblDelta
            // 
            lblDelta.AutoSize = true;
            lblDelta.Dock = DockStyle.Top;
            lblDelta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDelta.ForeColor = Color.FromArgb(120, 125, 140);
            lblDelta.Location = new Point(5, 27);
            lblDelta.Margin = new Padding(5, 5, 0, 0);
            lblDelta.Name = "lblDelta";
            lblDelta.Size = new Size(89, 15);
            lblDelta.TabIndex = 14;
            lblDelta.Text = "Delta";
            // 
            // lblRate
            // 
            lblRate.AutoSize = true;
            lblRate.Dock = DockStyle.Top;
            lblRate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRate.ForeColor = Color.FromArgb(120, 125, 140);
            lblRate.Location = new Point(5, 49);
            lblRate.Margin = new Padding(5, 5, 0, 0);
            lblRate.Name = "lblRate";
            lblRate.Size = new Size(89, 15);
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
            tableLayoutPanel2.Location = new Point(3, 151);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(188, 33);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lblETA
            // 
            lblETA.AutoSize = true;
            lblETA.BackColor = Color.FromArgb(21, 23, 28);
            lblETA.Dock = DockStyle.Top;
            lblETA.ForeColor = Color.FromArgb(120, 125, 140);
            lblETA.Location = new Point(0, 0);
            lblETA.Margin = new Padding(0);
            lblETA.Name = "lblETA";
            lblETA.Size = new Size(94, 15);
            lblETA.TabIndex = 0;
            lblETA.Text = "ETA";
            // 
            // lblTrend
            // 
            lblTrend.AutoSize = true;
            lblTrend.ForeColor = Color.FromArgb(120, 125, 140);
            lblTrend.Location = new Point(97, 0);
            lblTrend.Name = "lblTrend";
            lblTrend.Size = new Size(37, 15);
            lblTrend.TabIndex = 1;
            lblTrend.Text = "Trend";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(188, 68);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // btnEmergency
            // 
            btnEmergency.BackColor = Color.FromArgb(191, 0, 0);
            btnEmergency.Dock = DockStyle.Fill;
            btnEmergency.ForeColor = Color.FromArgb(210, 215, 225);
            btnEmergency.Location = new Point(3, 413);
            btnEmergency.Name = "btnEmergency";
            btnEmergency.Size = new Size(194, 67);
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
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(21, 23, 28);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(200, 463);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(554, 70);
            panelBottom.TabIndex = 3;
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
            ((System.ComponentModel.ISupportInitialize)chartPressure).EndInit();
            panelLeft.ResumeLayout(false);
            tlpLeft.ResumeLayout(false);
            tlpLiveStatus.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPressure;
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
    }
}