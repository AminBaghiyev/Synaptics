using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostsOfCurrentUser;

public class PostOfCurrentUserCommand : IRequest<PostItemOfCurrentUserDTO>
{
    public long Id { get; set; }
}
