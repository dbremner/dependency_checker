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
    using System;
    using System.IO;
    using DependencyChecker.Common;
    using CheckEvaluators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.PlatformInstaller;

    [TestClass]
    public class WpiCheckEvaluatorTest
    {
        private static ProductManager productManager;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            productManager = new ProductManager();
            var assembly = typeof(WpiCheckEvaluatorTest).Assembly;
            var config = assembly.GetManifestResourceStream("DependencyChecker.UnitTests.Dependencies.xml");

            var tempFile = Path.GetTempFileName();
            using (var sr = new StreamReader(config))
            {
                using (var sw = new StreamWriter(tempFile))
                {
                    sw.Write(sr.ReadToEnd());
                }
            }
            var uri = new Uri("file://" + tempFile);
            productManager.Load(uri);
        }

        [TestMethod]
        public void ShouldEvaluateToFalseMySql()
        {
            var evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            var check = new Check { CheckType = "WPI", Value = "MySQL" };

            bool evaluate = evaluator.Evaluate(check, null);

            Assert.IsFalse(evaluate);
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWifRuntime()
        {
            var evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            var check = new Check { CheckType = "WPIRuntime", Value = "WIFRuntime" };

            bool evaluate = evaluator.Evaluate(check, null);

            Assert.IsTrue(evaluate);
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWifSdk()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "WPISDK", Value = "WIFSDK" };

            Assert.IsTrue(evaluator.Evaluate(check, null));
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWCFHTTP()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "WCFHTTP", Value = "WCFHTTP" };

            Assert.IsTrue(evaluator.Evaluate(check, null));
        }

        [TestMethod]
        public void ShouldEvaluateToTrueIIS7CGCC()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "IIS7CGCC", Value = "IIS7CGCC" };

            Assert.IsTrue(evaluator.Evaluate(check, null));
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWindowsAzureTools()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "WindowsAzureToolsVS2010", Value = "WindowsAzureToolsVS2010" };

            Assert.IsTrue(evaluator.Evaluate(check, null));
        }
    }
}