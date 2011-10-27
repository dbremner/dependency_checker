//===============================================================================
// Microsoft patterns & practices
// Dependency Checker
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://dependencychecker.codeplex.com/license)
//===============================================================================


using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace DependencyChecker.SystemIntegration
{
    public class CertificateCommon
    {
        public static X509Certificate2 GetCertificate(string subject, string storeName, StoreLocation location)
        {
            var store = new X509Store(storeName, location);
            store.Open(OpenFlags.ReadOnly);

            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, subject, false);

            return certCollection.Count > 0 ? certCollection[0] : null;
        }

        public static string GetAppPoolUserName()
        {
            string val = Environment.OSVersion.Version.ToString();
            if (val.StartsWith("6.1"))
            {
                return @"BUILTIN\IIS_IUSRS";
            }

            return @"NT AUTHORITY\NETWORK SERVICE";
        }

        public static string GetNetworkServiceUser()
        {
            return @"NT AUTHORITY\NETWORK SERVICE";
        }

        public static string FindKeyLocation(string keyFileName)
        {
            string text1 =
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string text2 = text1 + @"\Microsoft\Crypto\RSA\MachineKeys";
            string[] textArray1 = Directory.GetFiles(text2, keyFileName);
            if (textArray1.Length > 0)
            {
                return text2;
            }

            string text3 =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string text4 = text3 + @"\Microsoft\Crypto\RSA\";
            textArray1 = Directory.GetDirectories(text4);
            if (textArray1.Length > 0)
            {
                foreach (string text5 in textArray1)
                {
                    textArray1 = Directory.GetFiles(text5, keyFileName);
                    if (textArray1.Length != 0)
                    {
                        return text5;
                    }
                }
            }

            return "Private key exists but is not accessible";
        }
    }
}
