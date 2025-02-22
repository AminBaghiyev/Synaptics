using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.AppUser.GetAppUserInfo;

public class GetAppUserInfoHandler : IRequestHandler<GetAppUserInfoQuery, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IHttpContextAccessor _contextAccessor;
    readonly IMapper _mapper;

    public GetAppUserInfoHandler(UserManager<Entities.AppUser> userManager, IHttpContextAccessor contextAccessor, IMapper mapper)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _mapper = mapper;
    }

    public async Task<Response> Handle(GetAppUserInfoQuery request, CancellationToken cancellationToken)
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

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<GetAppUserInfoQueryResponse>(user)
        };
    }
}
