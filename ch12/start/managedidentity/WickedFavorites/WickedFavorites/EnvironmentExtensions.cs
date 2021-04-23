using System;
namespace Wicked.Favorites
{
    public static class Environment
    {
        public static bool IsDevelopment()
        {
            return string.Compare(System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Development", true) == 0;
        }

        public static bool IsRunningInContainer()
        {
            return string.Compare(System.Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", true) == 0;
        }

        public static string GetEnvironmentVariable(string variable)
        {
            return System.Environment.GetEnvironmentVariable(variable);
        }
    }
}
