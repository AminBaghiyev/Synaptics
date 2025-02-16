using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.UserRelation.Followers;

public class FollowersCommand : IRequest<ICollection<FollowerDTO>>
{
    public string UserName { get; set; }
    public int Page { get; set; }
}
