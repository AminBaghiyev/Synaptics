using MediatR;

namespace Synaptics.Application.Commands.Post.LikePostCommentReply;

public class LikePostCommentReplyCommand : IRequest
{
    public long Id { get; set; }
}
