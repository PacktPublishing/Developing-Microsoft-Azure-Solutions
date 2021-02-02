using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkHide.Web
{
    public static class RegisterAssembly
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddControllersWithViews();
        }
    }
}
