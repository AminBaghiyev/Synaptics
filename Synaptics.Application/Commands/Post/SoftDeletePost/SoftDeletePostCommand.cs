using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.Post.SoftDeletePost;

public record SoftDeletePostCommand : IRequest<Response>
{
    public long Id { get; set; }
}
