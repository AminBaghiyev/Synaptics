using MediatR;

namespace Synaptics.Application.Commands.Post.SoftDeletePost;

public class SoftDeletePostCommand : IRequest
{
    public long Id { get; set; }
}
