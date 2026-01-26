using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PrecisionPressureController.UI.Features.Graph.Views
{
    /// <summary>
    /// Модальное окно для редактирования последовательности давлений
    /// </summary>
    public partial class SequenceEditorDialog : Form
    {
        private List<TargetItem> _points;
        private bool _isDarkTheme;

        /// <summary>
        /// Точки последовательности (копия для редактирования)
        /// </summary>
        public List<TargetItem> Points => new List<TargetItem>(_points);

        /// <summary>
        /// Результат: были ли изменения применены
        /// </summary>
        public bool DialogResultApplied { get; private set; }

        public SequenceEditorDialog(List<TargetItem> points, bool isDarkTheme = true)
        {
            _points = new List<TargetItem>(points.Select(p => new TargetItem
            {
                Number = p.Number,
                PSI = p.PSI,
                HoldMinutes = p.HoldMinutes,
                Status = TargetStatus.Waiting // Все точки в редакторе всегда Waiting
            }));
            _isDarkTheme = isDarkTheme;
            InitializeComponent();
            SetupDataGridView();
            RenderTable();
        }

        private void SetupDataGridView()
        {
            dgvPoints.Columns.Clear();

            // Колонка #
            var colNumber = new DataGridViewTextBoxColumn
            {
                Name = "colNumber",
                HeaderText = "#",
                Width = 40,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            // Колонка PSI (editable)
            var colPSI = new DataGridViewTextBoxColumn
            {
                Name = "colPSI",
                HeaderText = "PSI (PSIG)",
                Width = 140,
                ReadOnly = false,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            // Колонка Hold (editable)
            var colHold = new DataGridViewTextBoxColumn
            {
                Name = "colHold",
                HeaderText = "Hold (min)",
                Width = 120,
                ReadOnly = false,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            // Колонка Status (read-only)
            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                Width = 100,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            // Колонка Delete
            var colDelete = new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "Del",
                Width = 50,
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            dgvPoints.Columns.AddRange(new DataGridViewColumn[]
            {
                colNumber, colPSI, colHold, colStatus, colDelete
            });

            dgvPoints.CellFormatting += DgvPoints_CellFormatting;
            dgvPoints.CellValueChanged += DgvPoints_CellValueChanged;
            dgvPoints.CellContentClick += DgvPoints_CellContentClick;
            dgvPoints.CellValidating += DgvPoints_CellValidating;
        }

        private void DgvPoints_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _points.Count) return;

            var column = dgvPoints.Columns[e.ColumnIndex];
            if (column.Name == "colStatus")
            {
                e.Value = "○ wait";
                e.CellStyle.ForeColor = SystemColors.GrayText;
                e.FormattingApplied = true;
            }
            else if (column.Name == "colNumber")
            {
                e.Value = (e.RowIndex + 1).ToString();
                e.FormattingApplied = true;
            }
        }

        private void DgvPoints_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            var column = dgvPoints.Columns[e.ColumnIndex];
            if (column.Name == "colPSI" || column.Name == "colHold")
            {
                if (e.FormattedValue == null || string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                {
                    e.Cancel = true;
                    dgvPoints.Rows[e.RowIndex].ErrorText = "Value cannot be empty";
                    return;
                }

                if (double.TryParse(e.FormattedValue.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    if (value <= 0)
                    {
                        e.Cancel = true;
                        dgvPoints.Rows[e.RowIndex].ErrorText = "Value must be greater than 0";
                        return;
                    }
                }
                else
                {
                    e.Cancel = true;
                    dgvPoints.Rows[e.RowIndex].ErrorText = "Invalid number format";
                    return;
                }

                dgvPoints.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void DgvPoints_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _points.Count) return;

            var column = dgvPoints.Columns[e.ColumnIndex];
            var cellValue = dgvPoints.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

            if (column.Name == "colPSI")
            {
                if (double.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double psi) && psi > 0)
                {
                    _points[e.RowIndex].PSI = psi;
                }
            }
            else if (column.Name == "colHold")
            {
                if (double.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double hold) && hold > 0)
                {
                    _points[e.RowIndex].HoldMinutes = (int)Math.Round(hold);
                }
            }
        }

        private void DgvPoints_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var column = dgvPoints.Columns[e.ColumnIndex];
            if (column.Name == "colDelete")
            {
                _points.RemoveAt(e.RowIndex);
                RenderTable();
            }
        }

        private void RenderTable()
        {
            dgvPoints.Rows.Clear();

            foreach (var point in _points)
            {
                dgvPoints.Rows.Add(
                    point.Number.ToString(),
                    point.PSI.ToString("F1", CultureInfo.InvariantCulture),
                    point.HoldMinutes.ToString(),
                    "○ wait",
                    "" // Delete button
                );
            }
        }

        private void BtnClearAll_Click(object? sender, EventArgs e)
        {
            if (_points.Count == 0) return;

            var result = MessageBox.Show(
                "Clear all points?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _points.Clear();
                RenderTable();
            }
        }

        private void BtnDone_Click(object? sender, EventArgs e)
        {
            DialogResultApplied = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResultApplied = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (numPSI == null || numHold == null) return;

            double psi = (double)numPSI.Value;
            int holdMinutes = (int)numHold.Value;

            if (psi <= 0)
            {
                MessageBox.Show("PSI value must be greater than 0.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (holdMinutes < 0)
            {
                MessageBox.Show("Hold value cannot be negative.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Добавляем новую точку
            _points.Add(new TargetItem
            {
                Number = _points.Count + 1,
                PSI = psi,
                HoldMinutes = holdMinutes,
                Status = TargetStatus.Waiting
            });

            RenderTable();

            // Очищаем поля ввода
            numPSI.Value = 0;
            numHold.Value = 0;
        }

        private void BtnParse_Click(object? sender, EventArgs e)
        {
            if (txtQuick == null) return;

            string input = txtQuick.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a sequence string.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Парсим строку формата "PSI:MIN, PSI:MIN, ..."
            var parsedPoints = new List<(double PSI, int Hold)>();
            string[] parts = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                // Разделяем по двоеточию
                string[] values = trimmed.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != 2)
                {
                    MessageBox.Show($"Invalid format: '{trimmed}'. Expected format: PSI:MIN", "Parse Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuick.Focus();
                    return;
                }

                // Парсим PSI
                if (!double.TryParse(values[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double psi))
                {
                    MessageBox.Show($"Invalid PSI value: '{values[0]}'", "Parse Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuick.Focus();
                    return;
                }

                if (psi <= 0)
                {
                    MessageBox.Show($"PSI value must be greater than 0: '{values[0]}'", "Parse Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuick.Focus();
                    return;
                }

                // Парсим Hold (минуты)
                if (!int.TryParse(values[1].Trim(), out int holdMinutes) || holdMinutes < 0)
                {
                    MessageBox.Show($"Invalid Hold value (must be non-negative integer): '{values[1]}'", "Parse Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuick.Focus();
                    return;
                }

                parsedPoints.Add((psi, holdMinutes));
            }

            if (parsedPoints.Count == 0)
            {
                MessageBox.Show("No valid points found in the input string.", "Parse Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Добавляем все распарсенные точки
            foreach (var (psi, holdMinutes) in parsedPoints)
            {
                _points.Add(new TargetItem
                {
                    Number = _points.Count + 1,
                    PSI = psi,
                    HoldMinutes = holdMinutes,
                    Status = TargetStatus.Waiting
                });
            }

            RenderTable();

            // Очищаем поле ввода
            txtQuick.Text = string.Empty;
            
            MessageBox.Show($"Successfully added {parsedPoints.Count} point(s).", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}

