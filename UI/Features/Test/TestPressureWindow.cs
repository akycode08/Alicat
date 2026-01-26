using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrecisionPressureController.Presentation.Presenters;

namespace PrecisionPressureController.UI.Features.Test
{
    public partial class TestPressureWindow : Form
    {
        private MainPresenter? _presenter;
        private string _unit = "PSIG";

        public TestPressureWindow(MainPresenter? presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            
            // Инициализация значений
            UpdateCurrentValues();
            
            // Подписки на кнопки
            btnSetPressure.Click += BtnSetPressure_Click;
            btnSetRampSpeed.Click += BtnSetRampSpeed_Click;
        }

        private void UpdateCurrentValues()
        {
            if (_presenter == null)
            {
                lblCurrentPressureValue.Text = "N/A (Presenter недоступен)";
                lblCurrentRampSpeedValue.Text = "N/A (Presenter недоступен)";
                return;
            }

            // Получаем текущие значения из presenter через публичные свойства
            try
            {
                double currentPressure = _presenter.SetPoint;
                double currentRampSpeed = _presenter.RampSpeed;
                string unit = _presenter.Unit;

                // Обновляем единицы измерения
                _unit = unit;

                // Обновляем отображение
                lblCurrentPressureValue.Text = $"{currentPressure:F1} {unit}";
                lblCurrentRampSpeedValue.Text = $"{currentRampSpeed:F2} {unit}/s";
            }
            catch (Exception ex)
            {
                lblCurrentPressureValue.Text = $"Ошибка: {ex.Message}";
                lblCurrentRampSpeedValue.Text = $"Ошибка: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"UpdateCurrentValues error: {ex}");
            }
        }

        private void BtnSetPressure_Click(object? sender, EventArgs e)
        {
            if (_presenter == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = txtPressure.Text?.Trim() ?? string.Empty;
            if (raw.Length == 0)
            {
                MessageBox.Show("Enter target value.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPressure.Focus();
                return;
            }

            if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double pressure))
            {
                MessageBox.Show("Invalid target value format.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPressure.Focus();
                return;
            }

            try
            {
                // Используем SetTargetSilent для установки без подтверждения
                _presenter.SetTargetSilent(pressure);
                
                // Очищаем поле после установки
                txtPressure.Text = "";
                
                // Обновляем значения
                UpdateCurrentValues();
                
                lblStatus.Text = $"Pressure set to {pressure:F1} {_unit}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Failed to set pressure:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSetRampSpeed_Click(object? sender, EventArgs e)
        {
            if (_presenter == null)
            {
                MessageBox.Show("Device is not connected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string raw = txtRampSpeed.Text?.Trim() ?? string.Empty;
            if (raw.Length == 0)
            {
                MessageBox.Show("Enter ramp speed value.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRampSpeed.Focus();
                return;
            }

            if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double rampSpeed))
            {
                MessageBox.Show("Invalid ramp speed value format.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtRampSpeed.Focus();
                return;
            }

            if (rampSpeed < 0)
            {
                MessageBox.Show("Ramp speed cannot be negative.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRampSpeed.Focus();
                return;
            }

            try
            {
                // Используем SetRampSpeed для установки без подтверждения
                _presenter.SetRampSpeed(rampSpeed);
                
                // Очищаем поле после установки
                txtRampSpeed.Text = "";
                
                // Обновляем значения
                UpdateCurrentValues();
                
                lblStatus.Text = $"Ramp speed set to {rampSpeed:F2} {_unit}/s";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Failed to set ramp speed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void UpdateCurrentPressure(double pressure, string unit)
        {
            _unit = unit;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    lblCurrentPressureValue.Text = $"{pressure:F1} {unit}";
                }));
            }
            else
            {
                lblCurrentPressureValue.Text = $"{pressure:F1} {unit}";
            }
        }

        public void UpdateCurrentRampSpeed(double rampSpeed, string unit)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    lblCurrentRampSpeedValue.Text = $"{rampSpeed:F2} {unit}/s";
                }));
            }
            else
            {
                lblCurrentRampSpeedValue.Text = $"{rampSpeed:F2} {unit}/s";
            }
        }
    }
}

