using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.UnlikePostCommentReply;

public class UnlikePostCommentReplyHandler : IRequestHandler<UnlikePostCommentReplyCommand>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public UnlikePostCommentReplyHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(UnlikePostCommentReplyCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.UnlikeAsync(request.Id, username);
        await _service.SaveChangesAsync();
    }
}
