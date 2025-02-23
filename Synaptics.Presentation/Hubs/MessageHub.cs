using MediatR;
using Microsoft.AspNetCore.SignalR;
using Synaptics.Application.Commands.Message.SendMessage;
using Synaptics.Application.Common;
using System.Net;

namespace Synaptics.Presentation.Hubs;

public class MessageHub : Hub
{
    readonly IMediator _mediator;

    public MessageHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(SendMessageCommand command)
    {
        try
        {            
            MessageResponse response = await _mediator.Send(command);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                await Clients.User(response.ReceiverId).SendAsync("ReceiveMessage", new
                {
                    messageId = response.MessageId,
                    senderId = response.Sender,
                    content = response.Content,
                    messageType = response.MessageType,
                    sentAt = response.SentAt
                });
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveError", response.MessageCode);
            }
        }
        catch (Exception)
        {
            await Clients.Caller.SendAsync("ReceiveError", "Internal server error");
        }
    }
}
