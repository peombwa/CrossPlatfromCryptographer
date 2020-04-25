using System;
using System.Diagnostics;

namespace CrossPlatformCryptographer
{
    public static class MacOSSecurityCommand
    {
        private const string ServiceName = "graph_powershell_sdk";
        public static void Add(string appId, string plainText)
        {
            using (Process securityCmd = new Process())
            {
                securityCmd.StartInfo.FileName = "security";
                securityCmd.StartInfo.Arguments = $"add-generic-password -a {appId} -s {ServiceName} -w {plainText} -U -T";
                securityCmd.StartInfo.UseShellExecute = false;
                securityCmd.StartInfo.RedirectStandardOutput = true;
                securityCmd.StartInfo.RedirectStandardError = true;
                securityCmd.Start();
                securityCmd.WaitForExit();
            }
        }

        public static string Get(string appId)
        {
            using (Process securityCmd = new Process())
            {
                Console.WriteLine($"Executing: security find-generic-password -a {appId} -s {ServiceName} -w");
                securityCmd.StartInfo.FileName = "security";
                securityCmd.StartInfo.Arguments = $"find-generic-password -a {appId} -s {ServiceName} -w";
                securityCmd.StartInfo.UseShellExecute = false;
                securityCmd.StartInfo.RedirectStandardOutput = true;
                securityCmd.StartInfo.RedirectStandardError = true;
                securityCmd.Start();

                string decrptedText = securityCmd.StandardOutput.ReadToEnd();
                Console.WriteLine(decrptedText);

                securityCmd.WaitForExit();
                return decrptedText;
            }
        }

        public static void Remove(string appId)
        {
            using (Process securityCmd = new Process())
            {
                securityCmd.StartInfo.FileName = "security";
                securityCmd.StartInfo.Arguments = $"delete-generic-password -a {appId} -s {ServiceName}";
                securityCmd.StartInfo.UseShellExecute = false;
                securityCmd.StartInfo.RedirectStandardOutput = true;
                securityCmd.StartInfo.RedirectStandardError = true;
                securityCmd.Start();
                securityCmd.WaitForExit();
            }
        }
    }
}
