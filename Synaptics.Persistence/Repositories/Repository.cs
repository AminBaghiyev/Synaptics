using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities.Base;
using Synaptics.Persistence.Data;
using System.Linq.Expressions;

namespace Synaptics.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int page = 0, int count = 5, bool orderAsc = true, string orderByProperty = "Id", params string[] includes)
    {
        IQueryable<T> query = Table.AsNoTrackingWithIdentityResolution();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate is not null) query = query.Where(predicate);

        ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
        MemberExpression property = Expression.Property(parameter, orderByProperty);
        Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

        query = orderAsc ? query.OrderBy(lambda) : query.OrderByDescending(lambda);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return await query.ToListAsync();
    }

    public Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate, bool isTracking = false, params string[] includes)
    {
        IQueryable<T> query = Table;

        if (!isTracking) query = query.AsNoTrackingWithIdentityResolution();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.SingleOrDefaultAsync(predicate);
    }

    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        entity.UpdatedAt = entity.CreatedAt;
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        entity.IsUpdated = true;
        Table.Update(entity);
    }

    public void SoftDelete(T entity)
    {
        entity.DeletedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        entity.IsDeleted = true;
        Table.Update(entity);
    }

    public void HardDelete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
