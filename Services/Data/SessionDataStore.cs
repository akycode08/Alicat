using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Alicat.Business.Interfaces;

namespace Alicat.Services.Data
{
    /// <summary>
    /// Центральное хранилище данных сессии.
    /// Хранит в RAM всегда, а в CSV пишет ТОЛЬКО когда значения меняются (порог 0.3).
    /// </summary>
    public sealed class SessionDataStore : IDataStore, IDisposable
    {
        private readonly List<DataPoint> _points = new();
        private DateTime _sessionStart;
        private bool _isRunning;

        // CSV файл
        private StreamWriter? _writer;
        private string? _csvPath;

        // ===== CSV change-based logging =====
        private const double CsvDelta = 0.3;        // порог изменения
        private const double CsvHeartbeatSec = 0.0; // 0 = выключить, запись только при изменении >= CsvDelta

        private DataPoint? _lastCsvPoint;
        private DateTime _lastCsvWriteTime;
        
        // Счетчик строк CSV (RowNumber)
        private int _csvRowNumber = 0;

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
            _csvRowNumber = 0; // Сбрасываем счетчик строк

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
            _csvRowNumber = 0; // Сбрасываем счетчик строк

            // Создаём файл и пишем заголовок (новый формат с RowNumber и PointIndex)
            _writer = new StreamWriter(csvFilePath, append: false, Encoding.UTF8);
            _writer.WriteLine("RowNumber,Timestamp,Time_s,Current,Target,Unit,RampSpeed_psi_s,PollingFrequency,PointIndex,Event");
            _writer.Flush();

            OnSessionStarted?.Invoke();
        }

        // ═══════════════════════════════════════════
        // ЗАПИСЬ ТОЧКИ (каждые 500мс приходят данные)
        // В RAM пишем всегда, в CSV — только если изменилось >= 0.3
        // ═══════════════════════════════════════════
        public void RecordSample(double current, double target, string unit)
        {
            RecordSample(current, target, unit, 0.0, 500);
        }

        public void RecordSample(double current, double target, string unit, double rampSpeed, int pollingFrequency)
        {
            RecordSample(current, target, unit, rampSpeed, pollingFrequency, 0);
        }

