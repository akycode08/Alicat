using System;
using System.Windows.Forms;

namespace Alicat.UI.Features.Terminal.Views
{
    public partial class TerminalForm : Form
    {
        // 👉 AlicatForm подпишется на это событие
        public event Action<string>? CommandSent;

        public TerminalForm()
        {
            InitializeComponent();

            btnSend.Click += (_, __) => SendCommand();
            txtCommand.KeyDown += TxtCommand_KeyDown;
        }

        private void TxtCommand_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendCommand();
            }
        }

        private void SendCommand()
        {
            var cmd = txtCommand.Text.Trim();
            if (string.IsNullOrWhiteSpace(cmd))
                return;

            // Пока что всегда эхо, потом сделаем чекбокс Echo
            AppendLog($">> {cmd}");

            // 🔥 Главное место:
            CommandSent?.Invoke(cmd);

            txtCommand.Clear();
            txtCommand.Focus();
        }

        public void AppendLog(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), line);
                return;
            }

            // Добавляем новую строку
            if (rtbLog.TextLength > 0)
                rtbLog.AppendText(Environment.NewLine);

            rtbLog.AppendText(line);

            // ---- Ограничиваем последние 10 строк ----
            const int maxLines = 10;

            var lines = rtbLog.Lines;

            if (lines.Length > maxLines)
            {
                // Берём только последние maxLines строк
                var lastLines = lines.Skip(lines.Length - maxLines).ToArray();
                rtbLog.Lines = lastLines;
            }

            // Скроллим вниз
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }

    }
}
