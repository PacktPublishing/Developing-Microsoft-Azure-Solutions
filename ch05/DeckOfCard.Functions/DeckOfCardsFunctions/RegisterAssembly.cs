using Microsoft.Extensions.DependencyInjection;
using static Pineapple.Common.Preconditions;
using static Pineapple.Extensions.ServiceCollectionExtensions;

namespace DeckOfCards
{
    public static class RegisterAssembly
    {
        internal class StorageConfigured
        {
        }

        public static void AddStorage(this IServiceCollection services)
        {
            CheckIsNotNull(nameof(services), services);

            if (services.IsConfigured<StorageConfigured>())
                return;

            services.Configure<StorageConfigured>();

            void LoadMocks()
            {
                var store = InMemory.InMemoryStore.GetStore();
                Mocks.LoadMocks(store);
            }

            services.AddSingleton((sp) =>
            {
                LoadMocks();
                return InMemory.InMemoryStore.GetStore();
            });
        }

        internal class FunctionsConfigured
        {
        }

        public static void AddFunctions(this IServiceCollection services)
        {
            CheckIsNotNull(nameof(services), services);

            if (services.IsConfigured<FunctionsConfigured>())
                return;

            services.Configure<FunctionsConfigured>();

            services.AddTransient<IFunctionDepedencies, FunctionDepedencies>();
        }
    }
}
