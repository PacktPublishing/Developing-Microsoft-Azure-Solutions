using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shrinkify.Web.Controllers;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public static class RegisterAssembly
    {

        public static void AddApplication(this IServiceCollection services)
        {
            CheckIsNotNull(nameof(services), services);

            services.AddSingleton<AppSettings>();
            services.AddSingleton<IDependencies<HomeController>, HomeControllerDependencies>();
        }
    }
}
