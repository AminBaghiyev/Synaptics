using Microsoft.AspNetCore.Http;

namespace Synaptics.Application.DTOs;

public record ChangeProfilePhotoAppUserDTO
{
    public IFormFile ProfilePhoto { get; set; }
}
