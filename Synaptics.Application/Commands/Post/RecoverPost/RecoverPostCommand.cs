using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.Post.RecoverPost;

public record RecoverPostCommand : IRequest<Response>
{
    public long Id { get; set; }
}
