using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;

public class PostCommentReplyForUpdateHandler : IRequestHandler<PostCommentReplyForUpdateQuery, UpdatePostCommentReplyDTO>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostCommentReplyForUpdateHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<UpdatePostCommentReplyDTO> Handle(PostCommentReplyForUpdateQuery request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        return await _service.GetReplyByIdForUpdateAsync(request.Id, username);
    }
}
