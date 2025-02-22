using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.Post.LikesOfPost;

public record LikesOfPostQuery : IRequest<Response>
{
    public long PostId { get; set; }
    public int Page { get; set; }
}
