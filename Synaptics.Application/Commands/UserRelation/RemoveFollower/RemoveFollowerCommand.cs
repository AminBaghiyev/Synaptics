using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.UserRelation.RemoveFollower;

public record RemoveFollowerCommand : IRequest<Response>
{
    public string Follower { get; set; }
}
