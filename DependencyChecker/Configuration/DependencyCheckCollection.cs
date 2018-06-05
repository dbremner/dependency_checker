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

    public class DependencyCheckCollection : ConfigurationElementCollection
    {
        public DependencyCheckCollection() => AddElementName = "check";

        public new DependencyCheck this[string name]
        {
            get { return (DependencyCheck)BaseGet(name); }
        }

        public DependencyCheck this[int index]
        {
            get { return (DependencyCheck)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DependencyCheck();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DependencyCheck)element).Name;
        }
    }
}