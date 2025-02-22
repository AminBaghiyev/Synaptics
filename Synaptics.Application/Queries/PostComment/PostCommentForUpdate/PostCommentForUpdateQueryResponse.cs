namespace Synaptics.Application.Queries.PostComment.PostCommentForUpdate;

public record PostCommentForUpdateQueryResponse
{
    public long Id { get; set; }
    public string Content { get; set; }
}
