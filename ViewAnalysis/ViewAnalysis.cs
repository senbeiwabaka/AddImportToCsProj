using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ViewAnalysis.HelperForms;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Rules;
using ViewAnalysis.Models.Targets;
using ViewAnalysis.Utilities;

namespace ViewAnalysis
{
    public partial class ViewAnalysis : Form
    {
        private readonly List<NamespaceModel> NamespaceModelList = new List<NamespaceModel>();
        private readonly List<TargetModel> TargetModelList = new List<TargetModel>();
        private readonly List<RuleModel> RuleModelList = new List<RuleModel>();
        private readonly List<IssueModel> IssueModelList = new List<IssueModel>();

        private int count = 0;
        private string folderLocation;
        private string solutionPath;

        public ViewAnalysis()
        {
            InitializeComponent();
        }

        #region Tree Events

        private void TlvAnalysisTree_DoubleClick(object sender, EventArgs e)
        {
            if (tlvNamespaceAnalysisTree.SelectedObject is IssueModel || tlvNamespaceAnalysisTree.SelectedObject is RuleModel)
            {
                ViewAllDataForm form = new ViewAllDataForm(tlvNamespaceAnalysisTree.SelectedObject as BaseModel);

                form.Show();
            }
        }

        private void OlvIssues_DoubleClick(object sender, EventArgs e)
        {
            if (olvIssues.SelectedObject is IssueModel || olvIssues.SelectedObject is RuleModel)
            {
                ViewAllDataForm form = new ViewAllDataForm(olvIssues.SelectedObject as BaseModel);

                form.Show(this);
            }
        }

        private void TlvAnalysisTree_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            SetupTreeCellRightClickMenu(e);
        }

