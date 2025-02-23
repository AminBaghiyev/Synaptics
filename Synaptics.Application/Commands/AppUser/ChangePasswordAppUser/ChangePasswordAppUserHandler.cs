using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;

public class ChangePasswordAppUserHandler : IRequestHandler<ChangePasswordAppUserCommand, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IHttpContextAccessor _contextAccessor;
    readonly IRedisService _redisService;

    public ChangePasswordAppUserHandler(UserManager<Entities.AppUser> userManager, IHttpContextAccessor contextAccessor, IRedisService redisService)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _redisService = redisService;
    }

    public async Task<Response> Handle(ChangePasswordAppUserCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        Entities.AppUser? user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        IdentityResult res = await _userManager.ChangePasswordAsync(user, request.OriginalPassword, request.NewPassword);

        if (!res.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.CredentialsWrong
            };

        await _redisService.DeleteAllFromHashByKeyAsync($"{user.UserName}:refresh_tokens");

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
