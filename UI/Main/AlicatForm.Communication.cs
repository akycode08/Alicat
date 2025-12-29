using System.Diagnostics;
using System.IO.Ports;
using Alicat.Services.Serial;
using Alicat.Services.Controllers;

namespace Alicat
{
    public partial class AlicatForm
    {
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            // Сохраняем состояние подключения до открытия диалога
            bool wasConnected = _serial != null;
            
            using var dlg = new FormConnect { StartPosition = FormStartPosition.CenterParent };
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
                    _pollTimer.Stop();
                    _isWaitingForResponse = false; // Сбрасываем флаг при отключении
                    UI_UpdateConnectionStatus(false);
                }
                return;
            }

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

            if (!DataStore.IsRunning)
            {
                DataStore.StartSession();
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
                DataStore.RecordSample(_current, _isExhaust ? 0.0 : _setPoint, _unit);

                // 👉 ОБНОВЛЯЕМ ГРАФИК, ЕСЛИ ОКНО ОТКРЫТО
                if (_graphForm != null && !_graphForm.IsDisposed)
                {
                    double? targetForGraph = _isExhaust ? (double?)null : _setPoint;
                    _graphForm.AddSample(_current, targetForGraph);
                }

                // TableForm получает данные через события DataStore.OnNewPoint
                // Не нужно вызывать AddRecordFromDevice напрямую


            }));
        }

        // OnFormClosing moved to AlicatForm.Presenter.cs to avoid duplication

    }
}