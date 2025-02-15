using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Infrastructure.Exceptions;

public class IssuerNotFoundException : InternalException
{
    public IssuerNotFoundException(string message) : base(message) { }

    public IssuerNotFoundException() : base("Issuer not found!") { }
}
