﻿using FluentValidation;
using Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;

namespace Synaptics.Application.Validators;

public class ChangeAppUserInfoValidator : AbstractValidator<ChangeAppUserInfoCommand>
{
    public ChangeAppUserInfoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender is required and must be a valid enum value");

        RuleFor(x => x.Biography)
            .MaximumLength(500).WithMessage("Biography can't be longer than 500 characters");

        RuleFor(x => x.SelfDescription)
            .NotEmpty().WithMessage("Self description is required")
            .MaximumLength(1000).WithMessage("Self description can't be longer than 1000 characters");
    }
}
