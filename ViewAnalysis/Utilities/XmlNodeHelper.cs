using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Utilities
{
    internal static class XmlNodeHelper
    {
        private static readonly Dictionary<string, Type> typeObjectList = new Dictionary<string, Type>(10)
        {
            { "Types/Type", typeof(TypeModel) },
            { "Members/Member", typeof(MemberModel) },
            { "Accessors/Accessor", typeof(AccessorModel) },
            { "Namespaces/Namespace", typeof(NamespaceModel) }
        };

        internal static string GetNodeAttributeValue(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName]?.Value;
        }

        internal static List<MessageModel> NodeListHelper(XmlNode xmlNode, ref int count)
        {
            var messageModels = new List<MessageModel>();

            foreach (var typeObject in typeObjectList)
            {
                foreach (XmlNode node in xmlNode.SelectNodes(typeObject.Key))
                {
                    BaseModel baseModel = null;

                    if (typeObject.Value == typeof(TypeModel))
                    {
                        baseModel = ModelHelper.BuildTypeModel(node, string.Empty);
                    }

                    if (typeObject.Value == typeof(MemberModel))
                    {
                        baseModel = ModelHelper.BuildMemberModel(node, string.Empty);
                    }

                    if (typeObject.Value == typeof(AccessorModel))
                    {
                        baseModel = ModelHelper.BuildAccessorModel(node, string.Empty);
                    }

                    // Get the list of messages
                    messageModels.AddRange(MessageModelHelper.BuildListOfMessages(node, baseModel, ref count));

                    messageModels.AddRange(NodeListHelper(node, ref count));
                }
            }

            return messageModels;
        }
    }
}