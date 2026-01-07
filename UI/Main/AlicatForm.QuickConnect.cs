using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using Alicat.Services.Serial;
using Alicat.Services.Controllers;

namespace Alicat
{
    public partial class AlicatForm
    {
        /// <summary>
        /// Быстрое подключение используя последние сохраненные настройки порта
        /// </summary>
        private void QuickConnect()
        {
            try
            {
                // Используем presenter для подключения, если он доступен
                if (_presenter != null)
                {
                    _presenter.QuickConnect(this);
                    return;
                }

                // Fallback: старая логика, если presenter недоступен
                // Загружаем сохраненные настройки
                var commSettings = LoadCommunicationSettingsFromFileStatic();
                
                // Получаем PortName через рефлексию (так как это анонимный тип)
                string? portName = null;
                int baudRate = 19200;
                string parity = "None";
                string stopBits = "One";
                int dataBits = 8;
                int readTimeout = 700;
                int writeTimeout = 700;

                var portNameProp = commSettings.GetType().GetProperty("PortName");
                if (portNameProp != null)
                {
                    portName = portNameProp.GetValue(commSettings) as string;
                }

                var baudRateProp = commSettings.GetType().GetProperty("BaudRate");
                if (baudRateProp != null && baudRateProp.GetValue(commSettings) is int br)
                {
                    baudRate = br;
                }

                var parityProp = commSettings.GetType().GetProperty("Parity");
                if (parityProp != null)
                {
                    parity = parityProp.GetValue(commSettings)?.ToString() ?? "None";
                }

                var stopBitsProp = commSettings.GetType().GetProperty("StopBits");
                if (stopBitsProp != null)
                {
                    stopBits = stopBitsProp.GetValue(commSettings)?.ToString() ?? "One";
                }

                var dataBitsProp = commSettings.GetType().GetProperty("DataBits");
                if (dataBitsProp != null && dataBitsProp.GetValue(commSettings) is int db)
                {
                    dataBits = db;
                }

                var readTimeoutProp = commSettings.GetType().GetProperty("ReadTimeout");
                if (readTimeoutProp != null && readTimeoutProp.GetValue(commSettings) is int rt)
                {
                    readTimeout = rt;
                }

                var writeTimeoutProp = commSettings.GetType().GetProperty("WriteTimeout");
                if (writeTimeoutProp != null && writeTimeoutProp.GetValue(commSettings) is int wt)
                {
                    writeTimeout = wt;
                }

                // Если порт не сохранен, показываем диалог
                if (string.IsNullOrEmpty(portName))
                {
                    MessageBox.Show(this,
                        "No saved port settings found. Please use 'Connect...' to configure and save port settings first.",
                        "Quick Connect",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    btnCommunication_Click(this, EventArgs.Empty);
                    return;
                }

                // Проверяем, доступен ли порт
                if (!SerialPort.GetPortNames().Contains(portName))
                {
                    var result = MessageBox.Show(this,
                        $"Port {portName} is not available.\n\nWould you like to open connection dialog?",
                        "Port Not Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        btnCommunication_Click(this, EventArgs.Empty);
                    }
                    return;
                }

                // Создаем SerialPort из сохраненных настроек
                var port = new SerialPort(portName, baudRate)
                {
                    Parity = ParseParity(parity),
                    StopBits = ParseStopBits(stopBits),
                    DataBits = dataBits,
                    ReadTimeout = readTimeout,
                    WriteTimeout = writeTimeout
                };

                // Пытаемся подключиться
                ConnectToPort(port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Quick Connect failed:\n{ex.Message}\n\nWould you like to open connection dialog?",
                    "Quick Connect Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error);
                
                if (MessageBox.Show(this,
                    "Would you like to open connection dialog?",
                    "Quick Connect Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnCommunication_Click(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Подключается к указанному порту
        /// </summary>
        private void ConnectToPort(SerialPort port)
        {
            try
            {
                _serial?.Dispose();
                _serial = new SerialClient(port);
                _serial.LineReceived += Serial_LineReceived;
                _serial.Connected += (_, __) => BeginInvoke(new Action(() =>
                {
                    _pollTimer.Start();
                    UI_UpdateConnectionStatus(true, port.PortName);
                }));
                _serial.Disconnected += (_, __) => BeginInvoke(new Action(() =>
                {
                    _pollTimer.Stop();
                    UI_UpdateConnectionStatus(false);
                    
                    if (_graphForm != null && !_graphForm.IsDisposed)
                    {
                        _graphForm.ResetToZero();
                    }
                }));

                _serial.Attach();
                _ramp = new RampController(_serial);
                _serial.Send("ASR");

                if (!DataStore.IsRunning)
                {
                    DataStore.StartSession();
                }

                // Сохраняем настройки после успешного подключения
                SaveCommunicationSettingsFromPort(port);

                UI_UpdateConnectionStatus(true, port.PortName);
                UI_AppendStatusInfo($"Quick connected to {port.PortName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to {port.PortName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Сохраняет настройки коммуникации из SerialPort
        /// </summary>
        private void SaveCommunicationSettingsFromPort(SerialPort port)
        {
            try
            {
                // Используем рефлексию для вызова private static метода GetSettingsFilePath
                var method = typeof(AlicatForm).GetMethod("GetSettingsFilePath", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                string settingsPath = method != null 
                    ? method.Invoke(null, null)?.ToString() ?? System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Settings", "settings.json")
                    : System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Settings", "settings.json");
                string? directory = System.IO.Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                // Загружаем существующие настройки
                var existingSettings = LoadAllSettingsFromFile();

                // Обновляем настройки коммуникации
                var updatedSettings = new
                {
                    existingSettings.General,
                    existingSettings.Device,
                    existingSettings.Control,
                    Communication = new
                    {
                        PortName = port.PortName,
                        BaudRate = port.BaudRate,
                        Parity = port.Parity.ToString(),
                        StopBits = port.StopBits.ToString(),
                        DataBits = port.DataBits,
                        ReadTimeout = port.ReadTimeout,
                        WriteTimeout = port.WriteTimeout
                    },
                    LastSaved = DateTime.Now
                };

                string json = System.Text.Json.JsonSerializer.Serialize(updatedSettings, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                System.IO.File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save communication settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает настройки коммуникации из файла (вызывает метод из Presenter.cs)
        /// </summary>
        private object LoadCommunicationSettingsFromFileStatic()
        {
            // Используем рефлексию для вызова private static метода из другого partial class
            var method = typeof(AlicatForm).GetMethod("LoadCommunicationSettingsFromFile", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                return method.Invoke(null, null) ?? new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 };
            }
            return new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 };
        }
        

        /// <summary>
        /// Загружает все настройки из файла
        /// </summary>
        private dynamic LoadAllSettingsFromFile()
        {
            try
            {
                // Используем рефлексию для вызова private static метода GetSettingsFilePath
                var method = typeof(AlicatForm).GetMethod("GetSettingsFilePath", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                string settingsPath = method != null 
                    ? method.Invoke(null, null)?.ToString() ?? System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Settings", "settings.json")
                    : System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Settings", "settings.json");
                if (!System.IO.File.Exists(settingsPath))
                {
                    return new
                    {
                        General = new { PressureUnits = "PSI", TimeUnits = "s", PollingFrequency = 500 },
                        Device = new { PressureRamp = (double?)null, MaxPressure = (double?)null, MinPressure = (double?)null },
                        Control = new { MaxIncrement = (double?)null, MinIncrement = (double?)null },
                        Communication = new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 }
                    };
                }

                string json = System.IO.File.ReadAllText(settingsPath);
                var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                return new
                {
                    General = settingsData.TryGetProperty("General", out var gen) ? new
                    {
                        PressureUnits = gen.TryGetProperty("PressureUnits", out var pu) ? pu.GetString() ?? "PSI" : "PSI",
                        TimeUnits = gen.TryGetProperty("TimeUnits", out var tu) ? tu.GetString() ?? "s" : "s",
                        PollingFrequency = gen.TryGetProperty("PollingFrequency", out var pf) && pf.ValueKind != System.Text.Json.JsonValueKind.Null ? pf.GetInt32() : 500
                    } : new { PressureUnits = "PSI", TimeUnits = "s", PollingFrequency = 500 },
                    Device = settingsData.TryGetProperty("Device", out var dev) ? new
                    {
                        PressureRamp = dev.TryGetProperty("PressureRamp", out var pr) && pr.ValueKind != System.Text.Json.JsonValueKind.Null ? pr.GetDouble() : (double?)null,
                        MaxPressure = dev.TryGetProperty("MaxPressure", out var mp) && mp.ValueKind != System.Text.Json.JsonValueKind.Null ? mp.GetDouble() : (double?)null,
                        MinPressure = dev.TryGetProperty("MinPressure", out var minp) && minp.ValueKind != System.Text.Json.JsonValueKind.Null ? minp.GetDouble() : (double?)null
                    } : new { PressureRamp = (double?)null, MaxPressure = (double?)null, MinPressure = (double?)null },
                    Control = settingsData.TryGetProperty("Control", out var ctrl) ? new
                    {
                        MaxIncrement = ctrl.TryGetProperty("MaxIncrement", out var mi) && mi.ValueKind != System.Text.Json.JsonValueKind.Null ? mi.GetDouble() : (double?)null,
                        MinIncrement = ctrl.TryGetProperty("MinIncrement", out var mini) && mini.ValueKind != System.Text.Json.JsonValueKind.Null ? mini.GetDouble() : (double?)null
                    } : new { MaxIncrement = (double?)null, MinIncrement = (double?)null },
                    Communication = LoadCommunicationSettingsFromFileStatic()
                };
            }
            catch
            {
                return new
                {
                    General = new { PressureUnits = "PSI", TimeUnits = "s", PollingFrequency = 500 },
                    Device = new { PressureRamp = (double?)null, MaxPressure = (double?)null, MinPressure = (double?)null },
                    Control = new { MaxIncrement = (double?)null, MinIncrement = (double?)null },
                    Communication = LoadCommunicationSettingsFromFileStatic()
                };
            }
        }

        /// <summary>
        /// Парсит строку Parity в enum
        /// </summary>
        private static Parity ParseParity(string parity)
        {
            return parity?.ToUpperInvariant() switch
            {
                "NONE" => Parity.None,
                "ODD" => Parity.Odd,
                "EVEN" => Parity.Even,
                "MARK" => Parity.Mark,
                "SPACE" => Parity.Space,
                _ => Parity.None
            };
        }

        /// <summary>
        /// Парсит строку StopBits в enum
        /// </summary>
        private static StopBits ParseStopBits(string stopBits)
        {
            return stopBits?.ToUpperInvariant() switch
            {
                "ONE" => StopBits.One,
                "TWO" => StopBits.Two,
                "ONEPOINTFIVE" => StopBits.OnePointFive,
                _ => StopBits.One
            };
        }
    }
}

