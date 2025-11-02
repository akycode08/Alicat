// Подключаем нужные пространства имён
using System;                     // базовые типы и исключения
using System.IO.Ports;             // работа с COM-портами
using System.Linq;                 // LINQ — для сортировки и работы с массивами
using System.Text;                 // кодировки (ASCII)
using System.Windows.Forms;        // элементы управления WinForms

namespace Alicat
{
    // Определяем частичный класс формы (часть кода — здесь, часть в Designer)
    public partial class FormConnect : Form
    {
        // Храним ссылку на объект SerialPort, если порт открыт
        private SerialPort? _port;

        // Конструктор формы — вызывается при создании окна
        public FormConnect()
        {
            InitializeComponent(); // создаёт элементы интерфейса (из .Designer.cs)
        }

        // Событие при загрузке формы
        private void FormConnect_Load(object? sender, EventArgs e)
        {
            // Добавляем стандартные скорости обмена в ComboBox
            cbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });

            // Добавляем возможные варианты чётности (Parity)
            cbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));

            // Добавляем возможные варианты стоп-битов (StopBits)
            cbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            // Устанавливаем значения по умолчанию
            cbBaudRate.SelectedItem = "19200";
            cbParity.SelectedItem = nameof(Parity.None);
            cbStopBits.SelectedItem = nameof(StopBits.One);
            nudDataBits.Value = 8;         // количество бит данных
            nudReadTimeout.Value = 700;    // таймаут чтения (мс)
            nudWriteTimeout.Value = 700;   // таймаут записи (мс)

            // Загружаем список доступных COM-портов
            RefreshPorts();

            // Делаем обе кнопки активными сразу
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        // Кнопка обновления списка COM-портов
        private void btnRefreshPorts_Click(object? sender, EventArgs e)
        {
            RefreshPorts(); // просто вызывает функцию обновления
        }

        // Кнопка "сбросить на настройки по умолчанию"
        private void btnDefaults_Click(object? sender, EventArgs e)
        {
            cbBaudRate.SelectedItem = "19200";
            cbParity.SelectedItem = nameof(Parity.None);
            cbStopBits.SelectedItem = nameof(StopBits.One);
            nudDataBits.Value = 8;
            nudReadTimeout.Value = 700;
            nudWriteTimeout.Value = 700;
        }

        // Кнопка "Подключиться"
        private void btnConnect_Click(object? sender, EventArgs e)
        {
            var prev = Cursor.Current;          // сохраняем текущий курсор
            Cursor.Current = Cursors.WaitCursor; // показываем "часики" во время операции
            btnConnect.Enabled = false;          // блокируем кнопку, чтобы не нажали дважды

            try
            {
                // Если порт уже открыт — закрываем перед новым подключением
                if (_port?.IsOpen == true)
                {
                    TryClosePort();
                }

                // Создаём и настраиваем порт из значений UI
                _port = BuildPort();
                _port.Open(); // открываем COM-порт

                // Проверяем, отвечает ли прибор
                string resp = PingAlicat(_port);

                // Если нет ожидаемого ответа (например, не начинается с "A")
                if (string.IsNullOrWhiteSpace(resp) || !resp.StartsWith("A"))
                    MessageBox.Show("Порт открыт, но прибор ответил не так, как ожидалось.", "Внимание");
                else
                    MessageBox.Show($"Успешно подключено.\r\nОтвет: {resp}", "OK");
            }
            catch (TimeoutException)
            {
                // Если не отвечает — пробуем другие скорости (автоопределение)
                TryReconnectWithAutoBaud();
            }
            catch (UnauthorizedAccessException)
            {
                // Если порт занят другой программой
                MessageBox.Show("COM-порт занят другой программой. Закройте терминал/драйвер и попробуйте снова.", "Порт занят");
                TryClosePort();
            }
            catch (Exception ex)
            {
                // Любая другая ошибка
                MessageBox.Show(ex.Message, "Ошибка");
                TryClosePort();
            }
            finally
            {
                // Восстанавливаем курсор и активируем кнопки
                Cursor.Current = prev;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = true;
            }
        }

        // Кнопка "Отключиться"
        private void btnDisconnect_Click(object? sender, EventArgs e)
        {
            bool wasOpen = _port?.IsOpen == true; // запоминаем, был ли порт открыт

            TryClosePort(); // закрываем порт

            // Показываем сообщение
            if (wasOpen)
                MessageBox.Show("Соединение разорвано.", "Disconnected");
            else
                MessageBox.Show("Порт уже был закрыт.", "Info");

            // Делаем обе кнопки снова активными
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = true;
        }

        // ---------------- Вспомогательные методы ----------------

        // Обновление списка COM-портов
        private void RefreshPorts()
        {
            var ports = SerialPort.GetPortNames().OrderBy(x => x).ToArray(); // получаем список портов
            cbPortName.Items.Clear();          // очищаем старый список
            cbPortName.Items.AddRange(ports);  // добавляем новые
            if (cbPortName.Items.Count > 0)    // если что-то найдено
                cbPortName.SelectedIndex = 0;  // выбираем первый порт
        }

        // Создание и настройка объекта SerialPort из UI
        private SerialPort BuildPort()
        {
            // Получаем имя выбранного порта
            var portName = cbPortName.SelectedItem as string
                ?? throw new InvalidOperationException("Select PortName.");

            // Получаем скорость из ComboBox
            var baudText = cbBaudRate.SelectedItem as string ?? "19200";
            if (!int.TryParse(baudText, out var baud)) baud = 19200;

            // Получаем параметры чётности и стоп-битов
            var parityText = cbParity.SelectedItem as string ?? nameof(Parity.None);
            var stopBitsText = cbStopBits.SelectedItem as string ?? nameof(StopBits.One);

            // Преобразуем строки в enum
            var parity = Enum.Parse<Parity>(parityText);
            var stopBits = Enum.Parse<StopBits>(stopBitsText);

            // Возвращаем готовый настроенный SerialPort
            return new SerialPort(portName, baud, parity, (int)nudDataBits.Value, stopBits)
            {
                Handshake = Handshake.None,      // без аппаратного рукопожатия
                Encoding = Encoding.ASCII,       // кодировка ASCII
                NewLine = "\r",                  // Alicat завершает строки символом CR
                ReadTimeout = Math.Max(1500, (int)nudReadTimeout.Value),  // таймаут чтения
                WriteTimeout = Math.Max(1500, (int)nudWriteTimeout.Value), // таймаут записи
                DtrEnable = false,               // не используем DTR
                RtsEnable = false                // не используем RTS
            };
        }

        // Отправка команды "A" и получение ответа от Alicat
        private string PingAlicat(SerialPort sp)
        {
            sp.DiscardInBuffer();  // очищаем буфер входящих данных
            sp.Write("A\r");       // отправляем команду "A" (запрос статуса)
            return sp.ReadLine();  // читаем ответ до символа '\r'
        }

        // Попытка переподключения с автоподбором скорости (если таймаут)
        private void TryReconnectWithAutoBaud()
        {
            TryClosePort(); // закрываем, если открыт
            var speeds = new[] { "19200", "9600" }; // две возможные скорости

            foreach (var s in speeds)
            {
                try
                {
                    cbBaudRate.SelectedItem = s; // пробуем следующую скорость
                    _port = BuildPort();         // создаём порт с новой скоростью
                    _port.Open();                // открываем порт
                    string resp = PingAlicat(_port); // пробуем получить ответ
                    if (!string.IsNullOrWhiteSpace(resp) && resp.StartsWith("A"))
                    {
                        MessageBox.Show($"Подключено на {s} бод.\r\nОтвет: {resp}", "OK");
                        return; // если успех — выходим
                    }
                }
                catch
                {
                    TryClosePort(); // на ошибке — закрываем порт и пробуем следующую скорость
                }
            }

            // Если ни одна скорость не подошла
            MessageBox.Show("Таймаут ожидания ответа прибора.", "Ошибка");
            TryClosePort();
        }

        // Безопасное закрытие порта
        private void TryClosePort()
        {
            try
            {
                if (_port != null)
                {
                    try { _port.DiscardInBuffer(); } catch { /* игнорируем */ }

                    if (_port.IsOpen)
                    {
                        _port.DtrEnable = false;
                        _port.RtsEnable = false;
                        _port.Close(); // закрываем порт
                    }

                    _port.Dispose(); // освобождаем ресурсы
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при закрытии порта: " + ex.Message, "Ошибка");
            }
            finally
            {
                _port = null; // очищаем ссылку
            }
        }
    }
}
