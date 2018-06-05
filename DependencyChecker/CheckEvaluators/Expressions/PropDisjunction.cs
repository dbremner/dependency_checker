//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker.CheckEvaluators.Expressions
{
    public class PropDisjunction : PropExpression
    {
        public PropDisjunction(PropExpression left, PropExpression right)
        {
            Left = left;
            Right = right;
        }

        public PropExpression Left { get; }

        public PropExpression Right { get; }
    }
}