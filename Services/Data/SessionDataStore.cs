using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alicat.Services.Data
{
    /// <summary>
    /// Центральное хранилище данных сессии.
    /// Пока только RAM — автосохранение добавим позже.
    /// </summary>
    public sealed class SessionDataStore
    {
        private readonly List<DataPoint> _points = new();
        private DateTime _sessionStart;
        private bool _isRunning;

        /// <summary>
        /// Все точки данных (только чтение)
        /// </summary>
        public IReadOnlyList<DataPoint> Points => _points;

        /// <summary>
        /// Сессия запущена?
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Время начала сессии
        /// </summary>
        public DateTime SessionStart => _sessionStart;

        /// <summary>
        /// Событие: новая точка добавлена
        /// </summary>
        public event Action<DataPoint>? OnNewPoint;

        /// <summary>
        /// Событие: сессия началась
        /// </summary>
        public event Action? OnSessionStarted;

        /// <summary>
        /// Событие: сессия завершена
        /// </summary>
        public event Action? OnSessionEnded;

        // ═══════════════════════════════════════════
        // СТАРТ СЕССИИ
        // ═══════════════════════════════════════════
        public void StartSession()
        {
            _points.Clear();
            _sessionStart = DateTime.Now;
            _isRunning = true;

            OnSessionStarted?.Invoke();
        }

        // ═══════════════════════════════════════════
        // ЗАПИСЬ ТОЧКИ (каждые 500мс)
        // ═══════════════════════════════════════════
        public void RecordSample(double current, double target, string unit)
        {
            if (!_isRunning) return;

            var now = DateTime.Now;
            var elapsed = (now - _sessionStart).TotalSeconds;

            var point = new DataPoint(now, elapsed, current, target, unit);

            _points.Add(point);
            OnNewPoint?.Invoke(point);
        }

        // ═══════════════════════════════════════════
        // ЗАПИСЬ СОБЫТИЯ (немедленно)
        // ═══════════════════════════════════════════
        public void RecordEvent(double current, double target, string unit, string eventType)
        {
            if (!_isRunning) return;

            var now = DateTime.Now;
            var elapsed = (now - _sessionStart).TotalSeconds;

            var point = new DataPoint(now, elapsed, current, target, unit, eventType);

            _points.Add(point);
            OnNewPoint?.Invoke(point);
        }

        // ═══════════════════════════════════════════
        // КОНЕЦ СЕССИИ
        // ═══════════════════════════════════════════
        public void EndSession()
        {
            _isRunning = false;
            OnSessionEnded?.Invoke();
        }

        // ═══════════════════════════════════════════
        // ОЧИСТКА (для новой сессии без завершения)
        // ═══════════════════════════════════════════
        public void Clear()
        {
            _points.Clear();
        }
    }
}
