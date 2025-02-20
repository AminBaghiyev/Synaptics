using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Persistence.Data;
using System.Linq.Expressions;

namespace Synaptics.Persistence.Repositories;

public class PostLikeRepository : IPostLikeRepository
{
    protected readonly AppDbContext _context;

    public PostLikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<PostLike> Table => _context.PostLikes;

    public async Task<ICollection<PostLike>> GetAllAsync(Expression<Func<PostLike, bool>>? predicate = null, int page = 0, int count = 20, bool orderAsc = false, params string[] includes)
    {
        IQueryable<PostLike> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate is not null) query = query.Where(predicate);

        query = orderAsc ? query.OrderBy(e => e.LikedAt) : query.OrderByDescending(e => e.LikedAt);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return await query.ToListAsync();
    }

    public IQueryable<PostLike> GetAllAsQueryable(Expression<Func<PostLike, bool>>? predicate = null, int page = 0, int count = 20, bool orderAsc = false, params string[] includes)
    {
        IQueryable<PostLike> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate is not null) query = query.Where(predicate);

        query = orderAsc ? query.OrderBy(e => e.LikedAt) : query.OrderByDescending(e => e.LikedAt);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return query;
    }

    public async Task<PostLike?> GetOneAsync(Expression<Func<PostLike, bool>> predicate, params string[] includes)
    {
        IQueryable<PostLike> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task CreateAsync(PostLike entity)
    {
        entity.LikedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await Table.AddAsync(entity);
    }

    public void Delete(PostLike entity)
    {
        Table.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
