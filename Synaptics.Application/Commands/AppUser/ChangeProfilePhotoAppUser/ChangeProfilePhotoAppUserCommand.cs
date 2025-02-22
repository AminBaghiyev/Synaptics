using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;

public record ChangeProfilePhotoAppUserCommand : IRequest<Response>
{
    public IFormFile ProfilePhoto { get; set; }
}
