using System;
using System.Xml;
using ViewAnalysis.Enums;
using ViewAnalysis.Models.Targets;

namespace ViewAnalysis.Utilities
{
    internal static class ModelHelper
    {
        internal static TypeModel BuildTypeModel(XmlNode node, string documentUri)
        {
            return new TypeModel
            {
                Name = XmlNodeHelper.GetNodeAttributeValue(node, "Name"),
                Kind = (Kinds)Enum.Parse(typeof(Kinds), XmlNodeHelper.GetNodeAttributeValue(node, "Kind")),
                Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), XmlNodeHelper.GetNodeAttributeValue(node, "Accessibility")),
                ExternallyVisible = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(node, "ExternallyVisible"))
            };
        }

        internal static MemberModel BuildMemberModel(XmlNode node, string documentUri)
        {
            return new MemberModel()
            {
                Name = XmlNodeHelper.GetNodeAttributeValue(node, "Name"),
                Kind = (Kinds)Enum.Parse(typeof(Kinds), XmlNodeHelper.GetNodeAttributeValue(node, "Kind")),
                Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities),
                    XmlNodeHelper.GetNodeAttributeValue(node, "Accessibility")),
                ExternallyVisible = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(node, "ExternallyVisible")),
                Static = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(node, "Static"))
            };
        }

        internal static AccessorModel BuildAccessorModel(XmlNode node, string documentUri)
        {
            return new AccessorModel()
            {
                Name = XmlNodeHelper.GetNodeAttributeValue(node, "Name"),
                Kind = (Kinds)Enum.Parse(typeof(Kinds), XmlNodeHelper.GetNodeAttributeValue(node, "Kind")),
                Accessibility = (Accessibilities)Enum.Parse(typeof(Accessibilities), XmlNodeHelper.GetNodeAttributeValue(node, "Accessibility")),
                ExternallyVisible = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(node, "ExternallyVisible")),
                Static = Convert.ToBoolean(XmlNodeHelper.GetNodeAttributeValue(node, "Static"))
            };
        }
    }
}