using System;

namespace Alicat.Domain
{
    /// <summary>
    /// Утилита для расчета ETA (Estimated Time of Arrival - расчетное время достижения цели)
    /// </summary>
    public static class ETACalculator
    {
        /// <summary>
        /// Порог разницы давлений для определения "At target"
        /// </summary>
        private const double AtTargetThreshold = 0.1;

        /// <summary>
        /// Порог скорости для определения стабильности (нулевая скорость)
        /// </summary>
        private const double ZeroRateThreshold = 0.01;

        /// <summary>
        /// Результат расчета ETA
        /// </summary>
        public class ETAResult
        {
            /// <summary>
            /// Текст для отображения ETA
            /// </summary>
            public string DisplayText { get; set; } = "--";

            /// <summary>
            /// ETA в секундах (если рассчитан)
            /// </summary>
            public double? EtaSeconds { get; set; }

            /// <summary>
            /// Статус: достигнута ли цель
            /// </summary>
            public bool IsAtTarget { get; set; }

            /// <summary>
            /// Статус: стабильное состояние
            /// </summary>
            public bool IsStable { get; set; }
        }

        /// <summary>
        /// Рассчитывает ETA на основе фактической скорости изменения давления (Rate)
        /// Это более точный метод, который учитывает реальное поведение системы
        /// </summary>
        /// <param name="currentPressure">Текущее давление</param>
        /// <param name="targetPressure">Целевое давление (null если нет цели)</param>
        /// <param name="rate">Фактическая скорость изменения давления (Current - Previous) / TimeStep</param>
        /// <param name="isExhaust">Режим выхлопа (purge)</param>
        /// <returns>Результат расчета ETA</returns>
        public static ETAResult CalculateETA(double currentPressure, double? targetPressure, double rate, bool isExhaust = false)
        {
            var result = new ETAResult();

            // Если режим выхлопа
            if (isExhaust)
            {
                result.DisplayText = "Purging";
                return result;
            }

            // Если нет целевого давления
            if (!targetPressure.HasValue)
            {
                result.DisplayText = "--";
                return result;
            }

            double delta = currentPressure - targetPressure.Value;
            double absDelta = Math.Abs(delta);

            // Проверяем, достигнута ли цель
            if (absDelta < AtTargetThreshold)
            {
                result.DisplayText = "Done";
                result.IsAtTarget = true;
                return result;
            }

            // Проверяем, есть ли скорость изменения давления
            double absRate = Math.Abs(rate);
            if (absRate > ZeroRateThreshold)
            {
                // Рассчитываем ETA: |Current - Target| / Rate
                double etaSeconds = absDelta / absRate;
                result.EtaSeconds = etaSeconds;

                // Форматируем в MM:SS
                int etaMins = (int)(etaSeconds / 60);
                int etaSecs = (int)(etaSeconds % 60);
                result.DisplayText = $"{etaMins}:{etaSecs:D2}";
            }
            else
            {
                // Скорость слишком мала - стабильное состояние
                result.DisplayText = "Stable";
                result.IsStable = true;
            }

            return result;
        }

        /// <summary>
        /// Рассчитывает ETA на основе установленной скорости рампы (RampSpeed)
        /// Используется как запасной вариант, когда нет фактического Rate
        /// </summary>
        /// <param name="currentPressure">Текущее давление</param>
        /// <param name="targetPressure">Целевое давление</param>
        /// <param name="rampSpeed">Установленная скорость рампы</param>
        /// <param name="isExhaust">Режим выхлопа (purge)</param>
        /// <param name="unit">Единица измерения давления (для отображения разницы)</param>
        /// <returns>Результат расчета ETA</returns>
        public static ETAResult CalculateETAFromRampSpeed(double currentPressure, double targetPressure, double rampSpeed, bool isExhaust = false, string unit = "PSIG")
        {
            var result = new ETAResult();

            // Если режим выхлопа
            if (isExhaust)
            {
                result.DisplayText = "Purging";
                return result;
            }

            double delta = currentPressure - targetPressure;
            double absDelta = Math.Abs(delta);

            // Проверяем, достигнута ли цель (используем 0.5 для совместимости со старым кодом)
            if (absDelta < 0.5)
            {
                result.DisplayText = "At target";
                result.IsAtTarget = true;
                return result;
            }

            // Проверяем, есть ли установленная скорость рампы
            if (rampSpeed > 0.001)
            {
                // Рассчитываем ETA: |Current - Target| / RampSpeed
                double etaSeconds = absDelta / rampSpeed;
                result.EtaSeconds = etaSeconds;

                // Форматируем в секунды с 1 знаком
                result.DisplayText = $"ETA: {etaSeconds:F1} s";
            }
            else
            {
                // Скорость не установлена - показываем разницу
                result.DisplayText = $"→ {absDelta:F1} {unit}";
            }

            return result;
        }
    }
}

