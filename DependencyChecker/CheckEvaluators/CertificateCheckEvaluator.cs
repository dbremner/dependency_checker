//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;

namespace DependencyChecker.CheckEvaluators
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;

    public class CertificateCheckEvaluator : ICheckEvaluator
    {
        public virtual bool Evaluate(Check check, IEvaluationContext context)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            string subject;
            string storeName;
            StoreLocation location;
            this.ExtractCertParameters(check.Value, out subject, out storeName, out location);

            var certificate = CertificateCommon.GetCertificate(subject, storeName, location);
            if (certificate == null)
            {
                return false;
            }

            if (storeName != "My")
            {
                return true;
            }

            return CheckFullControlAccessToCertificate(certificate, CertificateCommon.GetAppPoolUserName()) &&
                CheckFullControlAccessToCertificate(certificate, CertificateCommon.GetNetworkServiceUser());
        }

        private static bool CheckFullControlAccessToCertificate(X509Certificate2 cert, string user)
        {
            var rsa = cert.PrivateKey as RSACryptoServiceProvider;

            if (rsa == null)
            {
                return false;
            }

            string keyfilepath =
                CertificateCommon.FindKeyLocation(rsa.CspKeyContainerInfo.UniqueKeyContainerName);

            var file = new FileInfo(keyfilepath + "\\" +
                                    rsa.CspKeyContainerInfo.UniqueKeyContainerName);

            var accessRules = file.GetAccessControl().GetAccessRules(true, true, typeof(NTAccount)).Cast<FileSystemAccessRule>();
            var fullControlRule =
                accessRules.SingleOrDefault(
                    r =>
                        r.IdentityReference.Value == user &&
                        r.AccessControlType == AccessControlType.Allow &&
                        r.FileSystemRights == FileSystemRights.FullControl);

            return fullControlRule != null;
        }

        private void ExtractCertParameters(string input, out string subject, out string storeName, out StoreLocation location)
        {
            var certData = input.Split(',');
            if (certData.Length < 3)
            {
                throw new InvalidOperationException("Missing certificate data");
            }

            subject = certData[2];
            storeName = certData[1];
            location = StoreLocation.CurrentUser;
            if (certData[0].Equals("LocalMachine"))
            {
                location = StoreLocation.LocalMachine;
            }
        }
    }
}