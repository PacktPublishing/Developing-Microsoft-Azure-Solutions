using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public class ProfileContext : IDisposable
    {
        private const string DatabaseId = "DeckOfCards";
        private const string ContainerId = "Profile";

        private static readonly string _endpoint;
        private static readonly string _authKey;
        private readonly CosmosClient _client;

        static ProfileContext()
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

            _endpoint = config["EndPointUrl"];
            CheckIsNotNullOrWhitespace(nameof(_endpoint), _endpoint);

            _authKey = config["AuthorizationKey"];
            CheckIsNotNullOrWhitespace(nameof(_authKey), _authKey);
            CheckIsNotCondition(nameof(_authKey), _authKey == "Please specify the key", "Please specify the key");
        }

        public ProfileContext()
        {
            _client = new CosmosClient(_endpoint, _authKey);
        }

        ~ProfileContext()
        {
            DisposeInternal();
        }

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeInternal()
        {
            _client.Dispose();
        }

        private async Task<Container> GetContainer()
        {
            var databaseResponse = await _client.CreateDatabaseIfNotExistsAsync(DatabaseId);
            databaseResponse.EnsureSuccess();

            var database = databaseResponse.Database;
            var containerProperties = new ContainerProperties(ContainerId, partitionKeyPath: "/id");
            
            var containerResponse = await database.CreateContainerIfNotExistsAsync(
                containerProperties,
                throughput: 1000);

            return containerResponse.Container;
        }

        public async Task AddProfile(Profile profile)
        {
            CheckIsNotNull(nameof(profile), profile);

            var container = await GetContainer();
            var itemResponse = await container.CreateItemAsync(profile,
                new PartitionKey(profile.UserId.ToString()),
                new ItemRequestOptions()
                {
                    EnableContentResponseOnWrite = false
                });

            itemResponse.EnsureSuccess();
        }

        public async Task<Profile> GetProfile(Guid id)
        {
            CheckIsNotNull(nameof(id), id);

            string idToFind = id.ToString();

            var container = await GetContainer();
            
            return await container.ReadItemAsync<Profile>(idToFind, new PartitionKey(idToFind));
        }
    }
}
