using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.PostForUpdate;

public class PostForUpdateHandler : IRequestHandler<PostForUpdateCommand, UpdatePostDTO>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostForUpdateHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<UpdatePostDTO> Handle(PostForUpdateCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        return await _service.GetByIdOfCurrentUserForUpdateAsync(request.Id, username);
    }
}
