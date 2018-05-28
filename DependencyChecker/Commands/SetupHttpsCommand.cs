//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;

namespace DependencyChecker.Commands
{
    public class SetupHttpsCommand : IDependencySetupCommand
    {
        public SetupHttpsCommand()
        {
            this.Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            var appCmd = new AppCmd();
            appCmd.SetHttpsBinding();
            this.Completed = true;
        }
    }
}