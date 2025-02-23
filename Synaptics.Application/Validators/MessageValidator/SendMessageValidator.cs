using FluentValidation;
using Synaptics.Application.Commands.Message.SendMessage;

namespace Synaptics.Application.Validators;

public class SendMessageValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.Receiver)
            .NotEmpty().WithMessage("Receiver cannot be empty.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Message content cannot be empty.")
            .MaximumLength(1000).WithMessage("Message content cannot exceed 1000 characters.");

        RuleFor(x => x.MessageType)
            .IsInEnum().WithMessage("Invalid message type.");
    }
}
