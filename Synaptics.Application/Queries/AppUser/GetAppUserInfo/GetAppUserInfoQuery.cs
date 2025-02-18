using MediatR;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Queries.AppUser.GetAppUserInfo;

public class GetAppUserInfoQuery : IRequest<ChangeAppUserInfoDTO>
{
}
