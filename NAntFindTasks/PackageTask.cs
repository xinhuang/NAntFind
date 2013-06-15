using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAntFind
{
    [TaskName("package")]
    public class PackageTask : Task
    {
        private readonly List<Entry> _hints = new List<Entry>();
        private readonly List<Entry> _names = new List<Entry>();

        public class Entry : Element
        {
            [TaskAttribute("value")]
            public string Value { get; set; }
        }

        [TaskAttribute("name")]
        public string PackageName { get; set; }

        [BuildElementCollection("hints", "hint")]
        public Entry[] Hints
        {
            set { _hints.AddRange(value); }
        }

        [BuildElementCollection("names", "name")]
        public Entry[] Names
        {
            set { _names.AddRange(value); }
        }

        protected override void ExecuteTask()
        {
            var hints = (from hint in _hints
                         where Directory.Exists(hint.Value)
                         select hint).ToList();

            if (!hints.Any())
                throw new PackageNotFoundException("None of given hints exists. Search aborted.");

            foreach (var hint in hints)
            {
                if (!_names.All(name => Exist(hint, name))) 
                    continue;

                Project.Properties["package.path"] = hint.Value;
                return;
            }

            throw new PackageNotFoundException(NotFoundMessage());
        }

        private string NotFoundMessage()
        {
            const int DisplayEntryMax = 5;
            string files = string.Join(", ", _names.Take(DisplayEntryMax).Select(o => o.Value));
            if (_names.Count > DisplayEntryMax)
                files += "...";
            string path = string.Join("\n    ", _hints.Take(DisplayEntryMax).Select(o => o.Value));
            if (_hints.Count > DisplayEntryMax)
                path += "\n...";
            return string.Format("Searching for files\n    {0}\nunder path\n    {1}\nfailed.", files, path);
        }

        private static bool Exist(Entry hint, Entry name)
        {
            var file = Path.Combine(hint.Value, name.Value);
            return File.Exists(file);
        }
    }
}