using Synaptics.Domain.Enums;

namespace Synaptics.Application.DTOs;

public record CreatePostDTO
{
    public string Thought { get; set; }
    public PostVisibility Visibility { get; set; }
}
