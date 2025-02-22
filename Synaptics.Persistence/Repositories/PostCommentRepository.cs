using Microsoft.EntityFrameworkCore;
using Npgsql;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Persistence.Data;

namespace Synaptics.Persistence.Repositories;

public class PostCommentRepository : Repository<PostComment>, IPostCommentRepository
{
    public PostCommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<int> SoftDeleteRepliesAsync(long id)
    {
        string query = @"
            WITH RECURSIVE children AS (
                SELECT ""Id"", ""ParentId"" FROM ""PostComments"" WHERE ""Id"" = @id
                UNION ALL
                SELECT c.""Id"", c.""ParentId""
                FROM ""PostComments"" c
                INNER JOIN children ch ON c.""ParentId"" = ch.""Id""
            )
            UPDATE ""PostComments""
            SET ""IsDeleted"" = TRUE
            WHERE ""Id"" IN (SELECT ""Id"" FROM children)
            RETURNING 1;
        ";

        int affectedRows = await _context.Database.ExecuteSqlRawAsync(query, new NpgsqlParameter("@id", id));
        return affectedRows;
    }

    public async Task<bool> HasDeletedParentAsync(long id)
    {
        var query = @"
            WITH RECURSIVE parent_check AS ( 
                SELECT ""Id"", ""ParentId"", ""IsDeleted""
                FROM ""PostComments""
                WHERE ""Id"" = @id

                UNION ALL 
 
                SELECT t.""Id"", t.""ParentId"", t.""IsDeleted""
                FROM ""PostComments"" t
                JOIN parent_check p ON t.""Id"" = p.""ParentId""
            )
            SELECT EXISTS (
                SELECT 1
                FROM parent_check
                WHERE ""IsDeleted"" = 't'
            ) AS ""IsAnyParentDeleted""
        ";

        bool[] result = await _context.Database.SqlQueryRaw<bool>(query, new NpgsqlParameter("@id", id)).ToArrayAsync();

        return result[0];
    }
}