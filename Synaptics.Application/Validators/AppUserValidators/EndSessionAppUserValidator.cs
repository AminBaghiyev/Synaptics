using FluentValidation;
using Synaptics.Application.Commands.AppUser.EndSessionAppUser;

namespace Synaptics.Application.Validators.AppUserValidators;

public class EndSessionAppUserValidator : AbstractValidator<EndSessionAppUserCommand>
{
    public EndSessionAppUserValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}
