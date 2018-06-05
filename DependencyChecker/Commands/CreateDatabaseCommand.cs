//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;
using System.Linq;
using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;

namespace DependencyChecker.Commands
{
    public class CreateDatabaseCommand : IDependencySetupCommand
    {
        public CreateDatabaseCommand()
        {
            Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }
            var settings = dependency.Settings
                .Split('!')
                .Where(pair => pair.IndexOf('=') < pair.Length - 1)
                .ToDictionary(pair => pair.Split('=')[0], pair => pair.Split('=')[1]);

            if(!settings.ContainsKey("alias"))
            {
                throw new ArgumentException("Expected a value for 'alias' specifying the SQL Server in the settings for " + GetType().Name);
            }

            if (!settings.ContainsKey("db"))
            {
                throw new ArgumentException("Expected a value for 'db' specifying the SQL Server in the settings for " + GetType().Name);
            }

            (new DatabaseCommands(settings["alias"], settings["db"])).CreateDb();
            Completed = true;
        }
    }
}
