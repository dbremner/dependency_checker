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
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class PropExpression
    {
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class PropDisjunction : PropExpression
    {
        public PropDisjunction(PropExpression left, PropExpression right)
        {
            this.Left = left;
            this.Right = right;
        }

        public PropExpression Left { get; set; }

        public PropExpression Right { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class PropNegation : PropExpression
    {
        public PropNegation(PropExpression inner)
        {
            this.Inner = inner;
        }

        public PropExpression Inner { get; set; }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class PropIdentifier : PropExpression
    {
        public PropIdentifier(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}