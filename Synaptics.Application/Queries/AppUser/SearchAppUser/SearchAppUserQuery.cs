using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public class SearchAppUserQuery : IRequest<ICollection<SearchAppUserDTO>>
{
    public string Query { get; set; }
    public int Offset { get; set; }
}
