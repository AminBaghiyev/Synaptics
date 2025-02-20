using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;

public class RepliesOfPostCommentQuery : IRequest<ICollection<PostCommentItemDTO>>
{
    public long ParentId { get; set; }
    public int Page { get; set; }
}
