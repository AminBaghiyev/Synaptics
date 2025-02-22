using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.PostComment.CommentsOfPost;

public record CommentsOfPostQuery : IRequest<Response>
{
    public long PostId { get; set; }
    public int Page { get; set; }
}
