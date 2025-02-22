using MediatR;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;

public record ChangeAppUserInfoCommand : IRequest<Response>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string Biography { get; set; }
    public string SelfDescription { get; set; }
}
