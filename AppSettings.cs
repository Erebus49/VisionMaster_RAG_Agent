using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;

namespace ChatDemoCs
{
    /// <summary>
    /// Wraps reading / writing configuration values from App.config.
    /// Edits are persisted by re-saving the configuration file in place,
    /// which keeps the demo dependency-free (no NuGet, no user.config gymnastics).
    /// </summary>
    public static class AppSettings
    {
        public const string KeyApiKey = "DeepSeek.ApiKey";
        public const string KeyBaseUrl = "DeepSeek.BaseUrl";
        public const string KeyModel = "DeepSeek.Model";
        public const string KeyTemperature = "DeepSeek.Temperature";
        public const string KeyMaxTokens = "DeepSeek.MaxTokens";
        public const string KeyTimeoutSeconds = "DeepSeek.TimeoutSeconds";
        public const string KeySystemPrompt = "DeepSeek.SystemPrompt";
        public const string KeyVmManualRagEnabled = "VmManualRag.Enabled";
        public const string KeyVmManualRagChmPath = "VmManualRag.ChmPath";
        public const string KeyVmManualRagIndexDirectory = "VmManualRag.IndexDirectory";
        public const string KeyVmManualRagTopK = "VmManualRag.TopK";
        public const string KeyVmManualRagMaxContextChars = "VmManualRag.MaxContextChars";

        public static string ApiKey
        {
            get { return GetString(KeyApiKey, string.Empty); }
            set { SetString(KeyApiKey, value ?? string.Empty); }
        }

        public static string BaseUrl
        {
            get { return GetString(KeyBaseUrl, "https://api.deepseek.com"); }
            set { SetString(KeyBaseUrl, value ?? string.Empty); }
        }

        public static string Model
        {
            get { return GetString(KeyModel, "deepseek-chat"); }
            set { SetString(KeyModel, value ?? string.Empty); }
        }

        public static double Temperature
        {
            get { return GetDouble(KeyTemperature, 0.7); }
            set { SetString(KeyTemperature, value.ToString("0.##", CultureInfo.InvariantCulture)); }
        }

        public static int MaxTokens
        {
            get { return GetInt(KeyMaxTokens, 2048); }
            set { SetString(KeyMaxTokens, value.ToString(CultureInfo.InvariantCulture)); }
        }

        public static int TimeoutSeconds
        {
            get { return GetInt(KeyTimeoutSeconds, 120); }
            set { SetString(KeyTimeoutSeconds, value.ToString(CultureInfo.InvariantCulture)); }
        }

        public static string SystemPrompt
        {
            get
            {
                return GetString(KeySystemPrompt,
                    "你是专门服务海康机器人 VisionMaster/VM 平台的中文 AI 助手，优先基于已检索到的平台说明手册回答；当手册资料不足时要明确说明，并给出谨慎的工程建议。");
            }
            set { SetString(KeySystemPrompt, value ?? string.Empty); }
        }

        public static bool VmManualRagEnabled
        {
            get { return GetBool(KeyVmManualRagEnabled, true); }
            set { SetString(KeyVmManualRagEnabled, value ? "true" : "false"); }
        }

        public static string VmManualRagChmPath
        {
            get { return GetString(KeyVmManualRagChmPath, string.Empty); }
            set { SetString(KeyVmManualRagChmPath, value ?? string.Empty); }
        }

        public static string VmManualRagIndexDirectory
        {
            get { return GetString(KeyVmManualRagIndexDirectory, string.Empty); }
            set { SetString(KeyVmManualRagIndexDirectory, value ?? string.Empty); }
        }

        public static int VmManualRagTopK
        {
            get { return Math.Max(1, Math.Min(8, GetInt(KeyVmManualRagTopK, 4))); }
            set { SetString(KeyVmManualRagTopK, value.ToString(CultureInfo.InvariantCulture)); }
        }

        public static int VmManualRagMaxContextChars
        {
            get { return Math.Max(1500, Math.Min(12000, GetInt(KeyVmManualRagMaxContextChars, 6000))); }
            set { SetString(KeyVmManualRagMaxContextChars, value.ToString(CultureInfo.InvariantCulture)); }
        }

        private static string GetString(string key, string fallback)
        {
            try
            {
                string val = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(val) ? fallback : val;
            }
            catch
            {
                return fallback;
            }
        }

        private static int GetInt(string key, int fallback)
        {
            int v;
            return int.TryParse(GetString(key, fallback.ToString(CultureInfo.InvariantCulture)),
                NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : fallback;
        }

        private static bool GetBool(string key, bool fallback)
        {
            bool v;
            string text = GetString(key, fallback ? "true" : "false");
            if (bool.TryParse(text, out v)) return v;
            int numeric;
            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out numeric)) return numeric != 0;
            return fallback;
        }

        private static double GetDouble(string key, double fallback)
        {
            double v;
            return double.TryParse(GetString(key, fallback.ToString(CultureInfo.InvariantCulture)),
                NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : fallback;
        }

        /// <summary>
        /// Persists a value back to App.config (the .exe.config next to the running executable).
        /// </summary>
        private static void SetString(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                else
                {
                    config.AppSettings.Settings[key].Value = value;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                // As a fall-back keep the value only in-process.
                System.Diagnostics.Debug.WriteLine("AppSettings.SetString failed: " + ex.Message);
            }
        }
    }
}
