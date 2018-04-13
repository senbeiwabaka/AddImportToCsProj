using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ViewAnalysis.HelperForms;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Rules;

namespace ViewAnalysis.Controls
{
    public partial class TargetsView : UserControl
    {
        public TargetsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Take the loaded list of target issues and populate/setup the tree
        /// </summary>
        internal void SetTreeUp(IReadOnlyCollection<BaseModel> models)
        {
            tlvTargetsAnalysisTree.SetObjects(models);

            tlvTargetsAnalysisTree.CanExpandGetter = delegate (object x)
            {
                if (x is MessageModel messageModel)
                {
                    return messageModel.Issue != null;
                }

                return false;
            };

            tlvTargetsAnalysisTree.ChildrenGetter = delegate (object x)
            {
                if (x is MessageModel messageModel)
                {
                    var list = new List<IssueModel>(1)
                    {
                        messageModel.Issue
                    };

                    return list;
                }

                return null;
            };
        }

        internal void Collapse()
        {
            tlvTargetsAnalysisTree.CollapseAll();
        }

        internal void Expand()
        {
            tlvTargetsAnalysisTree.ExpandAll();
        }

        #region Tree Events

        private void TlvAnalysisTree_DoubleClick(object sender, EventArgs e)
        {
            if (tlvTargetsAnalysisTree.SelectedObject is IssueModel || tlvTargetsAnalysisTree.SelectedObject is RuleModel)
            {
                var form = new ViewAllDataForm((BaseModel) tlvTargetsAnalysisTree.SelectedObject);

                form.Show(this);
            }
        }

        private void TlvAnalysisTree_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            SetupTreeCellRightClickMenu(e);
        }

        private void TreeMenuItemViewClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag != null)
            {
                var form = new ViewAllDataForm(menuItem.Tag as BaseModel);

                form.Show(this);
            }
        }

        #endregion Tree Events

        private void SetupTreeCellRightClickMenu(BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            if (!(e.Model is IssueModel) && !(e.Model is RuleModel))
            {
                return;
            }

            e.MenuStrip = new ContextMenuStrip();
            e.MenuStrip.Items.Add(new ToolStripMenuItem("View Data", null, TreeMenuItemViewClick)
            {
                Tag = e.Model
            });
        }
    }
}