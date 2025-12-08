using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);
        entity.Property(u => u.Id).ValueGeneratedOnAdd();
        entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
        entity.Property(u => u.PasswordHash).IsRequired();
        entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        entity.Property(u => u.Phone).HasMaxLength(20);
        entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
        entity.Property(u => u.IsActive).HasDefaultValue(true);
        entity.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
        entity.Property(u => u.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

        //Indexes
        entity.HasIndex(u => u.Email).IsUnique();
        entity.HasIndex(u => u.Phone).IsUnique();
        entity.HasIndex(u => u.Role);
        entity.HasIndex(u => u.IsActive);
        entity.HasIndex(u => new { u.FirstName, u.LastName });
    }
}
