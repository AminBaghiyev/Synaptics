using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.UserRelation.FollowUser;

public record FollowUserCommand : IRequest<Response>
{
    public string FollowTo { get; set; }
}
