using Synaptics.Application.Interfaces.Repositories;

namespace Synaptics.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPostRepository PostRepository { get; }
    IPostLikeRepository PostLikeRepository { get; }
    IPostCommentRepository PostCommentRepository { get; }
    ICommentLikeRepository CommentLikeRepository { get; }
    IUserRelationRepository UserRelationRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
