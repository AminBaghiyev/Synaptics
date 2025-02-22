using Synaptics.Domain.Enums;

namespace Synaptics.Application.Queries.Post.PostOfUser;

public record PostOfCurrentUserQueryResponse
{
    public long Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime LastTime { get; set; }
    public string Thought { get; set; }
    public long LikeCount { get; set; }
    public long CommentCount { get; set; }
    public PostVisibility Visibility { get; set; }
    public long ShareCount { get; set; }
    public bool IsLiked { get; set; }
}
