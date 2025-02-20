using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.Post.UpdatePostComment;

public class UpdatePostCommentCommand : IRequest
{
    public UpdatePostCommentDTO Comment { get; set; }
}
