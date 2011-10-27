//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Services
{
    public class MessageService : IMessageService
    {
        private const string MessageTitle = "Dependency Checker";

        public void ShowMessage(string message)
        {
            System.Windows.Forms.MessageBox.Show(
                message, 
                MessageTitle, 
                System.Windows.Forms.MessageBoxButtons.OK, 
                System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
