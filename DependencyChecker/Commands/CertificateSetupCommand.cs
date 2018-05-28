//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


using System;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using DependencyChecker.Common;
using DependencyChecker.SystemIntegration;

namespace DependencyChecker.Commands
{
    public class CertificateSetupCommand : IDependencySetupCommand
    {
        public CertificateSetupCommand()
        {
            this.Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException(nameof(dependency));
            }
            string appPoolUserName = CertificateCommon.GetAppPoolUserName();
            string networkServiceUserName = CertificateCommon.GetNetworkServiceUser();
            var certs = dependency.Settings.Split('!');
            foreach (var cert in certs)
            {
                string certFile;
                string storeName;
                string password;
                this.ExtractCertParameters(cert, out certFile, out storeName, out password);

                if (storeName == @"AuthRoot")
                {
                    ImportCer(certFile, storeName);
                    continue;
                }

                var certificate = InstallOrUpdatePfxCertificate(certFile, storeName, password);
                AddAccessToCertificate(certificate, appPoolUserName);
                AddAccessToCertificate(certificate, networkServiceUserName);
            }

            this.Completed = true;
        }

        private static void AddAccessToCertificate(X509Certificate2 cert, string user)
        {
            var rsa = cert.PrivateKey as RSACryptoServiceProvider;

            if (rsa != null)
            {
                string keyfilepath =
                    CertificateCommon.FindKeyLocation(rsa.CspKeyContainerInfo.UniqueKeyContainerName);

                var file = new FileInfo(keyfilepath + "\\" +
                                        rsa.CspKeyContainerInfo.UniqueKeyContainerName);

                FileSecurity fs = file.GetAccessControl();

                var account = new NTAccount(user);
                fs.AddAccessRule(new FileSystemAccessRule(account, FileSystemRights.FullControl, AccessControlType.Allow));

                file.SetAccessControl(fs);
            }
            else
            {
                var message =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Unable to access the certificate's private key to assign the required permissions. Certificate name: {0}",
                        cert.SubjectName);
                throw new ApplicationException(message);
            }
        }

        private static void ImportCer(string cerPath, string storeName)
        {
            var cer = new X509Certificate2();
            cer.Import(cerPath);

            var store = new X509Store(storeName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            store.Add(cer);
            store.Close();
        }

        private static X509Certificate2 ImportPfx(string pfxPath, string pfxPwd, string storeName)
        {
            const X509KeyStorageFlags Options = X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet;
            var pfx = new X509Certificate2();
            pfx.Import(pfxPath, pfxPwd, Options);
            var store = new X509Store(storeName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            store.Add(pfx);
            store.Close();

            return pfx;
        }

        private static X509Certificate2 InstallOrUpdatePfxCertificate(string pfxCertificatePath, string storeName, string password)
        {
            var certificate = ImportPfx(pfxCertificatePath, password, storeName);
            var installedCertificate = CertificateCommon.GetCertificate(certificate.Thumbprint, storeName, StoreLocation.LocalMachine);
            if (installedCertificate != null)
            {
                certificate = installedCertificate;
            }
            else
            {
                ImportPfx(pfxCertificatePath, password, storeName);
            }

            return certificate;
        }

        private void ExtractCertParameters(string input, out string certFile, out string storeName, out string password)
        {
            var certData = input.Split(',');
            if (certData.Length < 3)
            {
                throw new InvalidOperationException("Missing certificate data");
            }

            certFile = certData[0];
            storeName = certData[1];
            password = certData[2];
        }
    }
}