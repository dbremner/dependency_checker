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
    using System.Collections.Generic;
    using System.Configuration;
    using DependencyChecker.Configuration;
    using Microsoft.Web.PlatformInstaller;
    using OperatingSystem = CheckEvaluators.Helpers.OperatingSystem;

    public class DependenciesInfoBuilder
    {
        private readonly ProductManager productManager;

        public DependenciesInfoBuilder(ProductManager productManager) =>
            this.productManager = productManager;

        public DependenciesInfo BuildDependenciesInfo(DependenciesSection configSection)
        {
            return BuildDependenciesInfo(configSection, OperatingSystem.GetOsBuild());
        }

        public DependenciesInfo BuildDependenciesInfo(DependenciesSection configSection, int osBuild)
        {
            if (configSection == null)
            {
                throw new ArgumentNullException(nameof(configSection));
            }
            bool supported = TryGetOsBuildNumber(configSection, ref osBuild);

            if (!supported)
            {
                throw new NotSupportedOperatingSystemException(osBuild);
            }

            var info = new DependenciesInfo
                           {
                               CompatibleOsBuild = osBuild,
                               EvaluationContext = new EvaluationContext(),
                               Dependencies = new List<Dependency>()
                           };

            // Add common checks & dependencies
            AddDependencyChecks(info, configSection.CommonChecks);
            AddDependencies(info, configSection.MinimumRequirements.Dependencies);

            // Add OS checks & dependencies
            DependencyGroup osDependencyGroup = configSection.DependencyGroups.GetDependencyGroupByOsBuild(osBuild);

            if (osDependencyGroup != null)
            {
                AddDependencyChecks(info, osDependencyGroup.Checks);
                AddDependencies(info, osDependencyGroup.Dependencies);
            }

            // Add evaluators
            AddCheckEvaluators((EvaluationContext)info.EvaluationContext, configSection.CheckEvaluators);

            return info;
        }

        protected virtual void AddCheckEvaluators(EvaluationContext context, DependencyCheckEvaluatorCollection checkEvaluators)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (checkEvaluators == null)
            {
                throw new ArgumentNullException(nameof(checkEvaluators));
            }
            foreach (DependencyCheckEvaluator checkEvaluator in checkEvaluators)
            {
                var evaluator = Activator.CreateInstance(Type.GetType(checkEvaluator.Type)) as ICheckEvaluator;
                if (evaluator != null)
                {
                    if (evaluator is IRequiresProductManager rpm)
                    {
                        rpm.ProductManager = productManager;
                    }
                    context.SetEvaluatorForCheckType(checkEvaluator.Name, evaluator);
                }
            }
        }

        protected virtual Check GetCheck(DependencyCheck dependencyCheck)
        {
            if (dependencyCheck == null)
            {
                throw new ArgumentNullException(nameof(dependencyCheck));
            }
            return new Check { CheckType = dependencyCheck.CheckType, Name = dependencyCheck.Name, Value = dependencyCheck.Value };
        }

        protected virtual Dependency GetDependency(DependencyElement dependencyElement)
        {
            if (dependencyElement == null)
            {
                throw new ArgumentNullException(nameof(dependencyElement));
            }
            var dependency = new Dependency
                                 {
                                     Check = dependencyElement.Check,
                                     DownloadUrl = dependencyElement.DownloadUrl,
                                     Enabled = dependencyElement.Enabled,
                                     Explanation = dependencyElement.Explanation,
                                     InfoUrl = dependencyElement.InfoUrl,
                                     Category = dependencyElement.Category,
                                     ScriptName = dependencyElement.ScriptName,
                                     Title = dependencyElement.Title,
                                     Settings = dependencyElement.Settings,
                                     DependsOn = dependencyElement.DependsOn
                                 };

            return dependency;
        }

        private static bool TryGetOsBuildNumber(DependenciesSection configSection, ref int currentOsBuild)
        {
            if (!int.TryParse(configSection.MinimumRequirements.MinimumOsBuild, out int minimumOsBuild))
            {
                throw new ConfigurationErrorsException("MinimumOSBuild property of MinimumRequirements element is not in correct format. Expected integer");
            }

            return minimumOsBuild <= currentOsBuild;
        }

        private void AddDependencies(DependenciesInfo info, DependencyElementCollection dependencies)
        {
            foreach (DependencyElement dependencyElement in dependencies)
            {
                if (dependencyElement.Enabled)
                {
                    info.Dependencies.Add(GetDependency(dependencyElement));
                }
            }
        }

        private void AddDependencyChecks(DependenciesInfo info, DependencyCheckCollection checks)
        {
            if (checks == null)
            {
                return;
            }

            foreach (DependencyCheck dependencyCheck in checks)
            {
                Check check = GetCheck(dependencyCheck);
                info.EvaluationContext[check.Name] = check;
            }
        }
    }
}