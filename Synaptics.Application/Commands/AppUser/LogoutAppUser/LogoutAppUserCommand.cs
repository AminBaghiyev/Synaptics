using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.LogoutAppUser;

public record LogoutAppUserCommand : IRequest<Response>
{
    public string? Username { get; set; }
}
