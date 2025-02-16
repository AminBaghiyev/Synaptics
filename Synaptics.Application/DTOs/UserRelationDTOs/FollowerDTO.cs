namespace Synaptics.Application.DTOs;

public record FollowerDTO
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public bool IsFollow { get; set; }
}