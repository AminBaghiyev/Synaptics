using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.LikePost;

public class LikePostHandler : IRequestHandler<LikePostCommand>
{
    readonly IPostService _postService;
    readonly IHttpContextAccessor _contextAccessor;

    public LikePostHandler(IPostService postService, IHttpContextAccessor contextAccessor)
    {
        _postService = postService;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _postService.LikeAsync(request.PostId, request.UserName, username);
        await _postService.SaveChangesAsync();
    }
}
