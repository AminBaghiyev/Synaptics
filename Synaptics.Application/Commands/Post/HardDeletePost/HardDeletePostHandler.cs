using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.HardDeletePost;

public class HardDeletePostHandler : IRequestHandler<HardDeletePostCommand>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public HardDeletePostHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(HardDeletePostCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.HardDeleteAsync(request.Id, username);
        await _service.SaveChangesAsync();
    }
}
