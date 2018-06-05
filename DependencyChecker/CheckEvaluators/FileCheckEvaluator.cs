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

namespace DependencyChecker.CheckEvaluators
{
    public class FileCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            return System.IO.File.Exists(check.Value);
        }
    }
}
