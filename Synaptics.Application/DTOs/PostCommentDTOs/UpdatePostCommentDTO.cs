namespace Synaptics.Application.DTOs;

public record UpdatePostCommentDTO
{
    public long Id { get; set; }
    public string Content { get; set; }
}
