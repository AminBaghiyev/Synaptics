using FluentValidation;
using Synaptics.Application.Commands.PostCommentReply.CreatePostCommentReply;

namespace Synaptics.Application.Validators;

public class CreatePostCommentReplyValidator : AbstractValidator<CreatePostCommentReplyCommand>
{
    public CreatePostCommentReplyValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment cannot be empty")
            .MaximumLength(512).WithMessage("Comment can be up to 512 characters");

        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(1).WithMessage("Id must be a natural number");
    }
}
