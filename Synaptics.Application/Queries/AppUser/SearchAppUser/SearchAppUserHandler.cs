using MediatR;
using Synaptics.Application.DTOs;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public class SearchAppUserHandler : IRequestHandler<SearchAppUserCommand, ICollection<SearchAppUserDTO>>
{
    readonly IAppUserService _appUserService;

    public SearchAppUserHandler(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    public async Task<ICollection<SearchAppUserDTO>> Handle(SearchAppUserCommand request, CancellationToken cancellationToken)
    {
        return await _appUserService.SearchUserAsync(request.Query);
    }
}
