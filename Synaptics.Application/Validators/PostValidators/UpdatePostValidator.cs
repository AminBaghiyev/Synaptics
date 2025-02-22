using FluentValidation;
using Synaptics.Application.Commands.Post.UpdatePost;

namespace Synaptics.Application.Validators;

public class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1).WithMessage("Id must be a natural number");

        RuleFor(x => x.Thought)
            .NotEmpty().WithMessage("Post content cannot be empty")
            .MaximumLength(512).WithMessage("Post content can be up to 512 characters");

        RuleFor(x => x.Visibility)
            .IsInEnum().WithMessage("You must select a valid post visibility");
    }
}
