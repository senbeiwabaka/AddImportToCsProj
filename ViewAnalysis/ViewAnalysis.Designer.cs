using ViewAnalysis.Controls;

namespace ViewAnalysis
{
    partial class ViewAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbInformation = new System.Windows.Forms.TextBox();
            this.tcAnalysisTabs = new System.Windows.Forms.TabControl();
            this.tpIssues = new System.Windows.Forms.TabPage();
            this.olvIssues = new BrightIdeasSoftware.ObjectListView();
            this.olvcIssueName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcIssueRule = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcIssueText = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcIssueFixCategory = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcIssueCheckId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcIssueUrl = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tpNamespaces = new System.Windows.Forms.TabPage();
            this.tpTargets = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFolderLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAnalysisResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.totalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportListToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAnalysisFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcAnalysisTabs.SuspendLayout();
            this.tpIssues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvIssues)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbInformation
            // 
            this.tbInformation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbInformation.Location = new System.Drawing.Point(0, 334);
            this.tbInformation.Multiline = true;
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.ReadOnly = true;
            this.tbInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInformation.Size = new System.Drawing.Size(800, 116);
            this.tbInformation.TabIndex = 1;
            // 
            // tcAnalysisTabs
            // 
            this.tcAnalysisTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcAnalysisTabs.Controls.Add(this.tpIssues);
            this.tcAnalysisTabs.Controls.Add(this.tpNamespaces);
            this.tcAnalysisTabs.Controls.Add(this.tpTargets);
            this.tcAnalysisTabs.Location = new System.Drawing.Point(0, 28);
            this.tcAnalysisTabs.Name = "tcAnalysisTabs";
            this.tcAnalysisTabs.SelectedIndex = 0;
            this.tcAnalysisTabs.Size = new System.Drawing.Size(800, 300);
            this.tcAnalysisTabs.TabIndex = 2;
            this.tcAnalysisTabs.SelectedIndexChanged += new System.EventHandler(this.TcAnalysisTabs_SelectedIndexChanged);
            // 
            // tpIssues
            // 
            this.tpIssues.Controls.Add(this.olvIssues);
            this.tpIssues.Location = new System.Drawing.Point(4, 25);
            this.tpIssues.Name = "tpIssues";
            this.tpIssues.Padding = new System.Windows.Forms.Padding(3);
            this.tpIssues.Size = new System.Drawing.Size(792, 271);
            this.tpIssues.TabIndex = 2;
            this.tpIssues.Text = "Issues";
            this.tpIssues.UseVisualStyleBackColor = true;
            // 
            // olvIssues
            // 
            this.olvIssues.AllColumns.Add(this.olvcIssueName);
            this.olvIssues.AllColumns.Add(this.olvcIssueRule);
            this.olvIssues.AllColumns.Add(this.olvcIssueText);
            this.olvIssues.AllColumns.Add(this.olvcIssueFixCategory);
            this.olvIssues.AllColumns.Add(this.olvcIssueCheckId);
            this.olvIssues.AllColumns.Add(this.olvcIssueUrl);
            this.olvIssues.CausesValidation = false;
            this.olvIssues.CellEditUseWholeCell = false;
            this.olvIssues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvcIssueName,
            this.olvcIssueRule,
            this.olvcIssueText,
            this.olvcIssueFixCategory,
            this.olvcIssueCheckId,
            this.olvcIssueUrl});
            this.olvIssues.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvIssues.EmptyListMsg = "No issues found";
            this.olvIssues.FullRowSelect = true;
            this.olvIssues.Location = new System.Drawing.Point(3, 3);
            this.olvIssues.MultiSelect = false;
            this.olvIssues.Name = "olvIssues";
            this.olvIssues.PersistentCheckBoxes = false;
            this.olvIssues.SelectAllOnControlA = false;
            this.olvIssues.ShowItemCountOnGroups = true;
            this.olvIssues.Size = new System.Drawing.Size(786, 265);
            this.olvIssues.TabIndex = 0;
            this.olvIssues.UseCompatibleStateImageBehavior = false;
            this.olvIssues.UseFilterIndicator = true;
            this.olvIssues.UseFiltering = true;
            this.olvIssues.View = System.Windows.Forms.View.Details;
            this.olvIssues.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.OlvIssues_CellRightClick);
            this.olvIssues.DoubleClick += new System.EventHandler(this.OlvIssues_DoubleClick);
            // 
            // olvcIssueName
            // 
            this.olvcIssueName.AspectName = "Name";
            this.olvcIssueName.Hideable = false;
            this.olvcIssueName.IsEditable = false;
            this.olvcIssueName.Text = "Name";
            this.olvcIssueName.Width = 150;
            // 
            // olvcIssueRule
            // 
            this.olvcIssueRule.AspectName = "MessageModel.Rule.Name";
            this.olvcIssueRule.Hideable = false;
            this.olvcIssueRule.IsEditable = false;
            this.olvcIssueRule.Text = "Rule";
            this.olvcIssueRule.Width = 150;
            // 
            // olvcIssueText
            // 
            this.olvcIssueText.AspectName = "Text";
            this.olvcIssueText.Groupable = false;
            this.olvcIssueText.IsEditable = false;
            this.olvcIssueText.Searchable = false;
            this.olvcIssueText.Text = "Resolution";
            this.olvcIssueText.UseFiltering = false;
            this.olvcIssueText.Width = 150;
            // 
            // olvcIssueFixCategory
            // 
            this.olvcIssueFixCategory.AspectName = "FixCategory";
            this.olvcIssueFixCategory.Searchable = false;
            this.olvcIssueFixCategory.Text = "Fix Category";
            // 
            // olvcIssueCheckId
            // 
            this.olvcIssueCheckId.AspectName = "issueModel.MessageModel.CheckId";
            this.olvcIssueCheckId.HeaderCheckBoxUpdatesRowCheckBoxes = false;
            this.olvcIssueCheckId.IsEditable = false;
            this.olvcIssueCheckId.Searchable = false;
            this.olvcIssueCheckId.Text = "Check Id";
            // 
            // olvcIssueUrl
            // 
            this.olvcIssueUrl.AspectName = "issueModel.MessageModel.Rule.Url";
            this.olvcIssueUrl.Groupable = false;
            this.olvcIssueUrl.HeaderCheckBoxUpdatesRowCheckBoxes = false;
            this.olvcIssueUrl.Hyperlink = true;
            this.olvcIssueUrl.IsEditable = false;
            this.olvcIssueUrl.Searchable = false;
            this.olvcIssueUrl.Sortable = false;
            this.olvcIssueUrl.Text = "URL";
            this.olvcIssueUrl.UseFiltering = false;
            // 
            // tpNamespaces
            // 
            this.tpNamespaces.Location = new System.Drawing.Point(4, 25);
            this.tpNamespaces.Name = "tpNamespaces";
            this.tpNamespaces.Padding = new System.Windows.Forms.Padding(3);
            this.tpNamespaces.Size = new System.Drawing.Size(792, 271);
            this.tpNamespaces.TabIndex = 0;
            this.tpNamespaces.Text = "Namespaces";
            this.tpNamespaces.UseVisualStyleBackColor = true;
            // 
            // tpTargets
            // 
            this.tpTargets.Location = new System.Drawing.Point(4, 25);
            this.tpTargets.Name = "tpTargets";
            this.tpTargets.Padding = new System.Windows.Forms.Padding(3);
            this.tpTargets.Size = new System.Drawing.Size(792, 271);
            this.tpTargets.TabIndex = 1;
            this.tpTargets.Text = "Targets";
            this.tpTargets.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.totalsToolStripMenuItem,
            this.utilitiesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFolderLocationToolStripMenuItem,
            this.generateAnalysisToolStripMenuItem,
            this.loadAnalysisResultsToolStripMenuItem,
            this.clearAnalysisFilesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectFolderLocationToolStripMenuItem
            // 
            this.selectFolderLocationToolStripMenuItem.Name = "selectFolderLocationToolStripMenuItem";
            this.selectFolderLocationToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.selectFolderLocationToolStripMenuItem.Text = "Select Folder Location";
            this.selectFolderLocationToolStripMenuItem.Click += new System.EventHandler(this.SelectFolderLocationToolStripMenuItem_Click);
            // 
            // generateAnalysisToolStripMenuItem
            // 
            this.generateAnalysisToolStripMenuItem.Enabled = false;
            this.generateAnalysisToolStripMenuItem.Name = "generateAnalysisToolStripMenuItem";
            this.generateAnalysisToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.generateAnalysisToolStripMenuItem.Text = "Generate Analysis";
            this.generateAnalysisToolStripMenuItem.Click += new System.EventHandler(this.GenerateAnalysisToolStripMenuItem_Click);
            // 
            // loadAnalysisResultsToolStripMenuItem
            // 
            this.loadAnalysisResultsToolStripMenuItem.Enabled = false;
            this.loadAnalysisResultsToolStripMenuItem.Name = "loadAnalysisResultsToolStripMenuItem";
            this.loadAnalysisResultsToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.loadAnalysisResultsToolStripMenuItem.Text = "Load Analysis Results";
            this.loadAnalysisResultsToolStripMenuItem.Click += new System.EventHandler(this.LoadAnalysisResultsToolStripMenuItem_Click);
            // 
            // totalsToolStripMenuItem
            // 
            this.totalsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.warningToolStripMenuItem});
            this.totalsToolStripMenuItem.Name = "totalsToolStripMenuItem";
            this.totalsToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.totalsToolStripMenuItem.Text = "Totals";
            // 
            // warningToolStripMenuItem
            // 
            this.warningToolStripMenuItem.Name = "warningToolStripMenuItem";
            this.warningToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.warningToolStripMenuItem.Text = "Warnings";
            this.warningToolStripMenuItem.Click += new System.EventHandler(this.WarningToolStripMenuItem_Click);
            // 
            // utilitiesToolStripMenuItem
            // 
            this.utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.exportListToExcelToolStripMenuItem});
            this.utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            this.utilitiesToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.ExpandAllToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.CollapseAllToolStripMenuItem_Click);
            // 
            // exportListToExcelToolStripMenuItem
            // 
            this.exportListToExcelToolStripMenuItem.Enabled = false;
            this.exportListToExcelToolStripMenuItem.Name = "exportListToExcelToolStripMenuItem";
            this.exportListToExcelToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.exportListToExcelToolStripMenuItem.Text = "Export List to Excel";
            this.exportListToExcelToolStripMenuItem.Click += new System.EventHandler(this.ExportListToExcelToolStripMenuItem_Click);
            // 
            // clearAnalysisFilesToolStripMenuItem
            // 
            this.clearAnalysisFilesToolStripMenuItem.Enabled = false;
            this.clearAnalysisFilesToolStripMenuItem.Name = "clearAnalysisFilesToolStripMenuItem";
            this.clearAnalysisFilesToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.clearAnalysisFilesToolStripMenuItem.Text = "Clear Analysis Files";
            this.clearAnalysisFilesToolStripMenuItem.Visible = false;
            this.clearAnalysisFilesToolStripMenuItem.Click += new System.EventHandler(this.ClearAnalysisFilesToolStripMenuItem_Click);
            // 
            // ViewAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tcAnalysisTabs);
            this.Controls.Add(this.tbInformation);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ViewAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Analysis";
            this.Load += new System.EventHandler(this.ViewAnalysis_Load);
            this.tcAnalysisTabs.ResumeLayout(false);
            this.tpIssues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvIssues)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbInformation;
        private System.Windows.Forms.TabControl tcAnalysisTabs;
        private System.Windows.Forms.TabPage tpNamespaces;
        private System.Windows.Forms.TabPage tpTargets;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFolderLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAnalysisResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem totalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem warningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.TabPage tpIssues;
        private BrightIdeasSoftware.ObjectListView olvIssues;
        private BrightIdeasSoftware.OLVColumn olvcIssueName;
        private BrightIdeasSoftware.OLVColumn olvcIssueRule;
        private BrightIdeasSoftware.OLVColumn olvcIssueText;
        private BrightIdeasSoftware.OLVColumn olvcIssueFixCategory;
        private System.Windows.Forms.ToolStripMenuItem exportListToExcelToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvcIssueCheckId;
        private BrightIdeasSoftware.OLVColumn olvcIssueUrl;
        private System.Windows.Forms.ToolStripMenuItem clearAnalysisFilesToolStripMenuItem;
    }
}

