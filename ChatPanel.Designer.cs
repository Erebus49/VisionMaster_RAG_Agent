namespace ChatDemoCs
{
    partial class ChatPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            try { CancelInFlight(); } catch { }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.richTextTranscript = new System.Windows.Forms.RichTextBox();
            this.panelInput = new System.Windows.Forms.Panel();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.panelInput.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // richTextTranscript
            //
            this.richTextTranscript.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.richTextTranscript.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextTranscript.DetectUrls = true;
            this.richTextTranscript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextTranscript.Font = new System.Drawing.Font("Consolas", 9F);
            this.richTextTranscript.ForeColor = System.Drawing.Color.White;
            this.richTextTranscript.Location = new System.Drawing.Point(0, 0);
            this.richTextTranscript.Name = "richTextTranscript";
            this.richTextTranscript.ReadOnly = true;
            this.richTextTranscript.Size = new System.Drawing.Size(366, 240);
            this.richTextTranscript.TabIndex = 0;
            this.richTextTranscript.Text = "";
            //
            // panelInput
            //
            this.panelInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelInput.Controls.Add(this.textBoxInput);
            this.panelInput.Controls.Add(this.panelButtons);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInput.Location = new System.Drawing.Point(0, 240);
            this.panelInput.Name = "panelInput";
            this.panelInput.Padding = new System.Windows.Forms.Padding(4);
            this.panelInput.Size = new System.Drawing.Size(366, 180);
            this.panelInput.TabIndex = 1;
            //
            // textBoxInput
            //
            this.textBoxInput.AcceptsReturn = true;
            this.textBoxInput.AcceptsTab = true;
            this.textBoxInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F);
            this.textBoxInput.ForeColor = System.Drawing.Color.White;
            this.textBoxInput.Location = new System.Drawing.Point(4, 4);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInput.Size = new System.Drawing.Size(358, 130);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxInput_KeyDown);
            //
            // panelButtons
            //
            this.panelButtons.Controls.Add(this.labelStatus);
            this.panelButtons.Controls.Add(this.buttonSettings);
            this.panelButtons.Controls.Add(this.buttonExport);
            this.panelButtons.Controls.Add(this.buttonClear);
            this.panelButtons.Controls.Add(this.buttonStop);
            this.panelButtons.Controls.Add(this.buttonSend);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(4, 134);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(358, 42);
            this.panelButtons.TabIndex = 1;
            //
            // labelStatus
            //
            this.labelStatus.AutoSize = true;
            this.labelStatus.ForeColor = System.Drawing.Color.LightGray;
            this.labelStatus.Location = new System.Drawing.Point(2, 27);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(155, 12);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "Ready (Ctrl+Enter to send)";
            //
            // buttonSettings
            //
            this.buttonSettings.BackColor = System.Drawing.Color.DimGray;
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.buttonSettings.ForeColor = System.Drawing.Color.White;
            this.buttonSettings.Location = new System.Drawing.Point(2, 2);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(40, 22);
            this.buttonSettings.TabIndex = 4;
            this.buttonSettings.Text = "⚙";
            this.buttonSettings.UseVisualStyleBackColor = false;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            //
            // buttonExport
            //
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.BackColor = System.Drawing.Color.DimGray;
            this.buttonExport.FlatAppearance.BorderSize = 0;
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.ForeColor = System.Drawing.Color.White;
            this.buttonExport.Location = new System.Drawing.Point(58, 2);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(54, 22);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = false;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            //
            // buttonClear
            //
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.BackColor = System.Drawing.Color.DimGray;
            this.buttonClear.FlatAppearance.BorderSize = 0;
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.ForeColor = System.Drawing.Color.White;
            this.buttonClear.Location = new System.Drawing.Point(118, 2);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(54, 22);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            //
            // buttonStop
            //
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonStop.Enabled = false;
            this.buttonStop.FlatAppearance.BorderSize = 0;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.ForeColor = System.Drawing.Color.White;
            this.buttonStop.Location = new System.Drawing.Point(178, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(54, 22);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            //
            // buttonSend
            //
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(140)))), ((int)(((byte)(80)))));
            this.buttonSend.FlatAppearance.BorderSize = 0;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.ForeColor = System.Drawing.Color.White;
            this.buttonSend.Location = new System.Drawing.Point(238, 2);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(118, 22);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send (Ctrl+Enter)";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            //
            // ChatPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Controls.Add(this.richTextTranscript);
            this.Controls.Add(this.panelInput);
            this.Name = "ChatPanel";
            this.Size = new System.Drawing.Size(366, 420);
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextTranscript;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label labelStatus;
    }
}
