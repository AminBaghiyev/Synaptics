using MediatR;

namespace Synaptics.Application.Commands.Post.UnlikePost;

public class UnlikePostCommand : IRequest
{
    public string UserName { get; set; }
    public long PostId { get; set; }
}
