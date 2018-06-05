//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;
using Microsoft.Web.PlatformInstaller;

namespace DependencyChecker.Commands
{
    public class WpiCommand : IDependencySetupCommand, IRequiresProductManager, IDisposable
    {
        private readonly InstallManager installManager;
        private bool iisComponent;

        public WpiCommand()
        {
            installManager = new InstallManager();
        }

        public bool Completed { get; private set; }

        public ProductManager ProductManager { get; set; }

        public void Dispose()
        {
            installManager?.Dispose();
        }

        public void Execute(Dependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }
            var settings = dependency.Settings.Split('!');
            var installers = new Dictionary<string, Installer>();
            foreach (var setting in settings)
            {
                var product = ProductManager.GetProduct(setting);
                var sets = product.DependencySets.ToList();
                foreach (var installer in product.Installers)
                {
                    if (!installers.ContainsKey(installer.Product.ProductId))
                    {
                        installers.Add(installer.Product.ProductId, installer);
                    }
                }

                foreach (var item in sets.SelectMany(items => items))
                {
                    if (item.IsInstalled(false))
                    {
                        continue;
                    }
                    CheckIisComponent(item);
                    foreach (var installer in item.Installers)
                    {
                        if (!installers.ContainsKey(installer.Product.ProductId))
                        {
                            installers.Add(installer.Product.ProductId, installer);
                        }
                    }
                }

                CheckIisComponent(product);
            }
            installManager.Load(installers.Values);
            installManager.InstallCompleted += OnInstallCompleted;
            installManager.StartInstallation();
        }

        private void CheckIisComponent(Product product)
        {
            if (!iisComponent && product.IsIisComponent)
            {
                iisComponent = true;
            }
        }

        private void OnInstallCompleted(object sender, EventArgs e)
        {
            if (iisComponent)
            {
                (new RegisterAspNetInIIS()).Execute();
            }
            Completed = true;
        }
    }
}