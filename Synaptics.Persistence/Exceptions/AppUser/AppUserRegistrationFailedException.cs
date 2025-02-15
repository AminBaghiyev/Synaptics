using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Persistence.Exceptions;

public class AppUserRegistrationFailedException : ExternalException
{
    public AppUserRegistrationFailedException(string message) : base(message) { }

    public AppUserRegistrationFailedException() : base("User registration failed") { }
}
