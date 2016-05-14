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
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Windows.Forms;
    using DependencyChecker.Common;
    using Services;
    using DependencyChecker.Properties;
    using Microsoft.Web.PlatformInstaller;

    public partial class DependencyGroupView : UserControl
    {
        private readonly IErrorService errorService;
        private readonly IMessageService messageService;
        private readonly ProductManager productManager;

        public DependencyGroupView(IErrorService errorService, IMessageService messageService, ProductManager productManager)
        {
            this.errorService = errorService;
            this.productManager = productManager;
            this.messageService = messageService;
            this.InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public Collection<Dependency> Dependencies { get; set; }

        public string Heading
        {
            get { return this.lblHeading.Text; }
            set { this.lblHeading.Text = value; }
        }

        public bool AddDependency(Dependency dependency, IEvaluationContext context)
        {
            if (this.Dependencies == null)
            {
                this.Dependencies = new Collection<Dependency>();
            }
            this.Dependencies.Add(dependency);

            int controlNumber = this.flowLayoutPanel1.Controls.Count + 1;
            var newControl = new DependencyViewControl(this.errorService, this.messageService, this.productManager);
            newControl.SetDependency(dependency, context);

            // newControl.Location not needed because the flow layout will layout the control
            newControl.Name = "dependencyViewControl" + controlNumber;
            newControl.TabIndex = controlNumber - 1;
            this.flowLayoutPanel1.Controls.Add(newControl);
            return newControl.DependencyStatus;
        }

        public void Reset()
        {
            this.Dependencies = null;
            this.flowLayoutPanel1.Controls.Clear();
        }

        private void OnPanel1Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.Blue, 2), new Point(0, 9), new Point(this.panel1.Width, 9));
        }

        private void OnPictureBoxClicked(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Visible = !this.flowLayoutPanel1.Visible;
            this.pictureBox1.Image = this.flowLayoutPanel1.Visible ? Resources.Expanded : Resources.Collapsed;
        }
    }
}