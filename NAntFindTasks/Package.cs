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
        private readonly string _defaultVersion;

        public Package(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            Debug.Assert(doc.DocumentElement != null);
            Debug.Assert(doc.DocumentElement.Name == "package");
            Debug.Assert(doc.DocumentElement.Attributes["name"] != null);
            _name = doc.DocumentElement.GetAttribute("name");

            _defaultVersion = doc.DocumentElement.GetAttribute("default");

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

        public string DefaultVersion
        {
            get { return _defaultVersion; }
        }

        public Dictionary<string, string> FindFile(string file, string package, string ver, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(ver))
                ver = DefaultVersion;
            if (!Versions.ContainsKey(ver))
                throw new FindModuleException(String.Format("Dont know how to find `{0}' in {1} Ver {2}",
                                                            file, package, ver));

            var version = Versions[ver];
            return version.FindFile(file, recursive);
        }
    }
}