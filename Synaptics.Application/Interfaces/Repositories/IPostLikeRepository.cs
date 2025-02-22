using Microsoft.EntityFrameworkCore;
using Synaptics.Domain.Entities;
using System.Linq.Expressions;

namespace Synaptics.Application.Interfaces.Repositories;

public interface IPostLikeRepository
{
    DbSet<PostLike> Table { get; }
    Task<ICollection<PostLike>> GetAllAsync(Expression<Func<PostLike, bool>>? predicate = null, int page = 0, int count = 20, bool orderAsc = false, params string[] includes);
    IQueryable<PostLike> GetAllAsQueryable(Expression<Func<PostLike, bool>>? predicate = null, int page = 0, int count = 20, bool orderAsc = false, params string[] includes);
    Task<PostLike?> GetOneAsync(Expression<Func<PostLike, bool>> predicate, params string[] includes);
    Task CreateAsync(PostLike entity);
    void Delete(PostLike entity);
}
