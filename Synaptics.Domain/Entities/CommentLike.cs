namespace Synaptics.Domain.Entities;

public class CommentLike
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public long CommentId { get; set; }
    public PostComment Comment { get; set; }
}
