using System.Xml;

namespace ViewAnalysis.Utilities
{
    internal static class XmlNodeHelper
    {
        internal static string GetNodeAttributeValue(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName]?.Value;
        }
    }
}