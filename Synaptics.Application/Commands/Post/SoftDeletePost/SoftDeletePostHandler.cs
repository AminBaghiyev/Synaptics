using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.SoftDeletePost;

public class SoftDeletePostHandler : IRequestHandler<SoftDeletePostCommand>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public SoftDeletePostHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(SoftDeletePostCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.SoftDeleteAsync(request.Id, username);
        await _service.SaveChangesAsync();
    }
}
