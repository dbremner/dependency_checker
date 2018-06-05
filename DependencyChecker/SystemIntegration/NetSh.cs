using System;
using System.Diagnostics;

namespace DependencyChecker.SystemIntegration
{
    public class NetSh: WrappedProcessBase
    {
        private readonly string pathToNetSh;

        public NetSh()
        {
            pathToNetSh = "netsh";
        }

        public void Execute()
        {
            // this is the hash of the localhost certificate
            const string CertHash = "5a074d678466f59dbd063d1a98b1791474723365";

            // this appId is the default used to performed this operation
            const string AppId = "{4dc3e181-e14b-4a21-b022-59fc669b0914}";

            var start = CreateProcessStartInfo(pathToNetSh);
            string args = string.Format("http add sslcert ipport=0.0.0.0:443 certhash={0} appid={1}", CertHash, AppId);
            start.Arguments = args;

            using (var process = Process.Start(start))
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new InvalidOperationException(result);
                    }
                }
            }
        }
    }
}