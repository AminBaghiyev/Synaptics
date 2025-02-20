using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Persistence.Data;

namespace Synaptics.Persistence.Repositories;

public class PostCommentRepository : Repository<PostComment>, IPostCommentRepository
{
    public PostCommentRepository(AppDbContext context) : base(context)
    {
    }
}
