using System;
using System.Windows.Forms;
using Alicat.Presentation.Presenters;
using Alicat.Services.Data;

namespace Alicat
{
    /// <summary>
    /// Partial class AlicatForm: интеграция с MainPresenter.
    /// Этот файл содержит связь между View (AlicatForm) и Presenter (MainPresenter).
    /// </summary>
    public partial class AlicatForm
    {
        private MainPresenter? _presenter;
        private readonly SessionDataStore _dataStore = new();

        // ====================================================================
        // IMainView Implementation - Properties
        // ====================================================================

        string IMainView.TargetInputText
        {
            get => txtTargetInput.Text;
            set => txtTargetInput.Text = value;
        }

        string IMainView.IncrementText
        {
            get => txtIncrement.Text;
            set => txtIncrement.Text = value;
        }

        double IMainView.CurrentIncrement
        {
            get => _currentIncrement;
            set => _currentIncrement = value;
        }

        double IMainView.MaxPressure
        {
            get => _maxPressure;
            set => _maxPressure = value;
        }

        double IMainView.MaxIncrementLimit
        {
            get => _maxIncrementLimit;
            set => _maxIncrementLimit = value;
        }

        double IMainView.Current
        {
            get => _current;
            set => _current = value;
        }

        double IMainView.SetPoint
        {
            get => _setPoint;
            set => _setPoint = value;
        }

        string IMainView.Unit
        {
            get => _unit;
            set => _unit = value;
        }

        double IMainView.RampSpeed
        {
            get => _rampSpeed;
            set => _rampSpeed = value;
        }

        TerminalForm? IMainView.TerminalForm
        {
            get => _terminalForm;
            set => _terminalForm = value;
        }

        GraphForm? IMainView.GraphForm
        {
            get => _graphForm;
            set => _graphForm = value;
        }

        TableForm? IMainView.TableForm
        {
            get => _tableForm;
            set => _tableForm = value;
        }

        // ====================================================================
        // IMainView Implementation - Methods
        // ====================================================================

        void IMainView.BeginInvoke(Action action)
        {
            if (InvokeRequired)
                base.BeginInvoke(action);
            else
                action();
        }

        // ====================================================================
        // Presenter Initialization
        // ====================================================================

        private void InitializePresenter()
        {
            _presenter = new MainPresenter(this, _dataStore);
            _presenter.Initialize();
        }

        // ====================================================================
        // Event Handlers - делегирование в Presenter
        // ====================================================================

        private void btnCommunication_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ConnectDevice(this);
        }

        private void btnGoTarget_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.GoToTarget(txtTargetInput.Text);
        }

        private void btnPause_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.Pause();
        }

        private async void btnPurge_Click_Presenter(object? sender, EventArgs e)
        {
            if (_presenter != null)
                await _presenter.Purge();
        }

        private void btnIncrease_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.Increase();
        }

        private void btnDecrease_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.Decrease();
        }

        private void btnIncrementMinus_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.IncrementMinus();
        }

        private void btnIncrementPlus_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.IncrementPlus();
        }

        private void txtIncrement_TextChanged_Presenter(object? sender, EventArgs e)
        {
            _presenter?.UpdateIncrementFromText(txtIncrement.Text);
        }

        private void btnOptions_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowOptions(this);
        }

        private void menuFileNewSession_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.StartNewSession();
        }

        private void btnGraph_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowGraph(this);
        }

        private void btnTable_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowTable(this);
        }

        private void btnTerminal_Click_Presenter(object? sender, EventArgs e)
        {
            _presenter?.ShowTerminal(this);
        }

        // ====================================================================
        // Cleanup
        // ====================================================================

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _presenter?.Dispose();
            base.OnFormClosing(e);
        }
    }
}

