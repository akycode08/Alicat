namespace PrecisionPressureController.UI.Options
{
    partial class SessionConfigurationDialog
    {
        private System.ComponentModel.IContainer components = null;

        private TabControl tabControl;
        private TabPage tabPageSessionConfig;
        private RichTextBox txtJsonContent;
        private Panel panelButtons;
        private Button btnCopy;
        private Button btnSave;
        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSessionConfig = new System.Windows.Forms.TabPage();
            this.txtJsonContent = new System.Windows.Forms.RichTextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageSessionConfig.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSessionConfig);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(700, 510);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSessionConfig
            // 
            this.tabPageSessionConfig.Controls.Add(this.txtJsonContent);
            this.tabPageSessionConfig.Location = new System.Drawing.Point(4, 24);
            this.tabPageSessionConfig.Name = "tabPageSessionConfig";
            this.tabPageSessionConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSessionConfig.Size = new System.Drawing.Size(692, 482);
            this.tabPageSessionConfig.TabIndex = 0;
            this.tabPageSessionConfig.Text = "Session Configuration File";
            this.tabPageSessionConfig.UseVisualStyleBackColor = true;
            // 
            // txtJsonContent
            // 
            this.txtJsonContent.BackColor = System.Drawing.Color.White;
            this.txtJsonContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJsonContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJsonContent.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJsonContent.Location = new System.Drawing.Point(3, 3);
            this.txtJsonContent.Name = "txtJsonContent";
            this.txtJsonContent.ReadOnly = true;
            this.txtJsonContent.Size = new System.Drawing.Size(686, 476);
            this.txtJsonContent.TabIndex = 0;
            this.txtJsonContent.Text = "";
            this.txtJsonContent.WordWrap = false;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnCopy);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 510);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            this.panelButtons.Size = new System.Drawing.Size(700, 40);
            this.panelButtons.TabIndex = 1;
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(460, 10);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(541, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(622, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // SessionConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 550);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SessionConfigurationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Session Configuration";
            this.tabControl.ResumeLayout(false);
            this.tabPageSessionConfig.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}

