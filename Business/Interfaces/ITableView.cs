namespace PrecisionPressureController.Business.Interfaces
{
    /// <summary>
    /// Интерфейс для DataTableWindow - позволяет Presenter работать с таблицей без зависимости от UI
    /// </summary>
    public interface ITableView
    {
        /// <summary>
        /// Устанавливает информацию о подключении
        /// </summary>
        void SetConnectionInfo(string? portName, int? baudRate);

        /// <summary>
        /// Применяет тему оформления
        /// </summary>
        void ApplyTheme(bool isDark);

        /// <summary>
        /// Проверяет, уничтожена ли форма
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Позиция при запуске
        /// </summary>
        System.Windows.Forms.FormStartPosition StartPosition { get; set; }

        /// <summary>
        /// Состояние окна
        /// </summary>
        System.Windows.Forms.FormWindowState WindowState { get; set; }

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
