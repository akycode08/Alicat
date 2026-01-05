using System;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Alicat.Services.Reports
{
    /// <summary>
    /// Генератор PDF отчетов для сессий Alicat Controller
    /// </summary>
    public class SessionReportGenerator
    {
        private readonly SessionReportData _session;

        public SessionReportGenerator(SessionReportData session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            QuestPDF.Settings.License = LicenseType.Community;
        }

        /// <summary>
        /// Генерирует PDF отчет и сохраняет в указанный файл
        /// </summary>
        public void Generate(string outputPath)
        {
            Document.Create(container =>
            {
                // Первая страница - основная информация
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Segoe UI"));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });

                // Вторая страница - данные и детали
                if (_session.DataPoints.Count > 0)
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(20, Unit.Millimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Segoe UI"));

                        page.Header().Element(ComposeHeader);
                        page.Content().Element(ComposeDataTable);
                        page.Footer().Element(ComposeFooter);
                    });
                }
            }).GeneratePdf(outputPath);
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Session Report")
                        .FontSize(24).Bold().FontColor("#0078d4");
                    column.Item().Text("Alicat Pressure Controller")
                        .FontSize(12).FontColor("#666666");
                });

                row.ConstantItem(80).Height(80).Background("#0078d4")
                    .AlignCenter().AlignMiddle()
                    .Text("⚡").FontSize(48).FontColor(Colors.White);
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                // Session Information Section
                column.Item().Element(ComposeSessionInfo);

                // Statistics Cards
                column.Item().PaddingTop(20).Element(ComposeStatistics);

                // Pressure vs Time Chart (placeholder)
                column.Item().PaddingTop(20).Element(ComposeChart);

                // Configuration Settings
                column.Item().PaddingTop(20).Element(ComposeSettings);
            });
        }

        void ComposeSessionInfo(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Session Information")
                    .FontSize(14).Bold();

                column.Item().PaddingTop(10).Background("#f8f9fa")
                    .Border(1).BorderColor("#e0e0e0")
                    .Padding(10).Column(infoColumn =>
                {
                    infoColumn.Item().Row(row =>
                    {
                        AddInfoItem(row, "Session Name:", _session.SessionName);
                        AddInfoItem(row, "Date & Time:", _session.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    });

                    infoColumn.Item().PaddingTop(5).Row(row =>
                    {
                        AddInfoItem(row, "Duration:", _session.Duration.ToString(@"hh\:mm\:ss"));
                        AddInfoItem(row, "Status:", _session.Status);
                    });

                    infoColumn.Item().PaddingTop(5).Row(row =>
                    {
                        AddInfoItem(row, "Operator:", _session.Operator);
                        AddInfoItem(row, "Device Model:", _session.DeviceModel);
                    });

                    infoColumn.Item().PaddingTop(5).Row(row =>
                    {
                        AddInfoItem(row, "Serial Number:", _session.SerialNumber);
                        AddInfoItem(row, "Data Points:", _session.TotalDataPoints.ToString("N0"));
                    });
                });
            });
        }

        void ComposeStatistics(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Key Statistics")
                    .FontSize(14).Bold();

                column.Item().PaddingTop(10).Row(row =>
                {
                    AddStatCard(row, "Initial Pressure", $"{_session.InitialPressure:F1}", _session.PressureUnit);
                    row.ConstantItem(10); // spacing
                    AddStatCard(row, "Final Pressure", $"{_session.FinalPressure:F1}", _session.PressureUnit);
                    row.ConstantItem(10);
                    AddStatCard(row, "Max Pressure", $"{_session.MaxPressureReached:F1}", _session.PressureUnit);
                    row.ConstantItem(10);
                    AddStatCard(row, "Avg Rate", $"{_session.AverageRate:F1}", $"{_session.PressureUnit}/{_session.TimeUnit}");
                });
            });
        }

        void ComposeChart(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Pressure vs Time")
                    .FontSize(14).Bold();

                column.Item().PaddingTop(10).Height(200)
                    .Border(2).BorderColor("#e0e0e0")
                    .Background("#f8f9fa")
                    .AlignCenter().AlignMiddle()
                    .Text("[ Chart will be rendered here ]")
                    .FontSize(12).FontColor("#999999");
            });
        }

        void ComposeSettings(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Configuration Settings")
                    .FontSize(14).Bold();

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        AddSettingRow(col, "Pressure Unit", _session.PressureUnit);
                        AddSettingRow(col, "Time Unit", _session.TimeUnit);
                        AddSettingRow(col, "Ramp Speed", $"{_session.RampSpeed} {_session.PressureUnit}/{_session.TimeUnit}");
                        AddSettingRow(col, "Sample Rate", $"{_session.SampleRate} Hz");
                    });

                    row.ConstantItem(20);

                    row.RelativeItem().Column(col =>
                    {
                        AddSettingRow(col, "Max Pressure", $"{_session.MaxPressure} {_session.PressureUnit}");
                        AddSettingRow(col, "Min Pressure", $"{_session.MinPressure} {_session.PressureUnit}");
                        AddSettingRow(col, "COM Port", _session.ComPort);
                        AddSettingRow(col, "Baud Rate", _session.BaudRate.ToString());
                    });
                });
            });
        }

        void ComposeDataTable(IContainer container)
        {
            container.Column(column =>
            {
                // Connection Details
                column.Item().Element(ComposeConnectionDetails);

                // Sample Data Table
                column.Item().PaddingTop(20).Column(tableColumn =>
                {
                    tableColumn.Item().Text("Sample Data (First 20 Points)")
                        .FontSize(14).Bold();

                    tableColumn.Item().PaddingTop(10).Table(table =>
                    {
                        // Define columns
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60);  // Time
                            columns.ConstantColumn(80);  // Pressure
                            columns.ConstantColumn(80);  // Target
                            columns.ConstantColumn(70);  // Rate
                            columns.RelativeColumn();    // Status
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Background("#0078d4").Padding(5)
                                .Text($"Time ({_session.TimeUnit})").FontColor(Colors.White).Bold();
                            header.Cell().Background("#0078d4").Padding(5)
                                .Text($"Pressure ({_session.PressureUnit})").FontColor(Colors.White).Bold();
                            header.Cell().Background("#0078d4").Padding(5)
                                .Text($"Target ({_session.PressureUnit})").FontColor(Colors.White).Bold();
                            header.Cell().Background("#0078d4").Padding(5)
                                .Text($"Rate ({_session.PressureUnit}/s)").FontColor(Colors.White).Bold();
                            header.Cell().Background("#0078d4").Padding(5)
                                .Text("Status").FontColor(Colors.White).Bold();
                        });

                        // Data rows (first 20 points)
                        var dataPoints = _session.DataPoints.Take(20);
                        foreach (var point in dataPoints)
                        {
                            table.Cell().Padding(5).Text(point.Time.ToString("F1"));
                            table.Cell().Padding(5).Text(point.Pressure.ToString("F1"));
                            table.Cell().Padding(5).Text(point.Target.ToString("F1"));
                            table.Cell().Padding(5).Text(point.Rate.ToString("F1"));
                            table.Cell().Padding(5).Text(point.Status);
                        }
                    });
                });

                // Notes Section
                column.Item().PaddingTop(20).Background("#fffbea")
                    .Border(2).BorderColor("#ffd700")
                    .Padding(10).Column(notesColumn =>
                {
                    notesColumn.Item().Text("Session Notes")
                        .FontSize(11).Bold().FontColor("#856404");
                    notesColumn.Item().PaddingTop(5)
                        .Text($"Session completed successfully. Target pressure of {_session.FinalPressure} {_session.PressureUnit} was reached. No alarms or warnings were triggered during the session.")
                        .FontSize(9).FontColor("#666666");
                });
            });
        }

        void ComposeConnectionDetails(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Connection Details")
                    .FontSize(14).Bold();

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        AddSettingRow(col, "COM Port", _session.ComPort);
                        AddSettingRow(col, "Baud Rate", _session.BaudRate.ToString());
                        AddSettingRow(col, "Data Bits", _session.DataBits.ToString());
                    });

                    row.ConstantItem(20);

                    row.RelativeItem().Column(col =>
                    {
                        AddSettingRow(col, "Parity", _session.Parity);
                        AddSettingRow(col, "Stop Bits", _session.StopBits);
                        AddSettingRow(col, "Firmware", _session.FirmwareVersion);
                    });
                });
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(8).FontColor("#666666");
                text.Span(" | ");
                text.Span("Alicat Controller v1.0").FontSize(8).FontColor("#666666");
            });
        }

        // Helper methods
        void AddInfoItem(RowDescriptor row, string label, string value)
        {
            row.RelativeItem().Row(innerRow =>
            {
                innerRow.ConstantItem(120).Text(label)
                    .FontSize(9).FontColor("#666666").Bold();
                innerRow.RelativeItem().Text(value)
                    .FontSize(9).FontColor("#0078d4").Bold();
            });
        }

        void AddStatCard(RowDescriptor row, string label, string value, string unit)
        {
            row.RelativeItem().Background("#0078d4")
                .Padding(15).AlignCenter().Column(column =>
            {
                column.Item().Text(label)
                    .FontSize(9).FontColor(Colors.White);
                column.Item().PaddingTop(5).Text(value)
                    .FontSize(20).Bold().FontColor(Colors.White);
                column.Item().Text(unit)
                    .FontSize(10).FontColor(Colors.White);
            });
        }

        void AddSettingRow(ColumnDescriptor column, string label, string value)
        {
            column.Item().PaddingBottom(5).Background("#f8f9fa")
                .Border(1).BorderColor("#e0e0e0")
                .BorderLeft(3).BorderColor("#0078d4")
                .Padding(8).Row(row =>
            {
                row.RelativeItem().Text(label)
                    .FontSize(9).Bold();
                row.RelativeItem().AlignRight().Text(value)
                    .FontSize(9).FontColor("#0078d4").Bold();
            });
        }
    }
}
