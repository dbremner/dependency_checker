//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.UnitTests
{
    using DependencyChecker.Common;
    using CheckEvaluators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MultiCertificateCheckEvaluatorTest
    {
        [TestMethod]
        public void ShouldEvaluateToTrue()
        {
            var evaluator = new MultiCertificateCheckEvaluator();
            Check check = new Check { Name = "Certficate", Value = "LocalMachine,My,CN=adatum!LocalMachine,My,CN=localhost" };

            bool evaluate = evaluator.Evaluate(check, null);

            Assert.IsTrue(evaluate);
        }

        [TestMethod]
        public void ShouldEvaluateToFalse()
        {
            var evaluator = new MultiCertificateCheckEvaluator();
            Check check = new Check { Name = "Certficate", Value = "LocalMachine,My,CN=adatum!LocalMachine,My,CN=noexist" };

            bool evaluate = evaluator.Evaluate(check, null);

            Assert.IsFalse(evaluate);
        }
    }
}