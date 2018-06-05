//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;

namespace DependencyChecker.UnitTests
{
    using System.Collections.Generic;
    using DependencyChecker.Common;

    public class MockCheckEvaluator : ICheckEvaluator
    {
        public MockCheckEvaluator()
        {
            ReturnValues = new Dictionary<string, bool>();
            HitsPerCheck = new Dictionary<string, int>();
        }

        public IDictionary<string, int> HitsPerCheck { get; }

        public IDictionary<string, bool> ReturnValues { get; }

        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            HitsPerCheck[check.Name]++;
            return ReturnValues[check.Name];
        }

        public void ResetHitsPerCheck()
        {
            HitsPerCheck.Clear();
            foreach (string name in ReturnValues.Keys)
            {
                HitsPerCheck.Add(name, 0);
            }
        }
    }
}