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
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public Collection<Dependency> Dependencies { get; set; }

        public string Heading
        {
            get { return lblHeading.Text; }
            set { lblHeading.Text = value; }
        }

        public bool AddDependency(Dependency dependency, IEvaluationContext context)
        {
            if (Dependencies == null)
            {
                Dependencies = new Collection<Dependency>();
            }
            Dependencies.Add(dependency);

            int controlNumber = flowLayoutPanel1.Controls.Count + 1;
            var newControl = new DependencyViewControl(errorService, messageService, productManager);
            newControl.SetDependency(dependency, context);

            // newControl.Location not needed because the flow layout will layout the control
            newControl.Name = "dependencyViewControl" + controlNumber;
            newControl.TabIndex = controlNumber - 1;
            flowLayoutPanel1.Controls.Add(newControl);
            return newControl.DependencyStatus;
        }

        public void Reset()
        {
            Dependencies = null;
            flowLayoutPanel1.Controls.Clear();
        }

        private void OnPanel1Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.Blue, 2), new Point(0, 9), new Point(panel1.Width, 9));
        }

        private void OnPictureBoxClicked(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;
            pictureBox1.Image = flowLayoutPanel1.Visible ? Resources.Expanded : Resources.Collapsed;
        }
    }
}