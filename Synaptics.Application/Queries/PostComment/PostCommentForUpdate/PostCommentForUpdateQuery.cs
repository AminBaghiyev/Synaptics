using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.PostComment.PostCommentForUpdate;

public record PostCommentForUpdateQuery : IRequest<Response>
{
    public long Id { get; set; }
}
