using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.SoftDeletePostCommentReply;

public class SoftDeletePostCommentReplyHandler : IRequestHandler<SoftDeletePostCommentReplyCommand>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public SoftDeletePostCommentReplyHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(SoftDeletePostCommentReplyCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.SoftDeleteAsync(request.Id, username);
        await _service.SaveChangesAsync();
    }
}
