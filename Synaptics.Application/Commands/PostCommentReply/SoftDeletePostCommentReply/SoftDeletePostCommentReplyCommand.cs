using MediatR;

namespace Synaptics.Application.Commands.Post.SoftDeletePostCommentReply;

public class SoftDeletePostCommentReplyCommand : IRequest
{
    public long Id { get; set; }
}
