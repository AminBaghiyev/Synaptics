using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.RecoverPost;

public class RecoverPostHandler : IRequestHandler<RecoverPostCommand>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public RecoverPostHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(RecoverPostCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.RecoverAsync(request.Id, username);
        await _service.SaveChangesAsync();
    }
}
