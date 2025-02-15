using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.UserName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.ProfilePhotoPath)
            .HasMaxLength(50)
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.Property(u => u.CoverPhotoPath)
            .HasMaxLength(50)
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.Property(u => u.Gender)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.Biography)
            .HasMaxLength(500)
            .IsRequired(false)
            .HasDefaultValue("");

        builder.Property(u => u.SelfDescription)
            .HasMaxLength(1000)
            .IsRequired();
    }
}
