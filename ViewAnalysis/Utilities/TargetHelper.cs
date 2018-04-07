using System.Collections.Generic;
using System.Xml;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Utilities
{
    internal static class TargetHelper
    {
        /// <summary>
        /// Generates the list of "TargetModels" for the tree view "Target" for the specified document
        /// </summary>
        /// <param name="xmlDocument">The xml document that has the list of data</param>
        internal static void GenerateTargetTreeList(XmlDocument xmlDocument, List<TargetModel> targetModelList, ref int count)
        {
            var targets = xmlDocument.GetElementsByTagName("Target");

            foreach (XmlNode target in targets)
            {
                var targetModel = new TargetModel
                {
                    Name = XmlNodeHelper.GetNodeAttributeValue(target, "Name"),
                    XmlFile = xmlDocument.BaseURI
                };

                foreach (XmlNode module in target.SelectNodes("Modules/Module"))
                {
                    var moduleModel = new ModuleModel(targetModel)
                    {
                        Name = XmlNodeHelper.GetNodeAttributeValue(module, "Name")
                    };

                    // Get the list of messages
                    moduleModel.Messages.AddRange(MessageModelHelper.BuildListOfMessages(module, moduleModel, ref count));

                    moduleModel.Namespaces.AddRange(NamespaceHelper.SetupNamespaceList(module.SelectNodes("Namespaces/Namespace"), ref count, moduleModel));

                    targetModel.Modules.Add(moduleModel);
                }

                targetModelList.Add(targetModel);
            }
        }
    }
}