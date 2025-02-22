using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.AppUser.SearchAppUser;

public record SearchAppUserQuery : IRequest<Response>
{
    public string SearchQuery { get; set; }
    public int Offset { get; set; }
}
