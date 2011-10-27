using System;
using System.Diagnostics;

namespace DependencyChecker.SystemIntegration
{
    public class RegisterAspNetInIIS : WrappedProcessBase
    {
        private readonly string pathToAspnetRegIIS;
        
        public RegisterAspNetInIIS()
        {
            pathToAspnetRegIIS = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis");
        }

        public void Execute()
        {
            var start = CreateProcessStartInfo(pathToAspnetRegIIS);

            start.Arguments = "-ir";

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