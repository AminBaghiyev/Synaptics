namespace Synaptics.Application.DTOs;

public record CreatePostCommentReplyDTO
{
    public string Content { get; set; }
    public long ParentId { get; set; }
}
