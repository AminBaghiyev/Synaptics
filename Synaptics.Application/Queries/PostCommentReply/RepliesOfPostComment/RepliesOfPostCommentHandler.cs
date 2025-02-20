using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;

public class RepliesOfPostCommentHandler : IRequestHandler<RepliesOfPostCommentQuery, ICollection<PostCommentItemDTO>>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public RepliesOfPostCommentHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<ICollection<PostCommentItemDTO>> Handle(RepliesOfPostCommentQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        return await _service.GetAllRepliesAsync(request.ParentId, current: username, page: request.Page);
    }
}
