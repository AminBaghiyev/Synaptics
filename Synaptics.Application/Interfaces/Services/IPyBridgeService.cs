using Synaptics.Application.Common.Results;

namespace Synaptics.Application.Interfaces.Services;

public interface IPyBridgeService
{
    Task<(PyBridgeResult, float[])> EmbeddingAsync(string sentence);
}
