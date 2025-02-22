namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public record SearchAppUserQueryResponse
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
}
