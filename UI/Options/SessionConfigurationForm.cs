using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Alicat
{
    /// <summary>
    /// Форма для просмотра конфигурации сессии в формате JSON
    /// </summary>
    public partial class SessionConfigurationForm : Form
    {
        private string _jsonContent = string.Empty;

        public SessionConfigurationForm()
        {
            InitializeComponent();
            LoadDefaultJson();
        }

        /// <summary>
        /// Загружает JSON конфигурацию (можно заменить на реальные данные)
        /// </summary>
        public void LoadJson(string jsonContent)
        {
            _jsonContent = jsonContent;
            UpdateJsonDisplay();
        }

        private void LoadDefaultJson()
        {
            var sessionConfig = new
            {
                session = new
                {
                    name = "Session_2025-01-05_14-30",
                    created = "2025-01-05T14:30:15Z",
                    lastModified = "2025-01-05T15:15:47Z",
                    duration = "00:45:32",
                    status = "active"
                },
                device = new
                {
                    model = "ALICAT PC-15PSIG-D",
                    serialNumber = "12345-ABC-67890",
                    firmwareVersion = "v2.5.3"
                },
                connection = new
                {
                    port = "COM3",
                    baudRate = 19200,
                    dataBits = 8,
                    parity = "None"
                },
                measurement = new
                {
                    units = new
                    {
                        pressure = "PSIG",
                        time = "s"
                    },
                    settings = new
                    {
                        rampSpeed = 10.0,
                        sampleRate = 10
                    }
                },
                limits = new
                {
                    pressure = new
                    {
                        maximum = 200.0,
                        minimum = 0.0
                    },
                    increment = new
                    {
                        maximum = 20.0,
                        minimum = 0.1
                    }
                },
                safety = new
                {
                    emergencyStop = true,
                    overpressureProtection = true
                }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            _jsonContent = JsonSerializer.Serialize(sessionConfig, options);
            UpdateJsonDisplay();
        }

        private void UpdateJsonDisplay()
        {
            if (txtJsonContent != null)
            {
                txtJsonContent.Text = _jsonContent;
            }
        }

        private void BtnCopy_Click(object? sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_jsonContent);
                MessageBox.Show(this, "JSON content copied to clipboard.", "Copied", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to copy to clipboard: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FileName = "session_configuration.json",
                    DefaultExt = "json"
                };

                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    await File.WriteAllTextAsync(saveDialog.FileName, _jsonContent, Encoding.UTF8);
                    MessageBox.Show(this, $"Configuration saved to:\n{saveDialog.FileName}", "Saved", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save file: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

