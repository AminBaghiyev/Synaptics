﻿using FluentValidation;
using Synaptics.Application.Commands.PostCommentReply.UpdatePostCommentReply;

namespace Synaptics.Application.Validators;

public class UpdatePostCommentReplyValidator : AbstractValidator<UpdatePostCommentReplyCommand>
{
    public UpdatePostCommentReplyValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1).WithMessage("Id must be a natural number");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Post content cannot be empty")
            .MaximumLength(512).WithMessage("Post content can be up to 512 characters");
    }
}
