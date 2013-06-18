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
        private const string DefaultVersion = "default";

        public FindTask()
        {
            Recursive = true;
        }

        [TaskAttribute("package", Required = true)]
        public string Package { get; set; }

        [TaskAttribute("required")]
        public bool Required { get; set; }

        [TaskAttribute("version")]
        public string Version { get; set; }

        [TaskAttribute("file")]
        public string FileName { get; set; }

        [TaskAttribute("recursive")]
        public bool Recursive { get; set; }

        protected override void ExecuteTask()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FileName))
                {
                    Project.Log(Level.Info, "Finding package `{0}'...", Package);
                    var script = GetScriptDocument();
                    Run(script);
                }
                else
                {
                    Project.Log(Level.Info, "Finding file `{0}' in `{1}' {2}...",
                                FileName, Package, Recursive ? "recursively" : "only in top level");
                    FindFile();
                }
            }
            catch (FileNotFoundException e)
            {
                if (Required)
                    throw;
                Project.Log(Level.Warning, e.Message);
            }
        }

        private void FindFile()
        {
            string script = GetFindScriptPath(Package, GetSearchPath());
            var package = new Package(File.ReadAllText(script));
            Dictionary<string, string> result = package.FindFile(FileName, Package, Version, Recursive);
            foreach (var property in result)
            {
                Project.Properties[property.Key] = property.Value;
            }
        }

        private void Run(XmlDocument script)
        {
            var project = new Project(script, Level.Warning, Project.IndentationLevel);

            try
            {
                project.Run();

                string packagePath = project.Properties["package.path"];

                Project.Properties[Package] = packagePath;
                Project.Properties[Package + ".found"] = true.ToString();
                Project.Properties[Package + ".version"] = Version == DefaultVersion ? "N/A" : Version;
                Project.Log(Level.Info, packagePath);
            }
            catch (PackageNotFoundException e)
            {
                if (Required)
                    throw;
                Project.Log(Level.Warning, e.Message);
            }
        }

        private XmlDocument GetScriptDocument()
        {
            var versionNode = GetTargetVersionNode();
            XmlDocument script = ComposeScript(versionNode.OuterXml);

            return script;
        }

        private XmlNode GetTargetVersionNode()
        {
            string scriptPath = GetFindScriptPath(Package, GetSearchPath());

            var findDoc = new XmlDocument();
            findDoc.Load(scriptPath);

            XmlNode packageNode = GetPackageNode(findDoc, Package);
            if (string.IsNullOrWhiteSpace(Version))
                Version = GetPackageDefaultVersion(packageNode);

            XmlNode versionNode = GetVersionNode(packageNode, Version);
            return versionNode;
        }

        private string GetPackageDefaultVersion(XmlNode packageNode)
        {
            string result = packageNode.GetAttributeValue("default");
            return !string.IsNullOrWhiteSpace(result) ? result : DefaultVersion;
        }

        private XmlNode GetPackageNode(XmlDocument doc, string package)
        {
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name == "package"
                    && node.GetAttributeValue("name") == package)
                    return node;
            }
            throw new FindModuleException(string.Format("Cannot find package {0} in {1}.", 
                package, doc.BaseURI));
        }

        private XmlDocument ComposeScript(string taskXml)
        {
            var document = new XmlDocument();
            string targetName = "find." + Package + ".ver." + Version;
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
                var nodeVersion = node.GetAttributeValue("value");
                if (string.IsNullOrWhiteSpace(nodeVersion) && version == DefaultVersion)
                    return node;
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