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
                if (p is "PSIG" or "PSI" or "KPA" or "BAR") { unit = p; break; }
            }
            return true;
        }
    }
}
