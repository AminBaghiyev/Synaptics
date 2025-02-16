using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.UserRelation.Followings;

public class FollowingsCommand : IRequest<ICollection<FollowingDTO>>
{
    public string UserName { get; set; }
    public int Page { get; set; }
}
