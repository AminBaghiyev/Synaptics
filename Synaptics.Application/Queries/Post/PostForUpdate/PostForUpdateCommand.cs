using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.Post.PostForUpdate;

public class PostForUpdateCommand : IRequest<UpdatePostDTO>
{
    public long Id { get; set; }
}
