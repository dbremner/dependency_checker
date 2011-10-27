using System;
using System.Diagnostics;
using System.Globalization;

namespace DependencyChecker.SystemIntegration
{
    public class RegistertAspNetSql : WrappedProcessBase
    {
        private readonly string pathToAspnetRegSQL;

        public RegistertAspNetSql()
        {
            pathToAspnetRegSQL = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regsql");
            
        }

        public void Execute(string connectionString)
        {
            var start = CreateProcessStartInfo(pathToAspnetRegSQL);

            start.Arguments = string.Format(CultureInfo.InvariantCulture, "-A mr -C \"{0}\"", connectionString);

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