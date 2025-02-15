using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Infrastructure.Exceptions;

public class SecretKeyNotFoundException : InternalException
{
    public SecretKeyNotFoundException(string message) : base(message) { }

    public SecretKeyNotFoundException() : base("Secret key not found!") { }
}
