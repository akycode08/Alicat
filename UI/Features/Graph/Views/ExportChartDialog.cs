using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat.UI.Features.Graph.Views
{
    /// <summary>
    /// Настройки экспорта графика
    /// </summary>
    public class ExportSettings
    {
        public string Format { get; set; } = "PNG"; // "PNG" или "SVG"
        public int Scale { get; set; } = 2; // 1, 2, или 4 (только для PNG)
    }

    /// <summary>
    /// Диалог экспорта графика в PNG или SVG
    /// </summary>
    public partial class ExportChartDialog : Form
    {
        private RadioButton rbPng;
        private RadioButton rbSvg;
        private RadioButton rbScale1x;
        private RadioButton rbScale2x;
        private RadioButton rbScale4x;
        private GroupBox grpFormat;
        private GroupBox grpSize;
        private Button btnCancel;
        private Button btnOK;
        
        private int _chartWidth = 800;
        private int _chartHeight = 400;

        public ExportSettings Settings { get; private set; }

        public ExportChartDialog(bool isDarkTheme = true, int chartWidth = 800, int chartHeight = 400)
        {
            _chartWidth = chartWidth;
            _chartHeight = chartHeight;
            Settings = new ExportSettings
            {
                Format = "PNG",
                Scale = 2
            };
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Export Chart";
            this.ClientSize = new Size(320, 280);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            // FORMAT GroupBox
            grpFormat = new GroupBox
            {
                Text = "Format",
                Location = new Point(12, 12),
                Size = new Size(296, 80),
                TabIndex = 0
            };

            rbPng = new RadioButton
            {
                Text = "PNG",
                Location = new Point(12, 20),
                Size = new Size(120, 20),
                Checked = true,
                TabIndex = 0
            };

            rbSvg = new RadioButton
            {
                Text = "SVG",
                Location = new Point(12, 45),
                Size = new Size(140, 20),
                TabIndex = 1
            };

            grpFormat.Controls.Add(rbPng);
            grpFormat.Controls.Add(rbSvg);

            // SIZE GroupBox
            grpSize = new GroupBox
            {
                Text = "Size",
                Location = new Point(12, 100),
                Size = new Size(296, 100),
                TabIndex = 1
            };

            rbScale1x = new RadioButton
            {
                Text = $"1x ({_chartWidth} × {_chartHeight})",
                Location = new Point(12, 20),
                Size = new Size(200, 20),
                TabIndex = 0
            };

            rbScale2x = new RadioButton
            {
                Text = $"2x ({_chartWidth * 2} × {_chartHeight * 2})",
                Location = new Point(12, 45),
                Size = new Size(200, 20),
                Checked = true,
                TabIndex = 1
            };

            rbScale4x = new RadioButton
            {
                Text = $"4x ({_chartWidth * 4} × {_chartHeight * 4})",
                Location = new Point(12, 70),
                Size = new Size(200, 20),
                TabIndex = 2
            };

            grpSize.Controls.Add(rbScale1x);
            grpSize.Controls.Add(rbScale2x);
            grpSize.Controls.Add(rbScale4x);

            // Buttons
            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(155, 210),
                Size = new Size(75, 23),
                TabIndex = 2
            };

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(236, 210),
                Size = new Size(75, 23),
                TabIndex = 3
            };
            btnOK.Click += BtnOK_Click;

            // Add controls to form
            this.Controls.Add(grpFormat);
            this.Controls.Add(grpSize);
            this.Controls.Add(btnCancel);
            this.Controls.Add(btnOK);

            // Event handlers
            rbPng.CheckedChanged += (s, e) => 
            {
                if (rbPng.Checked)
                    UpdateSizeSectionVisibility();
            };
            rbSvg.CheckedChanged += (s, e) => 
            {
                if (rbSvg.Checked)
                    UpdateSizeSectionVisibility();
            };

            // Initialize visibility
            UpdateSizeSectionVisibility();

            this.ResumeLayout(false);
        }

        private void UpdateSizeSectionVisibility()
        {
            bool isSvg = rbSvg.Checked;
            grpSize.Visible = !isSvg;
            grpSize.Enabled = !isSvg;
            
            // If SVG is selected, disable size radio buttons
            if (isSvg)
            {
                rbScale1x.Enabled = false;
                rbScale2x.Enabled = false;
                rbScale4x.Enabled = false;
            }
            else
            {
                rbScale1x.Enabled = true;
                rbScale2x.Enabled = true;
                rbScale4x.Enabled = true;
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            // Update settings based on user selection
            Settings.Format = rbSvg.Checked ? "SVG" : "PNG";
            
            if (Settings.Format == "PNG")
            {
                // PNG supports scaling
                if (rbScale1x.Checked)
                    Settings.Scale = 1;
                else if (rbScale2x.Checked)
                    Settings.Scale = 2;
                else if (rbScale4x.Checked)
                    Settings.Scale = 4;
            }
            else
            {
                // SVG is vector format, always 1x (no scaling)
                Settings.Scale = 1;
            }
        }
    }
}
