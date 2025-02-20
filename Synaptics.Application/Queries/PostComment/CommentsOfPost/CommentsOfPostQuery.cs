using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.CommentsOfPost;

public class CommentsOfPostQuery : IRequest<ICollection<PostCommentItemDTO>>
{
    public long PostId { get; set; }
    public int Page { get; set; }
}
