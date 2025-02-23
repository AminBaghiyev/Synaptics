using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;

namespace Synaptics.Application.Queries.AppUser.GetAccessTokenAppUser;

public class GetAccessTokenAppUserHandler : IRequestHandler<GetAccessTokenAppUserQuery, Response>
{
    readonly IRedisService _redisService;
    readonly IHttpContextAccessor _contextAccessor;
    readonly IJWTTokenService _jwtTokenService;
    readonly IUserDeviceInfoService _userDeviceInfoService;

    public GetAccessTokenAppUserHandler(IRedisService redisService, IHttpContextAccessor contextAccessor, IJWTTokenService jwtTokenService, IUserDeviceInfoService userDeviceInfoService)
    {
        _redisService = redisService;
        _contextAccessor = contextAccessor;
        _jwtTokenService = jwtTokenService;
        _userDeviceInfoService = userDeviceInfoService;
    }

    public async Task<Response> Handle(GetAccessTokenAppUserQuery request, CancellationToken cancellationToken)
    {
        string? token = _contextAccessor.HttpContext?.User.FindFirstValue("token");
        if (request.Username is null || token is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        if (await _redisService.GetFieldFromHashAsync($"{request.Username}:refresh_tokens", token) is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        await _redisService.DeleteHashAsync($"{request.Username}:refresh_tokens", token);

        string refreshToken = _jwtTokenService.GenerateRefreshToken();
        string deviceInfo = JsonConvert.SerializeObject(_userDeviceInfoService.GetDeviceInfo());

        await _redisService.SetHashAsync($"{request.Username}:refresh_tokens", refreshToken, deviceInfo, TimeSpan.FromDays(7));

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = new GetAccessTokenAppUserQueryResponse
            {
                AccessToken = _jwtTokenService.GenerateToken([new("username", request.Username)], TimeSpan.FromMinutes(15)),
                RefreshToken = _jwtTokenService.GenerateToken([new("token", refreshToken)], TimeSpan.FromDays(7))
            }
        };
    }
}
