using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace NAntFind
{
    public class Package
    {
        private readonly string _defaultVersion;
        private readonly string _name;
        private readonly IDictionary<string, Version> _versions = new Dictionary<string, Version>();

        public Package(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            Debug.Assert(doc.DocumentElement != null);
            Debug.Assert(doc.DocumentElement.Name == "package");
            Debug.Assert(doc.DocumentElement.Attributes["name"] != null);
            _name = doc.DocumentElement.GetAttribute("name");

            _defaultVersion = doc.DocumentElement.GetAttribute("default");
            if (string.IsNullOrWhiteSpace(_defaultVersion))
                _defaultVersion = Version.Unspecified;

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

        public Dictionary<string, string> FindFile(string file, string ver, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(ver))
                ver = DefaultVersion;
            if (!Versions.ContainsKey(ver))
                throw new FindModuleException(String.Format("Don't know how to find `{0}' of Ver {1}",
                                                            file, ver));

            Version version = Versions[ver];
            return version.FindFile(file, recursive);
        }

        public Dictionary<string, string> Find(string ver)
        {
            if (string.IsNullOrWhiteSpace(ver))
                ver = DefaultVersion;
            if (!Versions.ContainsKey(ver))
                throw new FindModuleException("Dont know how to find Ver {2}" + ver);

            string path = Versions[ver].Find();
            return new Dictionary<string, string>
            {
                {Name, path},
                {Name + ".found", true.ToString()},
                {Name + ".version", ver},
            };
        }
    }
}