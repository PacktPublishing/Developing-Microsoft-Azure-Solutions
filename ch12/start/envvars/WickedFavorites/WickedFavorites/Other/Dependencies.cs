using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Wicked.Favorites
{
    internal class Dependencies<T> : IDependencies<T>
    {
        public Dependencies(IMapper mapper, ILogger<T> logger, FavoritesContext context)
        {
            Mapper = mapper;
            Logger = logger;
            Context = context;
        }

        public IMapper Mapper { get; }
        public ILogger<T> Logger { get; }

        public FavoritesContext Context { get; }
    }
}
