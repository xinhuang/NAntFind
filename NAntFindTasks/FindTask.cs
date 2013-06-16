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
        public FindTask()
        {
            Version = "default";
        }

        [TaskAttribute("package", Required = true)]
        public string Package { get; set; }

        [TaskAttribute("required")]
        public bool Required { get; set; }

        [TaskAttribute("version")]
        public string Version { get; set; }

        protected override void ExecuteTask()
        {
            Project.Log(Level.Info, "Finding package `{0}'...", Package);

            string scriptPath = GetFindScriptPath(Package, GetSearchPath());
            var findDoc = new XmlDocument();
            findDoc.Load(scriptPath);
            XmlNode packageNode = GetPackageNode(findDoc, Package);
            XmlNode version = GetVersionNode(packageNode, Version);
            XmlDocument script = ComposeScript(version.OuterXml);
            var findProject = new Project(script, Level.Warning, Project.IndentationLevel);

            try
            {
                findProject.Run();

                string packagePath = findProject.Properties["package.path"];

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

        private XmlNode GetPackageNode(XmlDocument doc, string package)
        {
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name == "package"
                    && node.Attributes != null
                    && node.Attributes["name"].Value == package)
                    return node;
            }
            throw new FindModuleException(string.Format("Cannot find package {0} in {1}.", 
                package, doc.BaseURI));
        }

        private XmlDocument ComposeScript(string taskXml)
        {
            var document = new XmlDocument();
            string targetName = "find." + Package + ".Ver." + Version;
            string content =
                string.Format(
                    "<?xml version=\"1.0\" encoding=\"utf-8\" ?><project default=\"{0}\"><target name=\"{0}\">{1}</target></project>",
                    targetName, taskXml);
            document.LoadXml(content);
            return document;
        }

        private XmlNode GetVersionNode(XmlNode doc, string version)
        {
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name != "version")
                    continue;
                if (node.Attributes == null || node.Attributes.Count == 0)
                    continue;
                string nodeVersion = node.Attributes["value"].Value;
                if (string.IsNullOrWhiteSpace(nodeVersion))
                    nodeVersion = "default";
                if (nodeVersion == version)
                    return node;
            }
            throw new FindModuleException("Don't know how to find version " + version);
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

        private string GetFindScriptPath(string package, IEnumerable<string> searchPath)
        {
            string filename = GetFindScriptName(package);
            string dir = searchPath.First(p => Exist(p, filename));
            if (string.IsNullOrWhiteSpace(dir))
                throw new FileNotFoundException("Cannot load file " + filename + ", find aborted.");
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