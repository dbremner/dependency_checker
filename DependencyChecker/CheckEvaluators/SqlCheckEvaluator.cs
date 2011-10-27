﻿//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;

namespace DependencyChecker.CheckEvaluators
{
    public class SqlCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            var servers = DatabaseCommands.GetSqlServerNameInComputer();
            return servers != null;
        }
    }
}
