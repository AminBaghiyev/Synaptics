using Synaptics.Application.Exceptions.Base;

namespace Synaptics.Application.Exceptions;

public class FileSizeException : ExternalException
{
    public FileSizeException(string message) : base(message) { }
}
