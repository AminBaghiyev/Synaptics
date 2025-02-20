using Microsoft.EntityFrameworkCore;
using Synaptics.Domain.Entities;
using System.Linq.Expressions;

namespace Synaptics.Application.Interfaces.Repositories;

public interface ICommentLikeRepository
{
    DbSet<CommentLike> Table { get; }
    Task<ICollection<CommentLike>> GetAllAsync(Expression<Func<CommentLike, bool>>? predicate = null, int page = 0, int count = 50, params string[] includes);
    IQueryable<CommentLike> GetAllAsQueryable(Expression<Func<CommentLike, bool>>? predicate = null, int page = 0, int count = 50, params string[] includes);
    Task<CommentLike?> GetOneAsync(Expression<Func<CommentLike, bool>> predicate, params string[] includes);
    Task CreateAsync(CommentLike entity);
    void Delete(CommentLike entity);
    Task<int> SaveChangesAsync();
}
