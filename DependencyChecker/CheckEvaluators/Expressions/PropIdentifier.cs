﻿//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker.CheckEvaluators.Expressions
{
    public class PropIdentifier : PropExpression
    {
        public PropIdentifier(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}