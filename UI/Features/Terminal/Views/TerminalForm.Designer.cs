namespace Alicat.UI.Features.Terminal.Views
{
    partial class TerminalForm
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
            splitMain = new SplitContainer();
            grpQuick = new GroupBox();
            layouRight = new TableLayoutPanel();
            panelInput = new Panel();
            layoutInput = new TableLayoutPanel();
            btnSend = new Button();
            txtCommand = new TextBox();
            rtbLog = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            layouRight.SuspendLayout();
            panelInput.SuspendLayout();
            layoutInput.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(grpQuick);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(layouRight);
            splitMain.Size = new Size(656, 485);
            splitMain.SplitterDistance = 217;
            splitMain.TabIndex = 0;
            // 
            // grpQuick
            // 
            grpQuick.Dock = DockStyle.Fill;
            grpQuick.Location = new Point(0, 0);
            grpQuick.Name = "grpQuick";
            grpQuick.Size = new Size(217, 485);
            grpQuick.TabIndex = 1;
            grpQuick.TabStop = false;
            grpQuick.Text = "Quick Command Reference";
            // 
            // layouRight
            // 
            layouRight.ColumnCount = 1;
            layouRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layouRight.Controls.Add(panelInput, 0, 1);
            layouRight.Controls.Add(rtbLog, 0, 0);
            layouRight.Dock = DockStyle.Fill;
            layouRight.Location = new Point(0, 0);
            layouRight.Name = "layouRight";
            layouRight.RowCount = 3;
            layouRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layouRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layouRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layouRight.Size = new Size(435, 485);
            layouRight.TabIndex = 1;
            // 
            // panelInput
            // 
            panelInput.Controls.Add(layoutInput);
            panelInput.Dock = DockStyle.Fill;
            panelInput.Location = new Point(3, 413);
            panelInput.Name = "panelInput";
            panelInput.Size = new Size(429, 34);
            panelInput.TabIndex = 1;
            // 
            // layoutInput
            // 
            layoutInput.ColumnCount = 2;
            layoutInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            layoutInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            layoutInput.Controls.Add(btnSend, 1, 0);
            layoutInput.Controls.Add(txtCommand, 0, 0);
            layoutInput.Dock = DockStyle.Fill;
            layoutInput.Location = new Point(0, 0);
            layoutInput.Name = "layoutInput";
            layoutInput.RowCount = 1;
            layoutInput.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutInput.Size = new Size(429, 34);
            layoutInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Dock = DockStyle.Fill;
            btnSend.Location = new Point(346, 3);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(80, 28);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            // 
            // txtCommand
            // 
            txtCommand.Dock = DockStyle.Fill;
            txtCommand.Location = new Point(3, 3);
            txtCommand.Name = "txtCommand";
            txtCommand.Size = new Size(337, 23);
            txtCommand.TabIndex = 1;
            // 
            // rtbLog
            // 
            rtbLog.Dock = DockStyle.Fill;
            rtbLog.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbLog.Location = new Point(3, 3);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(429, 404);
            rtbLog.TabIndex = 1;
            rtbLog.Text = "";
            rtbLog.WordWrap = false;
            // 
            // TerminalForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(656, 485);
            Controls.Add(splitMain);
            Name = "TerminalForm";
            Text = "TerminalForm";
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            layouRight.ResumeLayout(false);
            panelInput.ResumeLayout(false);
            layoutInput.ResumeLayout(false);
            layoutInput.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitMain;
        private GroupBox groupBox1;
        private TableLayoutPanel layouRight;
        private GroupBox grpQuick;
        private RichTextBox rtbLog;
        private Panel panelInput;
        private TextBox txtCommand;
        private TableLayoutPanel layoutInput;
        private Button btnSend;
        private FlowLayoutPanel panelBottom;
        private CheckBox chkTimestamp;
        private CheckBox checkBox1;
        private CheckBox chkEcho;
    }
}