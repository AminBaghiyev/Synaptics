using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.UpdatePostCommentReply;

public class UpdatePostCommentReplyHandler : IRequestHandler<UpdatePostCommentReplyCommand>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public UpdatePostCommentReplyHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(UpdatePostCommentReplyCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.UpdateReplyAsync(request.Reply, username);
        await _service.SaveChangesAsync();
    }
}
