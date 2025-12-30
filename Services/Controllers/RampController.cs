using System;
using System.Globalization;
using Alicat.Business.Interfaces;
using Alicat.Services.Protocol;

namespace Alicat.Services.Controllers
{
    public sealed class RampController : IRampController
    {
        private readonly ISerialClient _serial;

        public RampController(ISerialClient serial) => _serial = serial;

        /// <summary>
        /// Устанавливает скорость рампа SR без каких-либо конвертаций.
        /// Возвращает true, если команда отправлена.
        /// </summary>
        public bool TryApply(double? pressureRamp)
        {
            if (_serial is null || pressureRamp is null) return false;

            double r = pressureRamp.Value;
            if (r < 0) r = 0;                // простая защита

            _serial.Send(AlicatCommands.SetRamp(r, 4)); // 4 = секунды
            return true;
        }
    }
}
