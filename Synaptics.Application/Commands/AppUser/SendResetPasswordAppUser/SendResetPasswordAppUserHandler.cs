using MediatR;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.SendResetPasswordAppUser;

public class SendResetPasswordAppUserHandler : IRequestHandler<SendResetPasswordAppUserCommand, Response>
{
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IEmailService _emailService;

    public SendResetPasswordAppUserHandler(UserManager<Entities.AppUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<Response> Handle(SendResetPasswordAppUserCommand request, CancellationToken cancellationToken)
    {
        Entities.AppUser? user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _emailService.SendChangePasswordAsync(user.Email, token, user.UserName, $"{user.FirstName} {user.LastName}");

        return new Response
        {
            StatusCode = HttpStatusCode.OK
        };
    }
}
