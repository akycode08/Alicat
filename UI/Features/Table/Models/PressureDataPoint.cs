using System;

namespace Alicat.UI.Features.Table.Models
{
    /// <summary>
    /// Модель данных для строки таблицы давления
    /// </summary>
    public class PressureDataPoint
    {
        public int Index { get; set; }
        public DateTime Time { get; set; }
        public double Pressure { get; set; }
        public double Setpoint { get; set; }
        public double Rate { get; set; }
        public string Status { get; set; } = "";
        public string Comment { get; set; } = "";
    }
}
