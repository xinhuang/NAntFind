using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace NAntFind
{
    public class Package
    {
        private readonly string _name;
        private readonly IDictionary<string, Version> _versions = new Dictionary<string, Version>();

        public Package(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            Debug.Assert(doc.DocumentElement != null);
            Debug.Assert(doc.DocumentElement.Name == "package");
            _name = doc.DocumentElement.Attributes["name"].Value;

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Name == "version")
                {
                    var version = new Version(node);
                    _versions.Add(version.Value, version);
                }
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public IDictionary<string, Version> Versions
        {
            get { return _versions; }
        }
    }
}