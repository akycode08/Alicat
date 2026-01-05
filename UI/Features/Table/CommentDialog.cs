using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat.UI.Features.Table.Views
{
    public partial class CommentDialog : Form
    {
        // Data
        public string Comment { get; private set; } = "";
        private string rowInfo;
        private bool isEditMode;
        
        // Theme - используем ColorScheme из того же namespace
        private ColorScheme currentTheme;
        
        // Dragging
        private bool isDragging = false;
        private Point dragOffset;
        
        public CommentDialog(string rowInfo, string existingComment = "", ColorScheme theme = null)
        {
            this.rowInfo = rowInfo;
            this.isEditMode = !string.IsNullOrEmpty(existingComment);
            this.currentTheme = theme ?? GetDarkTheme();
            
            InitializeComponent(); // Сначала инициализируем контролы из Designer
            SetupControls(); // Затем настраиваем их
            
            if (isEditMode)
            {
                lblTitle.Text = "✏️ Edit Comment";
                txtComment.Text = existingComment;
                UpdateCharCounter();
            }
            
            ApplyTheme();
        }
        
        private void SetupControls()
        {
            // Устанавливаем текст для lblRowInfo
            lblRowInfo.Text = $"Row: {rowInfo}";
            
            // Позиция lblCharCounter устанавливается в Designer, не перезаписываем здесь
            
            // Title bar dragging
            titleBar.MouseDown += TitleBar_MouseDown;
            titleBar.MouseMove += TitleBar_MouseMove;
            titleBar.MouseUp += TitleBar_MouseUp;
            lblTitle.MouseDown += TitleBar_MouseDown;
            lblTitle.MouseMove += TitleBar_MouseMove;
            lblTitle.MouseUp += TitleBar_MouseUp;
            
            // Устанавливаем порядок отрисовки
            titleBar.BringToFront();
            
            // ========== EVENT HANDLERS ==========
            btnClose.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            btnCancel.Click += BtnCancel_Click;
            btnSave.Click += BtnSave_Click;
            txtComment.TextChanged += TxtComment_TextChanged;
            
            // Keyboard shortcuts
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnCancel.PerformClick();
                }
                else if (e.Control && e.KeyCode == Keys.Enter)
                {
                    btnSave.PerformClick();
                }
            };
            
            // Button hover effects
            SetupButtonHover(btnClose);
            SetupButtonHover(btnCancel);
            SetupButtonHover(btnSave);
            
            // Focus on textbox
            this.Shown += (s, e) => { txtComment.Focus(); txtComment.SelectAll(); };
            
            // Fade in animation
            this.Opacity = 0;
            System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer { Interval = 10 };
            fadeTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.1;
                else
                    fadeTimer.Stop();
            };
            fadeTimer.Start();
        }
        
        private void TxtComment_TextChanged(object sender, EventArgs e)
        {
            UpdateCharCounter();
        }
        
        private void UpdateCharCounter()
        {
            int count = txtComment.Text.Length;
            lblCharCounter.Text = $"{count} / 500";
            
            if (count > 450)
                lblCharCounter.ForeColor = Color.FromArgb(239, 68, 68); // Red
            else
                lblCharCounter.ForeColor = Color.FromArgb(107, 114, 128); // Muted
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Comment = txtComment.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtComment.Text.Trim()) && !isEditMode)
            {
                var result = MessageBox.Show(
                    "Discard changes?", 
                    "Confirm", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question
                );
                
                if (result == DialogResult.No)
                    return;
            }
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        // ========== DRAGGING ==========
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragOffset = e.Location;
                if (sender != titleBar)
                {
                    dragOffset.Offset(((Control)sender).Location);
                }
            }
        }
        
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - dragOffset.X;
                newLocation.Y += e.Y - dragOffset.Y;
                if (sender != titleBar)
                {
                    Control ctrl = (Control)sender;
                    newLocation.X += ctrl.Location.X;
                    newLocation.Y += ctrl.Location.Y;
                }
                this.Location = newLocation;
            }
        }
        
        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
        
        // ========== THEME ==========
        public void ApplyTheme()
        {
            // Form
            this.BackColor = currentTheme.BorderControl;
            
            // Title bar
            titleBar.BackColor = currentTheme.BgControl;
            lblTitle.ForeColor = currentTheme.TextPrimary;
            btnClose.BackColor = currentTheme.BgControl;
            btnClose.ForeColor = currentTheme.TextSecondary;
            btnClose.FlatAppearance.MouseOverBackColor = currentTheme.AccentRed;
            
            // Content
            contentPanel.BackColor = currentTheme.BgPanel;
            lblComment.ForeColor = currentTheme.TextSecondary;
            txtComment.BackColor = GetInputBackground();
            txtComment.ForeColor = currentTheme.TextPrimary;
            lblRowInfo.BackColor = GetInputBackground();
            lblRowInfo.ForeColor = currentTheme.TextMuted;
            
            // Keyboard hint in button panel
            lblKeyboardHint.ForeColor = currentTheme.TextMuted;
            
            // Button panel
            buttonPanel.BackColor = currentTheme.BgControl;
            
            // Buttons
            btnCancel.BackColor = currentTheme.BgButton;
            btnCancel.ForeColor = currentTheme.TextSecondary;
            btnCancel.FlatAppearance.MouseOverBackColor = currentTheme.BgButtonHover;
            
            btnSave.BackColor = currentTheme.AccentGreen;
            btnSave.ForeColor = Color.White;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 150, 105);
        }
        
        private Color GetInputBackground()
        {
            // Используем BgInput если есть, иначе BgControl
            return Color.FromArgb(26, 31, 46); // Темный фон для input
        }
        
        private void SetupButtonHover(Button btn)
        {
            Color originalColor = btn.BackColor;
            
            btn.MouseEnter += (s, e) =>
            {
                if (btn == btnSave)
                    btn.BackColor = Color.FromArgb(5, 150, 105);
                else if (btn == btnClose)
                    btn.ForeColor = Color.White;
            };
            
            btn.MouseLeave += (s, e) =>
            {
                if (btn == btnSave)
                    btn.BackColor = currentTheme.AccentGreen;
                else if (btn == btnClose)
                    btn.ForeColor = currentTheme.TextSecondary;
            };
        }
        
        // ========== PAINT ==========
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Draw border
            using (Pen pen = new Pen(currentTheme.BorderControl, 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
        
        // ========== DEFAULT THEMES ==========
        private ColorScheme GetDarkTheme()
        {
            return new ColorScheme
            {
                BgWindow = ColorTranslator.FromHtml("#0a0e1a"),
                BgPanel = ColorTranslator.FromHtml("#111827"),
                BgControl = ColorTranslator.FromHtml("#0f1320"),
                BgButton = ColorTranslator.FromHtml("#1f2937"),
                BgButtonHover = ColorTranslator.FromHtml("#2d3748"),
                BgRowOdd = ColorTranslator.FromHtml("#161d2d"),
                BgRowHover = ColorTranslator.FromHtml("#1f2937"),
                BgRowSelected = Color.FromArgb(38, 16, 185, 129),
                
                TextPrimary = ColorTranslator.FromHtml("#e4e7eb"),
                TextSecondary = ColorTranslator.FromHtml("#9ca3af"),
                TextMuted = ColorTranslator.FromHtml("#6b7280"),
                
                AccentGreen = ColorTranslator.FromHtml("#10b981"),
                AccentCyan = ColorTranslator.FromHtml("#06b6d4"),
                AccentGold = ColorTranslator.FromHtml("#f59e0b"),
                AccentRed = ColorTranslator.FromHtml("#ef4444"),
                AccentBlue = ColorTranslator.FromHtml("#3b82f6"),
                
                BorderControl = ColorTranslator.FromHtml("#374151"),
                BorderDark = ColorTranslator.FromHtml("#1a1f2e")
            };
        }
    }
}

