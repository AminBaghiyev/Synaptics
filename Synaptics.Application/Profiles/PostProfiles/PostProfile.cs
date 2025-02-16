using AutoMapper;
using Synaptics.Application.DTOs;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<CreatePostDTO, Post>();
        CreateMap<UpdatePostDTO, Post>()
            .ReverseMap();
        CreateMap<Post, PostItemDTO>()
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
        CreateMap<Post, PostItemOfCurrentUserDTO>()
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
