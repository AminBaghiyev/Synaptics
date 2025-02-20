using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.UnlikePostComment;

public class UnlikePostCommentHandler : IRequestHandler<UnlikePostCommentCommand>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public UnlikePostCommentHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(UnlikePostCommentCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.UnlikeAsync(request.CommentId, username);
        await _service.SaveChangesAsync();
    }
}
