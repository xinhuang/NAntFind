using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAntFind
{
    [TaskName("find")]
    public class FindTask : Task
    {
        private const string DefaultVersion = "default";
        private const string FindModuleExt = ".xml";

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
                    FindPackage();
                }
                else
                {
                    Project.Log(Level.Info, "Finding file `{0}' in `{1}' {2}...",
                                FileName, Package, Recursive ? "recursively" : "in top level only");
                    FindFile();
                }
            }
            catch (FindException e)
            {
                Project.Log(Level.Warning, e.Message);
                if (Required)
                    throw;
            }
        }

        private void FindPackage()
        {
            string script = GetFindScriptPath(Package, GetSearchPath());
            var package = new Package(File.ReadAllText(script));
            try
            {
                Dictionary<string, string> result = package.Find(Version);
                foreach (var property in result)
                {
                    Project.Properties[property.Key] = property.Value;
                }
            }
            catch (FindException)
            {
                Project.Properties[Package + ".found"] = false.ToString();
                throw;
            }
        }

        private void FindFile()
        {
            string script = GetFindScriptPath(Package, GetSearchPath());
            var package = new Package(File.ReadAllText(script));
            try
            {
                Dictionary<string, string> result = package.FindFile(FileName, Version, Recursive);
                foreach (var property in result)
                {
                    Project.Properties[property.Key] = property.Value;
                }
            }
            catch (FindException)
            {
                Project.Properties[FileName + ".found"] = false.ToString();
                throw;
            }
        }

        private IList<string> GetSearchPath()
        {
            var searchPath = new List<string> {Environment.CurrentDirectory};
            if (!string.IsNullOrWhiteSpace(Project.Properties["find.module.path"]))
                searchPath.AddRange(Project.Properties["find.module.path"]
                                        .Split(new[] {";"},
                                               StringSplitOptions.RemoveEmptyEntries));
            return searchPath;
        }

        private string GetFindScriptPath(string package, IList<string> searchPath)
        {
            string filename = GetFindScriptName(package);
            foreach (var path in searchPath)
            {
                var file = Path.Combine(path, filename);
                if (File.Exists(file))
                    return file;
            }
            var message = "Cannot locate find module " + filename + " under\n";
            foreach (var path in searchPath)
                message += path;
            throw new FileNotFoundException(message);
        }

        private static string GetFindScriptName(string packageName)
        {
            return "Find" + packageName + FindModuleExt;
        }
    }
}