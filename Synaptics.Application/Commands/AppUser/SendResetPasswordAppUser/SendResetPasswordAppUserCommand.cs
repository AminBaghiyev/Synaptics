using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.SendResetPasswordAppUser;

public record SendResetPasswordAppUserCommand : IRequest<Response>
{
    public string Username { get; set; }
}
