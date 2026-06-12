using System;
using System.Runtime.Serialization;

namespace ChatDemoCs
{
    /// <summary>
    /// Single chat message exchanged with the LLM API. Mirrors the OpenAI-style
    /// schema used by DeepSeek (role + content).
    /// </summary>
    [DataContract]
    public class ChatMessage
    {
        public const string RoleSystem = "system";
        public const string RoleUser = "user";
        public const string RoleAssistant = "assistant";

        [DataMember(Name = "role")]
        public string Role { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        /// <summary>Local timestamp; not sent to the API.</summary>
        public DateTime Timestamp { get; set; }

        public ChatMessage()
        {
            Timestamp = DateTime.Now;
        }

        public ChatMessage(string role, string content)
        {
            Role = role;
            Content = content;
            Timestamp = DateTime.Now;
        }
    }
}
