//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Configuration
{
    using System.Configuration;

    public class MinimumRequirements : ConfigurationElement
    {
        private const string DependencyCollectionProperty = "";
        private const string MinimumOsBuildProperty = "MinimumOSBuildNumber";

        [ConfigurationProperty(DependencyCollectionProperty, IsDefaultCollection = true, IsRequired = true)]
        public DependencyElementCollection Dependencies
        {
            get { return (DependencyElementCollection)this[DependencyCollectionProperty]; }
        }

        [ConfigurationProperty(MinimumOsBuildProperty, IsRequired = true)]
        public string MinimumOsBuild
        {
            get { return (string)this[MinimumOsBuildProperty]; }
            set { this[MinimumOsBuildProperty] = value; }
        }
    }
}