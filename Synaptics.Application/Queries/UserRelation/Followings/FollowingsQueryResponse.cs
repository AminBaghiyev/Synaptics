namespace Synaptics.Application.Queries.UserRelation.Followings;

public record FollowingsQueryResponse
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public bool IsFollow { get; set; }
}
