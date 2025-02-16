using MediatR;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Queries.Post.PostOfUser;

public class PostOfUserHandler : IRequestHandler<PostOfUserCommand, PostItemDTO>
{
    readonly IPostService _service;

    public PostOfUserHandler(IPostService service)
    {
        _service = service;
    }

    public async Task<PostItemDTO> Handle(PostOfUserCommand request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id, request.UserName);
    }
}
