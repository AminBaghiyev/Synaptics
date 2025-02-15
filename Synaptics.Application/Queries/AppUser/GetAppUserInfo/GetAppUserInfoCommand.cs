using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.AppUser.GetAppUserInfo;

public class GetAppUserInfoCommand : IRequest<ChangeAppUserInfoDTO>
{
}
