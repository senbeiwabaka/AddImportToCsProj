using System;
using System.Collections.Generic;
using System.Xml;
using ViewAnalysis.Enums;
using ViewAnalysis.Models;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Utilities
{
    internal static class NamespaceHelper
    {
        internal static IReadOnlyCollection<NamespaceModel> SetupNamespaceList(XmlNodeList namespaces, ref int count, ModuleModel moduleModel = null)
        {
            var namespaceModels = new List<NamespaceModel>(namespaces.Count);

            foreach (XmlNode @namespace in namespaces)
            {
                var namespaceModel = new NamespaceModel(moduleModel)
                {
                    Name = XmlNodeHelper.GetNodeAttributeValue(@namespace, "Name")
                };

                // Get the list of messages
                namespaceModel.Messages.AddRange(MessageModelHelper.BuildListOfMessages(@namespace, namespaceModel, ref count));

                foreach (XmlNode type in @namespace.SelectNodes("Types/Type"))
                {
                    var typeModel = new TypeModel(namespaceModel)
                    {
                        Name = XmlNodeHelper.GetNodeAttributeValue(type, "Name"),
                        Kind = (Kinds)Enum.Parse(typeof(Kinds), XmlNodeHelper.GetNodeAttributeValue(type, "Kind")),
                        Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), XmlNodeHelper.GetNodeAttributeValue(type, "Accessibility")),
                        ExternallyVisible = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(type, "ExternallyVisible"))
                    };

                    // Get the list of messages
                    typeModel.Messages.AddRange(MessageModelHelper.BuildListOfMessages(type, typeModel, ref count));

                    foreach (XmlNode member in type.SelectNodes("Members/Member"))
                    {
                        var memberModel = new MemberModel(typeModel)
                        {
                            Name = XmlNodeHelper.GetNodeAttributeValue(member, "Name"),
                            Kind = (Kinds)Enum.Parse(typeof(Kinds), XmlNodeHelper.GetNodeAttributeValue(member, "Kind")),
                            Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), XmlNodeHelper.GetNodeAttributeValue(member, "Accessibility")),
                            ExternallyVisible = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(member, "ExternallyVisible")),
                            Static = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(member, "Static"))
                        };

                        // Get the list of messages
                        memberModel.Messages.AddRange(MessageModelHelper.BuildListOfMessages(member, memberModel, ref count));

                        foreach (XmlNode accessor in member.SelectNodes("Accessors/Accessor"))
                        {
                            var accessorModel = new AccessorModel(memberModel)
                            {
                                Name = XmlNodeHelper.GetNodeAttributeValue(member, "Name"),
                                Kind = (Kinds)Enum.Parse(typeof(Kinds), XmlNodeHelper.GetNodeAttributeValue(member, "Kind")),
                                Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), XmlNodeHelper.GetNodeAttributeValue(member, "Accessibility")),
                                ExternallyVisible = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(member, "ExternallyVisible")),
                                Static = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(member, "Static"))
                            };

                            // Get the list of messages
                            accessorModel.Messages.AddRange(MessageModelHelper.BuildListOfMessages(accessor, accessorModel, ref count));

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
    }
}