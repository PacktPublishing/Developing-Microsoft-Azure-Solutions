using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public static void AddFunctions(this IServiceCollection services, IConfiguration config)
        {
            CheckIsNotNull(nameof(services), services);

            if (services.IsConfigured<FunctionsConfigured>())
                return;

            services.Configure<FunctionsConfigured>();

            services.AddTransient<IFunctionDependencies, FunctionDepedencies>();

            var connectionString = config["LeaderboardDb"];

            CheckIsNotNull(nameof(connectionString), connectionString);
            
            services.AddDbContext<LeaderboardContext>(o =>
            {
                var cb = o.UseSqlServer(connectionString);
                cb.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Transient);
        }
    }
}
