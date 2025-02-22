using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.UserRelation.Followers;

public record FollowersQuery : IRequest<Response>
{
    public string UserName { get; set; }
    public int Page { get; set; }
}
