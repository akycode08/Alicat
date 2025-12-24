// ----------------------------------------------------------------------------
// Файл: AlicatForm.Designer.cs
// Описание: Разметка (дизайн) главной формы.
// Макет:
//   [MenuStrip]
//   [Tabs: Graph | Table | Statistics | Terminal]  ← обычные кнопки
//   [2x2 grid:
//        Block 1: Increment
//        Block 2: Target
//        Block 3: Data (Current + values)
//        Block 4: Purge]
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

        // ---------- Главное меню ----------
        private MenuStrip menuMain;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuFileNewSession;
        private ToolStripMenuItem menuFileTestMode;
        private ToolStripMenuItem menuSettings;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuSettingsOptions;
        private ToolStripMenuItem menuSettingsCommunication;

        // ---------- Вкладки (просто 4 кнопки) ----------
        private Panel panelTabs;
        private TableLayoutPanel tableTabs;
        private Button btnGraph;
        private Button btnTable;
        private Button btnStatistics;
        private Button btnTerminal;

        // ---------- Основная 2x2 сетка ----------
        private TableLayoutPanel layoutMain;

        // Block 1: Increment
        private GroupBox grpIncrementBlock;
        private Label lblIncrement;
        private NumericUpDown nudIncrement;
        private Button btnGoPlus;
        private Button btnGoMinus;

        // Block 2: Target
        private GroupBox grpTargetBlock;
        private Label lblTargetTitle;
        private TextBox txtTarget;
        private CheckBox chkConfirmGo;
        private Button btnGoTarget;

        // Block 3: Data
        private GroupBox grpData;
        private Panel panelCurrent;
        private Label lblCurrentBig;
        private Label lblPressureUnits;
        private Label lblRampSpeedUnits;
        private Label lblSetPoint;
        private Label lblTimeToSetPoint;
        private Label lblStatus;

        private Label boxPressureUnits;
        private Label boxRampSpeedUnits;
        private Label boxSetPoint;
        private Label boxTimeToSetPoint;

        private Label icoUp;
        private Label icoMid;
        private Label icoDown;

        // Block 4: Purge
        private GroupBox grpPurge;
        private CheckBox chkConfirmPurge;
        private Button btnPurge;

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

            // ========= ИНИЦИАЛИЗАЦИЯ КОНТРОЛОВ =========
            root = new TableLayoutPanel();
            header = new TableLayoutPanel();

            // меню
            menuMain = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuSettings = new ToolStripMenuItem();
            menuView = new ToolStripMenuItem();
            menuSettingsOptions = new ToolStripMenuItem();
            menuSettingsCommunication = new ToolStripMenuItem();

            layoutMain = new TableLayoutPanel();

            grpIncrementBlock = new GroupBox();
            lblIncrement = new Label();
            nudIncrement = new NumericUpDown();
            btnGoPlus = new Button();
            btnGoMinus = new Button();

            grpTargetBlock = new GroupBox();
            lblTargetTitle = new Label();
            txtTarget = new TextBox();
            chkConfirmGo = new CheckBox();
            btnGoTarget = new Button();

            grpData = new GroupBox();
            panelCurrent = new Panel();
            lblCurrentBig = new Label();
            lblPressureUnits = new Label();
            lblRampSpeedUnits = new Label();
            lblSetPoint = new Label();
            lblTimeToSetPoint = new Label();
            lblStatus = new Label();
            boxPressureUnits = new Label();
            boxRampSpeedUnits = new Label();
            boxSetPoint = new Label();
            boxTimeToSetPoint = new Label();
            icoUp = new Label();
            icoMid = new Label();
            icoDown = new Label();

            grpPurge = new GroupBox();
            chkConfirmPurge = new CheckBox();
            btnPurge = new Button();

            // --- begin layout suspend ---
            root.SuspendLayout();
            header.SuspendLayout();
            menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudIncrement).BeginInit();
            panelCurrent.SuspendLayout();
            grpIncrementBlock.SuspendLayout();
            grpTargetBlock.SuspendLayout();
            grpData.SuspendLayout();
            grpPurge.SuspendLayout();
            SuspendLayout();

            // ====================================================================
            // MENU STRIP (верхняя строка)
            // ====================================================================
            menuMain.Name = "menuMain";
            menuMain.Dock = DockStyle.Top;
            menuMain.ImageScalingSize = new Size(20, 20);

            menuFile.Name = "menuFile";
            menuFile.Text = "File";

            menuFileNewSession = new ToolStripMenuItem();
            menuFileNewSession.Name = "menuFileNewSession";
            menuFileNewSession.Text = "New Session...";

            menuFile.DropDownItems.Add(menuFileNewSession);

            menuFileTestMode = new ToolStripMenuItem();
            menuFileTestMode.Name = "menuFileTestMode";
            menuFileTestMode.Text = "Start Test Mode";

            menuFile.DropDownItems.Add(menuFileTestMode);

            menuSettings.Name = "menuSettings";
            menuSettings.Text = "Settings";

            menuView.Name = "menuView";
            menuView.Text = "View";

            menuSettingsOptions.Name = "menuSettingsOptions";
            menuSettingsOptions.Text = "Options";

            menuSettingsCommunication.Name = "menuSettingsCommunication";
            menuSettingsCommunication.Text = "Communication";

            menuSettings.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuSettingsOptions,
                menuSettingsCommunication
            });

            menuMain.Items.AddRange(new ToolStripItem[]
            {
                menuFile,
                menuSettings,
                menuView,
            });

            // ====================================================================
            // ROOT
            // ====================================================================
            root.ColumnCount = 1;
            root.ColumnStyles.Clear();
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowCount = 2;
            root.RowStyles.Clear();
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));       // header (табы)
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));       // main layout
            root.Dock = DockStyle.Fill;
            root.Padding = new Padding(12);
            root.Name = "root";
            root.Size = new Size(1024, 520);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(layoutMain, 0, 1);

            // ====================================================================
            // HEADER (строка с 4 кнопками)
            // ====================================================================
            header.ColumnCount = 1;
            header.ColumnStyles.Clear();
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.RowCount = 1;
            header.RowStyles.Clear();
            header.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            header.Dock = DockStyle.Fill;
            header.Margin = new Padding(0, 6, 0, 8);
            header.Name = "header";
            header.Size = new Size(1000, 34);

            // ====== панель для кнопок ======
            panelTabs = new Panel();
            panelTabs.Dock = DockStyle.Fill;
            panelTabs.BackColor = SystemColors.Control;
            panelTabs.Padding = new Padding(0);
            panelTabs.Margin = new Padding(0);

            // ====== сетка 1x4 ======
            tableTabs = new TableLayoutPanel();
            tableTabs.ColumnCount = 4;
            tableTabs.RowCount = 1;
            tableTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTabs.Dock = DockStyle.Fill;
            tableTabs.Margin = new Padding(0);
            tableTabs.Padding = new Padding(4, 4, 4, 0);

            // ====== 4 обычные кнопки ======
            btnGraph = new Button();
            btnGraph.Text = "Graph";
            btnGraph.Dock = DockStyle.Fill;

            btnTable = new Button();
            btnTable.Text = "Table";
            btnTable.Dock = DockStyle.Fill;

            btnStatistics = new Button();
            btnStatistics.Text = "Statistics";
            btnStatistics.Dock = DockStyle.Fill;

            btnTerminal = new Button();
            btnTerminal.Text = "Terminal";
            btnTerminal.Dock = DockStyle.Fill;

            tableTabs.Controls.Add(btnGraph, 0, 0);
            tableTabs.Controls.Add(btnTable, 1, 0);
            tableTabs.Controls.Add(btnStatistics, 2, 0);
            tableTabs.Controls.Add(btnTerminal, 3, 0);

            panelTabs.Controls.Add(tableTabs);
            header.Controls.Add(panelTabs, 0, 0);

            // ====================================================================
            // LAYOUT MAIN (2x2 блоки)
            // ====================================================================
            layoutMain.Name = "layoutMain";
            layoutMain.Dock = DockStyle.Fill;
            layoutMain.Margin = new Padding(0);
            layoutMain.ColumnCount = 2;
            layoutMain.RowCount = 2;
            layoutMain.ColumnStyles.Clear();
            layoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layoutMain.RowStyles.Clear();
            layoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // ====================================================================
            // BLOCK 1: Increment
            // ====================================================================
            grpIncrementBlock.Text = "";
            grpIncrementBlock.Dock = DockStyle.Fill;
            grpIncrementBlock.Padding = new Padding(8);

            var incLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                Margin = new Padding(0)
            };
            incLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            incLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            incLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            incLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            incLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            incLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblIncrement.Anchor = AnchorStyles.Left;
            lblIncrement.AutoSize = true;
            lblIncrement.Text = "Increment:";
            lblIncrement.Margin = new Padding(0, 3, 0, 3);

            nudIncrement.DecimalPlaces = 1;
            nudIncrement.Increment = 0.1M;
            nudIncrement.Minimum = 0.001M;
            nudIncrement.Maximum = 100000M;
            nudIncrement.Value = 1.000M;
            nudIncrement.Size = new Size(110, 23);
            nudIncrement.Margin = new Padding(0, 0, 0, 0);
            nudIncrement.Name = "nudIncrement";
            nudIncrement.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            var incInner = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            incInner.Controls.Add(lblIncrement);
            incInner.Controls.Add(nudIncrement);

            incLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 0);
            incLayout.Controls.Add(incInner, 1, 0);
            incLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 0);

            btnGoPlus.Text = "GO: + increment";
            btnGoPlus.Margin = new Padding(0, 8, 0, 0);
            btnGoPlus.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            incLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 1);
            incLayout.Controls.Add(btnGoPlus, 1, 1);
            incLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 1);

            btnGoMinus.Text = "GO: - increment";
            btnGoMinus.Margin = new Padding(0, 8, 0, 0);
            btnGoMinus.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            incLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 2);
            incLayout.Controls.Add(btnGoMinus, 1, 2);
            incLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 2);

            grpIncrementBlock.Controls.Add(incLayout);

            // ====================================================================
            // BLOCK 2: Target
            // ====================================================================
            grpTargetBlock.Text = "";
            grpTargetBlock.Dock = DockStyle.Fill;
            grpTargetBlock.Padding = new Padding(8);

            var targetLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 4,
                Margin = new Padding(0)
            };
            targetLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            targetLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            targetLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblTargetTitle.AutoSize = true;
            lblTargetTitle.Font = new Font("Segoe UI", 10F);
            lblTargetTitle.Text = "Target value";
            lblTargetTitle.Margin = new Padding(0, 0, 0, 4);

            txtTarget.Font = new Font("Segoe UI", 10F);
            txtTarget.PlaceholderText = "Target value";
            txtTarget.Size = new Size(220, 25);
            txtTarget.Margin = new Padding(0, 0, 0, 4);
            txtTarget.Name = "txtTarget";

            chkConfirmGo.AutoSize = true;
            chkConfirmGo.Font = new Font("Segoe UI", 9F);
            chkConfirmGo.Text = "Confirm";
            chkConfirmGo.Margin = new Padding(0, 4, 0, 8);
            chkConfirmGo.Name = "chkConfirmGo";

            btnGoTarget.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGoTarget.Text = "Go to target";
            btnGoTarget.Size = new Size(220, 32);
            btnGoTarget.Margin = new Padding(0, 0, 0, 0);
            btnGoTarget.Name = "btnGoTarget";
            btnGoTarget.Enabled = false;

            chkConfirmGo.CheckedChanged += (s, e) =>
            {
                btnGoTarget.Enabled = chkConfirmGo.Checked;
            };

            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 0);
            targetLayout.Controls.Add(lblTargetTitle, 1, 0);
            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 0);

            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 1);
            targetLayout.Controls.Add(txtTarget, 1, 1);
            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 1);

            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 2);
            targetLayout.Controls.Add(chkConfirmGo, 1, 2);
            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 2);

            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 3);
            targetLayout.Controls.Add(btnGoTarget, 1, 3);
            targetLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 3);

            grpTargetBlock.Controls.Add(targetLayout);

            // ===================================================================
            // BLOCK 3: DATA
            // ===================================================================
            grpData.Text = "";
            grpData.Dock = DockStyle.Fill;
            grpData.Padding = new Padding(8);

            var dataLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };
            dataLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            dataLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            panelCurrent.BackColor = Color.White;
            panelCurrent.BorderStyle = BorderStyle.FixedSingle;
            panelCurrent.Dock = DockStyle.Fill;
            panelCurrent.Margin = new Padding(0, 0, 0, 8);

            lblCurrentBig.Dock = DockStyle.Fill;
            lblCurrentBig.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblCurrentBig.Text = "0.0 PSIG";
            lblCurrentBig.TextAlign = ContentAlignment.MiddleCenter;

            panelCurrent.Controls.Add(lblCurrentBig);
            dataLayout.Controls.Add(panelCurrent, 0, 0);

            var tableInfo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Margin = new Padding(0)
            };
            tableInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 5; i++)
                tableInfo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblPressureUnits.AutoSize = true;
            lblPressureUnits.Text = "Pressure units:";
            lblPressureUnits.Margin = new Padding(0, 2, 0, 2);

            boxPressureUnits.BorderStyle = BorderStyle.FixedSingle;
            boxPressureUnits.TextAlign = ContentAlignment.MiddleCenter;
            boxPressureUnits.Margin = new Padding(4, 2, 0, 2);
            boxPressureUnits.Size = new Size(90, 20);
            boxPressureUnits.Text = "PSIG";

            lblRampSpeedUnits.AutoSize = true;
            lblRampSpeedUnits.Margin = new Padding(0, 2, 0, 2);
            lblRampSpeedUnits.Text = "Ramp Speed:";

            boxRampSpeedUnits.BorderStyle = BorderStyle.FixedSingle;
            boxRampSpeedUnits.TextAlign = ContentAlignment.MiddleCenter;
            boxRampSpeedUnits.Margin = new Padding(4, 2, 0, 2);
            boxRampSpeedUnits.Size = new Size(90, 20);
            boxRampSpeedUnits.Text = "PSIG/s";

            lblSetPoint.AutoSize = true;
            lblSetPoint.Margin = new Padding(0, 2, 0, 2);
            lblSetPoint.Text = "Set point:";

            boxSetPoint.BorderStyle = BorderStyle.FixedSingle;
            boxSetPoint.TextAlign = ContentAlignment.MiddleCenter;
            boxSetPoint.Margin = new Padding(4, 2, 0, 2);
            boxSetPoint.Size = new Size(90, 20);
            boxSetPoint.Text = "0 PSIG";

            lblTimeToSetPoint.AutoSize = true;
            lblTimeToSetPoint.Margin = new Padding(0, 2, 0, 2);
            lblTimeToSetPoint.Text = "ETA:";

            boxTimeToSetPoint.BorderStyle = BorderStyle.FixedSingle;
            boxTimeToSetPoint.TextAlign = ContentAlignment.MiddleCenter;
            boxTimeToSetPoint.Margin = new Padding(4, 2, 0, 2);
            boxTimeToSetPoint.Size = new Size(90, 20);
            boxTimeToSetPoint.Text = "—";

            lblStatus.AutoSize = true;
            lblStatus.Margin = new Padding(0, 2, 0, 2);
            lblStatus.Text = "Status:";

            icoUp.BorderStyle = BorderStyle.FixedSingle;
            icoUp.TextAlign = ContentAlignment.MiddleCenter;
            icoUp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            icoUp.Size = new Size(20, 20);
            icoUp.Text = "▲";
            icoUp.ForeColor = Color.Gray;
            icoUp.Margin = new Padding(0, 0, 4, 0);

            icoMid.BorderStyle = BorderStyle.FixedSingle;
            icoMid.TextAlign = ContentAlignment.MiddleCenter;
            icoMid.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            icoMid.Size = new Size(20, 20);
            icoMid.Text = "●";
            icoMid.ForeColor = Color.Gray;
            icoMid.Margin = new Padding(0, 0, 4, 0);

            icoDown.BorderStyle = BorderStyle.FixedSingle;
            icoDown.TextAlign = ContentAlignment.MiddleCenter;
            icoDown.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            icoDown.Size = new Size(20, 20);
            icoDown.Text = "▼";
            icoDown.ForeColor = Color.Gray;
            icoDown.Margin = new Padding(0, 0, 0, 0);

            var panelStatusIcons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(4, 0, 0, 0)
            };
            panelStatusIcons.Controls.Add(icoUp);
            panelStatusIcons.Controls.Add(icoMid);
            panelStatusIcons.Controls.Add(icoDown);

            tableInfo.Controls.Add(lblPressureUnits, 0, 0);
            tableInfo.Controls.Add(boxPressureUnits, 1, 0);

            tableInfo.Controls.Add(lblRampSpeedUnits, 0, 1);
            tableInfo.Controls.Add(boxRampSpeedUnits, 1, 1);

            tableInfo.Controls.Add(lblSetPoint, 0, 2);
            tableInfo.Controls.Add(boxSetPoint, 1, 2);

            tableInfo.Controls.Add(lblTimeToSetPoint, 0, 3);
            tableInfo.Controls.Add(boxTimeToSetPoint, 1, 3);

            tableInfo.Controls.Add(lblStatus, 0, 4);
            tableInfo.Controls.Add(panelStatusIcons, 1, 4);

            dataLayout.Controls.Add(tableInfo, 0, 1);
            grpData.Controls.Add(dataLayout);

            // ===================================================================
            // BLOCK 4: PURGE
            // ===================================================================
            grpPurge.Text = "";
            grpPurge.Dock = DockStyle.Fill;
            grpPurge.Padding = new Padding(8);

            chkConfirmPurge.AutoSize = true;
            chkConfirmPurge.Text = "Confirm purge to 0";
            chkConfirmPurge.Name = "chkConfirmPurge";
            chkConfirmPurge.Margin = new Padding(0, 0, 0, 8);

            btnPurge.Text = "Purge";
            btnPurge.Name = "btnPurge";
            btnPurge.Size = new Size(150, 30);
            btnPurge.Margin = new Padding(0, 0, 0, 4);

            var lblPurgeInfo = new Label
            {
                AutoSize = true,
                Text = "Opens exhaust to 0,\nthen returns to control.",
                Margin = new Padding(0, 4, 0, 0)
            };

            var purgeLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                Margin = new Padding(0)
            };
            purgeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            purgeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            purgeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            purgeLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            purgeLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            purgeLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            purgeLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 0);
            purgeLayout.Controls.Add(chkConfirmPurge, 1, 0);
            purgeLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 0);

            purgeLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 0, 1);
            purgeLayout.Controls.Add(btnPurge, 1, 1);
            purgeLayout.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 2, 1);

            purgeLayout.SetColumnSpan(lblPurgeInfo, 3);
            purgeLayout.Controls.Add(lblPurgeInfo, 0, 2);

            grpPurge.Controls.Add(purgeLayout);

            // ====================================================================
            // РАЗМЕЩЕНИЕ БЛОКОВ В layoutMain
            // ====================================================================
            layoutMain.Controls.Add(grpIncrementBlock, 0, 0);
            layoutMain.Controls.Add(grpTargetBlock, 1, 0);
            layoutMain.Controls.Add(grpData, 0, 1);
            layoutMain.Controls.Add(grpPurge, 1, 1);

            // ====================================================================
            // ФОРМА
            // ====================================================================
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 520);
            MinimumSize = new Size(960, 420);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alicat — Controller";
            Name = "AlicatForm";

            Controls.Add(root);
            Controls.Add(menuMain);
            MainMenuStrip = menuMain;
            menuMain.BringToFront();

            // --- end layout resume ---
            root.ResumeLayout(false);
            header.ResumeLayout(false);
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudIncrement).EndInit();
            panelCurrent.ResumeLayout(false);
            grpIncrementBlock.ResumeLayout(false);
            grpTargetBlock.ResumeLayout(false);
            grpData.ResumeLayout(false);
            grpPurge.ResumeLayout(false);
            grpPurge.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
