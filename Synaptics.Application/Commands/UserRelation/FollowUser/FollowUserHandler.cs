using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.UserRelation.FollowUser;

public class FollowUserHandler : IRequestHandler<FollowUserCommand>
{
    readonly IUserRelationService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public FollowUserHandler(IUserRelationService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        if (username.ToUpper() == request.FollowTo.ToUpper()) throw new ExternalException("You can not follow yourself");

        await _service.FollowAsync(username, request.FollowTo);
        await _service.SaveChangesAsync();
    }
}
