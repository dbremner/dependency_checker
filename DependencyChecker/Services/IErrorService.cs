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
    using System;

    public interface IErrorService
    {
        void LogError(string message, Exception exception);
        void ShowError(string message, Exception exception);
    }
}