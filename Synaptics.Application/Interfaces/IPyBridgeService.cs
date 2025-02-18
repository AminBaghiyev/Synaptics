using Synaptics.Domain.Results;

namespace Synaptics.Application.Interfaces;

public interface IPyBridgeService
{
    Task<(PyBridgeResult, float[])> EmbeddingAsync(string sentence);
}
