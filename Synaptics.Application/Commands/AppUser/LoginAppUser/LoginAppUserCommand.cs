using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.LoginAppUser;

public record LoginAppUserCommand : IRequest<Response>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
