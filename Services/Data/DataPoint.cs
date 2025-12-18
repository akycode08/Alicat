using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alicat.Services.Data
{
    /// <summary>
    /// Одна точка данных эксперимента
    /// </summary>
    public sealed class DataPoint
    {
        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Секунды с начала сессии
        /// </summary>
        public double ElapsedSeconds { get; }

        /// <summary>
        /// Текущее давление
        /// </summary>
        public double Current { get; }

        /// <summary>
        /// Уставка (Target)
        /// </summary>
        public double Target { get; }

        /// <summary>
        /// Единицы измерения (PSIG, BAR, KPA)
        /// </summary>
        public string Unit { get; }

        /// <summary>
        /// Событие (TARGET_CHANGED, EXHAUST_ON, и т.д.) или null
        /// </summary>
        public string? Event { get; }

        public DataPoint(
            DateTime timestamp,
            double elapsedSeconds,
            double current,
            double target,
            string unit,
            string? eventType = null)
        {
            Timestamp = timestamp;
            ElapsedSeconds = elapsedSeconds;
            Current = current;
            Target = target;
            Unit = unit;
            Event = eventType;
        }
    }
}
