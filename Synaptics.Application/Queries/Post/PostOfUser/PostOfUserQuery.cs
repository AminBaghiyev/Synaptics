using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.Post.PostOfUser;

public record PostOfUserQuery : IRequest<Response>
{
    public long Id { get; set; }
}
