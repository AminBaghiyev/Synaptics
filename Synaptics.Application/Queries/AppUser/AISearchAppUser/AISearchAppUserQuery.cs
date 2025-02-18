using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.AppUser.AISearchAppUser;

public class AISearchAppUserQuery : IRequest<ICollection<AISearchAppUserDTO>>
{
    public string Query { get; set; }
    public ulong Offset { get; set; }
}
