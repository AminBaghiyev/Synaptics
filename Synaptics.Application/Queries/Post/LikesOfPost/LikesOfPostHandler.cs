using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.LikesOfPost;

public class LikesOfPostHandler : IRequestHandler<LikesOfPostCommand, ICollection<PostLikeUserDTO>>
{
    readonly IPostService _postService;
    readonly IHttpContextAccessor _contextAccessor;

    public LikesOfPostHandler(IPostService postService, IHttpContextAccessor contextAccessor)
    {
        _postService = postService;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<PostLikeUserDTO>> Handle(LikesOfPostCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _postService.GetLikesOfPostAsync(request.PostId, request.UserName, username, page: request.Page);
    }
}
