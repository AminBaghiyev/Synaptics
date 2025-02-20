using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostCommentForUpdate;

public class PostCommentForUpdateQuery : IRequest<UpdatePostCommentDTO>
{
    public long Id { get; set; }
}
