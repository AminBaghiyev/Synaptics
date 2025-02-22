using FluentValidation;
using Synaptics.Application.Commands.PostComment.CreatePostComment;

namespace Synaptics.Application.Validators;

public class CreatePostCommentValidator : AbstractValidator<CreatePostCommentCommand>
{
    public CreatePostCommentValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment cannot be empty")
            .MaximumLength(512).WithMessage("Comment can be up to 512 characters");

        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(1).WithMessage("Id must be a natural number");
    }
}
