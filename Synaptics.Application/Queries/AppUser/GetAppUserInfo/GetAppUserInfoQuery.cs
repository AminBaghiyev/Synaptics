using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.AppUser.GetAppUserInfo;

public record GetAppUserInfoQuery : IRequest<Response>
{
}
