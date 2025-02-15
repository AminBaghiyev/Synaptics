using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;

public class ChangeCoverPhotoAppUserCommand : IRequest<string>
{
    public ChangeCoverPhotoAppUserDTO File { get; set; }
}
