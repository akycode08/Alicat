using System;                           // Базовые типы, события, исключения
using System.Globalization;             // Парсинг чисел с инвариантной культурой
using System.IO.Ports;                  // Работа с SerialPort (COM)
using System.Text;                      // Кодировки для порта (ASCII)
using System.Windows.Forms;             // WinForms UI

// Разруливаем конфликт двух Timer: явно используем таймер из WinForms
using Timer = System.Windows.Forms.Timer;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        // ---- Состояние измерений/уставки ----
        private double _current = 0.0;        // Текущее давление, из ответа прибора
        private double _setPoint = 0.0;       // Текущая уставка (что мы отправили/что прибор вернул)
        private string _unit = "PSIG";        // Единицы измерения (из ответа ALS)

        private SerialClient? _serial;        // Наш обёрнутый клиент поверх SerialPort
        private readonly Timer _pollTimer = new() { Interval = 500 }; // Таймер опроса (каждые 500 мс)

        public AlicatForm()
        {
            InitializeComponent();            // Создаёт UI-элементы из .Designer.cs

            btnGoTarget.Click += btnGoTarget_Click;

            btnPurge.Click += btnPurge_Click;



            RefreshCurrent();                 // Отрисовать начальное значение на экране

            // Привязка обработчиков кнопок
            btnGoPlus.Click += btnGoPlus_Click;           // Кнопка + инкремент
            btnGoMinus.Click += btnGoMinus_Click;         // Кнопка - инкремент
            btnCommunication.Click += btnCommunication_Click; // Открыть окно настроек/подключения

            // При каждом тике опрашиваем прибор (ALS), если есть подключение
            _pollTimer.Tick += (_, __) => _serial?.RequestAls();
        }

        // ================= Окно коммуникаций (FormConnect) =================
        private void btnCommunication_Click(object? sender, EventArgs e)
        {
            using var dlg = new FormConnect             // Создаём диалоговое окно
            {
                StartPosition = FormStartPosition.CenterParent // Центрируем над родителем
            };

            dlg.ShowDialog(this);                       // Показываем модально, ждём закрытия

            // Забираем «уже открытый» SerialPort из FormConnect (аккуратно)
            var opened = GetOpenPortFrom(dlg);
            if (opened == null) return;                 // Если не получили — выходим

            _serial?.Dispose();                         // Закрываем прежнее соединение, если было

            _serial = new SerialClient(opened);         // Оборачиваем существующий открытый порт
            _serial.LineReceived += Serial_LineReceived; // Подписка на входящие строки
            _serial.Connected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Start())); // При «подключено» — старт опроса
            _serial.Disconnected += (_, __) => BeginInvoke(new Action(() => _pollTimer.Stop()));  // При «отключено» — стоп опроса

            _serial.Attach();        // Навесить обработчики на порт и сгенерировать событие Connected
            _serial.RequestAls();    // Сразу запросить состояние (ALS)
        }

        // Аккуратно получить открытый SerialPort у FormConnect через рефлексию (не меняя её API)
        private static SerialPort? GetOpenPortFrom(FormConnect fc)
        {
            try
            {
                var f = typeof(FormConnect).GetField("_port",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic); // Достаём private поле _port
                var sp = f?.GetValue(fc) as SerialPort;   // Преобразуем к SerialPort
                if (sp != null && sp.IsOpen) return sp;   // Возвращаем, если открыт
            }
            catch { /* игнорируем ошибки рефлексии */ }
            return null;                                   // Не удалось — null
        }

        // ================= Управление уставкой (GO ±) =================
        private void btnGoPlus_Click(object? sender, EventArgs e)
        {
            var inc = (double)nudIncrement.Value;       // Берём шаг инкремента из NumericUpDown
            SendSetPoint(_setPoint + inc);              // Увеличиваем уставку и отправляем
        }

        private void btnGoMinus_Click(object? sender, EventArgs e)
        {
            var inc = (double)nudIncrement.Value;       // Шаг инкремента
            SendSetPoint(_setPoint - inc);              // Уменьшаем уставку и отправляем
        }

        private void SendSetPoint(double sp)
        {
            _setPoint = sp;                             // Локально запоминаем новую уставку
            var cmd = $"AS {sp.ToString("F2", CultureInfo.InvariantCulture)}"; // Команда Alicat на установку SP
            _serial?.Send(cmd);                         // Отправляем в порт (если подключены)
            _serial?.RequestAls();                      // Сразу запросить состояние для обновления UI
        }

        // ================= Приём и отрисовка данных =================
        private void Serial_LineReceived(object? sender, string line)
        {
            // Парсим строку ответа ALS: "A +0030.0 +0030.0 10 PSIG" и т.п.
            if (!TryParseAls(line, out var cur, out var sp, out var unit))
                return;                                  // Если не распознали — игнор

            _current = cur;                              // Обновляем текущее
            _setPoint = sp;                              // Обновляем уставку
            _unit = unit ?? _unit;                       // Обновляем единицы, если пришли

            BeginInvoke(new Action(RefreshCurrent));     // Обновить UI из UI-потока
        }

        private void RefreshCurrent()
        {
            lblCurrentBig.Text = $"{_current:0.0} {_unit}"; // Большой ярлык с текущим значением
        }

        // ================= Парсер ответа ALS =================
        private static bool TryParseAls(string line, out double cur, out double sp, out string? unit)
        {
            cur = 0; sp = 0; unit = null;               // Значения по умолчанию

            if (string.IsNullOrWhiteSpace(line)) return false; // Пусто — не годится

            var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); // Разбиваем по пробелам/табам
            if (parts.Length < 3) return false;         // Минимум: маркер + текущее + уставка

            // parts[0] обычно "A", parts[1] — текущее, parts[2] — уставка
            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out cur)) return false;
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out sp)) return false;

            // Ищем единицы измерения в остатке: PSIG/PSI/KPA/BAR
            for (int i = 3; i < parts.Length; i++)
            {
                var p = parts[i].Trim().ToUpperInvariant();
                if (p is "PSIG" or "PSI" or "KPA" or "BAR")
                {
                    unit = p;                            // Нашли единицы — сохраняем
                    break;
                }
            }
            return true;                                 // Успешный парс
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);                      // Базовая логика закрытия формы
            _pollTimer.Stop();                          // Останавливаем опрос
            _serial?.Dispose();                         // Освобождаем порт/подписки
        }

        // ================= Встроенный клиент поверх SerialPort =================
        private sealed class SerialClient : IDisposable
        {
            private readonly SerialPort _port;          // Базовый COM-порт
            private bool _attached;                     // Навешены ли обработчики

            public event EventHandler? Connected;       // Событие «подключено» (логическое)
            public event EventHandler? Disconnected;    // Событие «отключено»
            public event EventHandler<string>? LineReceived; // Событие «получена строка»

            // Конструктор №1: использовать уже ОТКРЫТЫЙ порт из FormConnect
            public SerialClient(SerialPort existingOpenPort)
            {
                _port = existingOpenPort;               // Сохраняем ссылку на порт
                _port.NewLine = "\r";                   // Alicat завершает строки CR
                _port.Encoding = Encoding.ASCII;        // ASCII-обмен
            }

            // Конструктор №2: открыть порт по имени/скорости (на будущее)
            public SerialClient(string portName, int baud)
            {
                _port = new SerialPort(portName, baud)  // Создаём новый SerialPort
                {
                    NewLine = "\r",                     // Строки заканчиваются CR
                    Encoding = Encoding.ASCII,          // ASCII
                    DtrEnable = false,                  // DTR/RTS не используем
                    RtsEnable = false,
                    ReadTimeout = 1500,                 // Таймауты на случай зависаний
                    WriteTimeout = 1500
                };
            }

            // Навешиваем обработчики и генерируем «Connected»
            public void Attach()
            {
                if (_attached) return;                  // Повторно не навешиваем
                _port.DataReceived += Port_DataReceived; // Подписка на входящие данные
                _attached = true;
                Connected?.Invoke(this, EventArgs.Empty); // Сообщаем внешнему коду
            }

            public void Open()
            {
                if (_port.IsOpen) { Attach(); return; } // Если уже открыт — просто Attach
                _port.Open();                            // Иначе открываем физически
                Attach();                                // И навешиваем обработчики
            }

            public void Close()
            {
                if (!_port.IsOpen) return;               // Если уже закрыт — выходим
                _port.Close();                           // Закрываем физически
                Disconnected?.Invoke(this, EventArgs.Empty); // Сигнал «отключено»
            }

            public void Send(string cmd)
            {
                if (!_port.IsOpen) return;               // Если не открыт — игнор
                try { _port.Write(cmd + "\r"); }         // Пишем команду + CR
                catch { /* глушим I/O ошибки */ }
            }

            public void RequestAls() => Send("ALS");     // Удобный шорткат на опрос состояния

            private void Port_DataReceived(object? sender, SerialDataReceivedEventArgs e)
            {
                try
                {
                    var line = _port.ReadLine();         // Читаем до CR
                    if (!string.IsNullOrWhiteSpace(line))
                        LineReceived?.Invoke(this, line); // Прокидываем строку наружу
                }
                catch { /* глушим ошибки порта/таймаута */ }
            }

            public void Dispose()
            {
                try
                {
                    _port.DataReceived -= Port_DataReceived; // Снимаем подписку
                    if (_port.IsOpen) _port.Close();         // Закрываем при необходимости
                    _port.Dispose();                          // Освобождаем ресурсы
                }
                catch { /* игнорируем ошибки dispose */ }
            }
        }

        private void btnGoTarget_Click(object? sender, EventArgs e)
        {
            // Кнопка должна оставаться активной
            btnGoTarget.Enabled = true;

            if (_serial == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = txtTarget.Text?.Trim() ?? string.Empty;
            if (raw.Length == 0)
            {
                MessageBox.Show("Enter target value.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!double.TryParse(raw,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out double targetValue))
            {
                MessageBox.Show("Invalid target value format.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            const double MIN = 0.0, MAX = 100.0; // при необходимости подправь
            if (targetValue < MIN || targetValue > MAX)
            {
                MessageBox.Show($"Target value must be between {MIN} and {MAX}.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Текст для отображения значения (с единицами)
            string unit = string.IsNullOrWhiteSpace(_unit) ? "PSIG" : _unit;
            string displayVal = targetValue.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            string confirmText = $"Do you want to change the target value to {displayVal} {unit}?";

            // Если чекбокс не отмечен — спросим подтверждение с числом
            if (!chkConfirmGo.Checked)
            {
                var ask = MessageBox.Show(confirmText, "Confirm action",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ask != DialogResult.Yes)
                    return;
            }

            try
            {
                string cmd = $"AS {targetValue:F1}";
                _serial.Send(cmd);

                // по желанию — сразу обновить показания
                _serial.RequestAls();

                // Сброс UI
                chkConfirmGo.Checked = false;
                txtTarget.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send command:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGoTarget.Enabled = true; // гарантируем включено
            }
        }

        private async void btnPurge_Click(object? sender, EventArgs e)
        {
            if (_serial is null)
            {
                MessageBox.Show("Нет соединения с прибором.", "Purge", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool confirmed = chkConfirmPurge.Checked;

            // Если галочка не стоит — спрашиваем подтверждение
            if (!confirmed)
            {
                var ask = MessageBox.Show(
                    "Сбросить давление до 0?",
                    "Confirm purge",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (ask != DialogResult.Yes)
                    return;
            }

            // Очищаем галочку, чтобы не оставалась активной
            chkConfirmPurge.Checked = false;

            // Блокируем кнопку во время purge
            btnPurge.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                // Команда AE — сброс давления
                _serial.Send("AE");

                // Ждём немного, чтобы давление упало
                await Task.Delay(3000); // можно увеличить, если сброс идёт дольше

                // Команда AC — вернуться в обычный режим
                _serial.Send("AC");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка Purge: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnPurge.Enabled = true;
                Cursor = Cursors.Default;
            }
        }



    }

}
