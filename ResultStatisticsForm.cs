using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatDemoCs
{
    /// <summary>
    /// Modal dialog that visualises the per-label counts collected during a
    /// continuous-run window and lets the user ask DeepSeek to comment on the
    /// distribution. The chart is painted by hand with GDI+ so we don't need
    /// to add a NuGet dependency to the demo.
    /// </summary>
    public partial class ResultStatisticsForm : Form
    {
        private readonly List<KeyValuePair<string, int>> _data;
        private readonly int _total;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        private readonly string _procedureName;
        private readonly bool _isChinese;
        private readonly string _diagnostic;

        private readonly Color[] _palette = new[]
        {
            Color.FromArgb(0xFF, 0x4F, 0xC3, 0xF7),
            Color.FromArgb(0xFF, 0x81, 0xC7, 0x84),
            Color.FromArgb(0xFF, 0xFF, 0xB7, 0x4D),
            Color.FromArgb(0xFF, 0xE5, 0x73, 0x73),
            Color.FromArgb(0xFF, 0xBA, 0x68, 0xC8),
            Color.FromArgb(0xFF, 0xFF, 0xD5, 0x4F),
            Color.FromArgb(0xFF, 0x4D, 0xD0, 0xE1),
            Color.FromArgb(0xFF, 0xA1, 0x88, 0x7F),
        };

        private CancellationTokenSource _cts;
        private const int PdfPagePixelWidth = 794;
        private const int PdfPagePixelHeight = 1123;
        private const float PdfPagePointWidth = 595f;
        private const float PdfPagePointHeight = 842f;

        public ResultStatisticsForm(ResultStatistics stats)
        {
            InitializeComponent();
            if (stats == null) throw new ArgumentNullException("stats");

            _data = stats.SnapshotSorted();
            _total = stats.TotalSamples;
            _startTime = stats.StartTime;
            _endTime = stats.LastUpdate;
            _procedureName = stats.ProcedureName ?? string.Empty;
            _diagnostic = stats.LastDiagnostic ?? string.Empty;
            _isChinese = Thread.CurrentThread.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase);

            ApplyLanguage();
            chartPanel.Paint += chartPanel_Paint;
            chartPanel.Resize += (s, e) => chartPanel.Invalidate();
            FillSummary();
            FillTable();
            FillDiagnostic();
            Shown += (s, e) =>
            {
                if (_total > 0)
                {
                    buttonAnalyze_Click(s, e);
                }
            };
        }

        private void ApplyLanguage()
        {
            if (_isChinese)
            {
                this.Text = "结果统计";
                colRank.Text = "序号";
                colLabel.Text = "标签";
                colCount.Text = "数量";
                colPercent.Text = "占比";
                labelAiTitle.Text = "AI 分析";
                buttonAnalyze.Text = "AI 分析";
                buttonExport.Text = "结果导出";
                buttonCopy.Text = "复制";
                buttonClear.Text = "清除";
                buttonClose.Text = "关闭";
            }
            else
            {
                this.Text = "Result Statistics";
                colRank.Text = "#";
                colLabel.Text = "Label";
                colCount.Text = "Count";
                colPercent.Text = "Percent";
                labelAiTitle.Text = "AI Analysis";
                buttonAnalyze.Text = "Analyze";
                buttonExport.Text = "Export";
                buttonCopy.Text = "Copy";
                buttonClear.Text = "Clear";
                buttonClose.Text = "Close";
            }
        }

        private void FillSummary()
        {
            string template = _isChinese
                ? "流程：{0}    总样本：{1}    类别数：{2}    时间窗口：{3} - {4}"
                : "Procedure: {0}    Total: {1}    Classes: {2}    Window: {3} - {4}";
            labelSummary.Text = string.Format(CultureInfo.InvariantCulture, template,
                string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName,
                _total,
                _data.Count,
                _startTime.ToString("HH:mm:ss"),
                _endTime.ToString("HH:mm:ss"));
        }

        private void FillTable()
        {
            listViewStats.BeginUpdate();
            listViewStats.Items.Clear();
            int idx = 0;
            foreach (KeyValuePair<string, int> kv in _data)
            {
                ListViewItem item = new ListViewItem((idx + 1).ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(kv.Key);
                item.SubItems.Add(kv.Value.ToString(CultureInfo.InvariantCulture));
                double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                item.SubItems.Add(pct.ToString("0.00", CultureInfo.InvariantCulture) + "%");
                item.UseItemStyleForSubItems = false;
                item.SubItems[1].ForeColor = _palette[idx % _palette.Length];
                listViewStats.Items.Add(item);
                idx++;
            }
            listViewStats.EndUpdate();
        }

        private void FillDiagnostic()
        {
            if (_total > 0) return;
            richTextBoxAi.Text = _isChinese
                ? "未采集到标签数据。\r\n\r\n请检查下方诊断信息，确认当前流程是否有 CLASSINFO、字符串 out、标签/类别/分类等输出：\r\n\r\n" + _diagnostic
                : "No label statistics were collected.\r\n\r\nCheck the diagnostic details below to confirm whether the procedure exposes CLASSINFO, string out, label/class outputs, etc.:\r\n\r\n" + _diagnostic;
        }

        private void chartPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(chartPanel.BackColor);

            if (_data == null || _data.Count == 0)
            {
                using (Font f = new Font("Segoe UI", 11f))
                using (StringFormat fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(_isChinese ? "暂无数据" : "No data",
                        f, Brushes.Gray, chartPanel.ClientRectangle, fmt);
                }
                return;
            }

            Rectangle bounds = chartPanel.ClientRectangle;
            bounds.Inflate(-12, -10);
            int gap = 18;
            int halfWidth = (bounds.Width - gap) / 2;
            Rectangle barBounds = new Rectangle(bounds.Left, bounds.Top, halfWidth, bounds.Height);
            Rectangle pieBounds = new Rectangle(barBounds.Right + gap, bounds.Top, bounds.Right - barBounds.Right - gap, bounds.Height);

            DrawBarChart(g, barBounds);
            DrawPieChart(g, pieBounds);
        }

        private void DrawBarChart(Graphics g, Rectangle bounds)
        {
            int padTop = 32;
            int padBottom = 12;
            int padLeft = 4;
            int padRight = 4;
            int labelWidth = Math.Min(90, Math.Max(62, bounds.Width / 4));
            int countWidth = 78;
            int barAreaLeft = bounds.Left + padLeft + labelWidth + 6;
            int barAreaRight = bounds.Right - padRight - countWidth - 6;
            int barAreaWidth = Math.Max(20, barAreaRight - barAreaLeft);

            int rowCount = _data.Count;
            int rowHeight = Math.Max(28, (bounds.Height - padTop - padBottom) / Math.Max(1, rowCount));

            int yOffset = bounds.Top + padTop;

            int maxCount = 0;
            foreach (KeyValuePair<string, int> kv in _data) if (kv.Value > maxCount) maxCount = kv.Value;
            if (maxCount == 0) maxCount = 1;

            using (Font f = new Font("Segoe UI", 9f))
            using (Font fbold = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (SolidBrush brushFg = new SolidBrush(Color.White))
            using (SolidBrush brushDim = new SolidBrush(Color.FromArgb(220, 200, 200, 200)))
            using (SolidBrush brushTrack = new SolidBrush(Color.FromArgb(36, 255, 255, 255)))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(235, 235, 235)))
            using (StringFormat leftFmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            using (StringFormat rightFmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(_isChinese ? "柱状图" : "Bar Chart", fbold, titleBrush, new RectangleF(bounds.Left, bounds.Top, bounds.Width, 22), leftFmt);
                for (int i = 0; i < rowCount; i++)
                {
                    KeyValuePair<string, int> kv = _data[i];
                    int y = yOffset + i * rowHeight;
                    int barH = Math.Min(16, Math.Max(8, rowHeight / 4));
                    int barY = y + (rowHeight - barH) / 2;
                    int barLen = (int)Math.Round(barAreaWidth * (kv.Value / (double)maxCount));
                    if (barLen < 2) barLen = 2;

                    Color color = _palette[i % _palette.Length];
                    using (SolidBrush br = new SolidBrush(color))
                    using (LinearGradientBrush grad = new LinearGradientBrush(
                        new Rectangle(barAreaLeft, barY, Math.Max(1, barLen), barH),
                        ControlPaint.Light(color),
                        color,
                        LinearGradientMode.Horizontal))
                    {
                        Rectangle trackRect = new Rectangle(barAreaLeft, barY, barAreaWidth, barH);
                        g.FillRectangle(brushTrack, trackRect);
                        Rectangle barRect = new Rectangle(barAreaLeft, barY, barLen, barH);
                        g.FillRectangle(grad, barRect);
                    }

                    RectangleF labelRect = new RectangleF(bounds.Left + padLeft, y, labelWidth, rowHeight);
                    g.DrawString(kv.Key, f, brushFg, labelRect, leftFmt);

                    double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                    string countText = kv.Value.ToString(CultureInfo.InvariantCulture)
                        + " (" + pct.ToString("0.0", CultureInfo.InvariantCulture) + "%)";
                    RectangleF countRect = new RectangleF(barAreaRight + 6, y, countWidth, rowHeight);
                    g.DrawString(countText, f, brushDim, countRect, rightFmt);
                }
            }
        }

        private void DrawPieChart(Graphics g, Rectangle bounds)
        {
            using (Font f = new Font("Segoe UI", 9f))
            using (Font fbold = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(235, 235, 235)))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(220, 220, 220)))
            using (Pen borderPen = new Pen(Color.FromArgb(40, 40, 40), 2f))
            using (StringFormat centerFmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(_isChinese ? "扇形图" : "Pie Chart", fbold, titleBrush, new RectangleF(bounds.Left, bounds.Top, bounds.Width, 22), centerFmt);

                int legendHeight = Math.Min(82, Math.Max(42, _data.Count * 22));
                int diameter = Math.Min(bounds.Width - 24, bounds.Height - 42 - legendHeight);
                diameter = Math.Max(44, diameter);
                Rectangle pieRect = new Rectangle(
                    bounds.Left + (bounds.Width - diameter) / 2,
                    bounds.Top + 32,
                    diameter,
                    diameter);

                float startAngle = -90f;
                for (int i = 0; i < _data.Count; i++)
                {
                    KeyValuePair<string, int> kv = _data[i];
                    float sweepAngle = _total == 0 ? 0f : 360f * kv.Value / _total;
                    Color color = _palette[i % _palette.Length];
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        g.FillPie(brush, pieRect, startAngle, sweepAngle);
                    }
                    g.DrawPie(borderPen, pieRect, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }

                int legendTop = Math.Min(bounds.Bottom - legendHeight + 4, pieRect.Bottom + 14);
                int legendLeft = bounds.Left + Math.Max(8, (bounds.Width - 170) / 2);
                for (int i = 0; i < _data.Count; i++)
                {
                    KeyValuePair<string, int> kv = _data[i];
                    double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                    int y = legendTop + i * 22;
                    Color color = _palette[i % _palette.Length];
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        g.FillRectangle(brush, legendLeft, y + 5, 10, 10);
                    }
                    string text = string.Format(CultureInfo.InvariantCulture, "{0}  {1:0.0}%", kv.Key, pct);
                    g.DrawString(text, f, textBrush, legendLeft + 18, y);
                }
            }
        }

        private async void buttonAnalyze_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AppSettings.ApiKey))
                {
                    AppendAi(_isChinese
                        ? "[请先在“设置”中配置 DeepSeek API Key]\r\n"
                        : "[Configure DeepSeek API Key in Settings before analyzing]\r\n");
                    return;
                }
                if (_total == 0)
                {
                    AppendAi(_isChinese ? "[暂无数据可分析]\r\n" : "[No data to analyze]\r\n");
                    return;
                }

                buttonAnalyze.Enabled = false;
                richTextBoxAi.Clear();
                AppendAi(_isChinese ? "正在请求 AI 助手分析…\r\n\r\n" : "Asking the assistant for an analysis...\r\n\r\n");

                _cts = new CancellationTokenSource();
                DeepSeekClient ai = new DeepSeekClient
                {
                    ApiKey = AppSettings.ApiKey,
                    BaseUrl = AppSettings.BaseUrl,
                    Model = AppSettings.Model,
                    Temperature = AppSettings.Temperature,
                    MaxTokens = AppSettings.MaxTokens,
                };

                string systemPrompt = _isChinese
                    ? "你是机器视觉质检数据分析助手。结合给定的标签计数表，从分布特征、潜在异常、改进建议三个角度作答。优先使用中文，文风简洁，避免空话与重复。"
                    : "You are a machine-vision QA analyst. Given a label-count table, comment on the distribution shape, potential anomalies, and concrete engineering suggestions. Be concise and avoid filler.";

                List<ChatMessage> messages = new List<ChatMessage>
                {
                    new ChatMessage(ChatMessage.RoleSystem, systemPrompt),
                    new ChatMessage(ChatMessage.RoleUser, BuildPrompt()),
                };

                bool firstChunk = true;
                await ai.SendChatStreamingAsync(messages, delta =>
                {
                    if (firstChunk)
                    {
                        firstChunk = false;
                        try { richTextBoxAi.Clear(); } catch { }
                    }
                    AppendAi(delta);
                }, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                AppendAi(_isChinese ? "\r\n[已取消]\r\n" : "\r\n[Cancelled]\r\n");
            }
            catch (Exception ex)
            {
                AppendAi("\r\n[Error] " + ex.Message + "\r\n");
            }
            finally
            {
                buttonAnalyze.Enabled = true;
                if (_cts != null) { _cts.Dispose(); _cts = null; }
            }
        }

        private string BuildPrompt()
        {
            StringBuilder sb = new StringBuilder();
            if (_isChinese)
            {
                sb.AppendLine("以下是 VisionMaster 流程在一次连续运行中的标签统计：");
                sb.AppendLine("流程：" + (string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName));
                sb.AppendLine("总样本：" + _total.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("时间窗口：" + _startTime.ToString("yyyy-MM-dd HH:mm:ss") + " ~ " + _endTime.ToString("HH:mm:ss"));
                sb.AppendLine("类别数：" + _data.Count.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("分布（按数量降序）：");
                int rank = 1;
                foreach (KeyValuePair<string, int> kv in _data)
                {
                    double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "  {0}. {1}: {2} ({3:0.00}%)", rank, kv.Key, kv.Value, pct));
                    rank++;
                }
                sb.AppendLine();
                sb.AppendLine("请基于以上数据：");
                sb.AppendLine("1) 用 1-2 句话概括分布形态（是否一类占主导、是否长尾等）；");
                sb.AppendLine("2) 指出疑似的数据问题或质量异常（例如 NG 占比偏高、罕见类别样本不足等）；");
                sb.AppendLine("3) 给出 2-3 条可操作的工程改进建议（采集、模型、阈值、流程等）。");
            }
            else
            {
                sb.AppendLine("Label statistics from one VisionMaster continuous-run window:");
                sb.AppendLine("Procedure: " + (string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName));
                sb.AppendLine("Total samples: " + _total.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("Window: " + _startTime.ToString("yyyy-MM-dd HH:mm:ss") + " ~ " + _endTime.ToString("HH:mm:ss"));
                sb.AppendLine("Classes: " + _data.Count.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("Distribution (sorted by count desc):");
                int rank = 1;
                foreach (KeyValuePair<string, int> kv in _data)
                {
                    double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "  {0}. {1}: {2} ({3:0.00}%)", rank, kv.Key, kv.Value, pct));
                    rank++;
                }
                sb.AppendLine();
                sb.AppendLine("Please answer in three sections:");
                sb.AppendLine("1) One or two sentences on distribution shape.");
                sb.AppendLine("2) Possible data / quality anomalies.");
                sb.AppendLine("3) Two or three concrete engineering improvements.");
            }
            return sb.ToString();
        }

        private string BuildExportDocument()
        {
            StringBuilder sb = new StringBuilder();
            string exportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (_isChinese)
            {
                sb.AppendLine("结果统计报告");
                sb.AppendLine(new string('=', 32));
                sb.AppendLine("流程：" + (string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName));
                sb.AppendLine("总样本：" + _total.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("类别数：" + _data.Count.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("导出时间：" + exportTime);
                sb.AppendLine();
                sb.AppendLine("统计明细：");
                sb.AppendLine("序号\t标签\t数量\t占比");
            }
            else
            {
                sb.AppendLine("Result Statistics Report");
                sb.AppendLine(new string('=', 32));
                sb.AppendLine("Procedure: " + (string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName));
                sb.AppendLine("Total samples: " + _total.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("Classes: " + _data.Count.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("Export time: " + exportTime);
                sb.AppendLine();
                sb.AppendLine("Statistics:");
                sb.AppendLine("#\tLabel\tCount\tPercent");
            }

            int rank = 1;
            foreach (KeyValuePair<string, int> kv in _data)
            {
                double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "{0}\t{1}\t{2}\t{3:0.00}%", rank, kv.Key, kv.Value, pct));
                rank++;
            }

            if (!string.IsNullOrWhiteSpace(_diagnostic))
            {
                sb.AppendLine();
                sb.AppendLine(_isChinese ? "诊断信息：" : "Diagnostic:");
                sb.AppendLine(_diagnostic);
            }

            sb.AppendLine();
            sb.AppendLine(_isChinese ? "AI 分析：" : "AI Analysis:");
            sb.AppendLine(string.IsNullOrWhiteSpace(richTextBoxAi.Text)
                ? (_isChinese ? "暂无 AI 分析内容。" : "No AI analysis content.")
                : richTextBoxAi.Text);

            return sb.ToString();
        }

        private string BuildExportHtmlDocument()
        {
            string exportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string chartImage = CaptureChartImageBase64();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"utf-8\" />");
            sb.AppendLine("<title>" + HtmlEncode(_isChinese ? "结果统计报告" : "Result Statistics Report") + "</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body{font-family:'Segoe UI','Microsoft YaHei',Arial,sans-serif;margin:32px;color:#222;}");
            sb.AppendLine("h1{font-size:24px;margin:0 0 20px 0;}");
            sb.AppendLine(".meta{line-height:1.9;margin-bottom:22px;}");
            sb.AppendLine("table{border-collapse:collapse;width:100%;margin:12px 0 24px 0;}");
            sb.AppendLine("th,td{border:1px solid #ccc;padding:8px 10px;text-align:left;}");
            sb.AppendLine("th{background:#f2f2f2;}");
            sb.AppendLine(".chart{margin:12px 0 24px 0;}");
            sb.AppendLine(".chart img{max-width:100%;border:1px solid #ddd;background:#282828;}");
            sb.AppendLine("pre{white-space:pre-wrap;word-wrap:break-word;background:#f7f7f7;border:1px solid #ddd;padding:12px;}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>" + HtmlEncode(_isChinese ? "结果统计报告" : "Result Statistics Report") + "</h1>");
            sb.AppendLine("<div class=\"meta\">");
            sb.AppendLine("<div><strong>" + HtmlEncode(_isChinese ? "流程：" : "Procedure: ") + "</strong>" + HtmlEncode(string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName) + "</div>");
            sb.AppendLine("<div><strong>" + HtmlEncode(_isChinese ? "总样本：" : "Total samples: ") + "</strong>" + _total.ToString(CultureInfo.InvariantCulture) + "</div>");
            sb.AppendLine("<div><strong>" + HtmlEncode(_isChinese ? "类别数：" : "Classes: ") + "</strong>" + _data.Count.ToString(CultureInfo.InvariantCulture) + "</div>");
            sb.AppendLine("<div><strong>" + HtmlEncode(_isChinese ? "导出时间：" : "Export time: ") + "</strong>" + HtmlEncode(exportTime) + "</div>");
            sb.AppendLine("</div>");

            sb.AppendLine("<h2>" + HtmlEncode(_isChinese ? "图表" : "Charts") + "</h2>");
            sb.AppendLine("<div class=\"chart\"><img alt=\"charts\" src=\"data:image/png;base64," + chartImage + "\" /></div>");

            sb.AppendLine("<h2>" + HtmlEncode(_isChinese ? "统计明细" : "Statistics") + "</h2>");
            sb.AppendLine("<table>");
            sb.AppendLine("<thead><tr><th>" + HtmlEncode(_isChinese ? "序号" : "#") + "</th><th>" + HtmlEncode(_isChinese ? "标签" : "Label") + "</th><th>" + HtmlEncode(_isChinese ? "数量" : "Count") + "</th><th>" + HtmlEncode(_isChinese ? "占比" : "Percent") + "</th></tr></thead>");
            sb.AppendLine("<tbody>");
            int rank = 1;
            foreach (KeyValuePair<string, int> kv in _data)
            {
                double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                sb.AppendLine("<tr><td>" + rank.ToString(CultureInfo.InvariantCulture) + "</td><td>" + HtmlEncode(kv.Key) + "</td><td>" + kv.Value.ToString(CultureInfo.InvariantCulture) + "</td><td>" + pct.ToString("0.00", CultureInfo.InvariantCulture) + "%</td></tr>");
                rank++;
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            if (!string.IsNullOrWhiteSpace(_diagnostic))
            {
                sb.AppendLine("<h2>" + HtmlEncode(_isChinese ? "诊断信息" : "Diagnostic") + "</h2>");
                sb.AppendLine("<pre>" + HtmlEncode(_diagnostic) + "</pre>");
            }

            sb.AppendLine("<h2>" + HtmlEncode(_isChinese ? "AI 分析" : "AI Analysis") + "</h2>");
            string aiText = string.IsNullOrWhiteSpace(richTextBoxAi.Text)
                ? (_isChinese ? "暂无 AI 分析内容。" : "No AI analysis content.")
                : richTextBoxAi.Text;
            sb.AppendLine("<pre>" + HtmlEncode(aiText) + "</pre>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

        private string CaptureChartImageBase64()
        {
            int width = Math.Max(640, chartPanel.Width);
            int height = Math.Max(320, chartPanel.Height);
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    g.Clear(Color.White);
                    Rectangle bounds = new Rectangle(0, 0, width, height);
                    bounds.Inflate(-18, -16);
                    int gap = 26;
                    int halfWidth = (bounds.Width - gap) / 2;
                    Rectangle barBounds = new Rectangle(bounds.Left, bounds.Top, halfWidth, bounds.Height);
                    Rectangle pieBounds = new Rectangle(barBounds.Right + gap, bounds.Top, bounds.Right - barBounds.Right - gap, bounds.Height);
                    DrawExportBarChart(g, barBounds);
                    DrawExportPieChart(g, pieBounds);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private void ExportPdf(string fileName)
        {
            List<Bitmap> pages = BuildPdfPages();
            try
            {
                WritePdfWithImagePages(fileName, pages);
            }
            finally
            {
                foreach (Bitmap page in pages)
                {
                    page.Dispose();
                }
            }
        }

        private List<Bitmap> BuildPdfPages()
        {
            List<Bitmap> pages = new List<Bitmap>();
            Bitmap firstPage = new Bitmap(PdfPagePixelWidth, PdfPagePixelHeight);
            pages.Add(firstPage);
            using (Graphics g = Graphics.FromImage(firstPage))
            using (Font titleFont = new Font("Microsoft YaHei UI", 18f, FontStyle.Bold))
            using (Font headingFont = new Font("Microsoft YaHei UI", 12f, FontStyle.Bold))
            using (Font normalFont = new Font("Microsoft YaHei UI", 9.5f))
            using (Font smallFont = new Font("Microsoft YaHei UI", 8.5f))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(35, 35, 35)))
            using (SolidBrush dimBrush = new SolidBrush(Color.FromArgb(80, 80, 80)))
            using (Pen linePen = new Pen(Color.FromArgb(210, 210, 210)))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.Clear(Color.White);

                int x = 42;
                int y = 36;
                int width = PdfPagePixelWidth - x * 2;
                string exportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                g.DrawString(_isChinese ? "结果统计报告" : "Result Statistics Report", titleFont, textBrush, x, y);
                y += 44;
                g.DrawString((_isChinese ? "流程：" : "Procedure: ") + (string.IsNullOrEmpty(_procedureName) ? "-" : _procedureName), normalFont, textBrush, x, y);
                y += 24;
                g.DrawString((_isChinese ? "总样本：" : "Total samples: ") + _total.ToString(CultureInfo.InvariantCulture), normalFont, textBrush, x, y);
                y += 24;
                g.DrawString((_isChinese ? "类别数：" : "Classes: ") + _data.Count.ToString(CultureInfo.InvariantCulture), normalFont, textBrush, x, y);
                y += 24;
                g.DrawString((_isChinese ? "导出时间：" : "Export time: ") + exportTime, normalFont, textBrush, x, y);
                y += 42;

                g.DrawString(_isChinese ? "图表" : "Charts", headingFont, textBrush, x, y);
                y += 26;
                Rectangle chartBounds = new Rectangle(x, y, width, 250);
                g.FillRectangle(Brushes.White, chartBounds);
                int gap = 28;
                int halfWidth = (chartBounds.Width - gap) / 2;
                DrawExportBarChart(g, new Rectangle(chartBounds.Left, chartBounds.Top, halfWidth, chartBounds.Height));
                DrawExportPieChart(g, new Rectangle(chartBounds.Left + halfWidth + gap, chartBounds.Top, chartBounds.Width - halfWidth - gap, chartBounds.Height));
                y += chartBounds.Height + 34;

                g.DrawString(_isChinese ? "统计明细" : "Statistics", headingFont, textBrush, x, y);
                y += 28;
                y = DrawPdfTable(g, x, y, width, normalFont, smallFont, textBrush, dimBrush, linePen);
                y += 28;

                g.DrawString(_isChinese ? "AI 分析" : "AI Analysis", headingFont, textBrush, x, y);
                y += 26;
                string aiText = string.IsNullOrWhiteSpace(richTextBoxAi.Text)
                    ? (_isChinese ? "暂无 AI 分析内容。" : "No AI analysis content.")
                    : richTextBoxAi.Text;
                DrawWrappedText(g, aiText, normalFont, textBrush, new RectangleF(x, y, width, PdfPagePixelHeight - y - 42));

                g.DrawString("1", smallFont, dimBrush, PdfPagePixelWidth / 2 - 4, PdfPagePixelHeight - 30);
            }

            return pages;
        }

        private int DrawPdfTable(Graphics g, int x, int y, int width, Font normalFont, Font smallFont, Brush textBrush, Brush dimBrush, Pen linePen)
        {
            int rowHeight = 28;
            int[] colWidths = { 58, width - 58 - 120 - 120, 120, 120 };
            string[] headers = _isChinese
                ? new[] { "序号", "标签", "数量", "占比" }
                : new[] { "#", "Label", "Count", "Percent" };
            using (SolidBrush headerBrush = new SolidBrush(Color.FromArgb(244, 246, 248)))
            {
                g.FillRectangle(headerBrush, x, y, width, rowHeight);
            }
            DrawPdfTableRow(g, x, y, colWidths, rowHeight, headers, smallFont, textBrush, linePen);
            y += rowHeight;

            int rank = 1;
            foreach (KeyValuePair<string, int> kv in _data)
            {
                double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                string[] cells =
                {
                    rank.ToString(CultureInfo.InvariantCulture),
                    kv.Key,
                    kv.Value.ToString(CultureInfo.InvariantCulture),
                    pct.ToString("0.00", CultureInfo.InvariantCulture) + "%"
                };
                DrawPdfTableRow(g, x, y, colWidths, rowHeight, cells, smallFont, dimBrush, linePen);
                y += rowHeight;
                rank++;
            }
            return y;
        }

        private void DrawPdfTableRow(Graphics g, int x, int y, int[] colWidths, int rowHeight, string[] cells, Font font, Brush brush, Pen pen)
        {
            int currentX = x;
            for (int i = 0; i < colWidths.Length; i++)
            {
                Rectangle rect = new Rectangle(currentX, y, colWidths[i], rowHeight);
                g.DrawRectangle(pen, rect);
                using (StringFormat fmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter })
                {
                    RectangleF textRect = new RectangleF(rect.Left + 6, rect.Top, rect.Width - 12, rect.Height);
                    g.DrawString(cells[i], font, brush, textRect, fmt);
                }
                currentX += colWidths[i];
            }
        }

        private void DrawWrappedText(Graphics g, string text, Font font, Brush brush, RectangleF bounds)
        {
            using (StringFormat fmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near, Trimming = StringTrimming.Word })
            {
                g.DrawString(text ?? string.Empty, font, brush, bounds, fmt);
            }
        }

        private void WritePdfWithImagePages(string fileName, List<Bitmap> pages)
        {
            List<byte[]> images = new List<byte[]>();
            foreach (Bitmap page in pages)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    page.Save(ms, ImageFormat.Jpeg);
                    images.Add(ms.ToArray());
                }
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(Encoding.ASCII.GetBytes("%PDF-1.4\n"));
                List<long> offsets = new List<long>();
                Action<string> writeAscii = s => writer.Write(Encoding.ASCII.GetBytes(s));
                Action<int, string> writeObject = (number, body) =>
                {
                    offsets.Add(fs.Position);
                    writeAscii(number.ToString(CultureInfo.InvariantCulture) + " 0 obj\n");
                    writeAscii(body);
                    writeAscii("\nendobj\n");
                };

                int pageCount = pages.Count;
                int catalogObj = 1;
                int pagesObj = 2;
                int firstPageObj = 3;
                int firstContentObj = firstPageObj + pageCount;
                int firstImageObj = firstContentObj + pageCount;
                string kids = string.Empty;
                for (int i = 0; i < pageCount; i++)
                {
                    kids += (firstPageObj + i).ToString(CultureInfo.InvariantCulture) + " 0 R ";
                }

                writeObject(catalogObj, "<< /Type /Catalog /Pages " + pagesObj.ToString(CultureInfo.InvariantCulture) + " 0 R >>");
                writeObject(pagesObj, "<< /Type /Pages /Kids [" + kids + "] /Count " + pageCount.ToString(CultureInfo.InvariantCulture) + " >>");

                for (int i = 0; i < pageCount; i++)
                {
                    int pageObj = firstPageObj + i;
                    int contentObj = firstContentObj + i;
                    int imageObj = firstImageObj + i;
                    writeObject(pageObj,
                        "<< /Type /Page /Parent " + pagesObj.ToString(CultureInfo.InvariantCulture) + " 0 R /MediaBox [0 0 "
                        + PdfPagePointWidth.ToString(CultureInfo.InvariantCulture) + " " + PdfPagePointHeight.ToString(CultureInfo.InvariantCulture)
                        + "] /Resources << /XObject << /Im0 " + imageObj.ToString(CultureInfo.InvariantCulture)
                        + " 0 R >> >> /Contents " + contentObj.ToString(CultureInfo.InvariantCulture) + " 0 R >>");
                }

                for (int i = 0; i < pageCount; i++)
                {
                    string content = "q\n" + PdfPagePointWidth.ToString(CultureInfo.InvariantCulture) + " 0 0 "
                        + PdfPagePointHeight.ToString(CultureInfo.InvariantCulture) + " 0 0 cm\n/Im0 Do\nQ\n";
                    writeObject(firstContentObj + i, "<< /Length " + Encoding.ASCII.GetByteCount(content).ToString(CultureInfo.InvariantCulture) + " >>\nstream\n" + content + "endstream");
                }

                for (int i = 0; i < pageCount; i++)
                {
                    offsets.Add(fs.Position);
                    writeAscii((firstImageObj + i).ToString(CultureInfo.InvariantCulture) + " 0 obj\n");
                    writeAscii("<< /Type /XObject /Subtype /Image /Width " + PdfPagePixelWidth.ToString(CultureInfo.InvariantCulture)
                        + " /Height " + PdfPagePixelHeight.ToString(CultureInfo.InvariantCulture)
                        + " /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length "
                        + images[i].Length.ToString(CultureInfo.InvariantCulture) + " >>\nstream\n");
                    writer.Write(images[i]);
                    writeAscii("\nendstream\nendobj\n");
                }

                long xref = fs.Position;
                int objectCount = firstImageObj + pageCount - 1;
                writeAscii("xref\n0 " + (objectCount + 1).ToString(CultureInfo.InvariantCulture) + "\n");
                writeAscii("0000000000 65535 f \n");
                foreach (long offset in offsets)
                {
                    writeAscii(offset.ToString("0000000000", CultureInfo.InvariantCulture) + " 00000 n \n");
                }
                writeAscii("trailer\n<< /Size " + (objectCount + 1).ToString(CultureInfo.InvariantCulture) + " /Root 1 0 R >>\n");
                writeAscii("startxref\n" + xref.ToString(CultureInfo.InvariantCulture) + "\n%%EOF");
            }
        }

        private void DrawExportBarChart(Graphics g, Rectangle bounds)
        {
            int padTop = 34;
            int padBottom = 12;
            int padLeft = 6;
            int padRight = 6;
            int labelWidth = Math.Min(110, Math.Max(68, bounds.Width / 4));
            int countWidth = 86;
            int barAreaLeft = bounds.Left + padLeft + labelWidth + 8;
            int barAreaRight = bounds.Right - padRight - countWidth - 8;
            int barAreaWidth = Math.Max(20, barAreaRight - barAreaLeft);
            int rowCount = _data.Count;
            int rowHeight = Math.Max(32, (bounds.Height - padTop - padBottom) / Math.Max(1, rowCount));
            int yOffset = bounds.Top + padTop;
            int maxCount = 0;
            foreach (KeyValuePair<string, int> kv in _data) if (kv.Value > maxCount) maxCount = kv.Value;
            if (maxCount == 0) maxCount = 1;

            using (Font f = new Font("Microsoft YaHei UI", 9f))
            using (Font fbold = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(35, 35, 35)))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(45, 45, 45)))
            using (SolidBrush dimBrush = new SolidBrush(Color.FromArgb(95, 95, 95)))
            using (SolidBrush trackBrush = new SolidBrush(Color.FromArgb(238, 242, 246)))
            using (StringFormat leftFmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            using (StringFormat rightFmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(_isChinese ? "柱状图" : "Bar Chart", fbold, titleBrush, new RectangleF(bounds.Left, bounds.Top, bounds.Width, 24), leftFmt);
                for (int i = 0; i < rowCount; i++)
                {
                    KeyValuePair<string, int> kv = _data[i];
                    int y = yOffset + i * rowHeight;
                    int barH = Math.Min(16, Math.Max(8, rowHeight / 4));
                    int barY = y + (rowHeight - barH) / 2;
                    int barLen = (int)Math.Round(barAreaWidth * (kv.Value / (double)maxCount));
                    if (barLen < 2) barLen = 2;
                    Color color = _palette[i % _palette.Length];

                    using (SolidBrush barBrush = new SolidBrush(color))
                    {
                        g.FillRectangle(trackBrush, new Rectangle(barAreaLeft, barY, barAreaWidth, barH));
                        g.FillRectangle(barBrush, new Rectangle(barAreaLeft, barY, barLen, barH));
                    }

                    g.DrawString(kv.Key, f, textBrush, new RectangleF(bounds.Left + padLeft, y, labelWidth, rowHeight), leftFmt);
                    double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                    string countText = kv.Value.ToString(CultureInfo.InvariantCulture)
                        + " (" + pct.ToString("0.0", CultureInfo.InvariantCulture) + "%)";
                    g.DrawString(countText, f, dimBrush, new RectangleF(barAreaRight + 8, y, countWidth, rowHeight), rightFmt);
                }
            }
        }

        private void DrawExportPieChart(Graphics g, Rectangle bounds)
        {
            using (Font f = new Font("Microsoft YaHei UI", 9f))
            using (Font fbold = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(35, 35, 35)))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(45, 45, 45)))
            using (Pen borderPen = new Pen(Color.White, 2f))
            using (StringFormat centerFmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(_isChinese ? "扇形图" : "Pie Chart", fbold, titleBrush, new RectangleF(bounds.Left, bounds.Top, bounds.Width, 24), centerFmt);
                int legendHeight = Math.Min(84, Math.Max(44, _data.Count * 22));
                int diameter = Math.Min(bounds.Width - 34, bounds.Height - 48 - legendHeight);
                diameter = Math.Max(64, diameter);
                Rectangle pieRect = new Rectangle(
                    bounds.Left + (bounds.Width - diameter) / 2,
                    bounds.Top + 36,
                    diameter,
                    diameter);

                float startAngle = -90f;
                for (int i = 0; i < _data.Count; i++)
                {
                    KeyValuePair<string, int> kv = _data[i];
                    float sweepAngle = _total == 0 ? 0f : 360f * kv.Value / _total;
                    Color color = _palette[i % _palette.Length];
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        g.FillPie(brush, pieRect, startAngle, sweepAngle);
                    }
                    g.DrawPie(borderPen, pieRect, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }

                int legendTop = Math.Min(bounds.Bottom - legendHeight + 4, pieRect.Bottom + 14);
                int legendLeft = bounds.Left + Math.Max(8, (bounds.Width - 180) / 2);
                for (int i = 0; i < _data.Count; i++)
                {
                    KeyValuePair<string, int> kv = _data[i];
                    double pct = _total == 0 ? 0 : 100.0 * kv.Value / _total;
                    int y = legendTop + i * 22;
                    Color color = _palette[i % _palette.Length];
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        g.FillRectangle(brush, legendLeft, y + 5, 10, 10);
                    }
                    string text = string.Format(CultureInfo.InvariantCulture, "{0}  {1:0.0}%", kv.Key, pct);
                    g.DrawString(text, f, textBrush, legendLeft + 18, y);
                }
            }
        }

        private static string HtmlEncode(string value)
        {
            return WebUtility.HtmlEncode(value ?? string.Empty);
        }

        private static string SafeFileNamePart(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Procedure";
            string result = value;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                result = result.Replace(c, '_');
            }
            return result;
        }

        private void AppendAi(string text)
        {
            if (text == null) return;
            try
            {
                if (richTextBoxAi.IsDisposed) return;
                if (richTextBoxAi.InvokeRequired)
                {
                    richTextBoxAi.BeginInvoke(new Action<string>(AppendAi), text);
                    return;
                }
                richTextBoxAi.AppendText(text);
                richTextBoxAi.SelectionStart = richTextBoxAi.TextLength;
                richTextBoxAi.ScrollToCaret();
            }
            catch { /* control disposed */ }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBoxAi.TextLength > 0)
                {
                    Clipboard.SetText(richTextBoxAi.Text);
                }
            }
            catch { /* clipboard busy */ }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = _isChinese ? "导出结果统计" : "Export Result Statistics";
                    dlg.Filter = _isChinese ? "PDF 文档 (*.pdf)|*.pdf|所有文件 (*.*)|*.*" : "PDF Document (*.pdf)|*.pdf|All Files (*.*)|*.*";
                    dlg.FileName = "ResultStatistics_" + SafeFileNamePart(_procedureName) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + ".pdf";
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    chartPanel.Refresh();
                    ExportPdf(dlg.FileName);
                    MessageBox.Show(this,
                        _isChinese ? "结果已导出。" : "Result statistics exported.",
                        _isChinese ? "结果导出" : "Export",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    (_isChinese ? "导出失败：" : "Export failed: ") + ex.Message,
                    _isChinese ? "结果导出" : "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxAi.Clear();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try { if (_cts != null) _cts.Cancel(); } catch { }
            base.OnFormClosing(e);
        }
    }
}
