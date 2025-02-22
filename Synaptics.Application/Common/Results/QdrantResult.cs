namespace Synaptics.Application.Common.Results;

public class QdrantResult
{
    public bool Succeeded { get; }
    public string? ErrorMessage { get; }

    private QdrantResult(bool succeeded, string? errorMessage = null)
    {
        Succeeded = succeeded;
        ErrorMessage = errorMessage;
    }

    public static QdrantResult Success() => new(true);
    public static QdrantResult Failure(string errorMessage) => new(false, errorMessage);
}
