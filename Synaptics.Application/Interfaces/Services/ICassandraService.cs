using Synaptics.Application.Common.Results;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Interfaces.Services;

public interface ICassandraService
{
    Task SaveMessageAsync(Message message);
    Task<CassandraPaginatedResult> GetMessagesBetweenUsersAsync(string senderId, string receiverId, byte[]? pagingState = null, int limit = 30);
}
