using System;
using System.Threading.Tasks;

namespace PrecisionPressureController.Business.Interfaces
{
    /// <summary>
    /// Интерфейс для ChartWindow - позволяет Presenter работать с графиком без зависимости от UI
    /// </summary>
    public interface IGraphView
    {
        /// <summary>
        /// Обновляет состояние последовательности на графике
        /// </summary>
        void RefreshSequenceState();

        /// <summary>
        /// Устанавливает информацию о подключении
        /// </summary>
        void SetConnectionInfo(string? comPort, int? baudRate);

        /// <summary>
        /// Добавляет точку данных на график
        /// </summary>
        void AddSample(double current, double? target);

        /// <summary>
        /// Обновляет состояние паузы
        /// </summary>
        void UpdatePauseState(bool isPaused);

        /// <summary>
        /// Устанавливает обработчик паузы
        /// </summary>
        void SetPauseHandler(Action handler);

        /// <summary>
        /// Устанавливает обработчик установки цели
        /// </summary>
        void SetTargetHandler(Action<double> handler);

        /// <summary>
        /// Устанавливает обработчик установки цели без подтверждения
        /// </summary>
        void SetTargetHandlerSilent(Action<double> handler);

        /// <summary>
        /// Устанавливает обработчик аварийной вентиляции
        /// </summary>
        void SetEmergencyVentHandler(Action handler);

        /// <summary>
        /// Устанавливает сервис последовательностей
        /// </summary>
        void SetSequenceService(object? sequenceService);

        /// <summary>
        /// Применяет тему оформления
        /// </summary>
        void ApplyTheme(bool isDark);

        /// <summary>
        /// Обновляет пороги из настроек
        /// </summary>
        void UpdateThresholdsFromSettings();

        /// <summary>
        /// Устанавливает обработчик изменения порогов
        /// </summary>
        void SetThresholdsChangedHandler(Action handler);

        /// <summary>
        /// Обновляет пороги из настроек (обновление)
        /// </summary>
        void RefreshThresholdsFromSettings();

        /// <summary>
        /// Проверяет, уничтожена ли форма
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Состояние окна
        /// </summary>
        System.Windows.Forms.FormWindowState WindowState { get; set; }

        /// <summary>
        /// Показать форму
        /// </summary>
        void Show(System.Windows.Forms.IWin32Window? owner);

        /// <summary>
        /// Установить фокус на форму
        /// </summary>
        void Focus();
    }
}
