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
    public class PropNegation : PropExpression
    {
        public PropNegation(PropExpression inner)
        {
            Inner = inner;
        }

        public PropExpression Inner { get; }
    }
}