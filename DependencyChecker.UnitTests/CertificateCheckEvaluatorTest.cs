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
    public class CertificateCheckEvaluatorTest
    {
        [TestMethod]
        public void ShouldEvaluateToTrue()
        {
            CertificateCheckEvaluator evaluator = new CertificateCheckEvaluator();
            Check check = new Check { Name = "Certficate", Value = "LocalMachine,My,CN=adatum" };
            Assert.IsTrue(evaluator.Evaluate(check, null));
        }

        [TestMethod]
        public void ShouldEvaluateToFalse()
        {
            CertificateCheckEvaluator evaluator = new CertificateCheckEvaluator();
            Check check = new Check { Name = "Certficate", Value = "LocalMachine,My,CN=noExist" };
            Assert.IsFalse(evaluator.Evaluate(check, null));
        }
    }
}