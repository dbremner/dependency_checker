//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================

namespace DependencyChecker.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using DependencyChecker.Common;
    using Services;
    using DependencyChecker.Properties;
    using Microsoft.Web.PlatformInstaller;

    public partial class DependencyViewControl : UserControl
    {
        private readonly IErrorService errorService;
        private readonly ProductManager productManager;
        private readonly IMessageService messageService;
        private readonly Color titleSavedColor;
        private Dependency dependency;
        private InstallingDependency installingDependency;
        private string dependsOnTitle;

        public DependencyViewControl(IErrorService errorService, IMessageService messageService, ProductManager productManager)
        {
            this.errorService = errorService;
            this.productManager = productManager;
            this.messageService = messageService;
            InitializeComponent();
            llDownloadUrl.Text = string.Empty;
            titleSavedColor = lblTitle.ForeColor;
        }

        public bool DependencyStatus { get; set; }

        public void SetDependency(Dependency dependencyToEvalulate, IEvaluationContext context)
        {
            if (dependencyToEvalulate == null)
            {
                throw new ArgumentNullException(nameof(dependencyToEvalulate));
            }
            dependency = dependencyToEvalulate;

            dependsOnTitle = "The following items must be installed before this one:";
            lblTitle.Text = dependencyToEvalulate.Title;
            lblExplanation.Text = dependencyToEvalulate.Explanation;
            toolTip1.SetToolTip(lblExplanation, dependencyToEvalulate.Explanation);
            if (!string.IsNullOrEmpty(dependencyToEvalulate.DownloadUrl))
            {
                llDownloadUrl.Links[0].Description = dependencyToEvalulate.DownloadUrl;
                llDownloadUrl.Text = @"Download";
            }
            else if (!string.IsNullOrEmpty(dependencyToEvalulate.ScriptName))
            {
                llDownloadUrl.Links[0].Description = dependencyToEvalulate.ScriptName;
                llDownloadUrl.Text = @"Install now";
            }
            else if (!string.IsNullOrEmpty(dependencyToEvalulate.InfoUrl))
            {
                llDownloadUrl.Links[0].Description = dependencyToEvalulate.InfoUrl;
                llDownloadUrl.Text = @"More information";
            }
            if (context != null)
            {
                EvaluateDependency(dependencyToEvalulate, context);
            }
        }

        private void EvaluateDependency(Dependency dependencyToEvalulate, IEvaluationContext context)
        {
            try
            {
                if (context.Evaluate(dependencyToEvalulate.Check))
                {
                    pictureBox1.Image = Resources.Checked;
                    llDownloadUrl.Visible = false;
                    lblExplanation.Visible = false;
                    Height = 25;
                    lblTitle.ForeColor = titleSavedColor;
                    lblTitle.Top = 4;
                    DependencyStatus = true;
                }
                else
                {
                    if (dependencyToEvalulate.RequiredDependencies?.Count > 0)
                    {
                        var deps = GetItemDependencies(dependencyToEvalulate, context);

                        if (!string.IsNullOrEmpty(deps))
                        {
                            toolTip1.SetToolTip(lblDependsOn, deps);
                            pictureBox1.Image = Resources.Unchecked;
                            pictureBox1.Top = 10;
                            lblTitle.ForeColor = Color.Red;
                            DependencyStatus = false;
                            lblExplanation.Visible = true;
                            lblExplanation.Text = dependsOnTitle;
                            llDownloadUrl.Visible = false;
                            lblDependsOn.Text = deps;
                            lblDependsOn.Visible = true;
                        }
                        else
                        {
                            pictureBox1.Image = Resources.Unchecked;
                            pictureBox1.Top = 10;
                            lblTitle.ForeColor = Color.Red;
                            DependencyStatus = false;
                            lblExplanation.Visible = true;
                            llDownloadUrl.Visible = true;
                        }
                    }
                    else
                    {
                        pictureBox1.Image = Resources.Unchecked;
                        pictureBox1.Top = 10;
                        lblTitle.ForeColor = Color.Red;
                        DependencyStatus = false;
                        lblExplanation.Visible = true;
                        llDownloadUrl.Visible = true;
                    }
                }
            }
            catch (Exception exception)
            {
                string errorMessage =
                    string.Format(
                        "'{0}' dependency could not be verified. Install components above this one first.",
                        dependencyToEvalulate.Title);

                if (dependencyToEvalulate.RequiredDependencies?.Count > 0)
                {
                    var deps = GetItemDependencies(dependencyToEvalulate, context);

                    if (!string.IsNullOrEmpty(deps))
                    {
                        toolTip1.SetToolTip(lblDependsOn, deps);
                        pictureBox1.Image = Resources.Unchecked;
                        pictureBox1.Top = 10;
                        lblTitle.ForeColor = Color.Red;
                        DependencyStatus = false;
                        lblExplanation.Visible = true;
                        lblExplanation.Text = dependsOnTitle;
                        llDownloadUrl.Visible = false;
                        lblDependsOn.Text = deps;
                        lblDependsOn.Visible = true;
                    }
                    else
                    {
                        pictureBox1.Image = Resources.Unchecked;
                        pictureBox1.Top = 10;
                        lblTitle.ForeColor = Color.Red;
                        lblExplanation.Visible = true;
                        DependencyStatus = false;
                        llDownloadUrl.Visible = false;
                        lblExplanation.Text = errorMessage;
                        toolTip1.SetToolTip(lblExplanation, errorMessage);
                    }
                }
                else
                {
                    pictureBox1.Image = Resources.Unchecked;
                    pictureBox1.Top = 10;
                    lblTitle.ForeColor = Color.Red;
                    lblExplanation.Visible = true;
                    DependencyStatus = false;
                    llDownloadUrl.Visible = false;
                    lblExplanation.Text = errorMessage;
                    toolTip1.SetToolTip(lblExplanation, errorMessage);
                }

                errorService.LogError(errorMessage, exception);
            }
        }

        private string GetItemDependencies(Dependency dependency, IEvaluationContext context)
        {
            string innerDeps = string.Empty;

            var sb = new System.Text.StringBuilder();
            foreach (var dep in dependency.RequiredDependencies)
            {
                if (dep != null)
                {
                    if (dep.RequiredDependencies?.Count > 0)
                    {
                        innerDeps = GetItemDependencies(dep, context);
                    }

                    if (!string.IsNullOrEmpty(innerDeps))
                    {
                        sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}, ", dep.Title));
                    }
                    else
                    {
                        if (!context.Evaluate(dep.Check))
                        {
                            sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}, ", dep.Title));
                        }
                    }
                }
            }

            return sb.Length > 0 ? sb.ToString().Remove(sb.Length - 2) : string.Empty;
        }

        private void OnDownloadUrlLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(dependency.DownloadUrl))
            {
                Process.Start(dependency.DownloadUrl);
            }
            else if (!string.IsNullOrEmpty(dependency.ScriptName))
            {
                if (dependency.ScriptName.StartsWith("cmd:"))
                {
                    var worker = new BackgroundWorker { WorkerReportsProgress = true };
                    worker.DoWork += OnWorkerDoWork;
                    worker.ProgressChanged += OnWorkerProgressChanged;
                    worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;

                    installingDependency = new InstallingDependency();
                    try
                    {
                        TopLevelControl.Cursor = Cursors.WaitCursor;
                        installingDependency.Show();
                        Application.DoEvents();
                        worker.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        errorService.ShowError("This dependency could not be verified. Install components above this one first.", ex);
                    }
                }
                else
                {
                    Process.Start(dependency.ScriptName);
                }
            }
            else if (!string.IsNullOrEmpty(dependency.InfoUrl))
            {
                Process.Start(dependency.InfoUrl);
            }
        }

        private void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var cmdType = dependency.ScriptName.Split(':')[1];
            var command = Activator.CreateInstance(Type.GetType(cmdType)) as IDependencySetupCommand;
            if (command is IRequiresProductManager rpm)
            {
                rpm.ProductManager = productManager;
            }

            try
            {
                command.Execute(dependency);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }

            while (!command.Completed && e.Result == null)
            {
                Thread.Sleep(1500);
            }
            var disposable = command as IDisposable;
            disposable?.Dispose();
        }

        private void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            installingDependency.ShowProgress();
        }

        private void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            installingDependency.Close();
            TopLevelControl.Cursor = Cursors.Default;
            if (e.Result != null)
            {
                errorService.ShowError("This dependency could not be verified. Install components above this one first.", e.Result as Exception);
                return;
            }

            messageService.ShowMessage("The installation of this dependency has completed successfully. To verify it, press 'Rescan'.");
        }
    }
}