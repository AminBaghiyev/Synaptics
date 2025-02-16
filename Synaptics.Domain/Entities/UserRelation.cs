namespace Synaptics.Domain.Entities;

public class UserRelation
{
    public string FollowerId { get; set; }
    public AppUser Follower { get; set; }

    public string FollowingId { get; set; }
    public AppUser Following { get; set; }

    public DateTime FollowedAt { get; set; }
}
