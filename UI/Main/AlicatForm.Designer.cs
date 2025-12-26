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

        private readonly Color lightBgPrimary = Color.FromArgb(240, 240, 240);
        private readonly Color lightBgWindow = Color.White;
        private readonly Color lightBgMenu = Color.FromArgb(245, 245, 245);
        private readonly Color lightBgStatus = Color.FromArgb(232, 244, 248);
        private readonly Color lightBgSection = Color.FromArgb(250, 250, 250);
        private readonly Color lightTextPrimary = Color.FromArgb(51, 51, 51);
        private readonly Color lightTextSecondary = Color.FromArgb(85, 85, 85);
        private readonly Color lightTextMuted = Color.FromArgb(119, 119, 119);
        private readonly Color lightBorderColor = Color.FromArgb(204, 204, 204);
        private readonly Color lightAccentBlue = Color.FromArgb(33, 150, 243);
        private readonly Color lightAccentGold = Color.FromArgb(255, 193, 7);
        private readonly Color lightAccentGreen = Color.FromArgb(76, 175, 80);
        private readonly Color lightStatusDot = Color.FromArgb(76, 175, 80);

        private readonly Color darkBgPrimary = Color.FromArgb(15, 15, 22);
        private readonly Color darkBgWindow = Color.FromArgb(22, 22, 32);
        private readonly Color darkBgMenu = Color.FromArgb(28, 28, 40);
        private readonly Color darkBgStatus = Color.FromArgb(26, 40, 50);
        private readonly Color darkBgSection = Color.FromArgb(34, 34, 44);
        private readonly Color darkTextPrimary = Color.FromArgb(240, 240, 245);
        private readonly Color darkTextSecondary = Color.FromArgb(176, 176, 192);
        private readonly Color darkTextMuted = Color.FromArgb(112, 112, 133);
        private readonly Color darkBorderColor = Color.FromArgb(54, 54, 72);
        private readonly Color darkAccentBlue = Color.FromArgb(0, 200, 240);
        private readonly Color darkAccentGold = Color.FromArgb(240, 200, 0);
        private readonly Color darkAccentGreen = Color.FromArgb(0, 224, 128);
        private readonly Color darkStatusDot = Color.FromArgb(0, 224, 128);

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new Container();

            // =========================
            // Menu
            // =========================
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

            // =========================
            // Root
            // =========================
            rootLayout = new TableLayoutPanel();

            // =========================
            // Status bar
            // =========================
            panelStatusBar = new Panel();
            statusLayout = new TableLayoutPanel();
            statusConnPanel = new FlowLayoutPanel();
            lblStatusDot = new Label();
            lblConnectionStatus = new Label();
            lblBaudRate = new Label();
            lblLastUpdate = new Label();
            picLogo = new PictureBox();

            // =========================
            // Toolbar
            // =========================
            panelToolbar = new Panel();
            toolbarFlow = new FlowLayoutPanel();
            lblToolbarControl = new Label();
            btnPurge = new Button();
            lblToolbarWindows = new Label();
            btnGraph = new Button();
            btnTable = new Button();
            btnTerminal = new Button();

            // =========================
            // Content
            // =========================
            layoutContent = new TableLayoutPanel();
            panelLeft = new Panel();
            panelRight = new Panel();

            spacerContentCol = new Panel();
            spacerLeftGap1 = new Panel();
            spacerLeftGap2 = new Panel();
            spacerLeftGap3 = new Panel();
            spacerRightGap = new Panel();

            // Left
            leftLayout = new TableLayoutPanel();
            layoutPressureCards = new TableLayoutPanel();

            cardCurrent = new Panel();
            lblCurrentTitle = new Label();
            lblCurrentValue = new Label();
            lblCurrentUnit = new Label();
            lblCurrentRate = new Label();

            cardTarget = new Panel();
            lblTargetTitle = new Label();
            lblTargetValue = new Label();
            lblTargetUnit = new Label();
            lblTargetStatus = new Label();

            sectionSetTarget = new Panel();
            lblSetTargetTitle = new Label();
            setTargetRow = new TableLayoutPanel();
            txtTargetInput = new TextBox();
            lblTargetInputUnit = new Label();
            btnGoToTarget = new Button();

            sectionPressureControl = new Panel();
            lblPressureControlTitle = new Label();
            incrementRow = new TableLayoutPanel();
            lblIncrementLabel = new Label();
            btnIncrementMinus = new Button();
            txtIncrement = new TextBox();
            btnIncrementPlus = new Button();
            lblIncrementUnit = new Label();
            lblAdjustPressureLabel = new Label();
            btnIncrease = new Button();
            btnDecrease = new Button();

            // Right
            rightLayout = new TableLayoutPanel();
            panelSystemSettings = new Panel();
            lblSystemSettingsTitle = new Label();
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

            panelStatusInfo = new Panel();
            lblStatusInfoTitle = new Label();
            lblStatusInfoText = new Label();

            ((ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();

            // ====================================================================
            // MENU STRIP
            // ====================================================================
            menuMain.Dock = DockStyle.Top;
            menuMain.Name = "menuMain";
            menuMain.AutoSize = false;
            menuMain.Height = 35;
            menuMain.Padding = new Padding(8, 6, 8, 6);
            menuMain.TabIndex = 0;

            menuFile.Name = "menuFile";
            menuFile.Text = "File";

            menuFileNewSession.Name = "menuFileNewSession";
            menuFileNewSession.Text = "New Session...";
            menuFileNewSession.Click += menuFileNewSession_Click;

            menuFileTestMode.Name = "menuFileTestMode";
            menuFileTestMode.Text = "Start Test Mode";

            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuFileNewSession,
                menuFileTestMode
            });

            menuSettings.Name = "menuSettings";
            menuSettings.Text = "Settings";

            menuSettingsOptions.Name = "menuSettingsOptions";
            menuSettingsOptions.Text = "Options";
            menuSettingsOptions.Click += btnOptions_Click;

            menuSettingsCommunication.Name = "menuSettingsCommunication";
            menuSettingsCommunication.Text = "Communication";
            menuSettingsCommunication.Click += btnCommunication_Click;

            menuSettings.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuSettingsOptions,
                menuSettingsCommunication
            });

            menuView.Name = "menuView";
            menuView.Text = "View";

            menuViewLightTheme.Name = "menuViewLightTheme";
            menuViewLightTheme.Text = "Light Theme";
            menuViewLightTheme.Checked = true;
            menuViewLightTheme.Click += MenuViewLightTheme_Click;

            menuViewDarkTheme.Name = "menuViewDarkTheme";
            menuViewDarkTheme.Text = "Dark Theme";
            menuViewDarkTheme.Checked = false;
            menuViewDarkTheme.Click += MenuViewDarkTheme_Click;

            menuView.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuViewLightTheme,
                menuViewDarkTheme
            });

            menuHelp.Name = "menuHelp";
            menuHelp.Text = "Help";

            toolStripVersion.Name = "toolStripVersion";
            toolStripVersion.Text = "version 1.2.0";
            toolStripVersion.Alignment = ToolStripItemAlignment.Right;
            toolStripVersion.ForeColor = Color.Gray;

            menuMain.Items.AddRange(new ToolStripItem[]
            {
                menuFile, menuSettings, menuView, menuHelp, toolStripVersion
            });

            // ====================================================================
            // ROOT LAYOUT (status + toolbar + content)
            // ====================================================================
            rootLayout.ColumnCount = 1;
            rootLayout.RowCount = 3;
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = new Padding(0);
            rootLayout.Padding = new Padding(0);

            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // status
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));  // toolbar
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // content

            // ====================================================================
            // STATUS BAR
            // ====================================================================
            panelStatusBar.Dock = DockStyle.Fill;
            panelStatusBar.Margin = new Padding(0);
            panelStatusBar.Padding = new Padding(16, 0, 16, 0);

            statusLayout.Dock = DockStyle.Fill;
            statusLayout.Margin = new Padding(0);
            statusLayout.Padding = new Padding(0);
            statusLayout.ColumnCount = 7;
            statusLayout.RowCount = 1;

            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            statusConnPanel.FlowDirection = FlowDirection.LeftToRight;
            statusConnPanel.WrapContents = false;
            statusConnPanel.AutoSize = true;
            statusConnPanel.Margin = new Padding(0);
            statusConnPanel.Padding = new Padding(0);

            lblStatusDot.AutoSize = true;
            lblStatusDot.Text = "●";
            lblStatusDot.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatusDot.Margin = new Padding(0, 14, 8, 0);

            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Text = "Connected (COM3)";
            lblConnectionStatus.Font = new Font("Segoe UI", 9F);
            lblConnectionStatus.Margin = new Padding(0, 16, 0, 0);

            statusConnPanel.Controls.Add(lblStatusDot);
            statusConnPanel.Controls.Add(lblConnectionStatus);

            lblBaudRate.AutoSize = true;
            lblBaudRate.Text = "Baud: 19200";
            lblBaudRate.Font = new Font("Segoe UI", 9F);
            lblBaudRate.Margin = new Padding(0, 16, 0, 0);

            lblLastUpdate.AutoSize = true;
            lblLastUpdate.Text = "Last update: 0.5s ago";
            lblLastUpdate.Font = new Font("Segoe UI", 9F);
            lblLastUpdate.Margin = new Padding(0, 16, 0, 0);

            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.Width = 180;
            picLogo.Height = 45;
            picLogo.Margin = new Padding(0, 2, 0, 0);
            picLogo.Anchor = AnchorStyles.Right;

            statusLayout.Controls.Add(statusConnPanel, 0, 0);
            statusLayout.Controls.Add(new Panel(), 1, 0);
            statusLayout.Controls.Add(lblBaudRate, 2, 0);
            statusLayout.Controls.Add(new Panel(), 3, 0);
            statusLayout.Controls.Add(lblLastUpdate, 4, 0);
            statusLayout.Controls.Add(new Panel(), 5, 0);
            statusLayout.Controls.Add(picLogo, 6, 0);

            panelStatusBar.Controls.Add(statusLayout);

            // ====================================================================
            // TOOLBAR
            // ====================================================================
            panelToolbar.Dock = DockStyle.Fill;
            panelToolbar.Margin = new Padding(0);
            panelToolbar.Padding = new Padding(16, 0, 16, 0);

            toolbarFlow.Dock = DockStyle.Fill;
            toolbarFlow.FlowDirection = FlowDirection.LeftToRight;
            toolbarFlow.WrapContents = false;
            toolbarFlow.Margin = new Padding(0);
            toolbarFlow.Padding = new Padding(0, 5, 0, 5);

            lblToolbarControl.AutoSize = true;
            lblToolbarControl.Text = "Control:";
            lblToolbarControl.Font = new Font("Segoe UI", 9F);
            lblToolbarControl.Margin = new Padding(0, 7, 8, 0);

            btnPurge.Text = "🧪  Purge";
            btnPurge.AutoSize = true;
            btnPurge.Margin = new Padding(0, 0, 22, 0);
            btnPurge.Padding = new Padding(10, 6, 10, 6);
            btnPurge.FlatStyle = FlatStyle.Flat;
            btnPurge.Click += btnPurge_Click;

            lblToolbarWindows.AutoSize = true;
            lblToolbarWindows.Text = "Windows:";
            lblToolbarWindows.Font = new Font("Segoe UI", 9F);
            lblToolbarWindows.Margin = new Padding(0, 7, 10, 0);

            btnGraph.Text = "📊  Graph";
            btnGraph.AutoSize = true;
            btnGraph.Margin = new Padding(0, 0, 6, 0);
            btnGraph.Padding = new Padding(10, 6, 10, 6);
            btnGraph.FlatStyle = FlatStyle.Flat;
            btnGraph.Click += btnGraph_Click;

            btnTable.Text = "📋  Table";
            btnTable.AutoSize = true;
            btnTable.Margin = new Padding(0, 0, 6, 0);
            btnTable.Padding = new Padding(10, 6, 10, 6);
            btnTable.FlatStyle = FlatStyle.Flat;
            btnTable.Click += btnTable_Click;

            btnTerminal.Text = "💻  Terminal";
            btnTerminal.AutoSize = true;
            btnTerminal.Margin = new Padding(0);
            btnTerminal.Padding = new Padding(10, 6, 10, 6);
            btnTerminal.FlatStyle = FlatStyle.Flat;
            btnTerminal.Click += btnTerminal_Click;

            toolbarFlow.Controls.Add(lblToolbarControl);
            toolbarFlow.Controls.Add(btnPurge);
            toolbarFlow.Controls.Add(lblToolbarWindows);
            toolbarFlow.Controls.Add(btnGraph);
            toolbarFlow.Controls.Add(btnTable);
            toolbarFlow.Controls.Add(btnTerminal);

            panelToolbar.Controls.Add(toolbarFlow);

            // ====================================================================
            // CONTENT LAYOUT (Left 840 + gap 20 + Right 400)
            // ====================================================================
            layoutContent.ColumnCount = 3;
            layoutContent.RowCount = 1;
            layoutContent.Dock = DockStyle.Fill;
            layoutContent.Margin = new Padding(0);
            layoutContent.Padding = new Padding(20);

            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 840F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            layoutContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            spacerContentCol.Dock = DockStyle.Fill;

            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Margin = new Padding(0);
            panelLeft.Padding = new Padding(0);

            panelRight.Dock = DockStyle.Fill;
            panelRight.Margin = new Padding(0);
            panelRight.Padding = new Padding(0);

            layoutContent.Controls.Add(panelLeft, 0, 0);
            layoutContent.Controls.Add(spacerContentCol, 1, 0);
            layoutContent.Controls.Add(panelRight, 2, 0);

            // ====================================================================
            // LEFT LAYOUT (cards / set target / control)
            // ====================================================================
            leftLayout.ColumnCount = 1;
            leftLayout.RowCount = 5;
            leftLayout.Dock = DockStyle.Fill;
            leftLayout.Margin = new Padding(0);
            leftLayout.Padding = new Padding(0);

            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 240F)); // cards
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));  // gap
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F)); // set target
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));  // gap
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // control

            panelLeft.Controls.Add(leftLayout);

            // ---- cards row: 410 + 20 + 410
            layoutPressureCards.ColumnCount = 3;
            layoutPressureCards.RowCount = 1;
            layoutPressureCards.Dock = DockStyle.Fill;
            layoutPressureCards.Margin = new Padding(0);
            layoutPressureCards.Padding = new Padding(0);
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410F));
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410F));
            layoutPressureCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            spacerLeftGap1.Dock = DockStyle.Fill;

            // Current card
            cardCurrent.Dock = DockStyle.Fill;
            cardCurrent.Margin = new Padding(0);
            cardCurrent.Padding = new Padding(20);
            cardCurrent.BorderStyle = BorderStyle.FixedSingle;

            lblCurrentTitle.Dock = DockStyle.Top;
            lblCurrentTitle.Height = 30;
            lblCurrentTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCurrentTitle.Text = "CURRENT PRESSURE";

            lblCurrentValue.Dock = DockStyle.Top;
            lblCurrentValue.Height = 92;
            lblCurrentValue.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentValue.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
            lblCurrentValue.Text = "0.0";

            lblCurrentUnit.Dock = DockStyle.Top;
            lblCurrentUnit.Height = 35;
            lblCurrentUnit.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentUnit.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCurrentUnit.Text = "PSIG";

            lblCurrentRate.Dock = DockStyle.Fill;
            lblCurrentRate.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentRate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCurrentRate.Text = "→ 0.0 /s";

            cardCurrent.Controls.Add(lblCurrentRate);
            cardCurrent.Controls.Add(lblCurrentUnit);
            cardCurrent.Controls.Add(lblCurrentValue);
            cardCurrent.Controls.Add(lblCurrentTitle);

            // Target card
            cardTarget.Dock = DockStyle.Fill;
            cardTarget.Margin = new Padding(0);
            cardTarget.Padding = new Padding(20);
            cardTarget.BorderStyle = BorderStyle.FixedSingle;

            lblTargetTitle.Dock = DockStyle.Top;
            lblTargetTitle.Height = 30;
            lblTargetTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTargetTitle.Text = "TARGET PRESSURE";

            lblTargetValue.Dock = DockStyle.Top;
            lblTargetValue.Height = 92;
            lblTargetValue.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetValue.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
            lblTargetValue.Text = "0.0";

            lblTargetUnit.Dock = DockStyle.Top;
            lblTargetUnit.Height = 35;
            lblTargetUnit.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetUnit.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTargetUnit.Text = "PSIG";

            lblTargetStatus.Dock = DockStyle.Fill;
            lblTargetStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTargetStatus.Text = "At target";

            cardTarget.Controls.Add(lblTargetStatus);
            cardTarget.Controls.Add(lblTargetUnit);
            cardTarget.Controls.Add(lblTargetValue);
            cardTarget.Controls.Add(lblTargetTitle);

            layoutPressureCards.Controls.Add(cardCurrent, 0, 0);
            layoutPressureCards.Controls.Add(spacerLeftGap1, 1, 0);
            layoutPressureCards.Controls.Add(cardTarget, 2, 0);

            // ---- Set Target section
            sectionSetTarget.Dock = DockStyle.Fill;
            sectionSetTarget.Margin = new Padding(0);
            sectionSetTarget.Padding = new Padding(18);
            sectionSetTarget.BorderStyle = BorderStyle.FixedSingle;

            lblSetTargetTitle.Dock = DockStyle.Top;
            lblSetTargetTitle.Height = 26;
            lblSetTargetTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblSetTargetTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSetTargetTitle.Text = "Set Target Pressure";

            setTargetRow.Dock = DockStyle.Fill;
            setTargetRow.Margin = new Padding(0, 10, 0, 0);
            setTargetRow.Padding = new Padding(0);
            setTargetRow.ColumnCount = 4;
            setTargetRow.RowCount = 1;
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // input
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F)); // unit
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15F)); // gap
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F)); // button
            setTargetRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            txtTargetInput.Dock = DockStyle.Fill;
            txtTargetInput.Font = new Font("Courier New", 12F);
            txtTargetInput.Text = "100.0";

            lblTargetInputUnit.Dock = DockStyle.Fill;
            lblTargetInputUnit.TextAlign = ContentAlignment.MiddleLeft;
            lblTargetInputUnit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTargetInputUnit.Text = "PSIG";
            lblTargetInputUnit.Margin = new Padding(10, 0, 0, 0);

            btnGoToTarget.Dock = DockStyle.Fill;
            btnGoToTarget.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGoToTarget.Text = "🎯  Go to Target";
            btnGoToTarget.FlatStyle = FlatStyle.Flat;
            btnGoToTarget.Click += btnGoTarget_Click;

            setTargetRow.Controls.Add(txtTargetInput, 0, 0);
            setTargetRow.Controls.Add(lblTargetInputUnit, 1, 0);
            setTargetRow.Controls.Add(new Panel(), 2, 0);
            setTargetRow.Controls.Add(btnGoToTarget, 3, 0);

            sectionSetTarget.Controls.Add(setTargetRow);
            sectionSetTarget.Controls.Add(lblSetTargetTitle);

            // ---- Pressure Control section
            sectionPressureControl.Dock = DockStyle.Fill;
            sectionPressureControl.Margin = new Padding(0);
            sectionPressureControl.Padding = new Padding(18);
            sectionPressureControl.BorderStyle = BorderStyle.FixedSingle;

            lblPressureControlTitle.Dock = DockStyle.Top;
            lblPressureControlTitle.Height = 28;
            lblPressureControlTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblPressureControlTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPressureControlTitle.Text = "Pressure Control";

            incrementRow.Dock = DockStyle.Top;
            incrementRow.Height = 52;
            incrementRow.Margin = new Padding(0, 16, 0, 0);
            incrementRow.Padding = new Padding(0);
            incrementRow.ColumnCount = 6;
            incrementRow.RowCount = 1;

            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));  // label
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));  // minus
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F)); // value
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));  // plus
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // unit
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));  // filler

            lblIncrementLabel.Dock = DockStyle.Fill;
            lblIncrementLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblIncrementLabel.Font = new Font("Segoe UI", 10F);
            lblIncrementLabel.Text = "Increment:";

            btnIncrementMinus.Dock = DockStyle.Fill;
            btnIncrementMinus.FlatStyle = FlatStyle.Flat;
            btnIncrementMinus.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnIncrementMinus.Text = "−";
            btnIncrementMinus.Click += btnIncrementMinus_Click;

            txtIncrement.Dock = DockStyle.Fill;
            txtIncrement.Font = new Font("Courier New", 12F, FontStyle.Bold);
            txtIncrement.TextAlign = HorizontalAlignment.Center;
            txtIncrement.Text = "5.0";

            btnIncrementPlus.Dock = DockStyle.Fill;
            btnIncrementPlus.FlatStyle = FlatStyle.Flat;
            btnIncrementPlus.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnIncrementPlus.Text = "+";
            btnIncrementPlus.Click += btnIncrementPlus_Click;

            lblIncrementUnit.Dock = DockStyle.Fill;
            lblIncrementUnit.TextAlign = ContentAlignment.MiddleLeft;
            lblIncrementUnit.Font = new Font("Segoe UI", 10F);
            lblIncrementUnit.Text = "PSIG";
            lblIncrementUnit.Margin = new Padding(8, 0, 0, 0);

            incrementRow.Controls.Add(lblIncrementLabel, 0, 0);
            incrementRow.Controls.Add(btnIncrementMinus, 1, 0);
            incrementRow.Controls.Add(txtIncrement, 2, 0);
            incrementRow.Controls.Add(btnIncrementPlus, 3, 0);
            incrementRow.Controls.Add(lblIncrementUnit, 4, 0);
            incrementRow.Controls.Add(new Panel(), 5, 0);

            lblAdjustPressureLabel.Dock = DockStyle.Top;
            lblAdjustPressureLabel.Height = 26;
            lblAdjustPressureLabel.Margin = new Padding(0, 16, 0, 0);
            lblAdjustPressureLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblAdjustPressureLabel.Font = new Font("Segoe UI", 10F);
            lblAdjustPressureLabel.Text = "Adjust Pressure:";

            btnIncrease.Dock = DockStyle.Top;
            btnIncrease.Height = 55;
            btnIncrease.Margin = new Padding(0, 10, 0, 10);
            btnIncrease.FlatStyle = FlatStyle.Flat;
            btnIncrease.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIncrease.Text = "▲  Increase (+5.0 PSIG)";
            btnIncrease.Click += btnIncrease_Click;

            btnDecrease.Dock = DockStyle.Top;
            btnDecrease.Height = 55;
            btnDecrease.Margin = new Padding(0);
            btnDecrease.FlatStyle = FlatStyle.Flat;
            btnDecrease.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDecrease.Text = "▼  Decrease (-5.0 PSIG)";
            btnDecrease.Click += btnDecrease_Click;

            sectionPressureControl.Controls.Add(btnDecrease);
            sectionPressureControl.Controls.Add(btnIncrease);
            sectionPressureControl.Controls.Add(lblAdjustPressureLabel);
            sectionPressureControl.Controls.Add(incrementRow);
            sectionPressureControl.Controls.Add(lblPressureControlTitle);

            // Add to left stack
            spacerLeftGap2.Dock = DockStyle.Fill;
            spacerLeftGap3.Dock = DockStyle.Fill;

            leftLayout.Controls.Add(layoutPressureCards, 0, 0);
            leftLayout.Controls.Add(spacerLeftGap2, 0, 1);
            leftLayout.Controls.Add(sectionSetTarget, 0, 2);
            leftLayout.Controls.Add(spacerLeftGap3, 0, 3);
            leftLayout.Controls.Add(sectionPressureControl, 0, 4);

            // ====================================================================
            // RIGHT LAYOUT (System Settings + gap + Status Info)
            // ====================================================================
            rightLayout.ColumnCount = 1;
            rightLayout.RowCount = 3;
            rightLayout.Dock = DockStyle.Fill;
            rightLayout.Margin = new Padding(0);
            rightLayout.Padding = new Padding(0);
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            panelRight.Controls.Add(rightLayout);

            // ---- System Settings panel
            panelSystemSettings.Dock = DockStyle.Fill;
            panelSystemSettings.Margin = new Padding(0);
            panelSystemSettings.Padding = new Padding(16);
            panelSystemSettings.BorderStyle = BorderStyle.FixedSingle;

            lblSystemSettingsTitle.Dock = DockStyle.Top;
            lblSystemSettingsTitle.Height = 28;
            lblSystemSettingsTitle.Margin = new Padding(0, 0, 0, 10);
            lblSystemSettingsTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblSystemSettingsTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSystemSettingsTitle.Text = "System Settings";

            systemSettingsStack.Dock = DockStyle.Fill;
            systemSettingsStack.Margin = new Padding(0);
            systemSettingsStack.Padding = new Padding(0);
            systemSettingsStack.ColumnCount = 1;
            systemSettingsStack.RowCount = 5;
            systemSettingsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            systemSettingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            // 1) Ramp Speed
            settingRampSpeed.Dock = DockStyle.Fill;
            settingRampSpeed.Margin = new Padding(0, 0, 0, 10);
            settingRampSpeed.BorderStyle = BorderStyle.FixedSingle;

            settingRampSpeedRow.Dock = DockStyle.Fill;
            settingRampSpeedRow.Margin = new Padding(0);
            settingRampSpeedRow.Padding = new Padding(0);
            settingRampSpeedRow.ColumnCount = 3;
            settingRampSpeedRow.RowCount = 1;
            settingRampSpeedRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingRampSpeedRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingRampSpeedRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingRampSpeedRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            stripRampSpeed.Dock = DockStyle.Fill;
            stripRampSpeed.Margin = new Padding(0);

            lblRampSpeedLabel.Dock = DockStyle.Fill;
            lblRampSpeedLabel.Margin = new Padding(12, 0, 0, 0);
            lblRampSpeedLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblRampSpeedLabel.Font = new Font("Segoe UI", 10F);
            lblRampSpeedLabel.Text = "Ramp Speed";

            lblRampSpeedValue.Dock = DockStyle.Fill;
            lblRampSpeedValue.Margin = new Padding(0, 0, 12, 0);
            lblRampSpeedValue.TextAlign = ContentAlignment.MiddleRight;
            lblRampSpeedValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblRampSpeedValue.Text = "PSIG/s";

            settingRampSpeedRow.Controls.Add(stripRampSpeed, 0, 0);
            settingRampSpeedRow.Controls.Add(lblRampSpeedLabel, 1, 0);
            settingRampSpeedRow.Controls.Add(lblRampSpeedValue, 2, 0);
            settingRampSpeed.Controls.Add(settingRampSpeedRow);

            // 2) Max Pressure
            settingMaxPressure.Dock = DockStyle.Fill;
            settingMaxPressure.Margin = new Padding(0, 0, 0, 10);
            settingMaxPressure.BorderStyle = BorderStyle.FixedSingle;

            settingMaxPressureRow.Dock = DockStyle.Fill;
            settingMaxPressureRow.Margin = new Padding(0);
            settingMaxPressureRow.Padding = new Padding(0);
            settingMaxPressureRow.ColumnCount = 3;
            settingMaxPressureRow.RowCount = 1;
            settingMaxPressureRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingMaxPressureRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingMaxPressureRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingMaxPressureRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            stripMaxPressure.Dock = DockStyle.Fill;
            stripMaxPressure.Margin = new Padding(0);

            lblMaxPressureLabel.Dock = DockStyle.Fill;
            lblMaxPressureLabel.Margin = new Padding(12, 0, 0, 0);
            lblMaxPressureLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblMaxPressureLabel.Font = new Font("Segoe UI", 10F);
            lblMaxPressureLabel.Text = "Max Pressure";

            lblMaxPressureValue.Dock = DockStyle.Fill;
            lblMaxPressureValue.Margin = new Padding(0, 0, 12, 0);
            lblMaxPressureValue.TextAlign = ContentAlignment.MiddleRight;
            lblMaxPressureValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblMaxPressureValue.Text = "200 PSIG";

            settingMaxPressureRow.Controls.Add(stripMaxPressure, 0, 0);
            settingMaxPressureRow.Controls.Add(lblMaxPressureLabel, 1, 0);
            settingMaxPressureRow.Controls.Add(lblMaxPressureValue, 2, 0);
            settingMaxPressure.Controls.Add(settingMaxPressureRow);

            // 3) Units
            settingUnits.Dock = DockStyle.Fill;
            settingUnits.Margin = new Padding(0, 0, 0, 10);
            settingUnits.BorderStyle = BorderStyle.FixedSingle;

            settingUnitsRow.Dock = DockStyle.Fill;
            settingUnitsRow.Margin = new Padding(0);
            settingUnitsRow.Padding = new Padding(0);
            settingUnitsRow.ColumnCount = 3;
            settingUnitsRow.RowCount = 1;
            settingUnitsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingUnitsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingUnitsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingUnitsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            stripUnits.Dock = DockStyle.Fill;
            stripUnits.Margin = new Padding(0);

            lblUnitsLabel.Dock = DockStyle.Fill;
            lblUnitsLabel.Margin = new Padding(12, 0, 0, 0);
            lblUnitsLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblUnitsLabel.Font = new Font("Segoe UI", 10F);
            lblUnitsLabel.Text = "Units";

            lblUnitsValue.Dock = DockStyle.Fill;
            lblUnitsValue.Margin = new Padding(0, 0, 12, 0);
            lblUnitsValue.TextAlign = ContentAlignment.MiddleRight;
            lblUnitsValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblUnitsValue.Text = "PSIG";

            settingUnitsRow.Controls.Add(stripUnits, 0, 0);
            settingUnitsRow.Controls.Add(lblUnitsLabel, 1, 0);
            settingUnitsRow.Controls.Add(lblUnitsValue, 2, 0);
            settingUnits.Controls.Add(settingUnitsRow);

            // 4) Connection
            settingConnection.Dock = DockStyle.Fill;
            settingConnection.Margin = new Padding(0, 0, 0, 10);
            settingConnection.BorderStyle = BorderStyle.FixedSingle;

            settingConnectionRow.Dock = DockStyle.Fill;
            settingConnectionRow.Margin = new Padding(0);
            settingConnectionRow.Padding = new Padding(0);
            settingConnectionRow.ColumnCount = 3;
            settingConnectionRow.RowCount = 1;
            settingConnectionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingConnectionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingConnectionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingConnectionRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            stripConnection.Dock = DockStyle.Fill;
            stripConnection.Margin = new Padding(0);

            lblConnectionLabel.Dock = DockStyle.Fill;
            lblConnectionLabel.Margin = new Padding(12, 0, 0, 0);
            lblConnectionLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblConnectionLabel.Font = new Font("Segoe UI", 10F);
            lblConnectionLabel.Text = "Connection";

            lblConnectionValue.Dock = DockStyle.Fill;
            lblConnectionValue.Margin = new Padding(0, 0, 12, 0);
            lblConnectionValue.TextAlign = ContentAlignment.MiddleRight;
            lblConnectionValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblConnectionValue.Text = "COM3";

            settingConnectionRow.Controls.Add(stripConnection, 0, 0);
            settingConnectionRow.Controls.Add(lblConnectionLabel, 1, 0);
            settingConnectionRow.Controls.Add(lblConnectionValue, 2, 0);
            settingConnection.Controls.Add(settingConnectionRow);

            // 5) Baud Rate
            settingBaudRate.Dock = DockStyle.Fill;
            settingBaudRate.Margin = new Padding(0, 0, 0, 0);
            settingBaudRate.BorderStyle = BorderStyle.FixedSingle;

            settingBaudRateRow.Dock = DockStyle.Fill;
            settingBaudRateRow.Margin = new Padding(0);
            settingBaudRateRow.Padding = new Padding(0);
            settingBaudRateRow.ColumnCount = 3;
            settingBaudRateRow.RowCount = 1;
            settingBaudRateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            settingBaudRateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            settingBaudRateRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            settingBaudRateRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            stripBaudRate.Dock = DockStyle.Fill;
            stripBaudRate.Margin = new Padding(0);

            lblBaudRateLabel.Dock = DockStyle.Fill;
            lblBaudRateLabel.Margin = new Padding(12, 0, 0, 0);
            lblBaudRateLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblBaudRateLabel.Font = new Font("Segoe UI", 10F);
            lblBaudRateLabel.Text = "Baud Rate";

            lblBaudRateValue.Dock = DockStyle.Fill;
            lblBaudRateValue.Margin = new Padding(0, 0, 12, 0);
            lblBaudRateValue.TextAlign = ContentAlignment.MiddleRight;
            lblBaudRateValue.Font = new Font("Courier New", 10F, FontStyle.Bold);
            lblBaudRateValue.Text = "19200";

            settingBaudRateRow.Controls.Add(stripBaudRate, 0, 0);
            settingBaudRateRow.Controls.Add(lblBaudRateLabel, 1, 0);
            settingBaudRateRow.Controls.Add(lblBaudRateValue, 2, 0);
            settingBaudRate.Controls.Add(settingBaudRateRow);

            systemSettingsStack.Controls.Add(settingRampSpeed, 0, 0);
            systemSettingsStack.Controls.Add(settingMaxPressure, 0, 1);
            systemSettingsStack.Controls.Add(settingUnits, 0, 2);
            systemSettingsStack.Controls.Add(settingConnection, 0, 3);
            systemSettingsStack.Controls.Add(settingBaudRate, 0, 4);

            panelSystemSettings.Controls.Add(systemSettingsStack);
            panelSystemSettings.Controls.Add(lblSystemSettingsTitle);

            // ---- Status Info panel
            panelStatusInfo.Dock = DockStyle.Fill;
            panelStatusInfo.Margin = new Padding(0);
            panelStatusInfo.Padding = new Padding(16);
            panelStatusInfo.BorderStyle = BorderStyle.FixedSingle;

            lblStatusInfoTitle.Dock = DockStyle.Top;
            lblStatusInfoTitle.Height = 26;
            lblStatusInfoTitle.Margin = new Padding(0, 0, 0, 8);
            lblStatusInfoTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblStatusInfoTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatusInfoTitle.Text = "ℹ️  Status Information";

            lblStatusInfoText.Dock = DockStyle.Fill;
            lblStatusInfoText.TextAlign = ContentAlignment.TopLeft;
            lblStatusInfoText.Font = new Font("Segoe UI", 9F);
            lblStatusInfoText.Text =
                "• Controller is running\r\n" +
                "• Pressure increasing\r\n" +
                "• Systems operational\r\n" +
                "• Last update: 0.5s ago";

            panelStatusInfo.Controls.Add(lblStatusInfoText);
            panelStatusInfo.Controls.Add(lblStatusInfoTitle);

            spacerRightGap.Dock = DockStyle.Fill;

            rightLayout.Controls.Add(panelSystemSettings, 0, 0);
            rightLayout.Controls.Add(spacerRightGap, 0, 1);
            rightLayout.Controls.Add(panelStatusInfo, 0, 2);

            // ====================================================================
            // Root add rows
            // ====================================================================
            rootLayout.Controls.Add(panelStatusBar, 0, 0);
            rootLayout.Controls.Add(panelToolbar, 0, 1);
            rootLayout.Controls.Add(layoutContent, 0, 2);

            // ====================================================================
            // FORM
            // ====================================================================
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;

            ClientSize = new Size(1300, 820);
            MinimumSize = new Size(1300, 820);
            MaximumSize = new Size(1300, 820);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            Text = "Alicat Controller";
            Name = "AlicatForm";

            // IMPORTANT: order matters for Docking
            Controls.Add(rootLayout);
            Controls.Add(menuMain);
            MainMenuStrip = menuMain;

            // ✅ чтобы View всегда нажимался (никто не перекрывает меню)
            menuMain.BringToFront();

            // Apply theme defaults
            ApplyLightTheme();

            ((ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ====================================================================
        // View -> Light/Dark handlers (сделано здесь, чтобы файл был "готовый")
        // ====================================================================
        private void MenuViewLightTheme_Click(object sender, EventArgs e)
        {
            ApplyLightTheme();
            menuViewLightTheme.Checked = true;
            menuViewDarkTheme.Checked = false;
        }

        private void MenuViewDarkTheme_Click(object sender, EventArgs e)
        {
            ApplyDarkTheme();
            menuViewLightTheme.Checked = false;
            menuViewDarkTheme.Checked = true;
        }

        private void ApplyLightTheme()
        {
            isDarkTheme = false;

            BackColor = lightBgPrimary;

            menuMain.BackColor = lightBgMenu;
            menuMain.ForeColor = lightTextPrimary;

            panelStatusBar.BackColor = lightBgStatus;
            lblStatusDot.ForeColor = lightStatusDot;
            lblConnectionStatus.ForeColor = lightTextSecondary;
            lblBaudRate.ForeColor = lightTextSecondary;
            lblLastUpdate.ForeColor = lightTextSecondary;

            panelToolbar.BackColor = lightBgWindow;
            lblToolbarControl.ForeColor = lightTextMuted;
            lblToolbarWindows.ForeColor = lightTextMuted;

            layoutContent.BackColor = lightBgWindow;
            panelLeft.BackColor = lightBgWindow;
            panelRight.BackColor = lightBgWindow;

            cardCurrent.BackColor = lightBgSection;
            lblCurrentTitle.ForeColor = lightTextMuted;
            lblCurrentValue.ForeColor = lightAccentBlue;
            lblCurrentUnit.ForeColor = lightTextMuted;
            lblCurrentRate.ForeColor = lightAccentGreen;

            cardTarget.BackColor = lightBgSection;
            lblTargetTitle.ForeColor = lightTextMuted;
            lblTargetValue.ForeColor = lightAccentGold;
            lblTargetUnit.ForeColor = lightTextMuted;
            lblTargetStatus.ForeColor = lightAccentGreen;

            sectionSetTarget.BackColor = lightBgSection;
            lblSetTargetTitle.ForeColor = lightTextSecondary;
            txtTargetInput.BackColor = lightBgWindow;
            txtTargetInput.ForeColor = lightTextPrimary;
            lblTargetInputUnit.ForeColor = lightTextMuted;

            btnGoToTarget.BackColor = lightAccentGreen;
            btnGoToTarget.ForeColor = Color.White;

            sectionPressureControl.BackColor = lightBgSection;
            lblPressureControlTitle.ForeColor = lightTextSecondary;
            lblIncrementLabel.ForeColor = lightTextSecondary;
            txtIncrement.BackColor = lightBgWindow;
            txtIncrement.ForeColor = lightTextPrimary;
            lblIncrementUnit.ForeColor = lightTextMuted;
            lblAdjustPressureLabel.ForeColor = lightTextMuted;

            btnIncrementMinus.BackColor = lightBgWindow;
            btnIncrementMinus.ForeColor = lightTextPrimary;
            btnIncrementMinus.FlatAppearance.BorderColor = lightBorderColor;

            btnIncrementPlus.BackColor = lightBgWindow;
            btnIncrementPlus.ForeColor = lightTextPrimary;
            btnIncrementPlus.FlatAppearance.BorderColor = lightBorderColor;

            btnIncrease.BackColor = Color.FromArgb(240, 248, 255);
            btnIncrease.ForeColor = lightAccentBlue;
            btnIncrease.FlatAppearance.BorderColor = lightAccentBlue;

            btnDecrease.BackColor = Color.FromArgb(240, 248, 255);
            btnDecrease.ForeColor = lightAccentBlue;
            btnDecrease.FlatAppearance.BorderColor = lightAccentBlue;

            panelSystemSettings.BackColor = lightBgSection;
            lblSystemSettingsTitle.ForeColor = lightTextSecondary;

            stripRampSpeed.BackColor = lightAccentBlue;
            stripMaxPressure.BackColor = lightAccentBlue;
            stripUnits.BackColor = lightAccentBlue;
            stripConnection.BackColor = lightAccentBlue;
            stripBaudRate.BackColor = lightAccentBlue;

            settingRampSpeed.BackColor = lightBgWindow;
            settingMaxPressure.BackColor = lightBgWindow;
            settingUnits.BackColor = lightBgWindow;
            settingConnection.BackColor = lightBgWindow;
            settingBaudRate.BackColor = lightBgWindow;

            lblRampSpeedLabel.ForeColor = lightTextPrimary;
            lblMaxPressureLabel.ForeColor = lightTextPrimary;
            lblUnitsLabel.ForeColor = lightTextPrimary;
            lblConnectionLabel.ForeColor = lightTextPrimary;
            lblBaudRateLabel.ForeColor = lightTextPrimary;

            lblRampSpeedValue.ForeColor = lightAccentBlue;
            lblMaxPressureValue.ForeColor = lightAccentBlue;
            lblUnitsValue.ForeColor = lightAccentBlue;
            lblConnectionValue.ForeColor = lightAccentBlue;
            lblBaudRateValue.ForeColor = lightAccentBlue;

            panelStatusInfo.BackColor = Color.FromArgb(227, 242, 253);
            lblStatusInfoTitle.ForeColor = Color.FromArgb(21, 101, 192);
            lblStatusInfoText.ForeColor = lightTextSecondary;

            btnPurge.BackColor = lightBgWindow;
            btnPurge.ForeColor = lightTextPrimary;
            btnGraph.BackColor = lightBgWindow;
            btnGraph.ForeColor = lightTextPrimary;
            btnTable.BackColor = lightBgWindow;
            btnTable.ForeColor = lightTextPrimary;
            btnTerminal.BackColor = lightBgWindow;
            btnTerminal.ForeColor = lightTextPrimary;
        }

        private void ApplyDarkTheme()
        {
            isDarkTheme = true;

            BackColor = darkBgPrimary;

            menuMain.BackColor = darkBgMenu;
            menuMain.ForeColor = darkTextPrimary;

            panelStatusBar.BackColor = darkBgStatus;
            lblStatusDot.ForeColor = darkStatusDot;
            lblConnectionStatus.ForeColor = darkTextSecondary;
            lblBaudRate.ForeColor = darkTextSecondary;
            lblLastUpdate.ForeColor = darkTextSecondary;

            panelToolbar.BackColor = darkBgWindow;
            lblToolbarControl.ForeColor = darkTextMuted;
            lblToolbarWindows.ForeColor = darkTextMuted;

            layoutContent.BackColor = darkBgWindow;
            panelLeft.BackColor = darkBgWindow;
            panelRight.BackColor = darkBgWindow;

            cardCurrent.BackColor = darkBgSection;
            lblCurrentTitle.ForeColor = darkTextMuted;
            lblCurrentValue.ForeColor = darkAccentBlue;
            lblCurrentUnit.ForeColor = darkTextMuted;
            lblCurrentRate.ForeColor = darkAccentGreen;

            cardTarget.BackColor = darkBgSection;
            lblTargetTitle.ForeColor = darkTextMuted;
            lblTargetValue.ForeColor = darkAccentGold;
            lblTargetUnit.ForeColor = darkTextMuted;
            lblTargetStatus.ForeColor = darkAccentGreen;

            sectionSetTarget.BackColor = darkBgSection;
            lblSetTargetTitle.ForeColor = darkTextSecondary;
            txtTargetInput.BackColor = darkBgWindow;
            txtTargetInput.ForeColor = darkTextPrimary;
            lblTargetInputUnit.ForeColor = darkTextMuted;

            btnGoToTarget.BackColor = darkAccentGreen;
            btnGoToTarget.ForeColor = Color.White;

            sectionPressureControl.BackColor = darkBgSection;
            lblPressureControlTitle.ForeColor = darkTextSecondary;
            lblIncrementLabel.ForeColor = darkTextSecondary;
            txtIncrement.BackColor = darkBgWindow;
            txtIncrement.ForeColor = darkTextPrimary;
            lblIncrementUnit.ForeColor = darkTextMuted;
            lblAdjustPressureLabel.ForeColor = darkTextMuted;

            btnIncrementMinus.BackColor = darkBgWindow;
            btnIncrementMinus.ForeColor = darkTextPrimary;
            btnIncrementMinus.FlatAppearance.BorderColor = darkBorderColor;

            btnIncrementPlus.BackColor = darkBgWindow;
            btnIncrementPlus.ForeColor = darkTextPrimary;
            btnIncrementPlus.FlatAppearance.BorderColor = darkBorderColor;

            btnIncrease.BackColor = Color.FromArgb(24, 32, 44);
            btnIncrease.ForeColor = darkAccentBlue;
            btnIncrease.FlatAppearance.BorderColor = darkAccentBlue;

            btnDecrease.BackColor = Color.FromArgb(24, 32, 44);
            btnDecrease.ForeColor = darkAccentBlue;
            btnDecrease.FlatAppearance.BorderColor = darkAccentBlue;

            panelSystemSettings.BackColor = darkBgSection;
            lblSystemSettingsTitle.ForeColor = darkTextSecondary;

            stripRampSpeed.BackColor = darkAccentBlue;
            stripMaxPressure.BackColor = darkAccentBlue;
            stripUnits.BackColor = darkAccentBlue;
            stripConnection.BackColor = darkAccentBlue;
            stripBaudRate.BackColor = darkAccentBlue;

            settingRampSpeed.BackColor = darkBgWindow;
            settingMaxPressure.BackColor = darkBgWindow;
            settingUnits.BackColor = darkBgWindow;
            settingConnection.BackColor = darkBgWindow;
            settingBaudRate.BackColor = darkBgWindow;

            lblRampSpeedLabel.ForeColor = darkTextPrimary;
            lblMaxPressureLabel.ForeColor = darkTextPrimary;
            lblUnitsLabel.ForeColor = darkTextPrimary;
            lblConnectionLabel.ForeColor = darkTextPrimary;
            lblBaudRateLabel.ForeColor = darkTextPrimary;

            lblRampSpeedValue.ForeColor = darkAccentBlue;
            lblMaxPressureValue.ForeColor = darkAccentBlue;
            lblUnitsValue.ForeColor = darkAccentBlue;
            lblConnectionValue.ForeColor = darkAccentBlue;
            lblBaudRateValue.ForeColor = darkAccentBlue;

            panelStatusInfo.BackColor = Color.FromArgb(28, 40, 56);
            lblStatusInfoTitle.ForeColor = Color.FromArgb(0, 168, 224);
            lblStatusInfoText.ForeColor = darkTextSecondary;

            btnPurge.BackColor = darkBgWindow;
            btnPurge.ForeColor = darkTextPrimary;
            btnGraph.BackColor = darkBgWindow;
            btnGraph.ForeColor = darkTextPrimary;
            btnTable.BackColor = darkBgWindow;
            btnTable.ForeColor = darkTextPrimary;
            btnTerminal.BackColor = darkBgWindow;
            btnTerminal.ForeColor = darkTextPrimary;
        }
    }
}
