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

namespace DependencyChecker.CheckEvaluators
{
    public class MultiCertificateCheckEvaluator : CertificateCheckEvaluator
    {
        public override bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            var checks = check.Value.Split('!');
            foreach (var c in checks)
            {
                var tempCheck = new Check { Value = c };
                if (!base.Evaluate(tempCheck, context))
                {
                    return false;
                }
            }

            return true;
        }
    }
}