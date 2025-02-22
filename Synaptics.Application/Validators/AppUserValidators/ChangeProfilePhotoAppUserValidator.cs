using FluentValidation;
using Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;

namespace Synaptics.Application.Validators;

public class ChangeProfilePhotoAppUserValidator : AbstractValidator<ChangeProfilePhotoAppUserCommand>
{
    public ChangeProfilePhotoAppUserValidator()
    {
        RuleFor(x => x.ProfilePhoto)
            .NotEmpty().WithMessage("Profile photo is required");
    }
}
