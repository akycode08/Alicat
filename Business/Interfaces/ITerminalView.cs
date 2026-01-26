using System;

namespace PrecisionPressureController.Business.Interfaces
{
    /// <summary>
    /// Интерфейс для TerminalWindow - позволяет Presenter работать с терминалом без зависимости от UI
    /// </summary>
    public interface ITerminalView
    {
        /// <summary>
        /// Добавляет строку в лог терминала
        /// </summary>
        void AppendLog(string line);

        /// <summary>
        /// Событие отправки команды
        /// </summary>
        event Action<string>? CommandSent;

        /// <summary>
        /// Проверяет, уничтожена ли форма
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Видимость формы
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Показать форму
        /// </summary>
        void Show(System.Windows.Forms.IWin32Window? owner);

        /// <summary>
        /// Установить фокус на форму
        /// </summary>
        void Focus();
    }
}
