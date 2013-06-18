using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace NAntFind
{
    public class Version
    {
        private readonly string _value;
        private readonly List<string> _hints;
        private readonly List<string> _names;

        public Version(XmlNode node)
        {
            Debug.Assert(node.Attributes != null);
            Debug.Assert(node.Attributes["value"] != null);
            _value = node.Attributes["value"].Value;

            _hints = ParseValues(node["hints"]);
            _names = ParseValues(node["names"]);
        }

        private List<string> ParseValues(XmlElement element)
        {
            if (element == null)
                return new List<string>();
            return (from XmlNode node in element.ChildNodes 
                    where node.Attributes != null && node.Attributes["value"] != null 
                    select node.Attributes["value"].Value).ToList();
        }

        public string Value
        {
            get { return _value; }
        }

        public List<string> Hints
        {
            get { return _hints; }
        }

        public List<string> Names
        {
            get { return _names; }
        }
    }
}