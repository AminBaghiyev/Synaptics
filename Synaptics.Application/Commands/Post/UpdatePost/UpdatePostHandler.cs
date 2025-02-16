using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.Post.UpdatePost;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand>
{
    readonly IPostService _service;
    readonly IHttpContextAccessor _contextAccessor;

    public UpdatePostHandler(IPostService service, IHttpContextAccessor contextAccessor)
    {
        _service = service;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _service.UpdateAsync(request.Post, username);
        await _service.SaveChangesAsync();
    }
}
