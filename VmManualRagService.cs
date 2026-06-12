using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace ChatDemoCs
{
    public class VmRagAugmentation
    {
        public List<ChatMessage> Messages { get; set; }
        public string Status { get; set; }
    }

    public sealed class VmManualRagService
    {
        private static readonly Lazy<VmManualRagService> LazyInstance = new Lazy<VmManualRagService>(() => new VmManualRagService());
        private readonly object syncRoot = new object();
        private readonly JavaScriptSerializer json = new JavaScriptSerializer();
        private List<ManualChunk> chunks;
        private Dictionary<string, int> documentFrequency;
        private bool loaded;
        private string loadStatus;

        public static VmManualRagService Instance
        {
            get { return LazyInstance.Value; }
        }

        private VmManualRagService()
        {
            json.MaxJsonLength = int.MaxValue;
            chunks = new List<ManualChunk>();
            documentFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        public VmRagAugmentation Augment(IEnumerable<ChatMessage> conversation, string userQuestion)
        {
            List<ChatMessage> messages = CopyMessages(conversation);
            if (!AppSettings.VmManualRagEnabled)
            {
                return new VmRagAugmentation { Messages = messages, Status = string.Empty };
            }

            EnsureLoaded();
            if (chunks.Count == 0)
            {
                return new VmRagAugmentation { Messages = messages, Status = loadStatus };
            }

            List<SearchHit> hits = Search(userQuestion, AppSettings.VmManualRagTopK);
            if (hits.Count == 0)
            {
                return new VmRagAugmentation { Messages = messages, Status = loadStatus + " 未检索到相关 VM 手册片段。" };
            }

            string augmentedUserPrompt = BuildAugmentedUserPrompt(userQuestion, hits, AppSettings.VmManualRagMaxContextChars);
            ReplaceLastUserMessage(messages, augmentedUserPrompt);
            return new VmRagAugmentation
            {
                Messages = messages,
                Status = loadStatus + " 已检索 VM 手册片段 " + hits.Count.ToString(CultureInfo.InvariantCulture) + " 条。"
            };
        }

        private void EnsureLoaded()
        {
            lock (syncRoot)
            {
                if (loaded) return;
                loaded = true;
                chunks = new List<ManualChunk>();
                documentFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                string chmPath = ResolveManualPath();
                if (string.IsNullOrEmpty(chmPath) || !File.Exists(chmPath))
                {
                    loadStatus = "VM 手册 RAG 未启用：找不到 CHM 文件 " + chmPath;
                    return;
                }

                try
                {
                    string cacheRoot = GetCacheRoot();
                    Directory.CreateDirectory(cacheRoot);
                    string indexPath = Path.Combine(cacheRoot, "index.json");
                    string metaPath = Path.Combine(cacheRoot, "meta.txt");
                    string metadata = BuildMetadata(chmPath);

                    if (File.Exists(indexPath) && File.Exists(metaPath) && File.ReadAllText(metaPath, Encoding.UTF8) == metadata)
                    {
                        chunks = LoadChunks(indexPath);
                    }
                    if (chunks.Count == 0)
                    {
                        string extractDir = Path.Combine(cacheRoot, "extract");
                        if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                        Directory.CreateDirectory(extractDir);
                        string localChmPath = Path.Combine(cacheRoot, "manual.chm");
                        CopyMainDataStream(chmPath, localChmPath);
                        DecompileChm(localChmPath, extractDir);
                        chunks = BuildChunksFromDirectory(extractDir);
                        SaveChunks(indexPath, chunks);
                        File.WriteAllText(metaPath, metadata, Encoding.UTF8);
                    }

                    BuildDocumentFrequency();
                    loadStatus = "VM 手册 RAG 已加载 " + chunks.Count.ToString(CultureInfo.InvariantCulture) + " 个片段。";
                }
                catch (Exception ex)
                {
                    chunks = new List<ManualChunk>();
                    documentFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    loadStatus = "VM 手册 RAG 加载失败：" + ex.Message;
                }
            }
        }

        private string GetCacheRoot()
        {
            string configured = AppSettings.VmManualRagIndexDirectory;
            if (!string.IsNullOrEmpty(configured)) return configured;
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(local, "ChatDemoCs", "VmManualRag");
        }

        private string ResolveManualPath()
        {
            string assemblyDir = Path.GetDirectoryName(typeof(VmManualRagService).Assembly.Location);
            string startupManual = Path.Combine(assemblyDir ?? AppDomain.CurrentDomain.BaseDirectory, "Manuals", "HikRobotVMHelp.chm");
            if (File.Exists(startupManual)) return startupManual;

            string configured = AppSettings.VmManualRagChmPath;
            if (!string.IsNullOrEmpty(configured) && File.Exists(configured)) return configured;

            return startupManual;
        }

        private string BuildMetadata(string chmPath)
        {
            FileInfo info = new FileInfo(chmPath);
            return "v6|" + info.FullName + "|" + info.Length.ToString(CultureInfo.InvariantCulture) + "|" + info.LastWriteTimeUtc.Ticks.ToString(CultureInfo.InvariantCulture);
        }

        private void CopyMainDataStream(string sourcePath, string destinationPath)
        {
            if (File.Exists(destinationPath)) File.Delete(destinationPath);
            using (FileStream source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                source.CopyTo(destination);
            }
        }

        private void DecompileChm(string chmPath, string extractDir)
        {
            string hh = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "hh.exe");
            if (!File.Exists(hh)) throw new FileNotFoundException("找不到 hh.exe，无法解包 CHM。", hh);

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "powershell.exe";
            string psCommand = "& " + QuotePowerShell(hh) + " -decompile " + QuotePowerShell(extractDir) + " " + QuotePowerShell(chmPath);
            psi.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + psCommand.Replace("\"", "\\\"") + "\"";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            using (Process process = Process.Start(psi))
            {
                if (process == null) throw new InvalidOperationException("启动 CHM 解包进程失败。 ");
                if (!process.WaitForExit(120000))
                {
                    try { process.Kill(); }
                    catch { }
                    throw new TimeoutException("解包 CHM 超时。 ");
                }
            }
            WaitForExtractedFiles(extractDir);
            int fileCount = Directory.GetFiles(extractDir, "*", SearchOption.AllDirectories).Length;
            if (fileCount == 0) throw new InvalidOperationException("CHM 解包失败：hh.exe 未输出任何文件。");
        }

        private string QuotePowerShell(string value)
        {
            return "'" + (value ?? string.Empty).Replace("'", "''") + "'";
        }

        private void WaitForExtractedFiles(string extractDir)
        {
            DateTime deadline = DateTime.Now.AddSeconds(30);
            int lastCount = -1;
            int stableCount = 0;
            while (DateTime.Now < deadline)
            {
                int count = 0;
                try
                {
                    count = Directory.GetFiles(extractDir, "*", SearchOption.AllDirectories).Length;
                }
                catch
                {
                    count = 0;
                }

                if (count > 0 && count == lastCount)
                {
                    stableCount++;
                    if (stableCount >= 3) return;
                }
                else
                {
                    stableCount = 0;
                    lastCount = count;
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        private List<ManualChunk> LoadChunks(string indexPath)
        {
            string raw = File.ReadAllText(indexPath, Encoding.UTF8);
            ManualChunk[] data = json.Deserialize<ManualChunk[]>(raw);
            if (data == null) return new List<ManualChunk>();
            return data.Where(c => c != null && !string.IsNullOrEmpty(c.Text)).ToList();
        }

        private void SaveChunks(string indexPath, List<ManualChunk> data)
        {
            File.WriteAllText(indexPath, json.Serialize(data.ToArray()), Encoding.UTF8);
        }

        private List<ManualChunk> BuildChunksFromDirectory(string extractDir)
        {
            List<ManualChunk> result = new List<ManualChunk>();
            string[] patterns = new string[] { "*.htm", "*.html", "*.txt" };
            foreach (string pattern in patterns)
            {
                foreach (string file in Directory.GetFiles(extractDir, pattern, SearchOption.AllDirectories))
                {
                    string raw = ReadTextFile(file);
                    string title;
                    string text = ExtractReadableText(raw, Path.GetFileNameWithoutExtension(file), out title);
                    if (string.IsNullOrWhiteSpace(text) || text.Length < 40) continue;
                    AddTextChunks(result, title, MakeRelativePath(extractDir, file), text);
                }
            }
            return result;
        }

        private string ReadTextFile(string file)
        {
            byte[] bytes = File.ReadAllBytes(file);
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF) return Encoding.UTF8.GetString(bytes);
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE) return Encoding.Unicode.GetString(bytes);
            string asciiHead = Encoding.ASCII.GetString(bytes, 0, Math.Min(bytes.Length, 4096));
            Match charset = Regex.Match(asciiHead, "charset\\s*=\\s*[\"']?([^\"'\\s>]+)", RegexOptions.IgnoreCase);
            if (charset.Success)
            {
                try { return Encoding.GetEncoding(charset.Groups[1].Value).GetString(bytes); }
                catch { }
            }
            try { return new UTF8Encoding(false, true).GetString(bytes); }
            catch { return Encoding.GetEncoding("GB18030").GetString(bytes); }
        }

        private string ExtractReadableText(string raw, string fallbackTitle, out string title)
        {
            title = fallbackTitle;
            if (string.IsNullOrEmpty(raw)) return string.Empty;
            Match titleMatch = Regex.Match(raw, "<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (titleMatch.Success)
            {
                title = NormalizeText(WebUtility.HtmlDecode(StripTags(titleMatch.Groups[1].Value)));
                if (string.IsNullOrEmpty(title)) title = fallbackTitle;
            }
            string text = Regex.Replace(raw, "<script[\\s\\S]*?</script>", " ", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "<style[\\s\\S]*?</style>", " ", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "<(br|p|div|li|tr|h[1-6])[^>]*>", "\n", RegexOptions.IgnoreCase);
            text = StripTags(text);
            text = WebUtility.HtmlDecode(text);
            return NormalizeText(text);
        }

        private string StripTags(string text)
        {
            return Regex.Replace(text ?? string.Empty, "<[^>]+>", " ");
        }

        private string NormalizeText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.Replace('\u00A0', ' ');
            text = Regex.Replace(text, "[ \t\r\f\v]+", " ");
            text = Regex.Replace(text, "\n\\s*\n+", "\n");
            return text.Trim();
        }

        private void AddTextChunks(List<ManualChunk> result, string title, string source, string text)
        {
            int max = 1200;
            int overlap = 120;
            int start = 0;
            while (start < text.Length)
            {
                int len = Math.Min(max, text.Length - start);
                string chunk = text.Substring(start, len).Trim();
                if (chunk.Length >= 80)
                {
                    result.Add(new ManualChunk { Title = title, Source = source, Text = chunk });
                }
                if (start + len >= text.Length) break;
                start += Math.Max(1, max - overlap);
            }
        }

        private string MakeRelativePath(string root, string file)
        {
            if (file.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                return file.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            return Path.GetFileName(file);
        }

        private void BuildDocumentFrequency()
        {
            documentFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (ManualChunk chunk in chunks)
            {
                Dictionary<string, int> counts = CountTokens(chunk.Title + " " + chunk.Source + " " + chunk.Text);
                chunk.TokenCounts = counts;
                foreach (string token in counts.Keys)
                {
                    int count;
                    documentFrequency.TryGetValue(token, out count);
                    documentFrequency[token] = count + 1;
                }
            }
        }

        private List<SearchHit> Search(string query, int topK)
        {
            Dictionary<string, int> queryTokens = CountTokens(query);
            List<SearchHit> hits = new List<SearchHit>();
            if (queryTokens.Count == 0) return hits;

            int total = Math.Max(1, chunks.Count);
            foreach (ManualChunk chunk in chunks)
            {
                double score = 0;
                foreach (string token in queryTokens.Keys)
                {
                    int tf;
                    if (!chunk.TokenCounts.TryGetValue(token, out tf)) continue;
                    int df;
                    documentFrequency.TryGetValue(token, out df);
                    double idf = Math.Log((1.0 + total) / (1.0 + df)) + 1.0;
                    score += tf * idf;
                    if (!string.IsNullOrEmpty(chunk.Title) && chunk.Title.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) score += 3.0 * idf;
                    if (!string.IsNullOrEmpty(chunk.Source) && chunk.Source.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) score += 1.5 * idf;
                }
                if (score <= 0) continue;
                score = score / Math.Sqrt(Math.Max(20, chunk.TokenCounts.Count));
                hits.Add(new SearchHit { Chunk = chunk, Score = score });
            }

            return hits.OrderByDescending(h => h.Score).Take(Math.Max(1, topK)).ToList();
        }

        private Dictionary<string, int> CountTokens(string text)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (string token in Tokenize(text))
            {
                int count;
                counts.TryGetValue(token, out count);
                counts[token] = count + 1;
            }
            return counts;
        }

        private IEnumerable<string> Tokenize(string text)
        {
            List<string> tokens = new List<string>();
            if (string.IsNullOrEmpty(text)) return tokens;
            StringBuilder latin = new StringBuilder();
            List<char> cjkRun = new List<char>();

            Action flushLatin = delegate
            {
                if (latin.Length >= 2)
                {
                    string token = latin.ToString().ToLowerInvariant();
                    tokens.Add(token);
                    foreach (string part in SplitCamel(token)) tokens.Add(part);
                }
                latin.Length = 0;
            };

            Action flushCjk = delegate
            {
                if (cjkRun.Count > 0)
                {
                    for (int i = 0; i < cjkRun.Count; i++) tokens.Add(cjkRun[i].ToString());
                    for (int i = 0; i < cjkRun.Count - 1; i++) tokens.Add(new string(new char[] { cjkRun[i], cjkRun[i + 1] }));
                }
                cjkRun.Clear();
            };

            foreach (char ch in text)
            {
                if (IsCjk(ch))
                {
                    flushLatin();
                    cjkRun.Add(ch);
                }
                else if (char.IsLetterOrDigit(ch) || ch == '_' || ch == '#')
                {
                    flushCjk();
                    latin.Append(ch);
                }
                else
                {
                    flushLatin();
                    flushCjk();
                }
            }
            flushLatin();
            flushCjk();
            return tokens;
        }

        private IEnumerable<string> SplitCamel(string token)
        {
            List<string> result = new List<string>();
            foreach (Match match in Regex.Matches(token, "[a-z]+|[0-9]+"))
            {
                if (match.Value.Length >= 2) result.Add(match.Value);
            }
            return result;
        }

        private bool IsCjk(char ch)
        {
            return (ch >= 0x4e00 && ch <= 0x9fff) || (ch >= 0x3400 && ch <= 0x4dbf) || (ch >= 0xf900 && ch <= 0xfaff);
        }

        private string BuildAugmentedUserPrompt(string userQuestion, List<SearchHit> hits, int maxChars)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("你是专门适应海康机器人 VisionMaster/VM 平台的聊天助手。请优先依据下面从平台说明手册检索到的资料回答；资料不足时明确说明不足，并结合通用机器视觉和 C#/.NET 经验给出谨慎建议。");
            sb.AppendLine();
            sb.AppendLine("【VM 平台说明手册相关片段】");
            int index = 1;
            foreach (SearchHit hit in hits)
            {
                string header = "[片段" + index.ToString(CultureInfo.InvariantCulture) + "] " + hit.Chunk.Title + " / " + hit.Chunk.Source;
                string block = header + Environment.NewLine + hit.Chunk.Text + Environment.NewLine;
                if (sb.Length + block.Length > maxChars) break;
                sb.AppendLine(block);
                index++;
            }
            sb.AppendLine("【用户问题】");
            sb.AppendLine(userQuestion);
            return sb.ToString();
        }

        private List<ChatMessage> CopyMessages(IEnumerable<ChatMessage> conversation)
        {
            List<ChatMessage> messages = new List<ChatMessage>();
            if (conversation == null) return messages;
            foreach (ChatMessage message in conversation)
            {
                if (message == null) continue;
                ChatMessage copy = new ChatMessage(message.Role, message.Content);
                copy.Timestamp = message.Timestamp;
                messages.Add(copy);
            }
            return messages;
        }

        private void ReplaceLastUserMessage(List<ChatMessage> messages, string content)
        {
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                if (messages[i].Role == ChatMessage.RoleUser)
                {
                    messages[i].Content = content;
                    return;
                }
            }
            messages.Add(new ChatMessage(ChatMessage.RoleUser, content));
        }

        private class SearchHit
        {
            public ManualChunk Chunk { get; set; }
            public double Score { get; set; }
        }

        public class ManualChunk
        {
            public string Title { get; set; }
            public string Source { get; set; }
            public string Text { get; set; }
            public Dictionary<string, int> TokenCounts { get; set; }
        }
    }
}
