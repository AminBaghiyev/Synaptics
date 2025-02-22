using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;

public record ChangeCoverPhotoAppUserCommand : IRequest<Response>
{
    public IFormFile CoverPhoto { get; set; }
}
