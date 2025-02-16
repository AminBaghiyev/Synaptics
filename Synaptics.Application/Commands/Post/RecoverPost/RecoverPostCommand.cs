using MediatR;

namespace Synaptics.Application.Commands.Post.RecoverPost;

public class RecoverPostCommand : IRequest
{
    public long Id { get; set; }
}
