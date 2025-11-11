using System;
using System.IO.Ports;
using System.Text;

namespace Alicat.Services.Serial
{
    public sealed class SerialClient : IDisposable
    {
        private readonly SerialPort _port;
        private bool _attached;

        public event EventHandler? Connected;
        public event EventHandler? Disconnected;
        public event EventHandler<string>? LineReceived;

        public SerialClient(SerialPort existingOpenPort)
        {
            _port = existingOpenPort ?? throw new ArgumentNullException(nameof(existingOpenPort));
            _port.NewLine = "\r";
            _port.Encoding = Encoding.ASCII;
        }

        public void Attach()
        {
            if (_attached) return;
            _port.DataReceived += Port_DataReceived;
            _attached = true;
            Connected?.Invoke(this, EventArgs.Empty);
        }

        public void Send(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd)) return;
            if (!_port.IsOpen) return;
            try { _port.Write(cmd + "\r"); } catch { /* ignore */ }
        }

        public void RequestAls() => Send("ALS");

        private void Port_DataReceived(object? sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (_port.BytesToRead > 0)
                {
                    var line = _port.ReadLine(); // читает до CR
                    if (!string.IsNullOrWhiteSpace(line))
                        LineReceived?.Invoke(this, line);
                }
            }
            catch { /* ignore */ }
        }

        public void Dispose()
        {
            try
            {
                _port.DataReceived -= Port_DataReceived;
                if (_port.IsOpen) _port.Close();
                _port.Dispose();
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
            catch { /* ignore */ }
        }
    }
}
