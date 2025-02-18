using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.LikesOfPost;

public class LikesOfPostQuery : IRequest<ICollection<PostLikeUserDTO>>
{
    public string UserName { get; set; }
    public long PostId { get; set; }
    public int Page { get; set; }
}
