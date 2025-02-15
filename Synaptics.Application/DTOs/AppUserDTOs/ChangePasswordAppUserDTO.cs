namespace Synaptics.Application.DTOs;

public record ChangePasswordAppUserDTO
{
    public string OriginalPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
