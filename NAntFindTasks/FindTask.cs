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
                                FileName, Package, Recursive ? "recursively" : "only in top level");
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
            Dictionary<string, string> result = package.Find(Version);
            foreach (var property in result)
            {
                Project.Properties[property.Key] = property.Value;
            }
        }

        private void FindFile()
        {
            string script = GetFindScriptPath(Package, GetSearchPath());
            var package = new Package(File.ReadAllText(script));
            Dictionary<string, string> result = package.FindFile(FileName, Version, Recursive);
            foreach (var property in result)
            {
                Project.Properties[property.Key] = property.Value;
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

        private string GetFindScriptPath(string package, IEnumerable<string> searchPath)
        {
            string filename = GetFindScriptName(package);
            string dir = searchPath.First(p => File.Exists(Path.Combine(p, filename)));
            if (string.IsNullOrWhiteSpace(dir))
                throw new FileNotFoundException("Cannot load file " + filename + ", find aborted.");
            return Path.Combine(dir, filename);
        }

        private static string GetFindScriptName(string packageName)
        {
            return "Find" + packageName + ".include";
        }
    }
}