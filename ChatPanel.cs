using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatDemoCs
{
    /// <summary>
    /// Compact AI assistant control intended to dock into the bottom-right groupbox of
    /// <see cref="MainForm"/>. Contains a transcript, a multi-line input, and a small
    /// toolbar with Send / Stop / Clear / Export / Settings buttons.
    /// </summary>
    public partial class ChatPanel : UserControl
    {
        private readonly List<ChatMessage> _conversation = new List<ChatMessage>();
        private DeepSeekClient _client;
        private CancellationTokenSource _cts;
        private bool _busy;

        /// <summary>Surfaces status / errors to the host form.</summary>
        public event Action<string> LogMessage;

        public ChatPanel()
        {
            InitializeComponent();
            try
            {
                _client = new DeepSeekClient();
            }
            catch (Exception ex)
            {
                Raise("Failed to init DeepSeek client: " + ex.Message);
            }
            ResetConversation();
        }

        /// <summary>Re-read App.config-backed settings (called after the Settings dialog closes).</summary>
        public void RefreshSettings()
        {
            try
            {
                if (_client != null)
                {
                    _client.Dispose();
                }
                _client = new DeepSeekClient();
                Raise("AI settings refreshed. Model = " + AppSettings.Model);
            }
            catch (Exception ex)
            {
                Raise("Failed to refresh AI settings: " + ex.Message);
            }
        }

        public void ResetConversation()
        {
            if (_busy) return;
            _conversation.Clear();
            string sys = AppSettings.SystemPrompt;
            if (!string.IsNullOrEmpty(sys))
            {
                _conversation.Add(new ChatMessage(ChatMessage.RoleSystem, sys));
            }
            richTextTranscript.Clear();
            AppendSystemNotice("New conversation. Active model: " + AppSettings.Model);
        }

        public void ExportTranscript()
        {
            if (_conversation.Count == 0)
            {
                Raise("Nothing to export: conversation is empty.");
                return;
            }
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Markdown (*.md)|*.md|Text File (*.txt)|*.txt";
                dlg.FileName = "chat-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".md";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (StreamWriter sw = new StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8))
                    {
                        foreach (ChatMessage m in _conversation)
                        {
                            sw.WriteLine("### " + m.Role + "  (" + m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                            sw.WriteLine();
                            sw.WriteLine(m.Content);
                            sw.WriteLine();
                        }
                    }
                    Raise("Transcript saved to " + dlg.FileName);
                }
                catch (Exception ex)
                {
                    Raise("Save failed: " + ex.Message);
                }
            }
        }

        public void CancelInFlight()
        {
            try { if (_cts != null) _cts.Cancel(); }
            catch { /* ignore */ }
        }

        // ---- Event handlers -------------------------------------------------------

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            string input = textBoxInput.Text == null ? string.Empty : textBoxInput.Text.Trim();
            if (input.Length == 0) return;

            if (_client == null || string.IsNullOrEmpty(AppSettings.ApiKey))
            {
                AppendSystemNotice("API Key is not configured. Click the Settings button (⚙) below.");
                Raise("API Key missing - request aborted.");
                return;
            }

            ChatMessage userMsg = new ChatMessage(ChatMessage.RoleUser, input);
            _conversation.Add(userMsg);
            AppendMessage(userMsg);
            textBoxInput.Clear();

            SetBusy(true);
            _cts = new CancellationTokenSource();

            string finalText = null;
            string error = null;
            try
            {
                VmRagAugmentation augmentation = await Task.Run(() => VmManualRagService.Instance.Augment(_conversation, input), _cts.Token).ConfigureAwait(true);
                if (augmentation != null && !string.IsNullOrEmpty(augmentation.Status))
                {
                    AppendSystemNotice(augmentation.Status);
                    Raise(augmentation.Status);
                }
                BeginAssistantStreaming();
                IEnumerable<ChatMessage> requestMessages = augmentation == null || augmentation.Messages == null ? _conversation : augmentation.Messages;
                finalText = await _client.SendChatStreamingAsync(requestMessages, OnStreamingChunk, _cts.Token).ConfigureAwait(true);
            }
            catch (OperationCanceledException) { error = "[cancelled]"; }
            catch (Exception ex) { error = ex.Message; }

            if (error != null)
            {
                AppendStreamingChunk(Environment.NewLine + "[Error] " + error);
                Raise("Chat request failed: " + error);
            }
            else if (finalText != null)
            {
                _conversation.Add(new ChatMessage(ChatMessage.RoleAssistant, finalText));
                Raise("AI replied (" + finalText.Length + " chars).");
            }

            EndAssistantStreaming();
            SetBusy(false);
            if (_cts != null) { _cts.Dispose(); _cts = null; }
        }

        private void buttonStop_Click(object sender, EventArgs e) { CancelInFlight(); }
        private void buttonClear_Click(object sender, EventArgs e) { ResetConversation(); }
        private void buttonExport_Click(object sender, EventArgs e) { ExportTranscript(); }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm dlg = new SettingsForm())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshSettings();
                }
            }
        }

        private void textBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+Enter sends, plain Enter inserts a newline.
            if (e.KeyCode == Keys.Enter && (e.Modifiers & Keys.Control) == Keys.Control)
            {
                e.SuppressKeyPress = true;
                buttonSend.PerformClick();
            }
        }

        private void OnStreamingChunk(string delta)
        {
            if (string.IsNullOrEmpty(delta)) return;
            if (richTextTranscript.IsHandleCreated && richTextTranscript.InvokeRequired)
            {
                try { richTextTranscript.BeginInvoke(new Action<string>(AppendStreamingChunk), delta); }
                catch { /* control disposed */ }
            }
            else
            {
                AppendStreamingChunk(delta);
            }
        }

        // ---- Rendering helpers ----------------------------------------------------

        private void AppendMessage(ChatMessage msg)
        {
            string header = (msg.Role == ChatMessage.RoleUser ? "You" :
                             msg.Role == ChatMessage.RoleAssistant ? "Assistant" :
                             msg.Role) + "  " + msg.Timestamp.ToString("HH:mm:ss");

            Color headerColor = msg.Role == ChatMessage.RoleUser
                ? Color.FromArgb(102, 204, 255)
                : Color.FromArgb(255, 196, 102);

            AppendColored(header + Environment.NewLine, headerColor, true);
            AppendColored(msg.Content + Environment.NewLine + Environment.NewLine, Color.White, false);
            ScrollToEnd();
        }

        private void BeginAssistantStreaming()
        {
            string header = "Assistant  " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine;
            AppendColored(header, Color.FromArgb(255, 196, 102), true);
            ScrollToEnd();
        }

        private void AppendStreamingChunk(string delta)
        {
            richTextTranscript.SelectionStart = richTextTranscript.TextLength;
            richTextTranscript.SelectionLength = 0;
            richTextTranscript.SelectionColor = Color.White;
            richTextTranscript.SelectionFont = richTextTranscript.Font;
            richTextTranscript.AppendText(delta);
            richTextTranscript.SelectionStart = richTextTranscript.TextLength;
            richTextTranscript.ScrollToCaret();
        }

        private void EndAssistantStreaming()
        {
            AppendColored(Environment.NewLine + Environment.NewLine, Color.White, false);
            ScrollToEnd();
        }

        private void AppendSystemNotice(string text)
        {
            AppendColored("[system] " + text + Environment.NewLine + Environment.NewLine,
                Color.FromArgb(170, 170, 170), false);
            ScrollToEnd();
        }

        private void AppendColored(string text, Color color, bool bold)
        {
            richTextTranscript.SelectionStart = richTextTranscript.TextLength;
            richTextTranscript.SelectionLength = 0;
            richTextTranscript.SelectionColor = color;
            richTextTranscript.SelectionFont = new Font(richTextTranscript.Font,
                bold ? FontStyle.Bold : FontStyle.Regular);
            richTextTranscript.AppendText(text);
            richTextTranscript.SelectionColor = richTextTranscript.ForeColor;
        }

        private void ScrollToEnd()
        {
            richTextTranscript.SelectionStart = richTextTranscript.TextLength;
            richTextTranscript.ScrollToCaret();
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            buttonSend.Enabled = !busy;
            buttonClear.Enabled = !busy;
            buttonExport.Enabled = !busy;
            buttonSettings.Enabled = !busy;
            buttonStop.Enabled = busy;
            labelStatus.Text = busy ? "Sending... (Stop to cancel)" : "Ready (Ctrl+Enter to send)";
            labelStatus.ForeColor = busy ? Color.Orange : Color.LightGray;
        }

        private void Raise(string msg)
        {
            Action<string> h = LogMessage;
            if (h != null) h(msg);
        }
    }
}
