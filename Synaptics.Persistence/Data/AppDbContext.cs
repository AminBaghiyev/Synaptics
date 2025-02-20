using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Synaptics.Domain.Entities;
using System.Reflection;

namespace Synaptics.Persistence.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<UserRelation> UserRelations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
