namespace ViewAnalysis.Controls
{
    partial class TargetsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlvTargetsAnalysisTree = new BrightIdeasSoftware.TreeListView();
            this.olvcName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcCertainty = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvcLevel = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.tlvTargetsAnalysisTree)).BeginInit();
            this.SuspendLayout();
            // 
            // tlvTargetsAnalysisTree
            // 
            this.tlvTargetsAnalysisTree.AllColumns.Add(this.olvcName);
            this.tlvTargetsAnalysisTree.AllColumns.Add(this.olvcCertainty);
            this.tlvTargetsAnalysisTree.AllColumns.Add(this.olvcStatus);
            this.tlvTargetsAnalysisTree.AllColumns.Add(this.olvcLevel);
            this.tlvTargetsAnalysisTree.CellEditUseWholeCell = false;
            this.tlvTargetsAnalysisTree.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvcName,
            this.olvcCertainty,
            this.olvcStatus,
            this.olvcLevel});
            this.tlvTargetsAnalysisTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlvTargetsAnalysisTree.Location = new System.Drawing.Point(0, 0);
            this.tlvTargetsAnalysisTree.Name = "tlvTargetsAnalysisTree";
            this.tlvTargetsAnalysisTree.ShowGroups = false;
            this.tlvTargetsAnalysisTree.Size = new System.Drawing.Size(378, 194);
            this.tlvTargetsAnalysisTree.TabIndex = 0;
            this.tlvTargetsAnalysisTree.UseCompatibleStateImageBehavior = false;
            this.tlvTargetsAnalysisTree.View = System.Windows.Forms.View.Details;
            this.tlvTargetsAnalysisTree.VirtualMode = true;
            this.tlvTargetsAnalysisTree.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.TlvAnalysisTree_CellRightClick);
            this.tlvTargetsAnalysisTree.DoubleClick += new System.EventHandler(this.TlvAnalysisTree_DoubleClick);
            // 
            // olvcName
            // 
            this.olvcName.AspectName = "Name";
            this.olvcName.AutoCompleteEditor = false;
            this.olvcName.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvcName.Groupable = false;
            this.olvcName.HeaderCheckBoxUpdatesRowCheckBoxes = false;
            this.olvcName.Hideable = false;
            this.olvcName.IsEditable = false;
            this.olvcName.Text = "Name";
            // 
            // olvcCertainty
            // 
            this.olvcCertainty.AspectName = "Certainty";
            this.olvcCertainty.AutoCompleteEditor = false;
            this.olvcCertainty.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvcCertainty.Groupable = false;
            this.olvcCertainty.HeaderCheckBoxUpdatesRowCheckBoxes = false;
            this.olvcCertainty.IsEditable = false;
            this.olvcCertainty.Text = "Certainty";
            // 
            // olvcStatus
            // 
            this.olvcStatus.AspectName = "Status";
            this.olvcStatus.AutoCompleteEditor = false;
            this.olvcStatus.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvcStatus.Groupable = false;
            this.olvcStatus.HeaderCheckBoxUpdatesRowCheckBoxes = false;
            this.olvcStatus.IsEditable = false;
            this.olvcStatus.Text = "Status";
            // 
            // olvcLevel
            // 
            this.olvcLevel.AspectName = "Level";
            this.olvcLevel.AutoCompleteEditor = false;
            this.olvcLevel.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvcLevel.Groupable = false;
            this.olvcLevel.HeaderCheckBoxUpdatesRowCheckBoxes = false;
            this.olvcLevel.IsEditable = false;
            this.olvcLevel.Text = "Level";
            // 
            // TargetsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlvTargetsAnalysisTree);
            this.Name = "TargetsView";
            this.Size = new System.Drawing.Size(378, 194);
            ((System.ComponentModel.ISupportInitialize)(this.tlvTargetsAnalysisTree)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView tlvTargetsAnalysisTree;
        private BrightIdeasSoftware.OLVColumn olvcName;
        private BrightIdeasSoftware.OLVColumn olvcCertainty;
        private BrightIdeasSoftware.OLVColumn olvcStatus;
        private BrightIdeasSoftware.OLVColumn olvcLevel;
    }
}
