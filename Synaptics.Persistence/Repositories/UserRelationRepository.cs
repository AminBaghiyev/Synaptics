using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Persistence.Data;
using System.Linq.Expressions;

namespace Synaptics.Persistence.Repositories;

public class UserRelationRepository : IUserRelationRepository
{
    protected readonly AppDbContext _context;

    public UserRelationRepository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<UserRelation> Table => _context.UserRelations;

    public async Task<ICollection<UserRelation>> GetAllAsync(Expression<Func<UserRelation, bool>>? predicate = null, int page = 0, int count = 20, bool orderAsc = false, params string[] includes)
    {
        IQueryable<UserRelation> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate is not null) query = query.Where(predicate);

        query = orderAsc ? query.OrderBy(e => e.FollowedAt) : query.OrderByDescending(e => e.FollowedAt);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return await query.ToListAsync();
    }

    public Task<UserRelation?> GetOneAsync(Expression<Func<UserRelation, bool>> predicate, params string[] includes)
    {
        IQueryable<UserRelation> query = Table.AsNoTracking();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.SingleOrDefaultAsync(predicate);
    }

    public async Task<bool> IsFriendAsync(string firstUserId, string secondUserId)
    {
        int friendCount = await Table.CountAsync(e =>
            (e.FollowerId == firstUserId && e.FollowingId == secondUserId) ||
            (e.FollowerId == secondUserId && e.FollowingId == firstUserId));

        return friendCount == 2;
    }

    public async Task CreateAsync(UserRelation entity)
    {
        entity.FollowedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await Table.AddAsync(entity);
    }

    public void Delete(UserRelation entity)
    {
        Table.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
