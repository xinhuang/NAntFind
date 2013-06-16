using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAntFind
{
    [TaskName("find")]
    public class FindTask : Task
    {
        [TaskAttribute("package", Required = true)]
        public string Package { get; set; }

        [TaskAttribute("required")]
        public bool Required { get; set; }

        [TaskAttribute("version")]
        public string Version { get; set; }

        protected override void ExecuteTask()
        {
            Project.Log(Level.Info, "Finding package `{0}'...", Package);

            var findScript = GetFindScriptContent(Package, GetSearchPath());
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(findScript);

            var findProject = new Project(xmlDoc, Level.Info, Project.IndentationLevel);

            try
            {
                findProject.Run();

                var packagePath = findProject.Properties["package.path"];

                Project.Properties[Package] = packagePath;
                Project.Properties[Package + ".found"] = true.ToString();
                Project.Log(Level.Info, packagePath);
            }
            catch (PackageNotFoundException e)
            {
                if (Required)
                    throw;
                Project.Log(Level.Warning, e.Message);
            }
        }

        private IEnumerable<string> GetSearchPath()
        {
            var searchPath = new List<string> {Environment.CurrentDirectory};
            if (!string.IsNullOrWhiteSpace(Project.Properties["find.module.path"]))
                searchPath.AddRange(Project.Properties["find.module.path"]
                                        .Split(new[] {";"},
                                               StringSplitOptions.RemoveEmptyEntries));
            return searchPath;
        }

        private string GetFindScriptContent(string package, IEnumerable<string> searchPath)
        {
            var findFile = GetFindScriptPath(package, searchPath);
            var content = File.ReadAllText(findFile);
            content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><project default=\"NAntFindTarget\"><target name=\"NAntFindTarget\">"
                      + content + "</target></project>";
            return content;
        }

        private string GetFindScriptPath(string package, IEnumerable<string> searchPath)
        {
            var filename = GetFindScriptName(package);
            var dir = searchPath.First(p => Exist(p, filename));
            if (string.IsNullOrWhiteSpace(dir))
                throw new FileNotFoundException("Cannot load file " + filename +", find aborted.");
            return Path.Combine(dir, filename);
        }

        private bool Exist(string path, string filename)
        {
            return File.Exists(Path.Combine(path, filename));
        }

        private static string GetFindScriptName(string packageName)
        {
            return "Find" + packageName + ".include";
        }
    }
}
