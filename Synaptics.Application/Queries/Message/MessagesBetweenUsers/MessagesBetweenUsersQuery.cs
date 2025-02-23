using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.Message.MessagesBetweenUsers;

public record MessagesBetweenUsersQuery : IRequest<Response>
{
    public string Receiver { get; set; }
    public byte[]? PagingState { get; set; }
}
