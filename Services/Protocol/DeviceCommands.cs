namespace PrecisionPressureController.Services.Protocol;

/// <summary>
/// Команды протокола для управления устройством давления
/// </summary>
public static class DeviceCommands
{
    // ====================================================================
    // ЧТЕНИЕ ДАННЫХ
    // ====================================================================
    public const string ReadSingleFrame = "A";    // один кадр
    public const string ReadAls = "ALS";          // расширенный кадр (All Status)
    
    // ====================================================================
    // УСТАВКИ ДАВЛЕНИЯ
    // ====================================================================
    public static string SetPressure(double valueInvariant) => $"AP {valueInvariant}";
    public static string SetSetPoint(double valueInvariant) => $"AS {valueInvariant}";
    
    // ====================================================================
    // РАМП (СКОРОСТЬ НАРАСТАНИЯ УСТАВКИ)
    // ====================================================================
    /// <summary>
    /// Установить скорость рампа: ASR value timeUnit
    /// timeUnit: 1=ms, 2=min, 3=hour, 4=sec (по умолчанию используем 4)
    /// </summary>
    public static string SetRamp(double value, int timeUnit = 4) => $"ASR {value.ToString("G", System.Globalization.CultureInfo.InvariantCulture)} {timeUnit}";
    
    /// <summary>
    /// Запросить текущую скорость рампа (без параметров)
    /// </summary>
    public const string ReadRampSpeed = "ASR";
    
    // ====================================================================
    // РЕЖИМЫ РАБОТЫ
    // ====================================================================
    public const string ExhaustHold = "AE";       // открыть и держать выхлоп (Exhaust)
    public const string ControlOn = "AC";        // вернуться в управление (закрыть выхлоп)
    
    // ====================================================================
    // ЕДИНИЦЫ ИЗМЕРЕНИЯ
    // ====================================================================
    /// <summary>
    /// Изменить единицы измерения: ADCU unitCode unitCode
    /// Первый параметр всегда 6 (код для единиц давления)
    /// Второй параметр - код единицы измерения (2=Pa, 3=hPa, 4=kPa, 5=MPa, 6=mbar, 7=bar, 8=g/cm², 9=kg/cm, 10=PSI, 11=PSF, 12=mTorr, 13=Torr)
    /// </summary>
    public static string SetPressureUnits(int unitCode) => $"ADCU 6 {unitCode}";
    
    // ====================================================================
    // ИНФОРМАЦИЯ ОБ УСТРОЙСТВЕ
    // ====================================================================
    public const string GetDeviceInfo = "AVE";    // получить информацию об устройстве (модель, версия, дата)
}

