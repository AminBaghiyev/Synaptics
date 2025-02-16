using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostsOfCurrentUser;

public class PostsOfCurrentUserCommand : IRequest<ICollection<PostItemOfCurrentUserDTO>>
{
    public int Page { get; set; }
}
