using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.ResetPasswordAppUser;

public record ResetPasswordAppUserCommand : IRequest<Response>
{
    public string Token { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
