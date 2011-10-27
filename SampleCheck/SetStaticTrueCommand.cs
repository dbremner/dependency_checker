//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System.Diagnostics;
using DependencyChecker.Common;

namespace SampleCheck
{
    public class SetStaticTrueCommand : IDependencySetupCommand
    {
        public SetStaticTrueCommand()
        {
            Completed = false;
        }

        public bool Completed { get; set; }

        public void Execute(Dependency dependency)
        {
            Debug.WriteLine("We setup a required dependency!");

            Completed = CheckStaticBoolean.IsDependencySatisfied = true;
        }
    }
}