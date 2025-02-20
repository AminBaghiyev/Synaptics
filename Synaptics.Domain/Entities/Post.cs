using Synaptics.Domain.Entities.Base;
using Synaptics.Domain.Enums;

namespace Synaptics.Domain.Entities;

public class Post : BaseEntity
{
    public string Thought { get; set; }
    public long LikeCount { get; set; }
    public long CommentCount { get; set; }
    public PostVisibility Visibility { get; set; }
    public long ShareCount { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public ICollection<PostLike> Likes { get; set; }
    public ICollection<PostComment> Comments { get; set; }
}
