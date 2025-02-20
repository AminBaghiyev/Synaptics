using FluentValidation;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Validators;

public class CreatePostCommentDTOValidator : AbstractValidator<CreatePostCommentDTO>
{
    public CreatePostCommentDTOValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment cannot be empty")
            .MaximumLength(512).WithMessage("Comment can be up to 512 characters");

        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(1).WithMessage("Id must be a natural number");
    }
}
