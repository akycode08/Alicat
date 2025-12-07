using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alicat.UI.Features.Table
{
    public partial class addComment : Form
    {
        public string CommentText { get; private set; } = "";

        public addComment()
        {
            InitializeComponent();
        }

        public addComment(string infoText, string? existingComment = null)
            : this()
        {
            lblinfo.Text = infoText;
            txtComment.Text = existingComment ?? "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CommentText = txtComment.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void txtComment_TextChanged(object sender, EventArgs e) { }

        private void lblinfo_Load(object sender, EventArgs e) { }

    }
}
