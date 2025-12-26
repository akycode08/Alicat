// ----------------------------------------------------------------------------
// Файл: AlicatForm.Designer.cs (ФИНАЛЬНАЯ ВЕРСИЯ)
// Все исправлено: System Settings labels видны, точные пропорции
// ----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat
{
    partial class AlicatForm
    {
        private System.ComponentModel.IContainer components = null;

        // ========== MAIN LAYOUT ==========
        private TableLayoutPanel rootLayout;

        // ========== MENU BAR ==========
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

        // ========== STATUS BAR ==========
        private Panel panelStatusBar;
        private Label lblConnectionStatus;
        private Label lblStatusDot;
        private Label lblBaudRate;
        private Label lblLastUpdate;
        private PictureBox picLogo;

        // ========== TOOLBAR ==========
        private Panel panelToolbar;
        private Label lblToolbarControl;
        private Button btnPurge;
        private Label lblToolbarWindows;
        private Button btnGraph;
        private Button btnTable;
        private Button btnTerminal;

        // ========== CONTENT ==========
        private TableLayoutPanel layoutContent;
        private Panel panelLeft;
        private Panel panelRight;

        // ========== LEFT: SECTIONS ==========
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

        // Set Target
        private Panel sectionSetTarget;
        private Label lblSetTargetTitle;
        private TextBox txtTargetInput;
        private Label lblTargetInputUnit;
        private Button btnGoToTarget;

        // Pressure Control
        private Panel sectionPressureControl;
        private Label lblPressureControlTitle;
        private Label lblIncrementLabel;
        private Button btnIncrementMinus;
        private TextBox txtIncrement;
        private Button btnIncrementPlus;
        private Label lblIncrementUnit;
        private Label lblAdjustPressureLabel;
        private Button btnIncrease;
        private Button btnDecrease;

        // ========== RIGHT ==========
        private TableLayoutPanel rightLayout;

        private Panel panelSystemSettings;
        private Label lblSystemSettingsTitle;

        private Panel settingRampSpeed;
        private Panel stripRampSpeed;
        private Label lblRampSpeedLabel;
        private Label lblRampSpeedValue;

        private Panel settingMaxPressure;
        private Panel stripMaxPressure;
        private Label lblMaxPressureLabel;
        private Label lblMaxPressureValue;

        private Panel settingUnits;
        private Panel stripUnits;
        private Label lblUnitsLabel;
        private Label lblUnitsValue;

        private Panel settingConnection;
        private Panel stripConnection;
        private Label lblConnectionLabel;
        private Label lblConnectionValue;

        private Panel settingBaudRate;
        private Panel stripBaudRate;
        private Label lblBaudRateLabel;
        private Label lblBaudRateValue;

        private Panel panelStatusInfo;
        private Label lblStatusInfoTitle;
        private Label lblStatusInfoText;

        // ========== THEME COLORS ==========
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
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            rootLayout = new TableLayoutPanel();

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

            panelStatusBar = new Panel();
            lblStatusDot = new Label();
            lblConnectionStatus = new Label();
            lblBaudRate = new Label();
            lblLastUpdate = new Label();
            picLogo = new PictureBox();

            panelToolbar = new Panel();
            lblToolbarControl = new Label();
            btnPurge = new Button();
            lblToolbarWindows = new Label();
            btnGraph = new Button();
            btnTable = new Button();
            btnTerminal = new Button();

            layoutContent = new TableLayoutPanel();
            panelLeft = new Panel();
            panelRight = new Panel();

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
            txtTargetInput = new TextBox();
            lblTargetInputUnit = new Label();
            btnGoToTarget = new Button();

            sectionPressureControl = new Panel();
            lblPressureControlTitle = new Label();
            lblIncrementLabel = new Label();
            btnIncrementMinus = new Button();
            txtIncrement = new TextBox();
            btnIncrementPlus = new Button();
            lblIncrementUnit = new Label();
            lblAdjustPressureLabel = new Label();
            btnIncrease = new Button();
            btnDecrease = new Button();

            rightLayout = new TableLayoutPanel();
            panelSystemSettings = new Panel();
            lblSystemSettingsTitle = new Label();

            panelStatusInfo = new Panel();
            lblStatusInfoTitle = new Label();
            lblStatusInfoText = new Label();

            // ✅ INITIALIZE SYSTEM SETTINGS LABELS (prevent null reference)
            lblRampSpeedLabel = new Label();
            lblRampSpeedValue = new Label();
            lblMaxPressureLabel = new Label();
            lblMaxPressureValue = new Label();
            lblUnitsLabel = new Label();
            lblUnitsValue = new Label();
            lblConnectionLabel = new Label();
            lblConnectionValue = new Label();
            lblBaudRateLabel = new Label();
            lblBaudRateValue = new Label();
            stripRampSpeed = new Panel();
            stripMaxPressure = new Panel();
            stripUnits = new Panel();
            stripConnection = new Panel();
            stripBaudRate = new Panel();

            menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
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

            menuFileTestMode.Name = "menuFileTestMode";
            menuFileTestMode.Text = "Start Test Mode";

            menuFile.DropDownItems.AddRange(new ToolStripItem[] {
                menuFileNewSession,
                menuFileTestMode
            });

            menuSettings.Name = "menuSettings";
            menuSettings.Text = "Settings";

            menuSettingsOptions.Name = "menuSettingsOptions";
            menuSettingsOptions.Text = "Options";

            menuSettingsCommunication.Name = "menuSettingsCommunication";
            menuSettingsCommunication.Text = "Communication";

            menuSettings.DropDownItems.AddRange(new ToolStripItem[] {
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
            menuViewDarkTheme.Click += MenuViewDarkTheme_Click;

            menuView.DropDownItems.AddRange(new ToolStripItem[] {
                menuViewLightTheme,
                menuViewDarkTheme
            });

            menuHelp.Name = "menuHelp";
            menuHelp.Text = "Help";

            toolStripVersion.Name = "toolStripVersion";
            toolStripVersion.Text = "version 1.2.0";
            toolStripVersion.Alignment = ToolStripItemAlignment.Right;
            toolStripVersion.ForeColor = Color.Gray;

            menuMain.Items.AddRange(new ToolStripItem[] {
                menuFile,
                menuSettings,
                menuView,
                menuHelp,
                toolStripVersion
            });

            // ====================================================================
            // ROOT LAYOUT
            // ====================================================================
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowCount = 3;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = new Padding(0);
            rootLayout.Padding = new Padding(0);

            rootLayout.Controls.Add(panelStatusBar, 0, 0);
            rootLayout.Controls.Add(panelToolbar, 0, 1);
            rootLayout.Controls.Add(layoutContent, 0, 2);

            // ====================================================================
            // STATUS BAR
            // ====================================================================
            panelStatusBar.Dock = DockStyle.Fill;
            panelStatusBar.Padding = new Padding(16, 0, 16, 0);
            panelStatusBar.Margin = new Padding(0);

            var statusLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var statusConnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            lblStatusDot.Text = "●";
            lblStatusDot.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatusDot.AutoSize = true;
            lblStatusDot.Margin = new Padding(0, 14, 8, 0);

            lblConnectionStatus.Text = "Connected (COM3)";
            lblConnectionStatus.Font = new Font("Segoe UI", 9F);
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Margin = new Padding(0, 16, 0, 0);
            lblConnectionStatus.MinimumSize = new Size(110, 0);

            statusConnPanel.Controls.Add(lblStatusDot);
            statusConnPanel.Controls.Add(lblConnectionStatus);

            lblBaudRate.Text = "Baud: 19200";
            lblBaudRate.Font = new Font("Segoe UI", 9F);
            lblBaudRate.AutoSize = true;
            lblBaudRate.Margin = new Padding(0, 16, 0, 0);

            lblLastUpdate.Text = "Last update: 0.5s ago";
            lblLastUpdate.Font = new Font("Segoe UI", 9F);
            lblLastUpdate.AutoSize = true;
            lblLastUpdate.Margin = new Padding(0, 16, 0, 0);

            picLogo.Width = 180;
            picLogo.Height = 45;
            picLogo.Anchor = AnchorStyles.Right;
            picLogo.Margin = new Padding(0, 2, 0, 0);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;

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
            panelToolbar.Padding = new Padding(16, 0, 16, 0);
            panelToolbar.Margin = new Padding(0);

            var toolbarFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0, 5, 0, 5)
            };

            lblToolbarControl.Text = "Control:";
            lblToolbarControl.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblToolbarControl.AutoSize = true;
            lblToolbarControl.Margin = new Padding(0, 7, 8, 0);

            btnPurge.Text = "🧪 Purge";
            btnPurge.AutoSize = true;
            btnPurge.Margin = new Padding(0, 0, 22, 0);
            btnPurge.Padding = new Padding(10, 6, 10, 6);
            btnPurge.FlatStyle = FlatStyle.Flat;

            lblToolbarWindows.Text = "Windows:";
            lblToolbarWindows.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblToolbarWindows.AutoSize = true;
            lblToolbarWindows.Margin = new Padding(0, 7, 10, 0);

            btnGraph.Text = "📊 Graph";
            btnGraph.AutoSize = true;
            btnGraph.Margin = new Padding(0, 0, 6, 0);
            btnGraph.Padding = new Padding(10, 6, 10, 6);
            btnGraph.FlatStyle = FlatStyle.Flat;

            btnTable.Text = "📋 Table";
            btnTable.AutoSize = true;
            btnTable.Margin = new Padding(0, 0, 6, 0);
            btnTable.Padding = new Padding(10, 6, 10, 6);
            btnTable.FlatStyle = FlatStyle.Flat;

            btnTerminal.Text = "💻 Terminal";
            btnTerminal.AutoSize = true;
            btnTerminal.Margin = new Padding(0);
            btnTerminal.Padding = new Padding(10, 6, 10, 6);
            btnTerminal.FlatStyle = FlatStyle.Flat;

            toolbarFlow.Controls.Add(lblToolbarControl);
            toolbarFlow.Controls.Add(btnPurge);
            toolbarFlow.Controls.Add(lblToolbarWindows);
            toolbarFlow.Controls.Add(btnGraph);
            toolbarFlow.Controls.Add(btnTable);
            toolbarFlow.Controls.Add(btnTerminal);

            panelToolbar.Controls.Add(toolbarFlow);

            // ====================================================================
            // CONTENT LAYOUT
            // ====================================================================
            layoutContent.ColumnCount = 3;
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 840F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            layoutContent.RowCount = 1;
            layoutContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layoutContent.Dock = DockStyle.Fill;
            layoutContent.Padding = new Padding(20);
            layoutContent.Margin = new Padding(0);

            layoutContent.Controls.Add(panelLeft, 0, 0);
            layoutContent.Controls.Add(new Panel(), 1, 0);
            layoutContent.Controls.Add(panelRight, 2, 0);

            // ====================================================================
            // LEFT PANEL
            // ====================================================================
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Margin = new Padding(0);
            panelLeft.Padding = new Padding(0);

            leftLayout.Dock = DockStyle.Fill;
            leftLayout.Margin = new Padding(0);
            leftLayout.Padding = new Padding(0);
            leftLayout.ColumnCount = 1;
            leftLayout.RowCount = 5;
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 240F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            panelLeft.Controls.Add(leftLayout);

            // Pressure Cards
            layoutPressureCards.Dock = DockStyle.Fill;
            layoutPressureCards.Margin = new Padding(0);
            layoutPressureCards.Padding = new Padding(0);
            layoutPressureCards.ColumnCount = 3;
            layoutPressureCards.RowCount = 1;
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410F));
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layoutPressureCards.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 410F));
            layoutPressureCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Current Card
            cardCurrent.Dock = DockStyle.Fill;
            cardCurrent.Margin = new Padding(0);
            cardCurrent.Padding = new Padding(24);
            cardCurrent.BorderStyle = BorderStyle.FixedSingle;

            var currentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 1,
                RowCount = 4
            };
            currentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            currentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            currentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            currentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblCurrentTitle.Text = "CURRENT PRESSURE";
            lblCurrentTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCurrentTitle.Dock = DockStyle.Fill;
            lblCurrentTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentTitle.Margin = new Padding(0, 0, 0, 6);

            lblCurrentValue.Text = "100.0";
            lblCurrentValue.Font = new Font("Segoe UI", 56F, FontStyle.Bold);
            lblCurrentValue.Dock = DockStyle.Fill;
            lblCurrentValue.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentValue.Margin = new Padding(0);

            lblCurrentUnit.Text = "PSIG";
            lblCurrentUnit.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCurrentUnit.Dock = DockStyle.Fill;
            lblCurrentUnit.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentUnit.Margin = new Padding(0, 0, 0, 6);

            lblCurrentRate.Text = "→ 0.0 /s";
            lblCurrentRate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCurrentRate.Dock = DockStyle.Fill;
            lblCurrentRate.TextAlign = ContentAlignment.MiddleCenter;
            lblCurrentRate.Margin = new Padding(0);

            currentLayout.Controls.Add(lblCurrentTitle, 0, 0);
            currentLayout.Controls.Add(lblCurrentValue, 0, 1);
            currentLayout.Controls.Add(lblCurrentUnit, 0, 2);
            currentLayout.Controls.Add(lblCurrentRate, 0, 3);

            cardCurrent.Controls.Add(currentLayout);

            // Target Card
            cardTarget.Dock = DockStyle.Fill;
            cardTarget.Margin = new Padding(0);
            cardTarget.Padding = new Padding(24);
            cardTarget.BorderStyle = BorderStyle.FixedSingle;

            var targetLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 1,
                RowCount = 4
            };
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblTargetTitle.Text = "TARGET PRESSURE";
            lblTargetTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTargetTitle.Dock = DockStyle.Fill;
            lblTargetTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetTitle.Margin = new Padding(0, 0, 0, 6);

            lblTargetValue.Text = "100.0";
            lblTargetValue.Font = new Font("Segoe UI", 56F, FontStyle.Bold);
            lblTargetValue.Dock = DockStyle.Fill;
            lblTargetValue.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetValue.Margin = new Padding(0);

            lblTargetUnit.Text = "PSIG";
            lblTargetUnit.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTargetUnit.Dock = DockStyle.Fill;
            lblTargetUnit.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetUnit.Margin = new Padding(0, 0, 0, 6);

            lblTargetStatus.Text = "At target";
            lblTargetStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTargetStatus.Dock = DockStyle.Fill;
            lblTargetStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblTargetStatus.Margin = new Padding(0);

            targetLayout.Controls.Add(lblTargetTitle, 0, 0);
            targetLayout.Controls.Add(lblTargetValue, 0, 1);
            targetLayout.Controls.Add(lblTargetUnit, 0, 2);
            targetLayout.Controls.Add(lblTargetStatus, 0, 3);

            cardTarget.Controls.Add(targetLayout);

            layoutPressureCards.Controls.Add(cardCurrent, 0, 0);
            layoutPressureCards.Controls.Add(new Panel(), 1, 0);
            layoutPressureCards.Controls.Add(cardTarget, 2, 0);

            // Set Target Section
            sectionSetTarget.Dock = DockStyle.Fill;
            sectionSetTarget.Margin = new Padding(0);
            sectionSetTarget.Padding = new Padding(18);
            sectionSetTarget.BorderStyle = BorderStyle.FixedSingle;

            var setTargetLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 1,
                RowCount = 2
            };
            setTargetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            setTargetLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblSetTargetTitle.Text = "Set Target Pressure";
            lblSetTargetTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSetTargetTitle.Dock = DockStyle.Fill;
            lblSetTargetTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblSetTargetTitle.Margin = new Padding(0, 0, 0, 10);

            var setTargetRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 4,
                RowCount = 1
            };
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            setTargetRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));

            txtTargetInput.Text = "100.0";
            txtTargetInput.Font = new Font("Courier New", 12F);
            txtTargetInput.Dock = DockStyle.Fill;
            txtTargetInput.Margin = new Padding(0);

            lblTargetInputUnit.Text = "PSIG";
            lblTargetInputUnit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTargetInputUnit.Dock = DockStyle.Fill;
            lblTargetInputUnit.TextAlign = ContentAlignment.MiddleLeft;
            lblTargetInputUnit.Margin = new Padding(8, 0, 0, 0);
            lblTargetInputUnit.AutoSize = false;

            btnGoToTarget.Text = "🎯 Go to Target";
            btnGoToTarget.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGoToTarget.Dock = DockStyle.Fill;
            btnGoToTarget.Margin = new Padding(0);
            btnGoToTarget.FlatStyle = FlatStyle.Flat;

            setTargetRow.Controls.Add(txtTargetInput, 0, 0);
            setTargetRow.Controls.Add(lblTargetInputUnit, 1, 0);
            setTargetRow.Controls.Add(new Panel(), 2, 0);
            setTargetRow.Controls.Add(btnGoToTarget, 3, 0);

            setTargetLayout.Controls.Add(lblSetTargetTitle, 0, 0);
            setTargetLayout.Controls.Add(setTargetRow, 0, 1);

            sectionSetTarget.Controls.Add(setTargetLayout);

            // Pressure Control Section
            sectionPressureControl.Dock = DockStyle.Fill;
            sectionPressureControl.Margin = new Padding(0);
            sectionPressureControl.Padding = new Padding(18);
            sectionPressureControl.BorderStyle = BorderStyle.FixedSingle;

            var pressureControlLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 1,
                RowCount = 6
            };
            pressureControlLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            pressureControlLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            pressureControlLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            pressureControlLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            pressureControlLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            pressureControlLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));

            lblPressureControlTitle.Text = "Pressure Control";
            lblPressureControlTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPressureControlTitle.Dock = DockStyle.Fill;
            lblPressureControlTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblPressureControlTitle.Margin = new Padding(0);

            var incrementRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 6,
                RowCount = 1
            };
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            incrementRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            lblIncrementLabel.Text = "Increment:";
            lblIncrementLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblIncrementLabel.Dock = DockStyle.Fill;
            lblIncrementLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblIncrementLabel.Margin = new Padding(0);

            btnIncrementMinus.Text = "−";
            btnIncrementMinus.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnIncrementMinus.Dock = DockStyle.Fill;
            btnIncrementMinus.Margin = new Padding(0, 0, 10, 0);
            btnIncrementMinus.FlatStyle = FlatStyle.Flat;
            btnIncrementMinus.TextAlign = ContentAlignment.MiddleCenter;

            txtIncrement.Text = "5.0";
            txtIncrement.Font = new Font("Courier New", 12F, FontStyle.Bold);
            txtIncrement.Dock = DockStyle.Fill;
            txtIncrement.TextAlign = HorizontalAlignment.Center;
            txtIncrement.Margin = new Padding(0, 0, 10, 0);

            btnIncrementPlus.Text = "+";
            btnIncrementPlus.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnIncrementPlus.Dock = DockStyle.Fill;
            btnIncrementPlus.Margin = new Padding(0, 0, 10, 0);
            btnIncrementPlus.FlatStyle = FlatStyle.Flat;
            btnIncrementPlus.TextAlign = ContentAlignment.MiddleCenter;

            lblIncrementUnit.Text = "PSIG";
            lblIncrementUnit.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblIncrementUnit.Dock = DockStyle.Fill;
            lblIncrementUnit.TextAlign = ContentAlignment.MiddleLeft;
            lblIncrementUnit.Margin = new Padding(0);
            lblIncrementUnit.AutoSize = false;

            incrementRow.Controls.Add(lblIncrementLabel, 0, 0);
            incrementRow.Controls.Add(btnIncrementMinus, 1, 0);
            incrementRow.Controls.Add(txtIncrement, 2, 0);
            incrementRow.Controls.Add(btnIncrementPlus, 3, 0);
            incrementRow.Controls.Add(lblIncrementUnit, 4, 0);
            incrementRow.Controls.Add(new Panel(), 5, 0);

            lblAdjustPressureLabel.Text = "Adjust Pressure:";
            lblAdjustPressureLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblAdjustPressureLabel.Dock = DockStyle.Fill;
            lblAdjustPressureLabel.TextAlign = ContentAlignment.MiddleLeft;
            lblAdjustPressureLabel.Margin = new Padding(0, 10, 0, 0);

            btnIncrease.Text = "▲  Increase (+5.0 PSIG)";
            btnIncrease.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIncrease.Dock = DockStyle.Fill;
            btnIncrease.Margin = new Padding(0, 8, 0, 8);
            btnIncrease.FlatStyle = FlatStyle.Flat;

            btnDecrease.Text = "▼  Decrease (-5.0 PSIG)";
            btnDecrease.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDecrease.Dock = DockStyle.Fill;
            btnDecrease.Margin = new Padding(0);
            btnDecrease.FlatStyle = FlatStyle.Flat;

            pressureControlLayout.Controls.Add(lblPressureControlTitle, 0, 0);
            pressureControlLayout.Controls.Add(new Panel(), 0, 1);
            pressureControlLayout.Controls.Add(incrementRow, 0, 2);
            pressureControlLayout.Controls.Add(lblAdjustPressureLabel, 0, 3);
            pressureControlLayout.Controls.Add(btnIncrease, 0, 4);
            pressureControlLayout.Controls.Add(btnDecrease, 0, 5);

            sectionPressureControl.Controls.Add(pressureControlLayout);

            leftLayout.Controls.Add(layoutPressureCards, 0, 0);
            leftLayout.Controls.Add(new Panel(), 0, 1);
            leftLayout.Controls.Add(sectionSetTarget, 0, 2);
            leftLayout.Controls.Add(new Panel(), 0, 3);
            leftLayout.Controls.Add(sectionPressureControl, 0, 4);

            // ====================================================================
            // RIGHT PANEL
            // ====================================================================
            panelRight.Dock = DockStyle.Fill;
            panelRight.Margin = new Padding(0);
            panelRight.Padding = new Padding(0);

            rightLayout.Dock = DockStyle.Fill;
            rightLayout.Margin = new Padding(0);
            rightLayout.Padding = new Padding(0);
            rightLayout.ColumnCount = 1;
            rightLayout.RowCount = 3;
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            panelRight.Controls.Add(rightLayout);

            // System Settings Panel
            panelSystemSettings.Dock = DockStyle.Fill;
            panelSystemSettings.Margin = new Padding(0);
            panelSystemSettings.Padding = new Padding(16);
            panelSystemSettings.BorderStyle = BorderStyle.FixedSingle;

            lblSystemSettingsTitle.Text = "System Settings";
            lblSystemSettingsTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSystemSettingsTitle.Dock = DockStyle.Top;
            lblSystemSettingsTitle.Height = 28;
            lblSystemSettingsTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblSystemSettingsTitle.Margin = new Padding(0, 0, 0, 10);

            var settingsStack = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                ColumnCount = 1,
                RowCount = 6
            };
            settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            settingsStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            // ✅ CREATE SETTINGS ITEMS (using pre-initialized labels)
            settingRampSpeed = CreateSettingItem(stripRampSpeed, lblRampSpeedLabel, lblRampSpeedValue, "Ramp Speed", "10 PSIG/s");
            settingMaxPressure = CreateSettingItem(stripMaxPressure, lblMaxPressureLabel, lblMaxPressureValue, "Max Pressure", "140 PSIG");
            settingUnits = CreateSettingItem(stripUnits, lblUnitsLabel, lblUnitsValue, "Units", "PSIG");
            settingConnection = CreateSettingItem(stripConnection, lblConnectionLabel, lblConnectionValue, "Connection", "COM3");
            settingBaudRate = CreateSettingItem(stripBaudRate, lblBaudRateLabel, lblBaudRateValue, "Baud Rate", "19200");

            settingsStack.Controls.Add(new Panel(), 0, 0);
            settingsStack.Controls.Add(settingRampSpeed, 0, 1);
            settingsStack.Controls.Add(settingMaxPressure, 0, 2);
            settingsStack.Controls.Add(settingUnits, 0, 3);
            settingsStack.Controls.Add(settingConnection, 0, 4);
            settingsStack.Controls.Add(settingBaudRate, 0, 5);

            panelSystemSettings.Controls.Add(settingsStack);
            panelSystemSettings.Controls.Add(lblSystemSettingsTitle);

            // Status Info Panel
            panelStatusInfo.Dock = DockStyle.Fill;
            panelStatusInfo.Margin = new Padding(0);
            panelStatusInfo.Padding = new Padding(16);
            panelStatusInfo.BorderStyle = BorderStyle.FixedSingle;

            lblStatusInfoTitle.Text = "ℹ️ Status Information";
            lblStatusInfoTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatusInfoTitle.Dock = DockStyle.Top;
            lblStatusInfoTitle.Height = 26;
            lblStatusInfoTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblStatusInfoTitle.Margin = new Padding(0, 0, 0, 8);

            lblStatusInfoText.Text = "• Controller is running\n• Pressure increasing\n• Systems operational\n• Last update: 0.5s ago";
            lblStatusInfoText.Font = new Font("Segoe UI", 9F);
            lblStatusInfoText.Dock = DockStyle.Fill;
            lblStatusInfoText.AutoSize = false;
            lblStatusInfoText.TextAlign = ContentAlignment.TopLeft;

            panelStatusInfo.Controls.Add(lblStatusInfoText);
            panelStatusInfo.Controls.Add(lblStatusInfoTitle);

            rightLayout.Controls.Add(panelSystemSettings, 0, 0);
            rightLayout.Controls.Add(new Panel(), 0, 1);
            rightLayout.Controls.Add(panelStatusInfo, 0, 2);

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

            Controls.Add(rootLayout);
            Controls.Add(menuMain);
            MainMenuStrip = menuMain;

            ApplyLightTheme();

            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // ====================================================================
        // HELPER: CREATE SETTING ITEM
        // ====================================================================
        private Panel CreateSettingItem(Panel strip, Label labelCtrl, Label valueCtrl, string labelText, string valueText)
        {
            var host = new Panel();
            host.Dock = DockStyle.Fill;
            host.Margin = new Padding(0, 0, 0, 10);
            host.Padding = new Padding(0);
            host.BorderStyle = BorderStyle.FixedSingle;

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.Margin = new Padding(0);
            layout.Padding = new Padding(0);
            layout.ColumnCount = 3;
            layout.RowCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            strip.Dock = DockStyle.Fill;
            strip.Margin = new Padding(0);

            labelCtrl.Text = labelText;
            labelCtrl.Dock = DockStyle.Fill;
            labelCtrl.TextAlign = ContentAlignment.MiddleLeft;
            labelCtrl.Margin = new Padding(12, 0, 0, 0);
            labelCtrl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            labelCtrl.AutoSize = false;

            valueCtrl.Text = valueText;
            valueCtrl.Dock = DockStyle.Fill;
            valueCtrl.TextAlign = ContentAlignment.MiddleRight;
            valueCtrl.Margin = new Padding(0, 0, 12, 0);
            valueCtrl.Font = new Font("Courier New", 10F, FontStyle.Bold);
            valueCtrl.AutoSize = false;

            layout.Controls.Add(strip, 0, 0);
            layout.Controls.Add(labelCtrl, 1, 0);
            layout.Controls.Add(valueCtrl, 2, 0);

            host.Controls.Add(layout);

            return host;
        }

        // ====================================================================
        // THEME METHODS
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
            btnGoToTarget.FlatAppearance.BorderColor = lightAccentGreen;

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
            btnGoToTarget.FlatAppearance.BorderColor = darkAccentGreen;

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