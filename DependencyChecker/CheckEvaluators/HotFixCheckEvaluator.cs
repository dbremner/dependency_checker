﻿//===============================================================================
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

    public class HotFixCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            return InstalledHotFixes.IsHotFixInstalled(check.Value);
        }
    }
}