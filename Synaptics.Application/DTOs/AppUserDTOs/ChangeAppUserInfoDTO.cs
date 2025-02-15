using Synaptics.Domain.Enums;

namespace Synaptics.Application.DTOs;

public record ChangeAppUserInfoDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string Biography { get; set; }
    public string SelfDescription { get; set; }
}
