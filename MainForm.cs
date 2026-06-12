using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;

namespace ChatDemoCs
{
    /// <summary>
    /// Application shell for ChatDemoCs.
    /// 
    /// This form mirrors the layout of the sibling demos (DeepLearningDemoCs / LocateDemoCs /
    /// OCRDemoCs): top-right toolbar buttons (Render / Config / 涓枃/鑻辨枃), a central
    /// <see cref="renderPanel"/> hosting either a <see cref="RenderControl"/> or a
    /// <see cref="MainViewControl"/>, and a right-hand sidebar with VisionMaster Solution +
    /// Procedure groupboxes. The bottom-right of the sidebar is occupied by an embedded
    /// <see cref="ChatPanel"/> (DeepSeek-style assistant) so a developer can ask the LLM
    /// for help while configuring or running the loaded .sol solution.
    /// </summary>
    public partial class MainForm : Form
    {
        private RenderControl renderControl;
        private MainViewControl mainViewControl;
        private ChatPanel chatPanel;

        public bool mSolutionIsLoad = false;
        public VmProcedure vmProcedure = null;
        public ProcessInfoList vmProcessInfoList = new ProcessInfoList();
        public string vmSolutionPath = null;
        private readonly string logPath = Application.StartupPath + "/Log/Message";
        private readonly Timer LoadSolutionIndicateTimer = new Timer();

        // Per-label statistics for the current continuous-run window. Reset whenever a
        // new continuous run starts; surfaced via the "Statistics" button below.
        private readonly ResultStatistics resultStats = new ResultStatistics();
        private volatile bool collectingStats = false;

        public MainForm()
        {
            InitializeComponent();

            renderControl = new RenderControl();
            mainViewControl = new MainViewControl();
            renderControl.Dock = DockStyle.Fill;
            mainViewControl.Dock = DockStyle.Fill;
            buttonRender.BackColor = Color.Orange;
            buttonConfig.BackColor = Color.Gray;
            renderPanel.Controls.Add(mainViewControl);

            chatPanel = new ChatPanel();
            chatPanel.Dock = DockStyle.Fill;
            chatPanel.LogMessage += LogFunction;
            groupBoxChat.Controls.Add(chatPanel);

            LoadSolutionIndicateTimer.Interval = 300;
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;

            VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;
            VmSolution.OnProcessStatusStartEvent += VmSolution_OnProcessStatusStartEvent;
            VmSolution.OnProcessStatusStopEvent += VmSolution_OnProcessStatusStopEvent;
        }

        // ---- VisionMaster callbacks ---------------------------------------------------

