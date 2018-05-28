//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================
using System.Windows.Forms;

namespace DependencyChecker.Services
{
    public class MessageService : IMessageService
    {
        private const string MessageTitle = "Dependency Checker";

        public void ShowMessage(string message)
        {
            MessageBox.Show(
                message, 
                MessageTitle,
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
    }
}
