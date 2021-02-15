using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Shrinkify
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestManager
    {
        public static ILogger Logger { get; private set; }

        public static IServiceProvider ServiceProvider { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        public static T GetService<T>() where T: class
        {
            return ServiceProvider.GetService<T>();
        }

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables().Build();

            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(loggingBuilder => loggingBuilder
                .AddConsole()
                .AddDebug()
                .SetMinimumLevel(LogLevel.Debug));

            services.AddSingleton(Configuration);

            Configure(services, Configuration);

            ServiceProvider = services.BuildServiceProvider();

            var logger = ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger<TestManager>();

            Logger = logger;
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
        }

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
        }
    }
}
