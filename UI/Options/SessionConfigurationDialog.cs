using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using PrecisionPressureController.Business.Interfaces;
using PrecisionPressureController.Services.Data;
using PrecisionPressureController.Services.Serial;
using PrecisionPressureController.Presentation.Presenters;

namespace PrecisionPressureController.UI.Options
{
    /// <summary>
    /// Форма для просмотра конфигурации сессии в формате JSON
    /// </summary>
    public partial class SessionConfigurationDialog : Form
    {
        private string _jsonContent = string.Empty;

        public SessionConfigurationDialog()
        {
            InitializeComponent();
            LoadDefaultJson();
        }

        /// <summary>
        /// Загружает JSON конфигурацию из реальных данных сессии
        /// </summary>
        public void LoadSessionData(SessionDataStore dataStore, MainPresenter? presenter, ISerialClient? serialClient)
        {
            try
            {
                var now = DateTime.UtcNow;
                var sessionStart = dataStore.SessionStart;
                var duration = dataStore.IsRunning 
                    ? (now - sessionStart) 
                    : (dataStore.Points.Count > 0 
                        ? (dataStore.Points.Last().Timestamp - sessionStart) 
                        : TimeSpan.Zero);

                // Генерируем имя сессии из даты и времени
                string sessionName = $"Session_{sessionStart:yyyy-MM-dd_HH-mm-ss}";
                
                // Получаем последнюю модификацию (время последней точки данных или текущее время)
                DateTime lastModified = dataStore.Points.Count > 0 
                    ? dataStore.Points.Last().Timestamp 
                    : sessionStart;

                // Получаем информацию о подключении
                string? portName = null;
                int? baudRate = null;
                int? dataBits = null;
                string? parity = null;
                
                if (serialClient is SerialClient serial)
                {
                    portName = serial.PortName;
                    baudRate = serial.BaudRate;
                    // SerialClient не хранит dataBits и parity, используем значения по умолчанию
                    dataBits = 8;
                    parity = "None";
                }

                // Получаем настройки измерения
                string pressureUnit = "PSIG";
                double rampSpeed = 0.0;
                int sampleRate = 500;
                
                if (dataStore.Points.Count > 0)
                {
                    var lastPoint = dataStore.Points.Last();
                    pressureUnit = lastPoint.Unit;
                    rampSpeed = lastPoint.RampSpeed;
                    sampleRate = lastPoint.PollingFrequency;
                }
                else if (presenter != null)
                {
                    pressureUnit = presenter.Unit;
                    rampSpeed = presenter.RampSpeed;
                    sampleRate = OptionsWindow.AppOptions.Current.PollingFrequency ?? 500;
                }

                // Получаем лимиты из настроек
                double maxPressure = OptionsWindow.AppOptions.Current.MaxPressure ?? 200.0;
                double minPressure = OptionsWindow.AppOptions.Current.MinPressure ?? 0.0;
                double maxIncrement = OptionsWindow.AppOptions.Current.MaxIncrement ?? 20.0;
                double minIncrement = OptionsWindow.AppOptions.Current.MinIncrement ?? 0.1;

                var sessionConfig = new
                {
                    session = new
                    {
                        name = sessionName,
                        created = sessionStart.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        lastModified = lastModified.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        duration = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}",
                        status = dataStore.IsRunning ? "active" : "completed",
                        totalDataPoints = dataStore.Points.Count,
                        csvPath = dataStore.CsvPath ?? (string?)null
                    },
                    device = new
                    {
                        model = "ALICAT Pressure Controller",
                        serialNumber = (string?)null, // Не доступно через текущий API
                        firmwareVersion = (string?)null // Не доступно через текущий API
                    },
                    connection = new
                    {
                        port = portName ?? (string?)null,
                        baudRate = baudRate ?? (int?)null,
                        dataBits = dataBits ?? (int?)null,
                        parity = parity ?? (string?)null,
                        connected = serialClient?.IsConnected ?? false
                    },
                    measurement = new
                    {
                        units = new
                        {
                            pressure = pressureUnit,
                            time = OptionsWindow.AppOptions.Current.TimeUnits ?? "s"
                        },
                        settings = new
                        {
                            rampSpeed = rampSpeed,
                            sampleRate = sampleRate
                        }
                    },
                    limits = new
                    {
                        pressure = new
                        {
                            maximum = maxPressure,
                            minimum = minPressure
                        },
                        increment = new
                        {
                            maximum = maxIncrement,
                            minimum = minIncrement
                        }
                    },
                    safety = new
                    {
                        emergencyStop = true,
                        overpressureProtection = true
                    },
                    autoSave = new
                    {
                        enabled = true, // Auto-save всегда включен в текущей реализации
                        description = "Settings are automatically saved when changed"
                    },
                    statistics = dataStore.Points.Count > 0 ? new
                    {
                        initialPressure = dataStore.Points.First().Current,
                        finalPressure = dataStore.Points.Last().Current,
                        maxPressureReached = dataStore.Points.Max(p => p.Current),
                        minPressureReached = dataStore.Points.Min(p => p.Current),
                        averagePressure = dataStore.Points.Average(p => p.Current)
                    } : (object?)null
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                _jsonContent = JsonSerializer.Serialize(sessionConfig, options);
                UpdateJsonDisplay();
            }
            catch (Exception ex)
            {
                // В случае ошибки загружаем дефолтные данные
                System.Diagnostics.Debug.WriteLine($"Error loading session data: {ex.Message}");
                LoadDefaultJson();
            }
        }

