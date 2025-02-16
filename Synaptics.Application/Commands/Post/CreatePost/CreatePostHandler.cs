using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.CreatePost;

public class CreatePostHandler : IRequestHandler<CreatePostCommand>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public CreatePostHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.CreateAsync(request.Post, username);
        await _service.SaveChangesAsync();
    }
}
