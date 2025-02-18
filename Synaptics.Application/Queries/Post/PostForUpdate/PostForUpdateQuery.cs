using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostForUpdate;

public class PostForUpdateQuery : IRequest<UpdatePostDTO>
{
    public long Id { get; set; }
}
