using Microsoft.EntityFrameworkCore;
using Synaptics.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Synaptics.Application.Interfaces.Repositories;

public interface IRepository<T> where T : BaseEntity, new()
{
    DbSet<T> Table { get; }
    Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int page = 0, int count = 5, bool orderAsc = true, string orderByProperty = "Id", params string[] includes);
    Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate, bool isTracking = false, params string[] includes);
    Task CreateAsync(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    void HardDelete(T entity);
    Task<int> SaveChangesAsync();
}
