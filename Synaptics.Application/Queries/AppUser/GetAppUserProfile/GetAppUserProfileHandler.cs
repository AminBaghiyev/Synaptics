using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;
using System.Net;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.AppUser.GetAppUserProfile;

public class GetAppUserProfileHandler : IRequestHandler<GetAppUserProfileQuery, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public GetAppUserProfileHandler(UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(GetAppUserProfileQuery request, CancellationToken cancellationToken)
    {
        Entities.AppUser? user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<GetAppUserProfileQueryResponse>(user)
        };
    }
}
