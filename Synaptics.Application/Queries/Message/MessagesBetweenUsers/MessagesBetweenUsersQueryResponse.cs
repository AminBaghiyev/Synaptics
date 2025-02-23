using Synaptics.Domain.Enums;

namespace Synaptics.Application.Queries.Message.MessagesBetweenUsers;

public class MessagesBetweenUsersQueryResponse
{
    public Guid MessageId { get; set; }
    public string Content { get; set; }
    public MessageTypes MessageType { get; set; }
    public DateTime SentAt { get; set; }
}
