using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string userName = textBoxUserName.Text == null ? string.Empty : textBoxUserName.Text.Trim();
            string password = textBoxPassword.Text == null ? string.Empty : textBoxPassword.Text;
            if (userName.Length == 0 || password.Length == 0)
            {
                MessageBox.Show(this, "用户名和密码不能为空。", "注册", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AuthService.Register(userName, password);
            MessageBox.Show(this, "注册成功，请登录。", "注册", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
