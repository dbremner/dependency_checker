using System.Diagnostics;

namespace DependencyChecker.SystemIntegration
{
    public abstract class WrappedProcessBase
    {
        protected ProcessStartInfo CreateProcessStartInfo(string pathToExe)
        {
            var start = new ProcessStartInfo
                            {
                                FileName = pathToExe,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            };
            return start;
        }
    }
}