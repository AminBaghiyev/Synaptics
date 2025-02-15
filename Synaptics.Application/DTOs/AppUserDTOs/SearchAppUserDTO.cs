namespace Synaptics.Application.DTOs;

public record SearchAppUserDTO
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
}
