using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.UserRelation.UnfollowUser;

public record UnfollowUserCommand : IRequest<Response>
{
    public string UnfollowTo { get; set; }
}
