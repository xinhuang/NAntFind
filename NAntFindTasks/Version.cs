using System.Diagnostics;
using System.Xml;

namespace NAntFind
{
    public class Version
    {
        private readonly string _value;

        public Version(XmlNode node)
        {
            Debug.Assert(node.Attributes != null);
            Debug.Assert(node.Attributes["value"] != null);
            _value = node.Attributes["value"].Value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}