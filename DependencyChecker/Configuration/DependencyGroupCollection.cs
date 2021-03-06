﻿//===============================================================================
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

    public class DependencyGroupCollection : ConfigurationElementCollection
    {
        public DependencyGroupCollection() => AddElementName = "dependencyGroup";

        public new DependencyGroup this[string name]
        {
            get { return (DependencyGroup)BaseGet(name); }
        }

        public DependencyGroup this[int index]
        {
            get { return (DependencyGroup)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public DependencyGroup GetDependencyGroupByOsBuild(int osBuild)
        {
            foreach (DependencyGroup group in this)
            {
                if (int.Parse(group.BuildNumber) == osBuild)
                {
                    return group;
                }
            }
            return null;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DependencyGroup();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DependencyGroup)element).Name;
        }
    }
}