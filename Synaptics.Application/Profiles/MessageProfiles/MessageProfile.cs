using AutoMapper;
using Synaptics.Application.Commands.Message.SendMessage;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<SendMessageCommand, Message>();
    }
}
