using FluentValidation;
using Synaptics.Application.Commands.AppUser.LoginAppUser;

namespace Synaptics.Application.Validators;

public class LoginAppUserValidator : AbstractValidator<LoginAppUserCommand>
{
    public LoginAppUserValidator()
    {
        RuleFor(e => e.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 256).WithMessage("Username must be between 3 and 256 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
}
