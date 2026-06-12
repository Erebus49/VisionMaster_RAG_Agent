using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (AuthService.Validate(textBoxUserName.Text, textBoxPassword.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this, "用户名或密码错误，请重试。", "登录", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
