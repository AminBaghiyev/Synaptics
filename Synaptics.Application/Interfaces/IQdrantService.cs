using Synaptics.Domain.Results;

namespace Synaptics.Application.Interfaces;

public interface IQdrantService
{
    Task<QdrantResult> AddDataAsync(string collectionName, Guid id, float[] vector);
    Task<QdrantResult> UpdateDataAsync(string collectionName, Guid id, float[] vector);
    Task<QdrantResult> DeleteDataAsync(string collectionName, Guid id);
    Task<(QdrantResult, ICollection<(string Id, float Score)>)> SearchCosineSimilarityAsync(string collectionName, float[] queryVector, ulong limit = 10, ulong offset = 0);
}
