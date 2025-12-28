using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using Alicat.Services.Data;
using DataPointModel = Alicat.Services.Data.DataPoint;

namespace Alicat.UI.Features.Table.Views
{
    public partial class TableForm : Form
    {
        private readonly SessionDataStore _dataStore;

        public TableForm(SessionDataStore dataStore)
        {
            _dataStore = dataStore;

            InitializeComponent();
            dgvLog.CellDoubleClick += dgvLog_CellDoubleClick;

            // Загрузить историю из Store
            LoadHistoryFromStore();

            // Подписаться на новые точки
            _dataStore.OnNewPoint += OnNewPointReceived;
        }

        public double Threshold => (double)numThreshold.Value;

        private enum LogFilter
        {
            All,
            WithComments,
            Setpoints
        }

        private LogFilter _currentFilter = LogFilter.All;

        public void AddRecordFromDevice(double pressure, double setPoint, string units)
        {
            int rowIndex = dgvLog.Rows.Add();
            var row = dgvLog.Rows[rowIndex];

            row.Cells["colTime"].Value = DateTime.Now.ToString("HH:mm:ss");
            row.Cells["colPressure"].Value = pressure.ToString("G", CultureInfo.InvariantCulture);
            row.Cells["colSetpoint"].Value = setPoint.ToString("G", CultureInfo.InvariantCulture);
            // если есть колонка Units — заполняем её, иначе убери строку
            // row.Cells["colUnits"].Value = units;
            row.Cells["colComment"].Value = "";

            // автопрокрутка вниз
            dgvLog.FirstDisplayedScrollingRowIndex = rowIndex;

            ApplyCurrentFilter();

        }

        private void dgvLog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvLog.Rows[e.RowIndex];

            string time = Convert.ToString(row.Cells["colTime"].Value) ?? "";
            string pressure = Convert.ToString(row.Cells["colPressure"].Value) ?? "";
            string setpoint = Convert.ToString(row.Cells["colSetpoint"].Value) ?? "";
            string currentComment = Convert.ToString(row.Cells["colComment"].Value) ?? "";

            string info = $"{time} — {pressure} → {setpoint}";

            using var dlg = new addComment(info, currentComment);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                row.Cells["colComment"].Value = dlg.CommentText;
            }
        }

        private void dgvLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTabAll_Click(object sender, EventArgs e)
        {
            _currentFilter = LogFilter.All;
            ApplyCurrentFilter();
        }

        private void btnTabWidthComments_Click(object sender, EventArgs e)
        {
            _currentFilter = LogFilter.WithComments;
            ApplyCurrentFilter();
        }

        private void btnTabSetpoints_Click(object sender, EventArgs e)
        {
            _currentFilter = LogFilter.Setpoints;
            ApplyCurrentFilter();
        }

        private void ApplyCurrentFilter()
        {
            switch (_currentFilter)
            {
                case LogFilter.All:
                    ApplyFilterAll();
                    break;
                case LogFilter.WithComments:
                    ApplyFilterWithComments();
                    break;
                case LogFilter.Setpoints:
                    ApplyFilterSetpoints();
                    break;
            }
        }

        private void ApplyFilterAll()
        {
            foreach (DataGridViewRow row in dgvLog.Rows)
            {
                if (row.IsNewRow) continue;
                row.Visible = true;
            }
        }

        private void ApplyFilterWithComments()
        {
            foreach (DataGridViewRow row in dgvLog.Rows)
            {
                if (row.IsNewRow) continue;

                string comment = Convert.ToString(row.Cells["colComment"].Value) ?? "";
                row.Visible = !string.IsNullOrWhiteSpace(comment);
            }
        }

        private void ApplyFilterSetpoints()
        {
            double? lastSp = null;

            foreach (DataGridViewRow row in dgvLog.Rows)
            {
                if (row.IsNewRow) continue;

                string text = Convert.ToString(row.Cells["colSetpoint"].Value) ?? "";
                if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double sp))
                {
                    row.Visible = false;
                    continue;
                }

                bool visible;
                if (lastSp == null)
                {
                    visible = true;      // первую строку показываем всегда
                    lastSp = sp;
                }
                else if (Math.Abs(sp - lastSp.Value) > 1e-9)
                {
                    visible = true;      // setpoint изменился
                    lastSp = sp;
                }
                else
                {
                    visible = false;     // такой же, как предыдущий
                }

                row.Visible = visible;
            }
        }

        // =========================
        // SessionDataStore integration
        // =========================
        private void LoadHistoryFromStore()
        {
            foreach (var point in _dataStore.Points)
            {
                AddRowFromPoint(point);
            }
        }

        private void OnNewPointReceived(DataPointModel point)
        {
            if (IsDisposed) return;

            // Вызываем в UI потоке
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnNewPointReceived(point)));
                return;
            }

            // Проверяем threshold
            if (ShouldLogPoint(point))
            {
                AddRowFromPoint(point);
            }
        }

        private double? _lastLoggedPressure = null;
        private double? _lastLoggedSetpoint = null;

        private bool ShouldLogPoint(DataPointModel point)
        {
            double threshold = Threshold;

            // первая запись всегда
            if (_lastLoggedPressure == null)
            {
                _lastLoggedPressure = point.Current;
                _lastLoggedSetpoint = point.Target;
                return true;
            }

            // Записываем если изменился setpoint (пользователь изменил давление)
            if (_lastLoggedSetpoint.HasValue && Math.Abs(point.Target - _lastLoggedSetpoint.Value) > 0.01)
            {
                _lastLoggedPressure = point.Current;
                _lastLoggedSetpoint = point.Target;
                return true;
            }

            // Записываем если изменилось текущее давление на величину >= threshold
            double delta = Math.Abs(point.Current - _lastLoggedPressure.Value);
            if (delta >= threshold)
            {
                _lastLoggedPressure = point.Current;
                _lastLoggedSetpoint = point.Target;
                return true;
            }

            return false;
        }

        private void AddRowFromPoint(DataPointModel point)
        {
            int rowIndex = dgvLog.Rows.Add();
            var row = dgvLog.Rows[rowIndex];

            row.Cells["colTime"].Value = point.Timestamp.ToString("HH:mm:ss");
            row.Cells["colPressure"].Value = point.Current.ToString("G", CultureInfo.InvariantCulture);
            row.Cells["colSetpoint"].Value = point.Target.ToString("G", CultureInfo.InvariantCulture);
            row.Cells["colComment"].Value = point.Event ?? "";

            // автопрокрутка вниз
            dgvLog.FirstDisplayedScrollingRowIndex = rowIndex;

            ApplyCurrentFilter();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Отписаться от событий
            _dataStore.OnNewPoint -= OnNewPointReceived;
            base.OnFormClosing(e);
        }

    }
}
