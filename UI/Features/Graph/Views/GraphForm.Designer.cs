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
            panelRight = new Panel();
            tableSettings = new TableLayoutPanel();
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
            tlpTimeWindow = new TableLayoutPanel();
            tlpGrid = new TableLayoutPanel();
            tlpThresholds = new TableLayoutPanel();
            tlpDisplay = new TableLayoutPanel();
            tlpAlerts = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            lblGridTitle = new Label();
            lblXStep = new Label();
            lblYStep = new Label();
            cmbYStep = new ComboBox();
            cmbXStep = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)chartPressure).BeginInit();
            panelRight.SuspendLayout();
            tableSettings.SuspendLayout();
            panelChartHeader.SuspendLayout();
            panelChartButtons.SuspendLayout();
            flowLegend.SuspendLayout();
            panelCenter.SuspendLayout();
            tlpTimeWindow.SuspendLayout();
            tlpGrid.SuspendLayout();
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
            chartPressure.Size = new Size(567, 440);
            chartPressure.TabIndex = 0;
            chartPressure.Text = "chart1";
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(21, 23, 28);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 50);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(200, 550);
            panelLeft.TabIndex = 1;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(21, 23, 28);
            panelRight.Controls.Add(tableSettings);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(767, 50);
            panelRight.Margin = new Padding(4);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(10, 4, 10, 4);
            panelRight.Size = new Size(200, 550);
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
            tableSettings.Size = new Size(180, 542);
            tableSettings.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(21, 23, 28);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(200, 530);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(567, 70);
            panelBottom.TabIndex = 3;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(21, 23, 28);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(967, 50);
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
            panelChartHeader.Size = new Size(567, 40);
            panelChartHeader.TabIndex = 5;
            // 
            // panelChartButtons
            // 
            panelChartButtons.Controls.Add(btnChartReset);
            panelChartButtons.Controls.Add(btnFullscreen);
            panelChartButtons.Dock = DockStyle.Right;
            panelChartButtons.Location = new Point(367, 0);
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
            panelCenter.Size = new Size(567, 440);
            panelCenter.TabIndex = 6;
            // 
            // tlpTimeWindow
            // 
            tlpTimeWindow.BackColor = Color.FromArgb(32, 35, 44);
            tlpTimeWindow.ColumnCount = 2;
            tlpTimeWindow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpTimeWindow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpTimeWindow.Controls.Add(label1, 0, 0);
            tlpTimeWindow.Controls.Add(label2, 0, 1);
            tlpTimeWindow.Controls.Add(comboBox1, 1, 1);
            tlpTimeWindow.Dock = DockStyle.Fill;
            tlpTimeWindow.Location = new Point(3, 3);
            tlpTimeWindow.Name = "tlpTimeWindow";
            tlpTimeWindow.RowCount = 3;
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpTimeWindow.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpTimeWindow.Size = new Size(174, 84);
            tlpTimeWindow.TabIndex = 0;
            // 
            // tlpGrid
            // 
            tlpGrid.BackColor = Color.FromArgb(32, 35, 44);
            tlpGrid.ColumnCount = 2;
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpGrid.Controls.Add(cmbXStep, 1, 1);
            tlpGrid.Controls.Add(lblGridTitle, 0, 0);
            tlpGrid.Controls.Add(lblXStep, 0, 1);
            tlpGrid.Controls.Add(lblYStep, 0, 2);
            tlpGrid.Controls.Add(cmbYStep, 1, 2);
            tlpGrid.Dock = DockStyle.Fill;
            tlpGrid.ForeColor = Color.FromArgb(120, 125, 140);
            tlpGrid.Location = new Point(3, 93);
            tlpGrid.Name = "tlpGrid";
            tlpGrid.RowCount = 3;
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpGrid.Size = new Size(174, 84);
            tlpGrid.TabIndex = 1;
            // 
            // tlpThresholds
            // 
            tlpThresholds.BackColor = Color.FromArgb(32, 35, 44);
            tlpThresholds.ColumnCount = 2;
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpThresholds.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpThresholds.Dock = DockStyle.Fill;
            tlpThresholds.Location = new Point(3, 183);
            tlpThresholds.Name = "tlpThresholds";
            tlpThresholds.RowCount = 3;
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpThresholds.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpThresholds.Size = new Size(174, 84);
            tlpThresholds.TabIndex = 2;
            // 
            // tlpDisplay
            // 
            tlpDisplay.BackColor = Color.FromArgb(32, 35, 44);
            tlpDisplay.ColumnCount = 2;
            tlpDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpDisplay.Dock = DockStyle.Fill;
            tlpDisplay.Location = new Point(3, 273);
            tlpDisplay.Name = "tlpDisplay";
            tlpDisplay.RowCount = 3;
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpDisplay.Size = new Size(174, 84);
            tlpDisplay.TabIndex = 3;
            // 
            // tlpAlerts
            // 
            tlpAlerts.BackColor = Color.FromArgb(32, 35, 44);
            tlpAlerts.ColumnCount = 2;
            tlpAlerts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpAlerts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpAlerts.Dock = DockStyle.Fill;
            tlpAlerts.Location = new Point(3, 363);
            tlpAlerts.Name = "tlpAlerts";
            tlpAlerts.RowCount = 3;
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpAlerts.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tlpAlerts.Size = new Size(174, 84);
            tlpAlerts.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 25);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 1;
            label2.Text = "label2";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(107, 28);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(64, 23);
            comboBox1.TabIndex = 2;
            // 
            // lblGridTitle
            // 
            lblGridTitle.AutoSize = true;
            lblGridTitle.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGridTitle.LiveSetting = System.Windows.Forms.Automation.AutomationLiveSetting.Polite;
            lblGridTitle.Location = new Point(5, 5);
            lblGridTitle.Margin = new Padding(5, 5, 0, 0);
            lblGridTitle.Name = "lblGridTitle";
            lblGridTitle.Size = new Size(33, 13);
            lblGridTitle.TabIndex = 0;
            lblGridTitle.Text = "GRID";
            // 
            // lblXStep
            // 
            lblXStep.AutoSize = true;
            lblXStep.ForeColor = Color.FromArgb(210, 215, 225);
            lblXStep.Location = new Point(4, 29);
            lblXStep.Margin = new Padding(4, 4, 0, 0);
            lblXStep.Name = "lblXStep";
            lblXStep.Padding = new Padding(5, 5, 0, 0);
            lblXStep.Size = new Size(45, 20);
            lblXStep.TabIndex = 1;
            lblXStep.Text = "X Step";
            lblXStep.Click += lblXStep_Click;
            // 
            // lblYStep
            // 
            lblYStep.AutoSize = true;
            lblYStep.ForeColor = Color.FromArgb(210, 215, 225);
            lblYStep.Location = new Point(4, 58);
            lblYStep.Margin = new Padding(4, 4, 0, 0);
            lblYStep.Name = "lblYStep";
            lblYStep.Padding = new Padding(5, 5, 0, 0);
            lblYStep.Size = new Size(45, 20);
            lblYStep.TabIndex = 2;
            lblYStep.Text = "Y Step";
            // 
            // cmbYStep
            // 
            cmbYStep.BackColor = Color.FromArgb(40, 43, 52);
            cmbYStep.Dock = DockStyle.Fill;
            cmbYStep.ForeColor = Color.White;
            cmbYStep.FormattingEnabled = true;
            cmbYStep.Location = new Point(107, 57);
            cmbYStep.Name = "cmbYStep";
            cmbYStep.Size = new Size(64, 23);
            cmbYStep.TabIndex = 4;
            // 
            // cmbXStep
            // 
            cmbXStep.BackColor = Color.FromArgb(40, 43, 52);
            cmbXStep.Dock = DockStyle.Fill;
            cmbXStep.ForeColor = Color.White;
            cmbXStep.FormattingEnabled = true;
            cmbXStep.Location = new Point(107, 28);
            cmbXStep.Name = "cmbXStep";
            cmbXStep.Size = new Size(64, 23);
            cmbXStep.TabIndex = 6;
            // 
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 19, 23);
            ClientSize = new Size(967, 600);
            Controls.Add(panelCenter);
            Controls.Add(panelChartHeader);
            Controls.Add(panelBottom);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Controls.Add(panelHeader);
            Name = "GraphForm";
            Text = "Pressure Graph";
            ((System.ComponentModel.ISupportInitialize)chartPressure).EndInit();
            panelRight.ResumeLayout(false);
            tableSettings.ResumeLayout(false);
            panelChartHeader.ResumeLayout(false);
            panelChartHeader.PerformLayout();
            panelChartButtons.ResumeLayout(false);
            panelChartButtons.PerformLayout();
            flowLegend.ResumeLayout(false);
            flowLegend.PerformLayout();
            panelCenter.ResumeLayout(false);
            tlpTimeWindow.ResumeLayout(false);
            tlpTimeWindow.PerformLayout();
            tlpGrid.ResumeLayout(false);
            tlpGrid.PerformLayout();
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
        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private Label lblGridTitle;
        private Label lblXStep;
        private Label lblYStep;
        private ComboBox cmbXStep;
        private ComboBox cmbYStep;
    }
}