using MediatR;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Web;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.ResetPasswordAppUser;

public class ResetPasswordAppUserHandler : IRequestHandler<ResetPasswordAppUserCommand, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IRedisService _redisService;

    public ResetPasswordAppUserHandler(UserManager<Entities.AppUser> userManager, IRedisService redisService)
    {
        _userManager = userManager;
        _redisService = redisService;
    }

    public async Task<Response> Handle(ResetPasswordAppUserCommand request, CancellationToken cancellationToken)
    {
        Entities.AppUser? user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        if (await _userManager.CheckPasswordAsync(user, request.Password))
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                MessageCode = MessageCode.PasswordsCantBeSame
            };

        IdentityResult res = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(request.Token), request.Password);

        if (!res.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        await _redisService.DeleteAllFromHashByKeyAsync($"{user.UserName}:refresh_tokens");

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
