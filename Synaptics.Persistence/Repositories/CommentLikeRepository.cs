using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Persistence.Data;
using System.Linq.Expressions;

namespace Synaptics.Persistence.Repositories;

public class CommentLikeRepository : ICommentLikeRepository
{
    protected readonly AppDbContext _context;

    public CommentLikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<CommentLike> Table => _context.CommentLikes;

    public async Task<ICollection<CommentLike>> GetAllAsync(Expression<Func<CommentLike, bool>>? predicate = null, int page = 0, int count = 20, params string[] includes)
    {
        IQueryable<CommentLike> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate is not null) query = query.Where(predicate);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return await query.ToListAsync();
    }

    public IQueryable<CommentLike> GetAllAsQueryable(Expression<Func<CommentLike, bool>>? predicate = null, int page = 0, int count = 20, params string[] includes)
    {
        IQueryable<CommentLike> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate is not null) query = query.Where(predicate);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return query;
    }

    public async Task<CommentLike?> GetOneAsync(Expression<Func<CommentLike, bool>> predicate, params string[] includes)
    {
        IQueryable<CommentLike> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task CreateAsync(CommentLike entity)
    {
        await Table.AddAsync(entity);
    }

    public void Delete(CommentLike entity)
    {
        Table.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
