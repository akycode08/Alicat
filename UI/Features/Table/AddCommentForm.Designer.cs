namespace Alicat.UI.Features.Table
{
    partial class addComment
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
            lblinfo = new Label();
            txtComment = new TextBox();
            btnCancel = new Button();
            btnSave = new Button();
            panelMain = new Panel();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // lblinfo
            // 
            lblinfo.AutoSize = true;
            lblinfo.Location = new Point(89, 60);
            lblinfo.Name = "lblinfo";
            lblinfo.Size = new Size(184, 15);
            lblinfo.TabIndex = 0;
            lblinfo.Text = "Time -- Pressure -> Setpoint PSIG";
            lblinfo.Click += label1_Click;
            // 
            // txtComment
            // 
            txtComment.Location = new Point(89, 78);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(184, 23);
            txtComment.TabIndex = 1;
            txtComment.TextChanged += txtComment_TextChanged;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(89, 107);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(198, 107);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // panelMain
            // 
            panelMain.Anchor = AnchorStyles.None;
            panelMain.Controls.Add(lblinfo);
            panelMain.Controls.Add(btnCancel);
            panelMain.Controls.Add(btnSave);
            panelMain.Controls.Add(txtComment);
            panelMain.Location = new Point(12, 12);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(400, 150);
            panelMain.TabIndex = 4;
            // 
            // addComment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 181);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "addComment";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add comment";
            Load += lblinfo_Load;
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblinfo;
        private TextBox txtComment;
        private Button btnCancel;
        private Button btnSave;
        private Panel panelMain;
    }
}