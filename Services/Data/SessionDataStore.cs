using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Alicat.Services.Data
{
    /// <summary>
    /// Центральное хранилище данных сессии.
    /// Хранит в RAM всегда, а в CSV пишет ТОЛЬКО когда значения меняются (порог 0.3).
    /// </summary>
    public sealed class SessionDataStore : IDisposable
    {
        private readonly List<DataPoint> _points = new();
        private DateTime _sessionStart;
        private bool _isRunning;

        // CSV файл
        private StreamWriter? _writer;
        private string? _csvPath;

        // ===== CSV change-based logging =====
        private const double CsvDelta = 0.3;        // порог изменения
        private const double CsvHeartbeatSec = 0.0; // 0 = выключить, можно поставить 10

        private DataPoint? _lastCsvPoint;
        private DateTime _lastCsvWriteTime;

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
        /// Путь к CSV файлу (null если не записывается)
        /// </summary>
        public string? CsvPath => _csvPath;

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
        // СТАРТ СЕССИИ (без файла — только RAM)
        // ═══════════════════════════════════════════
        public void StartSession()
        {
            EndSession(); // закрыть предыдущую если была

            _points.Clear();
            _sessionStart = DateTime.Now;
            _isRunning = true;
            _csvPath = null;

            _lastCsvPoint = null;
            _lastCsvWriteTime = DateTime.MinValue;

            OnSessionStarted?.Invoke();
        }

        // ═══════════════════════════════════════════
        // СТАРТ СЕССИИ (с записью в CSV)
        // ═══════════════════════════════════════════
        public void StartSession(string csvFilePath)
        {
            EndSession(); // закрыть предыдущую если была

            _points.Clear();
            _sessionStart = DateTime.Now;
            _isRunning = true;
            _csvPath = csvFilePath;

            _lastCsvPoint = null;
            _lastCsvWriteTime = DateTime.MinValue;

            // Создаём файл и пишем заголовок
            _writer = new StreamWriter(csvFilePath, append: false, Encoding.UTF8);
            _writer.WriteLine("Timestamp,ElapsedSec,Current,Target,Unit,Event");
            _writer.Flush();

            OnSessionStarted?.Invoke();
        }

        // ═══════════════════════════════════════════
        // ЗАПИСЬ ТОЧКИ (каждые 500мс приходят данные)
        // В RAM пишем всегда, в CSV — только если изменилось >= 0.3
        // ═══════════════════════════════════════════
        public void RecordSample(double current, double target, string unit)
        {
            if (!_isRunning) return;

            var now = DateTime.Now;
            var elapsed = (now - _sessionStart).TotalSeconds;

            var point = new DataPoint(now, elapsed, current, target, unit);

            // RAM
            _points.Add(point);

            // CSV (по изменению)
            if (ShouldWriteToCsv(point))
            {
                WritePointToCsv(point);
                _lastCsvPoint = point;
                _lastCsvWriteTime = point.Timestamp;
            }

            // UI
            OnNewPoint?.Invoke(point);
        }

        // ═══════════════════════════════════════════
        // ЗАПИСЬ СОБЫТИЯ (немедленно)
        // События в CSV пишем всегда.
        // ═══════════════════════════════════════════
        public void RecordEvent(double current, double target, string unit, string eventType)
        {
            if (!_isRunning) return;

            var now = DateTime.Now;
            var elapsed = (now - _sessionStart).TotalSeconds;

            var point = new DataPoint(now, elapsed, current, target, unit, eventType);

            // RAM
            _points.Add(point);

            // CSV (события всегда)
            if (_writer != null)
            {
                WritePointToCsv(point);
                _lastCsvPoint = point;
                _lastCsvWriteTime = point.Timestamp;
            }

            // UI
            OnNewPoint?.Invoke(point);
        }

        // ═══════════════════════════════════════════
        // Нужно ли писать в CSV?
        // ═══════════════════════════════════════════
        private bool ShouldWriteToCsv(DataPoint p)
        {
            // если файл не открыт — не пишем
            if (_writer == null) return false;

            // если это событие — пишем всегда
            if (!string.IsNullOrEmpty(p.Event)) return true;

            // первая точка — пишем
            if (_lastCsvPoint == null) return true;

            // heartbeat (если включишь)
            if (CsvHeartbeatSec > 0.0)
            {
                var silent = (p.Timestamp - _lastCsvWriteTime).TotalSeconds;
                if (silent >= CsvHeartbeatSec) return true;
            }

            // главное: порог 0.3
            if (Math.Abs(p.Current - _lastCsvPoint.Current) >= CsvDelta) return true;
            if (Math.Abs(p.Target - _lastCsvPoint.Target) >= CsvDelta) return true;

            // (опционально) если единицы сменились
            if (!string.Equals(p.Unit, _lastCsvPoint.Unit, StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        // ═══════════════════════════════════════════
        // ЗАПИСЬ В CSV
        // ═══════════════════════════════════════════
        private void WritePointToCsv(DataPoint point)
        {
            if (_writer == null) return;

            var line = string.Format(
                CultureInfo.InvariantCulture,
                "{0:yyyy-MM-dd HH:mm:ss.fff},{1:F3},{2:F2},{3:F2},{4},{5}",
                point.Timestamp,
                point.ElapsedSeconds,
                point.Current,
                point.Target,
                point.Unit,
                point.Event ?? ""
            );

            _writer.WriteLine(line);
            _writer.Flush(); // сразу на диск
        }

        // ═══════════════════════════════════════════
        // КОНЕЦ СЕССИИ
        // ═══════════════════════════════════════════
        public void EndSession()
        {
            _isRunning = false;

            _writer?.Flush();
            _writer?.Dispose();
            _writer = null;

            OnSessionEnded?.Invoke();
        }

        // ═══════════════════════════════════════════
        // ОЧИСТКА
        // ═══════════════════════════════════════════
        public void Clear()
        {
            _points.Clear();
            _lastCsvPoint = null;
            _lastCsvWriteTime = DateTime.MinValue;
        }

        public void Dispose()
        {
            EndSession();
        }
    }
}
