using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostsOfUser;

public class PostsOfUserCommand : IRequest<ICollection<PostItemDTO>>
{
    public string UserName { get; set; }
    public int Page { get; set; }
}
