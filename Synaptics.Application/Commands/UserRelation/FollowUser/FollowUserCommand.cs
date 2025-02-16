using MediatR;

namespace Synaptics.Application.Commands.UserRelation.FollowUser;

public class FollowUserCommand : IRequest
{
    public string FollowTo { get; set; }
}
