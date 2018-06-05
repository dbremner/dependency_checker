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
    using System.Management;
    using CheckEvaluators.Helpers;

    public class WmiCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            ManagementObjectSearcher searcher = WmiHelper.RunWmiQuery(check.Value);
            if (searcher.Get().Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}