using FluentValidation;
using Synaptics.Application.Commands.AppUser.SendResetPasswordAppUser;

namespace Synaptics.Application.Validators.AppUserValidators;

public class SendResetPasswordAppUserValidator : AbstractValidator<SendResetPasswordAppUserCommand>
{
    public SendResetPasswordAppUserValidator()
    {
        RuleFor(e => e.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 256).WithMessage("Username must be between 3 and 256 characters");
    }
}
