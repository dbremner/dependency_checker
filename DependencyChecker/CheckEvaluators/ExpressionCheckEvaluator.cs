﻿//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using DependencyChecker.CheckEvaluators.Expressions;
using DependencyChecker.Common;

namespace DependencyChecker.CheckEvaluators
{
    using System;
    using CheckEvaluators.Expressions;

    public class ExpressionCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            return this.Evaluate(check.Value, context);
        }

        public bool Evaluate(string check, IEvaluationContext context)
        {
            return this.Interpret(PropLogicParser.Parse(check), context);
        }

        private bool Interpret(PropExpression e, IEvaluationContext context)
        {
            switch (e)
            {
                case PropIdentifier pi:
                    return context.Evaluate(context[pi.Name]);
                case PropConjunction pc:
                    return this.Interpret(pc.Left, context) && this.Interpret(pc.Right, context);
                case PropDisjunction pd:
                    return this.Interpret(pd.Left, context) || this.Interpret(pd.Right, context);
                case PropNegation pn:
                    return !this.Interpret(pn.Inner, context);
            }

            throw new NotImplementedException();
        }
    }
}