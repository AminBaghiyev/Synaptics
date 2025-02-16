using MediatR;

namespace Synaptics.Application.Commands.UserRelation.RemoveFollower;

public class RemoveFollowerCommand : IRequest
{
    public string Follower { get; set; }
}
