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

    public class DependencyCheckEvaluatorCollection : ConfigurationElementCollection
    {
        public DependencyCheckEvaluatorCollection() => AddElementName = "checkEvaluator";

        public new DependencyCheckEvaluator this[string name]
        {
            get { return (DependencyCheckEvaluator)BaseGet(name); }
        }

        public DependencyCheckEvaluator this[int index]
        {
            get { return (DependencyCheckEvaluator)BaseGet(index); }
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
            return new DependencyCheckEvaluator();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DependencyCheckEvaluator)element).Name;
        }
    }
}