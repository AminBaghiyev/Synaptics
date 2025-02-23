using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;

namespace Synaptics.Application.Commands.AppUser.LogoutAppUser;

public class LogoutAppUserHandler : IRequestHandler<LogoutAppUserCommand, Response>
{
    readonly IRedisService _redisService;
    readonly IHttpContextAccessor _contextAccessor;

    public LogoutAppUserHandler(IRedisService redisService, IHttpContextAccessor contextAccessor)
    {
        _redisService = redisService;
        _contextAccessor = contextAccessor;
    }

    public async Task<Response> Handle(LogoutAppUserCommand request, CancellationToken cancellationToken)
    {
        string? token = _contextAccessor.HttpContext?.User.FindFirstValue("token");
        if (request.Username is null || token is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        string? metadata = await _redisService.GetFieldFromHashAsync($"{request.Username}:refresh_tokens", token);
        if (metadata is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        await _redisService.DeleteHashAsync($"{request.Username}:refresh_tokens", token);

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
