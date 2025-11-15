using System;                           // базовые типы
using System.Diagnostics;
using System.Globalization;             // парс чисел Invariant
using System.IO.Ports;                  // SerialPort
using System.Text;                      // Encoding.ASCII
using System.Threading.Tasks;           // Task.Delay (на будущее)
using System.Windows.Forms;             // WinForms
using Alicat.Services.Serial;           // ✅ внешний SerialClient
using Alicat.Domain;
using Alicat.Services.Controllers;
using Alicat.Services.Protocol;
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

        private SerialClient? _serial;
        private readonly Timer _pollTimer = new() { Interval = 500 };

        private double _maxPressure = 200.0;
        private double _maxIncrementLimit = 20.0;

        private readonly DeviceState _state = new();
        private RampController? _ramp;


        public AlicatForm()
        {
            InitializeComponent();

            menuSettingsOptions.Click += btnOptions_Click;
            menuSettingsCommunication.Click += btnCommunication_Click;
            btnGoTarget.Click += btnGoTarget_Click;
            btnPurge.Click += btnPurge_Click;
            btnGoPlus.Click += btnGoPlus_Click;
            btnGoMinus.Click += btnGoMinus_Click;
          

            txtTarget.TextChanged += (_, __) => ValidateTargetAgainstMax();
            chkConfirmGo.CheckedChanged += (_, __) => ValidateTargetAgainstMax();
            nudIncrement.ValueChanged += (_, __) => ValidateIncrementAgainstMax();

            UI_SetPressureUnits(_unit);
            UI_SetRampSpeedUnits("PSIG/s");
            UI_SetSetPoint(_setPoint, _unit);
            UI_SetTimeToSetPoint(null);
            UI_Status_Up(false);
            UI_Status_Mid(false);
            UI_Status_Down(false);

            RefreshCurrent();

            _pollTimer.Tick += (_, __) => _serial?.RequestAls();
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
                if (p is "PSIG" or "PSI" or "KPA" or "BAR") { unit = p; break; }
            }
            return true;
        }
     }
}
