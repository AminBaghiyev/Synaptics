using MediatR;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Queries.AppUser.GetAppUserProfile;

public class GetAppUserProfileHandler : IRequestHandler<GetAppUserProfileCommand, GetAppUserProfileDTO>
{
    readonly IAppUserService _userService;

    public GetAppUserProfileHandler(IAppUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetAppUserProfileDTO> Handle(GetAppUserProfileCommand request, CancellationToken cancellationToken)
    {
        return await _userService.GetProfileAsync(request.UserName);
    }
}
