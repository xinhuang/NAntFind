using System;
using System.Diagnostics;
using System.Text;

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
            using (var process = new Process())
            {
                process.StartInfo.FileName = "nant.exe";
                process.StartInfo.Arguments = string.Format(@"-nologo /f:{0}", script);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

                var buffer = new StringBuilder(4096);
                var handler = CreateDataReceivedHandler(buffer);

                process.OutputDataReceived += handler;
                process.ErrorDataReceived += handler;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                process.OutputDataReceived -= handler;
                process.ErrorDataReceived -= handler;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, process.ExitCode,
                                                                             buffer.ToString());
            }
        }

        static DataReceivedEventHandler CreateDataReceivedHandler(StringBuilder buffer)
        {
            return (sender, e) =>
            {
                lock (buffer)
                    buffer.AppendLine(e.Data);
            };
        }
    }
}
