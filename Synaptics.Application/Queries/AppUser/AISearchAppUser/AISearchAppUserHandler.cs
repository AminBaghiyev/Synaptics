using MediatR;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Queries.AppUser.AISearchAppUser;

public class AISearchAppUserHandler : IRequestHandler<AISearchAppUserQuery, ICollection<AISearchAppUserDTO>>
{
    readonly IAppUserService _service;

    public AISearchAppUserHandler(IAppUserService service)
    {
        _service = service;
    }

    public async Task<ICollection<AISearchAppUserDTO>> Handle(AISearchAppUserQuery request, CancellationToken cancellationToken)
    {
        if (request.Query.Trim().Length == 0) throw new ExternalException("Invalid query");

        return await _service.SearchUserWithAIAsync(request.Query, offset: request.Offset);
    }
}
