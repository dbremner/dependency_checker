//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using DependencyChecker.Common;

namespace SampleCheck
{
    public class CheckStaticBoolean: ICheckEvaluator
    {
        public static bool IsDependencySatisfied { get; set; }

        static CheckStaticBoolean()
        {
            IsDependencySatisfied = false;
        }

        public bool Evaluate(Check check, IEvaluationContext context)
        {
            return IsDependencySatisfied;
        }
    }
}