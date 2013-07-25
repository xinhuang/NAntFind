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

        internal string Name
        {
            get { return _name; }
        }

        internal IDictionary<string, Version> Versions
        {
            get { return _versions; }
        }

        private string DefaultVersion
        {
            get { return _defaultVersion; }
        }

        public FindResult FindFile(string file, string ver, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(ver))
                ver = DefaultVersion;

            Version targetVersion;
            if (!Versions.TryGetValue(ver, out targetVersion))
                throw new FindModuleException("Don't know how to find version " + ver);

            return new FindResult()
                {
                    Path = targetVersion.FindFile(file, recursive),
                    Version = targetVersion.Value,
                };
        }

        public FindResult Find(string ver)
        {
            if (string.IsNullOrWhiteSpace(ver))
                return FindExactly(DefaultVersion);
            return FindExactly(ver);
        }

        private FindResult FindExactly(string ver)
        {
            Version targetVersion;
            if (!Versions.TryGetValue(ver, out targetVersion))
                throw new FindModuleException("Don't know how to find version " + ver);

            return new FindResult()
            {
                Path = targetVersion.Find(),
                Version = targetVersion.Value,
            };
        }
    }
}