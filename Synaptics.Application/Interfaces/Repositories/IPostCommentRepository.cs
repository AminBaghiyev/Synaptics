using Synaptics.Domain.Entities;

namespace Synaptics.Application.Interfaces.Repositories;

public interface IPostCommentRepository : IRepository<PostComment>
{
    Task<int> SoftDeleteRepliesAsync(long id);
    Task<bool> HasDeletedParentAsync(long id);
}
