using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.Post.CreatePost;

public class CreatePostCommand : IRequest
{
    public CreatePostDTO Post { get; set; }
}
