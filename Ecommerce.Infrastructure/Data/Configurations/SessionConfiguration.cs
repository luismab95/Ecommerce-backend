using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;


public class SessionConfiguration : IEntityTypeConfiguration<Session>
{

    public void Configure(EntityTypeBuilder<Session> entity)
    {

        entity.HasKey(s => s.Id);
        entity.Property(s => s.Id).ValueGeneratedOnAdd();
        entity.Property(s => s.DeviceInfo).HasMaxLength(500);
        entity.Property(s => s.IpAddress).HasMaxLength(45);
        entity.Property(s => s.RefreshToken).IsRequired();
        entity.Property(s => s.IsActive).HasDefaultValue(true);
        entity.Property(s => s.LoginAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
        entity.Property(s => s.LogoutAt).HasDefaultValue(null);

        // Indexes
        entity.HasIndex(s => s.UserId);
        entity.HasIndex(s => new { s.UserId, s.IsActive });

        // Relationships
        entity.HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}