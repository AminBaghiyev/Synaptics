using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;

public class ChangeAppUserInfoCommand : IRequest
{
    public ChangeAppUserInfoDTO Info { get; set; }
}
