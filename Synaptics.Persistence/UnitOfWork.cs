using Microsoft.EntityFrameworkCore.Storage;
using Synaptics.Application.Interfaces;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Persistence.Data;

namespace Synaptics.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public IPostRepository PostRepository { get; }
    public IPostLikeRepository PostLikeRepository { get; }
    public IPostCommentRepository PostCommentRepository { get; }
    public ICommentLikeRepository CommentLikeRepository { get; }
    public IUserRelationRepository UserRelationRepository { get; }

    readonly AppDbContext _context;
    IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context, IPostRepository postRepository, IPostLikeRepository postLikeRepository, IPostCommentRepository postCommentRepository, ICommentLikeRepository commentLikeRepository, IUserRelationRepository userRelationRepository)
    {
        _context = context;
        PostRepository = postRepository;
        PostLikeRepository = postLikeRepository;
        PostCommentRepository = postCommentRepository;
        CommentLikeRepository = commentLikeRepository;
        UserRelationRepository = userRelationRepository;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction is not null) throw new InvalidOperationException("There is already an active transaction.");

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is null) throw new InvalidOperationException("No active transaction to commit.");

        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is null) throw new InvalidOperationException("No active transaction to rollback.");

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _context.Dispose();
        _transaction?.Dispose();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
}
