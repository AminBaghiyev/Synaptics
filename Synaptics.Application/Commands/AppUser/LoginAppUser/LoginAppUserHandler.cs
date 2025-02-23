using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.LoginAppUser;

public class LoginAppUserHandler : IRequestHandler<LoginAppUserCommand, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IJWTTokenService _jWTTokenService;
    readonly IRedisService _redisService;
    readonly IUserDeviceInfoService _userDeviceInfoService;

    public LoginAppUserHandler(UserManager<Entities.AppUser> userManager, IJWTTokenService jWTTokenService, IRedisService redisService, IUserDeviceInfoService userDeviceInfoService)
    {
        _userManager = userManager;
        _jWTTokenService = jWTTokenService;
        _redisService = redisService;
        _userDeviceInfoService = userDeviceInfoService;
    }

    public async Task<Response> Handle(LoginAppUserCommand request, CancellationToken cancellationToken)
    {
        Entities.AppUser? user = await _userManager.FindByNameAsync(request.UserName.ToUpper());
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.CredentialsWrong
            };

        bool res = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!res)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.CredentialsWrong
            };

        string refreshToken = _jWTTokenService.GenerateRefreshToken();
        string deviceInfo = JsonConvert.SerializeObject(_userDeviceInfoService.GetDeviceInfo());

        await _redisService.SetHashAsync($"{user.UserName}:refresh_tokens", refreshToken, deviceInfo, TimeSpan.FromDays(7));

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = new
            {
                AccessToken = _jWTTokenService.GenerateToken([new("username", user.UserName)], TimeSpan.FromMinutes(15)),
                RefreshToken = _jWTTokenService.GenerateToken([new("token", refreshToken)], TimeSpan.FromDays(7))
            }
        };
    }
}
