using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostsOfCurrentUser;

public class PostOfCurrentUserQuery : IRequest<PostItemOfCurrentUserDTO>
{
    public long Id { get; set; }
}
