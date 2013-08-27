using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.Win32;

namespace NAntFind
{
    public class Hints : IEnumerable<string>
    {
        private readonly List<string> _values = new List<string>();

        public void AddPath(string value)
        {
            value = value.Trim();
            if (string.IsNullOrEmpty(value) || _values.Contains(value))
                return;
            _values.Add(value);
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
                hints.AddPath(GetPath(node, hints));
            }

            return hints;
        }

        private static string GetPath(XmlNode node, Hints hints)
        {
            Debug.Assert(node.Attributes != null);
            if (node.Attributes["value"] != null)
                return node.GetAttributeValue("value");
            if (node.Attributes["key"] != null && node.Attributes["name"] != null)
            {
                var key = node.GetAttributeValue("key").Trim();
                var name = node.GetAttributeValue("name").Trim();
                try
                {
                    return Registry.GetValue(key, name, String.Empty).ToString();
                }
                catch (NullReferenceException)
                {
                    return string.Empty;
                }
            }
            if (node.Attributes["env"] != null)
                return Environment.GetEnvironmentVariable(node.Attributes["env"].Value) ?? string.Empty;
            throw new Exception("Invalid find module. Context: `" + node.InnerXml + "'");
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