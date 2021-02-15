using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public class Test
    {
        public Test()
        {
        }
    }

    public static class RegisterAssembly
    {
        private class DefaultDependencies : Dependencies<Test>
        {
            public DefaultDependencies(ILogger<Test> logger, AppSettings settings)
                : base(logger, settings)
            {

            }
        }

        public static void AddApplication(this IServiceCollection services)
        {
            CheckIsNotNull(nameof(services), services);

            services.AddSingleton<AppSettings>();
            services.AddSingleton<IDependencies<Test>, DefaultDependencies>();
        }
    }
}
