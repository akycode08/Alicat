// ----------------------------------------------------------------------------
// Файл: AlicatForm.Designer.cs
// Описание: Разметка (дизайн) главной формы. Минимальная логика только для UI:
//           кнопка "Go to target" активируется при отметке "Confirm to go".
// ----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat
{
    partial class AlicatForm
    {
        private System.ComponentModel.IContainer components = null;

        // ---------- Верхний уровень макета ----------
        private TableLayoutPanel root;
        private TableLayoutPanel header;
        private Label lblTitle;
        private Button btnCommunication;

        // ---------- Группа с основными элементами ----------
        private GroupBox groupTop;
        private TableLayoutPanel tableTop;

        // 1-я строка tableTop
        private Button btnGoPlus;
        private Panel panelCurrent;
        private Label lblCurrentBig;
        private Button btnGoMinus;

        // 2-я строка tableTop: Increment
        private TableLayoutPanel tableIncrement;
        private Label lblIncrement;
        private NumericUpDown nudIncrement;

        // --- Purge ---
        private System.Windows.Forms.GroupBox grpPurge;
        private System.Windows.Forms.Label lblPurgeHint;
        private System.Windows.Forms.CheckBox chkConfirmPurge;
        private System.Windows.Forms.Button btnPurge;

        // 3-я строка tableTop: Target value
        private TableLayoutPanel tableTarget;
        private Label lblTargetTitle;
        private TextBox txtTarget;
        private CheckBox chkConfirmGo;
        private Button btnGoTarget;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            root = new TableLayoutPanel();
            header = new TableLayoutPanel();
            lblTitle = new Label();
            btnCommunication = new Button();

            groupTop = new GroupBox();
            tableTop = new TableLayoutPanel();

            btnGoPlus = new Button();
            panelCurrent = new Panel();
            lblCurrentBig = new Label();
            btnGoMinus = new Button();

            tableIncrement = new TableLayoutPanel();
            lblIncrement = new Label();
            nudIncrement = new NumericUpDown();

            tableTarget = new TableLayoutPanel();
            lblTargetTitle = new Label();
            txtTarget = new TextBox();
            chkConfirmGo = new CheckBox();
            btnGoTarget = new Button();

            // --- begin layout suspend ---
            root.SuspendLayout();
            header.SuspendLayout();
            groupTop.SuspendLayout();
            tableTop.SuspendLayout();
            tableIncrement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudIncrement).BeginInit();
            panelCurrent.SuspendLayout();
            tableTarget.SuspendLayout();
            SuspendLayout();

            // ====================================================================
            // ROOT
            // ====================================================================
            root.ColumnCount = 1;
            root.ColumnStyles.Clear();
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowCount = 2;
            root.RowStyles.Clear();
            root.RowStyles.Add(new RowStyle());                          // header
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));    // content
            root.Dock = DockStyle.Fill;
            root.Padding = new Padding(12);
            root.Name = "root";
            root.Size = new Size(1024, 520);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(groupTop, 0, 1);

            // ====================================================================
            // HEADER
            // ====================================================================
            header.ColumnCount = 2;
            header.ColumnStyles.Clear();
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.RowCount = 1;
            header.RowStyles.Clear();
            header.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            header.Dock = DockStyle.Top;
            header.Margin = new Padding(0, 0, 0, 8);
            header.Name = "header";
            header.Size = new Size(1000, 48);

            lblTitle.Anchor = AnchorStyles.Left;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Text = "Alicat Controller";

            btnCommunication.Anchor = AnchorStyles.Right;
            btnCommunication.AutoSize = true;
            btnCommunication.Text = "Communication…";
            btnCommunication.Margin = new Padding(8, 8, 0, 8);
            btnCommunication.Name = "btnCommunication";

            header.Controls.Add(lblTitle, 0, 0);
            header.Controls.Add(btnCommunication, 1, 0);

            // ====================================================================
            // GROUP TOP
            // ====================================================================
            groupTop.AutoSize = true;
            groupTop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupTop.Dock = DockStyle.Fill;
            groupTop.Padding = new Padding(12);
            groupTop.Name = "groupTop";
            groupTop.Controls.Add(tableTop);

            // ====================================================================
            // TABLE TOP (3 колонки)
            // ====================================================================
            tableTop.AutoSize = true;
            tableTop.Dock = DockStyle.Fill;
            tableTop.ColumnCount = 3;
            tableTop.ColumnStyles.Clear();
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F)); // +
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Current
            tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F)); // -
            tableTop.RowCount = 3; // 1: +/-/current, 2: increment, 3: target
            tableTop.RowStyles.Clear();
            tableTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // верхняя линия
            tableTop.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // increment
            tableTop.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // target тянется вниз
            tableTop.Name = "tableTop";

            // ---------- [+] ----------
            btnGoPlus.Dock = DockStyle.Fill;
            btnGoPlus.Margin = new Padding(0, 0, 6, 6);
            btnGoPlus.Text = "GO: Value + increment";
            tableTop.Controls.Add(btnGoPlus, 0, 0);

            // ---------- [Current] ----------
            panelCurrent.BackColor = Color.White;
            panelCurrent.BorderStyle = BorderStyle.FixedSingle;
            panelCurrent.Dock = DockStyle.Fill;
            panelCurrent.Margin = new Padding(6, 0, 6, 6);
            panelCurrent.Controls.Add(lblCurrentBig);
            tableTop.Controls.Add(panelCurrent, 1, 0);

            lblCurrentBig.Dock = DockStyle.Fill;
            lblCurrentBig.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblCurrentBig.Text = "0.0 PSIG";
            lblCurrentBig.TextAlign = ContentAlignment.MiddleCenter;

            // ---------- [–] ----------
            btnGoMinus.Dock = DockStyle.Fill;
            btnGoMinus.Margin = new Padding(6, 0, 0, 6);
            btnGoMinus.Text = "GO: Value - increment";
            tableTop.Controls.Add(btnGoMinus, 2, 0);

            // ====================================================================
            // INCREMENT (2-я строка)
            // ====================================================================
            tableIncrement.AutoSize = true;
            tableIncrement.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableIncrement.Dock = DockStyle.Top;
            tableIncrement.Margin = new Padding(0, 4, 0, 0);
            tableIncrement.ColumnCount = 3;
            tableIncrement.ColumnStyles.Clear();
            tableIncrement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // spacer
            tableIncrement.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));      // label
            tableIncrement.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));      // nud

            lblIncrement.Anchor = AnchorStyles.Left;
            lblIncrement.AutoSize = true;
            lblIncrement.Margin = new Padding(0, 6, 8, 6);
            lblIncrement.Text = "Increment";

            nudIncrement.DecimalPlaces = 3;
            nudIncrement.Increment = 0.001M;
            nudIncrement.Minimum = 0.001M;
            nudIncrement.Maximum = 100000M;
            nudIncrement.Value = 1.000M;
            nudIncrement.Size = new Size(110, 23);
            nudIncrement.Margin = new Padding(0, 3, 0, 3);
            nudIncrement.Name = "nudIncrement";

            tableIncrement.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 0);
            tableIncrement.Controls.Add(lblIncrement, 1, 0);
            tableIncrement.Controls.Add(nudIncrement, 2, 0);

            tableTop.Controls.Add(tableIncrement, 0, 1);
            tableTop.SetColumnSpan(tableIncrement, 3);

            this.grpPurge = new System.Windows.Forms.GroupBox();
            this.chkConfirmPurge = new System.Windows.Forms.CheckBox();
            this.btnPurge = new System.Windows.Forms.Button();

            // 
            // grpPurge
            // 
            this.grpPurge.Text = "Purge";
            this.grpPurge.Name = "grpPurge";
            this.grpPurge.Size = new System.Drawing.Size(200, 100); // подстрой под свою правую панель
            this.grpPurge.Location = new System.Drawing.Point(750, 300); // поставь ниже Increment
            this.grpPurge.TabStop = false;

            // 
            // chkConfirmPurge
            // 
            this.chkConfirmPurge.AutoSize = true;
            this.chkConfirmPurge.Text = "Confirm purge to 0";
            this.chkConfirmPurge.Location = new System.Drawing.Point(15, 25);
            this.chkConfirmPurge.Name = "chkConfirmPurge";

            // 
            // btnPurge
            // 
            this.btnPurge.Text = "Purge";
            this.btnPurge.Size = new System.Drawing.Size(150, 30);
            this.btnPurge.Location = new System.Drawing.Point(15, 55);
            this.btnPurge.Name = "btnPurge";
            // обработчик добавим позже:
            // this.btnPurge.Click += new System.EventHandler(this.btnPurge_Click);

            // 
            // Добавляем элементы в группу
            // 
            this.grpPurge.Controls.Add(this.chkConfirmPurge);
            this.grpPurge.Controls.Add(this.btnPurge);

            // 
            // Добавляем группу на форму (в правую часть, под Increment)
            // 
            this.Controls.Add(this.grpPurge);

            // ====================================================================
            // TARGET (3-я строка) — чистый дизайн
            // ====================================================================
            tableTarget.AutoSize = true;
            tableTarget.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableTarget.Dock = DockStyle.Fill;               // занимает оставшееся место
            tableTarget.Margin = new Padding(0, 10, 0, 0);
            tableTarget.ColumnCount = 3;
            tableTarget.ColumnStyles.Clear();
            tableTarget.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableTarget.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // центральная колонка с контентом
            tableTarget.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));

            // Внутренняя стек-табличка для центральной колонки
            var targetStack = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 4,
                Margin = new Padding(0)
            };
            targetStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            targetStack.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // title
            targetStack.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // textbox
            targetStack.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // checkbox
            targetStack.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // button

            // Заголовок
            lblTargetTitle.AutoSize = true;
            lblTargetTitle.Font = new Font("Segoe UI", 10F);
            lblTargetTitle.Text = "Target value";
            lblTargetTitle.Margin = new Padding(0, 0, 0, 6);

            // Поле ввода
            txtTarget.Font = new Font("Segoe UI", 10F);
            txtTarget.PlaceholderText = "Target value";
            txtTarget.Size = new Size(220, 25);
            txtTarget.Margin = new Padding(0, 0, 0, 6);
            txtTarget.Name = "txtTarget";

            // Чекбокс
            chkConfirmGo.AutoSize = true;
            chkConfirmGo.Font = new Font("Segoe UI", 9F);
            chkConfirmGo.Text = "Confirm to go";
            chkConfirmGo.Margin = new Padding(0, 0, 0, 8);
            chkConfirmGo.Name = "chkConfirmGo";

            // Кнопка
            btnGoTarget.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGoTarget.Text = "Go to target";
            btnGoTarget.Size = new Size(220, 36);
            btnGoTarget.Margin = new Padding(0, 0, 0, 0);
            btnGoTarget.Name = "btnGoTarget";
            //btnGoTarget.Enabled = false; // по умолчанию выключена

            // Мини-логика UI: активируем кнопку, когда отметили чекбокс
            chkConfirmGo.CheckedChanged += (s, e) =>
            {
                btnGoTarget.Enabled = chkConfirmGo.Checked;
            };

            // Сборка стека
            targetStack.Controls.Add(lblTargetTitle, 0, 0);
            targetStack.Controls.Add(txtTarget, 0, 1);
            targetStack.Controls.Add(chkConfirmGo, 0, 2);
            targetStack.Controls.Add(btnGoTarget, 0, 3);

            // Кладём стек в центральную колонку tableTarget
            tableTarget.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 0);
            tableTarget.Controls.Add(targetStack, 1, 0);
            tableTarget.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 0);

            tableTop.Controls.Add(tableTarget, 0, 2);
            tableTop.SetColumnSpan(tableTarget, 3);

            // ====================================================================
            // ФОРМА
            // ====================================================================
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 520);
            MinimumSize = new Size(960, 420);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alicat — Controller";
            Controls.Add(root);
            Name = "AlicatForm";

            // --- end layout resume ---
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
            tableTarget.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
    }
}
