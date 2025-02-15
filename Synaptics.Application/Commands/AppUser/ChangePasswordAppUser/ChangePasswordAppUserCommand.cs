using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;

public class ChangePasswordAppUserCommand : IRequest
{
    public ChangePasswordAppUserDTO Passwords { get; set; }
}
