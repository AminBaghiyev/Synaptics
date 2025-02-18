using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.PostsOfUser;

public class PostsOfUserHandler : IRequestHandler<PostsOfUserQuery, ICollection<PostItemDTO>>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostsOfUserHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<PostItemDTO>> Handle(PostsOfUserQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _service.GetAllAsync(request.UserName, current: username, page: request.Page);
    }
}
