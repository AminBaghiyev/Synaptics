using Synaptics.Application.Common.Results;

namespace Synaptics.Application.Interfaces.Services;

public interface IQdrantService
{
    Task<QdrantResult> AddDataAsync(string collectionName, Guid id, float[] vector);
    Task<QdrantResult> UpdateDataAsync(string collectionName, Guid id, float[] vector);
    Task<QdrantResult> DeleteDataAsync(string collectionName, Guid id);
    Task<(QdrantResult, ICollection<(string Id, float Score)>)> SearchCosineSimilarityAsync(string collectionName, float[] queryVector, ulong limit = 10, ulong offset = 0);
}
