using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VM.Core;
using VM.PlatformSDKCS;

namespace ChatDemoCs
{
    /// <summary>
    /// Tallies per-label counts collected over a continuous-run window of a
    /// VisionMaster procedure. Writes happen on VM SDK callback threads while
    /// reads happen on the WinForms UI thread, so all access is guarded by an
    /// internal lock.
    /// </summary>
    public class ResultStatistics
    {
        private readonly object _gate = new object();
        private readonly Dictionary<string, int> _counts = new Dictionary<string, int>(StringComparer.Ordinal);

        public DateTime StartTime { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public int TotalSamples { get; private set; }
        public string ProcedureName { get; private set; }
        public string LastDiagnostic { get; private set; }

        public bool HasData
        {
            get { lock (_gate) { return TotalSamples > 0; } }
        }

        public int ClassCount
        {
            get { lock (_gate) { return _counts.Count; } }
        }

        public void Reset(string procedureName)
        {
            lock (_gate)
            {
                _counts.Clear();
                StartTime = DateTime.Now;
                LastUpdate = StartTime;
                TotalSamples = 0;
                ProcedureName = procedureName ?? string.Empty;
                LastDiagnostic = string.Empty;
            }
        }

        public void SetDiagnostic(string diagnostic)
        {
            lock (_gate)
            {
                LastDiagnostic = diagnostic ?? string.Empty;
            }
        }

        public void Add(string label)
        {
            string key = string.IsNullOrEmpty(label) ? "(empty)" : label;
            lock (_gate)
            {
                int existing;
                if (_counts.TryGetValue(key, out existing))
                {
                    _counts[key] = existing + 1;
                }
                else
                {
                    _counts.Add(key, 1);
                }
                TotalSamples++;
                LastUpdate = DateTime.Now;
            }
        }

        public void AddRange(IEnumerable<string> labels)
        {
            if (labels == null) return;
            foreach (string label in labels)
            {
                Add(label);
            }
        }

        public void UseDemoResultData(string procedureName)
        {
            lock (_gate)
            {
                _counts.Clear();
                _counts["单个"] = 20;
                _counts["叠放"] = 20;
                TotalSamples = 40;
                StartTime = DateTime.Now;
                LastUpdate = StartTime;
                ProcedureName = procedureName ?? ProcedureName ?? string.Empty;
                LastDiagnostic = string.Empty;
            }
        }

        /// <summary>
        /// Returns a snapshot of (label, count) entries sorted by count descending,
        /// then by label ascending for deterministic ordering.
        /// </summary>
        public List<KeyValuePair<string, int>> SnapshotSorted()
        {
            lock (_gate)
            {
                var copy = new List<KeyValuePair<string, int>>(_counts.Count);
                foreach (var kv in _counts) copy.Add(kv);
                copy.Sort((a, b) =>
                {
                    int c = b.Value.CompareTo(a.Value);
                    if (c != 0) return c;
                    return string.Compare(a.Key, b.Key, StringComparison.Ordinal);
                });
                return copy;
            }
        }

        /// <summary>
        /// Tries to extract a representative classification label from a procedure's
        /// dynamic outputs. Search order:
        ///   1. string-typed output named exactly "out"
        ///   2. string-typed output whose name contains a well-known keyword
        ///      (label/class/name/result/ocr/text)
        ///   3. the first string-typed output of any name
        /// Returns null when the procedure exposes no usable string output.
        /// </summary>
        public static string TryExtractLabel(VmProcedure proc)
        {
            List<string> labels = TryExtractLabels(proc);
            return labels == null || labels.Count == 0 ? null : labels[0];
        }

        public static List<string> TryExtractLabels(VmProcedure proc)
        {
            List<string> labels = new List<string>();
            if (proc == null) return labels;
            try
            {
                List<VmDynamicIODefine.IoNameInfo> ioNameInfos = proc.ModuResult.GetAllOutputNameInfo();
                if (ioNameInfos != null && ioNameInfos.Count > 0)
                {
                    foreach (VmDynamicIODefine.IoNameInfo info in ioNameInfos)
                    {
                        if (info.TypeName != IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_CLASSINFO) continue;
                        AddClassInfoLabels(proc, info.Name, labels);
                    }
                    if (labels.Count > 0) return labels;

                    string[] preferredKeywords = { "label", "class", "category", "branch", "标签", "类别", "分类", "分支" };
                    foreach (string keyword in preferredKeywords)
                    {
                        foreach (VmDynamicIODefine.IoNameInfo info in ioNameInfos)
                        {
                            if (info.Name == null) continue;
                            if (info.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                AddOutputLabels(proc, info, labels);
                                if (labels.Count > 0) return labels;
                            }
                        }
                    }

                }

                AddProcedureOutputTreeLabels(proc, labels);
                if (labels.Count > 0) return labels;
                AddFrontOutputLabels(proc, labels);
                return labels;
            }
            catch
            {
                return labels;
            }
        }

        public static string BuildDiagnostic(VmProcedure proc)
        {
            StringBuilder sb = new StringBuilder();
            if (proc == null)
            {
                return "Procedure is null.";
            }
            try
            {
                List<VmDynamicIODefine.IoNameInfo> ioNameInfos = proc.ModuResult.GetAllOutputNameInfo();
                sb.AppendLine("ModuResult outputs:");
                if (ioNameInfos == null || ioNameInfos.Count == 0)
                {
                    sb.AppendLine("  <none>");
                }
                else
                {
                    foreach (VmDynamicIODefine.IoNameInfo info in ioNameInfos)
                    {
                        sb.AppendLine("  " + info.Name + " : " + info.TypeName);
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("ModuResult outputs failed: " + ex.Message);
            }
            try
            {
                MethodInfo method = proc.GetType().GetMethod("GetFrontOutputItems", new[] { typeof(ArrayList) });
                sb.AppendLine("Procedure Outputs:");
                AppendCollectionDiagnostic(sb, proc.Outputs, "  ", 0);
                sb.AppendLine("Procedure Modules:");
                AppendModuleDiagnostic(sb, proc.Modules, "  ");
                sb.AppendLine("Front output items:");
                if (method == null)
                {
                    sb.AppendLine("  GetFrontOutputItems not found.");
                }
                else
                {
                    ArrayList items = new ArrayList();
                    method.Invoke(proc, new object[] { items });
                    sb.AppendLine("  Count=" + items.Count);
                    int count = 0;
                    foreach (object item in items)
                    {
                        if (count++ >= 20) break;
                        AppendIoDiagnostic(sb, item, "  ", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("Front output items failed: " + ex.Message);
            }
            return sb.ToString();
        }

        private static void AppendIoDiagnostic(StringBuilder sb, object io, string indent, int depth)
        {
            if (io == null || depth > 4) return;
            sb.AppendLine(indent + DescribeObject(io));
            object children = TryGetMemberValue(io, "Children") ?? TryGetMemberValue(io, "Childs");
            int count = 0;
            foreach (object child in EnumerateChildren(children))
            {
                if (count++ >= 30)
                {
                    sb.AppendLine(indent + "  ...");
                    return;
                }
                AppendIoDiagnostic(sb, child, indent + "  ", depth + 1);
            }
        }

        private static void AppendCollectionDiagnostic(StringBuilder sb, object collection, string indent, int depth)
        {
            if (collection == null || depth > 4)
            {
                sb.AppendLine(indent + "<none>");
                return;
            }
            int count = 0;
            foreach (object item in EnumerateChildren(collection))
            {
                if (count++ >= 30)
                {
                    sb.AppendLine(indent + "...");
                    return;
                }
                AppendIoDiagnostic(sb, item, indent, depth);
            }
            if (count == 0) sb.AppendLine(indent + "<empty>");
        }

        private static void AppendModuleDiagnostic(StringBuilder sb, object modules, string indent)
        {
            if (modules == null)
            {
                sb.AppendLine(indent + "<none>");
                return;
            }
            int count = 0;
            foreach (object module in EnumerateChildren(modules))
            {
                if (count++ >= 30)
                {
                    sb.AppendLine(indent + "...");
                    return;
                }
                sb.AppendLine(indent + DescribeObject(module));
                object outputs = TryGetMemberValue(module, "Outputs");
                AppendCollectionDiagnostic(sb, outputs, indent + "  ", 0);
            }
            if (count == 0) sb.AppendLine(indent + "<empty>");
        }

        private static void AddClassInfoLabels(VmProcedure proc, string name, List<string> labels)
        {
            try
            {
                var classInfos = proc.ModuResult.GetOutputClassInfoArray(name);
                if (classInfos == null) return;
                foreach (var classInfo in classInfos)
                {
                    if (classInfo == null) continue;
                    string label = classInfo.Name;
                    if (string.IsNullOrWhiteSpace(label))
                    {
                        label = "GrayValue=" + classInfo.GrayValue.ToString();
                    }
                    labels.Add(label.Trim());
                }
            }
            catch
            {
            }
        }

        private static void AddStringLabels(string value, List<string> labels)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            string[] parts = value.Split(new[] { '\r', '\n', '\t', ',', ';', '，', '；', '|', '、' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 1)
            {
                labels.Add(value.Trim());
                return;
            }
            foreach (string part in parts)
            {
                string label = part == null ? string.Empty : part.Trim();
                if (label.Length > 0) labels.Add(label);
            }
        }

        private static void AddOutputLabels(VmProcedure proc, VmDynamicIODefine.IoNameInfo info, List<string> labels)
        {
            if (proc == null || info.Name == null) return;
            if (info.TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
            {
                AddStringLabels(ReadStringOutput(proc, info.Name), labels);
                return;
            }
            if (info.TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_CLASSINFO)
            {
                AddClassInfoLabels(proc, info.Name, labels);
                return;
            }
        }

        private static void AddTextLikeOutputLabels(VmProcedure proc, VmDynamicIODefine.IoNameInfo info, List<string> labels)
        {
            AddOutputLabels(proc, info, labels);
        }

        private static void AddReflectedOutputValues(VmProcedure proc, string methodName, string outputName, List<string> labels)
        {
            try
            {
                MethodInfo method = proc.ModuResult.GetType().GetMethod(methodName, new[] { typeof(string) });
                if (method == null) return;
                object value = method.Invoke(proc.ModuResult, new object[] { outputName });
                AddObjectValues(outputName, value, labels, 0);
            }
            catch
            {
            }
        }

        private static void AddFrontOutputLabels(VmProcedure proc, List<string> labels)
        {
            try
            {
                MethodInfo method = proc.GetType().GetMethod("GetFrontOutputItems", new[] { typeof(ArrayList) });
                if (method == null) return;
                ArrayList items = new ArrayList();
                method.Invoke(proc, new object[] { items });
                foreach (object item in items)
                {
                    AddFrontOutputIoLabels(proc, item, labels, 0);
                    if (labels.Count > 0) return;
                }
            }
            catch
            {
            }
        }

        private static void AddProcedureOutputTreeLabels(VmProcedure proc, List<string> labels)
        {
            if (proc == null || labels == null || labels.Count > 0) return;
            AddVmIOCollectionLabels(proc, proc.Outputs, labels, 0);
            if (labels.Count > 0) return;
            foreach (object module in EnumerateChildren(proc.Modules))
            {
                object outputs = TryGetMemberValue(module, "Outputs");
                AddVmIOCollectionLabels(proc, outputs, labels, 0);
                if (labels.Count > 0) return;
            }
        }

        private static void AddVmIOCollectionLabels(VmProcedure proc, object collection, List<string> labels, int depth)
        {
            if (collection == null || labels == null || labels.Count > 0 || depth > 10) return;
            foreach (object item in EnumerateChildren(collection))
            {
                AddFrontOutputIoLabels(proc, item, labels, depth + 1);
                if (labels.Count > 0) return;
            }
        }

        private static void AddFrontOutputIoLabels(VmProcedure proc, object io, List<string> labels, int depth)
        {
            if (proc == null || io == null || labels == null || labels.Count > 0 || depth > 10) return;

            string[] candidateNames =
            {
                TryGetStringMember(io, "UniqueName"),
                TryGetStringMember(io, "Name"),
                TryGetStringMember(io, "CustomName"),
                TryGetStringMember(io, "ModulePathName"),
                TryGetStringMember(io, "AutoSubscribeName"),
                TryGetStringMember(io, "SubscriptionInfo")
            };
            bool allowString = IsLabelLikeIo(io, candidateNames);

            foreach (string name in candidateNames)
            {
                if (string.IsNullOrWhiteSpace(name)) continue;
                AddVmIOValue(proc, name, labels, allowString);
                if (labels.Count > 0) return;
            }

            if (allowString || IsClassInfoLike(TryGetMemberValue(io, "Value")))
            {
                object value = TryGetMemberValue(io, "Value");
                AddObjectValues(null, value, labels, depth + 1);
                if (labels.Count > 0) return;
            }

            object children = TryGetMemberValue(io, "Children") ?? TryGetMemberValue(io, "Childs");
            foreach (object child in EnumerateChildren(children))
            {
                AddFrontOutputIoLabels(proc, child, labels, depth + 1);
                if (labels.Count > 0) return;
            }
        }

        private static void AddVmIOValue(VmProcedure proc, string ioName, List<string> labels, bool allowString)
        {
            AddVmIOValuesByMethod(proc, "GetVmIOClassInfoValue", ioName, labels);
            if (labels.Count > 0) return;
            if (allowString)
            {
                AddVmIOValuesByMethod(proc, "GetVmIOStringValue", ioName, labels);
            }
        }

        private static void AddVmIOValuesByMethod(VmProcedure proc, string methodName, string ioName, List<string> labels)
        {
            try
            {
                MethodInfo method = proc.GetType().GetMethod(methodName, new[] { typeof(string) });
                if (method == null) return;
                object value = method.Invoke(proc, new object[] { ioName });
                AddObjectValues(ioName, value, labels, 0);
            }
            catch
            {
            }
        }

        private static IEnumerable<object> EnumerateChildren(object children)
        {
            if (children == null) yield break;
            IEnumerable enumerable = children as IEnumerable;
            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    yield return item;
                }
                yield break;
            }
            object countObject = TryGetMemberValue(children, "Count");
            int count;
            if (countObject != null && int.TryParse(countObject.ToString(), out count))
            {
                PropertyInfo itemProperty = children.GetType().GetProperty("Item", new[] { typeof(int) });
                if (itemProperty == null) itemProperty = children.GetType().GetProperty("Item", new[] { typeof(string) });
                for (int i = 0; i < count && i < 100; i++)
                {
                    object item = null;
                    try
                    {
                        if (itemProperty != null) item = itemProperty.GetValue(children, new object[] { i });
                    }
                    catch
                    {
                    }
                    if (item != null) yield return item;
                }
            }
        }

        private static void AddObjectValues(string prefix, object value, List<string> labels, int depth)
        {
            if (value == null || labels == null || depth > 4 || labels.Count > 0) return;
            string text = value as string;
            if (text != null)
            {
                AddStringLabels(text, labels);
                return;
            }
            Type type = value.GetType();
            if (type.IsPrimitive || value is decimal)
            {
                labels.Add(string.IsNullOrEmpty(prefix) ? value.ToString() : prefix + "=" + value.ToString());
                return;
            }
            string direct = TryGetStringMember(value, "ClassName");
            if (!string.IsNullOrWhiteSpace(direct))
            {
                labels.Add(direct.Trim());
                return;
            }
            direct = TryGetStringMember(value, "Name");
            if (!string.IsNullOrWhiteSpace(direct) && IsClassInfoLike(value))
            {
                labels.Add(direct.Trim());
                return;
            }
            direct = TryGetStringMember(value, "strValue");
            if (!string.IsNullOrWhiteSpace(direct))
            {
                AddStringLabels(direct, labels);
                return;
            }

            object memberValue = TryGetMemberValue(value, "Value");
            if (memberValue != null && !object.ReferenceEquals(memberValue, value))
            {
                AddObjectValues(prefix, memberValue, labels, depth + 1);
                if (labels.Count > 0) return;
            }
            memberValue = TryGetMemberValue(value, "Children");
            if (memberValue != null && !object.ReferenceEquals(memberValue, value))
            {
                AddObjectValues(prefix, memberValue, labels, depth + 1);
                if (labels.Count > 0) return;
            }
            memberValue = TryGetMemberValue(value, "pIntVal");
            if (memberValue != null)
            {
                AddObjectValues(prefix, memberValue, labels, depth + 1);
                if (labels.Count > 0) return;
            }
            memberValue = TryGetMemberValue(value, "pFloatVal");
            if (memberValue != null)
            {
                AddObjectValues(prefix, memberValue, labels, depth + 1);
                if (labels.Count > 0) return;
            }
            memberValue = TryGetMemberValue(value, "astStringVal");
            if (memberValue != null)
            {
                AddObjectValues(prefix, memberValue, labels, depth + 1);
                if (labels.Count > 0) return;
            }

            IEnumerable enumerable = value as IEnumerable;
            if (enumerable != null)
            {
                int count = 0;
                foreach (object item in enumerable)
                {
                    AddObjectValues(prefix, item, labels, depth + 1);
                    if (labels.Count > 0 || ++count >= 20) return;
                }
            }

            if (!string.IsNullOrWhiteSpace(direct))
            {
                labels.Add(direct.Trim());
            }
        }

        private static bool IsClassInfoLike(object value)
        {
            if (value == null) return false;
            Type type = value.GetType();
            return type.Name.IndexOf("ClassInfo", StringComparison.OrdinalIgnoreCase) >= 0
                || TryGetMemberValue(value, "GrayValue") != null;
        }

        private static bool IsLabelLikeIo(object io, IEnumerable<string> candidateNames)
        {
            bool hasBranch = false;
            foreach (string name in candidateNames)
            {
                if (ContainsAny(name, "branch", "分支"))
                {
                    hasBranch = true;
                    continue;
                }
                if (IsLabelLikeName(name)) return true;
            }
            string typeName = TryGetStringMember(io, "TypeName");
            return hasBranch || IsLabelLikeName(typeName);
        }

        private static bool IsLabelLikeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            string[] include = { "label", "class", "category", "branchmatch", "标签", "类别", "分类", "分支" };
            string[] exclude = { "ocr", "text", "字符", "文本", "条码", "二维码", "code", "barcode", "path", "image", "图像", "路径", "文件", "model", "模板" };
            foreach (string word in exclude)
            {
                if (name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) return false;
            }
            foreach (string word in include)
            {
                if (name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }
            return false;
        }

        private static bool ContainsAny(string value, params string[] words)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            foreach (string word in words)
            {
                if (value.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }
            return false;
        }

        private static string DescribeObject(object value)
        {
            if (value == null) return "<null>";
            StringBuilder sb = new StringBuilder();
            sb.Append(value.GetType().Name);
            AppendMemberDescription(sb, value, "Name");
            AppendMemberDescription(sb, value, "ClassName");
            AppendMemberDescription(sb, value, "UniqueName");
            AppendMemberDescription(sb, value, "CustomName");
            AppendMemberDescription(sb, value, "TypeName");
            AppendMemberDescription(sb, value, "DataType");
            AppendMemberDescription(sb, value, "IsOutput");
            AppendMemberDescription(sb, value, "Value");
            AppendMemberDescription(sb, value, "strValue");
            return sb.ToString();
        }

        private static void AppendMemberDescription(StringBuilder sb, object value, string name)
        {
            object member = TryGetMemberValue(value, name);
            if (member == null) return;
            string text = member as string;
            if (text == null)
            {
                text = member.GetType().IsPrimitive || member is decimal ? member.ToString() : member.GetType().Name;
            }
            if (string.IsNullOrEmpty(text)) return;
            if (text.Length > 80) text = text.Substring(0, 80);
            sb.Append(" ");
            sb.Append(name);
            sb.Append("=");
            sb.Append(text);
        }

        private static string TryGetStringMember(object value, string name)
        {
            object member = TryGetMemberValue(value, name);
            return member as string;
        }

        private static object TryGetMemberValue(object value, string name)
        {
            try
            {
                if (value == null) return null;
                Type type = value.GetType();
                PropertyInfo property = type.GetProperty(name);
                if (property != null && property.GetIndexParameters().Length == 0) return property.GetValue(value, null);
                FieldInfo field = type.GetField(name);
                if (field != null) return field.GetValue(value);
            }
            catch
            {
            }
            return null;
        }

        private static string ReadStringOutput(VmProcedure proc, string name)
        {
            try
            {
                var arr = proc.ModuResult.GetOutputString(name);
                if (arr.astStringVal == null || arr.astStringVal.Length == 0) return null;
                return arr.astStringVal[0].strValue;
            }
            catch
            {
                return null;
            }
        }
    }
}
