using FluentValidation;
using Synaptics.Application.Commands.AppUser.ResetPasswordAppUser;

namespace Synaptics.Application.Validators.AppUserValidators;

public class ResetPasswordAppUserValidator : AbstractValidator<ResetPasswordAppUserCommand>
{
    public ResetPasswordAppUserValidator()
    {
        RuleFor(e => e.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 256).WithMessage("Username must be between 3 and 256 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
}
