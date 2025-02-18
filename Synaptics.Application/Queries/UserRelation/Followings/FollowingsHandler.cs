using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.UserRelation.Followings;

public class FollowingsHandler : IRequestHandler<FollowingsQuery, ICollection<FollowingDTO>>
{
    readonly IUserRelationService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public FollowingsHandler(IUserRelationService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<FollowingDTO>> Handle(FollowingsQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _service.GetFollowingsAsync(request.UserName, username, request.Page);
    }
}
