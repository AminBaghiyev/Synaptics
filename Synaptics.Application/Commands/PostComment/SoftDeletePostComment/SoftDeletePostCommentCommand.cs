using MediatR;

namespace Synaptics.Application.Commands.Post.SoftDeletePostComment;

public class SoftDeletePostCommentCommand : IRequest
{
    public long Id { get; set; }
}
