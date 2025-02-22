using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostCommentReply.SoftDeletePostCommentReply;

public record SoftDeletePostCommentReplyCommand : IRequest<Response>
{
    public long Id { get; set; }
}
