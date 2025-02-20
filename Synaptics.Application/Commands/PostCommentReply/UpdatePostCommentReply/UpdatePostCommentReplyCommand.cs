using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.Post.UpdatePostCommentReply;

public class UpdatePostCommentReplyCommand : IRequest
{
    public UpdatePostCommentReplyDTO Reply { get; set; }
}
