using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostComment.SoftDeletePostComment;

public record SoftDeletePostCommentCommand : IRequest<Response>
{
    public long Id { get; set; }
}
