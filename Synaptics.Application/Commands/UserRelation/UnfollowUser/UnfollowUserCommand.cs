using MediatR;

namespace Synaptics.Application.Commands.UserRelation.UnfollowUser;

public class UnfollowUserCommand : IRequest
{
    public string UnfollowTo { get; set; }
}
