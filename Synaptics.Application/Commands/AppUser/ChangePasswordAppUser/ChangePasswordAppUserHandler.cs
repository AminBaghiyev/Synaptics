using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Exceptions.Base;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;

public class ChangePasswordAppUserHandler : IRequestHandler<ChangePasswordAppUserCommand>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IHttpContextAccessor _contextAccessor;

    public ChangePasswordAppUserHandler(UserManager<Entities.AppUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(ChangePasswordAppUserCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        Entities.AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        IdentityResult res = await _userManager.ChangePasswordAsync(user, request.Passwords.OriginalPassword, request.Passwords.NewPassword);

        if (!res.Succeeded) throw new ExternalException("Password are wrong!");
    }
}
