using AutoMapper;
using Synaptics.Application.Commands.PostComment.CreatePostComment;
using Synaptics.Application.Commands.PostComment.UpdatePostComment;
using Synaptics.Application.Queries.PostComment.CommentsOfPost;
using Synaptics.Application.Queries.PostComment.PostCommentForUpdate;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class PostCommentProfile : Profile
{
    public PostCommentProfile()
    {
        CreateMap<CreatePostCommentCommand, PostComment>();

        CreateMap<UpdatePostCommentCommand, PostComment>();
        CreateMap<PostComment, PostCommentForUpdateQueryResponse>();

        CreateMap<PostComment, CommentsOfPostQueryResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.ProfilePhotoPath, opt => opt.MapFrom(src => src.User.ProfilePhotoPath))
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
