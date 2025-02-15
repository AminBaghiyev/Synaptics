using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Persistence.Exceptions;

public class AppUserExistsException : ExternalException
{
    public AppUserExistsException(string message) : base(message) { }
}
