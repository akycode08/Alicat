using System;
using System.Globalization;

namespace PrecisionPressureController.Domain
{
    public sealed class DeviceState
    {
        public double Current { get; private set; }     // текущее давление
        public double SetPoint { get; private set; }    // уставка
        public string Units { get; private set; }       // PSIG / BAR / KPA
        public bool IsExhaust { get; private set; }     // активен выхлоп
        public DateTime UpdatedAtUtc { get; private set; }

        public DeviceState(double current = 0, double setPoint = 0, string units = "PSIG", bool isExhaust = false)
        {
            Current = current;
            SetPoint = setPoint;
            Units = units;
            IsExhaust = isExhaust;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Update(double current, double setPoint, string? units, bool isExhaust)
        {
            Current = current;
            SetPoint = setPoint;
            if (!string.IsNullOrWhiteSpace(units)) Units = units!;
            IsExhaust = isExhaust;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        // Парсер ALS-строки вида: "A +0030.0 +0030.0 10 PSIG"
        public static bool TryParseAls(string line, out double cur, out double sp, out string? unit)
        {
            cur = 0; sp = 0; unit = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            var parts = line.Trim().Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out cur)) return false;
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out sp)) return false;

            for (int i = 3; i < parts.Length; i++)
            {
                var p = parts[i].Trim().ToUpperInvariant();
                
                // Поддерживаемые единицы измерения давления из таблицы Alicat
                // Устройство возвращает единицы с "G" в конце (barG, kPaG, PSIG и т.д.)
                
                // Проверяем известные единицы (избегаем символа ² в pattern matching)
                bool isKnownUnit = p == "PA" || p == "PAG" || p == "HPA" || p == "HPAG" || 
                                   p == "KPA" || p == "KPAG" || p == "MPA" || p == "MPAG" || 
                                   p == "MBAR" || p == "MBARG" || p == "BAR" || p == "BARG" || 
                                   p == "KG/CM" || p == "KGCM" || p == "KG/CMG" || p == "KGCMG" ||
                                   p == "PSIG" || p == "PSI" || p == "PSFG" || p == "PSF" ||
                                   p == "MTORR" || p == "MTORRG" || p == "TORR" || p == "TORRG" ||
                                   p == "---" || p == "" ||
                                   p.StartsWith("G/CM") || p.StartsWith("GCM");
                
                if (isKnownUnit)
                {
                    unit = NormalizeUnit(p);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Нормализует единицу измерения к стандартному виду для отображения.
        /// Убирает "G" в конце для единиц, которые не должны его иметь (barG → bar, kPaG → kPa).
        /// </summary>
        private static string NormalizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit == "---" || unit == "")
                return "PSIG"; // Default unit

            var upper = unit.ToUpperInvariant();

            // Убираем "G" в конце, если есть (кроме PSIG, который уже содержит G)
            if (upper.EndsWith("G") && upper != "PSIG" && upper != "PSFG")
            {
                upper = upper.Substring(0, upper.Length - 1);
            }
            
            // Обработка вариантов g/cm²
            if (upper == "G/CM²" || upper == "G/CM2" || upper == "GCM²" || upper == "GCM2")
                return "g/cm²";
            
            // Обработка вариантов kg/cm
            if (upper == "KG/CM" || upper == "KGCM")
                return "kg/cm";
            

            return upper switch
            {
                "PA" => "Pa",
                "HPA" => "hPa",
                "KPA" => "kPa",
                "MPA" => "MPa",
                "MBAR" => "mbar",
                "BAR" => "bar",
                "PSIG" => "PSIG",
                "PSI" => "PSI",
                "PSFG" => "PSF",
                "PSF" => "PSF",
                "MTORR" => "mTorr",
                "TORR" => "torr",
                _ => unit // Возвращаем как есть, если не распознали
            };
        }
    }
}