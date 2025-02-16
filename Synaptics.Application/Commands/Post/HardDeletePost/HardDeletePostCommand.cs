using MediatR;

namespace Synaptics.Application.Commands.Post.HardDeletePost;

public class HardDeletePostCommand : IRequest
{
    public long Id { get; set; }
}
