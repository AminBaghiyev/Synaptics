using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.AppUser.RegisterAppUser;

public class RegisterAppUserCommand : IRequest<string>
{
    public RegisterAppUserDTO AppUser { get; set; }
}
