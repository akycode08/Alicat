using System;
using System.Windows.Forms;
using System.Drawing;


namespace Alicat
{
    partial class AlicatForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel root;
        private System.Windows.Forms.TableLayoutPanel header;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCommunication;

        private System.Windows.Forms.GroupBox groupTop;
        private System.Windows.Forms.TableLayoutPanel tableTop;
        private System.Windows.Forms.Button btnGoPlus;
        private System.Windows.Forms.Button btnGoMinus;
        private System.Windows.Forms.Panel panelCurrent;
        private System.Windows.Forms.Label lblCurrentBig;
        private System.Windows.Forms.TableLayoutPanel tableIncrement;
        private System.Windows.Forms.Label lblIncrement;
        private System.Windows.Forms.NumericUpDown nudIncrement;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            root = new TableLayoutPanel();
            header = new TableLayoutPanel();
            lblTitle = new Label();
            btnCommunication = new Button();
            groupTop = new GroupBox();
            tableTop = new TableLayoutPanel();
            tableIncrement = new TableLayoutPanel();
            lblIncrement = new Label();
            nudIncrement = new NumericUpDown();
            btnGoPlus = new Button();
            panelCurrent = new Panel();
            lblCurrentBig = new Label();
            btnGoMinus = new Button();
            root.SuspendLayout();
            header.SuspendLayout();
            groupTop.SuspendLayout();
            tableTop.SuspendLayout();
            tableIncrement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudIncrement).BeginInit();
            panelCurrent.SuspendLayout();
            SuspendLayout();
            // 
            // root
            // 
            root.ColumnCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            root.Controls.Add(header, 0, 0);
            root.Controls.Add(groupTop, 0, 1);
            root.Dock = DockStyle.Fill;
            root.Location = new Point(0, 0);
            root.Name = "root";
            root.Padding = new Padding(12);
            root.RowCount = 2;
            root.RowStyles.Add(new RowStyle());
            root.RowStyles.Add(new RowStyle());
            root.Size = new Size(1024, 420);
            root.TabIndex = 0;
            // 
            // header
            // 
            header.ColumnCount = 2;
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle());
            header.Controls.Add(lblTitle, 0, 0);
            header.Controls.Add(btnCommunication, 1, 0);
            header.Dock = DockStyle.Top;
            header.Location = new Point(12, 12);
            header.Margin = new Padding(0, 0, 0, 8);
            header.Name = "header";
            header.RowCount = 1;
            header.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            header.Size = new Size(1000, 100);
            header.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Left;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 42);
            lblTitle.Margin = new Padding(0, 8, 0, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(93, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Alicat Controller";
            // 
            // btnCommunication
            // 
            btnCommunication.Anchor = AnchorStyles.Right;
            btnCommunication.AutoSize = true;
            btnCommunication.Location = new Point(884, 37);
            btnCommunication.Name = "btnCommunication";
            btnCommunication.Size = new Size(113, 25);
            btnCommunication.TabIndex = 1;
            btnCommunication.Text = "Communication…";
            // 
            // groupTop
            // 
            groupTop.AutoSize = true;
            groupTop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupTop.Controls.Add(tableTop);
            groupTop.Dock = DockStyle.Top;
            groupTop.Location = new Point(12, 120);
            groupTop.Margin = new Padding(0, 0, 0, 8);
            groupTop.Name = "groupTop";
            groupTop.Padding = new Padding(12);
            groupTop.Size = new Size(1000, 172);
            groupTop.TabIndex = 1;
            groupTop.TabStop = false;
            // 
            // tableTop
            // 
            tableTop.AutoSize = true;
            tableTop.ColumnCount = 3;
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTop.Controls.Add(tableIncrement, 0, 1);
            tableTop.Controls.Add(btnGoPlus, 0, 0);
            tableTop.Controls.Add(panelCurrent, 1, 0);
            tableTop.Controls.Add(btnGoMinus, 2, 0);
            tableTop.Dock = DockStyle.Fill;
            tableTop.Location = new Point(12, 28);
            tableTop.Name = "tableTop";
            tableTop.RowCount = 2;
            tableTop.RowStyles.Add(new RowStyle());
            tableTop.RowStyles.Add(new RowStyle());
            tableTop.Size = new Size(976, 132);
            tableTop.TabIndex = 0;
            // 
            // tableIncrement
            // 
            tableIncrement.AutoSize = true;
            tableIncrement.ColumnCount = 3;
            tableTop.SetColumnSpan(tableIncrement, 3);
            tableIncrement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableIncrement.ColumnStyles.Add(new ColumnStyle());
            tableIncrement.ColumnStyles.Add(new ColumnStyle());
            tableIncrement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableIncrement.ColumnStyles.Add(new ColumnStyle());
            tableIncrement.ColumnStyles.Add(new ColumnStyle());
            tableIncrement.Controls.Add(lblIncrement, 1, 0);
            tableIncrement.Controls.Add(nudIncrement, 2, 0);
            tableIncrement.Dock = DockStyle.Top;
            tableIncrement.Location = new Point(0, 112);
            tableIncrement.Margin = new Padding(0, 8, 0, 0);
            tableIncrement.Name = "tableIncrement";
            tableIncrement.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableIncrement.Size = new Size(976, 20);
            tableIncrement.TabIndex = 0;
            // 
            // lblIncrement
            // 
            lblIncrement.Anchor = AnchorStyles.Left;
            lblIncrement.AutoSize = true;
            lblIncrement.Location = new Point(791, 6);
            lblIncrement.Margin = new Padding(0, 6, 8, 0);
            lblIncrement.Name = "lblIncrement";
            lblIncrement.Size = new Size(61, 14);
            lblIncrement.TabIndex = 1;
            lblIncrement.Text = "Increment";
            // 
            // nudIncrement
            // 
            nudIncrement.DecimalPlaces = 3;
            nudIncrement.Increment = new decimal(new int[] { 100, 0, 0, 196608 });
            nudIncrement.Location = new Point(863, 3);
            nudIncrement.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudIncrement.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
            nudIncrement.Name = "nudIncrement";
            nudIncrement.Size = new Size(110, 23);
            nudIncrement.TabIndex = 2;
            nudIncrement.Value = new decimal(new int[] { 1000, 0, 0, 196608 });
            // 
            // btnGoPlus
            // 
            btnGoPlus.Dock = DockStyle.Fill;
            btnGoPlus.Location = new Point(0, 0);
            btnGoPlus.Margin = new Padding(0, 0, 6, 0);
            btnGoPlus.Name = "btnGoPlus";
            btnGoPlus.Size = new Size(238, 104);
            btnGoPlus.TabIndex = 1;
            btnGoPlus.Text = "GO: Value + increment";
            // 
            // panelCurrent
            // 
            panelCurrent.BackColor = Color.White;
            panelCurrent.BorderStyle = BorderStyle.FixedSingle;
            panelCurrent.Controls.Add(lblCurrentBig);
            panelCurrent.Dock = DockStyle.Fill;
            panelCurrent.Location = new Point(250, 0);
            panelCurrent.Margin = new Padding(6, 0, 6, 4);
            panelCurrent.Name = "panelCurrent";
            panelCurrent.Size = new Size(476, 100);
            panelCurrent.TabIndex = 2;
            // 
            // lblCurrentBig
            // 
            lblCurrentBig.Dock = DockStyle.Fill;
            lblCurrentBig.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblCurrentBig.Location = new Point(0, 0);
            lblCurrentBig.Name = "lblCurrentBig";
            lblCurrentBig.Size = new Size(474, 98);
            lblCurrentBig.TabIndex = 0;
            lblCurrentBig.Text = "0.000 PSI";
            lblCurrentBig.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGoMinus
            // 
            btnGoMinus.Dock = DockStyle.Fill;
            btnGoMinus.Location = new Point(738, 0);
            btnGoMinus.Margin = new Padding(6, 0, 0, 0);
            btnGoMinus.Name = "btnGoMinus";
            btnGoMinus.Size = new Size(238, 104);
            btnGoMinus.TabIndex = 3;
            btnGoMinus.Text = "GO: Value - increment";
            // 
            // AlicatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 420);
            Controls.Add(root);
            MinimumSize = new Size(960, 360);
            Name = "AlicatForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alicat — Controller";
            root.ResumeLayout(false);
            root.PerformLayout();
            header.ResumeLayout(false);
            header.PerformLayout();
            groupTop.ResumeLayout(false);
            groupTop.PerformLayout();
            tableTop.ResumeLayout(false);
            tableTop.PerformLayout();
            tableIncrement.ResumeLayout(false);
            tableIncrement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudIncrement).EndInit();
            panelCurrent.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
    }
}
