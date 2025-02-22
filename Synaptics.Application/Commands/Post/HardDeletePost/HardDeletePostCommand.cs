using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.Post.HardDeletePost;

public record HardDeletePostCommand : IRequest<Response>
{
    public long Id { get; set; }
}
