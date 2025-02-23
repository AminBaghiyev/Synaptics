using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.AppUser.CurrentSessionsAppUser;

public record CurrentSessionsAppUserQuery : IRequest<Response>
{
    public string? Username { get; set; }
    public int Page { get; set; }
}
