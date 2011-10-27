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
    using System.Collections.Generic;

    public class Dependency
    {
        public string Category { get; set; }

        public string Check { get; set; }
        public string DownloadUrl { get; set; }
        public bool Enabled { get; set; }

        public string Explanation { get; set; }

        public string InfoUrl { get; set; }

        public string ScriptName { get; set; }

        public string Settings { get; set; }
        public string Title { get; set; }

        public string DependsOn { get; set; }

        public List<Dependency> RequiredDependencies { get; set; }
    }
}