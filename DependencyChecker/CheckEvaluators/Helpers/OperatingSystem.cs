﻿//===============================================================================
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

    internal static class OperatingSystem
    {
        public static int GetOsBuild()
        {
            ManagementObjectSearcher searcher = WmiHelper.RunWmiQuery("win32_operatingsystem");
            foreach (ManagementObject mo in searcher.Get())
            {
                return int.Parse(mo.SystemProperties["BuildNumber"].Value.ToString());
            }
            return 0;
        }
    }
}