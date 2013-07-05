using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Win32;

namespace NAntFind
{
    public class Hints : IEnumerable<string>
    {
        private readonly List<string> _values = new List<string>();

        public void AddPath(string value)
        {
            _values.Add(value);
        }

        public void AddRegistry(string key, string name)
        {
            _values.Add(Registry.GetValue(key, name, String.Empty).ToString());
        }

        public int Count { get { return _values.Count; } }

        public static Hints Parse(XmlElement element)
        {
            var hints = new Hints();
            if (element == null)
                return hints;

            var hintNodes = element.GetChildNodes("hint");
            foreach (var node in hintNodes)
            {
                var dir = node.GetAttributeValue("value").Trim();
                if (String.IsNullOrEmpty(dir))
                {
                    var key = node.GetAttributeValue("key").Trim();
                    var name = node.GetAttributeValue("name").Trim();
                    hints.AddRegistry(key, name);
                }
                if (!String.IsNullOrEmpty(dir))
                    hints.AddPath(dir);
            }

            return hints;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}