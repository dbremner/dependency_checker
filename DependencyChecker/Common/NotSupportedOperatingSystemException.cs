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

    public class NotSupportedOperatingSystemException : Exception
    {
        public NotSupportedOperatingSystemException(int osBuild)
            : base(string.Format("The dependency checker does not contain any information on the dependencies for your current Operating System (build: {0})", osBuild))
        {
            this.OsBuild = osBuild;
        }

        public int OsBuild { get; set; }
    }
}