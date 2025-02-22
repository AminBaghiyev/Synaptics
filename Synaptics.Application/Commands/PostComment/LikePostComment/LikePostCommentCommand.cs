using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostComment.LikePostComment;

public record LikePostCommentCommand : IRequest<Response>
{
    public long Id { get; set; }
}
