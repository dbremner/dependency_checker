//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


using System;
using DependencyChecker.CheckEvaluators.Helpers;
using DependencyChecker.Common;

namespace DependencyChecker.CheckEvaluators
{
    using CheckEvaluators.Helpers;

    public class SoftwareCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null) throw new ArgumentNullException(nameof(check));
            return 
                RegistryHelper.IsInKey(
                    @"SOFTWARE\microsoft\Windows\CurrentVersion\Uninstall", 
                    "DisplayName", 
                    check.Value);
        }
    }
}