using Synaptics.Domain.Enums;

namespace Synaptics.Domain.Entities;

public class Message
{
    public Guid MessageId { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; }
    public MessageTypes MessageType { get; set; }
    public DateTime SentAt { get; set; }
}
