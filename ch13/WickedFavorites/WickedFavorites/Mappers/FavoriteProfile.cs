using AutoMapper;
using Wicked.Favorites;
using Wicked.Favorites.Models;

namespace Wicked.Favorites
{
    public class FavoriteProfile : Profile
    {
        public FavoriteProfile()
        {
            CreateMap<Favorite, FavoriteModel>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Url, opts => opts.MapFrom(src => src.Url))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category.Name));
        }
    }
}