        private void TlvTargetsAnalysisTree_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            SetupTreeCellRightClickMenu(e);
        }

        private void OlvIssues_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
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

        #region Menu Events

        // TODO: Convert to helper form that can specify special folder structure
        private void SelectFolderLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Select the folder where the .sln is. It will search through the folder set for one."
            };
            var dialogResult = folderDialog.ShowDialog();

            if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
            {
                var solutionFiles = Directory.GetFiles(folderDialog.SelectedPath, "*.sln", SearchOption.AllDirectories)
                    .Select(x => x.ToLowerInvariant())
                    .Where(x => x.Contains("builds"))
                    .ToList();

                if (!string.IsNullOrWhiteSpace(folderDialog.SelectedPath) && solutionFiles.Any())
                {
                    generateAnalysisToolStripMenuItem.Enabled = true;

                    folderLocation = folderDialog.SelectedPath;
                    solutionPath = solutionFiles.First();

                    if (Directory.EnumerateFiles(folderLocation, "code-analysis.xml", SearchOption.AllDirectories).Any())
                    {
                        loadAnalysisResultsToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }

        private void LoadAnalysisResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var startTime = DateTime.Now;

            SetControlState(true);

            try
            {
                // Get all of the analysis xml files from the selected folder path
                var files = Directory.EnumerateFiles(folderLocation, "code-analysis.xml", SearchOption.AllDirectories).ToList();

                // Turn all of them into xml documents
                foreach (var file in files)
                {
                    tbInformation.AppendText($"File: {file} {Environment.NewLine}");

                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(file);

                    GenerateNamespaceTreeList(xmlDocument);
                    TargetHelper.GenerateTargetTreeList(xmlDocument, TargetModelList, ref count);
                    GenerateRuleTreeList(xmlDocument);
                }

                tbInformation.AppendText($"List size: {files.Count} {Environment.NewLine}");
            }
            catch (Exception ex)
            {
                tbInformation.AppendText($"ERROR: {ex} {Environment.NewLine}");
            }

            GetDistinctListofRules();
            SetIssueMssagesRule();
            LoadNamespacesResultsToTreeView();
            
            LoadIssuesResultsToTreeView();

            tvAnalysis.SetTreeUp(TargetModelList);

            SetControlState(false);

            var endTime = DateTime.Now;

            tbInformation.AppendText($"Time taken to execute: {endTime - startTime} {Environment.NewLine}");
        }

        private void GenerateAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                tbInformation.AppendText("Starting build for analysis");

                SetControlState(true);

                var msbuildPath = string.Empty;

                if (IsVisualStudio2017Installed())
                {
                    msbuildPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\";
                }
                else
                {
                    var visualStudioVersion = new[] { "14.0", "12.0" };

                    foreach (var version in visualStudioVersion)
                    {
                        msbuildPath = GetHeighestVersionOfVisualStudio(version);

                        if (!string.IsNullOrWhiteSpace(msbuildPath))
                        {
                            break;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(msbuildPath))
                {
                    MessageBox.Show("No Visual Studio build version found. Can not build project.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                var runBatContents = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}run.bat");

                runBatContents = runBatContents.Replace("{cdlocation}", $"\"{msbuildPath.Replace("\\", @"\").Trim()}\"");
                runBatContents = runBatContents.Replace("{solutionpath}", $"\"{solutionPath.Replace("\\", @"\").Trim()}\"");

                File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}run.bat", runBatContents);

                using (var cmd = new Process())
                {
                    cmd.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}run.bat";
                    cmd.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.RedirectStandardError = true;
                    cmd.EnableRaisingEvents = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.ErrorDataReceived += Cmd_ErrorDataReceived;
                    cmd.OutputDataReceived += Cmd_OutputDataReceived;
                    cmd.Exited += Cmd_Exited;
                    cmd.SynchronizingObject = this;

                    cmd.Start();

                    cmd.BeginOutputReadLine();
                    cmd.BeginErrorReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                tbInformation.AppendText($"ERROR: {ex} {Environment.NewLine}");

                SetControlState(false);
            }
        }

        private void WarningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"The total number of warnings is {IssueModelList.Count} or {count}",
                "Total Number of Warnings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcAnalysisTabs.SelectedTab.Name == "tpNamespaces")
            {
                tlvNamespaceAnalysisTree.ExpandAll();
            }
            else if (tcAnalysisTabs.SelectedTab.Name == "tpTargets")
            {
                tvAnalysis.Expand();
            }
            else
            {
                foreach (BrightIdeasSoftware.OLVGroup group in olvIssues.Groups)
                {
                    group.Collapsed = false;
                }
            }
        }

        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcAnalysisTabs.SelectedTab.Name == "tpNamespaces")
            {
                tlvNamespaceAnalysisTree.CollapseAll();
            }
            else if (tcAnalysisTabs.SelectedTab.Name == "tpTargets")
            {
                tvAnalysis.Collapse();
            }
            else
            {
                foreach (BrightIdeasSoftware.OLVGroup group in olvIssues.Groups)
                {
                    group.Collapsed = true;
                }
            }
        }

        private void ExportListToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                CreatePrompt = true,
                CheckPathExists = true,
                OverwritePrompt = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                SupportMultiDottedExtensions = false,
                ValidateNames = true
            };
            var dialogResult = saveDialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                MessageBox.Show("Must set/select save file", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (string.IsNullOrWhiteSpace(saveDialog.FileName))
            {
                MessageBox.Show("Must set/select save file", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            var result = ExportHelper.Export(saveDialog.FileName, new ReadOnlyCollection<object>(olvIssues.FilteredObjects.Cast<object>().ToList()));

            if (result.Successful)
            {
                MessageBox.Show("Export Successful", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                tbInformation.AppendText($"ERROR: {result.Exception.Message}");

                MessageBox.Show(result.Exception.Message, "Failed to export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Menu Events

        #region Events

        private void Cmd_Exited(object sender, EventArgs e)
        {
            tbInformation.Clear();

            tbInformation.AppendText($"Finished build for analysis {Environment.NewLine}");

            SetControlState(false);
        }

        private void Cmd_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                tbInformation.AppendText($"{e.Data ?? string.Empty} {Environment.NewLine}");
            }));
        }

        private void Cmd_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                tbInformation.Clear();

                tbInformation.AppendText($"{e.Data ?? string.Empty} {Environment.NewLine}");

                SetControlState(false);
            }));
        }

        private void TcAnalysisTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcAnalysisTabs.SelectedTab.Name == "tpIssues")
            {
                exportListToExcelToolStripMenuItem.Enabled = true;
            }
            else
            {
                exportListToExcelToolStripMenuItem.Enabled = false;
            }
        }

        private void ViewAnalysis_Load(object sender, EventArgs e)
        {
            exportListToExcelToolStripMenuItem.Enabled = tcAnalysisTabs.SelectedTab.Name == "tpIssues";
        }

        #endregion Events

        #region Helper Methods

        /// <summary>
        /// Generates the list of "NamespaceModels" for the tree view "Namespace" for the specified document
        /// </summary>
        /// <param name="xmlDocument">The xml document that has the list of data</param>
        private void GenerateNamespaceTreeList(XmlDocument xmlDocument)
        {
            var namespaces = xmlDocument.GetElementsByTagName("Namespace");
            var list = NamespaceHelper.SetupNamespaceList(namespaces, ref count).ToList();

            list.ForEach(x => x.XmlFile = xmlDocument.BaseURI);

            NamespaceModelList.AddRange(list);
        }

        /// <summary>
        /// Generates the list of "RulesModel" for each issue
        /// </summary>
        /// <param name="xmlDocument">The xml document that has the list of data</param>
        private void GenerateRuleTreeList(XmlDocument xmlDocument)
        {
            var rules = xmlDocument.GetElementsByTagName("Rule");

            foreach (XmlNode rule in rules)
            {
                var ruleModel = new RuleModel
                {
                    TypeName = XmlNodeHelper.GetNodeAttributeValue(rule, "TypeName"),
                    Category = XmlNodeHelper.GetNodeAttributeValue(rule, "Category"),
                    CheckId = XmlNodeHelper.GetNodeAttributeValue(rule, "CheckId"),
                    Name = rule["Name"]?.InnerText,
                    Description = rule["Description"]?.InnerText,
                    Resolution = new ResolutionModel
                    {
                        Name = XmlNodeHelper.GetNodeAttributeValue(rule["Resolution"], "Name"),
                        Text = rule["Resolution"]?.InnerText
                    },
                    Owner = rule["Owner"]?.InnerText,
                    Url = rule["Url"]?.InnerText,
                    MessageLevel = new MessageLevelModel
                    {
                        Certainty = Convert.ToInt32(XmlNodeHelper.GetNodeAttributeValue(rule["MessageLevel"], "Certainty") ?? "0"),
                        Text = rule["MessageLevel"]?.InnerText
                    },
                    XmlFile = xmlDocument.BaseURI
                };

                RuleModelList.Add(ruleModel);
            }
        }

        /// <summary>
        /// Take the loaded list of namespace issues and populate/setup the tree
        /// </summary>
        private void LoadNamespacesResultsToTreeView()
        {
            tlvNamespaceAnalysisTree.SetObjects(NamespaceModelList);

            tlvNamespaceAnalysisTree.CanExpandGetter = delegate (object x)
            {
                if (x is NamespaceModel)
                {
                    return (x as NamespaceModel).Messages.Any();
                }

                if (x is MessageModel)
                {
                    return (x as MessageModel).Issue != null;
                }

                return false;
            };

            tlvNamespaceAnalysisTree.ChildrenGetter = delegate (object x)
            {
                if (x is NamespaceModel)
                {
                    return (x as NamespaceModel).Messages;
                }

                if (x is MessageModel)
                {
                    var list = new List<IssueModel>(1)
                    {
                        (x as MessageModel).Issue
                    };

                    return list;
                }

                return null;
            };
        }
        
        private void LoadIssuesResultsToTreeView()
        {
            var issueList = new List<IssueModel>();

            NamespaceModelList.ForEach(namespaceModel => namespaceModel.Messages.ForEach(messageModel =>
            {
                issueList.Add(messageModel.Issue);
            }));

            TargetModelList.ForEach(targetModel =>
            {
                targetModel.Modules.ForEach(moduleModel =>
                {
                    moduleModel.Messages.ForEach(messageModel =>
                    {
                        issueList.Add(messageModel.Issue);
                    });

                    moduleModel.Namespaces.ForEach(namespaceModel =>
                    {
                        namespaceModel.Messages.ForEach(messageModel =>
                        {
                            issueList.Add(messageModel.Issue);
                        });

                        namespaceModel.Types.ForEach(typeModel =>
                        {
                            typeModel.Messages.ForEach(messageModel =>
                            {
                                issueList.Add(messageModel.Issue);
                            });

                            typeModel.Members.ForEach(memberModel =>
                            {
                                memberModel.Messages.ForEach(messageModel =>
                                {
                                    issueList.Add(messageModel.Issue);
                                });

                                memberModel.Accessors.ForEach(accessorModel =>
                                {
                                    accessorModel.Messages.ForEach(messageModel =>
                                    {
                                        issueList.Add(messageModel.Issue);
                                    });
                                });
                            });
                        });
                    });
                });
            });

            IssueModelList.AddRange(issueList);

            olvIssues.SetObjects(IssueModelList);
        }

        private void GetDistinctListofRules()
        {
            var distinctRules = RuleModelList.Distinct().ToList();
            RuleModelList.Clear();
            RuleModelList.AddRange(distinctRules);
        }

        private void SetIssueMssagesRule()
        {
            NamespaceModelList.ForEach(namespaceModel => namespaceModel.Messages.ForEach(messageModel =>
            {
                messageModel.Rule = RuleModelList.Single(ruleModel => ruleModel.CheckId == messageModel.CheckId);
            }));

            TargetModelList.ForEach(targetModel =>
            {
                targetModel.Modules.ForEach(moduleModel =>
                {
                    moduleModel.Messages.ForEach(messageModel =>
                    {
                        messageModel.Rule = RuleModelList.Single(ruleModel => ruleModel.CheckId == messageModel.CheckId);
                    });

                    moduleModel.Namespaces.ForEach(namespaceModel =>
                    {
                        namespaceModel.Messages.ForEach(messageModel =>
                        {
                            messageModel.Rule = RuleModelList.Single(ruleModel => ruleModel.CheckId == messageModel.CheckId);
                        });

                        namespaceModel.Types.ForEach(typeModel =>
                        {
                            typeModel.Messages.ForEach(messageModel =>
                            {
                                messageModel.Rule = RuleModelList.Single(ruleModel => ruleModel.CheckId == messageModel.CheckId);
                            });

                            typeModel.Members.ForEach(memberModel =>
                            {
                                memberModel.Messages.ForEach(messageModel =>
                                {
                                    messageModel.Rule = RuleModelList.Single(ruleModel => ruleModel.CheckId == messageModel.CheckId);
                                });

                                memberModel.Accessors.ForEach(accessorModel =>
                                {
                                    accessorModel.Messages.ForEach(messageModel =>
                                    {
                                        messageModel.Rule = RuleModelList.Single(ruleModel => ruleModel.CheckId == messageModel.CheckId);
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }

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

        private void SetControlState(bool state)
        {
            foreach (Control control in Controls)
            {
                control.UseWaitCursor = state;
                control.Enabled = !state;
            }
        }

        private bool IsVisualStudio2017Installed()
        {
            var registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7");

            return registryKey != null;
        }

        private string GetHeighestVersionOfVisualStudio(string version)
        {
            var visualStudioInstalledPath = string.Empty;
            var visualStudioRegistryPath = Registry.LocalMachine.OpenSubKey($@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\{version}");
            var msbuildPath = string.Empty;

            if (visualStudioRegistryPath != null)
            {
                visualStudioInstalledPath = visualStudioRegistryPath.GetValue("InstallDir", string.Empty) as string;
                msbuildPath = @" C:\Windows\Microsoft.Net\Framework64\v4.0.30319\";
            }

            if (string.IsNullOrEmpty(visualStudioInstalledPath) || !Directory.Exists(visualStudioInstalledPath))
            {
                visualStudioRegistryPath = Registry.LocalMachine.OpenSubKey($@"SOFTWARE\Microsoft\VisualStudio\{version}");
                if (visualStudioRegistryPath != null)
                {
                    visualStudioInstalledPath = visualStudioRegistryPath.GetValue("InstallDir", string.Empty) as string;
                    msbuildPath = @"C:\Windows\Microsoft.Net\Framework\v4.0.30319\";
                }
            }

            if (string.IsNullOrWhiteSpace(msbuildPath))
            {
                msbuildPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\";
            }

            return msbuildPath;
        }

        #endregion Helper Methods
    }
}