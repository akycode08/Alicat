using System;
using System.Windows.Forms;
using Alicat.Presentation.Presenters;
using Alicat.Services.Data;
using Alicat.UI.Features.Terminal.Views;
using Alicat.UI.Features.Graph.Views;
using Alicat.UI.Features.Table.Views;

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

        // Property to access _dataStore from other partial class files
        internal SessionDataStore DataStore => _dataStore;

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
        // Device Menu Handlers
        // ====================================================================

        private void menuDeviceDisconnect_Click(object? sender, EventArgs e)
        {
            _presenter?.DisconnectDevice();
        }

        private async void menuDeviceEmergencyStop_Click(object? sender, EventArgs e)
        {
            if (_presenter != null)
                await _presenter.EmergencyStop();
        }

        // ====================================================================
        // File Menu Handlers
        // ====================================================================

        private void menuFileExit_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to exit?\n\nAny unsaved data will be lost.",
                "Exit Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // ====================================================================
        // Help Menu Handlers
        // ====================================================================

        private void menuHelpAboutDACTools_Click(object? sender, EventArgs e)
        {
            string message = "DAC Tools\n\n" +
                           "Custom Equipment for High-Pressure Research\n\n" +
                           "Professional tools for Diamond Anvil Cell (DAC) experiments\n" +
                           "and high-pressure research applications.\n\n" +
                           "© 2025 DAC Tools";

            MessageBox.Show(message, "About DACTools",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuHelpAboutAlicat_Click(object? sender, EventArgs e)
        {
            string version = "1.2.0";
            string message = "Alicat Controller\n\n" +
                           $"Version {version}\n\n" +
                           "Professional Windows application for controlling\n" +
                           "Alicat Pressure Controllers via RS-232.\n\n" +
                           "Features:\n" +
                           "• Real-time pressure monitoring\n" +
                           "• Precise digital pressure control\n" +
                           "• Live graphing and data logging\n" +
                           "• Terminal for RS-232 debugging\n\n" +
                           "© 2025 DAC Tools\n" +
                           "Built with .NET 8.0";

            MessageBox.Show(message, "About Alicat Controller",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ====================================================================
        // Cleanup
        // ====================================================================

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop polling timer
            _pollTimer?.Stop();
            
            // End data store session
            DataStore.EndSession();
            
            // Dispose serial connection
            _serial?.Dispose();
            
            // Dispose presenter
            _presenter?.Dispose();
            
            base.OnFormClosing(e);
        }
    }
}

