//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using DependencyChecker.SystemIntegration;

namespace DependencyChecker.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AppCmdWrapperTest
    {
        [TestMethod]
        public void IsHttpsEnabledShouldReturnTrue()
        {
            var appCmd = new AppCmd();
            Assert.IsTrue(appCmd.IsHttpsEnabled());
        }
    }
}
