using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.Post.UnlikePost;

public record UnlikePostCommand : IRequest<Response>
{
    public long PostId { get; set; }
}
