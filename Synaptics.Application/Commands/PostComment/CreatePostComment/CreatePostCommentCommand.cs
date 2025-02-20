using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.Post.CreatePostComment;

public class CreatePostCommentCommand : IRequest
{
    public CreatePostCommentDTO Comment { get; set; }
}