        public void RecordSample(double current, double target, string unit, double rampSpeed, int pollingFrequency, int pointIndex)
        {
            if (!_isRunning) return;

            var now = DateTime.Now;
            var elapsed = (now - _sessionStart).TotalSeconds;

            var point = new DataPoint(now, elapsed, current, target, unit, rampSpeed, pollingFrequency);

            // RAM
            _points.Add(point);

            // CSV (по изменению)
            if (ShouldWriteToCsv(point))
            {
                WritePointToCsv(point, pointIndex);
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
            RecordEvent(current, target, unit, eventType, 0.0, 500);
        }

        public void RecordEvent(double current, double target, string unit, string eventType, double rampSpeed, int pollingFrequency)
        {
            RecordEvent(current, target, unit, eventType, rampSpeed, pollingFrequency, 0);
        }

        public void RecordEvent(double current, double target, string unit, string eventType, double rampSpeed, int pollingFrequency, int pointIndex)
        {
            if (!_isRunning) return;

            var now = DateTime.Now;
            var elapsed = (now - _sessionStart).TotalSeconds;

            var point = new DataPoint(now, elapsed, current, target, unit, rampSpeed, pollingFrequency, eventType);

            // RAM
            _points.Add(point);

            // CSV (события всегда)
            if (_writer != null)
            {
                WritePointToCsv(point, pointIndex);
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
        private void WritePointToCsv(DataPoint point, int pointIndex)
        {
            if (_writer == null) return;

            // Увеличиваем счетчик строк
            _csvRowNumber++;

            // Новый формат: RowNumber,Timestamp,Time_s,Current,Target,Unit,RampSpeed_psi_s,PollingFrequency,PointIndex,Event
            var line = string.Format(
                CultureInfo.InvariantCulture,
                "{0},{1:yyyy-MM-dd HH:mm:ss.fff},{2:F3},{3:F2},{4:F2},{5},{6:F2},{7},{8},{9}",
                _csvRowNumber,                    // RowNumber
                point.Timestamp,                  // Timestamp
                point.ElapsedSeconds,             // Time_s
                point.Current,                    // Current
                point.Target,                     // Target
                point.Unit,                       // Unit
                point.RampSpeed,                  // RampSpeed_psi_s
                point.PollingFrequency,           // PollingFrequency
                pointIndex,                       // PointIndex (0 = до старта, 1 = первая точка, и т.д.)
                point.Event ?? ""                 // Event
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
        // ЗАГРУЗКА ИСТОРИЧЕСКИХ ДАННЫХ (для read-only режима)
        // ═══════════════════════════════════════════
        /// <summary>
        /// Загружает исторические данные из CSV файла (для read-only режима)
        /// </summary>
        public void LoadHistoricalDataFromCsv(string csvFilePath, DateTime sessionStart)
        {
            _points.Clear();
            _sessionStart = sessionStart;
            _isRunning = false; // Не активная сессия, только для просмотра
            _csvPath = csvFilePath;

            if (!File.Exists(csvFilePath))
                return;

            var lines = File.ReadAllLines(csvFilePath);
            if (lines.Length < 2)
                return;

            // Определяем формат файла по заголовку
            bool isNewFormat = lines[0].StartsWith("RowNumber,Timestamp", StringComparison.OrdinalIgnoreCase);
            int timestampIndex = isNewFormat ? 1 : 0;
            int timeIndex = isNewFormat ? 2 : 1;
            int currentIndex = isNewFormat ? 3 : 2;
            int targetIndex = isNewFormat ? 4 : 3;
            int unitIndex = isNewFormat ? 5 : 4;
            int rampSpeedIndex = isNewFormat ? 6 : 5;
            int pollingFreqIndex = isNewFormat ? 7 : 6;
            int eventIndex = isNewFormat ? 9 : 7; // PointIndex на позиции 8 в новом формате, Event на 9

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(',');
                if (parts.Length < currentIndex + 1) continue;

                try
                {
                    string timestampStr = parts[timestampIndex];
                    if (DateTime.TryParse(timestampStr, out DateTime timestamp))
                    {
                        double elapsed = parts.Length > timeIndex && double.TryParse(parts[timeIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out double e) ? e : 0;
                        double current = parts.Length > currentIndex && double.TryParse(parts[currentIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out double c) ? c : 0;
                        double target = parts.Length > targetIndex && double.TryParse(parts[targetIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out double t) ? t : 0;
                        string unit = parts.Length > unitIndex ? parts[unitIndex] : "PSIG";
                        double rampSpeed = parts.Length > rampSpeedIndex && double.TryParse(parts[rampSpeedIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out double rs) ? rs : 0;
                        int pollingFreq = parts.Length > pollingFreqIndex && int.TryParse(parts[pollingFreqIndex], out int pf) ? pf : 500;
                        string? eventType = parts.Length > eventIndex && !string.IsNullOrWhiteSpace(parts[eventIndex]) ? parts[eventIndex] : null;

                        var point = new DataPoint(timestamp, elapsed, current, target, unit, rampSpeed, pollingFreq, eventType);
                        _points.Add(point);
                    }
                }
                catch
                {
                    // Пропускаем некорректные строки
                    continue;
                }
            }
        }

        // ═══════════════════════════════════════════
        // ОЧИСТКА
        // ═══════════════════════════════════════════
        public void Clear()
        {
            _points.Clear();
            _lastCsvPoint = null;
            _lastCsvWriteTime = DateTime.MinValue;
            _csvRowNumber = 0; // Сбрасываем счетчик строк
        }

        public void Dispose()
        {
            EndSession();
        }
    }
}
