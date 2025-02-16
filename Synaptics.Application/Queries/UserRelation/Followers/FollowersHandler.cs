using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.UserRelation.Followers;

public class FollowersHandler : IRequestHandler<FollowersCommand, ICollection<FollowerDTO>>
{
    readonly IUserRelationService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public FollowersHandler(IUserRelationService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<FollowerDTO>> Handle(FollowersCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _service.GetFollowersAsync(request.UserName, username, request.Page);
    }
}
