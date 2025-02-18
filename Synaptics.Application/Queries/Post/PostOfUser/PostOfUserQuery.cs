using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostOfUser;

public class PostOfUserQuery : IRequest<PostItemDTO>
{
    public long Id { get; set; }
    public string UserName { get; set; }
}
