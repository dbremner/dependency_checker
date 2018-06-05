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
            pathToExe = Environment.ExpandEnvironmentVariables(@"%windir%\\system32\pkgmgr.exe");
            Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }
            Completed = false;

            ProcessStartInfo start = CreateProcessStartInfo();
            start.Arguments = dependency.Settings;

            using (var process = Process.Start(start))
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

            Completed = true;
        }

        protected ProcessStartInfo CreateProcessStartInfo()
        {
            var start = new ProcessStartInfo
                            {
                                FileName = pathToExe,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            };
            return start;
        }
    }
}