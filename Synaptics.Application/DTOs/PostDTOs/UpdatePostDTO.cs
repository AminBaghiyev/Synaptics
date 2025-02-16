using Synaptics.Domain.Enums;

namespace Synaptics.Application.DTOs;

public record UpdatePostDTO
{
    public long Id { get; set; }
    public string Thought { get; set; }
    public PostVisibility Visibility { get; set; }
}
