namespace ChatDemoCs
{
    partial class ResultStatisticsForm
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
            this.tableLayoutPanelRoot = new System.Windows.Forms.TableLayoutPanel();
            this.labelSummary = new System.Windows.Forms.Label();
            this.splitContainerTop = new System.Windows.Forms.SplitContainer();
            this.chartPanel = new System.Windows.Forms.Panel();
            this.listViewStats = new System.Windows.Forms.ListView();
            this.colRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPercent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelAi = new System.Windows.Forms.Panel();
            this.richTextBoxAi = new System.Windows.Forms.RichTextBox();
            this.panelAiToolbar = new System.Windows.Forms.Panel();
            this.labelAiTitle = new System.Windows.Forms.Label();
            this.buttonAnalyze = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tableLayoutPanelRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            this.panelAi.SuspendLayout();
            this.panelAiToolbar.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanelRoot
            //
            this.tableLayoutPanelRoot.ColumnCount = 1;
            this.tableLayoutPanelRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoot.Controls.Add(this.labelSummary, 0, 0);
            this.tableLayoutPanelRoot.Controls.Add(this.splitContainerTop, 0, 1);
            this.tableLayoutPanelRoot.Controls.Add(this.panelAi, 0, 2);
            this.tableLayoutPanelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRoot.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRoot.Name = "tableLayoutPanelRoot";
            this.tableLayoutPanelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.tableLayoutPanelRoot.RowCount = 3;
            this.tableLayoutPanelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanelRoot.Size = new System.Drawing.Size(960, 640);
            this.tableLayoutPanelRoot.TabIndex = 0;
            //
            // labelSummary
            //
            this.labelSummary.AutoEllipsis = true;
            this.labelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelSummary.ForeColor = System.Drawing.Color.White;
            this.labelSummary.Location = new System.Drawing.Point(11, 8);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(938, 36);
            this.labelSummary.TabIndex = 0;
            this.labelSummary.Text = "Summary";
            this.labelSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // splitContainerTop
            //
            this.splitContainerTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(11, 47);
            this.splitContainerTop.Name = "splitContainerTop";
            //
            // splitContainerTop.Panel1
            //
            this.splitContainerTop.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.splitContainerTop.Panel1.Controls.Add(this.chartPanel);
            this.splitContainerTop.Panel1MinSize = 240;
            //
            // splitContainerTop.Panel2
            //
            this.splitContainerTop.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.splitContainerTop.Panel2.Controls.Add(this.listViewStats);
            this.splitContainerTop.Panel2MinSize = 240;
            this.splitContainerTop.Size = new System.Drawing.Size(938, 311);
            this.splitContainerTop.SplitterDistance = 540;
            this.splitContainerTop.SplitterWidth = 6;
            this.splitContainerTop.TabIndex = 1;
            //
            // chartPanel
            //
            this.chartPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.chartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartPanel.Location = new System.Drawing.Point(0, 0);
            this.chartPanel.Name = "chartPanel";
            this.chartPanel.Size = new System.Drawing.Size(540, 311);
            this.chartPanel.TabIndex = 0;
            //
            // listViewStats
            //
            this.listViewStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.listViewStats.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewStats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRank,
            this.colLabel,
            this.colCount,
            this.colPercent});
            this.listViewStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewStats.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listViewStats.ForeColor = System.Drawing.Color.White;
            this.listViewStats.FullRowSelect = true;
            this.listViewStats.GridLines = true;
            this.listViewStats.HideSelection = false;
            this.listViewStats.Location = new System.Drawing.Point(0, 0);
            this.listViewStats.Name = "listViewStats";
            this.listViewStats.Size = new System.Drawing.Size(392, 311);
            this.listViewStats.TabIndex = 0;
            this.listViewStats.UseCompatibleStateImageBehavior = false;
            this.listViewStats.View = System.Windows.Forms.View.Details;
            //
            // colRank
            //
            this.colRank.Text = "#";
            this.colRank.Width = 36;
            //
            // colLabel
            //
            this.colLabel.Text = "Label";
            this.colLabel.Width = 200;
            //
            // colCount
            //
            this.colCount.Text = "Count";
            this.colCount.Width = 70;
            this.colCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // colPercent
            //
            this.colPercent.Text = "Percent";
            this.colPercent.Width = 80;
            this.colPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // panelAi
            //
            this.panelAi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelAi.Controls.Add(this.richTextBoxAi);
            this.panelAi.Controls.Add(this.panelAiToolbar);
            this.panelAi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAi.Location = new System.Drawing.Point(11, 364);
            this.panelAi.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.panelAi.Name = "panelAi";
            this.panelAi.Size = new System.Drawing.Size(938, 234);
            this.panelAi.TabIndex = 2;
            //
            // richTextBoxAi
            //
            this.richTextBoxAi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.richTextBoxAi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxAi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxAi.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.richTextBoxAi.ForeColor = System.Drawing.Color.White;
            this.richTextBoxAi.Location = new System.Drawing.Point(0, 36);
            this.richTextBoxAi.Name = "richTextBoxAi";
            this.richTextBoxAi.ReadOnly = true;
            this.richTextBoxAi.Size = new System.Drawing.Size(938, 198);
            this.richTextBoxAi.TabIndex = 1;
            this.richTextBoxAi.Text = "";
            //
            // panelAiToolbar
            //
            this.panelAiToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.panelAiToolbar.Controls.Add(this.labelAiTitle);
            this.panelAiToolbar.Controls.Add(this.buttonAnalyze);
            this.panelAiToolbar.Controls.Add(this.buttonExport);
            this.panelAiToolbar.Controls.Add(this.buttonCopy);
            this.panelAiToolbar.Controls.Add(this.buttonClear);
            this.panelAiToolbar.Controls.Add(this.buttonClose);
            this.panelAiToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAiToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelAiToolbar.Name = "panelAiToolbar";
            this.panelAiToolbar.Size = new System.Drawing.Size(938, 36);
            this.panelAiToolbar.TabIndex = 0;
            //
            // labelAiTitle
            //
            this.labelAiTitle.AutoSize = true;
            this.labelAiTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelAiTitle.ForeColor = System.Drawing.Color.White;
            this.labelAiTitle.Location = new System.Drawing.Point(8, 10);
            this.labelAiTitle.Name = "labelAiTitle";
            this.labelAiTitle.Size = new System.Drawing.Size(83, 17);
            this.labelAiTitle.TabIndex = 0;
            this.labelAiTitle.Text = "AI Analysis";
            //
            // buttonAnalyze
            //
            this.buttonAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAnalyze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.buttonAnalyze.FlatAppearance.BorderSize = 0;
            this.buttonAnalyze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAnalyze.ForeColor = System.Drawing.Color.White;
            this.buttonAnalyze.Location = new System.Drawing.Point(554, 4);
            this.buttonAnalyze.Name = "buttonAnalyze";
            this.buttonAnalyze.Size = new System.Drawing.Size(100, 28);
            this.buttonAnalyze.TabIndex = 1;
            this.buttonAnalyze.Text = "Analyze";
            this.buttonAnalyze.UseVisualStyleBackColor = false;
            this.buttonAnalyze.Click += new System.EventHandler(this.buttonAnalyze_Click);
            //
            // buttonExport
            //
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.BackColor = System.Drawing.Color.DimGray;
            this.buttonExport.FlatAppearance.BorderSize = 0;
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.ForeColor = System.Drawing.Color.White;
            this.buttonExport.Location = new System.Drawing.Point(660, 4);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(80, 28);
            this.buttonExport.TabIndex = 2;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = false;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            //
            // buttonCopy
            //
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.BackColor = System.Drawing.Color.DimGray;
            this.buttonCopy.FlatAppearance.BorderSize = 0;
            this.buttonCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopy.ForeColor = System.Drawing.Color.White;
            this.buttonCopy.Location = new System.Drawing.Point(746, 4);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(70, 28);
            this.buttonCopy.TabIndex = 3;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = false;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            //
            // buttonClear
            //
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.BackColor = System.Drawing.Color.DimGray;
            this.buttonClear.FlatAppearance.BorderSize = 0;
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.ForeColor = System.Drawing.Color.White;
            this.buttonClear.Location = new System.Drawing.Point(822, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(48, 28);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            //
            // buttonClose
            //
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.DimGray;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(876, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(58, 28);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            //
            // ResultStatisticsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(960, 640);
            this.Controls.Add(this.tableLayoutPanelRoot);
            this.MinimumSize = new System.Drawing.Size(720, 520);
            this.Name = "ResultStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Result Statistics";
            this.tableLayoutPanelRoot.ResumeLayout(false);
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.panelAi.ResumeLayout(false);
            this.panelAiToolbar.ResumeLayout(false);
            this.panelAiToolbar.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRoot;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.SplitContainer splitContainerTop;
        private System.Windows.Forms.Panel chartPanel;
        private System.Windows.Forms.ListView listViewStats;
        private System.Windows.Forms.ColumnHeader colRank;
        private System.Windows.Forms.ColumnHeader colLabel;
        private System.Windows.Forms.ColumnHeader colCount;
        private System.Windows.Forms.ColumnHeader colPercent;
        private System.Windows.Forms.Panel panelAi;
        private System.Windows.Forms.Panel panelAiToolbar;
        private System.Windows.Forms.RichTextBox richTextBoxAi;
        private System.Windows.Forms.Label labelAiTitle;
        private System.Windows.Forms.Button buttonAnalyze;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonClose;
    }
}
