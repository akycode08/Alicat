using Alicat.Domain;
using Alicat.Services.Controllers;
using Alicat.Services.Protocol;
using Alicat.Services.Serial;
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alicat.Services.Data;
using Alicat.Presentation.Presenters;
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    /// <summary>
    /// Главная форма приложения Alicat Controller.
    /// Partial class: основная логика, поля, конструктор.
    /// </summary>
    public partial class AlicatForm : Form, IMainView
    {
        // ====================================================================
        // ПОЛЯ
        // ====================================================================
        private double _current = 0.0;
        private double _setPoint = 0.0;
        private string _unit = "PSIG";
        private double _rampSpeed = 0.0; // Current ramp speed value

        private bool _isExhaust = false;
        private bool _isPaused = false;
        private double? _lastCurrent = null;

        private SerialClient? _serial;
        private System.Timers.Timer? _pollTimer; // Используем System.Timers.Timer для более точного polling
        private bool _isWaitingForResponse = false; // Защита от переполнения при малых интервалах

        private double _maxPressure = 200.0;
        private double _minPressure = 0.0;
        private double _maxIncrementLimit = 20.0;
        private double _minIncrementLimit = 0.1;
        private double _currentIncrement = 5.0;
        private string _lastValidIncrementText = "5.0"; // Храним последнее валидное значение для отката

        private readonly DeviceState _state = new();
        private RampController? _ramp;

        // _dataStore moved to AlicatForm.Presenter.cs to avoid ambiguity
        // private readonly SessionDataStore _dataStore = new();

        private TerminalForm? _terminalForm;
        private GraphForm? _graphForm;
        private TableForm? _tableForm;
        private Alicat.UI.Features.Test.FormTestPressure? _testPressureForm;
        
        // Session management
        private SessionData? _currentSession;
        private string? _currentSessionFilePath;
        private bool _isReadOnlyMode = false;
        
        // Recent Sessions (max 3 items)
        private readonly List<string> _recentSessions = new();

        // ====================================================================
        // КОНСТРУКТОР
        // ====================================================================
        public AlicatForm()
        {
            InitializeComponent();

            // ✅ Create logo AFTER InitializeComponent (not in Designer!)
            CreateLogo();

            // Инициализируем Presenter
            InitializePresenter();
            
            // Загружаем настройки ДО применения к UI и проверки автоподключения
            LoadSettingsFromFile();
            
            // Загружаем список Recent Sessions
            LoadRecentSessionsFromSettings();
            
            // Подписываемся на событие завершения сессии для автоматического сохранения
            DataStore.OnSessionEnded += DataStore_OnSessionEnded;

            // Меню
            menuSettingsPreferences.Click += btnOptions_Click_Presenter;
            menuDeviceConnect.Click += btnCommunication_Click_Presenter;
            menuDeviceQuickConnect.Click += menuDeviceQuickConnect_Click;
            menuDeviceDisconnect.Click += menuDeviceDisconnect_Click;
            menuDeviceInfo.Click += menuDeviceInfo_Click;
            menuFileNewSession.Click += menuFileNewSession_Click_Presenter;
            menuFileOpenSession.Click += MenuFileOpenSession_Click;
            menuFileSaveSession.Click += MenuFileSaveSession_Click;
            menuFileSaveSessionAs.Click += MenuFileSaveSessionAs_Click;
            menuFileExportTable.Click += MenuFileExportTable_Click;
            menuFileExportGraphImage.Click += MenuFileExportGraphImage_Click;
            menuFileExportSessionReport.Click += MenuFileExportSessionReport_Click;
            menuFileRecentSessionsItem1.Click += MenuFileRecentSession_Click;
            menuFileRecentSessionsItem2.Click += MenuFileRecentSession_Click;
            menuFileRecentSessionsItem3.Click += MenuFileRecentSession_Click;
            menuFileRecentSessionsClearList.Click += MenuFileRecentSessionsClearList_Click;
            menuFileSessionConfiguration.Click += MenuFileSessionConfiguration_Click;
            menuFileExit.Click += menuFileExit_Click;
            menuHelpAboutDACTools.Click += menuHelpAboutDACTools_Click;
            menuHelpAboutAlicat.Click += menuHelpAboutAlicat_Click;

            // Навигация
            btnGraph.Click += btnGraph_Click_Presenter;
            btnTable.Click += btnTable_Click_Presenter;
            btnTerminal.Click += btnTerminal_Click_Presenter;

            // Управление давлением
            btnGoToTarget.Click += btnGoTarget_Click_Presenter;
            btnPause.Click += btnPause_Click_Presenter;
            btnPurge.Click += btnPurge_Click_Presenter;
            btnIncrease.Click += btnIncrease_Click_Presenter;
            btnDecrease.Click += btnDecrease_Click_Presenter;
            btnIncrementMinus.Click += btnIncrementMinus_Click_Presenter;
            btnIncrementPlus.Click += btnIncrementPlus_Click_Presenter;

            // Валидация
            txtTargetInput.TextChanged += (_, __) => ValidateTargetAgainstMax();

            txtIncrement.TextChanged += ValidateIncrementAgainstMaxPressure;

            // Начальные значения UI
            UI_SetPressureUnits(_unit);
            UI_SetSetPoint(_setPoint, _unit);
            RefreshCurrent();
            
            // Инициализируем последнее валидное значение increment
            _lastValidIncrementText = _currentIncrement.ToString("F1", CultureInfo.InvariantCulture);

            // Polling timer - работает всегда, даже при паузе
            // Pause останавливает только рампу, но не мониторинг
            // Используем System.Timers.Timer для более точного polling (не зависит от UI потока)
            _pollTimer = new System.Timers.Timer(500);
            _pollTimer.AutoReset = true;
            _pollTimer.Elapsed += (_, __) =>
            {
                // Не отправляем новый запрос, пока не получен ответ на предыдущий
                // Это защищает от переполнения при малых интервалах (например, 10мс)
                if (_serial != null && _serial.IsConnected && !_isWaitingForResponse)
                {
                    _isWaitingForResponse = true;
                    _serial.Send(AlicatCommands.ReadAls);
                }
            };
            _pollTimer.Start();

            // Загружаем сохраненные настройки ПЕРЕД применением к UI
            LoadSettingsFromFile();
            
            ApplyOptionsToUi();
            
            txtIncrement.TextChanged += txtIncrement_TextChanged_Presenter;


            // Применяем тему после инициализации (цвета и стили из AlicatForm.Theme.cs)
            // Всегда используем темную тему по умолчанию
            ApplyDarkTheme();

            // Устанавливаем правильный статус подключения ПОСЛЕ применения темы
            // (чтобы тема не перезаписала цвет индикатора)
            UI_UpdateConnectionStatus(false);

            // Auto-connect on startup if enabled (проверяем ПОСЛЕ загрузки настроек)
            // Используем Shown событие для автоподключения после полной загрузки формы
            this.Shown += (sender, e) =>
            {
                if (FormOptions.AppOptions.Current.AutoConnectOnStartup)
                {
                    // Небольшая задержка для полной инициализации формы
                    System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                if (_presenter != null)
                                {
                                    _presenter.QuickConnect(this);
                                }
                                else
                                {
                                    QuickConnect();
                                }
                            }));
                        }
                        else
                        {
                            if (_presenter != null)
                            {
                                _presenter.QuickConnect(this);
                            }
                            else
                            {
                                QuickConnect();
                            }
                        }
                    });
                }
            };
        }

        // ====================================================================
        // OPTIONS
        // ====================================================================
        private void ApplyOptionsToUi()
        {
            _maxPressure = FormOptions.AppOptions.Current.MaxPressure ?? 200.0;
            _minPressure = FormOptions.AppOptions.Current.MinPressure ?? 0.0;
            _maxIncrementLimit = FormOptions.AppOptions.Current.MaxIncrement ?? 20.0;
            _minIncrementLimit = FormOptions.AppOptions.Current.MinIncrement ?? 0.1;

            // Update Ramp Speed from Preferences
            // НЕ перезаписываем значение, если устройство подключено и уже получило значение от устройства
            // Значение должно обновляться только от устройства через ASR
            // Если устройство не подключено, используем значение из Preferences для отображения
            if (_serial == null || !_serial.IsConnected || _rampSpeed < 0.001)
            {
                var rampSpeed = FormOptions.AppOptions.Current.PressureRamp;
                if (rampSpeed.HasValue && rampSpeed.Value > 0.001)
                {
                    _rampSpeed = rampSpeed.Value;
                    UI_SetRampSpeedUnits($"{TrimZeros(_rampSpeed)} {_unit}/s");
                }
            }
            else
            {
                // Если устройство подключено и значение уже есть, просто обновляем отображение с текущими единицами
                if (_rampSpeed > 0.001)
                {
                    UI_SetRampSpeedUnits($"{TrimZeros(_rampSpeed)} {_unit}/s");
                }
            }

            // Update Polling Frequency from Preferences
            var pollingFreq = FormOptions.AppOptions.Current.PollingFrequency ?? 500;
            bool wasRunning = _pollTimer?.Enabled ?? false;
            _pollTimer?.Stop();
            if (_pollTimer != null)
            {
                _pollTimer.Interval = pollingFreq;
                if (wasRunning)
                {
                    _pollTimer.Start();
                }
            }

            ValidateTargetAgainstMax();
            ValidateIncrementAgainstMax();
            UpdateIncrementButtons();

            // Update System Settings display
            lblMaxPressureValue.Text = $"{_maxPressure:F0} {_unit}";
            lblMaxIncrementValue.Text = $"{_maxIncrementLimit:F1} {_unit}";
        }

        // ====================================================================
        // PARSERS (используются в Communication.cs)
        // ====================================================================
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

                // Поддерживаемые единицы измерения давления из таблицы Alicat
                // Устройство возвращает единицы с "G" в конце (barG, kPaG, PSIG и т.д.)
                // 0: Unit not specified (пустая единица)
                // 1: Unknown unit ("---")
                // 2: Pa / PaG
                // 3: hPa / hPaG
                // 4: kPa / kPaG
                // 5: MPa / MPaG
                // 6: mbar / mbarG
                // 7: bar / barG
                // 8: g/cm² / g/cm²G
                // 9: kg/cm / kg/cmG
                // 10: PSI / PSIG
                // 11: PSF / PSFG
                // 12: mTorr / mTorrG
                // 13: torr / torrG


                // Проверяем известные единицы (избегаем символа ² в pattern matching)
                bool isKnownUnit = p == "PA" || p == "PAG" || p == "HPA" || p == "HPAG" ||
                                   p == "KPA" || p == "KPAG" || p == "MPA" || p == "MPAG" ||
                                   p == "MBAR" || p == "MBARG" || p == "BAR" || p == "BARG" ||
                                   p == "KG/CM" || p == "KGCM" || p == "KG/CMG" || p == "KGCMG" ||
                                   p == "PSIG" || p == "PSI" || p == "PSFG" || p == "PSF" ||
                                   p == "MTORR" || p == "MTORRG" || p == "TORR" || p == "TORRG" ||
                                   p == "---" || p == "" ||
                                   p.StartsWith("G/CM") || p.StartsWith("GCM");

                if (isKnownUnit)
                {
                    // Нормализуем единицы к стандартному виду (убираем "G" в конце)
                    unit = NormalizeUnit(p);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Нормализует единицу измерения к стандартному виду для отображения.
        /// Убирает "G" в конце для единиц, которые не должны его иметь (barG → bar, kPaG → kPa).
        /// </summary>
        private static string NormalizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit == "---" || unit == "")
                return "PSIG"; // Default unit

            var upper = unit.ToUpperInvariant();

            // Убираем "G" в конце, если есть (кроме PSIG, который уже содержит G)
            if (upper.EndsWith("G") && upper != "PSIG" && upper != "PSFG")
            {
                upper = upper.Substring(0, upper.Length - 1);
            }


            // Обработка вариантов g/cm²
            if (upper == "G/CM²" || upper == "G/CM2" || upper == "GCM²" || upper == "GCM2")
                return "g/cm²";

            // Обработка вариантов kg/cm
            if (upper == "KG/CM" || upper == "KGCM")
                return "kg/cm";


            // Обработка вариантов g/cm²
            if (upper == "G/CM²" || upper == "G/CM2" || upper == "GCM²" || upper == "GCM2")
                return "g/cm²";

            // Обработка вариантов kg/cm
            if (upper == "KG/CM" || upper == "KGCM")
                return "kg/cm";


            return upper switch
            {
                "PA" => "Pa",
                "HPA" => "hPa",
                "KPA" => "kPa",
                "MPA" => "MPa",
                "MBAR" => "mbar",
                "BAR" => "bar",
                "PSIG" => "PSIG",
                "PSI" => "PSI",
                "PSFG" => "PSF",
                "PSF" => "PSF",
                "MTORR" => "mTorr",
                "TORR" => "torr",
                _ => unit // Возвращаем как есть, если не распознали
            };
        }

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

            // Извлекаем единицу без "/s", нормализуем и добавляем "/s" обратно
            var unitWithoutSlash = foundUnits.Substring(0, foundUnits.Length - 2).Trim();
            var normalizedUnit = NormalizeUnit(unitWithoutSlash);
            units = $"{normalizedUnit}/s";

            return true;
        }


        // ====================================================================
        // VALIDATION
        // ====================================================================

        /// <summary>
        /// Валидация целевого значения давления против максимального.
        /// Показывает красный цвет только если значение превышает максимум.
        /// Проверка минимума выполняется только при нажатии кнопки "Go to Target".
        /// </summary>
        public void ValidateTargetAgainstMax()
        {
            var text = txtTargetInput.Text?.Trim();
            bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetVal);
            // Показываем красный цвет только при превышении максимума
            bool over = parsed && targetVal > _maxPressure;

            if (isDarkTheme)
            {
                txtTargetInput.BackColor = over ? System.Drawing.Color.FromArgb(60, 20, 20) : darkBgWindow;
            }
            else
            {
                txtTargetInput.BackColor = over ? System.Drawing.Color.MistyRose : lightBgWindow;
            }
        }

        /// <summary>
        /// Валидация increment против максимального и минимального лимита.
        /// Вызывается из Communication.cs при получении данных.
        /// </summary>
        public void ValidateIncrementAgainstMax()
        {
            bool overLimit = _currentIncrement > _maxIncrementLimit;
            bool underLimit = _currentIncrement < _minIncrementLimit;
            bool invalid = overLimit || underLimit;

            if (isDarkTheme)
            {
                txtIncrement.BackColor = invalid ? System.Drawing.Color.FromArgb(60, 20, 20) : darkBgWindow;
            }
            else
            {
                txtIncrement.BackColor = invalid ? System.Drawing.Color.MistyRose : lightBgWindow;
            }

            // Опционально: можно отключать кнопки, если превышен лимит
            // btnIncrease.Enabled = !overLimit;
            // btnDecrease.Enabled = !overLimit;
        }

        /// <summary>
        /// Валидация increment поля против максимального лимита (MaxIncrementLimit) при изменении текста.
        /// Не позволяет ввести значение больше максимального - откатывает к предыдущему валидному или к максимуму.
        /// </summary>
        public void ValidateIncrementAgainstMaxPressure(object? sender, EventArgs e)
        {
            var text = txtIncrement.Text?.Trim();
            bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double incrementVal);
            
            // Проверяем против MaxIncrementLimit (20 PSI) - это лимит для increment
            bool over = parsed && incrementVal > _maxIncrementLimit;
            bool under = parsed && incrementVal < _minIncrementLimit;
            bool invalid = over || under;

            if (invalid)
            {
                // Показываем сообщение об ошибке (используем Tag для предотвращения повторных сообщений)
                if (txtIncrement.Tag?.ToString() != "error_shown")
                {
                    System.Media.SystemSounds.Beep.Play();
                    string errorMessage = over 
                        ? $"Cannot exceed Maximum Step ({_maxIncrementLimit.ToString("0.###", CultureInfo.InvariantCulture)} {_unit})."
                        : $"Cannot be less than Minimum Step ({_minIncrementLimit.ToString("0.###", CultureInfo.InvariantCulture)} {_unit}).";
                    
                    MessageBox.Show(this,
                        errorMessage,
                        "Limit exceeded",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIncrement.Tag = "error_shown";
                }
                
                // Откатываем к последнему валидному значению или к максимуму/минимуму
                int selectionStart = txtIncrement.SelectionStart;
                txtIncrement.TextChanged -= ValidateIncrementAgainstMaxPressure; // Временно отключаем обработчик
                
                if (over)
                {
                    // Откатываем к максимуму
                    txtIncrement.Text = _maxIncrementLimit.ToString("F1", CultureInfo.InvariantCulture);
                    _currentIncrement = _maxIncrementLimit;
                }
                else if (under)
                {
                    // Откатываем к минимуму
                    txtIncrement.Text = _minIncrementLimit.ToString("F1", CultureInfo.InvariantCulture);
                    _currentIncrement = _minIncrementLimit;
                }
                else
                {
                    // Откатываем к последнему валидному значению
                    txtIncrement.Text = _lastValidIncrementText;
                }
                
                txtIncrement.TextChanged += ValidateIncrementAgainstMaxPressure; // Включаем обратно
                txtIncrement.SelectionStart = Math.Min(selectionStart, txtIncrement.Text.Length);
                
                // Устанавливаем красный цвет фона
                if (isDarkTheme)
                {
                    txtIncrement.BackColor = System.Drawing.Color.FromArgb(60, 20, 20);
                }
                else
                {
                    txtIncrement.BackColor = System.Drawing.Color.MistyRose;
                }
                
                UpdateIncrementButtons();
                ValidateIncrementAgainstMax();
            }
            else
            {
                // Сбрасываем флаг, если значение валидно
                if (txtIncrement.Tag?.ToString() == "error_shown")
                {
                    txtIncrement.Tag = null;
                }
                
                // Обновляем значение increment, если оно валидно
                if (parsed)
                {
                    double clampedValue = Math.Clamp(incrementVal, _minIncrementLimit, _maxIncrementLimit);
                    _currentIncrement = clampedValue;
                    _lastValidIncrementText = clampedValue.ToString("F1", CultureInfo.InvariantCulture);
                    
                    // Если значение было обрезано (clamped), обновляем текст в поле
                    if (clampedValue != incrementVal)
                    {
                        txtIncrement.TextChanged -= ValidateIncrementAgainstMaxPressure; // Временно отключаем
                        txtIncrement.Text = _lastValidIncrementText;
                        txtIncrement.TextChanged += ValidateIncrementAgainstMaxPressure; // Включаем обратно
                    }
                    
                    UpdateIncrementButtons();
                    ValidateIncrementAgainstMax(); // Проверяем также против MaxIncrementLimit - этот метод установит правильный цвет
                }
                else if (string.IsNullOrWhiteSpace(text))
                {
                    // Если текст пустой, сбрасываем цвет на нормальный
                    if (isDarkTheme)
                    {
                        txtIncrement.BackColor = darkBgWindow;
                    }
                    else
                    {
                        txtIncrement.BackColor = lightBgWindow;
                    }
                }
            }
        }

        /// <summary>
        /// Обновляет текст "Last update" на основе интервала таймера опроса.
        /// </summary>
        private void UpdateLastUpdateText()
        {
            int intervalMs = (int)(_pollTimer?.Interval ?? 500);
            string text;

            if (intervalMs < 1000)
            {
                // Меньше секунды - показываем в миллисекундах
                double seconds = intervalMs / 1000.0;
                text = $"Last update: {TrimZeros(seconds, 1)}s ago";
            }
            else if (intervalMs < 60000)
            {
                // Меньше минуты - показываем в секундах
                double seconds = intervalMs / 1000.0;
                text = $"Last update: {TrimZeros(seconds, 0)}s ago";
            }
            else
            {
                // Больше минуты - показываем в минутах
                double minutes = intervalMs / 60000.0;
                text = $"Last update: {TrimZeros(minutes, 1)}m ago";
            }

            UI_UpdateLastUpdate(text);
        }

        private void UpdateIncrementFromText()
        {
            if (double.TryParse(txtIncrement.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
            {
                _currentIncrement = Math.Clamp(val, _minIncrementLimit, _maxIncrementLimit);
                UpdateIncrementButtons();
                ValidateIncrementAgainstMax();
            }
        }

        public void UpdateIncrementButtons()
        {
            btnIncrease.Text = $"▲ Increase (+{_currentIncrement:F1} {_unit})";
            btnDecrease.Text = $"▼ Decrease (-{_currentIncrement:F1} {_unit})";
        }

        // ====================================================================
        // UTILITY
        // ====================================================================
        private static string TrimZeros(double v, int maxDecimals = 2) =>
            v.ToString("0." + new string('#', maxDecimals), CultureInfo.InvariantCulture);

        // ====================================================================
        // LOGO CREATION (called from InitializeComponent in Designer)
        // ====================================================================
        private void CreateLogo()
        {
            if (picLogo == null) return;

            // Пытаемся загрузить логотип из файла
            // Проверяем несколько возможных путей (с учетом разных регистров)
            // Приоритет: Assets (правильное место) -> Images -> корень приложения
            string[] possiblePaths = new[]
            {
                // В папке Assets (правильное место для ресурсов)
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Logo.png"),
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo.png"),
                // В папке Images (альтернативное место)
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Logo.png"),
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "logo.png"),
                // В корне приложения (для обратной совместимости)
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logo.png"),
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png"),
                System.IO.Path.Combine(Application.StartupPath, "Logo.png"),
                System.IO.Path.Combine(Application.StartupPath, "logo.png"),
            };

            System.Drawing.Image? logoImage = null;

            foreach (var path in possiblePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        logoImage = System.Drawing.Image.FromFile(path);
                        break;
                    }
                    catch
                    {
                        // Продолжаем поиск, если файл не удалось загрузить
                    }
                }
            }

            // Если файл не найден, создаем пустое изображение
            if (logoImage == null)
            {
                logoImage = new System.Drawing.Bitmap(180, 45);
                using (var g = System.Drawing.Graphics.FromImage(logoImage))
                {
                    g.Clear(System.Drawing.Color.FromArgb(0, 102, 170)); // DAC Blue
                }
            }

            picLogo.Image = logoImage;
        }

        private void btnGoToTarget_Click(object sender, EventArgs e)
        {

        }

        private void lblBaudRate_Click(object sender, EventArgs e)
        {

        }
    }
}