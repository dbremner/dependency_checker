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

namespace DependencyChecker.CheckEvaluators
{
    public class DatabaseCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            var settings = check.Value
                .Split('!')
                .Where(pair => pair.IndexOf('=') < pair.Length - 1)
                .ToDictionary(pair => pair.Split('=')[0], pair => pair.Split('=')[1]);

            if (!settings.ContainsKey("alias"))
            {
                throw new ArgumentException("Expected a value for 'alias' specifying the SQL Server in the value for " +
                                            GetType().Name);
            }

            if (!settings.ContainsKey("db"))
            {
                throw new ArgumentException("Expected a value for 'db' specifying the SQL Server in the value for " +
                                            GetType().Name);
            }

            var db = new DatabaseCommands(settings["alias"], settings["db"]);

            return db.ExistsDb();
        }
    }
}