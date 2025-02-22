using Synaptics.Domain.Enums;

namespace Synaptics.Application.Queries.Post.PostForUpdate;

public record PostForUpdateQueryResponse
{
    public long Id { get; set; }
    public string Thought { get; set; }
    public PostVisibility Visibility { get; set; }
}
