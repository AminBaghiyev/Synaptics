using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.AppUser.GetAppUserProfile;

public class GetAppUserProfileCommand : IRequest<GetAppUserProfileDTO>
{
    public string UserName { get; set; }
}
