using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Structure.IO.GraphSON;
using static Gremlin.Net.Process.Traversal.AnonymousTraversalSource;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;
using Gremlin.Net.Process.Traversal;

namespace DeckOfCards
{
    public class ProfileContext : IDisposable
    {
        private const string DatabaseId = "DeckOfCards";
        private const string ContainerId = "Profile";
        private static readonly string ContainerLink = $@"/dbs/{DatabaseId}/colls/{ContainerId}";

        private static readonly string _host;
        private static readonly string _primaryKey;
        private readonly GremlinClient _client;

        static ProfileContext()
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

            _host = config["Host"];
            CheckIsNotNullOrWhitespace(nameof(_host), _host);

            _primaryKey = config["PrimaryKey"];
            CheckIsNotNullOrWhitespace(nameof(_primaryKey), _primaryKey);
            CheckIsNotCondition(nameof(_primaryKey), _primaryKey == "Please specify the key", "Please specify the key");
        }

        public ProfileContext()
        {
            ConnectionPoolSettings connectionPoolSettings = new ConnectionPoolSettings()
            {
                MaxInProcessPerConnection = 10,
                PoolSize = 30,
                ReconnectionAttempts = 3,
                ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
            };

            var webSocketConfiguration =
                new Action<ClientWebSocketOptions>(options =>
                {
                    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                });

            var gremlinServer = new GremlinServer(_host, 443, enableSsl: true,
                                        username: ContainerLink,
                                        password: _primaryKey);

            _client = new GremlinClient(
                gremlinServer,
                new GraphSON2Reader(),
                new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType,
                connectionPoolSettings,
                webSocketConfiguration);
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

        private GraphTraversalSource GetTraversalSource()
        {
            return Traversal().WithRemote(new DriverRemoteConnection(_client));
        }

        public async Task AddProfile(Profile profile)
        {
            CheckIsNotNull(nameof(profile), profile);

            var g = GetTraversalSource();

            var p = g.AddV(nameof(Profile));

            p.Property(nameof(profile.UserId).ToLower(), profile.UserId)
             .Property(nameof(profile.FirstName).ToLower(), profile.FirstName)
             .Property(nameof(profile.LastName).ToLower(), profile.LastName);

            p.AddV(nameof(profile.Location).ToLower())
             .Property(nameof(profile.Location.City).ToLower(), profile.Location.City)
             .Property(nameof(profile.Location.State).ToLower(), profile.Location.State);

            p.AddV(nameof(profile.Favorite).ToLower())
             .Property(nameof(profile.Favorite.Name).ToLower(), profile.Favorite.Name)
             .Property(nameof(profile.Favorite.WorkoutId).ToLower(), profile.Favorite.WorkoutId);

            await Task.CompletedTask;
        }

        public async Task<Profile> GetProfile(Guid id)
        {
            return await Task.FromResult<Profile>(null);

            var g = GetTraversalSource();

            var p = g.V().HasLabel("Profile").HasId(id);

            var userid = p.Property()

            return g;
        }
    }
}
