namespace Alicat.Services.Protocol
{
    public static class AlicatCommands
    {
        // Чтение
        public const string ReadSingleFrame = "A";      // один кадр
        public const string ReadAls = "ALS";     // расширенный кадр

        // Уставки
        public static string SetPressure(double valueInvariant) => $"AP {valueInvariant}";
        public static string SetSetPoint(double valueInvariant) => $"AS {valueInvariant}";

        // Рамп (скорость нарастания уставки)
        public static string SetRamp(string valueInvariant) => $"SR {valueInvariant}";

        // Режимы
        public const string ExhaustHold = "AE"; // открыть и держать выхлоп
        public const string ControlOn = "AC"; // вернуться в управление (закрыть выхлоп)
    }
}
