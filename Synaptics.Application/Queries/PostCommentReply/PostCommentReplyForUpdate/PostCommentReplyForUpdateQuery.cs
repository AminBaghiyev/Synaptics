using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;

public class PostCommentReplyForUpdateQuery : IRequest<UpdatePostCommentReplyDTO>
{
    public long Id { get; set; }
}
