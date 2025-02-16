using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.UserRelation.UnfollowUser;

public class UnfollowUserHandler : IRequestHandler<UnfollowUserCommand>
{
    readonly IUserRelationService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public UnfollowUserHandler(IUserRelationService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        if (username.ToUpper() == request.UnfollowTo.ToUpper()) throw new ExternalException("You do not follow yourself anyway");

        await _service.UnfollowAsync(username, request.UnfollowTo);
        await _service.SaveChangesAsync();
    }
}
