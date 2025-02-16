using MediatR;

namespace Synaptics.Application.Commands.Post.LikePost;

public class LikePostCommand : IRequest
{
    public string UserName { get; set; }
    public long PostId { get; set; }
}
