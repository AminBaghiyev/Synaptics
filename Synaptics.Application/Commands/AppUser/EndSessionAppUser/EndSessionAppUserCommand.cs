using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.EndSessionAppUser;

public record EndSessionAppUserCommand : IRequest<Response>
{
    public string Token { get; set; }
}
