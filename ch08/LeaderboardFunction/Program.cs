using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Leaderboard
{
    class Program
    {
        static Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder
                    .AddCommandLine(args)
                    .AddJsonFile("local.settings.json", true)
                    .AddEnvironmentVariables();
                })
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((hostbuilder, services) =>
                {
                    var config = hostbuilder.Configuration;
                    services.AddFunctions(config);
                })
                .Build();

            return host.RunAsync();
        }
    }
}
