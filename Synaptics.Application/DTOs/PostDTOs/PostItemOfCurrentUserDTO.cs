using Synaptics.Domain.Enums;

namespace Synaptics.Application.DTOs;

public record PostItemOfCurrentUserDTO
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
