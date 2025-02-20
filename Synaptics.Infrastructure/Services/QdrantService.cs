using Qdrant.Client;
using Qdrant.Client.Grpc;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Results;

namespace Synaptics.Infrastructure.Services;

public class QdrantService : IQdrantService
{
    readonly QdrantClient _qdrantClient;

    public QdrantService(QdrantClient qdrantClient)
    {
        _qdrantClient = qdrantClient;
    }

    public async Task<QdrantResult> AddDataAsync(string collectionName, Guid id, float[] vector)
    {
        try
        {
            UpdateResult res = await _qdrantClient.UpsertAsync(collectionName, points:
            [
                new()
                {
                    Id = id,
                    Vectors = vector
                }
            ]);

            return res.Status == UpdateStatus.Completed ? QdrantResult.Success() : QdrantResult.Failure("Error occurred while adding data");
        }
        catch (Exception ex)
        {
            return QdrantResult.Failure(ex.Message);
        }
    }

    public async Task<QdrantResult> UpdateDataAsync(string collectionName, Guid id, float[] vector)
    {
        try
        {
            UpdateResult res = await _qdrantClient.UpdateVectorsAsync(collectionName, points:
            [
                new()
                {
                    Id = id,
                    Vectors = vector
                }
            ]);

            return res.Status == UpdateStatus.Completed ? QdrantResult.Success() : QdrantResult.Failure("Error occurred while updating data");
        }
        catch (Exception ex)
        {
            return QdrantResult.Failure(ex.Message);
        }
    }

    public async Task<QdrantResult> DeleteDataAsync(string collectionName, Guid id)
    {
        try
        {
            UpdateResult res = await _qdrantClient.DeleteAsync(collectionName, id);

            return res.Status == UpdateStatus.Completed ? QdrantResult.Success() : QdrantResult.Failure("Error occurred while deleting data");
        }
        catch (Exception ex)
        {
            return QdrantResult.Failure(ex.Message);
        }
    }

    public async Task<(QdrantResult, ICollection<(string Id, float Score)>)> SearchCosineSimilarityAsync(string collectionName, float[] queryVector, ulong limit = 10, ulong offset = 0)
    {
        try
        {
            IReadOnlyCollection<ScoredPoint> res = await _qdrantClient.SearchAsync(collectionName, queryVector, limit: limit, offset: offset, scoreThreshold: 0.4f);

            return (QdrantResult.Success(), res.Select(p => (Id: p.Id.Uuid, p.Score)).ToList());
        }
        catch (Exception ex)
        {
            return (QdrantResult.Failure(ex.Message), []);
        }   
    }
}
