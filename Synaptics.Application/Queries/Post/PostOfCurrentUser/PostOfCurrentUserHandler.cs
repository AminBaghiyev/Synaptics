using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.PostsOfCurrentUser;

public class PostOfCurrentUserHandler : IRequestHandler<PostOfCurrentUserQuery, PostItemOfCurrentUserDTO>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostOfCurrentUserHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<PostItemOfCurrentUserDTO> Handle(PostOfCurrentUserQuery request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        return await _service.GetByIdOfCurrentUserAsync(request.Id, username);
    }
}
