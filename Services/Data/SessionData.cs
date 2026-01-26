using System;
using System.Collections.Generic;
using PrecisionPressureController.Domain;

namespace PrecisionPressureController.Services.Data
{
    /// <summary>
    /// Data model for session information used in PDF report generation
    /// </summary>
    public class SessionData
    {
        // Session Info
        public string SessionName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; } = "Active";
        public string Operator { get; set; } = string.Empty;
        
        /// <summary>
        /// Состояние сессии (None, Created, Active, Completed)
        /// </summary>
        public SessionState State { get; set; } = SessionState.Created;
        
        /// <summary>
        /// Режим только для чтения (true если State == Completed)
        /// </summary>
        public bool IsReadOnly => State == SessionState.Completed;
        
        // Device Info
        public string DeviceModel { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public DateTime CalibrationDate { get; set; }
        
        // Connection Info
        public string ComPort { get; set; } = "COM3";
        public int BaudRate { get; set; } = 19200;
        public int DataBits { get; set; } = 8;
        public string Parity { get; set; } = "None";
        public string StopBits { get; set; } = "1";
        
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
        
        // Data Points
        public List<ReportDataPoint> DataPoints { get; set; } = new List<ReportDataPoint>();
    }

    /// <summary>
    /// Data point for time-series data in the PDF report
    /// (Separate from SessionDataStore.DataPoint to avoid conflicts)
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

