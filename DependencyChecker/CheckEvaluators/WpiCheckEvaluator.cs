//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;
using DependencyChecker.Common;

namespace DependencyChecker.CheckEvaluators
{
    using Microsoft.Web.PlatformInstaller;

    public class WpiCheckEvaluator : ICheckEvaluator, IRequiresProductManager
    {
        public WpiCheckEvaluator(ProductManager productManager) => ProductManager = productManager;

        public ProductManager ProductManager { get; set; }

        public bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            var settings = check.Value.Split('!');
            bool ret = false;

            foreach (var setting in settings)
            {
                Product product = ProductManager.GetProduct(setting);
                ret = product.IsInstalled(false);
                if (!ret)
                {
                    break;
                }
            }
            return ret;
        }
    }
}