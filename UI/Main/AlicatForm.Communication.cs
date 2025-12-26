using System.Diagnostics;
using System.IO.Ports;
using Alicat.Services.Serial;
using Alicat.Services.Controllers;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormConnect { StartPosition = FormStartPosition.CenterParent };
            dlg.ShowDialog(this);

            // ✅ БЕЗ рефлексии — берём напрямую
            var opened = dlg.OpenPort;
            if (opened is null) return;

            _serial?.Dispose();
            _serial = new SerialClient(opened);
            _serial.LineReceived += Serial_LineReceived;
            _serial.Connected += (_, __) => BeginInvoke(new Action(() =>
            {
                _pollTimer.Start();
                UI_UpdateConnectionStatus(true, opened.PortName);
            }));
            _serial.Disconnected += (_, __) => BeginInvoke(new Action(() =>
            {
                _pollTimer.Stop();
                UI_UpdateConnectionStatus(false);
            }));

            _serial.Attach();
            _ramp = new RampController(_serial);
            _serial.Send("ASR");

            if (!_dataStore.IsRunning)
            {
                _dataStore.StartSession();
            }

            // Обновляем статус после подключения (Attach вызывает Connected событие, но обновим явно)
            UI_UpdateConnectionStatus(true, opened.PortName);

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
                BeginInvoke(new Action(() =>
                {
                    UI_SetRampSpeedUnits($"{TrimZeros(ramp)} {rampUnits}");
                }));

                // Это строка про скорость рампа — дальше ALS не трогаем
                return;
            }

            // 2) Пробуем распознать ответ ADCU (изменение единиц)
            // Формат ответа может быть разным, но обычно устройство отвечает на ADCU
            // и затем следующий ALS уже содержит новые единицы
            // Но если в ответе есть единицы, обновим их сразу
            if (TryParseAdcuResponse(line, out var newUnit))
            {
                if (!string.IsNullOrWhiteSpace(newUnit))
                {
                    _unit = newUnit;
                    BeginInvoke(new Action(() =>
                    {
                        UI_SetPressureUnits(_unit);
                    }));
                }
                // После ADCU обычно нужно запросить ALS для получения актуальных данных
                // Но это произойдет автоматически через polling timer
                return;
            }

            // 3) Если это не ASR и не ADCU — пробуем ALS
            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;

            _current = cur;
            if (!_isExhaust) _setPoint = sp;
            
            // Обновляем единицы, если они изменились
            bool unitsChanged = !string.IsNullOrWhiteSpace(unit) && unit != _unit;
            if (unitsChanged)
            {
                _unit = unit!;
            }

            BeginInvoke(new Action(() =>
            {
                // Если единицы изменились, обновляем их во всех местах UI
                if (unitsChanged)
                {
                    UI_SetPressureUnits(_unit);
                }
                
                UI_SetTrendStatus(_lastCurrent, _current, _isExhaust);
                RefreshCurrent();
                UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                ValidateTargetAgainstMax();
                ValidateIncrementAgainstMax();

                _state.Update(_current, _setPoint, _unit, _isExhaust);
                _lastCurrent = _current;

                // 👉 ЗАПИСЫВАЕМ В STORE (всегда, независимо от открытых окон)
                _dataStore.RecordSample(_current, _isExhaust ? 0.0 : _setPoint, _unit);

                // 👉 ОБНОВЛЯЕМ ГРАФИК, ЕСЛИ ОКНО ОТКРЫТО
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    double? targetForGraph = _isExhaust ? (double?)null : _setPoint;
                    _graphForm.AddSample(_current, targetForGraph);
                }

                if (_tableForm != null && !_tableForm.IsDisposed)
                {
                    if (ShouldLog(_current))
                    {
                        var spForLog = _isExhaust ? 0.0 : _setPoint;
                        _tableForm.AddRecordFromDevice(_current, spForLog, _unit);
                    }
                }


            }));
        }


        private bool ShouldLog(double currentPressure)
        {
            if (_tableForm == null || _tableForm.IsDisposed)
                return false;

            double threshold = _tableForm.Threshold;

            // первая запись всегда
            if (_lastLoggedPressure == null)
            {
                _lastLoggedPressure = currentPressure;
                return true;
            }

            double delta = Math.Abs(currentPressure - _lastLoggedPressure.Value);

            if (delta >= threshold)
            {
                _lastLoggedPressure = currentPressure;
                return true;
            }

            return false;
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _pollTimer.Stop();
            _dataStore.EndSession();
            _serial?.Dispose();
        }

    }
}