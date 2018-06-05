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
    public class PropConjunction : PropExpression
    {
        public PropConjunction(PropExpression left, PropExpression right)
        {
            this.Left = left;
            this.Right = right;
        }

        public PropExpression Left { get; }

        public PropExpression Right { get; }
    }
}