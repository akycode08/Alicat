namespace PrecisionPressureController.Business.Interfaces
{
    /// <summary>
    /// Интерфейс для управления рампой давления.
    /// Позволяет тестировать логику без реального устройства.
    /// </summary>
    public interface IRampController
    {
        /// <summary>
        /// Установить скорость рампа.
        /// </summary>
        /// <param name="pressureRamp">Скорость рампа (единицы/сек)</param>
        /// <returns>true если команда отправлена успешно</returns>
        bool TryApply(double? pressureRamp);
    }
}

