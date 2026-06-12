using System;
using System.Windows.Forms;

namespace ChatDemoCs
{
    /// <summary>
    /// Modal dialog used to edit DeepSeek API settings (key / base URL / model /
    /// temperature / max tokens / timeout / system prompt). Shown from the
    /// chat panel's gear button. Returns <see cref="DialogResult.OK"/> when
    /// the user clicks Save and the values are written to App.config.
    /// </summary>
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            comboModel.Items.AddRange(new object[] {
                "deepseek-chat",
                "deepseek-reasoner",
                "deepseek-coder"
            });
            LoadFromConfig();
        }

        private void LoadFromConfig()
        {
            textBoxApiKey.Text = AppSettings.ApiKey;
            textBoxBaseUrl.Text = AppSettings.BaseUrl;
            comboModel.Text = AppSettings.Model;
            numericTemperature.Value = (decimal)Math.Max(0, Math.Min(2, AppSettings.Temperature));
            numericMaxTokens.Value = Math.Max(numericMaxTokens.Minimum, Math.Min(numericMaxTokens.Maximum, AppSettings.MaxTokens));
            numericTimeout.Value = Math.Max(numericTimeout.Minimum, Math.Min(numericTimeout.Maximum, AppSettings.TimeoutSeconds));
            textBoxSystemPrompt.Text = AppSettings.SystemPrompt;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                AppSettings.ApiKey = (textBoxApiKey.Text ?? string.Empty).Trim();
                AppSettings.BaseUrl = (textBoxBaseUrl.Text ?? string.Empty).Trim();
                AppSettings.Model = (comboModel.Text ?? string.Empty).Trim();
                AppSettings.Temperature = (double)numericTemperature.Value;
                AppSettings.MaxTokens = (int)numericMaxTokens.Value;
                AppSettings.TimeoutSeconds = (int)numericTimeout.Value;
                AppSettings.SystemPrompt = textBoxSystemPrompt.Text ?? string.Empty;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonShowKey_Click(object sender, EventArgs e)
        {
            textBoxApiKey.UseSystemPasswordChar = !textBoxApiKey.UseSystemPasswordChar;
            buttonShowKey.Text = textBoxApiKey.UseSystemPasswordChar ? "Show" : "Hide";
        }
    }
}
