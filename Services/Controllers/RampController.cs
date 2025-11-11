using System;
using System.Globalization;
using Alicat.Services.Protocol;
using Alicat.Services.Serial;

namespace Alicat.Services.Controllers
{
    public sealed class RampController
    {
        private readonly SerialClient _serial;

        public RampController(SerialClient serial) => _serial = serial;

        /// <summary>
        /// Устанавливает скорость рампа SR без каких-либо конвертаций.
        /// Возвращает true, если команда отправлена.
        /// </summary>
        public bool TryApply(double? pressureRamp)
        {
            if (_serial is null || pressureRamp is null) return false;

            double r = pressureRamp.Value;
            if (r < 0) r = 0;                // простая защита
            var s = r.ToString("G", CultureInfo.InvariantCulture);

            _serial.Send(AlicatCommands.SetRamp(s));
            return true;
        }
    }
}
