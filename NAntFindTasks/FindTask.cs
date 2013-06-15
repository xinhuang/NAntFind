using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAntFind
{
    [TaskName("find")]
    public class FindTask : Task
    {
        [TaskAttribute("package", Required = true)]
        public string PackageName { get; set; }

        [TaskAttribute("required")]
        public bool Required { get; set; }

        protected override void ExecuteTask()
        {
            
        }
    }
}
