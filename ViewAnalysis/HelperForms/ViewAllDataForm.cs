using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using ViewAnalysis.Enums;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.HelperForms
{
    public partial class ViewAllDataForm : Form
    {
        private readonly BaseModel baseModel;

        public ViewAllDataForm()
        {
            InitializeComponent();
        }

        internal ViewAllDataForm(BaseModel baseModel) : this()
        {
            this.baseModel = baseModel;
        }

        private void ViewAllDataForm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (baseModel is IssueModel model)
                {
                    var messageModel = model.MessageModel;
                    var ruleModel = messageModel.Rule;
                    var stringBuilder = new StringBuilder();
                    
                    var projectLocation = ruleModel?.XmlFile ?? string.Empty;
                    var name = string.Empty;

                    if (messageModel.Model is NamespaceModel namespaceModel)
                    {
                        name = namespaceModel.Name;
                    }
                    if (messageModel.Model is MemberModel memberModel)
                    {
                        name = memberModel.Name;
                    }
                    if (messageModel.Model is TypeModel typeModel)
                    {
                        name = typeModel.Name;
                    }

                    stringBuilder.Append(@"{\rtf1\ansi");
                    AddContent(
                        "**Project Location",
                        projectLocation.Replace("file:///", string.Empty).Replace("/code-analysis.xml", string.Empty),
                        stringBuilder);
                    AddNewLine(stringBuilder);
                    AddContent(
                        "Analysis File Location",
                        projectLocation.Replace(' ', (char)160),
                        stringBuilder);
                    AddNewLine(stringBuilder);
                    AddNewLine(stringBuilder);
                    stringBuilder.Append("---- Issue ----");
                    AddNewLine(stringBuilder);

                    if (!string.IsNullOrWhiteSpace(model.Path) || !string.IsNullOrWhiteSpace(model.File))
                    {
                        AddContent("Offending File", $"file:///{(model.Path + @"\" + model.File).Replace("\\", "/").Replace(' ', (char)160)}", stringBuilder);
                        AddNewLine(stringBuilder);
                        if (model.Line > 0)
                        {
                            AddContent("Line on File", $"{model.Line}", stringBuilder);
                            AddNewLine(stringBuilder);
                        }
                    }

                    AddContent("Name", name ?? "No Name", stringBuilder);
                    AddContent("Message - Type Name", messageModel.TypeName ?? "No Message - Type Name", stringBuilder);
                    AddContent("Message - Category", messageModel.Category ?? "No Message - Category", stringBuilder);
                    AddContent("Message - CheckId", messageModel.CheckId ?? "No Message - CheckId", stringBuilder);
                    AddContent("Message - Status", Enum.GetName(typeof(Statuses), messageModel.Status), stringBuilder);
                    AddContent("Message - Fix Category", Enum.GetName(typeof(FixCategories), messageModel.FixCategory), stringBuilder);
                    AddNewLine(stringBuilder);
                    AddNewLine(stringBuilder);

                    AddContent("--**RESOLUTION**--", model.Text, stringBuilder);

                    AddNewLine(stringBuilder);
                    AddNewLine(stringBuilder);

                    if (ruleModel != null)
                    {
                        stringBuilder.Append("---- Rule ----");
                        AddNewLine(stringBuilder);
                        AddContent("Name", ruleModel.Name ?? "No Name", stringBuilder);
                        AddContent("Type Name", ruleModel.TypeName ?? "No Type Name", stringBuilder);
                        AddContent("Category", ruleModel.Category ?? "No Category", stringBuilder);
                        AddContent("CheckId", ruleModel.CheckId ?? "No CheckId", stringBuilder);
                        AddContent("Resolution - Name", ruleModel.Resolution?.Name ?? "No Resolution - Name", stringBuilder);
                        AddContent("Resolution - Text", ruleModel.Resolution?.Text, stringBuilder);
                        AddContent("URL", ruleModel.Url, stringBuilder);
                        AddContent("Message Level - Certainty", ruleModel.MessageLevel?.Certainty.ToString(), stringBuilder);
                        AddContent("Message Level - Text", ruleModel.MessageLevel?.Text, stringBuilder);
                        AddNewLine(stringBuilder);
                        AddContent("Description", ruleModel.Description ?? "No Description", stringBuilder);
                        AddNewLine(stringBuilder);
                        AddNewLine(stringBuilder);
                    }

                    stringBuilder.Append(@"}");

                    rtbData.Rtf = stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void AddContent(string contentName, string content, StringBuilder stringBuilder)
        {
            stringBuilder.Append($@"\b {contentName}: \b0 ");
            stringBuilder.Append(content);
            AddNewLine(stringBuilder);
        }

        private static void AddNewLine(StringBuilder stringBuilder)
        {
            stringBuilder.Append(@" \line ");
        }

        private void RtbData_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                using (var openLinkProcess = new Process())
                {
                    openLinkProcess.StartInfo.UseShellExecute = true;
                    openLinkProcess.StartInfo.FileName = e.LinkText.Replace((char)160, ' ');

                    openLinkProcess.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}