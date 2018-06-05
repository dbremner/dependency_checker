//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker.Common
{
    using System;
    using System.Collections.Generic;
    using CheckEvaluators;

    public class EvaluationContext : IEvaluationContext
    {
        private readonly Dictionary<string, ICheckEvaluator> evaluators;

        private readonly ExpressionCheckEvaluator expressionEvaluator;
        private readonly Dictionary<string, Check> namedChecks;

        public EvaluationContext()
        {
            evaluators = new Dictionary<string, ICheckEvaluator>();
            namedChecks = new Dictionary<string, Check>();
            expressionEvaluator = new ExpressionCheckEvaluator();
        }

        public Check this[string name]
        {
            get { return namedChecks[name]; }
            set
            {
                if (namedChecks.ContainsKey(name))
                {
                    namedChecks.Remove(name);
                }
                namedChecks.Add(name, value);
            }
        }

        public bool Evaluate(Check check)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            return GetEvaluatorForCheckType(check.CheckType).Evaluate(check, this);
        }

        public bool Evaluate(string check)
        {
            return expressionEvaluator.Evaluate(check, this);
        }

        public IEnumerable<string> GetCheckNames()
        {
            return namedChecks.Keys;
        }

        public ICheckEvaluator GetEvaluatorForCheckType(string checkType)
        {
            return evaluators[checkType];
        }

        public void SetEvaluatorForCheckType(string checkType, ICheckEvaluator evaluator)
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException();
            }
            evaluators.Add(checkType, evaluator);
        }
    }
}