using System;
using System.Windows.Forms;

namespace Alicat
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Стандартная инициализация WinForms (.NET 6/7/8)
            ApplicationConfiguration.Initialize();

            // Запускаем твоё главное окно
            Application.Run(new AlicatForm());
        }
    }
}
