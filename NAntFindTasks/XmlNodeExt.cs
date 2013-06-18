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
    }
}