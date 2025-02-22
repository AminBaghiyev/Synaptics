using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Common;
using Synaptics.Application.Exceptions.Base;
using System.Net;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public class SearchAppUserHandler : IRequestHandler<SearchAppUserQuery, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public SearchAppUserHandler(UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(SearchAppUserQuery request, CancellationToken cancellationToken)
    {
        string normalizedQuery = request.SearchQuery.ToUpper().Trim();

        if (normalizedQuery.Length == 0) throw new ExternalException("Invalid query");

        ICollection<Entities.AppUser> users = await _userManager.Users
            .Where(u =>
                u.FirstName.ToUpper().Contains(normalizedQuery) ||
                u.LastName.ToUpper().Contains(normalizedQuery) ||
                u.NormalizedUserName.Contains(normalizedQuery))
            .Skip(request.Offset)
            .Take(20)
            .ToListAsync(cancellationToken: cancellationToken);

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<ICollection<SearchAppUserQueryResponse>>(users)
        };
    }
}
