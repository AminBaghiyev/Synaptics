using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Configurations;

public class UserRelationConfiguration : IEntityTypeConfiguration<UserRelation>
{
    public void Configure(EntityTypeBuilder<UserRelation> builder)
    {
        builder.HasKey(ur => new { ur.FollowerId, ur.FollowingId });

        builder
            .HasOne(ur => ur.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(ur => ur.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ur => ur.Following)
            .WithMany(u => u.Followers)
            .HasForeignKey(ur => ur.FollowingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
