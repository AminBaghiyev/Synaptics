namespace Synaptics.Application.Queries.AppUser.AISearchAppUser;

public record AISearchAppUserQueryResponse
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public string SelfDescription { get; set; }
    public float Score { get; set; }
}
