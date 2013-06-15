using System.IO;
using System.Xml;
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
            var findScript = GetFindScriptContent(PackageName);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(findScript);

            var findProject = new Project(xmlDoc, Level.Info, Project.IndentationLevel);

            try
            {
                findProject.Run();
            }
            catch (PackageNotFoundException e)
            {
                if (Required)
                    throw;
                Project.Log(Level.Warning, e.Message);
            }
        }

        private string GetFindScriptContent(string packageName)
        {
            var content = File.ReadAllText("Find" + packageName + ".include");
            content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><project default=\"NAntFindTarget\"><target name=\"NAntFindTarget\">"
                      + content + "</target></project>";
            return content;
        }
    }
}
