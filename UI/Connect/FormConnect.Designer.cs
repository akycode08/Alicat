// Файл: FormConnect.Designer.cs
// Этот файл создаёт визуальные элементы формы и настраивает их свойства/раскладку.

namespace Alicat
{
    partial class FormConnect
    {
        // Контейнер для компонентов, чтобы правильно освобождать ресурсы
        private System.ComponentModel.IContainer components = null;

        // Правильная утилизация компонентов при закрытии формы
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Метод, который конструирует все контролы на форме и задаёт им свойства
        private void InitializeComponent()
        {
            grp = new GroupBox();
            grid = new TableLayoutPanel();
            lblPort = new Label();
            cbPortName = new ComboBox();
            btnRefreshPorts = new Button();
            lblBaud = new Label();
            cbBaudRate = new ComboBox();
            lblParity = new Label();
            cbParity = new ComboBox();
            lblDataBits = new Label();
            nudDataBits = new NumericUpDown();
            lblStopBits = new Label();
            cbStopBits = new ComboBox();
            lblReadTo = new Label();
            nudReadTimeout = new NumericUpDown();
            lblWriteTo = new Label();
            nudWriteTimeout = new NumericUpDown();
            btnDefaults = new Button();
            btnConnect = new Button();
            btnDisconnect = new Button();
            grp.SuspendLayout();
            grid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDataBits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudReadTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteTimeout).BeginInit();
            SuspendLayout();
            // 
            // grp
            // 
            grp.Controls.Add(grid);
            grp.Font = new Font("Segoe UI", 9F);
            grp.Location = new Point(12, 12);
            grp.Name = "grp";
            grp.Size = new Size(300, 230);
            grp.TabIndex = 0;
            grp.TabStop = false;
            grp.Text = "Serial Port Settings";
            // 
            // grid
            // 
            grid.ColumnCount = 3;
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 36F));
            grid.Controls.Add(lblPort, 0, 0);
            grid.Controls.Add(cbPortName, 1, 0);
            grid.Controls.Add(btnRefreshPorts, 2, 0);
            grid.Controls.Add(lblBaud, 0, 1);
            grid.Controls.Add(cbBaudRate, 1, 1);
            grid.Controls.Add(lblParity, 0, 2);
            grid.Controls.Add(cbParity, 1, 2);
            grid.Controls.Add(lblDataBits, 0, 3);
            grid.Controls.Add(nudDataBits, 1, 3);
            grid.Controls.Add(lblStopBits, 0, 4);
            grid.Controls.Add(cbStopBits, 1, 4);
            grid.Controls.Add(lblReadTo, 0, 5);
            grid.Controls.Add(nudReadTimeout, 1, 5);
            grid.Controls.Add(lblWriteTo, 0, 6);
            grid.Controls.Add(nudWriteTimeout, 1, 6);
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(3, 19);
            grid.Name = "grid";
            grid.RowCount = 7;
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            grid.Size = new Size(294, 208);
            grid.TabIndex = 0;
            // 
            // lblPort
            // 
            lblPort.Anchor = AnchorStyles.Left;
            lblPort.AutoSize = true;
            lblPort.Location = new Point(3, 7);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(61, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "PortName";
            // 
            // cbPortName
            // 
            cbPortName.Dock = DockStyle.Fill;
            cbPortName.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPortName.FormattingEnabled = true;
            cbPortName.Location = new Point(93, 3);
            cbPortName.Name = "cbPortName";
            cbPortName.Size = new Size(162, 23);
            cbPortName.TabIndex = 1;
            // 
            // btnRefreshPorts
            // 
            btnRefreshPorts.Dock = DockStyle.Fill;
            btnRefreshPorts.Location = new Point(261, 3);
            btnRefreshPorts.Name = "btnRefreshPorts";
            btnRefreshPorts.Size = new Size(30, 24);
            btnRefreshPorts.TabIndex = 2;
            btnRefreshPorts.Text = "↻";
            btnRefreshPorts.UseVisualStyleBackColor = true;
            btnRefreshPorts.Click += btnRefreshPorts_Click;
            // 
            // lblBaud
            // 
            lblBaud.Anchor = AnchorStyles.Left;
            lblBaud.AutoSize = true;
            lblBaud.Location = new Point(3, 37);
            lblBaud.Name = "lblBaud";
            lblBaud.Size = new Size(57, 15);
            lblBaud.TabIndex = 3;
            lblBaud.Text = "BaudRate";
            // 
            // cbBaudRate
            // 
            cbBaudRate.Dock = DockStyle.Fill;
            cbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaudRate.FormattingEnabled = true;
            cbBaudRate.Location = new Point(93, 33);
            cbBaudRate.Name = "cbBaudRate";
            cbBaudRate.Size = new Size(162, 23);
            cbBaudRate.TabIndex = 4;
            // 
            // lblParity
            // 
            lblParity.Anchor = AnchorStyles.Left;
            lblParity.AutoSize = true;
            lblParity.Location = new Point(3, 67);
            lblParity.Name = "lblParity";
            lblParity.Size = new Size(37, 15);
            lblParity.TabIndex = 5;
            lblParity.Text = "Parity";
            // 
            // cbParity
            // 
            cbParity.Dock = DockStyle.Fill;
            cbParity.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParity.FormattingEnabled = true;
            cbParity.Location = new Point(93, 63);
            cbParity.Name = "cbParity";
            cbParity.Size = new Size(162, 23);
            cbParity.TabIndex = 6;
            // 
            // lblDataBits
            // 
            lblDataBits.Anchor = AnchorStyles.Left;
            lblDataBits.AutoSize = true;
            lblDataBits.Location = new Point(3, 97);
            lblDataBits.Name = "lblDataBits";
            lblDataBits.Size = new Size(50, 15);
            lblDataBits.TabIndex = 7;
            lblDataBits.Text = "DataBits";
            // 
            // nudDataBits
            // 
            nudDataBits.Dock = DockStyle.Fill;
            nudDataBits.Location = new Point(93, 93);
            nudDataBits.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            nudDataBits.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudDataBits.Name = "nudDataBits";
            nudDataBits.Size = new Size(162, 23);
            nudDataBits.TabIndex = 8;
            nudDataBits.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // lblStopBits
            // 
            lblStopBits.Anchor = AnchorStyles.Left;
            lblStopBits.AutoSize = true;
            lblStopBits.Location = new Point(3, 127);
            lblStopBits.Name = "lblStopBits";
            lblStopBits.Size = new Size(50, 15);
            lblStopBits.TabIndex = 9;
            lblStopBits.Text = "StopBits";
            // 
            // cbStopBits
            // 
            cbStopBits.Dock = DockStyle.Fill;
            cbStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStopBits.FormattingEnabled = true;
            cbStopBits.Location = new Point(93, 123);
            cbStopBits.Name = "cbStopBits";
            cbStopBits.Size = new Size(162, 23);
            cbStopBits.TabIndex = 10;
            // 
            // lblReadTo
            // 
            lblReadTo.Anchor = AnchorStyles.Left;
            lblReadTo.AutoSize = true;
            lblReadTo.Location = new Point(3, 157);
            lblReadTo.Name = "lblReadTo";
            lblReadTo.Size = new Size(81, 15);
            lblReadTo.TabIndex = 11;
            lblReadTo.Text = "Read Timeout";
            // 
            // nudReadTimeout
            // 
            nudReadTimeout.Dock = DockStyle.Fill;
            nudReadTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            nudReadTimeout.Location = new Point(93, 153);
            nudReadTimeout.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudReadTimeout.Name = "nudReadTimeout";
            nudReadTimeout.Size = new Size(162, 23);
            nudReadTimeout.TabIndex = 12;
            nudReadTimeout.Value = new decimal(new int[] { 700, 0, 0, 0 });
            // 
            // lblWriteTo
            // 
            lblWriteTo.Anchor = AnchorStyles.Left;
            lblWriteTo.AutoSize = true;
            lblWriteTo.Location = new Point(3, 187);
            lblWriteTo.Name = "lblWriteTo";
            lblWriteTo.Size = new Size(83, 15);
            lblWriteTo.TabIndex = 13;
            lblWriteTo.Text = "Write Timeout";
            // 
            // nudWriteTimeout
            // 
            nudWriteTimeout.Dock = DockStyle.Fill;
            nudWriteTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            nudWriteTimeout.Location = new Point(93, 183);
            nudWriteTimeout.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudWriteTimeout.Name = "nudWriteTimeout";
            nudWriteTimeout.Size = new Size(162, 23);
            nudWriteTimeout.TabIndex = 14;
            nudWriteTimeout.Value = new decimal(new int[] { 700, 0, 0, 0 });
            // 
            // btnDefaults
            // 
            btnDefaults.Location = new Point(12, 252);
            btnDefaults.Name = "btnDefaults";
            btnDefaults.Size = new Size(300, 30);
            btnDefaults.TabIndex = 1;
            btnDefaults.Text = "Restore Defaults";
            btnDefaults.UseVisualStyleBackColor = true;
            btnDefaults.Click += btnDefaults_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(12, 288);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(300, 30);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(12, 324);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(300, 30);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // FormConnect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 366);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(btnDefaults);
            Controls.Add(grp);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormConnect";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Communication";
            Load += FormConnect_Load;
            grp.ResumeLayout(false);
            grid.ResumeLayout(false);
            grid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDataBits).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudReadTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWriteTimeout).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // ----- Поля-контролы: доступны из кода формы -----
        private System.Windows.Forms.GroupBox grp;
        private System.Windows.Forms.TableLayoutPanel grid;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox cbPortName;
        private System.Windows.Forms.Button btnRefreshPorts;
        private System.Windows.Forms.Label lblBaud;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.NumericUpDown nudDataBits;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cbStopBits;
        private System.Windows.Forms.Label lblReadTo;
        private System.Windows.Forms.NumericUpDown nudReadTimeout;
        private System.Windows.Forms.Label lblWriteTo;
        private System.Windows.Forms.NumericUpDown nudWriteTimeout;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
    }
}
