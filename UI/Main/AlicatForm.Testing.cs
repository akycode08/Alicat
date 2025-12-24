using System;
using System.Windows.Forms;

namespace Alicat
{
    public partial class AlicatForm : Form
    {
        // ═══════════════════════════════════════════
        // ТЕСТОВЫЙ КОД — удалить после тестирования
        // ═══════════════════════════════════════════

        private System.Windows.Forms.Timer? _testTimer;
        private Random? _testRandom;
        private bool _isTestRunning = false;

        private void menuFileTestMode_Click(object? sender, EventArgs e)
        {
            if (!_isTestRunning)
            {
                StartTestMode();
            }
            else
            {
                StopTestMode();
            }
        }

        private void StartTestMode()
        {
            _testRandom = new Random();
            _testTimer = new System.Windows.Forms.Timer { Interval = 15000 }; // 30 сек
            _testTimer.Tick += TestTimer_Tick;
            _testTimer.Start();
            _isTestRunning = true;

            // Обновляем текст меню
            menuFileTestMode.Text = "Stop Test Mode";

            MessageBox.Show(
                "Test mode started!\n\nRandom target (10-120) every 30 seconds.",
                "Test Mode",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void StopTestMode()
        {
            _testTimer?.Stop();
            _testTimer?.Dispose();
            _testTimer = null;
            _isTestRunning = false;

            // Обновляем текст меню
            menuFileTestMode.Text = "Start Test Mode";

            MessageBox.Show(
                "Test mode stopped.",
                "Test Mode",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void TestTimer_Tick(object? sender, EventArgs e)
        {
            if (_serial == null) return;
            if (_testRandom == null) return;

            // Случайное значение от 10 до 120
            double randomTarget = _testRandom.Next(10, 121);

            // Отправляем команду на устройство
            _serial.Send($"AS {randomTarget:F1}");
            _setPoint = randomTarget;

            // Обновляем UI
            UI_SetSetPoint(_setPoint, _unit);

            // Записываем событие в Store
            _dataStore.RecordEvent(_current, _setPoint, _unit, "TARGET_CHANGED");

            System.Diagnostics.Debug.WriteLine($"TEST: Set target to {randomTarget:F1}");
        }
    }
}