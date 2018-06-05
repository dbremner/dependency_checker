//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using DependencyChecker.Common;
    using Services;
    using DependencyChecker.Configuration;
    using DependencyChecker.Controls;
    using Microsoft.Web.PlatformInstaller;

    public partial class DependencyCheckerForm : Form
    {
        private readonly IErrorService errorService;
        private readonly ProductManager productManager;
        private readonly IMessageService messageService;
        private DependenciesSection configSection;
        private DependenciesInfo dependenciesInfo;
        private Dictionary<string, DependencyGroupView> dependencyGroupViews;
        private System.Configuration.Configuration fileConfig;

        public DependencyCheckerForm(IErrorService errorService, IMessageService messageService, ProductManager productManager)
        {
            this.productManager = productManager;
            this.errorService = errorService;
            this.messageService = messageService;

            try
            {
                InitializeComponent();
                InitializeConfigurationSection();
                titleLabel.Text = configSection.Title;
                Text = configSection.Title;
                descriptionLabel.Text = configSection.Description;
                InitializedDependencyGroupViews();
            }
            catch (Exception ex)
            {
                this.errorService.ShowError("Could not load DependencyChecker Form", ex);
                Application.Exit();
            }
        }

        protected virtual DependenciesInfoBuilder GetDependenciesInfoBuilder()
        {
            return new DependenciesInfoBuilder(productManager);
        }

        protected override void OnLoad(EventArgs e)
        {
            foreach (DependencyGroupView dependencyGroupView in dependencyGroupViews.Values)
            {
                dependencyGroupView.Reset();
            }

            foreach (Dependency d in dependenciesInfo.Dependencies)
            {
                AddDependencyControl(d, null);
            }
            base.OnLoad(e);
        }

        private static void OnButtonExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddDependencyControl(Dependency dependency, IEvaluationContext context)
        {
            dependencyGroupViews[dependency.Category].AddDependency(dependency, context);
        }

        private void InitializeConfigurationSection()
        {
            fileConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configSection = (DependenciesSection)fileConfig.GetSection(DependenciesSection.SectionName);

            try
            {
                dependenciesInfo = GetDependenciesInfoBuilder().BuildDependenciesInfo(configSection);
            }
            catch (NotSupportedException e)
            {
                MessageBox.Show(e.Message, @"OS Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        private void InitializedDependencyGroupViews()
        {
            SuspendLayout();

            dependencyGroupViews = new Dictionary<string, DependencyGroupView>();
            foreach (Dependency dependency in dependenciesInfo.Dependencies)
            {
                if (!dependencyGroupViews.ContainsKey(dependency.Category))
                {
                    var newGroupView = new DependencyGroupView(errorService, messageService, productManager)
                                           {
                                               Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                                               AutoSize = true,
                                               AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                               Dependencies = null,
                                               Heading = dependency.Category,
                                               Size = new Size(flowLayoutPanel.Width - 6, 18),
                                               MinimumSize = new Size(flowLayoutPanel.Width - 6, 18)
                                           };

                    flowLayoutPanel.Controls.Add(newGroupView);
                    dependencyGroupViews.Add(dependency.Category, newGroupView);
                }

                SetRequiredDependencies(dependency);
            }
            ResumeLayout(true);
        }

        private void SetRequiredDependencies(Dependency dependency)
        {
            if (!string.IsNullOrEmpty(dependency.DependsOn))
            {
                if (dependency.RequiredDependencies == null)
                {
                    dependency.RequiredDependencies = new List<Dependency>();
                }

                var deps = dependency.DependsOn.Split(',');

                foreach (var dep in deps.ToList())
                {
                    dependency.RequiredDependencies.Add(dependenciesInfo.Dependencies.Where(item => item.Check == dep).SingleOrDefault());
                }
            }
        }

        private void OnButtonScanClicked(object sender, EventArgs e)
        {
            try
            {
                scanButton.Enabled = false;
                Scan();
                scanButton.Text = @"&Rescan";
            }
            finally
            {
                scanButton.Enabled = true;
            }
        }

        private void Scan()
        {
            foreach (DependencyGroupView dependencyGroupView in dependencyGroupViews.Values)
            {
                dependencyGroupView.Reset();
            }

            var splash = new WorkingSplash();
            splash.Show();
            splash.MaxDependencies = dependenciesInfo.Dependencies.Count;
            Application.DoEvents();

            int current = 0;
            foreach (Dependency d in dependenciesInfo.Dependencies)
            {
                splash.ScanningPrompt = "Scanning for " + d.Title;
                splash.ShowCurrentProgress(current++);
                AddDependencyControl(d, dependenciesInfo.EvaluationContext);
            }

            splash.Close();
        }
    }
}