using Synaptics.Domain.Enums;
using System.Net;

namespace Synaptics.Application.Common;

public class MessageResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public MessageCode? MessageCode { get; set; }
    public Guid MessageId { get; set; }
    public string Sender { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; }
    public MessageTypes MessageType { get; set; }
    public DateTime SentAt { get; set; }
}
