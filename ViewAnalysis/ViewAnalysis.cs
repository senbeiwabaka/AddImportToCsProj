using Microsoft.Win32;
using NLog;
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
using ViewAnalysis.Utilities;

namespace ViewAnalysis
{
    public partial class ViewAnalysis : Form
    {
        private const string ErrorFileLocation = @"C:\Temp\Error.log";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly List<RuleModel> ruleModelList = new List<RuleModel>();
        private readonly List<IssueModel> issueModelList = new List<IssueModel>();
        private readonly Process analysisProcess = new Process();

        private int count;
        private string folderLocation;
        private string solutionPath;

        public ViewAnalysis()
        {
            InitializeComponent();
        }

        #region Tree Events

        private void TlvAnalysisTree_DoubleClick(object sender, EventArgs e)
        {
            //if (tlvNamespaceAnalysisTree.SelectedObject is IssueModel || tlvNamespaceAnalysisTree.SelectedObject is RuleModel)
            //{
            //    var form = new ViewAllDataForm((BaseModel) tlvNamespaceAnalysisTree.SelectedObject);

            //    form.Show(this);
            //}
        }

        private void OlvIssues_DoubleClick(object sender, EventArgs e)
        {
            if (olvIssues.SelectedObject is IssueModel || olvIssues.SelectedObject is RuleModel)
            {
                var form = new ViewAllDataForm((BaseModel)olvIssues.SelectedObject);

                form.Show(this);
            }
        }

        private void TlvAnalysisTree_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
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
                var form = new ViewAllDataForm(menuItem.Tag as BaseModel);

