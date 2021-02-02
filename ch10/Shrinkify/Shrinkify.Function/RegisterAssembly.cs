using Microsoft.Extensions.DependencyInjection;
using static Pineapple.Common.Preconditions;
using static Pineapple.Extensions.ServiceCollectionExtensions;

namespace Shrinkify
{
    public static class RegisterAssembly
    {
        public static void AddApplication(this IServiceCollection services)
        {
            CheckIsNotNull(nameof(services), services);

            services.AddSingleton<AppSettings>();
            services.AddTransient<IDependencies<Functions>, FunctionsDependencies>();
        }
    }
}
