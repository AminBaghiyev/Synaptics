namespace Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;

public record RepliesOfPostCommentQueryResponse
{
    public long Id { get; set; }
    public string Content { get; set; }
    public long LikeCount { get; set; }
    public long ReplyCount { get; set; }
    public string UserName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public bool IsLiked { get; set; }
    public bool IsUpdated { get; set; }
    public DateTime LastTime { get; set; }
}
