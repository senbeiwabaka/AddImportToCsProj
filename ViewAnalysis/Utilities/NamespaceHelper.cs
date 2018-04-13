using System.Collections.Generic;
using System.Xml;
using ViewAnalysis.Models;

namespace ViewAnalysis.Utilities
{
    internal static class NamespaceHelper
    {
        internal static IReadOnlyCollection<MessageModel> SetupNamespaceList(XmlNodeList namespaces, ref int count)
        {
            var messageModels = new List<MessageModel>(namespaces.Count);

            foreach (XmlNode @namespace in namespaces)
            {
                var namespaceModel = new NamespaceModel
                {
                    Name = XmlNodeHelper.GetNodeAttributeValue(@namespace, "Name")
                };

                // Get the list of messages
                messageModels.AddRange(MessageModelHelper.BuildListOfMessages(@namespace, namespaceModel, ref count));

                var list = XmlNodeHelper.NodeListHelper(@namespace, ref count);

                messageModels.AddRange(list);
            }

            return messageModels;
        }
    }
}