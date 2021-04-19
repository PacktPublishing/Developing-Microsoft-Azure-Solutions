using Microsoft.Extensions.DependencyInjection;
using static Pineapple.Common.Preconditions;
using static Pineapple.Extensions.ServiceCollectionExtensions;

namespace Leaderboard
{
    public static class RegisterAssembly
    {
        internal class FunctionsConfigured
        {
        }

        public static void AddFunctions(this IServiceCollection services)
        {
            CheckIsNotNull(nameof(services), services);

            if (services.IsConfigured<FunctionsConfigured>())
                return;

            services.Configure<FunctionsConfigured>();
        }
    }
}
