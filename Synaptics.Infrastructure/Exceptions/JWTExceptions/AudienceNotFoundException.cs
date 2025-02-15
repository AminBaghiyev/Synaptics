using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Infrastructure.Exceptions;

public class AudienceNotFoundException : InternalException
{
    public AudienceNotFoundException(string message) : base(message) { }

    public AudienceNotFoundException() : base("Audience not found!") { }
}
