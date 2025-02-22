using MediatR;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.LoginAppUser;

public class LoginAppUserHandler : IRequestHandler<LoginAppUserCommand, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IJWTTokenService _jWTTokenService;

    public LoginAppUserHandler(UserManager<Entities.AppUser> userManager, IJWTTokenService jWTTokenService)
    {
        _userManager = userManager;
        _jWTTokenService = jWTTokenService;
    }

    public async Task<Response> Handle(LoginAppUserCommand request, CancellationToken cancellationToken)
    {
        Entities.AppUser? user = await _userManager.FindByNameAsync(request.UserName.ToUpper());
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.CredentialsWrong
            };

        bool res = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!res)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.CredentialsWrong
            };

        IEnumerable<Claim> claims =
        [
            new("sub", user.Id),
            new("firstname", user.FirstName),
            new("lastname", user.LastName),
            new("username", user.UserName),
            new("email", user.Email),
            new("gender", user.Gender.ToString())
        ];

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _jWTTokenService.GenerateToken(claims)
        };
    }
}
