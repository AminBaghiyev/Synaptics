using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(p => p.Id)
            .HasColumnType("BIGINT");
        builder.Property(p => p.LikeCount)
            .HasColumnType("BIGINT");
        builder.Property(p => p.CommentCount)
            .HasColumnType("BIGINT");
        builder.Property(p => p.ShareCount)
            .HasColumnType("BIGINT");

        builder.Property(p => p.Thought)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(p => p.LikeCount).HasDefaultValue(0);
        builder.Property(p => p.CommentCount).HasDefaultValue(0);
        builder.Property(p => p.ShareCount).HasDefaultValue(0);

        builder.Property(p => p.Visibility)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
