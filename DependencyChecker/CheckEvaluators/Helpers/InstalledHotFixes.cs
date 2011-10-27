//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.CheckEvaluators.Helpers
{
    using System.Management;

    internal static class InstalledHotFixes
    {
        public static bool IsHotFixInstalled(string hotFixNumber)
        {
            string query = string.Format("select * from Win32_QuickFixEngineering where HotFixId = '{0}'", hotFixNumber);
            var selectQuery = new SelectQuery(query);
            var searcher = new ManagementObjectSearcher(selectQuery);

            return searcher.Get().Count > 0;
        }
    }
}