namespace Synaptics.Application.DTOs;

public record UpdatePostCommentReplyDTO
{
    public long Id { get; set; }
    public string Content { get; set; }
}
