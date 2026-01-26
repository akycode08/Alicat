using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PrecisionPressureController.Presentation.Presenters;
using PrecisionPressureController.Services.Data;

namespace PrecisionPressureController.UI.Main
{
    public partial class MainWindow
    {
        // ═══════════════════════════════════════════
        // ТЕСТОВЫЙ КОД — удалить после тестирования
        // ═══════════════════════════════════════════

        private System.Windows.Forms.Timer? _testTimer;
        private Random? _testRandom;
        private bool _isTestRunning = false;

        // NOTE: menuFileTestMode removed - test mode functionality moved or removed
        // This file is kept for other test-related functionality if needed
        private void StartTestMode()
        {
            _testRandom = new Random();
            _testTimer = new System.Windows.Forms.Timer { Interval = 120000 }; // 2 минуты (120 секунд)
            _testTimer.Tick += TestTimer_Tick;
            _testTimer.Start();
            _isTestRunning = true;

            // NOTE: menuFileTestMode removed - test mode functionality moved/removed

            MessageBox.Show(
                "Test mode started!\n\nRandom target (0-1700) every 2 minutes.",
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

            // NOTE: menuFileTestMode removed - text update removed

            MessageBox.Show(
                "Test mode stopped.",
                "Test Mode",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void TestTimer_Tick(object? sender, EventArgs e)
        {
            if (_testRandom == null)
            {
                System.Diagnostics.Debug.WriteLine("TEST: _testRandom is null!");
                return;
            }

            // Случайное значение от 0 до 1700
            double randomTarget = _testRandom.Next(0, 1701);

            System.Diagnostics.Debug.WriteLine($"TEST: Timer tick - setting target to {randomTarget:F1}");

            // Используем Presenter для установки целевого давления без подтверждения
            // _presenter доступен, так как это partial class
            if (_presenter != null)
            {
                try
                {
                    // Используем SetTargetSilent для установки без подтверждения
                    _presenter.SetTargetSilent(randomTarget);
                    
                    BeginInvoke(new Action(() =>
                    {
                        UI_AppendStatusInfo($"TEST: Target set to {randomTarget:F1} {_unit}");
                    }));
                    
                    System.Diagnostics.Debug.WriteLine($"TEST: Successfully set target via Presenter: {randomTarget:F1}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"TEST: Error setting target: {ex.Message}");
                    BeginInvoke(new Action(() =>
                    {
                        UI_AppendStatusInfo($"TEST ERROR: {ex.Message}");
                    }));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("TEST: _presenter is null - cannot set target");
                BeginInvoke(new Action(() =>
                {
                    UI_AppendStatusInfo("TEST ERROR: Presenter not available");
                }));
            }
        }
    }
}