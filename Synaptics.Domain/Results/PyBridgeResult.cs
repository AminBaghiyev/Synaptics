namespace Synaptics.Domain.Results;

public class PyBridgeResult
{
    public bool Succeeded { get; }
    public string? ErrorMessage { get; }

    private PyBridgeResult(bool succeeded, string? errorMessage = null)
    {
        Succeeded = succeeded;
        ErrorMessage = errorMessage;
    }

    public static PyBridgeResult Success() => new(true);
    public static PyBridgeResult Failure(string errorMessage) => new(false, errorMessage);
}
