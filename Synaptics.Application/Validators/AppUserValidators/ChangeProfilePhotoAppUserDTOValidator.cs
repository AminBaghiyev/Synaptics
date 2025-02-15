using FluentValidation;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Validators;

public class ChangeProfilePhotoAppUserDTOValidator : AbstractValidator<ChangeProfilePhotoAppUserDTO>
{
    public ChangeProfilePhotoAppUserDTOValidator()
    {
        RuleFor(x => x.ProfilePhoto)
            .NotEmpty().WithMessage("Profile photo is required");
    }
}
