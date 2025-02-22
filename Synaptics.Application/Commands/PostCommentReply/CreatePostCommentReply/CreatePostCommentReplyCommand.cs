using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Commands.PostCommentReply.CreatePostCommentReply;

public record CreatePostCommentReplyCommand : IRequest<Response>
{
    public string Content { get; set; }
    public long ParentId { get; set; }
}
