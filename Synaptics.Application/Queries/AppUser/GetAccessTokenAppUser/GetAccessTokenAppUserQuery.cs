using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.AppUser.GetAccessTokenAppUser;

public record GetAccessTokenAppUserQuery : IRequest<Response>
{
    public string? Username { get; set; }
}
