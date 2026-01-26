using System;
using PrecisionPressureController.Business.Interfaces;
using PrecisionPressureController.Services.Data;
using PrecisionPressureController.Services.Sequence;

namespace PrecisionPressureController.Presentation.Presenters
{
    /// <summary>
    /// Presenter для управления последовательностями давления.
    /// Отвечает только за работу с SequenceService.
    /// </summary>
    public class SequencePresenter
    {
        private readonly IMainView _view;
        private readonly IDataStore _dataStore;
        private SequenceService? _sequenceService;
        private double _currentPressure;

        public SequencePresenter(IMainView view, IDataStore dataStore)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        /// <summary>
        /// Получить SequenceService (для передачи в ChartWindow)
        /// </summary>
        public SequenceService? SequenceService => _sequenceService;

        /// <summary>
        /// Получить текущий индекс активной точки
        /// </summary>
        public int CurrentTargetIndex => _sequenceService?.CurrentTargetIndex ?? -1;

        /// <summary>
        /// Получить текущий PointIndex (1 = первая точка, 2 = вторая и т.д.)
        /// </summary>
        public int GetCurrentPointIndex()
        {
            if (_sequenceService == null)
                return 0;

            int currentTargetIndex = _sequenceService.CurrentTargetIndex;
            
            // Если последовательность не запущена или нет активной точки
            if (currentTargetIndex < 0)
                return 0;

            // PointIndex = CurrentTargetIndex + 1 (1 = первая точка, 2 = вторая и т.д.)
            return currentTargetIndex + 1;
        }

        /// <summary>
        /// Инициализирует SequenceService
        /// </summary>
        public void Initialize(Func<double> getCurrentPressure, Action<double> setTargetPressure, Action onSequenceStateChanged)
        {
            _sequenceService = new SequenceService(getCurrentPressure, setTargetPressure);
            
            // Загружаем сохраненные targets при старте
            _sequenceService.LoadTargetsFromFile();
            
            // Подписываемся на события для обновления UI
            _sequenceService.OnSequenceStateChanged += onSequenceStateChanged;
        }

        /// <summary>
        /// Обновляет текущее давление (для SequenceService)
        /// </summary>
        public void UpdateCurrentPressure(double current)
        {
            _currentPressure = current;
        }

        /// <summary>
        /// Очищает данные последовательности при закрытии программы
        /// </summary>
        public void ClearSequenceOnExit()
        {
            _sequenceService?.ClearTargetsOnExit();
        }
    }
}
