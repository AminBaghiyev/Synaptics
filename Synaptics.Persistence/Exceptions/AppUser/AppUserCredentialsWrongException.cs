using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Persistence.Exceptions;

public class AppUserCredentialsWrongException : ExternalException
{
    public AppUserCredentialsWrongException(string message) : base(message) { }

    public AppUserCredentialsWrongException() : base("Credentials are wrong!") { }
}
