using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.UserRelation.Followings;

public record FollowingsQuery : IRequest<Response>
{
    public string UserName { get; set; }
    public int Page { get; set; }
}
