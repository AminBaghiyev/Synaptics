using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public class SearchAppUserCommand : IRequest<ICollection<SearchAppUserDTO>>
{
    public string Query { get; set; }
}
