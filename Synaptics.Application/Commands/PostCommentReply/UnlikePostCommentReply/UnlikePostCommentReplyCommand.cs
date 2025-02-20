using MediatR;

namespace Synaptics.Application.Commands.Post.UnlikePostCommentReply;

public class UnlikePostCommentReplyCommand : IRequest
{
    public long Id { get; set; }
}
