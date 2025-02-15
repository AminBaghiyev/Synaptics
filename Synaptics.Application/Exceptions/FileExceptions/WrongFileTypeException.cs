using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Application.Exceptions;

public class WrongFileTypeException : ExternalException
{
    public WrongFileTypeException(string message) : base(message) { }
}
