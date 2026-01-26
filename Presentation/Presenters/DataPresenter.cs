using System;
using PrecisionPressureController.Business.Interfaces;
using PrecisionPressureController.Services.Data;

namespace PrecisionPressureController.Presentation.Presenters
{
    /// <summary>
    /// Presenter для управления данными сессии.
    /// Отвечает только за запись данных и управление сессией.
    /// </summary>
    public class DataPresenter
    {
        private readonly IDataStore _dataStore;

        public DataPresenter(IDataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        /// <summary>
        /// Записывает точку данных
        /// </summary>
        public void RecordDataPoint(double current, double setPoint, string unit, double rampSpeed, int pollingInterval, int pointIndex)
        {
            _dataStore.RecordSample(current, setPoint, unit, rampSpeed, pollingInterval, pointIndex);
        }

        /// <summary>
        /// Записывает сэмпл данных
        /// </summary>
        public void RecordSample(double current, double setPoint, string unit, double rampSpeed, int pollingInterval, int pointIndex)
        {
            _dataStore.RecordSample(current, setPoint, unit, rampSpeed, pollingInterval, pointIndex);
        }

        /// <summary>
        /// Записывает событие
        /// </summary>
        public void RecordEvent(double current, double setPoint, string unit, string eventType, double rampSpeed, int pollingInterval, int pointIndex)
        {
            _dataStore.RecordEvent(current, setPoint, unit, eventType, rampSpeed, pollingInterval, pointIndex);
        }

        /// <summary>
        /// Начинает новую сессию
        /// </summary>
        public void StartSession(string? filePath = null)
        {
            if (!_dataStore.IsRunning)
            {
                if (filePath != null)
                {
                    _dataStore.StartSession(filePath);
                }
                else
                {
                    _dataStore.StartSession();
                }
            }
        }

        /// <summary>
        /// Останавливает сессию
        /// </summary>
        public void StopSession()
        {
            if (_dataStore.IsRunning)
            {
                _dataStore.EndSession();
            }
        }

        /// <summary>
        /// Очищает данные сессии
        /// </summary>
        public void ClearSession()
        {
            _dataStore.Clear();
        }

        /// <summary>
        /// Проверяет, запущена ли сессия
        /// </summary>
        public bool IsRunning => _dataStore.IsRunning;

        /// <summary>
        /// Получает количество точек данных
        /// </summary>
        public int PointsCount
        {
            get
            {
                if (_dataStore is SessionDataStore sessionStore)
                {
                    return sessionStore.Points.Count;
                }
                return 0;
            }
        }

        /// <summary>
        /// Получает последнее значение давления
        /// </summary>
        public double? GetLastCurrent()
        {
            if (_dataStore is SessionDataStore sessionStore && sessionStore.Points.Count > 0)
            {
                return sessionStore.Points.Last().Current;
            }
            return null;
        }
    }
}
