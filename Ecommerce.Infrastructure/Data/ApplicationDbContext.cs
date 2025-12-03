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
    public DbSet<WishList> WishLists { get; set; }
    public DbSet<UserAddress> UserAddress { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderPayment> OrderPayments { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<OrderAddress> OrderAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.IsActive).HasDefaultValue(true);
            entity.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(u => u.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

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
            entity.Property(c => c.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

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
            entity.Property(i => i.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

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
            entity.Property(p => p.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

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

        // WishList configuration
        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).ValueGeneratedOnAdd();
            entity.Property(w => w.IsActive).HasDefaultValue(true);
            entity.Property(w => w.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(w => w.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            // Relación N a 1 con Product y User
            entity.HasOne(w => w.Product)
                  .WithMany(p => p.WishLists)
                  .HasForeignKey(w => w.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(w => w.User)
              .WithMany(u => u.WishLists)
              .HasForeignKey(w => w.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(w => w.ProductId);
            entity.HasIndex(w => w.UserId);
            entity.HasIndex(w => new { w.ProductId, w.UserId })
                  .IsUnique();
            entity.HasIndex(w => w.IsActive);
        });

        // UserAddress configuration
        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(ua => ua.Id);
            entity.Property(ua => ua.Id).ValueGeneratedOnAdd();
            entity.Property(ua => ua.UseSameAddressForBilling).HasDefaultValue(false);
            entity.Property(ua => ua.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(ua => ua.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            // Relación 1 a 1 con  User
            entity.HasOne(ua => ua.User)
                  .WithOne(u => u.UserAddress)
                  .HasForeignKey<UserAddress>(ua => ua.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(ua => ua.UserId).IsUnique();
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).ValueGeneratedOnAdd();
            entity.Property(o => o.OrderNumber)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(o => o.Subtotal)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(o => o.Total)
                 .IsRequired()
                 .HasColumnType("decimal(18,2)");
            entity.Property(o => o.Tax)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(o => o.ShippingCost)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(o => o.Discount)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0);
            entity.Property(o => o.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(o => o.UpdatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relaciones
            entity.HasOne(o => o.User)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.OrderPayment)
                  .WithOne(op => op.Order)
                  .HasForeignKey<OrderPayment>(op => op.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.OrderStatus)
                  .WithOne(os => os.Order)
                  .HasForeignKey<OrderStatus>(os => os.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.OrderAddress)
                  .WithOne(oa => oa.Order)
                  .HasForeignKey<OrderAddress>(oa => oa.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(o => o.OrderNumber).IsUnique();
            entity.HasIndex(o => o.UserId);
            entity.HasIndex(o => o.CreatedAt);
            entity.HasIndex(o => o.Total);
            entity.HasIndex(o => new { o.UserId, o.CreatedAt });
        });

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.Id).ValueGeneratedOnAdd();
            entity.Property(oi => oi.ProductName)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(oi => oi.Subtotal)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(oi => oi.Price)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(oi => oi.Quantity)
                  .IsRequired()
                  .HasDefaultValue(1);
            entity.Property(oi => oi.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(oi => oi.UpdatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relaciones
            entity.HasOne(oi => oi.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict); // No eliminar producto si se elimina el item

            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(oi => oi.OrderId);
            entity.HasIndex(oi => oi.ProductId);
            entity.HasIndex(oi => new { oi.OrderId, oi.ProductId });
            entity.HasIndex(oi => oi.Price);
        });

        // OrderPayment configuration
        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasKey(op => op.Id);
            entity.Property(op => op.Id).ValueGeneratedOnAdd();
            entity.Property(op => op.PaymentGatewayId)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(op => op.CardHolderName)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(op => op.CardLastFour)
                  .IsRequired()
                  .HasMaxLength(4);
            entity.Property(op => op.Method)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(op => op.Amount)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(op => op.Status)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(20);
            entity.Property(op => op.PaidAt)
                  .IsRequired(false);
            entity.Property(op => op.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(op => op.UpdatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relación con Order
            entity.HasOne(op => op.Order)
                  .WithOne(o => o.OrderPayment)
                  .HasForeignKey<OrderPayment>(op => op.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(op => op.OrderId).IsUnique();
            entity.HasIndex(op => op.PaymentGatewayId).IsUnique();
            entity.HasIndex(op => op.Method);
            entity.HasIndex(op => op.Status);
            entity.HasIndex(op => op.CreatedAt);
        });

        // OrderStatus configuration
        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(os => os.Id);
            entity.Property(os => os.Id).ValueGeneratedOnAdd();
            entity.Property(os => os.Status)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(20);
            entity.Property(os => os.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(os => os.UpdatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relación con Order
            entity.HasOne(os => os.Order)
                  .WithOne(o => o.OrderStatus)
                  .HasForeignKey<OrderStatus>(os => os.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(os => os.OrderId).IsUnique();
            entity.HasIndex(os => os.Status);
            entity.HasIndex(os => new { os.OrderId, os.Status });
            entity.HasIndex(os => os.CreatedAt);
        });

        // OrderAddress configuration
        modelBuilder.Entity<OrderAddress>(entity =>
        {
            entity.HasKey(oa => oa.Id);
            entity.Property(oa => oa.Id).ValueGeneratedOnAdd();

            // Shipping address properties
            entity.Property(oa => oa.ShippingStreet)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(oa => oa.ShippingCity)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(oa => oa.ShippingState)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(oa => oa.ShippingZipCode)
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(oa => oa.ShippingCountry)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(oa => oa.ShippingPhone)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(oa => oa.ShippingEmail)
                  .IsRequired()
                  .HasMaxLength(255);

            // Billing address properties
            entity.Property(oa => oa.UseSameAddressForBilling)
                  .IsRequired()
                  .HasDefaultValue(true);
            entity.Property(oa => oa.BillingStreet)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(oa => oa.BillingCity)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(oa => oa.BillingState)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(oa => oa.BillingZipCode)
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(oa => oa.BillingCountry)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(oa => oa.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(oa => oa.UpdatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relación con Order
            entity.HasOne(oa => oa.Order)
                  .WithOne(o => o.OrderAddress)
                  .HasForeignKey<OrderAddress>(oa => oa.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(oa => oa.OrderId).IsUnique();
            entity.HasIndex(oa => oa.ShippingCountry);
            entity.HasIndex(oa => oa.ShippingState);
            entity.HasIndex(oa => oa.ShippingCity);
        });
    }
}