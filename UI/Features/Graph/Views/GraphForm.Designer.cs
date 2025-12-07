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
            legend1.Name = "Legend1";
            chartPressure.Legends.Add(legend1);
            chartPressure.Location = new Point(15, 22);
            chartPressure.Margin = new Padding(0);
            chartPressure.Name = "chartPressure";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartPressure.Series.Add(series1);
            chartPressure.Size = new Size(562, 365);
            chartPressure.TabIndex = 0;
            chartPressure.Text = "chart1";
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(21, 23, 28);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 50);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(200, 500);
            panelLeft.TabIndex = 1;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(21, 23, 28);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(818, 50);
            panelRight.Margin = new Padding(4);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(10, 4, 10, 4);
            panelRight.Size = new Size(200, 500);
            panelRight.TabIndex = 2;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(21, 23, 28);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(200, 480);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(618, 70);
            panelBottom.TabIndex = 3;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(21, 23, 28);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1018, 50);
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
            panelChartHeader.Size = new Size(618, 40);
            panelChartHeader.TabIndex = 5;
            // 
            // panelChartButtons
            // 
            panelChartButtons.Controls.Add(btnChartReset);
            panelChartButtons.Controls.Add(btnFullscreen);
            panelChartButtons.Dock = DockStyle.Right;
            panelChartButtons.Location = new Point(418, 0);
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
            panelCenter.Size = new Size(618, 390);
            panelCenter.TabIndex = 6;
            // 
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 19, 23);
            ClientSize = new Size(1018, 550);
            Controls.Add(panelCenter);
            Controls.Add(panelChartHeader);
            Controls.Add(panelBottom);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Controls.Add(panelHeader);
            Name = "GraphForm";
            Text = "Pressure Graph";
            ((System.ComponentModel.ISupportInitialize)chartPressure).EndInit();
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
    }
}