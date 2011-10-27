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
using DependencyChecker.Common;

namespace DependencyChecker.Commands
{
    public class PkgMgrCommand : IDependencySetupCommand
    {
        private readonly string pathToExe;

        public PkgMgrCommand()
        {
            this.pathToExe = Environment.ExpandEnvironmentVariables(@"%windir%\\system32\pkgmgr.exe");
            this.Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            this.Completed = false;

            ProcessStartInfo start = this.CreateProcessStartInfo();
            start.Arguments = dependency.Settings;

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

            this.Completed = true;
        }

        protected ProcessStartInfo CreateProcessStartInfo()
        {
            var start = new ProcessStartInfo
                            {
                                FileName = this.pathToExe, 
                                UseShellExecute = false, 
                                RedirectStandardOutput = true, 
                                CreateNoWindow = true
                            };
            return start;
        }
    }
}