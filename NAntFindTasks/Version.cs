using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public Dictionary<string, string> FindFile(string file, bool recursive)
        {
            return RecursiveFind(file, recursive);
        }

        private Dictionary<string, string> RecursiveFind(string file, bool recursive)
        {
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var hint in Hints.Where(Directory.Exists))
            {
                foreach (var path in Directory.EnumerateFiles(hint, file, searchOption))
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
            foreach (var hint in Hints.Where(Directory.Exists))
            {
                bool found = true;
                foreach (var name in Names)
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