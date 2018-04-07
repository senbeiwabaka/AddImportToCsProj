using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ViewAnalysis.HelperForms;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Rules;
using ViewAnalysis.Models.Targets;
using System.Linq;

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
                if (x is TargetModel)
                {
                    return (x as TargetModel).Modules.Any();
                }

                if (x is ModuleModel)
                {
                    return (x as ModuleModel).Messages.Any();
                }

                if (x is MessageModel)
                {
                    return (x as MessageModel).Issue != null;
                }

                if (x is ModuleModel)
                {
                    return (x as ModuleModel).Namespaces.Any();
                }

                if (x is NamespaceModel)
                {
                    return (x as NamespaceModel).Types.Any();
                }

                return false;
            };

            tlvTargetsAnalysisTree.ChildrenGetter = delegate (object x)
            {
                if (x is TargetModel)
                {
                    return (x as TargetModel).Modules;
                }

                if (x is ModuleModel)
                {
                    return (x as ModuleModel).Messages;
                }

                if (x is MessageModel)
                {
                    var list = new List<IssueModel>(1)
                    {
                        (x as MessageModel).Issue
                    };

                    return list;
                }

                if (x is ModuleModel)
                {
                    return (x as ModuleModel).Namespaces;
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
                ViewAllDataForm form = new ViewAllDataForm(tlvTargetsAnalysisTree.SelectedObject as BaseModel);

                form.Show();
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
                ViewAllDataForm form = new ViewAllDataForm(menuItem.Tag as BaseModel);

                form.Show();
            }
        }

        #endregion Tree Events

        private void SetupTreeCellRightClickMenu(BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            if (e.Model is IssueModel || e.Model is RuleModel)
            {
                e.MenuStrip = new ContextMenuStrip();
                e.MenuStrip.Items.Add(new ToolStripMenuItem("View Data", null, TreeMenuItemViewClick)
                {
                    Tag = e.Model
                });
            }
        }
    }
}