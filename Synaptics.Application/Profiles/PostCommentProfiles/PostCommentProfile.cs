using AutoMapper;
using Synaptics.Application.DTOs;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class PostCommentProfile : Profile
{
    public PostCommentProfile()
    {
        CreateMap<CreatePostCommentDTO, PostComment>();
        CreateMap<UpdatePostCommentDTO, PostComment>()
            .ReverseMap();
        CreateMap<CreatePostCommentReplyDTO, PostComment>();
        CreateMap<UpdatePostCommentReplyDTO, PostComment>()
            .ReverseMap();
        CreateMap<PostComment, PostCommentItemDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.ProfilePhotoPath, opt => opt.MapFrom(src => src.User.ProfilePhotoPath))
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
