using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;

public record PostCommentReplyForUpdateQuery : IRequest<Response>
{
    public long Id { get; set; }
}
