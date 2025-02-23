using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.Message.SendMessage;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, MessageResponse>
{
    readonly ICassandraService _cassandraService;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IHttpContextAccessor _contextAccessor;
    readonly IMapper _mapper;

    public SendMessageHandler(ICassandraService cassandraService, UserManager<Entities.AppUser> userManager, IHttpContextAccessor contextAccessor, IMapper mapper)
    {
        _cassandraService = cassandraService;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _mapper = mapper;
    }

    public async Task<MessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new MessageResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        Entities.AppUser? sender = await _userManager.FindByNameAsync(username);
        if (sender is null)
            return new MessageResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        Entities.AppUser? receiver = await _userManager.FindByNameAsync(request.Receiver);
        if (receiver is null)
            return new MessageResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        Entities.Message message = _mapper.Map<Entities.Message>(request);
        message.MessageId = Guid.NewGuid();
        message.SenderId = sender.Id;
        message.ReceiverId = receiver.Id;
        message.SentAt = DateTime.UtcNow;
        await _cassandraService.SaveMessageAsync(message);

        return new MessageResponse
        {
            StatusCode = HttpStatusCode.Created,
            MessageId = message.MessageId,
            Sender = sender.UserName,
            ReceiverId = receiver.Id,
            Content = message.Content,
            MessageType = message.MessageType,
            SentAt = message.SentAt
        };
    }
}
