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
            this.InitializeComponent();
            this.llDownloadUrl.Text = string.Empty;
            this.titleSavedColor = this.lblTitle.ForeColor;
        }

        public bool DependencyStatus { get; set; }

        public void SetDependency(Dependency dependencyToEvalulate, IEvaluationContext context)
        {
            if (dependencyToEvalulate == null)
            {
                throw new ArgumentNullException(nameof(dependencyToEvalulate));
            }
            this.dependency = dependencyToEvalulate;

            this.dependsOnTitle = "The following items must be installed before this one:";
            this.lblTitle.Text = dependencyToEvalulate.Title;
            this.lblExplanation.Text = dependencyToEvalulate.Explanation;
            this.toolTip1.SetToolTip(this.lblExplanation, dependencyToEvalulate.Explanation);
            if (!string.IsNullOrEmpty(dependencyToEvalulate.DownloadUrl))
            {
                this.llDownloadUrl.Links[0].Description = dependencyToEvalulate.DownloadUrl;
                this.llDownloadUrl.Text = @"Download";
            }
            else if (!string.IsNullOrEmpty(dependencyToEvalulate.ScriptName))
            {
                this.llDownloadUrl.Links[0].Description = dependencyToEvalulate.ScriptName;
                this.llDownloadUrl.Text = @"Install now";
            }
            else if (!string.IsNullOrEmpty(dependencyToEvalulate.InfoUrl))
            {
                this.llDownloadUrl.Links[0].Description = dependencyToEvalulate.InfoUrl;
                this.llDownloadUrl.Text = @"More information";
            }
            if (context != null)
            {
                this.EvaluateDependency(dependencyToEvalulate, context);
            }
        }

        private void EvaluateDependency(Dependency dependencyToEvalulate, IEvaluationContext context)
        {
            try
            {
                if (context.Evaluate(dependencyToEvalulate.Check))
                {
                    this.pictureBox1.Image = Resources.Checked;
                    this.llDownloadUrl.Visible = false;
                    this.lblExplanation.Visible = false;
                    this.Height = 25;
                    this.lblTitle.ForeColor = this.titleSavedColor;
                    this.lblTitle.Top = 4;
                    this.DependencyStatus = true;
                }
                else
                {
                    if (dependencyToEvalulate.RequiredDependencies != null && dependencyToEvalulate.RequiredDependencies.Count > 0)
                    {
                        var deps = this.GetItemDependencies(dependencyToEvalulate, context);

                        if (!string.IsNullOrEmpty(deps))
                        {
                            this.toolTip1.SetToolTip(this.lblDependsOn, deps);
                            this.pictureBox1.Image = Resources.Unchecked;
                            this.pictureBox1.Top = 10;
                            this.lblTitle.ForeColor = Color.Red;
                            this.DependencyStatus = false;
                            this.lblExplanation.Visible = true;
                            this.lblExplanation.Text = this.dependsOnTitle;
                            this.llDownloadUrl.Visible = false;
                            this.lblDependsOn.Text = deps;
                            this.lblDependsOn.Visible = true;
                        }
                        else
                        {
                            this.pictureBox1.Image = Resources.Unchecked;
                            this.pictureBox1.Top = 10;
                            this.lblTitle.ForeColor = Color.Red;
                            this.DependencyStatus = false;
                            this.lblExplanation.Visible = true;
                            this.llDownloadUrl.Visible = true;
                        }
                    }
                    else
                    {
                        this.pictureBox1.Image = Resources.Unchecked;
                        this.pictureBox1.Top = 10;
                        this.lblTitle.ForeColor = Color.Red;
                        this.DependencyStatus = false;
                        this.lblExplanation.Visible = true;
                        this.llDownloadUrl.Visible = true;
                    }
                }
            }
            catch (Exception exception)
            {
                string errorMessage =
                    string.Format(
                        "'{0}' dependency could not be verified. Install components above this one first.",
                        dependencyToEvalulate.Title);

                if (dependencyToEvalulate.RequiredDependencies != null && dependencyToEvalulate.RequiredDependencies.Count > 0)
                {
                    var deps = this.GetItemDependencies(dependencyToEvalulate, context);

                    if (!string.IsNullOrEmpty(deps))
                    {
                        this.toolTip1.SetToolTip(this.lblDependsOn, deps);
                        this.pictureBox1.Image = Resources.Unchecked;
                        this.pictureBox1.Top = 10;
                        this.lblTitle.ForeColor = Color.Red;
                        this.DependencyStatus = false;
                        this.lblExplanation.Visible = true;
                        this.lblExplanation.Text = this.dependsOnTitle;
                        this.llDownloadUrl.Visible = false;
                        this.lblDependsOn.Text = deps;
                        this.lblDependsOn.Visible = true;
                    }
                    else
                    {
                        this.pictureBox1.Image = Resources.Unchecked;
                        this.pictureBox1.Top = 10;
                        this.lblTitle.ForeColor = Color.Red;
                        this.lblExplanation.Visible = true;
                        this.DependencyStatus = false;
                        this.llDownloadUrl.Visible = false;
                        this.lblExplanation.Text = errorMessage;
                        this.toolTip1.SetToolTip(this.lblExplanation, errorMessage);
                    }
                }
                else
                {
                    this.pictureBox1.Image = Resources.Unchecked;
                    this.pictureBox1.Top = 10;
                    this.lblTitle.ForeColor = Color.Red;
                    this.lblExplanation.Visible = true;
                    this.DependencyStatus = false;
                    this.llDownloadUrl.Visible = false;
                    this.lblExplanation.Text = errorMessage;
                    this.toolTip1.SetToolTip(this.lblExplanation, errorMessage);
                }

                this.errorService.LogError(errorMessage, exception);
            }
        }

        private string GetItemDependencies(Dependency dependency, IEvaluationContext context)
        {
            string innerDeps = string.Empty;

            var sb = new System.Text.StringBuilder();
            dependency.RequiredDependencies.ForEach(dep =>
            {
                if (dep != null)
                {
                    if (dep.RequiredDependencies != null && dep.RequiredDependencies.Count > 0)
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
            });

            return sb.Length > 0 ? sb.ToString().Remove(sb.Length - 2) : string.Empty;
        }

        private void OnDownloadUrlLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dependency.DownloadUrl))
            {
                Process.Start(this.dependency.DownloadUrl);
            }
            else if (!string.IsNullOrEmpty(this.dependency.ScriptName))
            {
                if (this.dependency.ScriptName.StartsWith("cmd:"))
                {
                    var worker = new BackgroundWorker { WorkerReportsProgress = true };
                    worker.DoWork += this.OnWorkerDoWork;
                    worker.ProgressChanged += this.OnWorkerProgressChanged;
                    worker.RunWorkerCompleted += this.OnWorkerRunWorkerCompleted;

                    this.installingDependency = new InstallingDependency();
                    try
                    {
                        this.TopLevelControl.Cursor = Cursors.WaitCursor;
                        this.installingDependency.Show();
                        Application.DoEvents();
                        worker.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        this.errorService.ShowError("This dependency could not be verified. Install components above this one first.", ex);
                    }
                }
                else
                {
                    Process.Start(this.dependency.ScriptName);
                }
            }
            else if (!string.IsNullOrEmpty(this.dependency.InfoUrl))
            {
                Process.Start(this.dependency.InfoUrl);
            }
        }

        private void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var cmdType = this.dependency.ScriptName.Split(':')[1];
            var command = Activator.CreateInstance(Type.GetType(cmdType)) as IDependencySetupCommand;
            var rpm = command as IRequiresProductManager;
            if (rpm != null)
            {
                rpm.ProductManager = this.productManager;
            }

            try
            {
                command.Execute(this.dependency);
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
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        private void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.installingDependency.ShowProgress();
        }

        private void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.installingDependency.Close();
            this.TopLevelControl.Cursor = Cursors.Default;
            if (e.Result != null)
            {
                this.errorService.ShowError("This dependency could not be verified. Install components above this one first.", e.Result as Exception);
                return;
            }

            this.messageService.ShowMessage("The installation of this dependency has completed successfully. To verify it, press 'Rescan'.");
        }
    }
}