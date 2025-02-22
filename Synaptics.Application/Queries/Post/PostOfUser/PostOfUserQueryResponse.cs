namespace Synaptics.Application.Queries.Post.PostOfUser;

public record PostOfUserQueryResponse
{
    public long Id { get; set; }
    public DateTime LastTime { get; set; }
    public string Thought { get; set; }
    public long LikeCount { get; set; }
    public long CommentCount { get; set; }
    public long ShareCount { get; set; }
    public bool IsLiked { get; set; }
}
