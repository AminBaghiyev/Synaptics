using Microsoft.EntityFrameworkCore;
using Synaptics.Domain.Entities;
using System.Linq.Expressions;

namespace Synaptics.Application.Interfaces.Repositories;

public interface IUserRelationRepository
{
    DbSet<UserRelation> Table { get; }
    Task<ICollection<UserRelation>> GetAllAsync(Expression<Func<UserRelation, bool>>? predicate = null, int page = 0, int count = 20, bool orderAsc = false, params string[] includes);
    Task<UserRelation?> GetOneAsync(Expression<Func<UserRelation, bool>> predicate, params string[] includes);
    Task CreateAsync(UserRelation entity);
    void Delete(UserRelation entity);
    Task<int> SaveChangesAsync();
}
