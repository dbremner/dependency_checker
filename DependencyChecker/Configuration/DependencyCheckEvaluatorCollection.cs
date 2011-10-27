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
        public DependencyCheckEvaluatorCollection()
        {
            this.AddElementName = "checkEvaluator";
        }

        public new DependencyCheckEvaluator this[string name]
        {
            get { return (DependencyCheckEvaluator)this.BaseGet(name); }
        }

        public DependencyCheckEvaluator this[int index]
        {
            get { return (DependencyCheckEvaluator)this.BaseGet(index); }
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
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