using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Configurations;

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.HasKey(cl => new { cl.UserId, cl.CommentId });

        builder.HasOne(cl => cl.User)
            .WithMany(u => u.CommentsLike)
            .HasForeignKey(cl => cl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cl => cl.Comment)
            .WithMany(p => p.Likes)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
