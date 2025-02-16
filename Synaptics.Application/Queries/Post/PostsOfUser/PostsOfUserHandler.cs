using MediatR;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Queries.Post.PostsOfUser;

public class PostsOfUserHandler : IRequestHandler<PostsOfUserCommand, ICollection<PostItemDTO>>
{
    readonly IPostService _service;

    public PostsOfUserHandler(IPostService service)
    {
        _service = service;
    }

    public async Task<ICollection<PostItemDTO>> Handle(PostsOfUserCommand request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(request.UserName, page: request.Page);
    }
}
