using MediatR;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Domain.Enums;
using System.Net;
using System.Web;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.ResetPasswordAppUser;

public class ResetPasswordAppUserHandler : IRequestHandler<ResetPasswordAppUserCommand, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;

    public ResetPasswordAppUserHandler(UserManager<Entities.AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response> Handle(ResetPasswordAppUserCommand request, CancellationToken cancellationToken)
    {
        Entities.AppUser? user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };
        
        IdentityResult res = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(request.Token), request.Password);

        if (!res.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