        /// <summary>
        /// Загружает JSON конфигурацию (можно заменить на реальные данные)
        /// </summary>
        public void LoadJson(string jsonContent)
        {
            _jsonContent = jsonContent;
            UpdateJsonDisplay();
        }

        private void LoadDefaultJson()
        {
            var sessionConfig = new
            {
                session = new
                {
                    name = "Session_2025-01-05_14-30",
                    created = "2025-01-05T14:30:15Z",
                    lastModified = "2025-01-05T15:15:47Z",
                    duration = "00:45:32",
                    status = "active"
                },
                device = new
                {
                    model = "ALICAT PC-15PSIG-D",
                    serialNumber = "12345-ABC-67890",
                    firmwareVersion = "v2.5.3"
                },
                connection = new
                {
                    port = "COM3",
                    baudRate = 19200,
                    dataBits = 8,
                    parity = "None"
                },
                measurement = new
                {
                    units = new
                    {
                        pressure = "PSIG",
                        time = "s"
                    },
                    settings = new
                    {
                        rampSpeed = 10.0,
                        sampleRate = 10
                    }
                },
                limits = new
                {
                    pressure = new
                    {
                        maximum = 200.0,
                        minimum = 0.0
                    },
                    increment = new
                    {
                        maximum = 20.0,
                        minimum = 0.1
                    }
                },
                safety = new
                {
                    emergencyStop = true,
                    overpressureProtection = true
                },
                autoSave = new
                {
                    enabled = true,
                    description = "Settings are automatically saved when changed"
                }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            _jsonContent = JsonSerializer.Serialize(sessionConfig, options);
            UpdateJsonDisplay();
        }

        private void UpdateJsonDisplay()
        {
            if (txtJsonContent != null)
            {
                txtJsonContent.Text = _jsonContent;
            }
        }

        private void BtnCopy_Click(object? sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_jsonContent);
                MessageBox.Show(this, "JSON content copied to clipboard.", "Copied", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to copy to clipboard: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FileName = "session_configuration.json",
                    DefaultExt = "json"
                };

                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    await File.WriteAllTextAsync(saveDialog.FileName, _jsonContent, Encoding.UTF8);
                    MessageBox.Show(this, $"Configuration saved to:\n{saveDialog.FileName}", "Saved", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save file: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

