using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.UserRelation.RemoveFollower;

public class RemoveFollowerHandler : IRequestHandler<RemoveFollowerCommand>
{
    readonly IUserRelationService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public RemoveFollowerHandler(IUserRelationService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(RemoveFollowerCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        if (username.ToUpper() == request.Follower.ToUpper()) throw new ExternalException("You do not follow yourself anyway");

        await _service.RemoveAsync(username, request.Follower);
        await _service.SaveChangesAsync();
    }
}
