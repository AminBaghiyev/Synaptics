using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.Post.CreatePostCommentReply;

public class CreatePostCommentReplyCommand : IRequest
{
    public CreatePostCommentReplyDTO Reply { get; set; }
}