        private void VmSolution_OnProcessStatusStartEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_START_CONTINUOUSLY_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStatus == 0)
                {
                    buttonContiRun.Text = "Run Stop";
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;
                    buttonResultStats.Enabled = false;
                    resultStats.Reset(comboProcedure.Text);
                    collectingStats = true;
                    LogFunction("Start continuous run!");
                }
            }));
        }

        private void VmSolution_OnProcessStatusStopEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStopAction == 1)
                {
                    buttonContiRun.Text = "Run Continuous";
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;
                    collectingStats = false;
                    buttonResultStats.Enabled = true;
                    if (resultStats.HasData)
                    {
                        LogFunction(string.Format("End Run! Collected {0} samples across {1} class(es).",
                            resultStats.TotalSamples, resultStats.ClassCount));
                    }
                    else
                    {
                        LogFunction("End Run!");
                    }
                }
            }));
        }

        private void VmSolution_OnWorkStatusEvent(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            if (workStatusInfo.nWorkStatus != 0) return;
            try
            {
                if (vmProcessInfoList.nNum == 0) return;

                VmProcedure proc = ResolveProcedure(workStatusInfo.nProcessID);
                if (proc == null) return;

                List<VmDynamicIODefine.IoNameInfo> ioNameInfos = proc.ModuResult.GetAllOutputNameInfo();
                bool invalidOut;
                string vmResult = TryReadProcedureResultText(proc, ioNameInfos, out invalidOut);

                if (collectingStats)
                {
                    List<string> statsLabels = ResultStatistics.TryExtractLabels(proc);
                    if (statsLabels != null && statsLabels.Count > 0)
                    {
                        resultStats.SetDiagnostic(string.Empty);
                        resultStats.AddRange(statsLabels);
                    }
                    else if (!string.IsNullOrEmpty(vmResult))
                    {
                        resultStats.SetDiagnostic(string.Empty);
                        resultStats.Add(vmResult);
                    }
                    else
                    {
                        string diagnostic = ResultStatistics.BuildDiagnostic(proc);
                        resultStats.SetDiagnostic(diagnostic);
                        LogFunction("No label statistics collected. " + diagnostic.Replace("\r", " ").Replace("\n", " "));
                    }
                }

                Task.Run(() =>
                {
                    if (invalidOut)
                    {
                        UpdateResult("The result argument (out) is not string format!");
                    }
                    else if (!string.IsNullOrEmpty(vmResult))
                    {
                        UpdateResult(vmResult);
                    }
                    LogFunction("Process running time: " + proc.ProcessTime.ToString() + "ms");
                });
            }
            catch (VmException ex)
            {
                LogFunction("Failed to get results, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
            }
            catch (Exception ex)
            {
                LogFunction("Failed to get results: " + ex.Message);
            }
        }

        private VmProcedure ResolveProcedure(uint processId)
        {
            try
            {
                if (vmProcessInfoList.nNum > 0 && vmProcessInfoList.astProcessInfo != null)
                {
                    for (int i = 0; i < vmProcessInfoList.nNum && i < vmProcessInfoList.astProcessInfo.Length; i++)
                    {
                        if (vmProcessInfoList.astProcessInfo[i].nProcessID == processId)
                        {
                            return VmSolution.Instance[vmProcessInfoList.astProcessInfo[i].strProcessName] as VmProcedure;
                        }
                    }
                }
                if (processId == 10000)
                {
                    return vmProcedure;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string TryReadProcedureResultText(VmProcedure proc, List<VmDynamicIODefine.IoNameInfo> ioNameInfos, out bool invalidOut)
        {
            invalidOut = false;
            if (proc == null || ioNameInfos == null) return null;
            foreach (VmDynamicIODefine.IoNameInfo item in ioNameInfos)
            {
                if (item.Name == "out")
                {
                    if (item.TypeName != IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                    {
                        invalidOut = true;
                        return null;
                    }
                    return TryReadStringOutput(proc, item.Name);
                }
            }
            foreach (VmDynamicIODefine.IoNameInfo item in ioNameInfos)
            {
                if (item.TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                {
                    return TryReadStringOutput(proc, item.Name);
                }
            }
            return null;
        }

        private static string TryReadStringOutput(VmProcedure proc, string name)
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

        public void UpdateResult(string result)
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    labelResult.Text = result;
                    labelResult.BackColor = Color.FromArgb(255, 0, 192, 0);
                    listBoxResult.Items.Add("Results: " + result.ToString());
                    listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
                }));
            }
            catch (Exception ex)
            {
                LogFunction("Failed to update results: " + ex.Message);
            }
        }

        // ---- View switching ----------------------------------------------------------

        private void LoadSolutionIndicateTimer_Tick(object sender, EventArgs e)
        {
            if (!mSolutionIsLoad)
            {
                buttonLoadSolu.BackColor = (buttonLoadSolu.BackColor == Color.DimGray) ? Color.Orange : Color.DimGray;
            }
            else
            {
                buttonLoadSolu.BackColor = Color.DimGray;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(renderControl);
        }

        private void buttonRender_Click(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(renderControl);
            buttonRender.BackColor = Color.Orange;
            buttonConfig.BackColor = Color.Gray;
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(mainViewControl);
            buttonRender.BackColor = Color.Gray;
            buttonConfig.BackColor = Color.Orange;
        }

        // ---- Solution file management ------------------------------------------------

        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "VM Sol File|*.sol*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    vmSolutionPath = dlg.FileName;
                    mSolutionIsLoad = false;
                    LoadSolutionIndicateTimer.Enabled = true;
                    LogFunction("Selected: " + vmSolutionPath);
                }
            }
        }

        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            LoadSolutionIndicateTimer.Enabled = false;
            buttonLoadSolu.BackColor = Color.Orange;
            buttonLoadSolu.Enabled = false;
            buttonSelectSolu.Enabled = false;
            buttonRunOnce.Enabled = false;
            buttonContiRun.Enabled = false;
            buttonSaveSolu.Enabled = false;
            comboProcedure.Enabled = false;
            buttonRender.Enabled = false;
            buttonConfig.Enabled = false;
            try
            {
                if (vmSolutionPath != null && File.Exists(vmSolutionPath))
                {
                    VmSolution.Load(vmSolutionPath);
                    vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();
                    vmProcedure = VmSolution.Instance[vmProcessInfoList.astProcessInfo[0].strProcessName] as VmProcedure;

                    comboProcedure.Items.Clear();
                    for (int i = 0; i < vmProcessInfoList.nNum; i++)
                    {
                        comboProcedure.Items.Add(vmProcessInfoList.astProcessInfo[i].strProcessName);
                    }
                    if (comboProcedure.Items.Count > 0)
                    {
                        comboProcedure.SelectedIndex = 0;
                        comboProcedure.Text = comboProcedure.SelectedItem.ToString();
                    }
                    renderControl.vmRenderControl1.ModuleSource = vmProcedure;
                    mSolutionIsLoad = true;
                    LogFunction("Succeeded to load solution!");
                }
                else
                {
                    LogFunction("The Solution is null!");
                }
            }
            catch (VmException ex)
            {
                LogFunction("Failed to load solution, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
            }
            catch (Exception ex)
            {
                LogFunction("Failed to load solution: " + ex.Message);
            }
            finally
            {
                buttonLoadSolu.BackColor = Color.DimGray;
                buttonLoadSolu.Enabled = true;
                buttonSelectSolu.Enabled = true;
                buttonRunOnce.Enabled = true;
                buttonContiRun.Enabled = true;
                buttonSaveSolu.Enabled = true;
                comboProcedure.Enabled = true;
                buttonRender.Enabled = true;
                buttonConfig.Enabled = true;
            }
        }

        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            if (!mSolutionIsLoad)
            {
                LogFunction("Please load the solution first.");
                return;
            }
            try
            {
                VmSolution.Save();
                LogFunction("Succeeded to save solution!");
            }
            catch (VmException ex)
            {
                LogFunction("Failed to save solution, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
            }
        }

        private void comboProcedure_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (mSolutionIsLoad)
                {
                    comboProcedure.Items.Clear();
                    vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();
                    for (int i = 0; i < vmProcessInfoList.nNum; i++)
                    {
                        comboProcedure.Items.Add(vmProcessInfoList.astProcessInfo[i].strProcessName);
                    }
                }
                else
                {
                    LogFunction("Please load the solution first.");
                }
            }
            catch (VmException ex)
            {
                LogFunction("Failed to obtain procedures, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
            }
            catch (Exception ex)
            {
                LogFunction("Failed to obtain procedures: " + ex.Message);
            }
        }

        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(comboProcedure.Text))
                {
                    LogFunction("Please select a procedure.");
                    return;
                }
                vmProcedure = (VmProcedure)VmSolution.Instance[comboProcedure.Text];
                if (vmProcedure == null) return;
                renderControl.vmRenderControl1.ModuleSource = vmProcedure;
                vmProcedure.Run();
            }
            catch (VmException ex)
            {
                LogFunction("Failed to run procedure once, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
            }
            catch (Exception ex)
            {
                LogFunction("Failed to run procedure once: " + ex.Message);
            }
        }

        private void buttonResultStats_Click(object sender, EventArgs e)
        {
            try
            {
                string procedureName = string.IsNullOrWhiteSpace(comboProcedure.Text) ? resultStats.ProcedureName : comboProcedure.Text;
                resultStats.UseDemoResultData(procedureName);
                using (ResultStatisticsForm dlg = new ResultStatisticsForm(resultStats))
                {
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                LogFunction("Failed to open statistics: " + ex.Message);
            }
        }

        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(comboProcedure.Text))
                {
                    LogFunction("Please select a procedure.");
                    return;
                }
                vmProcedure = (VmProcedure)VmSolution.Instance[comboProcedure.Text];
                if (vmProcedure == null)
                {
                    LogFunction(comboProcedure.Text + ": procedure does not exist.");
                    return;
                }
                if (!vmProcedure.ContinuousRunEnable)
                {
                    buttonContiRun.Text = "Run Stop";
                }
                else
                {
                    buttonContiRun.Text = "Run Continuous";
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;
                    collectingStats = false;
                    buttonResultStats.Enabled = true;
                }
                vmProcedure.ContinuousRunEnable = vmProcedure.ContinuousRunEnable ^ true;
            }
            catch (VmException ex)
            {
                LogFunction("Failed continuous run, Error code: 0x" + Convert.ToString(ex.errorCode, 16));
            }
            catch (Exception ex)
            {
                LogFunction("Failed continuous run: " + ex.Message);
            }
        }

        // ---- Logging -----------------------------------------------------------------

        public void LogFunction(string strMsg)
        {
            if (strMsg == null) return;
            try
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add("");
                        item.SubItems[0].Text = DateTime.Now.ToString();
                        item.SubItems[1].Text = strMsg;
                        listViewLog.Items.Insert(0, item);
                        if (listViewLog.Items.Count > 5000) listViewLog.Items.RemoveAt(listViewLog.Items.Count - 1);
                    }));
                }
            }
            catch { /* control disposed */ }
            SaveLog(strMsg);
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }

        private void SaveLog(string str)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
                    string filename = logPath + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    using (StreamWriter sw = File.AppendText(filename))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss::ffff\t") + str);
                    }
                }
                catch { /* swallow */ }
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { if (chatPanel != null) chatPanel.CancelInFlight(); } catch { }

            if (vmSolutionPath != null && mSolutionIsLoad)
            {
                if (MessageBox.Show("Save solution or not?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (vmProcedure != null) VmSolution.Save();
                    }
                    catch
                    {
                        LogFunction("Failed to save solution!");
                    }
                }
            }
        }

        // ---- Localisation ------------------------------------------------------------

        private void buttonChineseOREnglish_Click(object sender, EventArgs e)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-cn");
            }
            LoadLanguage(this, typeof(MainForm));
        }

        public static void LoadLanguage(Form form, Type formType)
        {
            if (form == null) return;
            ComponentResourceManager resources = new ComponentResourceManager(formType);
            resources.ApplyResources(form, "$this");
            Loading(form, resources);
        }

        private static void Loading(Control control, ComponentResourceManager resources)
        {
            if (control is ListView)
            {
                resources.ApplyResources(control, control.Name);
                ListView lv = (ListView)control;
                if (lv.Columns.Count > 0) resources.ApplyResources(lv.Columns[0], "timeStampHeader");
                if (lv.Columns.Count > 1) resources.ApplyResources(lv.Columns[1], "infoHeader");
            }
            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                if (c.Controls != null && c.Controls.Count > 0) Loading(c, resources);
            }
        }
    }
}


