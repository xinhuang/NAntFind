using System.Collections.Generic;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAntFind
{
    [TaskName("package")]
    public class PackageTask : Task
    {
        [TaskAttribute("name")]
        public string PackageName { get; set; }

        private readonly List<Entry> _hints = new List<Entry>();
        
        [BuildElementCollection("hints", "hint")]
        public Entry[] Hints
        {
            set { _hints.AddRange(value); }
        }

        private readonly List<Entry> _names = new List<Entry>();

        [BuildElementCollection("names", "name")]
        public Entry[] Names
        {
            set { _names.AddRange(value); }
        }

        public class Entry : Element
        {
            [TaskAttribute("value")]
            public string Value { get; set; }
        }

        protected override void ExecuteTask()
        {
            
        }
    }
}