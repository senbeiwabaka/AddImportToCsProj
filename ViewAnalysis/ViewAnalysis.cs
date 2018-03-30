using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ViewAnalysis.Enums;
using ViewAnalysis.HelperForms;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Rules;
using ViewAnalysis.Models.Targets;

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

                form.Show();
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
            SetControlState(true);

            var xmlDocuments = new List<XmlDocument>();

            try
            {
                // Get all of the analysis xml files from the selected folder path
                var files = Directory.EnumerateFiles(folderLocation, "code-analysis.xml", SearchOption.AllDirectories);

                // Turn all of them into xml documents
                foreach (var file in files)
                {
                    Console.WriteLine($"File: {file}");
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(file);

                    xmlDocuments.Add(xmlDocument);
                }

                // Go through each one of the documents and load them into models
                foreach (var xmlDocument in xmlDocuments)
                {
                    GenerateNamespaceTreeList(xmlDocument);
                    GenerateTargetTreeList(xmlDocument);
                    GenerateRuleTreeList(xmlDocument);
                }

                Console.WriteLine($"List size: {xmlDocuments.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                tbInformation.AppendText($"ERROR: {ex} {Environment.NewLine}");
            }

            xmlDocuments.Clear();
            xmlDocuments = null;

            GetDistinctListofRules();
            SetIssueMssagesRule();
            LoadNamespacesResultsToTreeView();
            LoadTargetsResultsToTreeView();
            LoadIssuesResultsToTreeView();

            SetControlState(false);
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
            var sumOfWarnings = NamespaceModelList.Sum(x => x.Messages.Count + x.Types.Count);
            sumOfWarnings += TargetModelList.Sum(x => x.Modules?.Sum(y => y.Messages.Count + y.Namespaces.Sum(z => z.Messages.Count + z.Types.Count))).Value;
            MessageBox.Show(
                $"The total number of warnings is {sumOfWarnings + RuleModelList.Count} or {count}",
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
                tlvTargetsAnalysisTree.ExpandAll();
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
                tlvTargetsAnalysisTree.CollapseAll();
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
            try
            {
                using (var stream = new FileStream(@"C:\Users\HB33713\Desktop\Export.xlsx", FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        using (var worksheet = package.Workbook.Worksheets.Add("First Sheet"))
                        {
                            // Header Columns
                            worksheet.Cells[1, 1].Value = "Name";
                            worksheet.Cells[1, 2].Value = "Rule";
                            worksheet.Cells[1, 3].Value = "Resolution";
                            worksheet.Cells[1, 4].Value = "Fix Category";
                            
                            var rowIndex = 2;

                            // Row Columns
                            foreach (var filteredObject in olvIssues.FilteredObjects)
                            {
                                var issueModel = filteredObject as IssueModel;

                                worksheet.Cells[rowIndex, 1].Value = issueModel.Name;
                                worksheet.Cells[rowIndex, 2].Value = issueModel.MessageModel.Rule.Name;
                                worksheet.Cells[rowIndex, 3].Value = issueModel.Text;
                                worksheet.Cells[rowIndex, 4].Value = Enum.Parse(typeof(FixCategories), issueModel.FixCategory);

                                ++rowIndex;
                            }

                            //worksheet.Cells.AutoFitColumns();

                            worksheet.View.FreezePanes(2, 4);

                            worksheet.Cells[1, 1, rowIndex, 4].AutoFilter = true;
                            //worksheet.Cells[1, 2, rowIndex, 2].AutoFilter = true;
                            //worksheet.Cells[1, 3, rowIndex, 3].AutoFilter = true;
                            //worksheet.Cells[1, 4, rowIndex, 4].AutoFilter = true;

                            package.SaveAs(stream);
                        }
                    }
                }

                MessageBox.Show("Export Successful", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                tbInformation.AppendText($"ERROR: {ex.Message}");

                MessageBox.Show(ex.Message, "Failed to export", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #endregion

        #region Helper Methods

        /// <summary>
        /// Generates the list of "NamespaceModels" for the tree view "Namespace" for the specified document
        /// </summary>
        /// <param name="xmlDocument">The xml document that has the list of data</param>
        private void GenerateNamespaceTreeList(XmlDocument xmlDocument)
        {
            var namespaces = xmlDocument.GetElementsByTagName("Namespace");

            var list = SetupNamespaceList(namespaces).ToList();

            list.ForEach(x => x.XmlFile = xmlDocument.BaseURI);

            NamespaceModelList.AddRange(list);
        }

        /// <summary>
        /// Generates the list of "TargetModels" for the tree view "Target" for the specified document
        /// </summary>
        /// <param name="xmlDocument">The xml document that has the list of data</param>
        private void GenerateTargetTreeList(XmlDocument xmlDocument)
        {
            var targets = xmlDocument.GetElementsByTagName("Target");

            foreach (XmlNode target in targets)
            {
                var targetModel = new TargetModel
                {
                    Name = GetNodeAttributeValue(target, "Name"),
                    XmlFile = xmlDocument.BaseURI
                };

                foreach (XmlNode module in target.SelectNodes("Modules/Module"))
                {
                    var moduleModel = new ModuleModel(targetModel)
                    {
                        Name = GetNodeAttributeValue(module, "Name")
                    };

                    // Get the list of messages
                    moduleModel.Messages.AddRange(BuildListOfMessages(module, moduleModel));

                    moduleModel.Namespaces.AddRange(SetupNamespaceList(module.SelectNodes("Namespaces/Namespace"), moduleModel));

                    targetModel.Modules.Add(moduleModel);
                }

                TargetModelList.Add(targetModel);
            }
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
                    TypeName = GetNodeAttributeValue(rule, "TypeName"),
                    Category = GetNodeAttributeValue(rule, "Category"),
                    CheckId = GetNodeAttributeValue(rule, "CheckId"),
                    Name = rule["Name"]?.InnerText,
                    Description = rule["Description"]?.InnerText,
                    Resolution = new ResolutionModel
                    {
                        Name = GetNodeAttributeValue(rule["Resolution"], "Name"),
                        Text = rule["Resolution"]?.InnerText
                    },
                    Owner = rule["Owner"]?.InnerText,
                    Url = rule["Url"]?.InnerText,
                    MessageLevel = new MessageLevelModel
                    {
                        Certainty = Convert.ToInt32(GetNodeAttributeValue(rule["MessageLevel"], "Certainty") ?? "0"),
                        Text = rule["MessageLevel"]?.InnerText
                    },
                    XmlFile = xmlDocument.BaseURI
                };

                RuleModelList.Add(ruleModel);
            }
        }

        private IReadOnlyCollection<NamespaceModel> SetupNamespaceList(XmlNodeList namespaces, ModuleModel moduleModel = null)
        {
            var namespaceModels = new List<NamespaceModel>(namespaces.Count);

            foreach (XmlNode @namespace in namespaces)
            {
                var namespaceModel = new NamespaceModel(moduleModel)
                {
                    Name = GetNodeAttributeValue(@namespace, "Name")
                };

                // Get the list of messages
                namespaceModel.Messages.AddRange(BuildListOfMessages(@namespace, namespaceModel));

                foreach (XmlNode type in @namespace.SelectNodes("Types/Type"))
                {
                    var typeModel = new TypeModel(namespaceModel)
                    {
                        Name = GetNodeAttributeValue(type, "Name"),
                        Kind = (Kinds)Enum.Parse(typeof(Kinds), GetNodeAttributeValue(type, "Kind")),
                        Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), GetNodeAttributeValue(type, "Accessibility")),
                        ExternallyVisible = Convert.ToBoolean(GetNodeAttributeValue(type, "ExternallyVisible"))
                    };

                    // Get the list of messages
                    typeModel.Messages.AddRange(BuildListOfMessages(type, typeModel));

                    foreach (XmlNode member in type.SelectNodes("Members/Member"))
                    {
                        var memberModel = new MemberModel(typeModel)
                        {
                            Name = GetNodeAttributeValue(member, "Name"),
                            Kind = (Kinds)Enum.Parse(typeof(Kinds), GetNodeAttributeValue(member, "Kind")),
                            Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), GetNodeAttributeValue(member, "Accessibility")),
                            ExternallyVisible = Convert.ToBoolean(GetNodeAttributeValue(member, "ExternallyVisible")),
                            Static = Convert.ToBoolean(GetNodeAttributeValue(member, "Static"))
                        };

                        // Get the list of messages
                        memberModel.Messages.AddRange(BuildListOfMessages(member, memberModel));

                        foreach (XmlNode accessor in member.SelectNodes("Accessors/Accessor"))
                        {
                            var accessorModel = new AccessorModel(memberModel)
                            {
                                Name = GetNodeAttributeValue(member, "Name"),
                                Kind = (Kinds)Enum.Parse(typeof(Kinds), GetNodeAttributeValue(member, "Kind")),
                                Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), GetNodeAttributeValue(member, "Accessibility")),
                                ExternallyVisible = Convert.ToBoolean(GetNodeAttributeValue(member, "ExternallyVisible")),
                                Static = Convert.ToBoolean(GetNodeAttributeValue(member, "Static"))
                            };

                            // Get the list of messages
                            accessorModel.Messages.AddRange(BuildListOfMessages(accessor, accessorModel));

                            memberModel.Accessors.Add(accessorModel);
                        }

                        typeModel.Members.Add(memberModel);
                    }

                    namespaceModel.Types.Add(typeModel);
                }

                namespaceModels.Add(namespaceModel);
            }

            return namespaceModels;
        }

        /// <summary>
        /// Build the list of messages from the specified node
        /// </summary>
        /// <param name="node">The node that has the list of messages</param>
        /// <param name="model">The model the messages are associated with</param>
        /// <returns></returns>
        private IReadOnlyCollection<MessageModel> BuildListOfMessages(XmlNode node, BaseModel model)
        {
            var messageModelList = new List<MessageModel>();

            foreach (XmlNode message in node.SelectNodes("Messages/Message"))
            {
                ++count;

                var messageModel = new MessageModel(model);

                BuildMessageModel(messageModel, message);

                messageModelList.Add(messageModel);
            }

            return messageModelList;
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

        /// <summary>
        /// Take the loaded list of target issues and populate/setup the tree
        /// </summary>
        private void LoadTargetsResultsToTreeView()
        {
            tlvTargetsAnalysisTree.SetObjects(TargetModelList);

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

        private static string GetNodeAttributeValue(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName]?.Value;
        }

        private static void BuildMessageModel(MessageModel messageModel, XmlNode node)
        {
            messageModel.Id = GetNodeAttributeValue(node, "Id");
            messageModel.Category = GetNodeAttributeValue(node, "Category");
            messageModel.CheckId = GetNodeAttributeValue(node, "CheckId");
            messageModel.TypeName = GetNodeAttributeValue(node, "TypeName");
            messageModel.FixCategory = (FixCategories)Enum.Parse(typeof(FixCategories), GetNodeAttributeValue(node, "FixCategory"));
            messageModel.Status = (Statuses)Enum.Parse(typeof(Statuses), GetNodeAttributeValue(node, "Status"));

            var issue = node.FirstChild;
            var issueModel = new IssueModel(messageModel)
            {
                Certainty = Convert.ToInt32(GetNodeAttributeValue(issue, "Certainty")),
                Level = (Levels)Enum.Parse(typeof(Levels), GetNodeAttributeValue(issue, "Level") ?? "None"),
                Name = GetNodeAttributeValue(issue, "Name") ?? "No Issue Name",
                Path = GetNodeAttributeValue(issue, "Path"),
                File = GetNodeAttributeValue(issue, "File"),
                Line = Convert.ToInt32(GetNodeAttributeValue(issue, "Line") ?? "0"),
                Text = issue?.InnerText
            };

            messageModel.Issue = issueModel;
        }

        #endregion Helper Methods

        
        
        
    }
}