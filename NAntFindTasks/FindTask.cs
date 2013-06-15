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
            Project.Log(Level.Info, "Finding package `{0}'...", PackageName);

            var findScript = GetFindScriptContent(PackageName);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(findScript);

            var findProject = new Project(xmlDoc, Level.None, Project.IndentationLevel);

            try
            {
                findProject.Run();

                var packagePath = findProject.Properties["package.path"];

                Project.Properties[PackageName] = packagePath;
                Project.Properties[PackageName + ".found"] = true.ToString();
                Project.Log(Level.Info, packagePath);
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
            var content = File.ReadAllText(GetFindScriptName(packageName));
            content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><project default=\"NAntFindTarget\"><target name=\"NAntFindTarget\">"
                      + content + "</target></project>";
            return content;
        }

        private static string GetFindScriptName(string packageName)
        {
            return "Find" + packageName + ".include";
        }
    }
}
