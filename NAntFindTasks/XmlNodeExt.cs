using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NAntFind
{
    internal static class XmlNodeExt
    {
        public static string GetAttributeValue(this XmlNode self, string name)
        {
            if (self.Attributes == null || self.Attributes.Count == 0)
                return string.Empty;
            if (self.Attributes[name] == null)
                return string.Empty;
            return self.Attributes[name].Value;
        }

        public static IEnumerable<XmlNode> GetChildNodes(this XmlElement self, string name)
        {
            return from XmlNode node in self.ChildNodes
                   where node.Name == name
                   select node;
        }
    }
}