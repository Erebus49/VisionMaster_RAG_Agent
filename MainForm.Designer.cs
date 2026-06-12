namespace ChatDemoCs
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.renderPanel = new System.Windows.Forms.Panel();
            this.groupBoxSolution = new System.Windows.Forms.GroupBox();
            this.buttonSaveSolu = new System.Windows.Forms.Button();
            this.buttonLoadSolu = new System.Windows.Forms.Button();
            this.buttonSelectSolu = new System.Windows.Forms.Button();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.timeStampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.infoHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ClearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRunOnce = new System.Windows.Forms.Button();
            this.buttonContiRun = new System.Windows.Forms.Button();
            this.buttonResultStats = new System.Windows.Forms.Button();
            this.groupBoxProcedure = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboProcedure = new System.Windows.Forms.ComboBox();
            this.buttonRender = new System.Windows.Forms.Button();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.labelResult = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.buttonChineseOREnglish = new System.Windows.Forms.Button();
            this.groupBoxChat = new System.Windows.Forms.GroupBox();
            this.groupBoxSolution.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBoxProcedure.SuspendLayout();
            this.resultPanel.SuspendLayout();
            this.groupBoxResult.SuspendLayout();
            this.SuspendLayout();
            //
            // renderPanel
            //
            this.renderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderPanel.Location = new System.Drawing.Point(12, 7);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(868, 380);
            this.renderPanel.TabIndex = 0;
            //
            // groupBoxSolution
            //
            this.groupBoxSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSolution.Controls.Add(this.buttonSaveSolu);
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolu);
            this.groupBoxSolution.Controls.Add(this.buttonSelectSolu);
            this.groupBoxSolution.ForeColor = System.Drawing.Color.White;
            this.groupBoxSolution.Location = new System.Drawing.Point(886, 38);
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.Size = new System.Drawing.Size(379, 70);
            this.groupBoxSolution.TabIndex = 2;
            this.groupBoxSolution.TabStop = false;
            this.groupBoxSolution.Text = "方案操作";
            //
            // buttonSaveSolu
            //
            this.buttonSaveSolu.BackColor = System.Drawing.Color.DimGray;
            this.buttonSaveSolu.FlatAppearance.BorderSize = 0;
            this.buttonSaveSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveSolu.ForeColor = System.Drawing.Color.White;
            this.buttonSaveSolu.Location = new System.Drawing.Point(263, 25);
            this.buttonSaveSolu.Name = "buttonSaveSolu";
            this.buttonSaveSolu.Size = new System.Drawing.Size(110, 35);
            this.buttonSaveSolu.TabIndex = 2;
            this.buttonSaveSolu.Text = "保存方案";
            this.buttonSaveSolu.UseVisualStyleBackColor = false;
            this.buttonSaveSolu.Click += new System.EventHandler(this.buttonSaveSolu_Click);
            //
            // buttonLoadSolu
            //
            this.buttonLoadSolu.BackColor = System.Drawing.Color.DimGray;
            this.buttonLoadSolu.FlatAppearance.BorderSize = 0;
            this.buttonLoadSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadSolu.ForeColor = System.Drawing.Color.White;
            this.buttonLoadSolu.Location = new System.Drawing.Point(135, 25);
            this.buttonLoadSolu.Name = "buttonLoadSolu";
            this.buttonLoadSolu.Size = new System.Drawing.Size(110, 35);
            this.buttonLoadSolu.TabIndex = 1;
            this.buttonLoadSolu.Text = "加载方案";
            this.buttonLoadSolu.UseVisualStyleBackColor = false;
            this.buttonLoadSolu.Click += new System.EventHandler(this.buttonLoadSolu_Click);
            //
            // buttonSelectSolu
            //
            this.buttonSelectSolu.BackColor = System.Drawing.Color.DimGray;
            this.buttonSelectSolu.FlatAppearance.BorderSize = 0;
            this.buttonSelectSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectSolu.ForeColor = System.Drawing.Color.White;
            this.buttonSelectSolu.Location = new System.Drawing.Point(6, 25);
            this.buttonSelectSolu.Name = "buttonSelectSolu";
            this.buttonSelectSolu.Size = new System.Drawing.Size(110, 35);
            this.buttonSelectSolu.TabIndex = 0;
            this.buttonSelectSolu.Text = "选择方案";
            this.buttonSelectSolu.UseVisualStyleBackColor = false;
            this.buttonSelectSolu.Click += new System.EventHandler(this.buttonSelectSolu_Click);
            //
            // groupBoxLog
            //
            this.groupBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLog.Controls.Add(this.listViewLog);
            this.groupBoxLog.ForeColor = System.Drawing.Color.White;
            this.groupBoxLog.Location = new System.Drawing.Point(548, 393);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(332, 154);
            this.groupBoxLog.TabIndex = 3;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "日志消息";
            //
            // listViewLog
            //
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeStampHeader,
            this.infoHeader});
            this.listViewLog.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewLog.ForeColor = System.Drawing.Color.White;
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.Location = new System.Drawing.Point(8, 18);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(316, 130);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            //
            // timeStampHeader
            //
            this.timeStampHeader.Text = "时间";
            this.timeStampHeader.Width = 110;
            //
            // infoHeader
            //
            this.infoHeader.Text = "消息";
            this.infoHeader.Width = 200;
            //
            // contextMenuStrip1
            //
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            //
            // ClearToolStripMenuItem
            //
            this.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem";
            this.ClearToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.ClearToolStripMenuItem.Text = "清除";
            this.ClearToolStripMenuItem.Click += new System.EventHandler(this.ClearToolStripMenuItem_Click);
            //
            // buttonRunOnce
            //
            this.buttonRunOnce.BackColor = System.Drawing.Color.DimGray;
            this.buttonRunOnce.FlatAppearance.BorderSize = 0;
            this.buttonRunOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRunOnce.ForeColor = System.Drawing.Color.White;
            this.buttonRunOnce.Location = new System.Drawing.Point(135, 28);
            this.buttonRunOnce.Name = "buttonRunOnce";
            this.buttonRunOnce.Size = new System.Drawing.Size(110, 38);
            this.buttonRunOnce.TabIndex = 0;
            this.buttonRunOnce.Text = "运行一次";
            this.buttonRunOnce.UseVisualStyleBackColor = false;
            this.buttonRunOnce.Click += new System.EventHandler(this.buttonRunOnce_Click);
            //
            // buttonContiRun
            //
            this.buttonContiRun.BackColor = System.Drawing.Color.DimGray;
            this.buttonContiRun.FlatAppearance.BorderSize = 0;
            this.buttonContiRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonContiRun.ForeColor = System.Drawing.Color.White;
            this.buttonContiRun.Location = new System.Drawing.Point(262, 28);
            this.buttonContiRun.Name = "buttonContiRun";
            this.buttonContiRun.Size = new System.Drawing.Size(110, 38);
            this.buttonContiRun.TabIndex = 1;
            this.buttonContiRun.Text = "连续运行";
            this.buttonContiRun.UseVisualStyleBackColor = false;
            this.buttonContiRun.Click += new System.EventHandler(this.buttonContiRun_Click);
            // buttonResultStats
            //
            this.buttonResultStats.BackColor = System.Drawing.Color.DimGray;
            this.buttonResultStats.Enabled = false;
            this.buttonResultStats.FlatAppearance.BorderSize = 0;
            this.buttonResultStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonResultStats.ForeColor = System.Drawing.Color.White;
            this.buttonResultStats.Location = new System.Drawing.Point(135, 72);
            this.buttonResultStats.Name = "buttonResultStats";
            this.buttonResultStats.Size = new System.Drawing.Size(237, 32);
            this.buttonResultStats.TabIndex = 2;
            this.buttonResultStats.Text = "结果统计";
            this.buttonResultStats.UseVisualStyleBackColor = false;
            this.buttonResultStats.Click += new System.EventHandler(this.buttonResultStats_Click);
            //
            // groupBoxProcedure
            //
            this.groupBoxProcedure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProcedure.Controls.Add(this.label1);
            this.groupBoxProcedure.Controls.Add(this.buttonContiRun);
            this.groupBoxProcedure.Controls.Add(this.comboProcedure);
            this.groupBoxProcedure.Controls.Add(this.buttonRunOnce);
            this.groupBoxProcedure.Controls.Add(this.buttonResultStats);
            this.groupBoxProcedure.ForeColor = System.Drawing.Color.White;
            this.groupBoxProcedure.Location = new System.Drawing.Point(886, 114);
            this.groupBoxProcedure.Name = "groupBoxProcedure";
            this.groupBoxProcedure.Size = new System.Drawing.Size(379, 116);
            this.groupBoxProcedure.TabIndex = 4;
            this.groupBoxProcedure.TabStop = false;
            this.groupBoxProcedure.Text = "流程操作";
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择流程";
            //
            // comboProcedure
            //
            this.comboProcedure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProcedure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboProcedure.ForeColor = System.Drawing.Color.White;
            this.comboProcedure.FormattingEnabled = true;
            this.comboProcedure.Location = new System.Drawing.Point(6, 43);
            this.comboProcedure.Name = "comboProcedure";
            this.comboProcedure.Size = new System.Drawing.Size(121, 20);
            this.comboProcedure.TabIndex = 0;
            this.comboProcedure.DropDown += new System.EventHandler(this.comboProcedure_DropDown);
            //
            // buttonRender
            //
            this.buttonRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRender.BackColor = System.Drawing.Color.DimGray;
            this.buttonRender.FlatAppearance.BorderSize = 0;
            this.buttonRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRender.ForeColor = System.Drawing.Color.White;
            this.buttonRender.Location = new System.Drawing.Point(888, 7);
            this.buttonRender.Name = "buttonRender";
            this.buttonRender.Size = new System.Drawing.Size(82, 25);
            this.buttonRender.TabIndex = 5;
            this.buttonRender.Text = "图像显示";
            this.buttonRender.UseVisualStyleBackColor = false;
            this.buttonRender.Click += new System.EventHandler(this.buttonRender_Click);
            //
            // buttonConfig
            //
            this.buttonConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfig.BackColor = System.Drawing.Color.DimGray;
            this.buttonConfig.FlatAppearance.BorderSize = 0;
            this.buttonConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConfig.ForeColor = System.Drawing.Color.White;
            this.buttonConfig.Location = new System.Drawing.Point(971, 7);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(85, 25);
            this.buttonConfig.TabIndex = 6;
            this.buttonConfig.Text = "参数配置";
            this.buttonConfig.UseVisualStyleBackColor = false;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            //
            // labelResult
            //
            this.labelResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelResult.Font = new System.Drawing.Font("宋体", 22F);
            this.labelResult.ForeColor = System.Drawing.Color.White;
            this.labelResult.Location = new System.Drawing.Point(425, 22);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(100, 100);
            this.labelResult.TabIndex = 7;
            this.labelResult.Text = "OK";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // resultPanel
            //
            this.resultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.resultPanel.Controls.Add(this.groupBoxResult);
            this.resultPanel.Location = new System.Drawing.Point(12, 393);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(530, 154);
            this.resultPanel.TabIndex = 8;
            //
            // groupBoxResult
            //
            this.groupBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxResult.Controls.Add(this.listBoxResult);
            this.groupBoxResult.Controls.Add(this.labelResult);
            this.groupBoxResult.ForeColor = System.Drawing.Color.White;
            this.groupBoxResult.Location = new System.Drawing.Point(2, 0);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(528, 153);
            this.groupBoxResult.TabIndex = 9;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "结果";
            //
            // listBoxResult
            //
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxResult.ForeColor = System.Drawing.Color.White;
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.ItemHeight = 12;
            this.listBoxResult.Location = new System.Drawing.Point(6, 22);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(414, 120);
            this.listBoxResult.TabIndex = 8;
            //
            // buttonChineseOREnglish
            //
            this.buttonChineseOREnglish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChineseOREnglish.BackColor = System.Drawing.Color.DimGray;
            this.buttonChineseOREnglish.FlatAppearance.BorderSize = 0;
            this.buttonChineseOREnglish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChineseOREnglish.ForeColor = System.Drawing.Color.White;
            this.buttonChineseOREnglish.Location = new System.Drawing.Point(1062, 7);
            this.buttonChineseOREnglish.Name = "buttonChineseOREnglish";
            this.buttonChineseOREnglish.Size = new System.Drawing.Size(85, 25);
            this.buttonChineseOREnglish.TabIndex = 9;
            this.buttonChineseOREnglish.Text = "中文/英文";
            this.buttonChineseOREnglish.UseVisualStyleBackColor = false;
            this.buttonChineseOREnglish.Click += new System.EventHandler(this.buttonChineseOREnglish_Click);
            //
            // groupBoxChat
            //
            this.groupBoxChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxChat.ForeColor = System.Drawing.Color.White;
            this.groupBoxChat.Location = new System.Drawing.Point(886, 236);
            this.groupBoxChat.Name = "groupBoxChat";
            this.groupBoxChat.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxChat.Size = new System.Drawing.Size(379, 453);
            this.groupBoxChat.TabIndex = 10;
            this.groupBoxChat.TabStop = false;
            this.groupBoxChat.Text = "AI 助手 (DeepSeek)";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1278, 701);
            this.Controls.Add(this.groupBoxChat);
            this.Controls.Add(this.buttonChineseOREnglish);
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.buttonRender);
            this.Controls.Add(this.groupBoxProcedure);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxSolution);
            this.Controls.Add(this.renderPanel);
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "MainForm";
            this.Text = "ChatDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBoxLog.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBoxProcedure.ResumeLayout(false);
            this.groupBoxProcedure.PerformLayout();
            this.resultPanel.ResumeLayout(false);
            this.groupBoxResult.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel renderPanel;
        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Button buttonContiRun;
        private System.Windows.Forms.Button buttonResultStats;
        private System.Windows.Forms.Button buttonSaveSolu;
        private System.Windows.Forms.Button buttonLoadSolu;
        private System.Windows.Forms.Button buttonRunOnce;
        private System.Windows.Forms.Button buttonSelectSolu;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.GroupBox groupBoxProcedure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboProcedure;
        private System.Windows.Forms.Button buttonRender;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.ColumnHeader timeStampHeader;
        private System.Windows.Forms.ColumnHeader infoHeader;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ClearToolStripMenuItem;
        private System.Windows.Forms.Button buttonChineseOREnglish;
        private System.Windows.Forms.GroupBox groupBoxChat;
    }
}
