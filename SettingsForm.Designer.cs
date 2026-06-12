namespace ChatDemoCs
{
    partial class SettingsForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelApiKey = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.buttonShowKey = new System.Windows.Forms.Button();
            this.labelBaseUrl = new System.Windows.Forms.Label();
            this.textBoxBaseUrl = new System.Windows.Forms.TextBox();
            this.labelModel = new System.Windows.Forms.Label();
            this.comboModel = new System.Windows.Forms.ComboBox();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.numericTemperature = new System.Windows.Forms.NumericUpDown();
            this.labelMaxTokens = new System.Windows.Forms.Label();
            this.numericMaxTokens = new System.Windows.Forms.NumericUpDown();
            this.labelTimeout = new System.Windows.Forms.Label();
            this.numericTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelSystemPrompt = new System.Windows.Forms.Label();
            this.textBoxSystemPrompt = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelHint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaxTokens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).BeginInit();
            this.SuspendLayout();
            //
            // labelApiKey
            //
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.ForeColor = System.Drawing.Color.White;
            this.labelApiKey.Location = new System.Drawing.Point(20, 24);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(53, 12);
            this.labelApiKey.TabIndex = 0;
            this.labelApiKey.Text = "API Key:";
            //
            // textBoxApiKey
            //
            this.textBoxApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApiKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxApiKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxApiKey.ForeColor = System.Drawing.Color.White;
            this.textBoxApiKey.Location = new System.Drawing.Point(140, 22);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(380, 21);
            this.textBoxApiKey.TabIndex = 1;
            this.textBoxApiKey.UseSystemPasswordChar = true;
            //
            // buttonShowKey
            //
            this.buttonShowKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowKey.BackColor = System.Drawing.Color.DimGray;
            this.buttonShowKey.FlatAppearance.BorderSize = 0;
            this.buttonShowKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowKey.ForeColor = System.Drawing.Color.White;
            this.buttonShowKey.Location = new System.Drawing.Point(526, 21);
            this.buttonShowKey.Name = "buttonShowKey";
            this.buttonShowKey.Size = new System.Drawing.Size(60, 23);
            this.buttonShowKey.TabIndex = 2;
            this.buttonShowKey.Text = "Show";
            this.buttonShowKey.UseVisualStyleBackColor = false;
            this.buttonShowKey.Click += new System.EventHandler(this.buttonShowKey_Click);
            //
            // labelBaseUrl
            //
            this.labelBaseUrl.AutoSize = true;
            this.labelBaseUrl.ForeColor = System.Drawing.Color.White;
            this.labelBaseUrl.Location = new System.Drawing.Point(20, 60);
            this.labelBaseUrl.Name = "labelBaseUrl";
            this.labelBaseUrl.Size = new System.Drawing.Size(59, 12);
            this.labelBaseUrl.TabIndex = 3;
            this.labelBaseUrl.Text = "Base URL:";
            //
            // textBoxBaseUrl
            //
            this.textBoxBaseUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBaseUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxBaseUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBaseUrl.ForeColor = System.Drawing.Color.White;
            this.textBoxBaseUrl.Location = new System.Drawing.Point(140, 58);
            this.textBoxBaseUrl.Name = "textBoxBaseUrl";
            this.textBoxBaseUrl.Size = new System.Drawing.Size(446, 21);
            this.textBoxBaseUrl.TabIndex = 4;
            //
            // labelModel
            //
            this.labelModel.AutoSize = true;
            this.labelModel.ForeColor = System.Drawing.Color.White;
            this.labelModel.Location = new System.Drawing.Point(20, 96);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(41, 12);
            this.labelModel.TabIndex = 5;
            this.labelModel.Text = "Model:";
            //
            // comboModel
            //
            this.comboModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboModel.ForeColor = System.Drawing.Color.White;
            this.comboModel.FormattingEnabled = true;
            this.comboModel.Location = new System.Drawing.Point(140, 92);
            this.comboModel.Name = "comboModel";
            this.comboModel.Size = new System.Drawing.Size(220, 20);
            this.comboModel.TabIndex = 6;
            //
            // labelTemperature
            //
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.ForeColor = System.Drawing.Color.White;
            this.labelTemperature.Location = new System.Drawing.Point(20, 132);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(77, 12);
            this.labelTemperature.TabIndex = 7;
            this.labelTemperature.Text = "Temperature:";
            //
            // numericTemperature
            //
            this.numericTemperature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.numericTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericTemperature.DecimalPlaces = 2;
            this.numericTemperature.ForeColor = System.Drawing.Color.White;
            this.numericTemperature.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            this.numericTemperature.Location = new System.Drawing.Point(140, 130);
            this.numericTemperature.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            this.numericTemperature.Name = "numericTemperature";
            this.numericTemperature.Size = new System.Drawing.Size(120, 22);
            this.numericTemperature.TabIndex = 8;
            this.numericTemperature.Value = new decimal(new int[] { 7, 0, 0, 65536 });
            //
            // labelMaxTokens
            //
            this.labelMaxTokens.AutoSize = true;
            this.labelMaxTokens.ForeColor = System.Drawing.Color.White;
            this.labelMaxTokens.Location = new System.Drawing.Point(20, 168);
            this.labelMaxTokens.Name = "labelMaxTokens";
            this.labelMaxTokens.Size = new System.Drawing.Size(71, 12);
            this.labelMaxTokens.TabIndex = 9;
            this.labelMaxTokens.Text = "Max Tokens:";
            //
            // numericMaxTokens
            //
            this.numericMaxTokens.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.numericMaxTokens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericMaxTokens.ForeColor = System.Drawing.Color.White;
            this.numericMaxTokens.Increment = new decimal(new int[] { 128, 0, 0, 0 });
            this.numericMaxTokens.Location = new System.Drawing.Point(140, 166);
            this.numericMaxTokens.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            this.numericMaxTokens.Minimum = new decimal(new int[] { 64, 0, 0, 0 });
            this.numericMaxTokens.Name = "numericMaxTokens";
            this.numericMaxTokens.Size = new System.Drawing.Size(120, 22);
            this.numericMaxTokens.TabIndex = 10;
            this.numericMaxTokens.Value = new decimal(new int[] { 2048, 0, 0, 0 });
            //
            // labelTimeout
            //
            this.labelTimeout.AutoSize = true;
            this.labelTimeout.ForeColor = System.Drawing.Color.White;
            this.labelTimeout.Location = new System.Drawing.Point(20, 204);
            this.labelTimeout.Name = "labelTimeout";
            this.labelTimeout.Size = new System.Drawing.Size(89, 12);
            this.labelTimeout.TabIndex = 11;
            this.labelTimeout.Text = "Timeout (sec):";
            //
            // numericTimeout
            //
            this.numericTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.numericTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericTimeout.ForeColor = System.Drawing.Color.White;
            this.numericTimeout.Location = new System.Drawing.Point(140, 202);
            this.numericTimeout.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.numericTimeout.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numericTimeout.Name = "numericTimeout";
            this.numericTimeout.Size = new System.Drawing.Size(120, 22);
            this.numericTimeout.TabIndex = 12;
            this.numericTimeout.Value = new decimal(new int[] { 120, 0, 0, 0 });
            //
            // labelSystemPrompt
            //
            this.labelSystemPrompt.AutoSize = true;
            this.labelSystemPrompt.ForeColor = System.Drawing.Color.White;
            this.labelSystemPrompt.Location = new System.Drawing.Point(20, 240);
            this.labelSystemPrompt.Name = "labelSystemPrompt";
            this.labelSystemPrompt.Size = new System.Drawing.Size(89, 12);
            this.labelSystemPrompt.TabIndex = 13;
            this.labelSystemPrompt.Text = "System Prompt:";
            //
            // textBoxSystemPrompt
            //
            this.textBoxSystemPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSystemPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxSystemPrompt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSystemPrompt.ForeColor = System.Drawing.Color.White;
            this.textBoxSystemPrompt.Location = new System.Drawing.Point(140, 238);
            this.textBoxSystemPrompt.Multiline = true;
            this.textBoxSystemPrompt.Name = "textBoxSystemPrompt";
            this.textBoxSystemPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSystemPrompt.Size = new System.Drawing.Size(446, 152);
            this.textBoxSystemPrompt.TabIndex = 14;
            //
            // buttonSave
            //
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(140)))), ((int)(((byte)(80)))));
            this.buttonSave.FlatAppearance.BorderSize = 0;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.ForeColor = System.Drawing.Color.White;
            this.buttonSave.Location = new System.Drawing.Point(496, 410);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(90, 30);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            //
            // buttonCancel
            //
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.DimGray;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(400, 410);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 30);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            //
            // labelHint
            //
            this.labelHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHint.ForeColor = System.Drawing.Color.LightGray;
            this.labelHint.Location = new System.Drawing.Point(20, 415);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(374, 25);
            this.labelHint.TabIndex = 17;
            this.labelHint.Text = "Saved values are persisted to ChatDemoCs.exe.config.";
            //
            // SettingsForm
            //
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(606, 458);
            this.Controls.Add(this.labelHint);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxSystemPrompt);
            this.Controls.Add(this.labelSystemPrompt);
            this.Controls.Add(this.numericTimeout);
            this.Controls.Add(this.labelTimeout);
            this.Controls.Add(this.numericMaxTokens);
            this.Controls.Add(this.labelMaxTokens);
            this.Controls.Add(this.numericTemperature);
            this.Controls.Add(this.labelTemperature);
            this.Controls.Add(this.comboModel);
            this.Controls.Add(this.labelModel);
            this.Controls.Add(this.textBoxBaseUrl);
            this.Controls.Add(this.labelBaseUrl);
            this.Controls.Add(this.buttonShowKey);
            this.Controls.Add(this.textBoxApiKey);
            this.Controls.Add(this.labelApiKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AI Settings (DeepSeek)";
            ((System.ComponentModel.ISupportInitialize)(this.numericTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaxTokens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Button buttonShowKey;
        private System.Windows.Forms.Label labelBaseUrl;
        private System.Windows.Forms.TextBox textBoxBaseUrl;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.ComboBox comboModel;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.NumericUpDown numericTemperature;
        private System.Windows.Forms.Label labelMaxTokens;
        private System.Windows.Forms.NumericUpDown numericMaxTokens;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.NumericUpDown numericTimeout;
        private System.Windows.Forms.Label labelSystemPrompt;
        private System.Windows.Forms.TextBox textBoxSystemPrompt;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelHint;
    }
}
