using System;
using System.Windows.Forms;
using PrecisionPressureController.UI.Main;

namespace PrecisionPressureController
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Стандартная инициализация WinForms (.NET 6/7/8)
                ApplicationConfiguration.Initialize();

                // Запускаем главное окно приложения
                var mainWindow = new MainWindow();
                mainWindow.WindowState = FormWindowState.Normal;
                mainWindow.Show();
                Application.Run(mainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при запуске приложения:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Критическая ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
