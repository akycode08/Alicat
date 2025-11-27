using Alicat.Domain;
using Alicat.Services.Controllers;
using Alicat.Services.Protocol;
using Alicat.Services.Serial;           // SerialClient
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Graph.Views;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        private double _current = 0.0;
        private double _setPoint = 0.0;
        private string _unit = "PSIG";

        private bool _isExhaust = false;
        private double? _lastCurrent = null;

        // ✅ теперь форма хранит ссылку на SerialClient, а не сама работает с SerialPort
        private SerialClient? _serial;
        private readonly Timer _pollTimer = new() { Interval = 500 };

        private double _maxPressure = 200.0;
        private double _maxIncrementLimit = 20.0;

        private readonly DeviceState _state = new();
        private RampController? _ramp;

        private TerminalForm? _terminalForm;
        private GraphForm? _graphForm;

        public AlicatForm()
        {
            InitializeComponent();

            // меню
            menuSettingsOptions.Click += btnOptions_Click;
            menuSettingsCommunication.Click += btnCommunication_Click;

            // навигация
            btnGraph.Click += btnGraph_Click;
            btnTable.Click += btnTable_Click;
            btnStatistics.Click += btnStatistic_Click;
            btnTerminal.Click += btnTerminal_Click;

            // управление давлением
            btnGoTarget.Click += btnGoTarget_Click;
            btnPurge.Click += btnPurge_Click;
            btnGoPlus.Click += btnGoPlus_Click;
            btnGoMinus.Click += btnGoMinus_Click;

            // валидация
            txtTarget.TextChanged += (_, __) => ValidateTargetAgainstMax();
            chkConfirmGo.CheckedChanged += (_, __) => ValidateTargetAgainstMax();
            nudIncrement.ValueChanged += (_, __) => ValidateIncrementAgainstMax();

            // начальные значения UI
            UI_SetPressureUnits(_unit);
            UI_SetRampSpeedUnits("PSIG/s");
            UI_SetSetPoint(_setPoint, _unit);
            UI_SetTimeToSetPoint(null);
            UI_Status_Up(false);
            UI_Status_Mid(false);
            UI_Status_Down(false);

            RefreshCurrent();

            // ❌ было: _serial?.RequestAls();
            // ✅ стало: форма сама не знает команд, она просит SerialClient отправить строку из AlicatCommands
            _pollTimer.Tick += (_, __) => _serial?.Send(AlicatCommands.ReadAls);

            ApplyOptionsToUi();
        }

        private void ApplyOptionsToUi()
        {
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _maxIncrementLimit = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;

            nudIncrement.DecimalPlaces = 1;
            nudIncrement.Increment = 0.1M;

            if (nudIncrement.Minimum <= 0) nudIncrement.Minimum = 0.1M;
            if (nudIncrement.Maximum < 100000M) nudIncrement.Maximum = 100000M;

            ValidateTargetAgainstMax();
            ValidateIncrementAgainstMax();
        }

        private static bool TryParseAls(string line, out double cur, out double sp, out string? unit)
        {
            cur = 0; sp = 0; unit = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out cur)) return false;
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out sp)) return false;

            for (int i = 3; i < parts.Length; i++)
            {
                var p = parts[i].Trim().ToUpperInvariant();
                if (p is "PSIG" or "PSI" or "KPA" or "BAR")
                {
                    unit = p;
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Парсер ответа ASR, например:
        /// "A 6.000001 10 4 PSIG/s"
        /// Возвращает true ТОЛЬКО если нашли единицы с "/s".
        /// </summary>
        private static bool TryParseAsr(string line, out double ramp, out string units)
        {
            ramp = 0;
            units = "PSIG/s";

            if (string.IsNullOrWhiteSpace(line))
                return false;

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return false;

            if (!parts[0].Equals("A", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out ramp))
                return false;

            string? foundUnits = null;
            for (int i = 1; i < parts.Length; i++)
            {
                var p = parts[i].Trim();
                if (p.EndsWith("/s", StringComparison.OrdinalIgnoreCase))
                {
                    foundUnits = p;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(foundUnits))
                return false;

            units = foundUnits;
            return true;
        }

        // 👉 если ниже файла у тебя есть другие методы AlicatForm — оставь их без изменений
    }
}
