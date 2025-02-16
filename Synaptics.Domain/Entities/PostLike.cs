namespace Synaptics.Domain.Entities;

public class PostLike
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public long PostId { get; set; }
    public Post Post { get; set; }
    public DateTime LikedAt { get; set; }
}
