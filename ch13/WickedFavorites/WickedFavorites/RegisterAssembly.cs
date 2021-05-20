using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wicked.Favorites.Controllers;

namespace Wicked.Favorites
{
    public static class RegisterAssembly
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration config)
        {
            const string DATABASE_CONNECTION = "WickedFavoritesDb";

            string connectionString = config[DATABASE_CONNECTION];

            services.AddDbContext<FavoritesContext>(o =>
            {
                var cb = o.UseSqlServer(connectionString);
                cb.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Transient);

            services.AddTransient<IDependencies<HomeController>, BadDependencies<HomeController>>();

            services.AddAutoMapper(typeof(RegisterAssembly));
        }
    }
}
