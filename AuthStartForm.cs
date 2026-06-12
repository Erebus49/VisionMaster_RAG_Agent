using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    public partial class AuthStartForm : Form
    {
        public bool LoginSucceeded { get; private set; }

        public AuthStartForm()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            using (RegisterForm dlg = new RegisterForm())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ShowLoginDialog();
                }
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ShowLoginDialog();
        }

        private void ShowLoginDialog()
        {
            using (LoginForm dlg = new LoginForm())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoginSucceeded = true;
                    Close();
                }
            }
        }
    }
}
