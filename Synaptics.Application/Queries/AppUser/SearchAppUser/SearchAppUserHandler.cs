using MediatR;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public class SearchAppUserHandler : IRequestHandler<SearchAppUserQuery, ICollection<SearchAppUserDTO>>
{
    readonly IAppUserService _appUserService;

    public SearchAppUserHandler(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    public async Task<ICollection<SearchAppUserDTO>> Handle(SearchAppUserQuery request, CancellationToken cancellationToken)
    {
        if (request.Query.Trim().Length == 0) throw new ExternalException("Invalid query");

        return await _appUserService.SearchUserAsync(request.Query, offset: request.Offset);
    }
}
