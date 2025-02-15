using FluentValidation;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Validators;

public class ChangeCoverPhotoAppUserDTOValidator : AbstractValidator<ChangeCoverPhotoAppUserDTO>
{
    public ChangeCoverPhotoAppUserDTOValidator()
    {
        RuleFor(x => x.CoverPhoto)
            .NotEmpty().WithMessage("Cover photo is required");
    }
}
