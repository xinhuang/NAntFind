using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestNAntFind.IntegrationTest
{
    [TestClass]
    public class NAntFindIntegrationTest
    {
        [TestMethod]
        public void should_find_visual_studio_110_from_registry()
        {
            Run("TestFindFromRegistry.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_hint()
        {
            Run("TestFindFromHint.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_env()
        {
            Run("TestFindFromEnv.build");
        }

        [TestMethod]
        public void should_find_devenv_in_visual_studio_110()
        {
            Run("TestFindExistingFile.build");
        }

        [TestMethod]
        public void given_no_version_specified_should_return_default_version()
        {
            Run("test.build");
        }

        private static void Run(string script)
        {
            var process = new Process
                {
                    StartInfo =
                        {
                            FileName = "nant.exe",
                            Arguments = string.Format(@"-nologo /f:{0} -v", script),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true,
                            WorkingDirectory = Environment.CurrentDirectory
                        }
                };

            process.Start();
            process.WaitForExit();

            Assert.AreEqual(0, process.ExitCode, process.StandardOutput.ReadToEnd());
        }
    }
}
