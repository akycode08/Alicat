using System.Diagnostics;
using System.IO.Ports;
using PrecisionPressureController.Services.Serial;
using PrecisionPressureController.Services.Controllers;
using PrecisionPressureController.UI.Connect;

namespace PrecisionPressureController.UI.Main
{
    public partial class MainWindow
    {
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            // Сохраняем состояние подключения до открытия диалога
            bool wasConnected = _serial != null;
            
            using var dlg = new ConnectionDialog { StartPosition = FormStartPosition.CenterParent };
            dlg.ShowDialog(this);

            // ✅ БЕЗ рефлексии — берём напрямую
            var opened = dlg.OpenPort;
            
            // Если порт не открыт, но до этого было подключение - отключаем
            if (opened is null)
            {
                if (wasConnected)
                {
                    // Пользователь отключил устройство через диалог
                    _serial?.Dispose();
                    _serial = null;
                    _ramp = null;
                    _pollTimer?.Stop();
                    _isWaitingForResponse = false; // Сбрасываем флаг при отключении
                    UI_UpdateConnectionStatus(false);
                    
                    // Reset graph values to zero on disconnect
                    if (_graphForm != null && !_graphForm.IsDisposed)
                    {
                        _graphForm.ResetToZero();
                    }
                }
                return;
            }

            _serial?.Dispose();
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected += (_, __) => BeginInvoke(new Action(() =>
            {
                _pollTimer?.Start();
                UI_UpdateConnectionStatus(true, opened.PortName);
            }));
            _serial.Disconnected += (_, __) => BeginInvoke(new Action(() =>
            {
                _pollTimer?.Stop();
                UI_UpdateConnectionStatus(false);
                
                // Reset graph values to zero on disconnect
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

            // Сохраняем настройки коммуникации после успешного подключения
            SaveCommunicationSettings(dlg, opened);

            // Обновляем статус после подключения (Attach вызывает Connected событие, но обновим явно)
            UI_UpdateConnectionStatus(true, opened.PortName);
        }

        private void SaveCommunicationSettings(ConnectionDialog dlg, SerialPort opened)
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                string? directory = System.IO.Path.GetDirectoryName(settingsPath);
                
                if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                // Загружаем существующие настройки или создаем новые
                var existingSettings = new
                {
                    General = new { PressureUnits = "PSI", TimeUnits = "s", PollingFrequency = 500 },
                    Device = new { PressureRamp = (double?)null, MaxPressure = (double?)null, MinPressure = (double?)null },
                    Control = new { MaxIncrement = (double?)null, MinIncrement = (double?)null },
                    Communication = new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 },
                    LastSaved = DateTime.Now
                };

                if (System.IO.File.Exists(settingsPath))
                {
                    try
                    {
                        string jsonContent = System.IO.File.ReadAllText(settingsPath);
                        var settingsData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(jsonContent);
                        
                        // Извлекаем существующие настройки
                        if (settingsData.TryGetProperty("General", out var gen))
                        {
                            existingSettings = new
                            {
                                General = new
                                {
                                    PressureUnits = gen.TryGetProperty("PressureUnits", out var pu) ? pu.GetString() ?? "PSI" : "PSI",
                                    TimeUnits = gen.TryGetProperty("TimeUnits", out var tu) ? tu.GetString() ?? "s" : "s",
                                    PollingFrequency = gen.TryGetProperty("PollingFrequency", out var pf) && pf.ValueKind != System.Text.Json.JsonValueKind.Null ? pf.GetInt32() : 500
                                },
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
                                Communication = new { PortName = (string?)null, BaudRate = 19200, Parity = "None", StopBits = "One", DataBits = 8, ReadTimeout = 700, WriteTimeout = 700 },
                                LastSaved = DateTime.Now
                            };
                        }
                    }
                    catch { }
                }

                // Обновляем настройки коммуникации
                var updatedSettings = new
                {
                    existingSettings.General,
                    existingSettings.Device,
                    existingSettings.Control,
                    Communication = new
                    {
                        PortName = dlg.PortName,
                        BaudRate = dlg.BaudRate,
                        Parity = dlg.Parity,
                        StopBits = dlg.StopBits,
                        DataBits = dlg.DataBits,
                        ReadTimeout = dlg.ReadTimeout,
                        WriteTimeout = dlg.WriteTimeout
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


        private void Serial_LineReceived(object? sender, string line)
        {
            Debug.WriteLine("RX: " + line);

            bool exh = line.IndexOf("EXH", StringComparison.OrdinalIgnoreCase) >= 0;
            if (exh) _isExhaust = true;

            // ЛОГ ТОЛЬКО ЕСЛИ ТЕРМИНАЛ ЖИВОЙ
            if (_terminalForm != null && !_terminalForm.IsDisposed)
            {
                _terminalForm.AppendLog("<< " + line);
            }

            // 1) Сначала пробуем распознать ответ ASR (Ramp Speed)
            if (TryParseAsr(line, out var ramp, out var rampUnits))
            {
                _rampSpeed = ramp; // Сохраняем значение скорости рампы
                // Сбрасываем флаг ожидания ответа
                _isWaitingForResponse = false;
                BeginInvoke(new Action(() =>
                {
                    UI_SetRampSpeedUnits($"{TrimZeros(ramp)} {rampUnits}");
                }));

                // Это строка про скорость рампа — дальше ALS не трогаем
                return;
            }

            // 2) Если это не ASR — пробуем ALS
            if (!TryParseAls(line, out var cur, out var sp, out var unit))
            {
                // Если это не ASR и не ALS, сбрасываем флаг ожидания
                // (на случай, если получен другой ответ)
                _isWaitingForResponse = false;
                return;
            }

            _current = cur;
            if (!_isExhaust) _setPoint = sp;
            if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

            // Сбрасываем флаг ожидания ответа
            _isWaitingForResponse = false;

            BeginInvoke(new Action(() =>
            {
                UI_SetTrendStatus(_lastCurrent, _current, _isExhaust, _rampSpeed);
                RefreshCurrent();
                UI_SetPressureUnits(_unit);
                UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                ValidateTargetAgainstMax();
                ValidateIncrementAgainstMax();

                _state.Update(_current, _setPoint, _unit, _isExhaust);
                _lastCurrent = _current;

                // Обновляем "Last update" на основе интервала таймера
                UpdateLastUpdateText();

                // 👉 ЗАПИСЫВАЕМ В STORE (всегда, независимо от открытых окон)
                // Используем старую версию без дополнительных параметров для обратной совместимости
                // В presenter версии используется новая версия с rampSpeed и pollingFrequency
                // PointIndex = 0 (так как в этом файле нет доступа к SequenceService)
                DataStore.RecordSample(_current, _isExhaust ? 0.0 : _setPoint, _unit, 0.0, 500, 0);

                // 👉 ОБНОВЛЯЕМ ГРАФИК, ЕСЛИ ОКНО ОТКРЫТО
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    double? targetForGraph = _isExhaust ? (double?)null : _setPoint;
                    _graphForm.AddSample(_current, targetForGraph);
                }

                // DataTableWindow получает данные через события DataStore.OnNewPoint
                // Не нужно вызывать AddRecordFromDevice напрямую


            }));
        }

        // OnFormClosing moved to MainWindow.Presenter.cs to avoid duplication

    }
}