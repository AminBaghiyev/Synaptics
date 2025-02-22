using AutoMapper;
using Synaptics.Application.Commands.PostCommentReply.CreatePostCommentReply;
using Synaptics.Application.Commands.PostCommentReply.UpdatePostCommentReply;
using Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;
using Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class PostCommentReplyProfile : Profile
{
    public PostCommentReplyProfile()
    {
        CreateMap<CreatePostCommentReplyCommand, PostComment>();

        CreateMap<UpdatePostCommentReplyCommand, PostComment>();
        CreateMap<PostComment, PostCommentReplyForUpdateQueryResponse>();

        CreateMap<PostComment, RepliesOfPostCommentQueryResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.ProfilePhotoPath, opt => opt.MapFrom(src => src.User.ProfilePhotoPath))
            .ForMember(dest => dest.LastTime, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
