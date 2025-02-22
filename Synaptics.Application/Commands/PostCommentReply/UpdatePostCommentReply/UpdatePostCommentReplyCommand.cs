using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostCommentReply.UpdatePostCommentReply;

public record UpdatePostCommentReplyCommand : IRequest<Response>
{
    public long Id { get; set; }
    public string Content { get; set; }
}
