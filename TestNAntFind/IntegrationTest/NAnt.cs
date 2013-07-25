using System;
using System.Diagnostics;

namespace TestNAntFind.IntegrationTest
{
    static class NAnt
    {
        public static void AssertFail(string script)
        {
            AssertExitCode(1, script);
        }

        public static void Assert(string script)
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