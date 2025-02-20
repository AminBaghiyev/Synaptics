using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.CommentsOfPost;

public class CommentsOfPostHandler : IRequestHandler<CommentsOfPostQuery, ICollection<PostCommentItemDTO>>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public CommentsOfPostHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<PostCommentItemDTO>> Handle(CommentsOfPostQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _service.GetAllAsync(request.PostId, current: username, page: request.Page);
    }
}
