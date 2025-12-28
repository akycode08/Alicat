namespace Alicat.Business.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с последовательным портом (RS-232).
    /// Позволяет тестировать логику без реального устройства.
    /// </summary>
    public interface ISerialClient
    {
        /// <summary>
        /// Событие: подключение установлено
        /// </summary>
        event EventHandler? Connected;

        /// <summary>
        /// Событие: подключение разорвано
        /// </summary>
        event EventHandler? Disconnected;

        /// <summary>
        /// Событие: получена строка от устройства
        /// </summary>
        event EventHandler<string>? LineReceived;

        /// <summary>
        /// Подключиться к устройству
        /// </summary>
        void Attach();

        /// <summary>
        /// Отправить команду устройству
        /// </summary>
        void Send(string command);

        /// <summary>
        /// Освободить ресурсы
        /// </summary>
        void Dispose();
    }
}

