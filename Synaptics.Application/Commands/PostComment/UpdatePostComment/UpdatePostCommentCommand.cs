using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostComment.UpdatePostComment;

public record UpdatePostCommentCommand : IRequest<Response>
{
    public long Id { get; set; }
    public string Content { get; set; }
}
