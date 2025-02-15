using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using System.Security.Claims;

namespace Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;

public class ChangeAppUserInfoHandler : IRequestHandler<ChangeAppUserInfoCommand>
{
    readonly IAppUserService _userService;
    readonly IHttpContextAccessor _contextAccessor;

    public ChangeAppUserInfoHandler(IAppUserService userService, IHttpContextAccessor contextAccessor)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(ChangeAppUserInfoCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        await _userService.ChangeInfoAsync(username, request.Info);
    }
}
