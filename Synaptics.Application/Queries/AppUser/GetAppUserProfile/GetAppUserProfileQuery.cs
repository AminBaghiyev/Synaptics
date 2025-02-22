using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.AppUser.GetAppUserProfile;

public record GetAppUserProfileQuery : IRequest<Response>
{
    public string UserName { get; set; }
}
