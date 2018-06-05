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
    using System.Configuration;
    using DependencyChecker.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigurationLoadTest
    {
        [TestMethod]
        public void CanLoadCommonChecksConfiguration()
        {
            Configuration config = TestHelper.GetConfiguration();

            var dependenciesSection = (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
            DependencyCheckCollection commonChecks = dependenciesSection.CommonChecks;
            Assert.AreEqual(6, commonChecks.Count);
        }

        [TestMethod]
        public void CanLoadOSConfiguration()
        {
            Configuration config = TestHelper.GetConfiguration();

            var dependenciesSection = (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
            Assert.IsNotNull(dependenciesSection);
            DependencyGroupCollection commonChecks = dependenciesSection.DependencyGroups;
            Assert.IsNotNull(commonChecks);
            Assert.AreEqual(4, commonChecks.Count);
        }

        [TestMethod]
        public void CanLoadOSSpecificChecksConfiguration()
        {
            Configuration config = TestHelper.GetConfiguration();

            var dependenciesSection = (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
            Assert.AreEqual(2, dependenciesSection.DependencyGroups["MockOS"].Checks.Count);
        }
    }
}