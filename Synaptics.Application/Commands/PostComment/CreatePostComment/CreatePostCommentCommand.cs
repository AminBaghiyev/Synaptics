using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostComment.CreatePostComment;

public record CreatePostCommentCommand : IRequest<Response>
{
    public string Content { get; set; }
    public long PostId { get; set; }
}
