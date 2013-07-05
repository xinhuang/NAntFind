using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Win32;

namespace NAntFind
{
    public class Version
    {
        public const string Unspecified = "default";
        private readonly List<string> _hints;
        private readonly List<string> _names;
        private readonly string _value;

        public Version(XmlNode node)
        {
            if (node.Attributes != null && node.Attributes["value"] != null)
                _value = node.Attributes["value"].Value;
            else
                _value = Unspecified;

            _hints = ParseValues(node["hints"]);
            _names = ParseValues(node["names"]);
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

        private List<string> ParseValues(XmlElement element)
        {
            if (element == null)
                return new List<string>();

            var hintNodes = GetChildNodes(element, "hint");
            var result = new List<string>();
            foreach (var hintNode in hintNodes)
            {
                var dir = hintNode.GetAttributeValue("value").Trim();
                if (string.IsNullOrEmpty(dir))
                {
                    var key = hintNode.GetAttributeValue("key").Trim();
                    var name = hintNode.GetAttributeValue("name").Trim();
                    dir = Registry.GetValue(key, name, string.Empty).ToString().Trim();
                }
                if (!string.IsNullOrEmpty(dir))
                    result.Add(dir);
            }

            return result;
        }

        private IEnumerable<XmlNode> GetChildNodes(XmlElement element, string name)
        {
            return from XmlNode node in element.ChildNodes
                   where node.Name == name
                   select node;
        }

        public Dictionary<string, string> FindFile(string file, bool recursive)
        {
            return RecursiveFind(file, recursive);
        }

        private Dictionary<string, string> RecursiveFind(string file, bool recursive)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (string hint in Hints.Where(Directory.Exists))
            {
                foreach (string path in Directory.EnumerateFiles(hint, file, searchOption))
                {
                    return MakeResult(file, path);
                }
            }
            throw new FileNotFoundException(file + " cannot be found.");
        }

        private Dictionary<string, string> MakeResult(string file, string path)
        {
            return new Dictionary<string, string>
            {
                {file, path},
                {file + ".found", true.ToString()},
                {file + ".version", Value}
            };
        }

        public string Find()
        {
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
                    return hint;
            }
            throw new PackageNotFoundException("Package of Version " + Value + " cannot be found.");
        }
    }
}