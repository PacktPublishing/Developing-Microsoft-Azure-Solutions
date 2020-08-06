using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public static class ReactExtensions
    {

        /// <summary>
        /// Replacement for the UseReactDevelopmentServer for .NET Core 3.1.x
        /// </summary>
        /// <param name="spa">Single Page Application Builder</param>
        public static void UseReactDevelopmentServer(this ISpaBuilder spa)
        {
            CheckIsNotNull(nameof(spa), spa);
            string sourcePath = spa.Options.SourcePath;
            CheckIsNotNullOrWhitespace(nameof(spa.Options.SourcePath), sourcePath);

            if (CanRunNpm(sourcePath))
            {
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        }

        private static bool CanRunNpm(string workingDirectory)
        {
            bool result = false;

            try
            {
                var npmExe = "npm";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Our assumption is that we can run on Window 
                    return true;
                }

                var processStartInfo = new ProcessStartInfo(npmExe)
                {
                    Arguments = string.Empty,
                    UseShellExecute = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    WorkingDirectory = workingDirectory
                };

                using (var process = Process.Start(processStartInfo))
                {
                    result = true;
                }
            }
            catch
            {
            }

            return result;
        }
    }
}
