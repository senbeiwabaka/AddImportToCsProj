using System;
using System.Collections.Generic;
using System.Xml;
using ViewAnalysis.Enums;
using ViewAnalysis.Models;

namespace ViewAnalysis.Utilities
{
    internal static class MessageModelHelper
    {
        internal static void BuildMessageModel(MessageModel messageModel, XmlNode node)
        {
            messageModel.Id = XmlNodeHelper.GetNodeAttributeValue(node, "Id");
            messageModel.Category = XmlNodeHelper.GetNodeAttributeValue(node, "Category");
            messageModel.CheckId = XmlNodeHelper.GetNodeAttributeValue(node, "CheckId");
            messageModel.TypeName = XmlNodeHelper.GetNodeAttributeValue(node, "TypeName");
            messageModel.FixCategory = (FixCategories)Enum.Parse(typeof(FixCategories), XmlNodeHelper.GetNodeAttributeValue(node, "FixCategory"));
            messageModel.Status = (Statuses)Enum.Parse(typeof(Statuses), XmlNodeHelper.GetNodeAttributeValue(node, "Status"));

            messageModel.Issue = IssueModelHelper.BuildIssueModel(messageModel, node.FirstChild);
        }

        /// <summary>
        /// Build the list of messages from the specified node
        /// </summary>
        /// <param name="node">The node that has the list of messages</param>
        /// <param name="model">The model the messages are associated with</param>
        /// <returns></returns>
        internal static IReadOnlyCollection<MessageModel> BuildListOfMessages(XmlNode node, BaseModel model, ref int count)
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
    }
}