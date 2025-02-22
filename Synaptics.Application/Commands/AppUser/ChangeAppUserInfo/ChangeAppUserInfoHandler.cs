using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Common.Results;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;

public class ChangeAppUserInfoHandler : IRequestHandler<ChangeAppUserInfoCommand, Response>
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IPyBridgeService _pyBridgeService;
    readonly IQdrantService _qdrantService;
    readonly IMapper _mapper;

    public ChangeAppUserInfoHandler(IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IPyBridgeService pyBridgeService, IQdrantService qdrantService, IMapper mapper)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _pyBridgeService = pyBridgeService;
        _qdrantService = qdrantService;
        _mapper = mapper;
    }

    public async Task<Response> Handle(ChangeAppUserInfoCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        (PyBridgeResult pyRes, float[] selfDescriptionEmbedding) = await _pyBridgeService.EmbeddingAsync(request.SelfDescription);

        if (!pyRes.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        Entities.AppUser? user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        Entities.AppUser oldUser = _mapper.Map<Entities.AppUser>(user);

        _mapper.Map(request, user);
        IdentityResult res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        QdrantResult qdrantRes = await _qdrantService.UpdateDataAsync("users", Guid.Parse(user.Id), selfDescriptionEmbedding);

        if (!qdrantRes.Succeeded)
        {
            await _userManager.UpdateAsync(oldUser);
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };
        }

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
