using System;
using System.Windows.Forms;

namespace Alicat
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

                // Запускаем твоё главное окно
                var form = new AlicatForm();
                form.WindowState = FormWindowState.Normal;
                form.Show();
                Application.Run(form);
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
