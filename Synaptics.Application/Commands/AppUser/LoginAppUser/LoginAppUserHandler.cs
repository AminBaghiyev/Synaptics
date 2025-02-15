using MediatR;
using Synaptics.Application.Interfaces;

namespace Synaptics.Application.Commands.AppUser.LoginAppUser;

public class LoginAppUserHandler : IRequestHandler<LoginAppUserCommand, string>
{
    readonly IAppUserService _appUserService;

    public LoginAppUserHandler(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    public async Task<string> Handle(LoginAppUserCommand request, CancellationToken cancellationToken)
    {
        return await _appUserService.LoginAsync(request.AppUser);
    }
}
