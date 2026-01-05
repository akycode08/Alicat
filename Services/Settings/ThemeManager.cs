using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alicat.Services.Settings
{
    /// <summary>
    /// Управление темами приложения (Light/Dark)
    /// </summary>
    public static class ThemeManager
    {
        // ====================================================================
        // ЦВЕТОВАЯ ПАЛИТРА (Light Theme)
        // ====================================================================
        public static class LightTheme
        {
            public static readonly Color Background = Color.FromArgb(240, 240, 240); // #F0F0F0
            public static readonly Color Foreground = Color.FromArgb(0, 0, 0);        // #000000
            public static readonly Color MenuStrip = Color.FromArgb(245, 245, 245);   // #F5F5F5
            public static readonly Color TextBox = Color.FromArgb(255, 255, 255);     // #FFFFFF
            public static readonly Color Border = Color.FromArgb(204, 204, 204);
        }

        // ====================================================================
        // ЦВЕТОВАЯ ПАЛИТРА (Dark Theme)
        // ====================================================================
        public static class DarkTheme
        {
            public static readonly Color Background = Color.FromArgb(30, 30, 30);     // #1E1E1E
            public static readonly Color Foreground = Color.FromArgb(204, 204, 204);  // #CCCCCC
            public static readonly Color MenuStrip = Color.FromArgb(45, 45, 48);      // #2D2D30
            public static readonly Color TextBox = Color.FromArgb(37, 37, 38);        // #252526
            public static readonly Color Border = Color.FromArgb(54, 54, 72);
        }

        private static bool _isDarkMode = false;

        /// <summary>
        /// Получить текущую тему (true = Dark, false = Light)
        /// </summary>
        public static bool IsDarkMode => _isDarkMode;

        /// <summary>
        /// Переключить тему
        /// </summary>
        public static void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
            OnThemeChanged?.Invoke(_isDarkMode);
        }

        /// <summary>
        /// Установить тему
        /// </summary>
        public static void SetTheme(bool isDark)
        {
            if (_isDarkMode != isDark)
            {
                _isDarkMode = isDark;
                OnThemeChanged?.Invoke(_isDarkMode);
            }
        }

        /// <summary>
        /// Событие изменения темы
        /// </summary>
        public static event Action<bool>? OnThemeChanged;

        /// <summary>
        /// Применить тему к форме
        /// </summary>
        public static void ApplyTheme(Form form, bool isDark)
        {
            if (form == null) return;

            if (isDark)
            {
                form.BackColor = DarkTheme.Background;
                form.ForeColor = DarkTheme.Foreground;
            }
            else
            {
                form.BackColor = LightTheme.Background;
                form.ForeColor = LightTheme.Foreground;
            }
        }

        /// <summary>
        /// Применить тему к MenuStrip
        /// </summary>
        public static void ApplyTheme(MenuStrip menuStrip, bool isDark)
        {
            if (menuStrip == null) return;

            if (isDark)
            {
                menuStrip.BackColor = DarkTheme.MenuStrip;
                menuStrip.ForeColor = DarkTheme.Foreground;
            }
            else
            {
                menuStrip.BackColor = LightTheme.MenuStrip;
                menuStrip.ForeColor = LightTheme.Foreground;
            }
        }

        /// <summary>
        /// Применить тему к TextBox
        /// </summary>
        public static void ApplyTheme(TextBox textBox, bool isDark)
        {
            if (textBox == null) return;

            if (isDark)
            {
                textBox.BackColor = DarkTheme.TextBox;
                textBox.ForeColor = DarkTheme.Foreground;
            }
            else
            {
                textBox.BackColor = LightTheme.TextBox;
                textBox.ForeColor = LightTheme.Foreground;
            }
        }
    }
}

