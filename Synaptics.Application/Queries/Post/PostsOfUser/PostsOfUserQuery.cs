using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.Post.PostsOfUser;

public record PostsOfUserQuery : IRequest<Response>
{
    public string UserName { get; set; }
    public int Page { get; set; }
}
