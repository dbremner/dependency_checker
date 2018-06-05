//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;
using System.Diagnostics;
using System.IO;

namespace DependencyChecker.SystemIntegration
{
    public class AppCmd : WrappedProcessBase
    {
        private readonly string pathToAppCmdExe;

        public AppCmd()
        {
            pathToAppCmdExe = Environment.ExpandEnvironmentVariables(@"%windir%\system32\inetsrv\appcmd");
        }

        public void LoadDefaultUserProfile()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "set apppool \"ASP.NET v4.0\" /processModel.loadUserProfile:true";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new InvalidOperationException(result);
                    }
                }
            }
        }

        public bool IsDefaultUserProfileEnabled()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "list apppools /name:\"ASP.NET v4.0\" /processModel.loadUserProfile:true";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("APPPOOL");
                }
            }
        }

        public void CreateWebApplication(string applicationPath, string applicationName)
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            string args = string.Format("add app /site.name:\"Default web site\" /path:\"/{1}\" /physicalPath:\"{0}\\{1}\"", applicationPath, applicationName);
            start.Arguments = args;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new InvalidOperationException(result);
                    }
                }
            }
        }

        public bool ExistsApplication(string applicationName)
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "list app";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains(applicationName);
                }
            }
        }

        public bool IsHttpsEnabled()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "list site /site.name:\"Default Web Site\"";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("https");
                }
            }
        }

        public void SetHttpsBinding()
        {
            (new NetSh()).Execute();

            var start = CreateProcessStartInfo(pathToAppCmdExe);
            start.Arguments = "set config -section:system.applicationHost/sites /+\"[name='Default Web Site'].bindings.[protocol='https',bindingInformation='*:443:']\" /commit:apphost";
            using (var process = Process.Start(start))
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new InvalidOperationException(result);
                    }
                }
            }
        }
    }
}