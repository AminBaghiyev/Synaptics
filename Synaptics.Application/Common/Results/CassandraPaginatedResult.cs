using Synaptics.Application.Queries.Message.MessagesBetweenUsers;

namespace Synaptics.Application.Common.Results;

public class CassandraPaginatedResult
{
    public IEnumerable<MessagesBetweenUsersQueryResponse> Messages { get; set; }
    public byte[] NextPagingState { get; set; }
}
