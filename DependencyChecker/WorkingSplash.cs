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

    public partial class WorkingSplash : Form
    {
        public WorkingSplash()
        {
            InitializeComponent();
        }

        public int MaxDependencies
        {
            get { return progressBar.Maximum; }
            set { progressBar.Maximum = value; }
        }

        public string ScanningPrompt
        {
            set { scanningLabel.Text = value; }
        }

        public void DisableProgressBar()
        {
            progressBar.Visible = false;
        }

        public void ShowCurrentProgress(int current)
        {
            progressBar.Value = current;
        }
    }
}