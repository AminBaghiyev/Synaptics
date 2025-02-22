namespace Synaptics.Application.Queries.UserRelation.Followers;

public record FollowersQueryResponse
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public bool IsFollow { get; set; }
}
