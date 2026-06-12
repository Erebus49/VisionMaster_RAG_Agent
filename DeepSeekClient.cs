using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ChatDemoCs
{
    /// <summary>
    /// Thin async client for the DeepSeek (and any OpenAI-compatible) Chat Completions API.
    /// Supports both buffered and Server-Sent-Events streaming responses.
    /// </summary>
    public class DeepSeekClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly JavaScriptSerializer _json;
        private bool _disposed;

        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string Model { get; set; }
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }

        public DeepSeekClient()
        {
            _http = new HttpClient();
            _http.Timeout = TimeSpan.FromSeconds(AppSettings.TimeoutSeconds);
            _json = new JavaScriptSerializer();
            _json.MaxJsonLength = int.MaxValue;

            ApiKey = AppSettings.ApiKey;
            BaseUrl = AppSettings.BaseUrl;
            Model = AppSettings.Model;
            Temperature = AppSettings.Temperature;
            MaxTokens = AppSettings.MaxTokens;
        }

        /// <summary>
        /// Sends a chat completion request and returns the full assistant message.
        /// </summary>
        public async Task<string> SendChatAsync(IEnumerable<ChatMessage> messages, CancellationToken cancellationToken)
        {
            EnsureConfigured();
            string url = CombineUrl(BaseUrl, "/v1/chat/completions");
            string body = BuildRequestBody(messages, false);

            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url))
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                req.Content = new StringContent(body, Encoding.UTF8, "application/json");

                using (HttpResponseMessage resp = await _http.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false))
                {
                    string raw = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!resp.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException("HTTP " + (int)resp.StatusCode + " " + resp.ReasonPhrase + ": " + raw);
                    }
                    return ExtractAssistantContent(raw);
                }
            }
        }

        /// <summary>
        /// Sends a streaming chat completion request, invoking <paramref name="onChunk"/>
        /// once per partial token batch on the calling synchronization context.
        /// Returns the concatenated assistant content when the stream finishes.
        /// </summary>
        public async Task<string> SendChatStreamingAsync(IEnumerable<ChatMessage> messages,
            Action<string> onChunk, CancellationToken cancellationToken)
        {
            EnsureConfigured();
            string url = CombineUrl(BaseUrl, "/v1/chat/completions");
            string body = BuildRequestBody(messages, true);

            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url))
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
                req.Content = new StringContent(body, Encoding.UTF8, "application/json");

                using (HttpResponseMessage resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
                {
                    if (!resp.IsSuccessStatusCode)
                    {
                        string err = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new InvalidOperationException("HTTP " + (int)resp.StatusCode + " " + resp.ReasonPhrase + ": " + err);
                    }

                    StringBuilder full = new StringBuilder();
                    using (Stream stream = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            string line = await reader.ReadLineAsync().ConfigureAwait(false);
                            if (line == null) break;
                            if (line.Length == 0) continue;
                            if (!line.StartsWith("data:", StringComparison.Ordinal)) continue;

                            string payload = line.Substring(5).Trim();
                            if (payload == "[DONE]") break;

                            string delta = ExtractStreamDelta(payload);
                            if (!string.IsNullOrEmpty(delta))
                            {
                                full.Append(delta);
                                if (onChunk != null) onChunk(delta);
                            }
                        }
                    }
                    return full.ToString();
                }
            }
        }

        private void EnsureConfigured()
        {
            if (string.IsNullOrEmpty(ApiKey))
                throw new InvalidOperationException("API Key is empty. Configure it in the Settings panel.");
            if (string.IsNullOrEmpty(BaseUrl))
                throw new InvalidOperationException("Base URL is empty. Configure it in the Settings panel.");
            if (string.IsNullOrEmpty(Model))
                throw new InvalidOperationException("Model is empty. Configure it in the Settings panel.");
        }

        private string BuildRequestBody(IEnumerable<ChatMessage> messages, bool stream)
        {
            List<Dictionary<string, object>> msgArray = new List<Dictionary<string, object>>();
            foreach (ChatMessage m in messages)
            {
                if (m == null || string.IsNullOrEmpty(m.Content)) continue;
                Dictionary<string, object> entry = new Dictionary<string, object>(2);
                entry["role"] = m.Role ?? ChatMessage.RoleUser;
                entry["content"] = m.Content;
                msgArray.Add(entry);
            }

            Dictionary<string, object> root = new Dictionary<string, object>();
            root["model"] = Model;
            root["messages"] = msgArray;
            root["temperature"] = Temperature;
            root["max_tokens"] = MaxTokens;
            root["stream"] = stream;
            return _json.Serialize(root);
        }

        private string ExtractAssistantContent(string responseJson)
        {
            try
            {
                object parsed = _json.DeserializeObject(responseJson);
                Dictionary<string, object> root = parsed as Dictionary<string, object>;
                if (root == null) return responseJson;

                object choicesObj;
                if (!root.TryGetValue("choices", out choicesObj)) return responseJson;
                object[] choices = choicesObj as object[];
                if (choices == null || choices.Length == 0) return responseJson;

                Dictionary<string, object> first = choices[0] as Dictionary<string, object>;
                if (first == null) return responseJson;

                object messageObj;
                if (!first.TryGetValue("message", out messageObj)) return responseJson;
                Dictionary<string, object> message = messageObj as Dictionary<string, object>;
                if (message == null) return responseJson;

                object contentObj;
                if (!message.TryGetValue("content", out contentObj)) return responseJson;
                return contentObj == null ? string.Empty : contentObj.ToString();
            }
            catch
            {
                return responseJson;
            }
        }

        private string ExtractStreamDelta(string payload)
        {
            try
            {
                object parsed = _json.DeserializeObject(payload);
                Dictionary<string, object> root = parsed as Dictionary<string, object>;
                if (root == null) return null;

                object choicesObj;
                if (!root.TryGetValue("choices", out choicesObj)) return null;
                object[] choices = choicesObj as object[];
                if (choices == null || choices.Length == 0) return null;

                Dictionary<string, object> first = choices[0] as Dictionary<string, object>;
                if (first == null) return null;

                object deltaObj;
                if (!first.TryGetValue("delta", out deltaObj)) return null;
                Dictionary<string, object> delta = deltaObj as Dictionary<string, object>;
                if (delta == null) return null;

                object contentObj;
                if (!delta.TryGetValue("content", out contentObj)) return null;
                return contentObj == null ? null : contentObj.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static string CombineUrl(string baseUrl, string path)
        {
            if (string.IsNullOrEmpty(baseUrl)) return path;
            string trimmed = baseUrl.TrimEnd('/');
            // Allow callers to point BaseUrl directly at the chat endpoint.
            if (trimmed.EndsWith("/chat/completions", StringComparison.OrdinalIgnoreCase)) return trimmed;
            if (trimmed.EndsWith("/v1", StringComparison.OrdinalIgnoreCase)) return trimmed + "/chat/completions";
            return trimmed + path;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _http.Dispose();
        }
    }
}
