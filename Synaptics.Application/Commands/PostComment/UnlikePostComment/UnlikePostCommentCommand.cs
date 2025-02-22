using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostComment.UnlikePostComment;

public record UnlikePostCommentCommand : IRequest<Response>
{
    public long Id { get; set; }
}
