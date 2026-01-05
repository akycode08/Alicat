using System;
using System.Collections.Generic;
using Alicat.Services.Data;

namespace Alicat.Services.Reports
{
    /// <summary>
    /// Модель данных для генерации PDF отчета сессии
    /// </summary>
    public class SessionReportData
    {
        // Session Info
        public string SessionName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        
        // Device Info
        public string DeviceModel { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public DateTime CalibrationDate { get; set; }
        
        // Connection Info
        public string ComPort { get; set; } = string.Empty;
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public string Parity { get; set; } = string.Empty;
        public string StopBits { get; set; } = string.Empty;
        
        // Measurement Settings
        public string PressureUnit { get; set; } = "PSIG";
        public string TimeUnit { get; set; } = "s";
        public decimal RampSpeed { get; set; }
        public int SampleRate { get; set; }
        public decimal MaxPressure { get; set; }
        public decimal MinPressure { get; set; }
        
        // Statistics
        public decimal InitialPressure { get; set; }
        public decimal FinalPressure { get; set; }
        public decimal MaxPressureReached { get; set; }
        public decimal AverageRate { get; set; }
        public int TotalDataPoints { get; set; }
        
        // Data Points (для таблицы в отчете)
        public List<ReportDataPoint> DataPoints { get; set; } = new List<ReportDataPoint>();
    }

    /// <summary>
    /// Точка данных для отчета (упрощенная версия DataPoint)
    /// </summary>
    public class ReportDataPoint
    {
        public decimal Time { get; set; }
        public decimal Pressure { get; set; }
        public decimal Target { get; set; }
        public decimal Rate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

