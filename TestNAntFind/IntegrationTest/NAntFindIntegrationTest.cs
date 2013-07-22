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
        public void a_big_test()
        {
            Run(@"NAnt.build");
        }

        private static void Run(string script)
        {
            string nantPath = Path.Combine(Environment.CurrentDirectory, @"nant.exe");
            var process = new Process();
            process.StartInfo.FileName = nantPath;
            process.StartInfo.Arguments = string.Format(@"-nologo -ext:NAntFindTasks.dll /f:{0}", script);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

            process.Start();
            process.WaitForExit();

            Assert.AreEqual(0, process.ExitCode, process.StandardOutput.ReadToEnd());
        }
    }
}
