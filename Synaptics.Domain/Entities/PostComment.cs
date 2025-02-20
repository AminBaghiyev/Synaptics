using Synaptics.Domain.Entities.Base;

namespace Synaptics.Domain.Entities;

public class PostComment : BaseEntity
{
    public string Content { get; set; }
    public long LikeCount { get; set; }
    public long ReplyCount { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public long PostId { get; set; }
    public Post Post { get; set; }
    public long? ParentId { get; set; }
    public PostComment Parent { get; set; }
    public ICollection<CommentLike> Likes { get; set; }
    public ICollection<PostComment> Replies { get; set; }
}
