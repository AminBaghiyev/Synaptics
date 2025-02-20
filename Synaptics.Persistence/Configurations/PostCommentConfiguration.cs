using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Configurations;

public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder.Property(p => p.Id)
            .HasColumnType("BIGINT");
        builder.Property(p => p.LikeCount)
            .HasColumnType("BIGINT");
        builder.Property(p => p.ReplyCount)
            .HasColumnType("BIGINT");

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(p => p.LikeCount).HasDefaultValue(0);
        builder.Property(p => p.ReplyCount).HasDefaultValue(0);

        builder.HasOne(p => p.User)
            .WithMany(u => u.PostComments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pc => pc.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(pc => pc.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
