using MediatR;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.Commands.Post.UpdatePost;

public record UpdatePostCommand : IRequest<Response>
{
    public long Id { get; set; }
    public string Thought { get; set; }
    public PostVisibility Visibility { get; set; }
}
