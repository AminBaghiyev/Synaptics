using FluentValidation;
using Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;

namespace Synaptics.Application.Validators;

public class ChangePasswordAppUserValidator : AbstractValidator<ChangePasswordAppUserCommand>
{
    public ChangePasswordAppUserValidator()
    {
        RuleFor(x => x.OriginalPassword)
            .NotEmpty().WithMessage("Original password is required")
            .MinimumLength(6).WithMessage("Original password must be at least 6 characters long");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters long")
            .NotEqual(x => x.OriginalPassword).WithMessage("Old password and new password cannot be the same");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("New password must be confirmed")
            .Equal(x => x.NewPassword).WithMessage("Passwords don't match!");
    }
}
