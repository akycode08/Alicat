using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private RadioButton? rbPng;
        private RadioButton? rbSvg;
        private RadioButton? rbScale1x;
        private RadioButton? rbScale2x;
        private RadioButton? rbScale4x;
        private Panel? panelSizeSection;
        private Button? btnCancel;
        private Button? btnExport;
        
        private bool _isDarkTheme = true;
        private int _chartWidth = 800;
        private int _chartHeight = 400;

        public ExportSettings Settings { get; private set; }

        public ExportChartDialog(bool isDarkTheme = true, int chartWidth = 800, int chartHeight = 400)
        {
            _isDarkTheme = isDarkTheme;
            _chartWidth = chartWidth;
            _chartHeight = chartHeight;
            Settings = new ExportSettings();
            InitializeComponent();
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Export Chart";
            this.ClientSize = new Size(320, 340);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            // Header panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(18, 12, 18, 12)
            };

            var lblTitle = new Label
            {
                Text = "Export Chart",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(lblTitle);

            // Main content panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 16, 18, 16),
                AutoScroll = true
            };

            // FORMAT section
            var lblFormat = new Label
            {
                Text = "FORMAT",
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(0, 0),
                Margin = new Padding(0, 0, 0, 8)
            };
            contentPanel.Controls.Add(lblFormat);

            // PNG option
            var panelPng = CreateOptionPanel("PNG", "Растровый", 0, 28);
            rbPng = new RadioButton
            {
                AutoSize = true,
                Location = new Point(14, 10),
                Checked = true // Default
            };
            panelPng.Controls.Add(rbPng);
            contentPanel.Controls.Add(panelPng);

            // SVG option
            var panelSvg = CreateOptionPanel("SVG", "Векторный", 0, 74);
            rbSvg = new RadioButton
            {
                AutoSize = true,
                Location = new Point(14, 10)
            };
            panelSvg.Controls.Add(rbSvg);
            contentPanel.Controls.Add(panelSvg);

            // SIZE section
            panelSizeSection = new Panel
            {
                Location = new Point(0, 120),
                Size = new Size(284, 120),
                AutoSize = false
            };

            var lblSize = new Label
            {
                Text = "SIZE",
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(0, 0),
                Margin = new Padding(0, 0, 0, 8)
            };
            panelSizeSection.Controls.Add(lblSize);

            // 1x option
            string size1x = $"{_chartWidth} × {_chartHeight}";
            var panel1x = CreateOptionPanel("1x", size1x, 0, 28);
            rbScale1x = new RadioButton
            {
                AutoSize = true,
                Location = new Point(14, 10)
            };
            panel1x.Controls.Add(rbScale1x);
            panelSizeSection.Controls.Add(panel1x);

            // 2x option (default)
            string size2x = $"{_chartWidth * 2} × {_chartHeight * 2}";
            var panel2x = CreateOptionPanel("2x", size2x, 0, 74);
            rbScale2x = new RadioButton
            {
                AutoSize = true,
                Location = new Point(14, 10),
                Checked = true // Default
            };
            panel2x.Controls.Add(rbScale2x);
            panelSizeSection.Controls.Add(panel2x);

            // 4x option
            string size4x = $"{_chartWidth * 4} × {_chartHeight * 4}";
            var panel4x = CreateOptionPanel("4x", size4x, 0, 120);
            rbScale4x = new RadioButton
            {
                AutoSize = true,
                Location = new Point(14, 10)
            };
            panel4x.Controls.Add(rbScale4x);
            panelSizeSection.Controls.Add(panel4x);

            contentPanel.Controls.Add(panelSizeSection);

            // Footer panel
            var footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(18, 10, 18, 10)
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(90, 32),
                Location = new Point(0, 9),
                DialogResult = DialogResult.Cancel,
                FlatStyle = FlatStyle.Flat
            };
            // BorderRadius is not available in WinForms FlatButtonAppearance
            // btnCancel.FlatAppearance.BorderRadius = 5;

            btnExport = new Button
            {
                Text = "📥 Export",
                Size = new Size(100, 32),
                Location = new Point(194, 9),
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat
            };
            // BorderRadius is not available in WinForms FlatButtonAppearance
            // btnExport.FlatAppearance.BorderRadius = 5;
            btnExport.Click += BtnExport_Click;

            footerPanel.Controls.Add(btnCancel);
            footerPanel.Controls.Add(btnExport);

            // Add panels to form
            this.Controls.Add(headerPanel);
            this.Controls.Add(contentPanel);
            this.Controls.Add(footerPanel);

            // Event handlers
            rbPng.CheckedChanged += (s, e) => 
            {
                UpdateSizeSectionVisibility();
                InvalidateOptionPanels();
            };
            rbSvg.CheckedChanged += (s, e) => 
            {
                UpdateSizeSectionVisibility();
                InvalidateOptionPanels();
            };
            
            rbScale1x.CheckedChanged += (s, e) => InvalidateOptionPanels();
            rbScale2x.CheckedChanged += (s, e) => InvalidateOptionPanels();
            rbScale4x.CheckedChanged += (s, e) => InvalidateOptionPanels();

            this.ResumeLayout(false);
        }

        private Panel CreateOptionPanel(string label, string description, int x, int y)
        {
            var panel = new Panel
            {
                Size = new Size(284, 40),
                Location = new Point(x, y),
                Padding = new Padding(10, 10, 14, 10),
                BorderStyle = BorderStyle.None
            };

            panel.Paint += (s, e) =>
            {
                var borderColor = _isDarkTheme ? Color.FromArgb(55, 65, 81) : Color.FromArgb(209, 213, 219);
                var selectedColor = Color.FromArgb(16, 185, 129); // Green #10b981
                
                // Check if this panel contains a checked radio button
                bool isSelected = false;
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is RadioButton rb && rb.Checked)
                    {
                        isSelected = true;
                        break;
                    }
                }

                using (var pen = new Pen(isSelected ? selectedColor : borderColor, 1))
                {
                    var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                    DrawRoundedRectangle(e.Graphics, pen, rect, 6);
                }
            };

            var lblOption = new Label
            {
                Text = label,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(36, 10)
            };

            var lblDesc = new Label
            {
                Text = description,
                AutoSize = true,
                Font = new Font("Consolas", 9F),
                Location = new Point(150, 12),
                TextAlign = ContentAlignment.MiddleRight
            };

            panel.Controls.Add(lblOption);
            panel.Controls.Add(lblDesc);

            // Make panel clickable to select radio button
            panel.Click += (s, e) =>
            {
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is RadioButton rb)
                    {
                        rb.Checked = true;
                        // Invalidate all option panels to update borders
                        if (panel.Parent != null)
                        {
                            foreach (Control parentCtrl in panel.Parent.Controls)
                            {
                                if (parentCtrl is Panel p && p != panel)
                                    p.Invalidate();
                            }
                        }
                        panel.Invalidate();
                        break;
                    }
                }
            };
            
            // Also make labels clickable - trigger panel click manually
            lblOption.Click += (s, e) =>
            {
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is RadioButton rb)
                    {
                        rb.Checked = true;
                        if (panel.Parent != null)
                        {
                            foreach (Control parentCtrl in panel.Parent.Controls)
                            {
                                if (parentCtrl is Panel p && p != panel)
                                    p.Invalidate();
                            }
                        }
                        panel.Invalidate();
                        break;
                    }
                }
            };
            lblDesc.Click += (s, e) =>
            {
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is RadioButton rb)
                    {
                        rb.Checked = true;
                        if (panel.Parent != null)
                        {
                            foreach (Control parentCtrl in panel.Parent.Controls)
                            {
                                if (parentCtrl is Panel p && p != panel)
                                    p.Invalidate();
                            }
                        }
                        panel.Invalidate();
                        break;
                    }
                }
            };

            return panel;
        }

        private void UpdateSizeSectionVisibility()
        {
            if (panelSizeSection == null) return;
            
            bool isSvg = rbSvg?.Checked ?? false;
            panelSizeSection.Visible = !isSvg;
            panelSizeSection.Enabled = !isSvg;
        }
        
        private void InvalidateOptionPanels()
        {
            // Обновляем все панели опций для перерисовки границ
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel contentPanel)
                {
                    foreach (Control child in contentPanel.Controls)
                    {
                        if (child is Panel optionPanel)
                        {
                            optionPanel.Invalidate();
                        }
                        else if (child is Panel sizeSection)
                        {
                            foreach (Control sizeChild in sizeSection.Controls)
                            {
                                if (sizeChild is Panel sizeOptionPanel)
                                {
                                    sizeOptionPanel.Invalidate();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            // Update settings
            Settings.Format = rbSvg?.Checked == true ? "SVG" : "PNG";
            
            if (Settings.Format == "PNG")
            {
                if (rbScale1x?.Checked == true)
                    Settings.Scale = 1;
                else if (rbScale2x?.Checked == true)
                    Settings.Scale = 2;
                else if (rbScale4x?.Checked == true)
                    Settings.Scale = 4;
            }
            else
            {
                Settings.Scale = 1; // SVG всегда 1x
            }
        }

        private void ApplyTheme()
        {
            if (_isDarkTheme)
            {
                this.BackColor = Color.FromArgb(17, 24, 39); // #111827
                
                // Header
                if (this.Controls.Count > 0 && this.Controls[0] is Panel header)
                {
                    header.BackColor = Color.FromArgb(15, 20, 25); // #0f1419
                    foreach (Control ctrl in header.Controls)
                    {
                        if (ctrl is Label lbl)
                            lbl.ForeColor = Color.FromArgb(228, 231, 235); // #e4e7eb
                    }
                }

                // Footer
                if (this.Controls.Count > 2 && this.Controls[2] is Panel footer)
                {
                    footer.BackColor = Color.FromArgb(15, 20, 25); // #0f1419
                }

                // Buttons
                if (btnCancel != null)
                {
                    btnCancel.BackColor = Color.FromArgb(31, 41, 55); // #1f2937
                    btnCancel.ForeColor = Color.FromArgb(228, 231, 235);
                    btnCancel.FlatAppearance.BorderColor = Color.FromArgb(55, 65, 81);
                }

                if (btnExport != null)
                {
                    btnExport.BackColor = Color.FromArgb(16, 185, 129); // #10b981
                    btnExport.ForeColor = Color.White;
                    btnExport.FlatAppearance.BorderColor = Color.FromArgb(16, 185, 129);
                }

                // Labels
                foreach (Control ctrl in this.Controls)
                {
                    ApplyDarkThemeToControl(ctrl);
                }
            }
            else
            {
                this.BackColor = Color.White;
                
                // Header
                if (this.Controls.Count > 0 && this.Controls[0] is Panel header)
                {
                    header.BackColor = Color.FromArgb(243, 244, 246); // #f3f4f6
                    foreach (Control ctrl in header.Controls)
                    {
                        if (ctrl is Label lbl)
                            lbl.ForeColor = Color.FromArgb(17, 24, 39); // #111827
                    }
                }

                // Footer
                if (this.Controls.Count > 2 && this.Controls[2] is Panel footer)
                {
                    footer.BackColor = Color.FromArgb(243, 244, 246); // #f3f4f6
                }

                // Buttons
                if (btnCancel != null)
                {
                    btnCancel.BackColor = Color.FromArgb(229, 231, 235); // #e5e7eb
                    btnCancel.ForeColor = Color.FromArgb(17, 24, 39);
                    btnCancel.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
                }

                if (btnExport != null)
                {
                    btnExport.BackColor = Color.FromArgb(16, 185, 129); // #10b981
                    btnExport.ForeColor = Color.White;
                    btnExport.FlatAppearance.BorderColor = Color.FromArgb(16, 185, 129);
                }

                // Labels
                foreach (Control ctrl in this.Controls)
                {
                    ApplyLightThemeToControl(ctrl);
                }
            }
        }

        private void ApplyDarkThemeToControl(Control ctrl)
        {
            if (ctrl is Label lbl)
            {
                if (lbl.Font.Bold && lbl.Text == lbl.Text.ToUpper())
                    lbl.ForeColor = Color.FromArgb(107, 114, 128); // #6b7280 (secondary)
                else if (lbl.Font.Name == "Consolas")
                    lbl.ForeColor = Color.FromArgb(107, 114, 128); // #6b7280 (secondary for descriptions)
                else
                    lbl.ForeColor = Color.FromArgb(228, 231, 235); // #e4e7eb (primary)
            }
            else if (ctrl is RadioButton rb)
            {
                rb.ForeColor = Color.FromArgb(228, 231, 235);
            }
            else if (ctrl is Panel panel)
            {
                panel.BackColor = Color.FromArgb(13, 17, 23); // #0d1117
                foreach (Control child in panel.Controls)
                {
                    ApplyDarkThemeToControl(child);
                }
            }
        }

        private void ApplyLightThemeToControl(Control ctrl)
        {
            if (ctrl is Label lbl)
            {
                if (lbl.Font.Bold && lbl.Text == lbl.Text.ToUpper())
                    lbl.ForeColor = Color.FromArgb(107, 114, 128); // #6b7280 (secondary)
                else if (lbl.Font.Name == "Consolas")
                    lbl.ForeColor = Color.FromArgb(107, 114, 128); // #6b7280 (secondary for descriptions)
                else
                    lbl.ForeColor = Color.FromArgb(17, 24, 39); // #111827 (primary)
            }
            else if (ctrl is RadioButton rb)
            {
                rb.ForeColor = Color.FromArgb(17, 24, 39);
            }
            else if (ctrl is Panel panel)
            {
                panel.BackColor = Color.White;
                foreach (Control child in panel.Controls)
                {
                    ApplyLightThemeToControl(child);
                }
            }
        }

        private void DrawRoundedRectangle(Graphics graphics, Pen pen, Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();
            graphics.DrawPath(pen, path);
        }
    }
}

