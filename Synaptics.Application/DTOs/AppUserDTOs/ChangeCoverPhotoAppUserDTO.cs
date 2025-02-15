using Microsoft.AspNetCore.Http;

namespace Synaptics.Application.DTOs;

public record ChangeCoverPhotoAppUserDTO
{
    public IFormFile CoverPhoto { get; set; }
}
