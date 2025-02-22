using AutoMapper;
using Synaptics.Application.Commands.Post.CreatePost;
using Synaptics.Application.Commands.Post.UpdatePost;
using Synaptics.Application.Queries.Post.LikesOfPost;
using Synaptics.Application.Queries.Post.PostForUpdate;
using Synaptics.Application.Queries.Post.PostOfUser;
using Synaptics.Application.Queries.Post.PostsOfUser;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<CreatePostCommand, Post>();

        CreateMap<UpdatePostCommand, Post>();
        CreateMap<Post, PostForUpdateQueryResponse>();

        CreateMap<Post, PostOfCurrentUserQueryResponse>()
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
        CreateMap<Post, PostsOfCurrentUserQueryResponse>()
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<Post, PostOfUserQueryResponse>()
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
        CreateMap<Post, PostsOfUserQueryResponse>()
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<PostLike, LikesOfPostQueryResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.ProfilePhotoPath, opt => opt.MapFrom(src => src.User.ProfilePhotoPath));
    }
}
