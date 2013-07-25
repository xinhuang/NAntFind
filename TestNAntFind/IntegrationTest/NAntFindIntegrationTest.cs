using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestNAntFind.IntegrationTest
{
    [TestClass]
    public class NAntFindIntegrationTest
    {
        [TestMethod]
        public void should_find_visual_studio_110_from_registry()
        {
            Assert("TestFindFromRegistry.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_hint()
        {
            Assert("TestFindFromHint.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_env()
        {
            Assert("TestFindFromEnv.build");
        }

        [TestMethod]
        public void should_find_devenv_in_visual_studio_110()
        {
            Assert("TestFindExistingFile.build");
        }

        [TestMethod]
        public void given_no_version_specified_should_return_default_version()
        {
            Assert("GivenNoVersionShouldReturnDefault.build");
        }

        [TestMethod]
        public void given_no_version_specified_and_no_default_should_return_default_version()
        {
            AssertFail("GivenNoVersionSpecifiedAndDefaultMissingShouldFail.build");
        }

        private void AssertFail(string script)
        {
            AssertExitCode(1, script);
        }

        private static void Assert(string script)
        {
            AssertExitCode(0, script);
        }

        private static void AssertExitCode(int expected, string script)
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

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, process.ExitCode,
                                                                         process.StandardOutput.ReadToEnd());
        }
    }
}
