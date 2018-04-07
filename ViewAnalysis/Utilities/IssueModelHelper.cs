using System;
using System.Xml;
using ViewAnalysis.Enums;
using ViewAnalysis.Models;

namespace ViewAnalysis.Utilities
{
    internal static class IssueModelHelper
    {
        internal static IssueModel BuildIssueModel(MessageModel messageModel, XmlNode issue)
        {
            var issueModel = new IssueModel(messageModel)
            {
                Certainty = Convert.ToInt32(XmlNodeHelper.GetNodeAttributeValue(issue, "Certainty")),
                Level = (Levels)Enum.Parse(typeof(Levels), XmlNodeHelper.GetNodeAttributeValue(issue, "Level") ?? "None"),
                Name = XmlNodeHelper.GetNodeAttributeValue(issue, "Name") ?? "No Issue Name",
                Path = XmlNodeHelper.GetNodeAttributeValue(issue, "Path"),
                File = XmlNodeHelper.GetNodeAttributeValue(issue, "File"),
                Line = Convert.ToInt32(XmlNodeHelper.GetNodeAttributeValue(issue, "Line") ?? "0"),
                Text = issue?.InnerText
            };

            return issueModel;
        }
    }
}