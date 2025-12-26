// ----------------------------------------------------------------------------
// Файл: AlicatForm.Designer.cs  (DESIGNER-FRIENDLY + View Light/Dark)
// Цель: чтобы Visual Studio WinForms Designer открывался и макет был как на скрине.
// ВАЖНО: В InitializeComponent нет циклов, нет helper-методов, нет var.
// ----------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat
{
    partial class AlicatForm
    {
        private IContainer components = null;

        // =========================
        // ROOT
        // =========================
        private TableLayoutPanel rootLayout;

        // =========================
        // MENU
        // =========================
        private MenuStrip menuMain;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuFileNewSession;
        private ToolStripMenuItem menuFileTestMode;
        private ToolStripMenuItem menuSettings;
        private ToolStripMenuItem menuSettingsOptions;
        private ToolStripMenuItem menuSettingsCommunication;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuViewLightTheme;
        private ToolStripMenuItem menuViewDarkTheme;
        private ToolStripMenuItem menuHelp;
        private ToolStripLabel toolStripVersion;

        // =========================
        // STATUS BAR
        // =========================
        private Panel panelStatusBar;
        private TableLayoutPanel statusLayout;
        private FlowLayoutPanel statusConnPanel;
        private Label lblStatusDot;
        private Label lblConnectionStatus;
        private Label lblBaudRate;
        private Label lblLastUpdate;
        private PictureBox picLogo;

        // =========================
        // TOOLBAR
        // =========================
        private Panel panelToolbar;
        private FlowLayoutPanel toolbarFlow;
        private Label lblToolbarControl;
        private Button btnPause;
        private Button btnPurge;
        private Label lblToolbarWindows;
        private Button btnGraph;
        private Button btnTable;
        private Button btnTerminal;

        // =========================
        // CONTENT
        // =========================
        private TableLayoutPanel layoutContent;
        private Panel panelLeft;
        private Panel panelRight;

        // LEFT SIDE LAYOUT
        private TableLayoutPanel leftLayout;

        // Pressure cards row
        private TableLayoutPanel layoutPressureCards;
        private Panel cardCurrent;
        private Label lblCurrentTitle;
        private Label lblCurrentValue;
        private Label lblCurrentUnit;
        private Label lblCurrentRate;

        private Panel cardTarget;
        private Label lblTargetTitle;
        private Label lblTargetValue;
        private Label lblTargetUnit;
        private Label lblTargetStatus;

        // Set Target section
        private Panel sectionSetTarget;
        private Label lblSetTargetTitle;
        private TableLayoutPanel setTargetRow;
        private TextBox txtTargetInput;
        private Label lblTargetInputUnit;
        private Button btnGoToTarget;

        // Pressure Control section
        private Panel sectionPressureControl;
        private Label lblPressureControlTitle;
        private TableLayoutPanel incrementRow;
        private Label lblIncrementLabel;
        private Button btnIncrementMinus;
        private TextBox txtIncrement;
        private Button btnIncrementPlus;
        private Label lblIncrementUnit;

        private Label lblAdjustPressureLabel;
        private Button btnIncrease;
        private Button btnDecrease;

        // RIGHT SIDE LAYOUT
        private TableLayoutPanel rightLayout;

        // System Settings panel
        private Panel panelSystemSettings;
        private Label lblSystemSettingsTitle;
        private TableLayoutPanel systemSettingsStack;

        private Panel settingRampSpeed;
        private TableLayoutPanel settingRampSpeedRow;
        private Panel stripRampSpeed;
        private Label lblRampSpeedLabel;
        private Label lblRampSpeedValue;

        private Panel settingMaxPressure;
        private TableLayoutPanel settingMaxPressureRow;
        private Panel stripMaxPressure;
        private Label lblMaxPressureLabel;
        private Label lblMaxPressureValue;

        private Panel settingUnits;
        private TableLayoutPanel settingUnitsRow;
        private Panel stripUnits;
        private Label lblUnitsLabel;
        private Label lblUnitsValue;

        private Panel settingConnection;
        private TableLayoutPanel settingConnectionRow;
        private Panel stripConnection;
        private Label lblConnectionLabel;
        private Label lblConnectionValue;

        private Panel settingBaudRate;
        private TableLayoutPanel settingBaudRateRow;
        private Panel stripBaudRate;
        private Label lblBaudRateLabel;
        private Label lblBaudRateValue;

        // Status Info panel
        private Panel panelStatusInfo;
        private Label lblStatusInfoTitle;
        private Label lblStatusInfoText;

        // Spacers
        private Panel spacerContentCol;
        private Panel spacerLeftGap1;
        private Panel spacerLeftGap2;
        private Panel spacerLeftGap3;
        private Panel spacerRightGap;

        // =========================
        // THEME FIELDS
        // =========================
        private bool isDarkTheme = false;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            menuMain = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuFileNewSession = new ToolStripMenuItem();
            menuFileTestMode = new ToolStripMenuItem();
            menuSettings = new ToolStripMenuItem();
            menuSettingsOptions = new ToolStripMenuItem();
            menuSettingsCommunication = new ToolStripMenuItem();
            menuView = new ToolStripMenuItem();
            menuViewLightTheme = new ToolStripMenuItem();
            menuViewDarkTheme = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            toolStripVersion = new ToolStripLabel();
            rootLayout = new TableLayoutPanel();
            panelStatusBar = new Panel();
            statusLayout = new TableLayoutPanel();
            statusConnPanel = new FlowLayoutPanel();
            lblStatusDot = new Label();
            lblConnectionStatus = new Label();
            lblBaudRate = new Label();
            lblLastUpdate = new Label();
            picLogo = new PictureBox();
            panelToolbar = new Panel();
            toolbarFlow = new FlowLayoutPanel();
            lblToolbarControl = new Label();
            btnPause = new Button();
            btnPurge = new Button();
            lblToolbarWindows = new Label();
            btnGraph = new Button();
            btnTable = new Button();
            btnTerminal = new Button();
            layoutContent = new TableLayoutPanel();
            panelLeft = new Panel();
            leftLayout = new TableLayoutPanel();
            layoutPressureCards = new TableLayoutPanel();
            cardCurrent = new Panel();
            lblCurrentRate = new Label();
            lblCurrentUnit = new Label();
            lblCurrentValue = new Label();
            lblCurrentTitle = new Label();
            spacerLeftGap1 = new Panel();
            cardTarget = new Panel();
            lblTargetStatus = new Label();
            lblTargetUnit = new Label();
            lblTargetValue = new Label();
            lblTargetTitle = new Label();
            spacerLeftGap2 = new Panel();
            sectionSetTarget = new Panel();
            setTargetRow = new TableLayoutPanel();
            txtTargetInput = new TextBox();
            lblTargetInputUnit = new Label();
            btnGoToTarget = new Button();
            lblSetTargetTitle = new Label();
            spacerLeftGap3 = new Panel();
            sectionPressureControl = new Panel();
            btnDecrease = new Button();
            btnIncrease = new Button();
            lblAdjustPressureLabel = new Label();
            incrementRow = new TableLayoutPanel();
            btnIncrementMinus = new Button();
            txtIncrement = new TextBox();
            btnIncrementPlus = new Button();
            lblIncrementUnit = new Label();
            lblIncrementLabel = new Label();
            lblPressureControlTitle = new Label();
            spacerContentCol = new Panel();
            panelRight = new Panel();
            rightLayout = new TableLayoutPanel();
            panelSystemSettings = new Panel();
            systemSettingsStack = new TableLayoutPanel();
            settingRampSpeed = new Panel();
            settingRampSpeedRow = new TableLayoutPanel();
            stripRampSpeed = new Panel();
            lblRampSpeedLabel = new Label();
            lblRampSpeedValue = new Label();
            settingMaxPressure = new Panel();
            settingMaxPressureRow = new TableLayoutPanel();
            stripMaxPressure = new Panel();
            lblMaxPressureLabel = new Label();
            lblMaxPressureValue = new Label();
            settingUnits = new Panel();
            settingUnitsRow = new TableLayoutPanel();
            stripUnits = new Panel();
            lblUnitsLabel = new Label();
            lblUnitsValue = new Label();
            settingConnection = new Panel();
            settingConnectionRow = new TableLayoutPanel();
            stripConnection = new Panel();
            lblConnectionLabel = new Label();
            lblConnectionValue = new Label();
            settingBaudRate = new Panel();
            settingBaudRateRow = new TableLayoutPanel();
            stripBaudRate = new Panel();
            lblBaudRateLabel = new Label();
            lblBaudRateValue = new Label();
            lblSystemSettingsTitle = new Label();
            spacerRightGap = new Panel();
            panelStatusInfo = new Panel();
            lblStatusInfoText = new Label();
            lblStatusInfoTitle = new Label();
            menuMain.SuspendLayout();
            rootLayout.SuspendLayout();
            panelStatusBar.SuspendLayout();
            statusLayout.SuspendLayout();
            statusConnPanel.SuspendLayout();
            ((ISupportInitialize)picLogo).BeginInit();
            panelToolbar.SuspendLayout();
            toolbarFlow.SuspendLayout();
            layoutContent.SuspendLayout();
            panelLeft.SuspendLayout();
            leftLayout.SuspendLayout();
            layoutPressureCards.SuspendLayout();
            cardCurrent.SuspendLayout();
            cardTarget.SuspendLayout();
            sectionSetTarget.SuspendLayout();
            setTargetRow.SuspendLayout();
            sectionPressureControl.SuspendLayout();
            incrementRow.SuspendLayout();
            panelRight.SuspendLayout();
            rightLayout.SuspendLayout();
            panelSystemSettings.SuspendLayout();
            systemSettingsStack.SuspendLayout();
            settingRampSpeed.SuspendLayout();
            settingRampSpeedRow.SuspendLayout();
            settingMaxPressure.SuspendLayout();
            settingMaxPressureRow.SuspendLayout();
            settingUnits.SuspendLayout();
            settingUnitsRow.SuspendLayout();
            settingConnection.SuspendLayout();
            settingConnectionRow.SuspendLayout();
            settingBaudRate.SuspendLayout();
            settingBaudRateRow.SuspendLayout();
            panelStatusInfo.SuspendLayout();
            SuspendLayout();
            // 
            // menuMain
            // 
            menuMain.AutoSize = false;
            menuMain.BackColor = Color.FromArgb(245, 245, 245);
            menuMain.Items.AddRange(new ToolStripItem[] { menuFile, menuSettings, menuView, menuHelp, toolStripVersion });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Padding = new Padding(8, 6, 8, 6);
            menuMain.Size = new Size(1284, 35);
            menuMain.TabIndex = 0;
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuFileNewSession, menuFileTestMode });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(37, 23);
            menuFile.Text = "File";
            // 
            // menuFileNewSession
            // 
            menuFileNewSession.Name = "menuFileNewSession";
            menuFileNewSession.Size = new Size(156, 22);
            menuFileNewSession.Text = "New Session...";
            menuFileNewSession.Click += menuFileNewSession_Click;
            // 
            // menuFileTestMode
            // 
            menuFileTestMode.Name = "menuFileTestMode";
            menuFileTestMode.Size = new Size(156, 22);
            menuFileTestMode.Text = "Start Test Mode";
            // 
            // menuSettings
            // 
            menuSettings.DropDownItems.AddRange(new ToolStripItem[] { menuSettingsOptions, menuSettingsCommunication });
            menuSettings.Name = "menuSettings";
            menuSettings.Size = new Size(61, 23);
            menuSettings.Text = "Settings";
            // 
            // menuSettingsOptions
            // 
            menuSettingsOptions.Name = "menuSettingsOptions";
            menuSettingsOptions.Size = new Size(161, 22);
            menuSettingsOptions.Text = "Options";
            menuSettingsOptions.Click += btnOptions_Click;
            // 
            // menuSettingsCommunication
            // 
            menuSettingsCommunication.Name = "menuSettingsCommunication";
            menuSettingsCommunication.Size = new Size(161, 22);
            menuSettingsCommunication.Text = "Communication";
            menuSettingsCommunication.Click += btnCommunication_Click;
            // 
            // menuView
            // 
            menuView.DropDownItems.AddRange(new ToolStripItem[] { menuViewLightTheme, menuViewDarkTheme });
            menuView.Name = "menuView";
            menuView.Size = new Size(44, 23);
            menuView.Text = "View";
            // 
            // menuViewLightTheme
            // 
            menuViewLightTheme.Checked = true;
            menuViewLightTheme.CheckState = CheckState.Checked;
            menuViewLightTheme.Name = "menuViewLightTheme";
            menuViewLightTheme.Size = new Size(141, 22);
            menuViewLightTheme.Text = "Light Theme";
            menuViewLightTheme.Click += MenuViewLightTheme_Click;
            // 
            // menuViewDarkTheme
            // 
            menuViewDarkTheme.Name = "menuViewDarkTheme";
            menuViewDarkTheme.Size = new Size(141, 22);
            menuViewDarkTheme.Text = "Dark Theme";
            menuViewDarkTheme.Click += MenuViewDarkTheme_Click;
            // 
            // menuHelp
            // 
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(44, 23);
            menuHelp.Text = "Help";
            // 
            // toolStripVersion
            // 
            toolStripVersion.Alignment = ToolStripItemAlignment.Right;
            toolStripVersion.ForeColor = Color.Gray;
            toolStripVersion.Name = "toolStripVersion";
            toolStripVersion.Size = new Size(72, 20);
            toolStripVersion.Text = "version 1.2.0";
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(panelStatusBar, 0, 0);
            rootLayout.Controls.Add(panelToolbar, 0, 1);
            rootLayout.Controls.Add(layoutContent, 0, 2);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 35);
            rootLayout.Margin = new Padding(0);
            rootLayout.Name = "rootLayout";
            rootLayout.RowCount = 3;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(1284, 746);
            rootLayout.TabIndex = 0;
            // 
            // panelStatusBar
            // 
            panelStatusBar.BackColor = Color.FromArgb(232, 244, 248);
            panelStatusBar.Controls.Add(statusLayout);
            panelStatusBar.Dock = DockStyle.Fill;
            panelStatusBar.Location = new Point(0, 0);
            panelStatusBar.Margin = new Padding(0);
            panelStatusBar.Name = "panelStatusBar";
            panelStatusBar.Padding = new Padding(16, 0, 16, 0);
            panelStatusBar.Size = new Size(1284, 50);
            panelStatusBar.TabIndex = 0;
            // 
            // statusLayout
            // 
            statusLayout.ColumnCount = 7;
            statusLayout.ColumnStyles.Add(new ColumnStyle());
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            statusLayout.ColumnStyles.Add(new ColumnStyle());
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            statusLayout.ColumnStyles.Add(new ColumnStyle());
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            statusLayout.ColumnStyles.Add(new ColumnStyle());
            statusLayout.Controls.Add(statusConnPanel, 0, 0);
            statusLayout.Controls.Add(lblBaudRate, 2, 0);
            statusLayout.Controls.Add(lblLastUpdate, 4, 0);
            statusLayout.Controls.Add(picLogo, 6, 0);
            statusLayout.Dock = DockStyle.Fill;
            statusLayout.Location = new Point(16, 0);
            statusLayout.Margin = new Padding(0);
            statusLayout.Name = "statusLayout";
            statusLayout.RowCount = 1;
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statusLayout.Size = new Size(1252, 50);
            statusLayout.TabIndex = 0;
            // 
            // statusConnPanel
            // 
            statusConnPanel.AutoSize = true;
            statusConnPanel.Controls.Add(lblStatusDot);
            statusConnPanel.Controls.Add(lblConnectionStatus);
            statusConnPanel.Location = new Point(0, 0);
            statusConnPanel.Margin = new Padding(0);
            statusConnPanel.Name = "statusConnPanel";
            statusConnPanel.Size = new Size(127, 45);
            statusConnPanel.TabIndex = 0;
            statusConnPanel.WrapContents = false;
            // 
            // lblStatusDot
            // 
            lblStatusDot.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblStatusDot.Location = new Point(0, 5);
            lblStatusDot.Margin = new Padding(0, 5, 8, 0);
            lblStatusDot.Name = "lblStatusDot";
            lblStatusDot.Size = new Size(40, 40);
            lblStatusDot.TabIndex = 0;
            lblStatusDot.Text = "●";
            lblStatusDot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Font = new Font("Segoe UI", 9F);
            lblConnectionStatus.Location = new Point(48, 20);
            lblConnectionStatus.Margin = new Padding(0, 20, 0, 0);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(79, 15);
            lblConnectionStatus.TabIndex = 1;
            lblConnectionStatus.Text = "Disconnected";
            // 
            // lblBaudRate
            // 
            lblBaudRate.AutoSize = true;
            lblBaudRate.Font = new Font("Segoe UI", 9F);
            lblBaudRate.Location = new Point(147, 16);
            lblBaudRate.Margin = new Padding(0, 16, 0, 0);
            lblBaudRate.Name = "lblBaudRate";
            lblBaudRate.Size = new Size(70, 15);
            lblBaudRate.TabIndex = 2;
            lblBaudRate.Text = "Baud: 19200";
            // 
            // lblLastUpdate
            // 
            lblLastUpdate.AutoSize = true;
            lblLastUpdate.Font = new Font("Segoe UI", 9F);
            lblLastUpdate.Location = new Point(237, 16);
            lblLastUpdate.Margin = new Padding(0, 16, 0, 0);
            lblLastUpdate.Name = "lblLastUpdate";
            lblLastUpdate.Size = new Size(117, 15);
            lblLastUpdate.TabIndex = 4;
            lblLastUpdate.Text = "Last update: 0.5s ago";
            // 
            // picLogo
            // 
            picLogo.Anchor = AnchorStyles.Right;
            picLogo.Location = new Point(1072, 3);
            picLogo.Margin = new Padding(0, 2, 0, 0);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(180, 45);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 6;
            picLogo.TabStop = false;
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.White;
            panelToolbar.Controls.Add(toolbarFlow);
            panelToolbar.Dock = DockStyle.Fill;
            panelToolbar.Location = new Point(0, 50);
            panelToolbar.Margin = new Padding(0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Padding = new Padding(16, 0, 16, 0);
            panelToolbar.Size = new Size(1284, 45);
            panelToolbar.TabIndex = 1;
            // 
            // toolbarFlow
            // 
            toolbarFlow.Controls.Add(lblToolbarControl);
            toolbarFlow.Controls.Add(btnPause);
            toolbarFlow.Controls.Add(btnPurge);
            toolbarFlow.Controls.Add(lblToolbarWindows);
            toolbarFlow.Controls.Add(btnGraph);
            toolbarFlow.Controls.Add(btnTable);
            toolbarFlow.Controls.Add(btnTerminal);
            toolbarFlow.Dock = DockStyle.Fill;
            toolbarFlow.Location = new Point(16, 0);
            toolbarFlow.Margin = new Padding(0);
            toolbarFlow.Name = "toolbarFlow";
            toolbarFlow.Size = new Size(1252, 45);
            toolbarFlow.TabIndex = 0;
            toolbarFlow.WrapContents = false;
            // 
            // lblToolbarControl
            // 
            lblToolbarControl.AutoSize = true;
            lblToolbarControl.Font = new Font("Segoe UI", 9F);
            lblToolbarControl.Location = new Point(0, 13);
            lblToolbarControl.Margin = new Padding(0, 13, 8, 0);
            lblToolbarControl.Name = "lblToolbarControl";
            lblToolbarControl.Size = new Size(50, 15);
            lblToolbarControl.TabIndex = 0;
            lblToolbarControl.Text = "Control:";
            // 
            // btnPause
            // 
            btnPause.FlatStyle = FlatStyle.Flat;
            btnPause.Font = new Font("Segoe UI", 9F);
            btnPause.Location = new Point(58, 0);
            btnPause.Margin = new Padding(0, 0, 8, 0);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(100, 45);
            btnPause.TabIndex = 1;
            btnPause.Text = "Pause";
            btnPause.Click += btnPause_Click;
            // 
            // btnPurge
            // 
            btnPurge.FlatStyle = FlatStyle.Flat;
            btnPurge.Font = new Font("Segoe UI", 9F);
            btnPurge.Location = new Point(166, 0);
            btnPurge.Margin = new Padding(0, 0, 8, 0);
            btnPurge.Name = "btnPurge";
            btnPurge.Size = new Size(100, 45);
            btnPurge.TabIndex = 2;
            btnPurge.Text = "Purge";
            btnPurge.Click += btnPurge_Click;
            // 
            // lblToolbarWindows
            // 
            lblToolbarWindows.AutoSize = true;
            lblToolbarWindows.Font = new Font("Segoe UI", 9F);
            lblToolbarWindows.Location = new Point(166, 13);
            lblToolbarWindows.Margin = new Padding(0, 13, 10, 0);
            lblToolbarWindows.Name = "lblToolbarWindows";
            lblToolbarWindows.Size = new Size(59, 15);
            lblToolbarWindows.TabIndex = 4;
            lblToolbarWindows.Text = "Windows:";
            // 
            // btnGraph
            // 
            btnGraph.FlatStyle = FlatStyle.Flat;
            btnGraph.Font = new Font("Segoe UI", 9F);
            btnGraph.Location = new Point(235, 0);
            btnGraph.Margin = new Padding(0, 0, 6, 0);
            btnGraph.Name = "btnGraph";
            btnGraph.Size = new Size(80, 45);
            btnGraph.TabIndex = 5;
            btnGraph.Text = "Graph";
            btnGraph.Click += btnGraph_Click;
            // 
            // btnTable
            // 
            btnTable.FlatStyle = FlatStyle.Flat;
            btnTable.Font = new Font("Segoe UI", 9F);
            btnTable.Location = new Point(321, 0);
            btnTable.Margin = new Padding(0, 0, 6, 0);
            btnTable.Name = "btnTable";
            btnTable.Size = new Size(80, 45);
            btnTable.TabIndex = 6;
            btnTable.Text = "Table";
            btnTable.Click += btnTable_Click;
            // 
            // btnTerminal
            // 
            btnTerminal.FlatStyle = FlatStyle.Flat;
            btnTerminal.Font = new Font("Segoe UI", 9F);
            btnTerminal.Location = new Point(407, 0);
            btnTerminal.Margin = new Padding(0);
            btnTerminal.Name = "btnTerminal";
            btnTerminal.Size = new Size(80, 45);
            btnTerminal.TabIndex = 7;
            btnTerminal.Text = "Terminal";
            btnTerminal.Click += btnTerminal_Click;
            // 
            // layoutContent
            // 
            layoutContent.BackColor = Color.White;
            layoutContent.ColumnCount = 3;
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 840F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            layoutContent.Controls.Add(panelLeft, 0, 0);
            layoutContent.Controls.Add(spacerContentCol, 1, 0);
            layoutContent.Controls.Add(panelRight, 2, 0);
            layoutContent.Dock = DockStyle.Fill;
            layoutContent.Location = new Point(0, 95);
            layoutContent.Margin = new Padding(0);
            layoutContent.Name = "layoutContent";
            layoutContent.Padding = new Padding(20);
            layoutContent.RowCount = 1;
            layoutContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutContent.Size = new Size(1284, 651);
            layoutContent.TabIndex = 2;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.White;
            panelLeft.Controls.Add(leftLayout);
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Location = new Point(20, 20);
            panelLeft.Margin = new Padding(0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(840, 611);
            panelLeft.TabIndex = 0;
            // 
            // leftLayout
            // 
            leftLayout.ColumnCount = 1;
            leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            leftLayout.Controls.Add(layoutPressureCards, 0, 0);
            leftLayout.Controls.Add(spacerLeftGap2, 0, 1);
            leftLayout.Controls.Add(sectionSetTarget, 0, 2);
            leftLayout.Controls.Add(spacerLeftGap3, 0, 3);
            leftLayout.Controls.Add(sectionPressureControl, 0, 4);
            leftLayout.Dock = DockStyle.Fill;
            leftLayout.Location = new Point(0, 0);
            leftLayout.Margin = new Padding(0);
            leftLayout.Name = "leftLayout";
            leftLayout.RowCount = 5;
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 240F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftLayout.Size = new Size(840, 611);
            leftLayout.TabIndex = 0;
            // 
            // layoutPressureCards
            // 
            layoutPressureCards.ColumnCount = 3;
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410F));
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410F));
            layoutPressureCards.Controls.Add(cardCurrent, 0, 0);
            layoutPressureCards.Controls.Add(spacerLeftGap1, 1, 0);
            layoutPressureCards.Controls.Add(cardTarget, 2, 0);
            layoutPressureCards.Dock = DockStyle.Fill;
            layoutPressureCards.Location = new Point(0, 0);
            layoutPressureCards.Margin = new Padding(0);
            layoutPressureCards.Name = "layoutPressureCards";
            layoutPressureCards.RowCount = 1;
            layoutPressureCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layoutPressureCards.Size = new Size(840, 240);
            layoutPressureCards.TabIndex = 0;
            // 
            // cardCurrent
            // 
            cardCurrent.BackColor = Color.FromArgb(250, 250, 250);
            cardCurrent.BorderStyle = BorderStyle.FixedSingle;
            cardCurrent.Controls.Add(lblCurrentRate);
            cardCurrent.Controls.Add(lblCurrentUnit);
            cardCurrent.Controls.Add(lblCurrentValue);
            cardCurrent.Controls.Add(lblCurrentTitle);
            cardCurrent.Dock = DockStyle.Fill;
            cardCurrent.Location = new Point(0, 0);
            cardCurrent.Margin = new Padding(0);
            cardCurrent.Name = "cardCurrent";
            cardCurrent.Padding = new Padding(0, 0, 0, 20);
            cardCurrent.Size = new Size(410, 240);
            cardCurrent.TabIndex = 0;
            // 
            // lblCurrentRate
            // 
            lblCurrentRate.Dock = DockStyle.Top;
            lblCurrentRate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCurrentRate.Location = new Point(0, 155);
            lblCurrentRate.Name = "lblCurrentRate";
            lblCurrentRate.Size = new Size(408, 25);
            lblCurrentRate.TabIndex = 0;
            lblCurrentRate.Text = "→ 0.0 /s";
            lblCurrentRate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrentUnit
            // 
            lblCurrentUnit.Dock = DockStyle.Top;
            lblCurrentUnit.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCurrentUnit.Location = new Point(0, 120);
            lblCurrentUnit.Name = "lblCurrentUnit";
            lblCurrentUnit.Size = new Size(408, 35);
            lblCurrentUnit.TabIndex = 1;
            lblCurrentUnit.Text = "PSIG";
            lblCurrentUnit.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrentValue
            // 
            lblCurrentValue.Dock = DockStyle.Top;
            lblCurrentValue.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
            lblCurrentValue.Location = new Point(0, 30);
            lblCurrentValue.Name = "lblCurrentValue";
            lblCurrentValue.Size = new Size(408, 90);
            lblCurrentValue.TabIndex = 2;
            lblCurrentValue.Text = "0.0";
            lblCurrentValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrentTitle
            // 
            lblCurrentTitle.Dock = DockStyle.Top;
            lblCurrentTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCurrentTitle.Location = new Point(0, 0);
            lblCurrentTitle.Name = "lblCurrentTitle";
            lblCurrentTitle.Size = new Size(408, 30);
            lblCurrentTitle.TabIndex = 3;
            lblCurrentTitle.Text = "CURRENT PRESSURE";
            lblCurrentTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // spacerLeftGap1
            // 
            spacerLeftGap1.Dock = DockStyle.Fill;
            spacerLeftGap1.Location = new Point(413, 3);
            spacerLeftGap1.Name = "spacerLeftGap1";
            spacerLeftGap1.Size = new Size(14, 234);
            spacerLeftGap1.TabIndex = 1;
            // 
            // cardTarget
            // 
            cardTarget.BackColor = Color.FromArgb(250, 250, 250);
            cardTarget.BorderStyle = BorderStyle.FixedSingle;
            cardTarget.Controls.Add(lblTargetStatus);
            cardTarget.Controls.Add(lblTargetUnit);
            cardTarget.Controls.Add(lblTargetValue);
            cardTarget.Controls.Add(lblTargetTitle);
            cardTarget.Dock = DockStyle.Fill;
            cardTarget.Location = new Point(430, 0);
            cardTarget.Margin = new Padding(0);
            cardTarget.Name = "cardTarget";
            cardTarget.Padding = new Padding(0, 0, 0, 20);
            cardTarget.Size = new Size(410, 240);
            cardTarget.TabIndex = 2;
            // 
            // lblTargetStatus
            // 
            lblTargetStatus.Dock = DockStyle.Top;
            lblTargetStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTargetStatus.Location = new Point(0, 155);
            lblTargetStatus.Name = "lblTargetStatus";
            lblTargetStatus.Size = new Size(408, 25);
            lblTargetStatus.TabIndex = 0;
            lblTargetStatus.Text = "At target";
            lblTargetStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTargetUnit
            // 
            lblTargetUnit.Dock = DockStyle.Top;
            lblTargetUnit.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTargetUnit.Location = new Point(0, 120);
            lblTargetUnit.Name = "lblTargetUnit";
            lblTargetUnit.Size = new Size(408, 35);
            lblTargetUnit.TabIndex = 1;
            lblTargetUnit.Text = "PSIG";
            lblTargetUnit.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTargetValue
            // 
            lblTargetValue.Dock = DockStyle.Top;
            lblTargetValue.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
            lblTargetValue.Location = new Point(0, 30);
            lblTargetValue.Name = "lblTargetValue";
            lblTargetValue.Size = new Size(408, 90);
            lblTargetValue.TabIndex = 2;
            lblTargetValue.Text = "0.0";
            lblTargetValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTargetTitle
            // 
            lblTargetTitle.Dock = DockStyle.Top;
            lblTargetTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTargetTitle.Location = new Point(0, 0);
            lblTargetTitle.Name = "lblTargetTitle";
            lblTargetTitle.Size = new Size(408, 30);
            lblTargetTitle.TabIndex = 3;
            lblTargetTitle.Text = "TARGET PRESSURE";
            lblTargetTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // spacerLeftGap2
            // 
            spacerLeftGap2.Dock = DockStyle.Fill;
            spacerLeftGap2.Location = new Point(3, 243);
            spacerLeftGap2.Name = "spacerLeftGap2";
            spacerLeftGap2.Size = new Size(834, 14);
            spacerLeftGap2.TabIndex = 1;
            // 
            // sectionSetTarget
            // 
            sectionSetTarget.BackColor = Color.FromArgb(250, 250, 250);
            sectionSetTarget.BorderStyle = BorderStyle.FixedSingle;
            sectionSetTarget.Controls.Add(setTargetRow);
            sectionSetTarget.Controls.Add(lblSetTargetTitle);
            sectionSetTarget.Dock = DockStyle.Fill;
            sectionSetTarget.Location = new Point(0, 260);
            sectionSetTarget.Margin = new Padding(0);
            sectionSetTarget.Name = "sectionSetTarget";
            sectionSetTarget.Padding = new Padding(18, 0, 18, 30);
            sectionSetTarget.Size = new Size(840, 116);
            sectionSetTarget.TabIndex = 2;
            // 
            // setTargetRow
            // 
            setTargetRow.ColumnCount = 4;
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            setTargetRow.Controls.Add(txtTargetInput, 0, 0);
            setTargetRow.Controls.Add(lblTargetInputUnit, 1, 0);
            setTargetRow.Controls.Add(btnGoToTarget, 3, 0);
            setTargetRow.Dock = DockStyle.Top;
            setTargetRow.Location = new Point(18, 25);
            setTargetRow.Margin = new Padding(0);
            setTargetRow.Name = "setTargetRow";
            setTargetRow.RowCount = 1;
            setTargetRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            setTargetRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            setTargetRow.Size = new Size(802, 50);
            setTargetRow.TabIndex = 0;
            // 
            // txtTargetInput
            // 
            txtTargetInput.Dock = DockStyle.Fill;
            txtTargetInput.Font = new Font("Courier New", 12F);
            txtTargetInput.Location = new Point(3, 3);
            txtTargetInput.Name = "txtTargetInput";
            txtTargetInput.Size = new Size(541, 26);
            txtTargetInput.TabIndex = 0;
            // 
            // lblTargetInputUnit
            // 
            lblTargetInputUnit.Dock = DockStyle.Fill;
            lblTargetInputUnit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTargetInputUnit.Location = new Point(557, 0);
            lblTargetInputUnit.Margin = new Padding(10, 0, 0, 0);
            lblTargetInputUnit.Name = "lblTargetInputUnit";
            lblTargetInputUnit.Size = new Size(60, 50);
            lblTargetInputUnit.TabIndex = 1;
            lblTargetInputUnit.Text = "PSIG";
            lblTargetInputUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnGoToTarget
            // 
            btnGoToTarget.Dock = DockStyle.Fill;
            btnGoToTarget.FlatStyle = FlatStyle.Flat;
            btnGoToTarget.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGoToTarget.Location = new Point(635, 3);
            btnGoToTarget.Name = "btnGoToTarget";
            btnGoToTarget.Size = new Size(164, 44);
            btnGoToTarget.TabIndex = 3;
            btnGoToTarget.Text = "🎯  Go to Target";
            btnGoToTarget.Click += btnGoTarget_Click;
            // 
            // lblSetTargetTitle
            // 
            lblSetTargetTitle.Dock = DockStyle.Top;
            lblSetTargetTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSetTargetTitle.Location = new Point(18, 0);
            lblSetTargetTitle.Name = "lblSetTargetTitle";
            lblSetTargetTitle.Size = new Size(802, 25);
            lblSetTargetTitle.TabIndex = 1;
            lblSetTargetTitle.Text = "Set Target Pressure";
            lblSetTargetTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // spacerLeftGap3
            // 
            spacerLeftGap3.Dock = DockStyle.Fill;
            spacerLeftGap3.Location = new Point(3, 379);
            spacerLeftGap3.Name = "spacerLeftGap3";
            spacerLeftGap3.Size = new Size(834, 14);
            spacerLeftGap3.TabIndex = 3;
            // 
            // sectionPressureControl
            // 
            sectionPressureControl.BackColor = Color.FromArgb(250, 250, 250);
            sectionPressureControl.BorderStyle = BorderStyle.FixedSingle;
            sectionPressureControl.Controls.Add(btnDecrease);
            sectionPressureControl.Controls.Add(btnIncrease);
            sectionPressureControl.Controls.Add(lblAdjustPressureLabel);
            sectionPressureControl.Controls.Add(incrementRow);
            sectionPressureControl.Controls.Add(lblIncrementLabel);
            sectionPressureControl.Controls.Add(lblPressureControlTitle);
            sectionPressureControl.Dock = DockStyle.Fill;
            sectionPressureControl.Location = new Point(0, 396);
            sectionPressureControl.Margin = new Padding(0);
            sectionPressureControl.Name = "sectionPressureControl";
            sectionPressureControl.Padding = new Padding(18, 15, 18, 15);
            sectionPressureControl.Size = new Size(840, 215);
            sectionPressureControl.TabIndex = 4;
            // 
            // btnDecrease
            // 
            btnDecrease.Dock = DockStyle.Top;
            btnDecrease.FlatStyle = FlatStyle.Flat;
            btnDecrease.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDecrease.Location = new Point(18, 180);
            btnDecrease.Margin = new Padding(0, 0, 0, 15);
            btnDecrease.Name = "btnDecrease";
            btnDecrease.Size = new Size(802, 50);
            btnDecrease.TabIndex = 0;
            btnDecrease.Text = "▼  Decrease (-5.0 PSIG)";
            btnDecrease.Click += btnDecrease_Click;
            // 
            // btnIncrease
            // 
            btnIncrease.Dock = DockStyle.Top;
            btnIncrease.FlatStyle = FlatStyle.Flat;
            btnIncrease.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIncrease.Location = new Point(18, 130);
            btnIncrease.Margin = new Padding(0, 0, 0, 10);
            btnIncrease.Name = "btnIncrease";
            btnIncrease.Size = new Size(802, 50);
            btnIncrease.TabIndex = 1;
            btnIncrease.Text = "▲  Increase (+5.0 PSIG)";
            btnIncrease.Click += btnIncrease_Click;
            // 
            // lblAdjustPressureLabel
            // 
            lblAdjustPressureLabel.Dock = DockStyle.Top;
            lblAdjustPressureLabel.Font = new Font("Segoe UI", 10F);
            lblAdjustPressureLabel.Location = new Point(18, 105);
            lblAdjustPressureLabel.Margin = new Padding(0);
            lblAdjustPressureLabel.Name = "lblAdjustPressureLabel";
            lblAdjustPressureLabel.Size = new Size(802, 25);
            lblAdjustPressureLabel.TabIndex = 2;
            lblAdjustPressureLabel.Text = "Adjust Pressure:";
            lblAdjustPressureLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // incrementRow
            // 
            incrementRow.ColumnCount = 5;
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            incrementRow.Controls.Add(btnIncrementMinus, 0, 0);
            incrementRow.Controls.Add(txtIncrement, 1, 0);
            incrementRow.Controls.Add(btnIncrementPlus, 2, 0);
            incrementRow.Controls.Add(lblIncrementUnit, 3, 0);
            incrementRow.Dock = DockStyle.Top;
            incrementRow.Location = new Point(18, 65);
            incrementRow.Margin = new Padding(0);
            incrementRow.Name = "incrementRow";
            incrementRow.RowCount = 1;
            incrementRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            incrementRow.Size = new Size(802, 40);
            incrementRow.TabIndex = 3;
            // 
            // btnIncrementMinus
            // 
            btnIncrementMinus.Dock = DockStyle.Fill;
            btnIncrementMinus.FlatStyle = FlatStyle.Flat;
            btnIncrementMinus.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnIncrementMinus.Location = new Point(3, 3);
            btnIncrementMinus.Name = "btnIncrementMinus";
            btnIncrementMinus.Size = new Size(40, 34);
            btnIncrementMinus.TabIndex = 0;
            btnIncrementMinus.Text = "−";
            btnIncrementMinus.Click += btnIncrementMinus_Click;
            // 
            // txtIncrement
            // 
            txtIncrement.Dock = DockStyle.Fill;
            txtIncrement.Font = new Font("Courier New", 12F, FontStyle.Bold);
            txtIncrement.Location = new Point(49, 3);
            txtIncrement.Name = "txtIncrement";
            txtIncrement.Size = new Size(104, 26);
            txtIncrement.TabIndex = 1;
            txtIncrement.Text = "5.0";
            txtIncrement.TextAlign = HorizontalAlignment.Center;
            // 
            // btnIncrementPlus
            // 
            btnIncrementPlus.Dock = DockStyle.Fill;
            btnIncrementPlus.FlatStyle = FlatStyle.Flat;
            btnIncrementPlus.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnIncrementPlus.Location = new Point(159, 3);
            btnIncrementPlus.Name = "btnIncrementPlus";
            btnIncrementPlus.Size = new Size(40, 34);
            btnIncrementPlus.TabIndex = 2;
            btnIncrementPlus.Text = "+";
            btnIncrementPlus.Click += btnIncrementPlus_Click;
            // 
            // lblIncrementUnit
            // 
            lblIncrementUnit.Dock = DockStyle.Fill;
            lblIncrementUnit.Font = new Font("Segoe UI", 10F);
            lblIncrementUnit.Location = new Point(210, 0);
            lblIncrementUnit.Margin = new Padding(8, 0, 0, 0);
            lblIncrementUnit.Name = "lblIncrementUnit";
            lblIncrementUnit.Size = new Size(62, 40);
            lblIncrementUnit.TabIndex = 3;
            lblIncrementUnit.Text = "PSIG";
            lblIncrementUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblIncrementLabel
            // 
            lblIncrementLabel.Dock = DockStyle.Top;
            lblIncrementLabel.Font = new Font("Segoe UI", 10F);
            lblIncrementLabel.Location = new Point(18, 45);
            lblIncrementLabel.Margin = new Padding(0);
            lblIncrementLabel.Name = "lblIncrementLabel";
            lblIncrementLabel.Size = new Size(802, 20);
            lblIncrementLabel.TabIndex = 4;
            lblIncrementLabel.Text = "Increment:";
            lblIncrementLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPressureControlTitle
            // 
            lblPressureControlTitle.Dock = DockStyle.Top;
            lblPressureControlTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPressureControlTitle.Location = new Point(18, 15);
            lblPressureControlTitle.Name = "lblPressureControlTitle";
            lblPressureControlTitle.Size = new Size(802, 30);
            lblPressureControlTitle.TabIndex = 5;
            lblPressureControlTitle.Text = "Pressure Control";
            lblPressureControlTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // spacerContentCol
            // 
            spacerContentCol.Dock = DockStyle.Fill;
            spacerContentCol.Location = new Point(863, 23);
            spacerContentCol.Name = "spacerContentCol";
            spacerContentCol.Size = new Size(14, 605);
            spacerContentCol.TabIndex = 1;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.White;
            panelRight.Controls.Add(rightLayout);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(880, 20);
            panelRight.Margin = new Padding(0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(400, 611);
            panelRight.TabIndex = 2;
            // 
            // rightLayout
            // 
            rightLayout.ColumnCount = 1;
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            rightLayout.Controls.Add(panelSystemSettings, 0, 0);
            rightLayout.Controls.Add(spacerRightGap, 0, 1);
            rightLayout.Controls.Add(panelStatusInfo, 0, 2);
            rightLayout.Dock = DockStyle.Fill;
            rightLayout.Location = new Point(0, 0);
            rightLayout.Margin = new Padding(0);
            rightLayout.Name = "rightLayout";
            rightLayout.RowCount = 3;
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightLayout.Size = new Size(400, 611);
            rightLayout.TabIndex = 0;
            // 
            // panelSystemSettings
            // 
            panelSystemSettings.BackColor = Color.FromArgb(250, 250, 250);
            panelSystemSettings.BorderStyle = BorderStyle.FixedSingle;
            panelSystemSettings.Controls.Add(systemSettingsStack);
            panelSystemSettings.Controls.Add(lblSystemSettingsTitle);
            panelSystemSettings.Dock = DockStyle.Fill;
            panelSystemSettings.Location = new Point(0, 0);
            panelSystemSettings.Margin = new Padding(0);
            panelSystemSettings.Name = "panelSystemSettings";
            panelSystemSettings.Padding = new Padding(16);
            panelSystemSettings.Size = new Size(400, 300);
            panelSystemSettings.TabIndex = 0;
            // 
            // systemSettingsStack
            // 
            systemSettingsStack.ColumnCount = 1;
            systemSettingsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            systemSettingsStack.Controls.Add(settingRampSpeed, 0, 0);
            systemSettingsStack.Controls.Add(settingMaxPressure, 0, 1);
            systemSettingsStack.Controls.Add(settingUnits, 0, 2);
            systemSettingsStack.Controls.Add(settingConnection, 0, 3);
            systemSettingsStack.Controls.Add(settingBaudRate, 0, 4);
            systemSettingsStack.Dock = DockStyle.Fill;
            systemSettingsStack.Location = new Point(16, 44);
            systemSettingsStack.Margin = new Padding(0);
            systemSettingsStack.Name = "systemSettingsStack";
            systemSettingsStack.RowCount = 5;
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.Size = new Size(366, 238);
            systemSettingsStack.TabIndex = 0;
            // 
            // settingRampSpeed
            // 
            settingRampSpeed.BorderStyle = BorderStyle.FixedSingle;
            settingRampSpeed.Controls.Add(settingRampSpeedRow);
            settingRampSpeed.Dock = DockStyle.Fill;
            settingRampSpeed.Location = new Point(0, 0);
            settingRampSpeed.Margin = new Padding(0, 0, 0, 10);
            settingRampSpeed.Name = "settingRampSpeed";
            settingRampSpeed.Size = new Size(366, 38);
            settingRampSpeed.TabIndex = 0;
            // 
            // settingRampSpeedRow
            // 
            settingRampSpeedRow.ColumnCount = 3;
            settingRampSpeedRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingRampSpeedRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingRampSpeedRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingRampSpeedRow.Controls.Add(stripRampSpeed, 0, 0);
            settingRampSpeedRow.Controls.Add(lblRampSpeedLabel, 1, 0);
            settingRampSpeedRow.Controls.Add(lblRampSpeedValue, 2, 0);
            settingRampSpeedRow.Dock = DockStyle.Fill;
            settingRampSpeedRow.Location = new Point(0, 0);
            settingRampSpeedRow.Margin = new Padding(0);
            settingRampSpeedRow.Name = "settingRampSpeedRow";
            settingRampSpeedRow.RowCount = 1;
            settingRampSpeedRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            settingRampSpeedRow.Size = new Size(364, 36);
            settingRampSpeedRow.TabIndex = 0;
            // 
            // stripRampSpeed
            // 
            stripRampSpeed.Dock = DockStyle.Fill;
            stripRampSpeed.Location = new Point(0, 0);
            stripRampSpeed.Margin = new Padding(0);
            stripRampSpeed.Name = "stripRampSpeed";
            stripRampSpeed.Size = new Size(4, 36);
            stripRampSpeed.TabIndex = 0;
            // 
            // lblRampSpeedLabel
            // 
            lblRampSpeedLabel.Dock = DockStyle.Fill;
            lblRampSpeedLabel.Font = new Font("Segoe UI", 10F);
            lblRampSpeedLabel.Location = new Point(16, 0);
            lblRampSpeedLabel.Margin = new Padding(12, 0, 0, 0);
            lblRampSpeedLabel.Name = "lblRampSpeedLabel";
            lblRampSpeedLabel.Size = new Size(222, 36);
            lblRampSpeedLabel.TabIndex = 1;
            lblRampSpeedLabel.Text = "Ramp Speed";
            lblRampSpeedLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRampSpeedValue
            // 
            lblRampSpeedValue.Dock = DockStyle.Fill;
            lblRampSpeedValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblRampSpeedValue.Location = new Point(238, 0);
            lblRampSpeedValue.Margin = new Padding(0, 0, 12, 0);
            lblRampSpeedValue.Name = "lblRampSpeedValue";
            lblRampSpeedValue.Size = new Size(114, 36);
            lblRampSpeedValue.TabIndex = 2;
            lblRampSpeedValue.Text = "PSIG/s";
            lblRampSpeedValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // settingMaxPressure
            // 
            settingMaxPressure.BorderStyle = BorderStyle.FixedSingle;
            settingMaxPressure.Controls.Add(settingMaxPressureRow);
            settingMaxPressure.Dock = DockStyle.Fill;
            settingMaxPressure.Location = new Point(0, 48);
            settingMaxPressure.Margin = new Padding(0, 0, 0, 10);
            settingMaxPressure.Name = "settingMaxPressure";
            settingMaxPressure.Size = new Size(366, 38);
            settingMaxPressure.TabIndex = 1;
            // 
            // settingMaxPressureRow
            // 
            settingMaxPressureRow.ColumnCount = 3;
            settingMaxPressureRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingMaxPressureRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingMaxPressureRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingMaxPressureRow.Controls.Add(stripMaxPressure, 0, 0);
            settingMaxPressureRow.Controls.Add(lblMaxPressureLabel, 1, 0);
            settingMaxPressureRow.Controls.Add(lblMaxPressureValue, 2, 0);
            settingMaxPressureRow.Dock = DockStyle.Fill;
            settingMaxPressureRow.Location = new Point(0, 0);
            settingMaxPressureRow.Margin = new Padding(0);
            settingMaxPressureRow.Name = "settingMaxPressureRow";
            settingMaxPressureRow.RowCount = 1;
            settingMaxPressureRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            settingMaxPressureRow.Size = new Size(364, 36);
            settingMaxPressureRow.TabIndex = 0;
            // 
            // stripMaxPressure
            // 
            stripMaxPressure.Dock = DockStyle.Fill;
            stripMaxPressure.Location = new Point(0, 0);
            stripMaxPressure.Margin = new Padding(0);
            stripMaxPressure.Name = "stripMaxPressure";
            stripMaxPressure.Size = new Size(4, 36);
            stripMaxPressure.TabIndex = 0;
            // 
            // lblMaxPressureLabel
            // 
            lblMaxPressureLabel.Dock = DockStyle.Fill;
            lblMaxPressureLabel.Font = new Font("Segoe UI", 10F);
            lblMaxPressureLabel.Location = new Point(16, 0);
            lblMaxPressureLabel.Margin = new Padding(12, 0, 0, 0);
            lblMaxPressureLabel.Name = "lblMaxPressureLabel";
            lblMaxPressureLabel.Size = new Size(222, 36);
            lblMaxPressureLabel.TabIndex = 1;
            lblMaxPressureLabel.Text = "Max Pressure";
            lblMaxPressureLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMaxPressureValue
            // 
            lblMaxPressureValue.Dock = DockStyle.Fill;
            lblMaxPressureValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblMaxPressureValue.Location = new Point(238, 0);
            lblMaxPressureValue.Margin = new Padding(0, 0, 12, 0);
            lblMaxPressureValue.Name = "lblMaxPressureValue";
            lblMaxPressureValue.Size = new Size(114, 36);
            lblMaxPressureValue.TabIndex = 2;
            lblMaxPressureValue.Text = "200 PSIG";
            lblMaxPressureValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // settingUnits
            // 
            settingUnits.BorderStyle = BorderStyle.FixedSingle;
            settingUnits.Controls.Add(settingUnitsRow);
            settingUnits.Dock = DockStyle.Fill;
            settingUnits.Location = new Point(0, 96);
            settingUnits.Margin = new Padding(0, 0, 0, 10);
            settingUnits.Name = "settingUnits";
            settingUnits.Size = new Size(366, 38);
            settingUnits.TabIndex = 2;
            // 
            // settingUnitsRow
            // 
            settingUnitsRow.ColumnCount = 3;
            settingUnitsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingUnitsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingUnitsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingUnitsRow.Controls.Add(stripUnits, 0, 0);
            settingUnitsRow.Controls.Add(lblUnitsLabel, 1, 0);
            settingUnitsRow.Controls.Add(lblUnitsValue, 2, 0);
            settingUnitsRow.Dock = DockStyle.Fill;
            settingUnitsRow.Location = new Point(0, 0);
            settingUnitsRow.Margin = new Padding(0);
            settingUnitsRow.Name = "settingUnitsRow";
            settingUnitsRow.RowCount = 1;
            settingUnitsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            settingUnitsRow.Size = new Size(364, 36);
            settingUnitsRow.TabIndex = 0;
            // 
            // stripUnits
            // 
            stripUnits.Dock = DockStyle.Fill;
            stripUnits.Location = new Point(0, 0);
            stripUnits.Margin = new Padding(0);
            stripUnits.Name = "stripUnits";
            stripUnits.Size = new Size(4, 36);
            stripUnits.TabIndex = 0;
            // 
            // lblUnitsLabel
            // 
            lblUnitsLabel.Dock = DockStyle.Fill;
            lblUnitsLabel.Font = new Font("Segoe UI", 10F);
            lblUnitsLabel.Location = new Point(16, 0);
            lblUnitsLabel.Margin = new Padding(12, 0, 0, 0);
            lblUnitsLabel.Name = "lblUnitsLabel";
            lblUnitsLabel.Size = new Size(222, 36);
            lblUnitsLabel.TabIndex = 1;
            lblUnitsLabel.Text = "Units";
            lblUnitsLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUnitsValue
            // 
            lblUnitsValue.Dock = DockStyle.Fill;
            lblUnitsValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblUnitsValue.Location = new Point(238, 0);
            lblUnitsValue.Margin = new Padding(0, 0, 12, 0);
            lblUnitsValue.Name = "lblUnitsValue";
            lblUnitsValue.Size = new Size(114, 36);
            lblUnitsValue.TabIndex = 2;
            lblUnitsValue.Text = "PSIG";
            lblUnitsValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // settingConnection
            // 
            settingConnection.BorderStyle = BorderStyle.FixedSingle;
            settingConnection.Controls.Add(settingConnectionRow);
            settingConnection.Dock = DockStyle.Fill;
            settingConnection.Location = new Point(0, 144);
            settingConnection.Margin = new Padding(0, 0, 0, 10);
            settingConnection.Name = "settingConnection";
            settingConnection.Size = new Size(366, 38);
            settingConnection.TabIndex = 3;
            // 
            // settingConnectionRow
            // 
            settingConnectionRow.ColumnCount = 3;
            settingConnectionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingConnectionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingConnectionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingConnectionRow.Controls.Add(stripConnection, 0, 0);
            settingConnectionRow.Controls.Add(lblConnectionLabel, 1, 0);
            settingConnectionRow.Controls.Add(lblConnectionValue, 2, 0);
            settingConnectionRow.Dock = DockStyle.Fill;
            settingConnectionRow.Location = new Point(0, 0);
            settingConnectionRow.Margin = new Padding(0);
            settingConnectionRow.Name = "settingConnectionRow";
            settingConnectionRow.RowCount = 1;
            settingConnectionRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            settingConnectionRow.Size = new Size(364, 36);
            settingConnectionRow.TabIndex = 0;
            // 
            // stripConnection
            // 
            stripConnection.Dock = DockStyle.Fill;
            stripConnection.Location = new Point(0, 0);
            stripConnection.Margin = new Padding(0);
            stripConnection.Name = "stripConnection";
            stripConnection.Size = new Size(4, 36);
            stripConnection.TabIndex = 0;
            // 
            // lblConnectionLabel
            // 
            lblConnectionLabel.Dock = DockStyle.Fill;
            lblConnectionLabel.Font = new Font("Segoe UI", 10F);
            lblConnectionLabel.Location = new Point(16, 0);
            lblConnectionLabel.Margin = new Padding(12, 0, 0, 0);
            lblConnectionLabel.Name = "lblConnectionLabel";
            lblConnectionLabel.Size = new Size(222, 36);
            lblConnectionLabel.TabIndex = 1;
            lblConnectionLabel.Text = "Connection";
            lblConnectionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConnectionValue
            // 
            lblConnectionValue.Dock = DockStyle.Fill;
            lblConnectionValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblConnectionValue.Location = new Point(238, 0);
            lblConnectionValue.Margin = new Padding(0, 0, 12, 0);
            lblConnectionValue.Name = "lblConnectionValue";
            lblConnectionValue.Size = new Size(114, 36);
            lblConnectionValue.TabIndex = 2;
            lblConnectionValue.Text = "COM3";
            lblConnectionValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // settingBaudRate
            // 
            settingBaudRate.BorderStyle = BorderStyle.FixedSingle;
            settingBaudRate.Controls.Add(settingBaudRateRow);
            settingBaudRate.Dock = DockStyle.Fill;
            settingBaudRate.Location = new Point(0, 192);
            settingBaudRate.Margin = new Padding(0);
            settingBaudRate.Name = "settingBaudRate";
            settingBaudRate.Size = new Size(366, 48);
            settingBaudRate.TabIndex = 4;
            // 
            // settingBaudRateRow
            // 
            settingBaudRateRow.ColumnCount = 3;
            settingBaudRateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingBaudRateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingBaudRateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingBaudRateRow.Controls.Add(stripBaudRate, 0, 0);
            settingBaudRateRow.Controls.Add(lblBaudRateLabel, 1, 0);
            settingBaudRateRow.Controls.Add(lblBaudRateValue, 2, 0);
            settingBaudRateRow.Dock = DockStyle.Fill;
            settingBaudRateRow.Location = new Point(0, 0);
            settingBaudRateRow.Margin = new Padding(0);
            settingBaudRateRow.Name = "settingBaudRateRow";
            settingBaudRateRow.RowCount = 1;
            settingBaudRateRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            settingBaudRateRow.Size = new Size(364, 46);
            settingBaudRateRow.TabIndex = 0;
            // 
            // stripBaudRate
            // 
            stripBaudRate.Dock = DockStyle.Fill;
            stripBaudRate.Location = new Point(0, 0);
            stripBaudRate.Margin = new Padding(0);
            stripBaudRate.Name = "stripBaudRate";
            stripBaudRate.Size = new Size(4, 46);
            stripBaudRate.TabIndex = 0;
            // 
            // lblBaudRateLabel
            // 
            lblBaudRateLabel.Dock = DockStyle.Fill;
            lblBaudRateLabel.Font = new Font("Segoe UI", 10F);
            lblBaudRateLabel.Location = new Point(16, 0);
            lblBaudRateLabel.Margin = new Padding(12, 0, 0, 0);
            lblBaudRateLabel.Name = "lblBaudRateLabel";
            lblBaudRateLabel.Size = new Size(222, 46);
            lblBaudRateLabel.TabIndex = 1;
            lblBaudRateLabel.Text = "Baud Rate";
            lblBaudRateLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBaudRateValue
            // 
            lblBaudRateValue.Dock = DockStyle.Fill;
            lblBaudRateValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblBaudRateValue.Location = new Point(238, 0);
            lblBaudRateValue.Margin = new Padding(0, 0, 12, 0);
            lblBaudRateValue.Name = "lblBaudRateValue";
            lblBaudRateValue.Size = new Size(114, 46);
            lblBaudRateValue.TabIndex = 2;
            lblBaudRateValue.Text = "19200";
            lblBaudRateValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSystemSettingsTitle
            // 
            lblSystemSettingsTitle.Dock = DockStyle.Top;
            lblSystemSettingsTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSystemSettingsTitle.Location = new Point(16, 16);
            lblSystemSettingsTitle.Margin = new Padding(0, 0, 0, 10);
            lblSystemSettingsTitle.Name = "lblSystemSettingsTitle";
            lblSystemSettingsTitle.Size = new Size(366, 28);
            lblSystemSettingsTitle.TabIndex = 1;
            lblSystemSettingsTitle.Text = "System Settings";
            lblSystemSettingsTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // spacerRightGap
            // 
            spacerRightGap.Dock = DockStyle.Fill;
            spacerRightGap.Location = new Point(3, 303);
            spacerRightGap.Name = "spacerRightGap";
            spacerRightGap.Size = new Size(394, 14);
            spacerRightGap.TabIndex = 1;
            // 
            // panelStatusInfo
            // 
            panelStatusInfo.BackColor = Color.FromArgb(227, 242, 253);
            panelStatusInfo.BorderStyle = BorderStyle.FixedSingle;
            panelStatusInfo.Controls.Add(lblStatusInfoText);
            panelStatusInfo.Controls.Add(lblStatusInfoTitle);
            panelStatusInfo.Dock = DockStyle.Fill;
            panelStatusInfo.Location = new Point(0, 320);
            panelStatusInfo.Margin = new Padding(0);
            panelStatusInfo.Name = "panelStatusInfo";
            panelStatusInfo.Padding = new Padding(16);
            panelStatusInfo.Size = new Size(400, 291);
            panelStatusInfo.TabIndex = 2;
            // 
            // lblStatusInfoText
            // 
            lblStatusInfoText.Dock = DockStyle.Fill;
            lblStatusInfoText.Font = new Font("Segoe UI", 9F);
            lblStatusInfoText.Location = new Point(16, 42);
            lblStatusInfoText.Name = "lblStatusInfoText";
            lblStatusInfoText.Size = new Size(366, 231);
            lblStatusInfoText.TabIndex = 0;
            lblStatusInfoText.Text = "• Controller is running\r\n• Pressure increasing\r\n• Systems operational\r\n• Last update: 0.5s ago";
            // 
            // lblStatusInfoTitle
            // 
            lblStatusInfoTitle.Dock = DockStyle.Top;
            lblStatusInfoTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatusInfoTitle.Location = new Point(16, 16);
            lblStatusInfoTitle.Margin = new Padding(0, 0, 0, 8);
            lblStatusInfoTitle.Name = "lblStatusInfoTitle";
            lblStatusInfoTitle.Size = new Size(366, 26);
            lblStatusInfoTitle.TabIndex = 1;
            lblStatusInfoTitle.Text = "ℹ️  Status Information";
            lblStatusInfoTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AlicatForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(1284, 781);
            Controls.Add(rootLayout);
            Controls.Add(menuMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuMain;
            MaximizeBox = false;
            MaximumSize = new Size(1300, 820);
            MinimumSize = new Size(1300, 820);
            Name = "AlicatForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alicat Controller";
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            rootLayout.ResumeLayout(false);
            panelStatusBar.ResumeLayout(false);
            statusLayout.ResumeLayout(false);
            statusLayout.PerformLayout();
            statusConnPanel.ResumeLayout(false);
            statusConnPanel.PerformLayout();
            ((ISupportInitialize)picLogo).EndInit();
            panelToolbar.ResumeLayout(false);
            toolbarFlow.ResumeLayout(false);
            toolbarFlow.PerformLayout();
            layoutContent.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            leftLayout.ResumeLayout(false);
            layoutPressureCards.ResumeLayout(false);
            cardCurrent.ResumeLayout(false);
            cardTarget.ResumeLayout(false);
            sectionSetTarget.ResumeLayout(false);
            setTargetRow.ResumeLayout(false);
            setTargetRow.PerformLayout();
            sectionPressureControl.ResumeLayout(false);
            incrementRow.ResumeLayout(false);
            incrementRow.PerformLayout();
            panelRight.ResumeLayout(false);
            rightLayout.ResumeLayout(false);
            panelSystemSettings.ResumeLayout(false);
            systemSettingsStack.ResumeLayout(false);
            settingRampSpeed.ResumeLayout(false);
            settingRampSpeedRow.ResumeLayout(false);
            settingMaxPressure.ResumeLayout(false);
            settingMaxPressureRow.ResumeLayout(false);
            settingUnits.ResumeLayout(false);
            settingUnitsRow.ResumeLayout(false);
            settingConnection.ResumeLayout(false);
            settingConnectionRow.ResumeLayout(false);
            settingBaudRate.ResumeLayout(false);
            settingBaudRateRow.ResumeLayout(false);
            panelStatusInfo.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
} 