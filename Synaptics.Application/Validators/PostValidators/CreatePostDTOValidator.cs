using FluentValidation;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Validators;

public class CreatePostDTOValidator : AbstractValidator<CreatePostDTO>
{
    public CreatePostDTOValidator()
    {
        RuleFor(x => x.Thought)
            .NotEmpty().WithMessage("Post content cannot be empty")
            .MaximumLength(512).WithMessage("Post content can be up to 512 characters");

        RuleFor(x => x.Visibility)
            .IsInEnum().WithMessage("You must select a valid post visibility");
    }
}
