using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostCommentReply.UnlikePostCommentReply;

public record UnlikePostCommentReplyCommand : IRequest<Response>
{
    public long Id { get; set; }
}
