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
    using System.Windows.Forms;

    public partial class InstallingDependency : Form
    {
        private int counter;

        public InstallingDependency()
        {
            InitializeComponent();
            counter = 0;
        }

        public void ShowProgress()
        {
            counter++;
            progressLabel.Text += @".";
            if (counter > 5)
            {
                counter = 0;
                progressLabel.Text = @".";
            }
        }
    }
}