using MediatR;

namespace Synaptics.Application.Commands.Post.UnlikePostComment;

public class UnlikePostCommentCommand : IRequest
{
    public long CommentId { get; set; }
}
