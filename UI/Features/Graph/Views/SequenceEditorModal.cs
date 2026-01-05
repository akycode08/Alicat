using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Alicat.UI.Features.Graph.Views
{
    /// <summary>
    /// Модальное окно для редактирования последовательности давлений
    /// </summary>
    public partial class SequenceEditorModal : Form
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

        public SequenceEditorModal(List<TargetItem> points, bool isDarkTheme = true)
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
            ApplyTheme(_isDarkTheme);
            SetupDataGridView();
            SetupButtonHoverEffects();
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
                Resizable = DataGridViewTriState.False
            };

            // Колонка PSI (editable)
            var colPSI = new DataGridViewTextBoxColumn
            {
                Name = "colPSI",
                HeaderText = "PSI (PSIG)",
                Width = 120,
                ReadOnly = false,
                Resizable = DataGridViewTriState.False
            };

            // Колонка Hold (editable)
            var colHold = new DataGridViewTextBoxColumn
            {
                Name = "colHold",
                HeaderText = "Hold (min)",
                Width = 120,
                ReadOnly = false,
                Resizable = DataGridViewTriState.False
            };

            // Колонка Status (read-only)
            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                Width = 100,
                ReadOnly = true,
                Resizable = DataGridViewTriState.False
            };

            // Колонка Delete
            var colDelete = new DataGridViewButtonColumn
            {
                Name = "colDelete",
                HeaderText = "Del",
                Width = 60,
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Resizable = DataGridViewTriState.False
            };

            dgvPoints.Columns.AddRange(new DataGridViewColumn[]
            {
                colNumber, colPSI, colHold, colStatus, colDelete
            });

            // Стили
            // --bg-panel: #111827
            dgvPoints.RowsDefaultCellStyle.BackColor = _isDarkTheme 
                ? Color.FromArgb(17, 24, 39) 
                : Color.White;
            dgvPoints.AlternatingRowsDefaultCellStyle.BackColor = _isDarkTheme
                ? Color.FromArgb(26, 31, 46) // --bg-input: #1a1f2e (slightly darker for alternating)
                : Color.FromArgb(250, 250, 250);
            // --text-primary: #e4e7eb
            dgvPoints.RowsDefaultCellStyle.ForeColor = _isDarkTheme
                ? Color.FromArgb(228, 231, 235)
                : Color.Black;
            
            // Column header styles
            if (_isDarkTheme)
            {
                dgvPoints.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 31, 46); // --bg-input
                dgvPoints.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(156, 163, 175); // --text-secondary
                dgvPoints.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }

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
                // --text-muted: #6b7280 (status-wait)
                e.CellStyle.ForeColor = _isDarkTheme 
                    ? Color.FromArgb(107, 114, 128) 
                    : Color.FromArgb(100, 100, 100);
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

        private void BtnAddPoint_Click(object? sender, EventArgs e)
        {
            if (!double.TryParse(txtAddPSI.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double psi) || psi <= 0)
            {
                MessageBox.Show("Enter a valid PSI value (greater than 0)", "Invalid Input", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddPSI.Focus();
                return;
            }

            if (!double.TryParse(txtAddHold.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double hold) || hold <= 0)
            {
                MessageBox.Show("Enter a valid Hold value (greater than 0)", "Invalid Input", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddHold.Focus();
                return;
            }

            _points.Add(new TargetItem
            {
                Number = _points.Count + 1,
                PSI = psi,
                HoldMinutes = (int)Math.Round(hold),
                Status = TargetStatus.Waiting
            });

            txtAddPSI.Text = "0";
            txtAddHold.Text = "0";
            RenderTable();
            txtAddPSI.Focus();
        }

        private void BtnParseAndAdd_Click(object? sender, EventArgs e)
        {
            string input = txtQuickImport.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Enter points to import", "Empty Input", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuickImport.Focus();
                return;
            }

            var pairs = input.Split(',').Select(s => s.Trim()).ToList();
            int addedCount = 0;

            foreach (var pair in pairs)
            {
                var parts = pair.Split(':');
                if (parts.Length != 2) continue;

                if (double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double psi) &&
                    double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double hold) &&
                    psi > 0 && hold > 0)
                {
                    _points.Add(new TargetItem
                    {
                        Number = _points.Count + 1,
                        PSI = psi,
                        HoldMinutes = (int)Math.Round(hold),
                        Status = TargetStatus.Waiting
                    });
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                txtQuickImport.Text = string.Empty;
                RenderTable();
                MessageBox.Show($"Imported {addedCount} point(s)", "Import Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid format. Use: PSI:MIN, PSI:MIN, ...\nExample: 10:5, 25:10, 50:15", 
                    "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuickImport.Focus();
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

        private void ApplyTheme(bool isDark)
        {
            _isDarkTheme = isDark;

            if (isDark)
            {
                // --bg-window: #0a0e1a
                BackColor = Color.FromArgb(10, 14, 26);
                // --text-primary: #e4e7eb
                ForeColor = Color.FromArgb(228, 231, 235);
                
                // Apply colors to all controls
                if (mainPanel != null)
                {
                    mainPanel.BackColor = Color.FromArgb(10, 14, 26); // --bg-window
                }
                
                if (bottomPanel != null)
                {
                    bottomPanel.BackColor = Color.FromArgb(17, 24, 39); // --bg-panel
                }
            }
            else
            {
                BackColor = Color.FromArgb(250, 250, 255);
                ForeColor = Color.FromArgb(30, 30, 35);
                
                if (mainPanel != null)
                {
                    mainPanel.BackColor = Color.FromArgb(250, 250, 255);
                }
                
                if (bottomPanel != null)
                {
                    bottomPanel.BackColor = Color.FromArgb(250, 250, 255);
                }
            }
        }
        
        /// <summary>
        /// Sets up hover effects for buttons (--bg-button-hover: #2d3748)
        /// </summary>
        private void SetupButtonHoverEffects()
        {
            if (!_isDarkTheme) return;
            
            var hoverColor = Color.FromArgb(45, 55, 72); // --bg-button-hover: #2d3748
            
            // Button hover effects
            if (btnParseAndAdd != null)
            {
                var originalColor = btnParseAndAdd.BackColor;
                btnParseAndAdd.MouseEnter += (s, e) => btnParseAndAdd.BackColor = hoverColor;
                btnParseAndAdd.MouseLeave += (s, e) => btnParseAndAdd.BackColor = originalColor;
            }
            
            if (btnClearAll != null)
            {
                var originalColor = btnClearAll.BackColor;
                btnClearAll.MouseEnter += (s, e) => btnClearAll.BackColor = hoverColor;
                btnClearAll.MouseLeave += (s, e) => btnClearAll.BackColor = originalColor;
            }
            
            if (btnCancel != null)
            {
                var originalColor = btnCancel.BackColor;
                btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = hoverColor;
                btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = originalColor;
            }
        }
    }
}

