using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;

public class ChangeProfilePhotoAppUserCommand : IRequest<string>
{
    public ChangeProfilePhotoAppUserDTO File { get; set; }
}
