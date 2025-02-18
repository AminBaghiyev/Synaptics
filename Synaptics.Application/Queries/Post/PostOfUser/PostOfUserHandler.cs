using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.PostOfUser;

public class PostOfUserHandler : IRequestHandler<PostOfUserQuery, PostItemDTO>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostOfUserHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<PostItemDTO> Handle(PostOfUserQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _service.GetByIdAsync(request.Id, request.UserName, username);
    }
}
