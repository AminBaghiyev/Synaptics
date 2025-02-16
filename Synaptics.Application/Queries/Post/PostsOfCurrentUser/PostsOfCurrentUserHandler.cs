using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.PostsOfCurrentUser;

public class PostsOfCurrentUserHandler : IRequestHandler<PostsOfCurrentUserCommand, ICollection<PostItemOfCurrentUserDTO>>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostsOfCurrentUserHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<PostItemOfCurrentUserDTO>> Handle(PostsOfCurrentUserCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        return await _service.GetAllOfCurrentUserAsync(username, page: request.Page);
    }
}
