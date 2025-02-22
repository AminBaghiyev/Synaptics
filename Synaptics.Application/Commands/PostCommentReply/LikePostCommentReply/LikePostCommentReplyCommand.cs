using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostCommentReply.LikePostCommentReply;

public record LikePostCommentReplyCommand : IRequest<Response>
{
    public long Id { get; set; }
}
