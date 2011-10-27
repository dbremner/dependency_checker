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

namespace SampleCheck
{
    public class CheckValueInConfig: ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            return Boolean.Parse(check.Value);
        }
    }
}
