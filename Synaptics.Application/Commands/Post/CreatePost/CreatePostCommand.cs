using MediatR;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.Commands.Post.CreatePost;

public record CreatePostCommand : IRequest<Response>
{
    public string Thought { get; set; }
    public PostVisibility Visibility { get; set; }
}
