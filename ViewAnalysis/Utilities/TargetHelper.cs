using System.Collections.Generic;
using System.Xml;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Utilities
{
    internal static class TargetHelper
    {
        /// <summary>
        /// Generates the list of "TargetModels" for the tree view "Target" for the specified document
        /// </summary>
        /// <param name="xmlDocument">The xml document that has the list of data</param>
        /// <param name="count"></param>
        internal static List<MessageModel> GenerateTargetTreeList(XmlDocument xmlDocument, ref int count)
        {
            var messageModels = new List<MessageModel>();
            var targets = xmlDocument.GetElementsByTagName("Target");

            foreach (XmlNode target in targets)
            {
                foreach (XmlNode module in target.SelectNodes("Modules/Module"))
                {
                    messageModels.AddRange(XmlNodeHelper.NodeListHelper(module, ref count));
                }
            }

            return messageModels;
        }
    }
}