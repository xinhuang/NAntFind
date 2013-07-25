using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace NAntFind
{
    public class Version
    {
        public const string Unspecified = "default";
        private readonly Hints _hints;
        private readonly List<string> _names;
        private readonly string _value;

        public Version(XmlNode node)
        {
            if (node.Attributes != null && node.Attributes["value"] != null)
                _value = node.Attributes["value"].Value;
            else
                _value = Unspecified;

            _hints = Hints.Parse(node["hints"]);
            _names = ParseNames(node["names"]);
        }

        public string Value { get { return _value; } }
        public Hints Hints { get { return _hints; } }
        public List<string> Names { get { return _names; } }

        private static List<string> ParseNames(XmlElement element)
        {
            if (element == null)
                return new List<string>();
            return (from XmlNode node in element.ChildNodes
                    where node.Attributes != null && node.Attributes["value"] != null
                    select node.Attributes["value"].Value).ToList();
        }

        public string Find()
        {
            string result;
            if (!TryFind(out result))
                throw new PackageNotFoundException("Package of Version " + Value + " cannot be found.");
            return result;
        }

        private bool TryFind(out string path)
        {
            path = string.Empty;
            foreach (string hint in Hints.Where(Directory.Exists))
            {
                bool found = true;
                foreach (string name in Names)
                {
                    if (!File.Exists(Path.Combine(hint, name)))
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    path = hint;
                    return true;
                }
            }
            return false;
        }

        public string FindFile(string file, bool recursive)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (string hint in Hints.Where(Directory.Exists))
            {
                foreach (string path in Directory.EnumerateFiles(hint, file, searchOption))
                {
                    return path;
                }
            }
            throw new FileNotFoundException(file + " cannot be found.");
        }
    }
}