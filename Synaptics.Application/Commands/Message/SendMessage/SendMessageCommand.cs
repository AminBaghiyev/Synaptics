using MediatR;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.Commands.Message.SendMessage;

public record SendMessageCommand : IRequest<MessageResponse>
{
    public string Receiver { get; set; }
    public string Content { get; set; }
    public MessageTypes MessageType { get; set; }
}
