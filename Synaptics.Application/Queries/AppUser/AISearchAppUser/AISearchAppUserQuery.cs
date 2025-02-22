using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.AppUser.AISearchAppUser;

public record AISearchAppUserQuery : IRequest<Response>
{
    public string SearchQuery { get; set; }
    public ulong Offset { get; set; }
}
