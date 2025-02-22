using FluentValidation;
using Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;

namespace Synaptics.Application.Validators;

public class ChangeCoverPhotoAppUserValidator : AbstractValidator<ChangeCoverPhotoAppUserCommand>
{
    public ChangeCoverPhotoAppUserValidator()
    {
        RuleFor(x => x.CoverPhoto)
            .NotEmpty().WithMessage("Cover photo is required");
    }
}
