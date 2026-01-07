using System;
using System.IO.Ports;
using System.Text;
using Alicat.Business.Interfaces;

namespace Alicat.Services.Serial
{
    public sealed class SerialClient : ISerialClient, IDisposable
    {
        private readonly SerialPort _port;
        private bool _attached;

        public event EventHandler? Connected;
        public event EventHandler? Disconnected;
        public event EventHandler<string>? LineReceived;

        // Port information
        public string PortName => _port.PortName;
        public int BaudRate => _port.BaudRate;
        public bool IsConnected
        {
            get
            {
                try
                {
                    return _port != null && _port.IsOpen;
                }
                catch
                {
                    return false;
                }
            }
        }

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
            if (!_port.IsOpen)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] SerialClient.Send: Port {_port.PortName} is not open");
                return;
            }
            try 
            { 
                _port.Write(cmd + "\r");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] TX: {cmd}");
            } 
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] SerialClient.Send error: {ex.Message}");
            }
        }

        //public void RequestAls() => Send("ALS");

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
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] SerialClient.Dispose: Closing port {_port.PortName}");
                _port.DataReceived -= Port_DataReceived;
                if (_port.IsOpen) _port.Close();
                _port.Dispose();
                Disconnected?.Invoke(this, EventArgs.Empty);
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] SerialClient.Dispose: Port {_port.PortName} closed and Disconnected event fired");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] SerialClient.Dispose error: {ex.Message}");
            }
        }
    }
}
