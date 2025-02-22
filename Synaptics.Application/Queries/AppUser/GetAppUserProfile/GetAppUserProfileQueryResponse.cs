using Synaptics.Domain.Enums;

namespace Synaptics.Application.Queries.AppUser.GetAppUserProfile;

public record GetAppUserProfileQueryResponse
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePhotoPath { get; set; }
    public string? CoverPhotoPath { get; set; }
    public Gender Gender { get; set; }
    public string Biography { get; set; }
}
