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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            chartPressure = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)chartPressure).BeginInit();
            SuspendLayout();
            // 
            // chartPressure
            // 
            chartArea2.Name = "ChartArea1";
            chartPressure.ChartAreas.Add(chartArea2);
            chartPressure.Dock = DockStyle.Fill;
            legend2.Name = "Legend1";
            chartPressure.Legends.Add(legend2);
            chartPressure.Location = new Point(0, 0);
            chartPressure.Name = "chartPressure";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chartPressure.Series.Add(series2);
            chartPressure.Size = new Size(1128, 619);
            chartPressure.TabIndex = 0;
            chartPressure.Text = "chart1";
            //chartPressure.Click += chartPressure_Click;
            // 
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1128, 619);
            Controls.Add(chartPressure);
            Name = "GraphForm";
            Text = "GraphForm";
            //Load += GraphForm_Load;
            ((System.ComponentModel.ISupportInitialize)chartPressure).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPressure;
    }
}