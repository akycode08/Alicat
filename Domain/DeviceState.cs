using System;
using System.Globalization;

namespace Alicat.Domain
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
                if (p is "PA" or "HPA" or "KPA" or "MPA" or 
                    "MBAR" or "BAR" or 
                    "G/CM²" or "G/CM2" or "GCM²" or "GCM2" or
                    "KG/CM" or "KGCM" or
                    "PSIG" or "PSI" or "PSF" or
                    "MTORR" or "TORR" or
                    "---" or "" or string.Empty)
                {
                    unit = NormalizeUnit(p);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Нормализует единицу измерения к стандартному виду для отображения.
        /// </summary>
        private static string NormalizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit == "---" || unit == "")
                return "PSIG"; // Default unit

            var upper = unit.ToUpperInvariant();
            
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
                "PSF" => "PSF",
                "MTORR" => "mTorr",
                "TORR" => "torr",
                _ => unit // Возвращаем как есть, если не распознали
            };
        }
    }
}
