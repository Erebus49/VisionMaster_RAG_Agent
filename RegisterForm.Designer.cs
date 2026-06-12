namespace ChatDemoCs
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxLabelUserName = new System.Windows.Forms.TextBox();
            this.textBoxLabelPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.textBoxLabelUserName.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBoxLabelUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLabelUserName.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLabelUserName.Location = new System.Drawing.Point(43, 66);
            this.textBoxLabelUserName.Multiline = true;
            this.textBoxLabelUserName.Name = "textBoxLabelUserName";
            this.textBoxLabelUserName.ReadOnly = true;
            this.textBoxLabelUserName.Size = new System.Drawing.Size(209, 58);
            this.textBoxLabelUserName.TabIndex = 0;
            this.textBoxLabelUserName.TabStop = false;
            this.textBoxLabelUserName.Text = "用户名:";
            this.textBoxLabelPassword.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBoxLabelPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLabelPassword.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLabelPassword.Location = new System.Drawing.Point(89, 184);
            this.textBoxLabelPassword.Multiline = true;
            this.textBoxLabelPassword.Name = "textBoxLabelPassword";
            this.textBoxLabelPassword.ReadOnly = true;
            this.textBoxLabelPassword.Size = new System.Drawing.Size(158, 58);
            this.textBoxLabelPassword.TabIndex = 1;
            this.textBoxLabelPassword.TabStop = false;
            this.textBoxLabelPassword.Text = "密码:";
            this.textBoxUserName.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxUserName.Location = new System.Drawing.Point(260, 66);
            this.textBoxUserName.Multiline = true;
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(249, 57);
            this.textBoxUserName.TabIndex = 2;
            this.textBoxUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxPassword.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxPassword.Location = new System.Drawing.Point(260, 185);
            this.textBoxPassword.Multiline = true;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(249, 57);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.buttonRegister.BackColor = System.Drawing.Color.White;
            this.buttonRegister.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRegister.Location = new System.Drawing.Point(193, 311);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(167, 91);
            this.buttonRegister.TabIndex = 4;
            this.buttonRegister.Text = "注册";
            this.buttonRegister.UseVisualStyleBackColor = false;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            this.AcceptButton = this.buttonRegister;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(552, 450);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.textBoxLabelPassword);
            this.Controls.Add(this.textBoxLabelUserName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox textBoxLabelUserName;
        private System.Windows.Forms.TextBox textBoxLabelPassword;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonRegister;
    }
}
