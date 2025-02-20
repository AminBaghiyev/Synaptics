using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Queries.Post.PostCommentForUpdate;

public class PostCommentForUpdateHandler : IRequestHandler<PostCommentForUpdateQuery, UpdatePostCommentDTO>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public PostCommentForUpdateHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task<UpdatePostCommentDTO> Handle(PostCommentForUpdateQuery request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        return await _service.GetByIdForUpdateAsync(request.Id, username);
    }
}
