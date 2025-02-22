using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;

public record ChangePasswordAppUserCommand : IRequest<Response>
{
    public string OriginalPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
