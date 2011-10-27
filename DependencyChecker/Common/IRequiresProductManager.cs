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
    using Microsoft.Web.PlatformInstaller;

    public interface IRequiresProductManager
    {
        ProductManager ProductManager { get; set; }
    }
}