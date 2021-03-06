﻿//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker.Common
{
    public class Check
    {
        public Check()
        {
        }

        public Check(string checkType, string name, string value)
        {
            CheckType = checkType;
            Name = name;
            Value = value;
        }

        public string CheckType { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }
    }
}