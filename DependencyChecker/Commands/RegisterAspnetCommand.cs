//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;
using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;

namespace DependencyChecker.Commands
{
    public class RegisterAspnetCommand : IDependencySetupCommand
    {
        public RegisterAspnetCommand() => Completed = false;

        public bool Completed
        {
            get;
            private set;
        }

        public void Execute(Dependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }
            dependency.Settings = "/iu:NetFx3;WCF-HTTP-Activation";

            var pkgMgrCmd = new PkgMgrCommand();
            pkgMgrCmd.Execute(dependency);

            (new RegisterAspNetInIIS()).Execute();

            Completed = true;
        }
    }
}
