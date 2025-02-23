using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;

namespace Synaptics.Application.Commands.AppUser.EndSessionAppUser;

public class EndSessionAppUserHandler : IRequestHandler<EndSessionAppUserCommand, Response>
{
    readonly IRedisService _redisService;
    readonly IHttpContextAccessor _contextAccessor;

    public EndSessionAppUserHandler(IRedisService redisService, IHttpContextAccessor contextAccessor)
    {
        _redisService = redisService;
        _contextAccessor = contextAccessor;
    }

    public async Task<Response> Handle(EndSessionAppUserCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        if (await _redisService.GetFieldFromHashAsync($"{username}:refresh_tokens", request.Token) is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.SessionNotFound
            };

        await _redisService.DeleteHashAsync($"{username}:refresh_tokens", request.Token);

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
