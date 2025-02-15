using FluentValidation;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Validators;

public class ChangePasswordAppUserDTOValidator : AbstractValidator<ChangePasswordAppUserDTO>
{
    public ChangePasswordAppUserDTOValidator()
    {
        RuleFor(x => x.OriginalPassword)
            .NotEmpty().WithMessage("Original password is required")
            .MinimumLength(6).WithMessage("Original password must be at least 6 characters long");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters long");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("New password must be confirmed")
            .Equal(x => x.NewPassword).WithMessage("Passwords don't match!");
    }
}
