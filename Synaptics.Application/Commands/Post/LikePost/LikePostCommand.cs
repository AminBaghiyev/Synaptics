using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.Post.LikePost;

public record LikePostCommand : IRequest<Response>
{
    public long PostId { get; set; }
}
