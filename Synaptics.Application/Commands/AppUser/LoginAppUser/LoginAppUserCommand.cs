using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.AppUser.LoginAppUser;

public class LoginAppUserCommand : IRequest<string>
{
    public LoginAppUserDTO AppUser { get; set; }
}
