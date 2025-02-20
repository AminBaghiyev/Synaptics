using MediatR;

namespace Synaptics.Application.Commands.Post.LikePostComment;

public class LikePostCommentCommand : IRequest
{
    public long CommentId { get; set; }
}