                form.Show(this);
            }
        }

        #endregion Tree Events

        #region Menu Events

        // TODO: Convert to helper form that can specify special folder structure
        private void SelectFolderLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = @"Select the folder where the .sln is. It will search through the folder set for one."
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
                        clearAnalysisFilesToolStripMenuItem.Enabled =
                            clearAnalysisFilesToolStripMenuItem.Visible = true;
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
                    GenerateNamespaceTreeListFromTarget(xmlDocument);
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

            LoadIssuesResultsToTreeView();

            SetControlState(false);

            var endTime = DateTime.Now;

            tbInformation.AppendText($"Time taken to execute: {endTime - startTime} {Environment.NewLine}");
            tbInformation.AppendText($"Memory: {ConvertBytesToMegabytes(GetMemoryUsed()):N2} MB");
        }

        private void GenerateAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                tbInformation.AppendText("Starting build for analysis");
                Logger.Info("Starting build for analysis");

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
                    MessageBox.Show("No Visual Studio build version found. Can not build project.", UserMessages.Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                var runBatContents = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}run.bat");

                runBatContents =
                    runBatContents.Replace("{cdlocation}", $"\"{msbuildPath.Replace("\\", @"\").Trim()}\"");
                runBatContents =
                    runBatContents.Replace("{solutionpath}", $"\"{solutionPath.Replace("\\", @"\").Trim()}\"");

                File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}run.bat", runBatContents);

                analysisProcess.StartInfo.FileName = $"{AppDomain.CurrentDomain.BaseDirectory}run.bat";
                analysisProcess.StartInfo.RedirectStandardInput = true;
                analysisProcess.StartInfo.RedirectStandardOutput = true;
                analysisProcess.StartInfo.RedirectStandardError = true;
                analysisProcess.EnableRaisingEvents = true;
                analysisProcess.StartInfo.CreateNoWindow = true;
                analysisProcess.StartInfo.UseShellExecute = false;
                analysisProcess.ErrorDataReceived += Cmd_ErrorDataReceived;
                analysisProcess.OutputDataReceived += Cmd_OutputDataReceived;
                analysisProcess.Exited += Cmd_Exited;
                analysisProcess.SynchronizingObject = this;

                analysisProcess.Start();

                analysisProcess.BeginOutputReadLine();
                analysisProcess.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Method: {nameof(GenerateAnalysisToolStripMenuItem_Click)}");

                tbInformation.AppendText($"ERROR: {ex.Message} {Environment.NewLine}");

                SetControlState(false);
            }
        }

        private void WarningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"The total number of warnings is {issueModelList.Count} or {count}",
                "Total Number of Warnings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // TODO: FIX
        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcAnalysisTabs.SelectedTab.Name == "tpNamespaces")
            {
                //tlvNamespaceAnalysisTree.ExpandAll();
            }
            else if (tcAnalysisTabs.SelectedTab.Name == "tpTargets")
            {
                //tvAnalysis.Expand();
            }
            else
            {
                // TODO: FIX
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
                //tlvNamespaceAnalysisTree.CollapseAll();
            }
            else if (tcAnalysisTabs.SelectedTab.Name == "tpTargets")
            {
                //tvAnalysis.Collapse();
            }
            else
            {
                // TODO: Fix
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
                MessageBox.Show("Must set/select save file", UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (string.IsNullOrWhiteSpace(saveDialog.FileName))
            {
                MessageBox.Show("Must set/select save file", UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var result = ExportHelper.Export(saveDialog.FileName, new ReadOnlyCollection<object>(olvIssues.FilteredObjects.Cast<object>().ToList()));

            if (result.Successful)
            {
                MessageBox.Show("Export Successful", UserMessages.Successful, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                tbInformation.AppendText($"ERROR: {result.Exception.Message}");

                MessageBox.Show(result.Exception.Message, UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAnalysisFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                tbInformation.AppendText($"Deleting files {Environment.NewLine}");

                loadAnalysisResultsToolStripMenuItem.Enabled = false;

                var files = Directory.EnumerateFiles(folderLocation, "code-analysis.xml", SearchOption.AllDirectories);

                // Turn all of them into xml documents
                foreach (var file in files)
                {
                    File.Delete(file);

                    tbInformation.AppendText($"File '{file}' deleted. {Environment.NewLine}");
                }

                tbInformation.AppendText($"Files deleted {Environment.NewLine}");

                clearAnalysisFilesToolStripMenuItem.Enabled = false;
                clearAnalysisFilesToolStripMenuItem.Visible = false;
            }
            catch (Exception ex)
            {
                tbInformation.AppendText($"ERROR: {ex} {Environment.NewLine}");
            }
        }

        private void ClearInformationWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbInformation.Clear();
        }

        private void ClearLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(ErrorFileLocation);
            }
            catch (Exception exception)
            {
                tbInformation.AppendText($"ERROR: {exception} {Environment.NewLine}");
            }
        }

        #endregion Menu Events

        #region Events

        private void Cmd_Exited(object sender, EventArgs e)
        {
            try
            {
                tbInformation.Clear();

                tbInformation.AppendText($"Finished build for analysis {Environment.NewLine}");
                Logger.Info("Finished build for analysis");

                SetControlState(false);

                tbInformation.AppendText(
                    $"Before GC: {ConvertBytesToMegabytes(GetMemoryUsed()):N2} MB {Environment.NewLine}");

                GC.Collect();

                tbInformation.AppendText(
                    $"After GC: {ConvertBytesToMegabytes(GetMemoryUsed()):N2} MB {Environment.NewLine}");
            }
            catch (Exception exception)
            {
                Logger.Error(exception, $"ERROR: {exception}");
            }
        }

        private void Cmd_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                tbInformation.AppendText($"{e.Data ?? string.Empty} {Environment.NewLine}");
                Logger.Debug(e.Data ?? string.Empty);
            }));
        }

        private void Cmd_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                tbInformation.Clear();

                Logger.Error($"ERROR: {e.Data ?? string.Empty}");
                Logger.Error($"ERROR: {sender}");
                tbInformation.AppendText($"ERROR: {e.Data ?? string.Empty} {Environment.NewLine}");
                tbInformation.AppendText($"ERROR: {sender} {Environment.NewLine}");

                try
                {
                    if (sender is Process process)
                    {
                        tbInformation.AppendText($"ERROR Exit Code: {process.ExitCode} {Environment.NewLine}");
                        Logger.Error($"ERROR Exit Code: {process.ExitCode}");
                    }
                }
                catch (Exception exception)
                {
                    tbInformation.AppendText($"ERROR: {exception} {Environment.NewLine}");
                    Logger.Error(exception, $"ERROR: {exception}");
                }

                try
                {
                    Logger.Error($"ERROR Exit Code: {analysisProcess.ExitCode}");
                    tbInformation.AppendText($"ERROR Exit Code: {analysisProcess.ExitCode} {Environment.NewLine}");
                }
                catch (Exception exception)
                {
                    tbInformation.AppendText($"ERROR: {exception} {Environment.NewLine}");
                    Logger.Error(exception, $"ERROR: {exception}");
                }

                SetControlState(false);

                tbInformation.AppendText($"Before GC: {ConvertBytesToMegabytes(GetMemoryUsed()):N2} MB {Environment.NewLine}");

                GC.Collect();

                tbInformation.AppendText($"After GC: {ConvertBytesToMegabytes(GetMemoryUsed()):N2} MB {Environment.NewLine}");
            }));
        }

        private void TcAnalysisTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            exportListToExcelToolStripMenuItem.Enabled = tcAnalysisTabs.SelectedTab.Name == "tpIssues";
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
            var list = NamespaceHelper.SetupNamespaceList(namespaces, ref count);

            issueModelList.AddRange(list.Select(x => x.Issue));
        }

        private void GenerateNamespaceTreeListFromTarget(XmlDocument xmlDocument)
        {
            var list = TargetHelper.GenerateTargetTreeList(xmlDocument, ref count);

            issueModelList.AddRange(list.Select(x => x.Issue));
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
                var ruleModel = new RuleModel(xmlDocument.BaseURI)
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
                    }
                };

                ruleModelList.Add(ruleModel);
            }
        }

        private void LoadIssuesResultsToTreeView()
        {
            olvIssues.SetObjects(issueModelList);
        }

        private void GetDistinctListofRules()
        {
            var distinctRules = ruleModelList.Distinct().ToList();
            ruleModelList.Clear();
            ruleModelList.AddRange(distinctRules);
        }

        private void SetIssueMssagesRule()
        {
            issueModelList.ForEach(issue =>
            {
                issue.MessageModel.Rule = ruleModelList.SingleOrDefault(rule => rule.CheckId == issue.MessageModel.CheckId);
            });

            ruleModelList.Clear();
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

        private static bool IsVisualStudio2017Installed()
        {
            var registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7");

            return registryKey != null;
        }

        private static string GetHeighestVersionOfVisualStudio(string version)
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
                    msbuildPath = @"C:\Windows\Microsoft.Net\Framework\v4.0.30319\";
                }
            }

            if (string.IsNullOrWhiteSpace(msbuildPath))
            {
                msbuildPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\";
            }

            return msbuildPath;
        }

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return bytes / 1024f / 1024f;
        }

        private static long GetMemoryUsed()
        {
            try
            {
                using (var process = Process.GetCurrentProcess())
                {
                    return process.WorkingSet64;
                }
            }
            catch
            {
                // ignored
            }

            return 0;
        }

        #endregion Helper Methods
    }
}
