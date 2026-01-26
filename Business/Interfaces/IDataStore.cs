using PrecisionPressureController.Services.Data;

namespace PrecisionPressureController.Business.Interfaces
{
    /// <summary>
    /// Интерфейс для хранения данных сессии.
    /// Позволяет тестировать логику без реальной записи в файл.
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Все точки данных (только чтение)
        /// </summary>
        IReadOnlyList<DataPoint> Points { get; }

        /// <summary>
        /// Сессия запущена?
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Время начала сессии
        /// </summary>
        DateTime SessionStart { get; }

        /// <summary>
        /// Путь к CSV файлу (null если не записывается)
        /// </summary>
        string? CsvPath { get; }

        /// <summary>
        /// Событие: новая точка добавлена
        /// </summary>
        event Action<DataPoint>? OnNewPoint;

        /// <summary>
        /// Событие: сессия началась
        /// </summary>
        event Action? OnSessionStarted;

        /// <summary>
        /// Событие: сессия завершена
        /// </summary>
        event Action? OnSessionEnded;

        /// <summary>
        /// Начать сессию (только в RAM)
        /// </summary>
        void StartSession();

        /// <summary>
        /// Начать сессию с записью в CSV
        /// </summary>
        void StartSession(string csvFilePath);

        /// <summary>
        /// Записать точку данных
        /// </summary>
        void RecordSample(double current, double target, string unit);

        /// <summary>
        /// Записать точку данных с дополнительными параметрами
        /// </summary>
        void RecordSample(double current, double target, string unit, double rampSpeed, int pollingFrequency);

        /// <summary>
        /// Записать точку данных с дополнительными параметрами и PointIndex
        /// </summary>
        void RecordSample(double current, double target, string unit, double rampSpeed, int pollingFrequency, int pointIndex);

        /// <summary>
        /// Записать событие
        /// </summary>
        void RecordEvent(double current, double target, string unit, string eventType);

        /// <summary>
        /// Записать событие с дополнительными параметрами
        /// </summary>
        void RecordEvent(double current, double target, string unit, string eventType, double rampSpeed, int pollingFrequency);

        /// <summary>
        /// Записать событие с дополнительными параметрами и PointIndex
        /// </summary>
        void RecordEvent(double current, double target, string unit, string eventType, double rampSpeed, int pollingFrequency, int pointIndex);

        /// <summary>
        /// Завершить сессию
        /// </summary>
        void EndSession();

        /// <summary>
        /// Очистить данные
        /// </summary>
        void Clear();

        /// <summary>
        /// Освободить ресурсы
        /// </summary>
        void Dispose();
    }
}

