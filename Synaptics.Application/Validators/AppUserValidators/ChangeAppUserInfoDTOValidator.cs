using FluentValidation;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Validators;

public class ChangeAppUserInfoDTOValidator : AbstractValidator<ChangeAppUserInfoDTO>
{
    public ChangeAppUserInfoDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender is required and must be a valid enum value");

        RuleFor(x => x.Biography)
            .MaximumLength(500).WithMessage("Biography can't be longer than 500 characters");

        RuleFor(x => x.SelfDescription)
            .NotEmpty().WithMessage("Self description is required")
            .MaximumLength(1000).WithMessage("Self description can't be longer than 1000 characters");
    }
}
