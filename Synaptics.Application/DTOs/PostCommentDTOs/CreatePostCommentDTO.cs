namespace Synaptics.Application.DTOs;

public record CreatePostCommentDTO
{
    public string Content { get; set; }
    public long PostId { get; set; }
}
