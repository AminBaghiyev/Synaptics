using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;

public record RepliesOfPostCommentQuery : IRequest<Response>
{
    public long ParentId { get; set; }
    public int Page { get; set; }
}
