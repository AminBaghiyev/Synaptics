using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.AppUser.GetAppUserInfo;

public class GetAppUserInfoHandler : IRequestHandler<GetAppUserInfoCommand, ChangeAppUserInfoDTO>
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

    public async Task<ChangeAppUserInfoDTO> Handle(GetAppUserInfoCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        return _mapper.Map<ChangeAppUserInfoDTO>(await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!"));
    }
}
