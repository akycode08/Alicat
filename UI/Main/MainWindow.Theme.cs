using System.Drawing;
using System.Windows.Forms;
using PrecisionPressureController.Presentation.Presenters;

namespace PrecisionPressureController.UI.Main
{
    /// <summary>
    /// Стили и темы для MainWindow.
    /// Отдельный файл для цветов, тем и анимаций (как CSS в веб-разработке).
    /// </summary>
    public partial class MainWindow
    {
        // ====================================================================
        // ЦВЕТОВАЯ ПАЛИТРА (Light Theme)
        // ====================================================================
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
        private readonly Color lightStatusDotDisconnected = Color.FromArgb(244, 67, 54); // Красный для отключенного состояния
        private readonly Color lightValueDisconnected = Color.FromArgb(244, 67, 54); // Красный для значений при отключении

        // ====================================================================
        // ЦВЕТОВАЯ ПАЛИТРА (Dark Theme)
        // ====================================================================
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
        private readonly Color darkStatusDotDisconnected = Color.FromArgb(255, 82, 82); // Красный для отключенного состояния (темная тема)
        private readonly Color darkValueDisconnected = Color.FromArgb(255, 82, 82); // Красный для значений при отключении (темная тема)

        // ====================================================================
        // ОБРАБОТЧИКИ МЕНЮ (View -> Light/Dark Theme Toggle)
        // ====================================================================
        private void MenuViewThemeToggle_Click(object sender, EventArgs e)
        {
            // Переключаем тему
            if (isDarkTheme)
            {
                // Переключаемся на Light
                ApplyLightTheme();
                
                // Update ChartWindow theme if open
                var graphForm = ((IMainView)this).GraphView;
                if (graphForm != null && !graphForm.IsDisposed)
                {
                    graphForm.ApplyTheme(false);
                }
                
                // Update DataTableWindow theme if open
                var tableForm = ((IMainView)this).TableView;
                if (tableForm != null && !tableForm.IsDisposed)
                {
                    tableForm.ApplyTheme(false);
                }
            }
            else
            {
                // Переключаемся на Dark
                ApplyDarkTheme();
                
                // Update ChartWindow theme if open
                var graphForm = ((IMainView)this).GraphView;
                if (graphForm != null && !graphForm.IsDisposed)
                {
                    graphForm.ApplyTheme(true);
                }
                
                // Update DataTableWindow theme if open
                var tableForm = ((IMainView)this).TableView;
                if (tableForm != null && !tableForm.IsDisposed)
                {
                    tableForm.ApplyTheme(true);
                }
            }
            
            // Обновляем текст меню: показываем, на ЧТО переключится (противоположная тема)
            UpdateThemeToggleMenuText();
        }
        
        /// <summary>
        /// Обновляет текст пункта меню Theme Toggle в зависимости от текущей темы
        /// </summary>
        private void UpdateThemeToggleMenuText()
        {
            // Если сейчас Dark, показываем "Light" (чтобы переключиться на Light)
            // Если сейчас Light, показываем "Dark" (чтобы переключиться на Dark)
            menuViewThemeToggle.Text = isDarkTheme ? "Light" : "Dark";
            menuViewThemeToggle.Checked = isDarkTheme;
        }

        // ====================================================================
        // ПРИМЕНЕНИЕ ТЕМ (Light Theme)
        // ====================================================================
        private void ApplyLightTheme()
        {
            isDarkTheme = false;
            UpdateThemeToggleMenuText();

            BackColor = lightBgPrimary;

            menuMain.BackColor = lightBgMenu;
            menuMain.ForeColor = lightTextPrimary;

            panelStatusBar.BackColor = lightBgStatus;
            // lblStatusDot.ForeColor устанавливается через UI_UpdateConnectionStatus, не здесь
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
            stripMaxIncrement.BackColor = lightAccentBlue;

            settingRampSpeed.BackColor = lightBgWindow;
            settingMaxPressure.BackColor = lightBgWindow;
            settingMaxIncrement.BackColor = lightBgWindow;

            lblRampSpeedLabel.ForeColor = lightTextPrimary;
            lblMaxPressureLabel.ForeColor = lightTextPrimary;
            lblMaxIncrementLabel.ForeColor = lightTextPrimary;

            lblRampSpeedValue.ForeColor = lightAccentBlue;
            lblMaxPressureValue.ForeColor = lightAccentBlue;
            lblMaxIncrementValue.ForeColor = lightAccentBlue;

            panelStatusInfo.BackColor = Color.FromArgb(227, 242, 253);
            lblStatusInfoTitle.ForeColor = Color.FromArgb(21, 101, 192);
            lblStatusInfoText.BackColor = Color.FromArgb(227, 242, 253);
            lblStatusInfoText.ForeColor = lightTextSecondary;

            btnPause.BackColor = lightBgWindow;
            btnPause.ForeColor = lightTextPrimary;
            btnPurge.BackColor = lightBgWindow;
            btnPurge.ForeColor = lightTextPrimary;
            btnGraph.BackColor = lightBgWindow;
            btnGraph.ForeColor = lightTextPrimary;
            btnTable.BackColor = lightBgWindow;
            btnTable.ForeColor = lightTextPrimary;
            btnTerminal.BackColor = lightBgWindow;
            btnTerminal.ForeColor = lightTextPrimary;
        }

        // ====================================================================
        // ПРИМЕНЕНИЕ ТЕМ (Dark Theme)
        // ====================================================================
        private void ApplyDarkTheme()
        {
            isDarkTheme = true;
            UpdateThemeToggleMenuText();

            BackColor = darkBgPrimary;

            menuMain.BackColor = darkBgMenu;
            menuMain.ForeColor = darkTextPrimary;

            panelStatusBar.BackColor = darkBgStatus;
            // lblStatusDot.ForeColor устанавливается через UI_UpdateConnectionStatus, не здесь
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
            stripMaxIncrement.BackColor = darkAccentBlue;

            settingRampSpeed.BackColor = darkBgWindow;
            settingMaxPressure.BackColor = darkBgWindow;
            settingMaxIncrement.BackColor = darkBgWindow;

            lblRampSpeedLabel.ForeColor = darkTextPrimary;
            lblMaxPressureLabel.ForeColor = darkTextPrimary;
            lblMaxIncrementLabel.ForeColor = darkTextPrimary;

            lblRampSpeedValue.ForeColor = darkAccentBlue;
            lblMaxPressureValue.ForeColor = darkAccentBlue;
            lblMaxIncrementValue.ForeColor = darkAccentBlue;

            panelStatusInfo.BackColor = Color.FromArgb(28, 40, 56);
            lblStatusInfoTitle.ForeColor = Color.FromArgb(0, 168, 224);
            lblStatusInfoText.BackColor = darkBgSection;
            lblStatusInfoText.ForeColor = darkTextSecondary;

            btnPause.BackColor = darkBgWindow;
            btnPause.ForeColor = darkTextPrimary;
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