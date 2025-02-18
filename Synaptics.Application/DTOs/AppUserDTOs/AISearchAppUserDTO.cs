namespace Synaptics.Application.DTOs;

public record AISearchAppUserDTO
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public string SelfDescription { get; set; }
    public float Score { get; set; }
}
