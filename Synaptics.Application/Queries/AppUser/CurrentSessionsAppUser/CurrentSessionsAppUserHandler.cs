using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Synaptics.Application.Queries.AppUser.CurrentSessionsAppUser;

public class CurrentSessionsAppUserHandler : IRequestHandler<CurrentSessionsAppUserQuery, Response>
{
    readonly IRedisService _redisService;
    readonly IHttpContextAccessor _contextAccessor;

    public CurrentSessionsAppUserHandler(IRedisService redisService, IHttpContextAccessor contextAccessor)
    {
        _redisService = redisService;
        _contextAccessor = contextAccessor;
    }

    public async Task<Response> Handle(CurrentSessionsAppUserQuery request, CancellationToken cancellationToken)
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

        Dictionary<string, string> sessions = await _redisService.GetAllFromHashByKeyAsync($"{request.Username}:refresh_tokens", request.Page);

        var sessionList = sessions.Select(s =>
        {
            var sessionData = JsonSerializer.Deserialize<CurrentSessionsAppUserQueryResponse>(s.Value);
            if (sessionData is null) return null;

            sessionData.Token = s.Key;
            sessionData.IsCurrent = token == s.Key;

            return sessionData;
        }).Where(s => s is not null).ToArray();

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = sessionList
        };
    }
}
