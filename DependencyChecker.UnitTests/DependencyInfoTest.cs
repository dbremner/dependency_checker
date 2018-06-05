//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;

namespace DependencyChecker.UnitTests
{
    using DependencyChecker.Common;
    using CheckEvaluators;
    using DependencyChecker.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyInfoTest
    {
        [TestMethod]
        public void CheckAnalysisIsPropagated()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new MockDependenciesInfoBuilder();

            builder.MockEvaluator.ReturnValues.Add("Reg1", true);
            builder.MockEvaluator.ReturnValues.Add("Reg2", true);
            builder.MockEvaluator.ResetHitsPerCheck();

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 400);
            bool checkValue1 = info.EvaluationContext.Evaluate(info.Dependencies[0].Check);
            bool checkValue2 = info.EvaluationContext.Evaluate(info.Dependencies[1].Check);

            Assert.IsTrue(checkValue1);
            Assert.IsTrue(checkValue2);
            Assert.AreEqual(1, builder.MockEvaluator.HitsPerCheck["Reg1"]);
            Assert.AreEqual(1, builder.MockEvaluator.HitsPerCheck["Reg2"]);
        }

        [TestMethod]
        public void CheckAnalysisIsPropagated2()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new MockDependenciesInfoBuilder();

            builder.MockEvaluator.ReturnValues.Add("Reg1", false);
            builder.MockEvaluator.ReturnValues.Add("Reg2", false);
            builder.MockEvaluator.ResetHitsPerCheck();

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 400);
            bool checkValue1 = info.EvaluationContext.Evaluate(info.Dependencies[0].Check);
            bool checkValue2 = info.EvaluationContext.Evaluate(info.Dependencies[1].Check);

            Assert.IsFalse(checkValue1);
            Assert.IsFalse(checkValue2);
            Assert.AreEqual(1, builder.MockEvaluator.HitsPerCheck["Reg1"]);
            Assert.AreEqual(1, builder.MockEvaluator.HitsPerCheck["Reg2"]);
        }

        [TestMethod]
        public void NonCachedResults()
        {
            // Just check if expression is analyzed completely.
            // Non cached results allow rescan with same objects
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new MockDependenciesInfoBuilder();

            builder.MockEvaluator.ReturnValues.Add("Reg1", true);
            builder.MockEvaluator.ReturnValues.Add("Reg2", true);
            builder.MockEvaluator.ReturnValues.Add("Soft1", true);
            builder.MockEvaluator.ResetHitsPerCheck();

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 200);
            bool checkValue1 = info.EvaluationContext.Evaluate(info.Dependencies[0].Check);
            bool checkValue2 = info.EvaluationContext.Evaluate(info.Dependencies[1].Check);

            Assert.IsTrue(checkValue1);
            Assert.IsTrue(checkValue2);
            Assert.AreEqual(2, builder.MockEvaluator.HitsPerCheck["Reg1"]);
            Assert.AreEqual(2, builder.MockEvaluator.HitsPerCheck["Reg2"]);
            Assert.AreEqual(0, builder.MockEvaluator.HitsPerCheck["Soft1"]);

            checkValue1 = info.EvaluationContext.Evaluate(info.Dependencies[0].Check);
            checkValue2 = info.EvaluationContext.Evaluate(info.Dependencies[1].Check);

            Assert.IsTrue(checkValue1);
            Assert.IsTrue(checkValue2);
            Assert.AreEqual(4, builder.MockEvaluator.HitsPerCheck["Reg1"]);
            Assert.AreEqual(4, builder.MockEvaluator.HitsPerCheck["Reg2"]);
            Assert.AreEqual(0, builder.MockEvaluator.HitsPerCheck["Soft1"]);
        }

        public class MockDependenciesInfoBuilder : DependenciesInfoBuilder
        {
            public MockDependenciesInfoBuilder() : base(null)
            {
            }

            public MockCheckEvaluator MockEvaluator { get; } = new MockCheckEvaluator();

            protected override void AddCheckEvaluators(EvaluationContext context, DependencyCheckEvaluatorCollection checkEvaluators)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }
                context.SetEvaluatorForCheckType("Registry", this.MockEvaluator);
                context.SetEvaluatorForCheckType("Software", this.MockEvaluator);
                context.SetEvaluatorForCheckType("Expression", new ExpressionCheckEvaluator());
            }
        }
    }
}