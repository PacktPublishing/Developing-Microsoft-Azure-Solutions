using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Wicked.Favorites
{
internal class BadDependencies<T> : IDependencies<T>
{
    private static int count = 0;

    private FavoritesContext _favoritesContext;

    public BadDependencies(IMapper mapper, ILogger<T> logger, FavoritesContext context)
    {
        Mapper = mapper;
        Logger = logger;
        _favoritesContext = context;
    }

    public IMapper Mapper { get; }
    public ILogger<T> Logger { get; }

    public FavoritesContext Context
    {
        get
        {
            count++;
            return count % 5 == 0 ? null : _favoritesContext;
        }
    }
}
}
