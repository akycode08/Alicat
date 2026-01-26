using PrecisionPressureController.Business.Interfaces;

namespace PrecisionPressureController.Presentation.Presenters
{
    /// <summary>
    /// Интерфейс для MainForm - позволяет Presenter обновлять UI без прямых зависимостей.
    /// </summary>
    public interface IMainView
    {
        // UI Update Methods
        void UI_SetPressureUnits(string units);
        void UI_SetRampSpeedUnits(string units);
        void UI_SetSetPoint(double sp, string? units = null);
        void UI_UpdateConnectionStatus(bool connected, string? portName = null);
        void UI_UpdateLastUpdate(string text);
        void UI_SetTrendStatus(double? prev, double now, bool isExhaust, double rampSpeed);
        void RefreshCurrent();
        void UI_AppendStatusInfo(string line);
        void ValidateTargetAgainstMax();
        void ValidateIncrementAgainstMax();
        void UpdateIncrementButtons();

        // Properties для доступа к UI элементам (для валидации)
        string TargetInputText { get; set; }
        string IncrementText { get; set; }
        double CurrentIncrement { get; set; }
        double MaxPressure { get; set; }
        double MaxIncrementLimit { get; set; }
        double Current { get; set; }
        double SetPoint { get; set; }
        string Unit { get; set; }
        double RampSpeed { get; set; }

        // Child Forms - теперь через интерфейсы
        ITerminalView? TerminalView { get; set; }
        IGraphView? GraphView { get; set; }
        ITableView? TableView { get; set; }

        // Invoke для thread-safe UI updates
        void BeginInvoke(Action action);

        // Settings persistence (always saves automatically)
        void SaveSettingsIfAutoSaveEnabled();

        // Apply options to UI (for internal View fields like _minPressure, _minIncrementLimit)
        void ApplyOptionsToUi();

        // Theme information
        bool IsDarkTheme { get; }
    }
}

