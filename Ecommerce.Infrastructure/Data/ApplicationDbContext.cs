namespace Ecommerce.Infrastructure.Data;

using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.IsActive).HasDefaultValue(true);
            entity.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");

            // Índices
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.IsActive);
            entity.HasIndex(u => new { u.FirstName, u.LastName });
        });

        // Session configuration
        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();
            entity.Property(s => s.DeviceInfo).HasMaxLength(500);
            entity.Property(s => s.IpAddress).HasMaxLength(45);
            entity.Property(s => s.RefreshToken).IsRequired();
            entity.Property(s => s.LoginAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(s => s.LogoutAt).HasDefaultValue(null);
            entity.Property(s => s.IsActive).HasDefaultValue(true);


            // Índices
            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => new { s.UserId, s.IsActive });

            // Relationships
            entity.HasOne(s => s.User)
                  .WithMany(u => u.Sessions)
                  .HasForeignKey(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).IsRequired().HasMaxLength(255);
            entity.Property(c => c.IsActive).HasDefaultValue(true);
            entity.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(c => c.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(c => c.Name).IsUnique();
            entity.HasIndex(c => c.IsActive);
        });

        // Image configuration
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.Path).IsRequired().HasMaxLength(500);
            entity.Property(i => i.IsActive).HasDefaultValue(true);
            entity.Property(i => i.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(i => i.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");

            // Relación N a 1 con Product (varias imágenes pertenecen a un producto)
            entity.HasOne(i => i.Product)
                  .WithMany(p => p.Images)  // Un producto tiene muchas imágenes
                  .HasForeignKey(i => i.ProductId)
                  .OnDelete(DeleteBehavior.Cascade); // Si se elimina el producto, se eliminan sus imágenes

            // Índices
            entity.HasIndex(i => i.ProductId);
            entity.HasIndex(i => new { i.ProductId, i.IsActive });
            entity.HasIndex(i => i.IsActive);
        });


        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(1000);
            entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
            entity.Property(p => p.Featured).HasDefaultValue(false);
            entity.Property(p => p.IsActive).HasDefaultValue(true);
            entity.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(p => p.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");

            // Relación 1 a varios con Category
            entity.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.CategoryId);
            entity.HasIndex(p => p.Featured);
            entity.HasIndex(p => p.IsActive);
            entity.HasIndex(p => new { p.CategoryId, p.IsActive });
            entity.HasIndex(p => new { p.Featured, p.IsActive });
            entity.HasIndex(p => p.Price);
        });

    }
}