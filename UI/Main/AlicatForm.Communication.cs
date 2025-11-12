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
                _serial.Connected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Start()));
                _serial.Disconnected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Stop()));

                _serial.Attach();
                _ramp = new RampController(_serial);
                _serial.RequestAls();

            }

            private void Serial_LineReceived(object? sender, string line)
            {
                Debug.WriteLine("RX: " + line);

                bool exh = line.IndexOf("EXH", StringComparison.OrdinalIgnoreCase) >= 0;
                if (exh) _isExhaust = true;

                if (!TryParseAls(line, out var cur, out var sp, out var unit))
                    return;

                _current = cur;
                if (!_isExhaust) _setPoint = sp;
                if (!string.IsNullOrWhiteSpace(unit)) _unit = unit!;

                BeginInvoke(new Action(() =>
                {
                    UI_SetTrendStatus(_lastCurrent, _current, _isExhaust);
                    RefreshCurrent();
                    UI_SetPressureUnits(_unit);
                    UI_SetSetPoint(_isExhaust ? 0.0 : _setPoint, _unit);

                    ValidateTargetAgainstMax();
                    ValidateIncrementAgainstMax();

                    _state.Update(_current, _setPoint, _unit, _isExhaust);
                    _lastCurrent = _current;
                }));
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                base.OnFormClosing(e);
                _pollTimer.Stop();
                _serial?.Dispose();
            }

    }
}