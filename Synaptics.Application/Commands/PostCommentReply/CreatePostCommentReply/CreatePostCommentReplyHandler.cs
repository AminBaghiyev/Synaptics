using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.CreatePostCommentReply;

public class CreatePostCommentReplyHandler : IRequestHandler<CreatePostCommentReplyCommand>
{
    readonly IPostCommentService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public CreatePostCommentReplyHandler(IPostCommentService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(CreatePostCommentReplyCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.CreateReplyAsync(request.Reply, username);
        await _service.SaveChangesAsync();
    }
}
