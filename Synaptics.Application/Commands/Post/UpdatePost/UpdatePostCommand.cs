using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.Post.UpdatePost;

public class UpdatePostCommand : IRequest
{
    public UpdatePostDTO Post { get; set; }
}
