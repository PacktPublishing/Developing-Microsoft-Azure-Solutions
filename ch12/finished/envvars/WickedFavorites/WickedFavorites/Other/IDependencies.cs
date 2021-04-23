using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Wicked.Favorites
{
    public interface IDependencies<T>
    {
        FavoritesContext Context { get; }
        ILogger<T> Logger { get; }
        IMapper Mapper { get; }
    }
}