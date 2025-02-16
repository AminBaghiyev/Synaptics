using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Configurations;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.HasKey(pl => new { pl.UserId, pl.PostId });

        builder.HasOne(pl => pl.User)
            .WithMany(u => u.PostsLike)
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pl => pl.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(pl => pl.LikedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
