using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<Session> Sessions => Set<Session>();
    // public DbSet<Category> Categories { get; set; }
    // public DbSet<Image> Images { get; set; }
    // public DbSet<Product> Products { get; set; }
    // public DbSet<WishList> WishLists { get; set; }
    // public DbSet<UserAddress> UserAddress { get; set; }
    // public DbSet<Order> Orders { get; set; }
    // public DbSet<OrderItem> OrderItems { get; set; }
    // public DbSet<OrderPayment> OrderPayments { get; set; }
    // public DbSet<OrderStatus> OrderStatuses { get; set; }
    // public DbSet<OrderAddress> OrderAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SessionConfiguration());
    }
}